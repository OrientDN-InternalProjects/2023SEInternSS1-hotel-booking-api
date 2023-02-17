using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Model.Entities
{
    public class Role : IdentityRole<Guid>
    {
        [StringLength(250)]
        public string Description { get; set; }
    }
}
