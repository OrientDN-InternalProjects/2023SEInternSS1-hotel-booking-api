using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBooking.Data.ViewModel
{
    public class DurationVM
    {
        [Column(TypeName = "Date")]
        public DateTime From { get; set; }
        [Column(TypeName = "Date")]
        public DateTime To { get; set; }
    }
}
