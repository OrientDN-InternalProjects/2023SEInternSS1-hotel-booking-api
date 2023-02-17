using HotelBooking.Common.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBooking.Model.Entities
{
    [Table("FHB_Discount")]
    public class Discount : BaseEntity
    {
        public double Percent { get; set; }
        [StringLength(250)]
        public string DiscountName { get; set; }
        public virtual ICollection<RoomDiscount> RoomDiscounts { get; set; }
    }
}