using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Data.ViewModel
{
    public class BookingVM
    {
        public IEnumerable<string> RoomIds { get; set; }
        public bool? PaymentStatus { get; set; }
        public DurationVM Duration { get; set; }
    }
}
