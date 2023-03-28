using HotelBooking.Common.Base;
using HotelBooking.Common.Enums;
using HotelBooking.Data.DTOs.Booking;
using HotelBooking.Data.DTOs.Hotel;
using HotelBooking.Data.ViewModel;
using HotelBooking.Service.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;

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

        [Authorize(Roles = "Administrator", AuthenticationSchemes = "Bearer")]
        [HttpPost("create-hotel")]
        public async Task<IActionResult> CreateHotelAsync([FromForm] HotelRequest model)
        {
            if (model == null)
            {
                logger.LogError("Invalid model sent from client.");
                return BadRequest("Invalid model object");
            }
            var result = await hotelService.AddHotelAsync(model);
            return StatusCode(StatusCodes.Status201Created, new ResponseModel
            {
                StatusCode = HttpStatusCode.Created,
                Message = "Created hotel successfully",
                Data = new
                {
                    id = result,
                },
                IsSuccess = true
            });

        }

        [HttpGet("all-hotels")]
        public async Task<IActionResult> GetAllHotels()
        {
            var result = await hotelService.GetAllHotel();
            if (result.Any())
            {
                return StatusCode(StatusCodes.Status200OK, new ResponseModel
                {
                    StatusCode = HttpStatusCode.OK,
                    IsSuccess = true,
                    Data = result
                });
            }
            return StatusCode(StatusCodes.Status404NotFound, new ResponseModel
            {
                StatusCode = HttpStatusCode.NotFound,
                IsSuccess = false
            });
        }

        [Authorize(Roles = "Administrator", AuthenticationSchemes = "Bearer")]
        [HttpPut("update-hotel")]
        public async Task<IActionResult> UpdateHotelAsync([FromForm] HotelRequest model, Guid id)
        {
            if (model == null)
            {
                logger.LogError("Invalid model sent from client.");
                return BadRequest("Invalid model object");
            }
            var result = await hotelService.UpdateHotel(model, id);
            if (result)
            {
                return StatusCode(StatusCodes.Status200OK, new ResponseModel
                {
                    StatusCode = HttpStatusCode.OK,
                    IsSuccess = true,
                    Data = new
                    {
                        Guid = id
                    }
                });
            }
            return StatusCode(StatusCodes.Status400BadRequest, new ResponseModel
            {
                StatusCode = HttpStatusCode.BadRequest,
                IsSuccess = false
            });
        }

        [Authorize(Roles = "Administrator", AuthenticationSchemes = "Bearer")]
        [HttpDelete("delete-hotel")]
        public async Task<IActionResult> DeleteHotel(Guid id)
        {
            var result = await hotelService.DeleteHotel(id);
            if (result)
            {
                return StatusCode(StatusCodes.Status200OK, new ResponseModel
                {
                    StatusCode = HttpStatusCode.OK,
                    IsSuccess = true,
                    Data = new
                    {
                        Guid = id
                    }
                });
            }
            return StatusCode(StatusCodes.Status400BadRequest, new ResponseModel
            {
                StatusCode = HttpStatusCode.BadRequest,
                IsSuccess = false
            });
        }

        [HttpPost("get-all-rooms-available")]
        public async Task<IActionResult> GetAllRoomsAvailable(Guid idHotel, DurationVM duration)
        {
            var result = await hotelService.GetAllRoomAvailable(idHotel, duration);
            if (result != null)
            {
                return StatusCode(StatusCodes.Status200OK, new ResponseModel
                {
                    StatusCode = HttpStatusCode.OK,
                    IsSuccess = true,
                    Data = result
                });
            }
            return StatusCode(StatusCodes.Status404NotFound, new ResponseModel
            {
                StatusCode = HttpStatusCode.NotFound,
                IsSuccess = false
            });
        }

        [HttpGet("fiters-hotel")]
        public async Task<IActionResult> FilterHotel(string name, DateTime? from, DateTime? to, string city, RoomType? roomType)
        {
            var result = await hotelService.SearchHotel(name, from, to, city, roomType);
            return result != null ?
                 StatusCode(StatusCodes.Status200OK, new ResponseModel { StatusCode = HttpStatusCode.OK, IsSuccess = true, Data = result})
                : StatusCode(StatusCodes.Status200OK, new ResponseModel { StatusCode = HttpStatusCode.NotFound, IsSuccess = false, Data = new List<HotelModel>()});
        }

        [Authorize(Roles = "Administrator", AuthenticationSchemes = "Bearer")]
        [HttpPost("create-room")]
        public async Task<IActionResult> CreateRoomAsync([FromForm] RoomRequest model)
        {
            var result = (await hotelService.AddRoomAsync(model));
            return StatusCode(StatusCodes.Status201Created, new ResponseModel
            {
                StatusCode = HttpStatusCode.Created,
                Message = "Created room successfully",
                Data = new
                {
                    id = result
                },
                IsSuccess = true
            });
        }

        [Authorize(Roles = "Administrator", AuthenticationSchemes = "Bearer")]
        [HttpPut("update-equip-room")]
        public async Task<IActionResult> UpdateEquipRoomAsync(EquipRoomRequest model)
        {
            var result = await hotelService.UpdateRoomEquipment(model);
            return result ?
                StatusCode(StatusCodes.Status200OK, new ResponseModel { StatusCode = HttpStatusCode.OK, IsSuccess = true, Data = new { Guid = model.RoomId } })
                : StatusCode(StatusCodes.Status400BadRequest, new ResponseModel { StatusCode = HttpStatusCode.BadRequest, IsSuccess = false });
        }

        [Authorize(Roles = "Administrator", AuthenticationSchemes = "Bearer")]
        [HttpDelete("delete-room")]
        public async Task<IActionResult> DeleteRoom(Guid id)
        {
            var result = await hotelService.DeleteRoom(id);
            return result ?
                StatusCode(StatusCodes.Status200OK, new ResponseModel { StatusCode = HttpStatusCode.OK, IsSuccess = true, Data = new { Guid = id } })
                : StatusCode(StatusCodes.Status400BadRequest, new ResponseModel { StatusCode = HttpStatusCode.BadRequest, IsSuccess = false });
        }

        [Authorize(Roles = "Administrator", AuthenticationSchemes = "Bearer")]
        [HttpPost("create-facility")]
        public async Task<IActionResult> CreateFacilityAsync([FromForm] FacilityModel model)
        {
            var result = await hotelService.AddFacilityAsync(model);
            return StatusCode(StatusCodes.Status201Created, new ResponseModel
            {
                StatusCode = HttpStatusCode.Created,
                Message = "Created facility successfully",
                Data = new
                {
                    id = result
                },
                IsSuccess = true
            });
        }

        [Authorize(Roles = "Administrator", AuthenticationSchemes = "Bearer")]
        [HttpPut("update-facility")]
        public async Task<IActionResult> UpdateFacilityAsync(FacilityModel model)
        {
            var result = await hotelService.UpdateFacilityAsync(model);
            return result
              ? StatusCode(StatusCodes.Status200OK, new ResponseModel { StatusCode = HttpStatusCode.OK, IsSuccess = true }) :
              StatusCode(StatusCodes.Status400BadRequest, new ResponseModel { StatusCode = HttpStatusCode.BadRequest, IsSuccess = false });
        }

        [Authorize(Roles = "Administrator", AuthenticationSchemes = "Bearer")]
        [HttpDelete("delete-facility")]
        public async Task<IActionResult> DeleteFacility(Guid id)
        {
            var result = await hotelService.DeleteFacilityAsync(id);
            return result
                ? StatusCode(StatusCodes.Status200OK, new ResponseModel { StatusCode = HttpStatusCode.OK, IsSuccess = true, Data = new { Guid = id } }) :
                StatusCode(StatusCodes.Status400BadRequest, new ResponseModel { StatusCode = HttpStatusCode.BadRequest, IsSuccess = false });
        }

        [HttpGet("get-all-facilities")]
        public async Task<IActionResult> GetAllFacilities()
        {
            var result = await hotelService.GetAllFacilities();
            return (result != null) ?
                StatusCode(StatusCodes.Status200OK, new ResponseModel { StatusCode = HttpStatusCode.OK, IsSuccess = true, Data = result })
                : StatusCode(StatusCodes.Status404NotFound, new ResponseModel { StatusCode = HttpStatusCode.NotFound, IsSuccess = false });
        }

        [HttpGet("get-facility-by-id")]
        public async Task<IActionResult> GetFacilityById(Guid id)
        {
            var result = await hotelService.GetFacilityById(id);
            return (result != null) ?
                StatusCode(StatusCodes.Status200OK, new ResponseModel { StatusCode = HttpStatusCode.OK, IsSuccess = true, Data = result }) :
                StatusCode(StatusCodes.Status404NotFound, new ResponseModel { StatusCode = HttpStatusCode.NotFound, IsSuccess = false });
        }

        [Authorize(Roles = "Administrator", AuthenticationSchemes = "Bearer")]
        [HttpPost("create-service")]
        public async Task<IActionResult> CreateServiceAsync([FromForm] ServiceHotelModel model)
        {
            var result = await hotelService.AddExtraServiceAsync(model);
            return StatusCode(StatusCodes.Status201Created, new ResponseModel
            {
                StatusCode = HttpStatusCode.Created,
                Message = "Created service successfully",
                Data = new
                {
                    id = result
                },
                IsSuccess = true
            });
        }

        [Authorize(Roles = "Administrator", AuthenticationSchemes = "Bearer")]
        [HttpPut("update-service")]
        public async Task<IActionResult> UpdateServiceAsync(ServiceHotelModel model)
        {
            var result = await hotelService.UpdateExtraService(model);
            return result ?
                StatusCode(StatusCodes.Status200OK, new ResponseModel { StatusCode = HttpStatusCode.OK, IsSuccess = true })
                : StatusCode(StatusCodes.Status400BadRequest, new ResponseModel { StatusCode = HttpStatusCode.BadRequest, IsSuccess = false });
        }

        [Authorize(Roles = "Administrator", AuthenticationSchemes = "Bearer")]
        [HttpDelete("delete-service")]
        public async Task<IActionResult> DeleteExtraService(Guid id)
        {
            var result = await hotelService.DeleteExtraService(id);
            return result ?
                StatusCode(StatusCodes.Status200OK, new ResponseModel { StatusCode = HttpStatusCode.OK, IsSuccess = true, Data = new { Guid = id } })
                : StatusCode(StatusCodes.Status400BadRequest, new ResponseModel { StatusCode = HttpStatusCode.BadRequest, IsSuccess = false });
        }

        [HttpGet("get-all-services")]
        public async Task<IActionResult> GetAllServices()
        {
            var result = await hotelService.GetAllExtraService();
            return (result != null) ?
                StatusCode(StatusCodes.Status200OK, new ResponseModel { StatusCode = HttpStatusCode.OK, IsSuccess = true, Data = result })
                : StatusCode(StatusCodes.Status404NotFound, new ResponseModel { StatusCode = HttpStatusCode.NotFound, IsSuccess = false });
        }

        [HttpGet("get-service-by-id")]
        public async Task<IActionResult> GetServiceById(Guid id)
        {
            var result = await hotelService.GetExtraServiceById(id);
            return (result != null) ?
                StatusCode(StatusCodes.Status200OK, new ResponseModel { StatusCode = HttpStatusCode.OK, IsSuccess = true, Data = result })
                : StatusCode(StatusCodes.Status404NotFound, new ResponseModel { StatusCode = HttpStatusCode.NotFound, IsSuccess = false });
        }

        [Authorize(Roles = "Administrator", AuthenticationSchemes = "Bearer")]
        [HttpPost("equip-room")]
        public async Task<IActionResult> EquipRoomAsync([FromForm] EquipRoomRequest model)
        {
            var result = await hotelService.AddServiceAndFacilityToRoomAsync(model);
            return StatusCode(StatusCodes.Status201Created,
                new ResponseModel
                {
                    StatusCode = HttpStatusCode.Created,
                    Message = "equip room successfully",
                    IsSuccess = true
                });
        }

        [HttpGet("get-hotel-by-id")]
        public async Task<IActionResult> GetHotelById(Guid id)
        {
            var result = await hotelService.GetHotelByIdAsync(id);
            return result != null ?
                StatusCode(StatusCodes.Status200OK, new ResponseModel { StatusCode = HttpStatusCode.OK, IsSuccess = true, Data = result })
                : StatusCode(StatusCodes.Status404NotFound, new ResponseModel { StatusCode = HttpStatusCode.NotFound, IsSuccess = false });
        }

        [HttpGet("get-room-by-id")]
        public async Task<IActionResult> GetRoomById(Guid id)
        {
            var result = await hotelService.GetRoomByIdAsync(id);
            return (result != null) ?
                StatusCode(StatusCodes.Status200OK, new ResponseModel
                { StatusCode = HttpStatusCode.OK, IsSuccess = true, Data = new { data = result, } })
                : StatusCode(StatusCodes.Status404NotFound, new ResponseModel { StatusCode = HttpStatusCode.NotFound, IsSuccess = false });
        }

        [HttpGet("get-all-hotel-paged-list")]
        public async Task<IActionResult> GetAllHotelPagedList([FromQuery] PagedListRequest request)
        {
            var hotels = await hotelService.GetHotelPagedList(request);
            return Ok(new ResponseModel
            { StatusCode = HttpStatusCode.OK, IsSuccess = true, Data = new { items = hotels, totalPage = hotels.TotalPages } });
        }

    }
}
