using HotelBooking.Common.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBooking.Model.Entities
{
    [Table("FHB_Address")]
    public class Address : BaseEntity
    {
        [StringLength(250)]
        public string City { get; set; }
        [StringLength(100)]
        public string PinCode { get; set; }
        [StringLength(250)]
        public string StreetNumber { get; set; }
        [StringLength(250)]
        public string District { get; set; }
        [StringLength(250)]
        public string Building { get; set; }
        public virtual Hotel Hotel { get; set; }
    }
}