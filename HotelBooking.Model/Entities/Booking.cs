﻿using HotelBooking.Common.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBooking.Model.Entities
{
    [Table("FHB_Booking")]
    public class Booking : BaseEntity
    {
        public double Amount { get; set; }

        [DefaultValue(false)]
        public bool? PaymentStatus { get; set; }
        public virtual ICollection<BookedRoom> BookedRooms { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
    }
}