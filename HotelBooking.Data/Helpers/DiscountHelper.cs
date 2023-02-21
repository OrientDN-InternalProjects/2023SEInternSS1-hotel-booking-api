using AutoMapper;
using HotelBooking.Data.DTOs;
using HotelBooking.Model.Entities;

namespace HotelBooking.Data.Helpers
{
    public class DiscountHelper : Profile
    {
        public DiscountHelper()
        {
            CreateMap<CreateDiscountDTO, Discount>().ReverseMap();
            CreateMap<UpdateDiscountDTO, Discount>().ReverseMap();
        }
    }
}
