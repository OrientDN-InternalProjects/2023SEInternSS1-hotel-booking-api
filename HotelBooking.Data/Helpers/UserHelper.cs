using AutoMapper;
using HotelBooking.Data.ViewModel;
using HotelBooking.Model.Entities;

namespace HotelBooking.Data.Helpers
{
    public class UserHelper : Profile
    {
        public UserHelper()
        {
            CreateMap<User, UserModel>().ReverseMap();
        }
    }
}
