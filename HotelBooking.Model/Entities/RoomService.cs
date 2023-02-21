using HotelBooking.Common.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBooking.Model.Entities
{
    [Table("FBH_Room_Service")]
    public class RoomService : BaseEntity
    {
        public Guid? RoomId { get; set; }
        [ForeignKey("RoomId")]
        public virtual Room Room { get; set; }
        public Guid? ServiceId { get; set; }
        [ForeignKey("ServiceId")]
        public virtual ExtraService Service { get; set; }
    }
}
