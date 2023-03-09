using AutoMapper;
using HotelBooking.Data.DTOs.Hotel;
using HotelBooking.Data.ViewModel;
using HotelBooking.Model.Entities;

namespace HotelBooking.Data.Helpers
{
    public class RoomHelper : Profile
    {
        public RoomHelper()
        {
            CreateMap<RoomRequest, Room>().ReverseMap();
            CreateMap<Room, RoomVM>()
                .ForPath(desc => desc.Facilities, opt => opt.MapFrom(src => src.RoomFacilities.Select(x => x.Facility)))
                .ForPath(desc => desc.ExtraServices, opt => opt.MapFrom(src => src.RoomServices.Select(x => x.Service)));
        }
    }
}
