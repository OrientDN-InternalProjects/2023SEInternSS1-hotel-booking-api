using HotelBooking.Data.ViewModel;
using HotelBooking.Service.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService bookingService;
        private readonly ILogger<BookingController> logger;

        public BookingController(IBookingService bookingService, ILogger<BookingController> logger)
        {
            this.bookingService = bookingService;
            this.logger = logger;
        }

        [HttpGet("search-hotel")]
        public async Task<IActionResult> SearchHotelByNameAsync(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                logger.LogError("Invalid name fo find");
                return BadRequest("Invalid model object");
            }
            logger.LogInformation($"Search hotel by name {name} and show Detail");
            var res = await bookingService.SearchHotelByName(name);
            if (res == null) return NotFound();
            return Ok(res);
        }

        [Authorize(Roles = "Administrator", AuthenticationSchemes = "Bearer")]
        [HttpPost("create-booking")]
        public async Task<IActionResult> CreateBookingAsync([FromForm] BookingVM model)
        {
            if (model == null)
            {
                logger.LogError("Invalid model sent from client.");
                return BadRequest("Invalid model object");
            }
            var res = await bookingService.AddBookingAsync(model);
            return Ok(res);
        }

        [HttpGet("check_room_filter_duration")]
        public async Task<IActionResult> FilterRoomByDuration(DurationVM model, string roomId)
        {
            await bookingService.CheckValidationDurationForRoom(model, roomId);
            return Ok(model);
        }

    }
}
