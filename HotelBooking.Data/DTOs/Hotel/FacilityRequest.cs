using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Data.DTOs.Hotel
{
    public class FacilityRequest
    {
        [StringLength(250)]
        public string FacilityName { get; set; }
    }
}
