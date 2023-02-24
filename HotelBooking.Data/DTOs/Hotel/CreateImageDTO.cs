using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace HotelBooking.Data.DTOs.Hotel
{
    public class CreateImageDTO
    {
        public IFormFile Image { get; set; }
        [StringLength(100)]
        public string Description { get; set; }
    }
}
