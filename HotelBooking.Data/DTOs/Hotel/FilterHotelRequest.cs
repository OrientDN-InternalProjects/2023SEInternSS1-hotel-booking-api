using HotelBooking.Common.Enums;
using HotelBooking.Data.ViewModel;
using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Data.DTOs.Hotel
{
    public class FilterHotelRequest
    {
        [Required]
        public string City { get; set; }
        [Required]
        public DurationVM Duration { get; set; }
        [Required]
        public RoomType RoomType { get; set; }
    }
}
