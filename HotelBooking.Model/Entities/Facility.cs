using HotelBooking.Common.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBooking.Model.Entities
{
    [Table("FBH_Facility")]
    public class Facility : BaseEntity
    {
        [StringLength(250)]
        public string FacilityName { get; set; }
        public virtual ICollection<RoomFacility> RoomFacilities { get; set; }

    }
}
