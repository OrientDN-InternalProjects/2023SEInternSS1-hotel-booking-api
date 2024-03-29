﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HotelBooking.Data.DTOs;
using HotelBooking.Data.DTOs.Hotel;
using HotelBooking.Model.Entities;

namespace HotelBooking.Data.Helpers
{
    public class AddressHelper : Profile
    {
        public AddressHelper()
        {
            CreateMap<AddressModel, Address>()
              .ForMember(x => x.Id, opt => opt.Ignore()).ReverseMap();
        }
    }
}
