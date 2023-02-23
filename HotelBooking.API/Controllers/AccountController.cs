using HotelBooking.Data.DTOs.Account;
using HotelBooking.Data.Extensions;
using HotelBooking.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository accountRepository;
        private readonly ICurrentUser currentUser;
        public AccountController(IAccountRepository accountRepository, ICurrentUser currentUser)
        {
            this.accountRepository = accountRepository;
            this.currentUser = currentUser;
        }

        [HttpPost("register")]
        public async Task<ActionResult> RegisterAsync(RegisterDTO model)
        {
            var result = await accountRepository.RegisterAsync(model);
            if (result.IsSuccess) return Ok(result);
            else return BadRequest(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(LoginDTO model)
        {
            var result = await accountRepository.LoginAsync(model);
            if (result.IsSuccess) return Ok(result);
            else return BadRequest(result);
        }

        [HttpPost("forget-password")]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordDTO model)
        {
            var result = await accountRepository.ForgetPassword(model);
            if (result.IsSuccess) return Ok(result);
            else return BadRequest(result);
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO model)
        {
            var result = await accountRepository.ResetPassword(model);
            if (result.IsSuccess) return Ok(result);
            else return BadRequest(result);
        }

        [HttpPost("change-password")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDTO model)
        {
            var email = currentUser.UserEmail;
            var result = await accountRepository.ChangePassword(email, model);
            if (result.IsSuccess) return Ok(result);
            else return BadRequest(result);

        }
    }
}
