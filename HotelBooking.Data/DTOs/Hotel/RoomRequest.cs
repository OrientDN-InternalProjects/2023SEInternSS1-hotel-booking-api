using HotelBooking.Common.Enums;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Data.DTOs.Hotel
{
    public class RoomRequest
    {
        public RoomType RoomType { get; set; }
        [StringLength(500)]
        public string HotelId { get; set; }
        public PriceRequest Price { get; set; }
        public string Description { get; set; }
        public IEnumerable<IFormFile> Images { get; set; }
    }
}
