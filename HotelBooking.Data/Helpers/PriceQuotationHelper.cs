using AutoMapper;
using HotelBooking.Data.DTOs.Hotel;
using HotelBooking.Model.Entities;

namespace HotelBooking.Data.Helpers
{
    public class PriceQuotationHelper : Profile
    {
        public PriceQuotationHelper()
        {
            CreateMap<PriceRequest, PriceQuotation>().ReverseMap();
        }
    }
}

