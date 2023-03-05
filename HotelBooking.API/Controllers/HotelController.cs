using HotelBooking.Common.Base;
using HotelBooking.Data.DTOs.Hotel;
using HotelBooking.Service.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> UpdateFacilityAsync(FacilityModel model, Guid id)
        {
            var result = await hotelService.UpdateFacilityAsync(model, id);
            return result
              ? StatusCode(StatusCodes.Status200OK, new ResponseModel { StatusCode = HttpStatusCode.OK, IsSuccess = true, Data = new { Guid = id } }) :
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
            if (model == null)
            {
                logger.LogError("Invalid model sent from client.");
                return BadRequest("Invalid model object");
            }
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
        public async Task<IActionResult> UpdateServiceAsync(ServiceHotelModel model, Guid id)
        {
            if (model == null)
            {
                logger.LogError("Invalid model sent from client.");
                return BadRequest("Invalid model object");
            }
            var result = await hotelService.UpdateExtraService(model, id);
            return result ?
                StatusCode(StatusCodes.Status200OK, new ResponseModel { StatusCode = HttpStatusCode.OK, IsSuccess = true, Data = new { Guid = id } })
                : StatusCode(StatusCodes.Status400BadRequest, new ResponseModel { StatusCode = HttpStatusCode.BadRequest, IsSuccess = false });
        }

        [Authorize(Roles = "Administrator", AuthenticationSchemes = "Bearer")]
        [HttpDelete("delete-service")]
        public async Task<IActionResult> DeleteExtraService(Guid id)
        {
            var result = await hotelService.DeleteExtraService(id);
            return result ?
                StatusCode(StatusCodes.Status200OK, new ResponseModel { StatusCode = HttpStatusCode.OK, IsSuccess = true, Data = new { Guid = id } })
                : StatusCode(StatusCodes.Status400BadRequest, new ResponseModel {StatusCode = HttpStatusCode.BadRequest,IsSuccess = false});
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

        [HttpPost("fiters-hotel")]
        public async Task<IActionResult> FilterHotel(FilterHotelRequest filter)
        {

            var result = await hotelService.GetHotelByAddressTypeRoomDuration(filter);
            return (result.Any()) ?
            StatusCode(StatusCodes.Status200OK, new ResponseModel{StatusCode = HttpStatusCode.OK,Data = new {hotel = result},IsSuccess = true})
            : StatusCode(StatusCodes.Status404NotFound, new ResponseModel{StatusCode = HttpStatusCode.NotFound,IsSuccess = false});
        }

        [HttpGet("get-hotel-by-name")]
        public async Task<IActionResult> GetHotelByNameAsync(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                logger.LogError("Invalid name fo find");
                return BadRequest("Invalid model object");
            }
            var result = await hotelService.GetHotelByName(name);
            return result != null ?
                 StatusCode(StatusCodes.Status200OK, new ResponseModel{StatusCode = HttpStatusCode.OK,IsSuccess = true,Data = new{data = result,}})
                : StatusCode(StatusCodes.Status404NotFound, new ResponseModel{StatusCode = HttpStatusCode.NotFound,IsSuccess = false});
        }

        [HttpGet("get-hotel-by-id")]
        public async Task<IActionResult> GetHotelById(Guid id)
        {
            var result = await hotelService.GetHotelByIdAsync(id);
            return result != null ?
                StatusCode(StatusCodes.Status200OK, new ResponseModel{StatusCode = HttpStatusCode.OK,IsSuccess = true,Data = new{data = result,}})
                : StatusCode(StatusCodes.Status404NotFound, new ResponseModel{StatusCode = HttpStatusCode.NotFound,IsSuccess = false});
        }

        [HttpGet("get-room-by-id")]
        public async Task<IActionResult> GetRoomById(Guid id)
        {
            var result = await hotelService.GetRoomByIdAsync(id);
            return (result != null) ?
                StatusCode(StatusCodes.Status200OK, new ResponseModel
                {StatusCode = HttpStatusCode.OK,IsSuccess = true,Data = new{data = result,}}) 
                :StatusCode(StatusCodes.Status404NotFound, new ResponseModel{StatusCode = HttpStatusCode.NotFound,IsSuccess = false});
        }
    }
}
