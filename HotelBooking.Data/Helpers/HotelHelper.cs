using AutoMapper;
using HotelBooking.Data.DTOs.Hotel;
using HotelBooking.Data.ViewModel;
using HotelBooking.Model.Entities;

namespace HotelBooking.Data.Helpers
{
    public class HotelHelper : Profile
    {
        public HotelHelper()
        {
            CreateMap<HotelRequest, Hotel>().ReverseMap();
            CreateMap<Hotel, HotelModel>()
                .ForPath(desc => desc.Address, opt => opt.MapFrom(src => src.Address))
                .ForPath(desc => desc.Urls, opt => opt.MapFrom(src => src.Urls.Select(x => x.ImageUrl)))
                .ForPath(desc => desc.Rooms, opt => opt.MapFrom(src => src.Rooms));



        }
    }
}
