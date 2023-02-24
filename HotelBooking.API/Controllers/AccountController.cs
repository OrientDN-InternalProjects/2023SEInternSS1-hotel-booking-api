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
        private readonly ILogger<AccountController> logger;
        public AccountController(IAccountRepository accountRepository, ICurrentUser currentUser, ILogger<AccountController> logger)
        {
            this.accountRepository = accountRepository;
            this.currentUser = currentUser;
            this.logger = logger;
        }

        [HttpPost("register")]
        public async Task<ActionResult> RegisterAsync(RegisterDTO model)
        {
            if (model == null)
            {
                logger.LogError("Invalid model sent from client.");
                return BadRequest("Invalid model object");
            }
            var result = await accountRepository.RegisterAsync(model);
            if (result.IsSuccess) return Ok(result);
            else return BadRequest(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(LoginDTO model)
        {
            if (model == null)
            {
                logger.LogError("Invalid model sent from client.");
                return BadRequest("Invalid model object");
            }
            var result = await accountRepository.LoginAsync(model);
            if (result.IsSuccess) return Ok(result);
            else return BadRequest(result);
        }

        [HttpPost("forget-password")]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordDTO model)
        {
            if (model == null)
            {
                logger.LogError("Invalid model sent from client.");
                return BadRequest("Invalid model object");
            }
            var result = await accountRepository.ForgetPassword(model);
            if (result.IsSuccess) return Ok(result);
            else return BadRequest(result);
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO model)
        {
            if (model == null)
            {
                logger.LogError("Invalid model sent from client.");
                return BadRequest("Invalid model object");
            }
            var result = await accountRepository.ResetPassword(model);
            if (result.IsSuccess) return Ok(result);
            else return BadRequest(result);
        }

        [HttpPost("change-password")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDTO model)
        {
            if (model == null)
            {
                logger.LogError("Invalid model sent from client.");
                return BadRequest("Invalid model object");
            }
            var email = currentUser.UserEmail;
            var result = await accountRepository.ChangePassword(email, model);
            if (result.IsSuccess) return Ok(result);
            else return BadRequest(result);

        }
    }
}
