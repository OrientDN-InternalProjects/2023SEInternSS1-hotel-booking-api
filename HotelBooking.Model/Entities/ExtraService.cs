using HotelBooking.Common.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBooking.Model.Entities
{
    [Table("FBH_Service")]
    public class ExtraService : BaseEntity
    {
        [StringLength(250)]
        public string ServiceName { get; set; }
        public double ServicePrice { get; set; }
        public ICollection<RoomService> RoomServices { get; set; }
    }
}
