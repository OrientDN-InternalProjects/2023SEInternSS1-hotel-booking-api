using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Data.DTOs
{
    public class CreateHotelDTO
    {
        [StringLength(100)]
        public string HotelName { get; set; }
        public double? Rating { get; set; }
        [StringLength(250)]
        public string Description { get; set; }
        public Guid AddressId { get; set; }
    }
}
