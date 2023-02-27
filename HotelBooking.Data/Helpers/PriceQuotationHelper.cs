using AutoMapper;
using HotelBooking.Data.DTOs.Hotel;
using HotelBooking.Model.Entities;

namespace HotelBooking.Data.Helpers
{
    public class PriceQuotationHelper : Profile
    {
        public PriceQuotationHelper()
        {
            CreateMap<CreatePriceQuotationDTO, PriceQuotation>().ReverseMap();
            CreateMap<UpdatePriceQuotationDTO, PriceQuotation>().ReverseMap();
        }
    }
}

