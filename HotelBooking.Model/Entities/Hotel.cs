using HotelBooking.Common.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBooking.Model.Entities
{
    [Table("FHB_Hotel")]
    public class Hotel : BaseEntity
    {
        [StringLength(100)]
        public string HotelName { get; set; }
        public double? Rating { get; set; }

        public string Description { get; set; }
        public Guid? AddressId { get; set; }
        [ForeignKey("AddressId")]
        public virtual Address Address { get; set; }
        public virtual ICollection<Image> Urls { get; set; }
        public virtual ICollection<Room> Rooms { get; set; }
    }
}
