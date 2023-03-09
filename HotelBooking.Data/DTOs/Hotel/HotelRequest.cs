using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Data.DTOs.Hotel
{
    public class HotelRequest
    {
        [StringLength(100)]
        public string HotelName { get; set; }
        public double? Rating { get; set; }
        [StringLength(250)]
        public string Description { get; set; }
        public AddressModel Address { get; set; }
        public IEnumerable<IFormFile> Images { get; set; }
    }
}
