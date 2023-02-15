using HotelBooking.Common.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBooking.Model.Entities
{
    [Table("FBH_Room_Facility")]
    public class RoomFacility : BaseEntity
    {
        public Guid? RoomId { get; set; }
        [ForeignKey("RoomId")]
        public virtual Room Room { get; set; }
        public Guid? FacilityId { get; set; }
        [ForeignKey("FacilityId")]
        public virtual Facility Facility { get; set; }
    }
}
