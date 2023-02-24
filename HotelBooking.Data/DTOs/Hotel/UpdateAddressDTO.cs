using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelBooking.Model.Entities;

namespace HotelBooking.Data.DTOs.Hotel
{
    public class UpdateAddressDTO
    {
        public string City { get; set; }

        [StringLength(100)]
        public string PinCode { get; set; }

        [StringLength(250)]
        public string StreetNumber { get; set; }

        [StringLength(250)]
        public string District { get; set; }

        [StringLength(250)]
        public string Building { get; set; }
    }
}
