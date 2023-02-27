using HotelBooking.Data.DTOs;
using HotelBooking.Service.IServices;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PriceQuotationController : ControllerBase
    {
        private readonly IPriceQuotationService priceQuotationService;
        private readonly ILogger<PriceQuotationController> logger;

        public PriceQuotationController(IPriceQuotationService priceQuotationService, ILogger<PriceQuotationController> logger)
        {
            this.priceQuotationService = priceQuotationService;
            this.logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePriceQuotationAsync([FromForm] CreatePriceQuotationDTO model)
        {
            if (model == null)
            {
                logger.LogError("Invalid model sent from client.");
                return BadRequest("Invalid model object");
            }
            return Ok(await priceQuotationService.AddAsync(model));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPriceQuotationAsync()
        {
            logger.LogInformation("Get All PriceQuotation");
            return Ok(await priceQuotationService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPriceQuotationByIdAsync(Guid id)
        {
            logger.LogInformation($"Get PriceQuotation with Id {id}");
            var res = await priceQuotationService.GetByIdAsync(id);
            if (res == null) return NotFound();
            return Ok(res);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePriceQuotation([FromForm] UpdatePriceQuotationDTO model)
        {
            if (model == null)
            {
                logger.LogError("Invalid model sent from client.");
                return BadRequest("Invalid model object");
            }
            return Ok(await priceQuotationService.UpdateAsync(model));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHotel(Guid id)
        {
            return Ok(await priceQuotationService.DeleteAsync(id));
        }

    }
}
