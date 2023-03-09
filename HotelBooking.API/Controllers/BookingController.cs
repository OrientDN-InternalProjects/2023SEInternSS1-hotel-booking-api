using HotelBooking.Common.Base;
using HotelBooking.Data.DTOs.Hotel;
using HotelBooking.Data.Extensions;
using HotelBooking.Data.ViewModel;
using HotelBooking.Service.IServices;
using HotelBooking.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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
            if (!res.Any())
            {
                logger.LogError("Can't find any hotels");
                return StatusCode(StatusCodes.Status404NotFound, new ResponseModel
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "Can't find any hotels",
                });
            }
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
            if (currentUser.IsAuthenicated)
            {
                model.UserId = currentUser.UserId;
                model.Email = currentUser.UserEmail;
            }
            var res = await bookingService.AddBookingAsync(model);

            if (res.IsSuccess == true)
            {
                return StatusCode(StatusCodes.Status201Created, res);
            }
            logger.LogError("Create booking failed");
            return StatusCode(StatusCodes.Status400BadRequest, new ResponseModel
            {
                StatusCode = HttpStatusCode.BadRequest,
                Message = "Created booking failed",
                IsSuccess = false
            });
        }

        [HttpPost("check-room-filter-duration")]
        public async Task<IActionResult> FilterRoomByDuration(DurationVM model, Guid roomId)
        {
            var result = await bookingService.CheckValidationDurationForRoom(model, roomId);
            if (result)
            {
                return StatusCode(StatusCodes.Status200OK, new ResponseModel
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    IsSuccess = true,
                    Message = "You can book at this duration"
                }
                );
            }
            logger.LogError("Choose duration in this duration failed");
            return StatusCode(StatusCodes.Status400BadRequest, new ResponseModel
            {
                StatusCode = System.Net.HttpStatusCode.BadRequest,
                IsSuccess = false,
                Message = "You can't book at this duration"
            }
            );
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPut("update-booking")]
        public async Task<IActionResult> UpdateBookingAsync(BookingVM model, Guid id)
        {
            var result = await bookingService.UpdateBookingAsync(model, id);
            if (result)
            {
                return StatusCode(StatusCodes.Status200OK, new ResponseModel
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    IsSuccess = true,
                    Message = "Update booking successfully"
                }
                );
            }
            logger.LogError("Update booking failed");
            return StatusCode(StatusCodes.Status400BadRequest, new ResponseModel
            {
                StatusCode = System.Net.HttpStatusCode.BadRequest,
                IsSuccess = false,
                Message = "Update booking false"
            }
            );
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpDelete("delete-booking")]
        public async Task<IActionResult> DeleteBookingAsync(Guid id)
        {
            var result = await bookingService.DeleteBookingAsync(id);
            if (result)
            {
                return StatusCode(StatusCodes.Status200OK, new ResponseModel
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    IsSuccess = true,
                    Message = "Invalid booking successfully"
                }
                );
            }
            logger.LogError("Invalid booking failed");
            return StatusCode(StatusCodes.Status400BadRequest, new ResponseModel
            {
                StatusCode = System.Net.HttpStatusCode.BadRequest,
                IsSuccess = false,
                Message = "Invalid booking false"
            }
            );
        }

    }
}
