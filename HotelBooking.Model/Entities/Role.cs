using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace HotelBooking.Model.Entities
{
    public class Role : IdentityRole<Guid>
    {
        [StringLength(250)]
        public string Description { get; set; }
    }
}
