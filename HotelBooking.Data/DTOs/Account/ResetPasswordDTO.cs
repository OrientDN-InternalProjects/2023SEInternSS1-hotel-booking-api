using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Data.DTOs.Account
{
    public class ResetPasswordDTO
    {
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmed password do not match.")]
        public string ConfirmPassword { get; set; }
        public string Token { get; set; }
    }
}
