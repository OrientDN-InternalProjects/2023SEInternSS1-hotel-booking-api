using HotelBooking.Common.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBooking.Model.Entities
{
    [Table("FHB_Booking")]
    public class Booking : BaseEntity
    {
        public double? Amount { get; set; }
        public bool? PaymentStatus { get; set; }
        public Guid? DurationId { get; set; }
        [ForeignKey("DurationId")]
        public virtual Duration Duration { get; set; }
        public virtual ICollection<Room> Rooms { get; set; }
    }
}