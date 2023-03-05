namespace HotelBooking.Data.ViewModel
{
    public class BookingResponse
    {
        public Guid Id { get; set; }
        public double Amount { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public IEnumerable<RoomVM> Rooms { get; set; }
        public bool? PaymentStatus { get; set; }
        public string Email { get; set; }
        public UserModel User { get; set; }
    }
}
