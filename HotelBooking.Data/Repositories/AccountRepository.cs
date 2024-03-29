﻿using HotelBooking.Common.Base;
using HotelBooking.Common.Constants;
using HotelBooking.Data.DTOs.Account;
using HotelBooking.Data.Extensions;
using HotelBooking.Data.Infrastructure;
using HotelBooking.Data.Interfaces;
using HotelBooking.Model.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace HotelBooking.Data.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IConfiguration configuration;
        private readonly BookingDbContext context;
        private readonly IMailSender mailSender;
        private readonly ITokenManager tokenManager;
        private readonly IUnitOfWork unitOfWork;
        public AccountRepository(UserManager<User> userManager,
            SignInManager<User> signInManager,
            IConfiguration configuration, BookingDbContext context,
            IMailSender mailSender, ITokenManager tokenManager,
            IUnitOfWork unitOfWork
            )
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
            this.context = context;
            this.mailSender = mailSender;
            this.tokenManager = tokenManager;
            this.unitOfWork = unitOfWork;
        }

        public async Task<ResponseModel> ChangePassword(string email, ChangePasswordRequest model)
        {
            var existUser = await userManager.FindByEmailAsync(email);
            if (existUser == null)
            {
                return new ResponseModel
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = $"Can't find any user with {email}",
                    IsSuccess = false
                };
            }
            var res = await userManager.ChangePasswordAsync(existUser, model.OldPassword, model.NewPassword);
            if (res.Succeeded)
            {
                await tokenManager.DeactivateCurrentAsync();
                JwtSecurityToken jwtSecurityToken = await CreateJwtToken(existUser);
                return new ResponseModel
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = "Congrats! Change password successfully!",
                    Data = new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken)
                    },
                    IsSuccess = true,
                };
            }
            return new ResponseModel
            {
                StatusCode = HttpStatusCode.BadRequest,
                Message = "Change-password failed",
                Errors = string.Join(", ", res.Errors.ToList().Select(e => e.Description)),
                IsSuccess = false
            };
        }

        public async Task<ResponseModel> ForgetPassword(ForgetPasswordRequest model)
        {
            var existUser = await userManager.FindByEmailAsync(model.Email);
            if (existUser == null)
            {
                return new ResponseModel
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = $"Can't find any user with {model.Email}",
                    IsSuccess = false
                };
            }
            var token = await userManager.GeneratePasswordResetTokenAsync(existUser);
            if (await mailSender.SendMailToResetPassword(existUser.Email, token))
            {
                return new ResponseModel
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = $"Please check mail at {existUser.Email}",
                    IsSuccess = true
                };
            }
            return new ResponseModel
            {
                StatusCode = HttpStatusCode.BadRequest,
                Message = $"Incorrect email for user {existUser.Email}.",
                IsSuccess = false
            };
        }

        public async Task<User> GetUserByEmail(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            return user;
        }

        public async Task<User> GetUserById(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            return user;
        }

        public async Task<ResponseModel> LoginAsync(LoginRequest model)
        {
            // Check user exists with email or not
            var existUser = await userManager.FindByEmailAsync(model.Email);
            if (existUser == null)
            {
                return new ResponseModel
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = $"Can't find any user with {model.Email}",
                    IsSuccess = false
                };
            }
            var check = await userManager.CheckPasswordAsync(existUser, model.Password);
            if (check)
            {
                JwtSecurityToken jwtSecurityToken = await CreateJwtToken(existUser);
                var refreshToken = GenerateRefreshToken();
                await SetRefreshToken(refreshToken, existUser);
                return new ResponseModel
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = "Login successfully",
                    Data = new
                    {
                        accessToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                        refreshToken = refreshToken.Token
                    },
                    IsSuccess = true
                };
            }
            return new ResponseModel
            {
                StatusCode = HttpStatusCode.BadRequest,
                Message = $"Login failed. Password wrong!",
                IsSuccess = false
            };
        }

        public async Task<ResponseModel> RegisterAsync(RegisterRequest model)
        {
            var user = new User
            {
                Email = model.Email,
                UserName = model.Username,
                FullName = model.Fullname,
                IsActive = true,
                CreatedOn = DateTime.Now,
                PhoneNumber = model.PhoneNumber
            };
            // Check if user with email or not
            var existUser = await userManager.FindByEmailAsync(user.Email);
            if (existUser != null)
            {
                return new ResponseModel
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = $"User existed with {model.Email}",
                    IsSuccess = false
                };
            }
            else
            {
                var return_result = await userManager.CreateAsync(user, model.Password);
                if (return_result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, RoleConstant.CustomerRole);
                    return new ResponseModel
                    {
                        StatusCode = HttpStatusCode.OK,
                        Message = "Register successfully",
                        IsSuccess = true
                    };
                }
                return new ResponseModel
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "Register failed",
                    Errors = string.Join(", ", return_result.Errors.ToList().Select(e => e.Description)),
                    IsSuccess = false
                };
            }
        }

        public async Task<ResponseModel> ResetPassword(ResetPasswordRequest model)
        {
            var existUser = await userManager.FindByEmailAsync(model.Email);
            if (existUser == null)
            {
                return new ResponseModel
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = $"Can't find any user with {model.Email}",
                    IsSuccess = false
                };
            }
            var resetResult = await userManager.ResetPasswordAsync(existUser, model.Token, model.Password);
            if (resetResult.Succeeded)
            {
                return new ResponseModel
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = "Congrats! Reset password successfully!",
                    IsSuccess = true
                };
            }
            return new ResponseModel
            {
                StatusCode = HttpStatusCode.BadRequest,
                Message = "reset-password failed",
                Errors = string.Join(", ", resetResult.Errors.ToList().Select(e => e.Description)),
                IsSuccess = false
            };
        }

        private RefreshToken GenerateRefreshToken()
        {
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32)),
                Created = DateTime.Now,
                Expires = DateTime.Now.AddDays(7)
            };
            return refreshToken;
        }

        private async Task SetRefreshToken(RefreshToken refreshToken, User existUser)
        {
            existUser.RefreshToken = refreshToken.Token;
            existUser.TokenCreated = refreshToken.Created;
            existUser.TokenExpires = refreshToken.Expires;
            await unitOfWork.SaveAsync();
        }

        private async Task<JwtSecurityToken> CreateJwtToken(User user)
        {
            var userClaims = await userManager.GetClaimsAsync(user);
            var roles = await userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();
            foreach (var i in roles)
            {
                roleClaims.Add(new Claim("role", i.ToString()));
            }
            var authClaims = new List<Claim>
            {
                new Claim("Email", user.Email),
                new Claim("Jti", Guid.NewGuid().ToString()),
                new Claim("Username", user.UserName),
                new Claim("Id", user.Id.ToString())
            }
            .Union(userClaims).Union(roleClaims);
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: configuration["JWT:Issuer"],
                audience: configuration["JWT:Audience"],
                claims: authClaims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: signingCredentials);
            return jwtSecurityToken;
        }

    }
}
