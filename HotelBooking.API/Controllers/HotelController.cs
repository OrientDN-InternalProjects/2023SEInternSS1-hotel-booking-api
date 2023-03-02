﻿using HotelBooking.Common.Base;
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

        [Authorize(Roles = "Administrator", AuthenticationSchemes = "Bearer")]
        [HttpPost("create-room")]
        public async Task<IActionResult> CreateRoomAsync([FromForm] RoomRequest model)
        {
            if (model == null)
            {
                logger.LogError("Invalid model sent from client.");
                return BadRequest("Invalid model object");
            }
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
        public async Task<IActionResult> CreateFacilityAsync([FromForm] FacilityRequest model)
        {
            if (model == null)
            {
                logger.LogError("Invalid model sent from client.");
                return BadRequest("Invalid model object");
            }
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
        [HttpPost("create-service")]
        public async Task<IActionResult> CreateServiceAsync([FromForm] ServiceHotelRequest model)
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
        [HttpPost("equip-room")]
        public async Task<IActionResult> EquipRoomAsync([FromForm] EquipRoomRequest model)
        {
            if (model == null)
            {
                logger.LogError("Invalid model sent from client.");
                return BadRequest("Invalid model object");
            }
            var result = await hotelService.AddServiceAndFacilityToRoomAsync(model);
            return StatusCode(StatusCodes.Status201Created,
                new ResponseModel
                {
                    StatusCode = HttpStatusCode.Created,
                    Message = "equip room successfully",
                    IsSuccess = true
                });
        }
    }
}
