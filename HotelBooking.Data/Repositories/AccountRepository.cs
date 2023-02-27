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
        public AccountRepository(UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration configuration, BookingDbContext context, IMailSender mailSender)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
            this.context = context;
            this.mailSender = mailSender;
        }

        public async Task<AuthenicationModel> ChangePassword(string email, ChangePasswordDTO model)
        {
            var exist_user = await userManager.FindByEmailAsync(email);
            AuthenicationModel result = new AuthenicationModel();
            if (exist_user == null)
            {
                result.Message = $"Can't find any user with {email}";
                result.IsSuccess = false;
                return result;
            }
            var res = await userManager.ChangePasswordAsync(exist_user, model.OldPassword, model.NewPassword);
            if (res.Succeeded)
            {
                result.Message = "Congrats! Change password successfully!";
                result.IsSuccess = true;
                return result;
            }
            result.Message = "Change-password failed";
            List<IdentityError> errors = res.Errors.ToList();
            result.Error = string.Join(", ", errors.Select(e => e.Description));
            result.IsSuccess = false;
            return result;
        }

        public async Task<AuthenicationModel> ForgetPassword(ForgetPasswordDTO model)
        {
            var exist_user = await userManager.FindByEmailAsync(model.Email);
            AuthenicationModel result = new AuthenicationModel();
            if (exist_user == null)
            {
                result.Message = $"Can't find any user with {model.Email}";
                result.IsSuccess = false;
                return result;
            }
            var token = await userManager.GeneratePasswordResetTokenAsync(exist_user);
            if (await mailSender.SendMailToResetPassword(exist_user.Email, token))
            {
                result.Message = $"Please check mail at {exist_user.Email}";
                result.IsSuccess = true;
                return result;
            }
            result.Message = $"Incorrect email for user {exist_user.Email}.";
            result.IsSuccess = false;
            return result;
        }

        public async Task<AuthenicationModel> LoginAsync(LoginDTO model)
        {
            AuthenicationModel result = new AuthenicationModel();
            // Check user exists with email or not
            var exist_user = await userManager.FindByEmailAsync(model.Email);
            if (exist_user == null)
            {
                result.Message = $"Can't find any user with {model.Email}";
                result.IsSuccess = false;
                return result;
            }
            var check = await userManager.CheckPasswordAsync(exist_user, model.Password);
            if (check)
            {
                result.Message = "Login successfully";
                JwtSecurityToken jwtSecurityToken = await CreateJwtToken(exist_user);
                result.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                result.IsSuccess = true;
                return result;
            }
            result.Message = $"Login failed. Password wrong!";
            result.IsSuccess = false;
            return result;
        }

        public async Task<AuthenicationModel> RegisterAsync(RegisterDTO model)
        {
            AuthenicationModel result = new AuthenicationModel();
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
                result.Message = $"User with {model.Email} exists. Please use another email!";
                result.IsSuccess = false;
                return result;
            }
            else
            {
                var return_result = await userManager.CreateAsync(user, model.Password);
                if (return_result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, RoleConstant.CustomerRole);
                    result.Message = "Register successfully";
                    result.IsSuccess = true;
                    return result;
                }
                result.Message = "Register failed";
                List<IdentityError> errors = return_result.Errors.ToList();
                result.Error = string.Join(", ", errors.Select(e => e.Description));
                result.IsSuccess = false;
                return result;
            }
        }

        public async Task<AuthenicationModel> ResetPassword(ResetPasswordDTO model)
        {
            var exist_user = await userManager.FindByEmailAsync(model.Email);
            AuthenicationModel result = new AuthenicationModel();
            if (exist_user == null)
            {
                result.Message = $"Can't find any user with {model.Email}";
                result.IsSuccess = false;
                return result;
            }
            var resetResult = await userManager.ResetPasswordAsync(exist_user, model.Token, model.Password);
            if (resetResult.Succeeded)
            {
                result.Message = "Congrats! Reset password successfully!";
                result.IsSuccess = true;
                return result;
            }
            result.Message = "reset-password failed";
            List<IdentityError> errors = resetResult.Errors.ToList();
            result.Error = string.Join(", ", errors.Select(e => e.Description));
            result.IsSuccess = false;
            return result;
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
