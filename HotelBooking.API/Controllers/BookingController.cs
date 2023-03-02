using HotelBooking.Data.Extensions;
using HotelBooking.Data.ViewModel;
using HotelBooking.Service.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace HotelBooking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService bookingService;
        private readonly ICurrentUser currentUser;
        private readonly ILogger<BookingController> logger;

        public BookingController(IBookingService bookingService, ILogger<BookingController> logger, ICurrentUser currentUser)
        {
            this.bookingService = bookingService;
            this.logger = logger;
            this.currentUser = currentUser;
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

        [HttpPost("create-booking")]
        public async Task<IActionResult> CreateBookingAsync([FromForm] BookingVM model)
        {
            if (model == null)
            {
                logger.LogError("Invalid model sent from client.");
                return BadRequest("Invalid model object");
            }
            if(currentUser.IsAuthenicated)
            {
                model.UserId = currentUser.UserId;
                model.Email = currentUser.UserEmail;
            }
            var res = await bookingService.AddBookingAsync(model);
            return Ok(res);
        }

        [HttpGet("check_room_filter_duration")]
        public async Task<IActionResult> FilterRoomByDuration(DurationVM model, Guid roomId)
        {
            var result = await bookingService.CheckValidationDurationForRoom(model, roomId);
            return Ok(result);
        }

        [HttpPut("update-booking")]
        public async Task<IActionResult> UpdateBookingAsync(BookingVM model, Guid id)
        {
            var result = await bookingService.UpdateBookingAsync(model,id);
            return Ok(result);
        }

        [HttpDelete("delete-booking")]
        public async Task<IActionResult> DeleteBookingAsync(Guid id)
        {
            var result = await bookingService.DeleteBookingAsync(id);
            return Ok(result);
        }

    }
}
