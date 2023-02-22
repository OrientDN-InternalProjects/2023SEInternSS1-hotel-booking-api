using HotelBooking.Data.DTOs;
using HotelBooking.Service.IServices;
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

        [HttpPost]
        public async Task<IActionResult> CreateHotelAsync([FromForm] CreateHotelDTO model)
        {
            if (model == null)
            {
                logger.LogError("Invalid model sent from client.");
                return BadRequest("Invalid model object");
            }
            return Ok(await hotelService.AddAsync(model));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllHotelsAsync()
        {
            logger.LogInformation("Get All Hotels");
            return Ok(await hotelService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetHotelByIdAsync(Guid id)
        {
            logger.LogInformation($"Get address with Id {id}");
            var res = await hotelService.GetByIdAsync(id);
            if (res == null) return NotFound();
            return Ok(res);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateHotel([FromForm] UpdateHotelDTO model)
        {
            if (model == null)
            {
                logger.LogError("Invalid model sent from client.");
                return BadRequest("Invalid model object");
            }
            return Ok(await hotelService.UpdateAsync(model));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHotel(Guid id)
        {
            return Ok(await hotelService.DeleteAsync(id));
        }

    }
}
