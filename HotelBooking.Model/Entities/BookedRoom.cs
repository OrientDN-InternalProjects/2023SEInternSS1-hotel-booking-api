using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelBooking.Common.Base;

namespace HotelBooking.Model.Entities
{
    [Table("FHB_BookedRoom")]
    public class BookedRoom : BaseEntity
    {
        public Guid? RoomId { get; set; }
        [ForeignKey("RoomId")]
        public virtual Room Room { get; set; }
        public Guid? BookingId{ get; set;}
        [ForeignKey("BookingId")]
        public virtual Booking Booking { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}
