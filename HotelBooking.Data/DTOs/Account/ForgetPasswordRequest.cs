using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Data.DTOs.Account
{
    public class ForgetPasswordRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
