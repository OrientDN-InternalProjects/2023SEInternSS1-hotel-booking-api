using HotelBooking.Common.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBooking.Model.Entities
{
    [Table("FHB_Room_Discount")]
    public class RoomDiscount : BaseEntity
    {
        public Guid? RoomId { get; set; }
        [ForeignKey("RoomId")]
        public virtual Room Room { get; set; }
        public Guid? DiscountId { get; set; }
        [ForeignKey("DiscountId")]
        public virtual Discount Discount { get; set; }
    }
}
