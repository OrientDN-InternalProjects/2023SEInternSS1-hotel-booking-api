﻿using AutoMapper;
using HotelBooking.Data.DTOs.Hotel;
using HotelBooking.Model.Entities;

namespace HotelBooking.Data.Helpers
{
    public class FacilityHelper : Profile
    {
        public FacilityHelper()
        {
            CreateMap<CreateFacilityDTO, Facility>().ReverseMap();
            CreateMap<UpdateFacilityDTO, Facility>().ReverseMap();
        }
    }
}
