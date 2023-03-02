using AutoMapper;
using HotelBooking.Data.DTOs.Hotel;
using HotelBooking.Model.Entities;

namespace HotelBooking.Data.Helpers
{
    public class RoomHelper : Profile
    {
        public RoomHelper()
        {
            CreateMap<RoomRequest, Room>().ReverseMap();
        }
    }
}
