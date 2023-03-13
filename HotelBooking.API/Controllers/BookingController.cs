using HotelBooking.Common.Base;
using HotelBooking.Data.DTOs.Booking;
using HotelBooking.Data.Extensions;
using HotelBooking.Data.ViewModel;
using HotelBooking.Service.IServices;
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
        private readonly ICheckDurationValidationService checkDurationValidationService;
        private readonly ICurrentUser currentUser;
        private readonly ILogger<BookingController> logger;

        public BookingController(IBookingService bookingService,
            ILogger<BookingController> logger, ICurrentUser currentUser,
            ICheckDurationValidationService checkDurationValidationService)
        {
            this.bookingService = bookingService;
            this.logger = logger;
            this.currentUser = currentUser;
            this.checkDurationValidationService = checkDurationValidationService;
        }

        [HttpPost("create-booking")]
        public async Task<IActionResult> CreateBookingAsync(BookingRequest model)
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

            return res.IsSuccess == true ?
                StatusCode(StatusCodes.Status201Created, res) : StatusCode(StatusCodes.Status400BadRequest, res);
        }

        [HttpPost("filter-room-by-duration")]
        public async Task<IActionResult> FilterRoomByDuration(DurationVM model, Guid roomId)
        {
            var result = await checkDurationValidationService.CheckValidationDurationForRoom(model, roomId);
            return result ?
                StatusCode(StatusCodes.Status200OK, new ResponseModel
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    IsSuccess = true,
                    Message = "You can book at this duration"
                }
                )
                :
                StatusCode(StatusCodes.Status400BadRequest, new ResponseModel
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    IsSuccess = false,
                    Message = "You can't book at this duration"
                }
                );
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPut("update-booking")]
        public async Task<IActionResult> UpdateBookingAsync(BookingRequest model, Guid id)
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
            logger.LogInformation("Update booking failed");
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
            return result ?
                StatusCode(StatusCodes.Status200OK, new ResponseModel
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    IsSuccess = true,
                    Message = "Invalid booking successfully"
                }
                )
                : StatusCode(StatusCodes.Status400BadRequest, new ResponseModel
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    IsSuccess = false,
                    Message = "Invalid booking false"
                }
                );
        }

        [HttpGet("get-booking-by-id")]
        public async Task<IActionResult> GetBookingByIdAsync(Guid id)
        {
            var result = await bookingService.GetBookingById(id);

            return result != null ?
                StatusCode(StatusCodes.Status200OK, new ResponseModel
                {
                    StatusCode = HttpStatusCode.OK,
                    IsSuccess = true,
                    Data = new
                    {
                        data = result,
                    }
                })
                : StatusCode(StatusCodes.Status404NotFound, new ResponseModel
                {
                    StatusCode = HttpStatusCode.NotFound,
                    IsSuccess = false
                });
        }

        [HttpGet("get-booking-by-user")]
        public async Task<IActionResult> GetBookingByUserAsync(string email)
        {
            var result = await bookingService.GetAllBookingsByUser(email);

            return result != null ?
            StatusCode(StatusCodes.Status200OK, new ResponseModel
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Data = new
                {
                    data = result,
                }
            })
            : StatusCode(StatusCodes.Status404NotFound, new ResponseModel
            {
                StatusCode = HttpStatusCode.NotFound,
                IsSuccess = false
            });
        }

        [HttpGet("get-all-booking-paged-list")]
        public async Task<IActionResult> GetAllBookingPagedList([FromQuery] PagedListRequest request)
        {
            var bookings = await bookingService.GetBookingPagedList(request);
            return Ok(bookings);
        }
    }
}
