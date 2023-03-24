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
        public async Task<ActionResult> RegisterAsync(RegisterRequest model)
        {
            if (model == null)
            {
                logger.LogError("Invalid model sent from client.");
                return BadRequest("Invalid model object");
            }
            var result = await accountRepository.RegisterAsync(model);
            if ((int)result.StatusCode == 404) return NotFound(result);
            if ((int)result.StatusCode == 400) return BadRequest(result);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(LoginRequest model)
        {
            if (model == null)
            {
                logger.LogError("Invalid model sent from client.");
                return BadRequest("Invalid model object");
            }
            var result = await accountRepository.LoginAsync(model);
            return Ok(result);
        }

        [HttpPost("forget-password")]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordRequest model)
        {
            if (model == null)
            {
                logger.LogError("Invalid model sent from client.");
                return BadRequest("Invalid model object");
            }
            var result = await accountRepository.ForgetPassword(model);
            if ((int)result.StatusCode == 404) return NotFound(result);
            if ((int)result.StatusCode == 400) return BadRequest(result);
            return Ok(result);
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest model)
        {
            if (model == null)
            {
                logger.LogError("Invalid model sent from client.");
                return BadRequest("Invalid model object");
            }
            var result = await accountRepository.ResetPassword(model);
            if ((int)result.StatusCode == 404) return NotFound(result);
            if ((int)result.StatusCode == 400) return BadRequest(result);
            return Ok(result);
        }

        [HttpPost("change-password")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest model)
        {
            if (model == null)
            {
                logger.LogError("Invalid model sent from client.");
                return BadRequest("Invalid model object");
            }
            var email = currentUser.UserEmail;
            var result = await accountRepository.ChangePassword(email, model);
            if ((int)result.StatusCode == 404) return NotFound(result);
            if ((int)result.StatusCode == 400) return BadRequest(result);
            return Ok(result);


        }
    }
}
