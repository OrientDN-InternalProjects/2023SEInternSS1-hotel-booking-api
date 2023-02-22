using AutoMapper;
using HotelBooking.Data.DTOs;
using HotelBooking.Model.Entities;

namespace HotelBooking.Data.Helpers
{
    public class HotelHelper : Profile
    {
        public HotelHelper()
        {
            CreateMap<CreateHotelDTO, Hotel>().ReverseMap();
            CreateMap<UpdateHotelDTO, Hotel>().ReverseMap();
        }
    }
}
