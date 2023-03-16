using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelBooking.Common.Base;

namespace HotelBooking.Model.Entities
{
    [Table("FHB_RoomImage")]
    public class RoomImage : BaseEntity
    {
        [StringLength(500)]
        public string ImageUrl { get; set; }
        [StringLength(100)]
        public string Description { get; set; }
        public Guid? RoomId { get; set; }
        [ForeignKey("RoomId")]
        public virtual Room Room { get; set; }
    }
}
