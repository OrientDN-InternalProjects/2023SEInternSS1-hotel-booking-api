
using HotelBooking.Common.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBooking.Model.Entities
{
    [Table("FHB_PriceQuotation")]
    public class PriceQuotation : BaseEntity
    {
        public double Price { get; set; }
        public virtual Room Room{ get; set; }
    }
}
