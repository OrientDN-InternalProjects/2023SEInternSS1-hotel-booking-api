using HotelBooking.Common.Base;
using HotelBooking.Common.Constants;
using HotelBooking.Data.DTOs.Account;
using HotelBooking.Data.Extensions;
using HotelBooking.Data.Interfaces;
using HotelBooking.Data.ViewModel;
using HotelBooking.Model.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
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
        public AccountRepository(UserManager<User> userManager, 
            SignInManager<User> signInManager, 
            IConfiguration configuration, BookingDbContext context, 
            IMailSender mailSender, ITokenManager tokenManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
            this.context = context;
            this.mailSender = mailSender;
            this.tokenManager = tokenManager;
        }

        public async Task<ResponseModel> ChangePassword(string email, ChangePasswordRequest model)
        {
            var exist_user = await userManager.FindByEmailAsync(email);
            if (exist_user == null)
            {
                return new ResponseModel
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = $"Can't find any user with {email}",
                    IsSuccess = false
                };
            }
            var res = await userManager.ChangePasswordAsync(exist_user, model.OldPassword, model.NewPassword);
            if (res.Succeeded)
            {
                await tokenManager.DeactivateCurrentAsync();
                JwtSecurityToken jwtSecurityToken = await CreateJwtToken(exist_user);
                result.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
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
            var exist_user = await userManager.FindByEmailAsync(model.Email);
            if (exist_user == null)
            {
                return new ResponseModel
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = $"Can't find any user with {model.Email}",
                    IsSuccess = false
                };
            }
            var token = await userManager.GeneratePasswordResetTokenAsync(exist_user);
            if (await mailSender.SendMailToResetPassword(exist_user.Email, token))
            {
                return new ResponseModel
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = $"Please check mail at {exist_user.Email}",
                    IsSuccess = true
                };
            }
            return new ResponseModel
            {
                StatusCode = HttpStatusCode.BadRequest,
                Message = $"Incorrect email for user {exist_user.Email}.",
                IsSuccess = false
            };
        }

        public async Task<ResponseModel> LoginAsync(LoginRequest model)
        {
            AuthenicationModel result = new AuthenicationModel();
            // Check user exists with email or not
            var exist_user = await userManager.FindByEmailAsync(model.Email);
            if (exist_user == null)
            {
                return new ResponseModel
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = $"Can't find any user with {model.Email}",
                    IsSuccess = false
                };
            }
            var check = await userManager.CheckPasswordAsync(exist_user, model.Password);
            if (check)
            {
                JwtSecurityToken jwtSecurityToken = await CreateJwtToken(exist_user);
                return new ResponseModel
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = "Login successfully",
                    Data = new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken)
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
            var exist_user = await userManager.FindByEmailAsync(user.Email);
            if (exist_user != null)
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
            var exist_user = await userManager.FindByEmailAsync(model.Email);
            AuthenicationModel result = new AuthenicationModel();
            if (exist_user == null)
            {
                return new ResponseModel
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = $"Can't find any user with {model.Email}",
                    IsSuccess = false
                };
            }
            var resetResult = await userManager.ResetPasswordAsync(exist_user, model.Token, model.Password);
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
