using HotelBooking.Data.DTOs;
using HotelBooking.Service.IServices;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountController : ControllerBase
    {
        private readonly IDiscountService discountService;
        private readonly ILogger<DiscountController> logger;
        public DiscountController(IDiscountService discountService, ILogger<DiscountController> logger)
        {
            this.discountService = discountService;
            this.logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateDiscountAsync([FromForm] CreateDiscountDTO model)
        {
            if (model == null)
            {
                logger.LogError("Invalid model sent from client.");
                return BadRequest("Invalid model object");
            }
            return Ok(await discountService.AddAsync(model));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDiscountAsync()
        {
            logger.LogInformation("Get All Discount!!!");
            return Ok(await discountService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDiscontByIdAsync(Guid id)
        {
            logger.LogInformation($"Get Discount with Id {id}");
            var res = await discountService.GetByIdAsync(id);
            if (res == null) return NotFound();
            return Ok(res);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDiscount([FromForm] UpdateDiscountDTO model)
        {
            if (model == null)
            {
                logger.LogError("Invalid model sent from client.");
                return BadRequest("Hic! Invalid model object");
            }
            return Ok(await discountService.UpdateAsync(model));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDiscont(Guid id)
        {
            return Ok(await discountService.DeleteAsync(id));
        }
    }
}
