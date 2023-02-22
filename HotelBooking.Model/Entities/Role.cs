using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Model.Entities
{
    public class Role : IdentityRole
    {
        [StringLength(250)]
        public string Description { get; set; }
    }
}
