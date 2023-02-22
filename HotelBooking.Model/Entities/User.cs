
using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace HotelBooking.Model.Entities
{
    public class User : IdentityUser
    {
        [StringLength(250)]
        public string FullName { get; set; }
        public DateTime CreatedOn { get; set; }
        [DefaultValue(true)]
        public bool IsActive { get; set; }
    }
}
