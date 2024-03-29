﻿using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Data.DTOs
{
    public class AddressModel
    {
        public Guid? Id { get; set; }
        [StringLength(250)]
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
