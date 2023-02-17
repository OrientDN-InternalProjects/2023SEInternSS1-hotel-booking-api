using HotelBooking.Common.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBooking.Model.Entities
{
    [Table("FHB_Image")]
    public class Image : BaseEntity
    {
        [StringLength(500)]
        public string ImageUrl { get; set; }
        [StringLength(100)]
        public string Description { get; set; }
        public Guid? HotelId { get; set; }
        [ForeignKey("HotelId")]
        public virtual Hotel Hotel { get; set; }
    }
}
