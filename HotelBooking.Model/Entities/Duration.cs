using HotelBooking.Common.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBooking.Model.Entities
{
    [Table("FBH_Duration")]
    public class Duration : BaseEntity
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public virtual Booking Booking { get; set; }
    }
}
