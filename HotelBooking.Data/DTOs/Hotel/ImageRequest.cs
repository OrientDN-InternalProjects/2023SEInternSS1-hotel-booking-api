using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Data.DTOs.Hotel
{
    public class ImageRequest
    {
        public IFormFile Image { get; set; }
        [StringLength(100)]
        public string Description { get; set; }
    }
}
