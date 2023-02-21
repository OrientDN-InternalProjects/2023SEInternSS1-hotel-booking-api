using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Data.DTOs
{
    public class UpdateDiscountDTO
    {
        public double Percent { get; set; }
        [StringLength(250)]
        public string DiscountName { get; set; }
        public DateTime? UpdatedDate { get; set; } = DateTime.Now;
        public Guid Id { get; set; }
    }
}
