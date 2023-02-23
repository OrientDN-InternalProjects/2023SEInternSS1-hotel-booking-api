using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Data.DTOs
{
    public class CreateFacilityDTO
    {
        [StringLength(250)]
        public string FacilityName { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
    }
}
