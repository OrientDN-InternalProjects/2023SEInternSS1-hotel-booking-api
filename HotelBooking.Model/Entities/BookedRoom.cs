using HotelBooking.Common.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBooking.Model.Entities
{
    [Table("FHB_BookedRoom")]
    public class BookedRoom : BaseEntity
    {
        public Guid? RoomId { get; set; }
        [ForeignKey("RoomId")]
        public virtual Room Room { get; set; }
        public Guid? BookingId { get; set; }
        [ForeignKey("BookingId")]
        public virtual Booking Booking { get; set; }
        [Column(TypeName = "Date")]
        public DateTime From { get; set; }
        [Column(TypeName = "Date")]
        public DateTime To { get; set; }
    }
}
