using AutoMapper;
using HotelBooking.Data.DTOs.Hotel;
using HotelBooking.Model.Entities;

namespace HotelBooking.Data.Helpers
{
    public class ServiceHotelHelper : Profile
    {
        public ServiceHotelHelper()
        {
            CreateMap<ServiceHotelModel, ExtraService>().ReverseMap();
            CreateMap<ServiceHotelModel, ExtraService>().ReverseMap();
        }
    }
}
