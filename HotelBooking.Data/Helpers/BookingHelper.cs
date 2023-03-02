using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HotelBooking.Data.ViewModel;
using HotelBooking.Model.Entities;

namespace HotelBooking.Data.Helpers
{
    public class BookingHelper : Profile
    {
        public BookingHelper()
        {
            CreateMap<BookingVM,Booking>().ReverseMap();
        }
    }
}
