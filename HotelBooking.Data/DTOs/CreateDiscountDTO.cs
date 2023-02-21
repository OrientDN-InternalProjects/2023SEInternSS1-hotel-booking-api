using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Data.DTOs
{
    public class CreateDiscountDTO
    {
        public double Percent { get; set; }
        [StringLength(250)]
        public string DiscountName { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
    }
}
