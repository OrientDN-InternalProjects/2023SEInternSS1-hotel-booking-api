using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Data.DTOs.Account
{
    public class ChangePasswordRequest
    {
        [Required]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "The password and confirmed password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
