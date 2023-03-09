using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Data.DTOs.Hotel
{
    public class FacilityModel
    {
        public Guid? Id { get; set; }
        [StringLength(250)]
        public string FacilityName { get; set; }
    }
}
