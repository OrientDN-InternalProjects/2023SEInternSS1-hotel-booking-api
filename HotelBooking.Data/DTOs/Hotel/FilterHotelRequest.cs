using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelBooking.Common.Enums;
using HotelBooking.Data.ViewModel;

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
