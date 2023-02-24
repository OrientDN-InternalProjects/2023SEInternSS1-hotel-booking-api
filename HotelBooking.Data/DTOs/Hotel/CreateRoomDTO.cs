using HotelBooking.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Data.DTOs.Hotel
{
    public class CreateRoomDTO
    {
        public RoomType RoomType { get; set; }
        [StringLength(500)]
        public string HotelId { get; set; }
        public CreatePriceQuotationDTO Price { get; set; }
        public string Description { get; set; }
    }
}
