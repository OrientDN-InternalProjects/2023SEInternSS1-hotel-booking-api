using AutoMapper;
using HotelBooking.Data.DTOs.Hotel;
using HotelBooking.Model.Entities;

namespace HotelBooking.Data.Helpers
{
    public class ServiceHotelHelper : Profile
    {
        public ServiceHotelHelper()
        {
            CreateMap<CreateServiceHotelDTO, ExtraService>().ReverseMap();
        }
    }
}
