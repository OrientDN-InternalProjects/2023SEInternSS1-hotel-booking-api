using HotelBooking.Data.DTOs;
using HotelBooking.Service.IServices;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExtraServiceController : ControllerBase
    {
        private readonly IServiceHotel serviceHotel;
        private readonly ILogger<ExtraServiceController> logger;
        public ExtraServiceController(IServiceHotel serviceHotel, ILogger<ExtraServiceController> logger)
        {
            this.serviceHotel = serviceHotel;
            this.logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateExtraSeviceAsync([FromForm] CreateServiceHotelDTO model)
        {
            if (model == null)
            {
                logger.LogError("Invalid model sent from client.");
                return BadRequest("Hic! Invalid model object");
            }
            return Ok(await serviceHotel.AddAsync(model));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllExtraServiceAsync()
        {
            logger.LogInformation("Get All ExtraService");
            return Ok(await serviceHotel.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetExtraServiceByIdAsync(Guid id)
        {
            logger.LogInformation($"Get ExtraService with Id {id}");
            var res = await serviceHotel.GetByIdAsync(id);
            if (res == null) return NotFound();
            return Ok(res);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAddress([FromForm] UpdateServiceHotelDTO model)
        {
            if (model == null)
            {
                logger.LogError("Invalid model sent from client.");
                return BadRequest("Hic! Invalid model object");
            }
            return Ok(await serviceHotel.UpdateAsync(model));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdress(Guid id)
        {
            return Ok(await serviceHotel.DeleteAsync(id));
        }
    }
}
