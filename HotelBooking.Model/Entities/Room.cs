using HotelBooking.Common.Base;
using HotelBooking.Common.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBooking.Model.Entities
{
    [Table("FHB_Room")]
    public class Room : BaseEntity
    {
        public RoomType? RoomType { get; set; }
        [StringLength(500)]
        public string Description { get; set; }
        public Guid? HotelId { get; set; }
        [ForeignKey("HotelId")]
        public virtual Hotel Hotel { get; set; }
        public Guid? BookingId { get; set; }
        [ForeignKey("BookingId")]
        public virtual Booking Booking { get; set; }
        public Guid? PriceId { get; set; }
        [ForeignKey("PriceId")]
        public virtual PriceQuotation Price { get; set; }
        public virtual ICollection<RoomService> RoomServices { get; set; }
        public virtual ICollection<RoomFacility> RoomFacilities { get; set; }
        public virtual ICollection<BookedRoom> BookedRooms { get; set;}
    }
}
