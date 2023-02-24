using HotelBooking.Data.DTOs.Hotel;
using HotelBooking.Service.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelController : ControllerBase
    {
        private readonly IHotelService hotelService;
        private readonly ILogger<HotelController> logger;

        public HotelController(IHotelService hotelService, ILogger<HotelController> logger)
        {
            this.hotelService = hotelService;
            this.logger = logger;
        }

        [Authorize(Roles = "Administrator", AuthenticationSchemes = "Bearer")]
        [HttpPost("create-hotel")]
        public async Task<IActionResult> CreateHotelAsync([FromForm] CreateHotelDTO model)
        {
            if (model == null)
            {
                logger.LogError("Invalid model sent from client.");
                return BadRequest("Invalid model object");
            }
            return Ok(await hotelService.AddHotelAsync(model));
        }

        [Authorize(Roles = "Administrator", AuthenticationSchemes = "Bearer")]
        [HttpPost("create-room")]
        public async Task<IActionResult> CreateRoomAsync([FromForm] CreateRoomDTO model)
        {
            if (model == null)
            {
                logger.LogError("Invalid model sent from client.");
                return BadRequest("Invalid model object");
            }
            return Ok(await hotelService.AddRoomAsync(model));
        }

        [Authorize(Roles = "Administrator", AuthenticationSchemes = "Bearer")]
        [HttpPost("create-facility")]
        public async Task<IActionResult> CreateFacilityAsync([FromForm] CreateFacilityDTO model)
        {
            if (model == null)
            {
                logger.LogError("Invalid model sent from client.");
                return BadRequest("Invalid model object");
            }
            return Ok(await hotelService.AddFacilityAsync(model));
        }

        [Authorize(Roles = "Administrator", AuthenticationSchemes = "Bearer")]
        [HttpPost("create-service")]
        public async Task<IActionResult> CreateServiceAsync([FromForm] CreateServiceHotelDTO model)
        {
            if (model == null)
            {
                logger.LogError("Invalid model sent from client.");
                return BadRequest("Invalid model object");
            }
            return Ok(await hotelService.AddExtraServiceAsync(model));
        }

        [Authorize(Roles = "Administrator", AuthenticationSchemes = "Bearer")]
        [HttpPost("equip-room")]
        public async Task<IActionResult> EquipRoomAsync([FromForm] EquipRoomDTO model)
        {
            if (model == null)
            {
                logger.LogError("Invalid model sent from client.");
                return BadRequest("Invalid model object");
            }
            return Ok(await hotelService.AddServiceAndFacilityToRoomAsync(model));
        }
    }
}
