using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Data.DTOs.Account
{
    public class ForgetPasswordDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
