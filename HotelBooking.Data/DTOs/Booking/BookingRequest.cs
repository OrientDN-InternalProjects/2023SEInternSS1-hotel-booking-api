using HotelBooking.Data.ViewModel;

namespace HotelBooking.Data.DTOs.Booking
{
    public class BookingRequest
    {
        public IEnumerable<Guid> RoomIds { get; set; }
        public bool? PaymentStatus { get; set; }
        public DurationVM Duration { get; set; }
        public string Email { get; set; }
        public string UserId { get; set; }
    }
}
