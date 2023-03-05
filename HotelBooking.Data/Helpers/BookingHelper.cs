using AutoMapper;
using HotelBooking.Data.DTOs.Booking;
using HotelBooking.Data.ViewModel;
using HotelBooking.Model.Entities;

namespace HotelBooking.Data.Helpers
{
    public class BookingHelper : Profile
    {
        public BookingHelper()
        {
            CreateMap<BookingRequest, Booking>().ReverseMap();
            CreateMap<Booking, BookingResponse>()
                .ForPath(desc => desc.Rooms, opt => opt.MapFrom(src => src.BookedRooms.Select(x => x.Room)));

        }
    }
}
