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
                .ForPath(dest => dest.Type, opt => opt.MapFrom(src => src.RoomType.ToString()))
                .ForPath(desc => desc.Facilities, opt => opt.MapFrom(src => src.RoomFacilities.Select(x => x.Facility)))
                .ForPath(desc => desc.ExtraServices, opt => opt.MapFrom(src => src.RoomServices.Select(x => x.Service)))
                .ForPath(desc => desc.Urls, opt => opt.MapFrom(src => src.Urls.Select(x => x.ImageUrl)))
                .ForPath(desc => desc.Price, opt => opt.MapFrom(src => src.Price.Price));

        }
    }
}
