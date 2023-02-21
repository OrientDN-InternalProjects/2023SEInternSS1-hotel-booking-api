using HotelBooking.Data.DTOs;
using HotelBooking.Service.IServices;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacilityController : ControllerBase
    {
        private readonly IFacilityService facilityService;
        private readonly ILogger<FacilityController> logger;
        public FacilityController(IFacilityService facilityService, ILogger<FacilityController> logger)
        {
            this.facilityService = facilityService;
            this.logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateFacilityAsync([FromForm] CreateFacilityDTO model)
        {
            if (model == null)
            {
                logger.LogError("Invalid model sent from client.");
                return BadRequest("Invalid model object");
            }
            return Ok(await facilityService.AddAsync(model));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllFacilityAsync()
        {
            logger.LogInformation("Get All Facility");
            return Ok(await facilityService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFacilityByIdAsync(Guid id)
        {
            logger.LogInformation($"Get facility with Id {id}");
            var res = await facilityService.GetByIdAsync(id);
            if (res == null) return NotFound();
            return Ok(res);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFacility([FromForm] UpdateFacilityDTO model)
        {
            if (model == null)
            {
                logger.LogError("Invalid model sent from client.");
                return BadRequest("Invalid model object");
            }
            return Ok(await facilityService.UpdateAsync(model));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFacility(Guid id)
        {
            return Ok(await facilityService.DeleteAsync(id));
        }
    }
}
