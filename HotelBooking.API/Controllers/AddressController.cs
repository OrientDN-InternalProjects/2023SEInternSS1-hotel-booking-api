using HotelBooking.Data.DTOs;
using HotelBooking.Service.IServices;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService addressService;
        private readonly ILogger<AddressController> logger;

        public AddressController(IAddressService addressService, ILogger<AddressController> logger)
        {
            this.addressService = addressService;
            this.logger = logger;
        }

        //[Authorize(Roles = "Administrator", AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> CreateAddressAsync([FromForm] CreateAddressDTO model)
        {
            if (model == null)
            {
                logger.LogError("Invalid model sent from client.");
                return BadRequest("Invalid model object");
            }
            return Ok(await addressService.AddAsync(model));
        }

        //[Authorize(Roles = "Administrator", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> GetAllAddressAsync()
        {
            logger.LogInformation("Get All Address");
            return Ok(await addressService.GetAllAsync());
        }

        //[Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAddressByIdAsync(Guid id)
        {
            logger.LogInformation($"Get address with Id {id}");
            var res = await addressService.GetByIdAsync(id);
            if (res == null) return NotFound();
            return Ok(res);
        }

        //[Authorize(Roles = "Administrator", AuthenticationSchemes = "Bearer")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAddress([FromForm] UpdateAddressDTO model)
        {
            if (model == null)
            {
                logger.LogError("Invalid model sent from client.");
                return BadRequest("Invalid model object");
            }
            return Ok(await addressService.UpdateAsync(model));
        }

        //[Authorize(Roles = "Administrator", AuthenticationSchemes = "Bearer")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdress(Guid id)
        {
            return Ok(await addressService.DeleteAsync(id));
        }
    }
}
