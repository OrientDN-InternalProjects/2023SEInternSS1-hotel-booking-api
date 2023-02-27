namespace HotelBooking.Data.ViewModel
{
    public class HotelVM
    {
        public Guid Id { get; set; }
        public string HotelName { get; set; }
        public double? Rating { get; set; }
        public string Description { get; set; }
        public AddressVM Address { get; set; }
        public IEnumerable<RoomVM> Rooms { get; set; }
        public IEnumerable<string> Urls { get; set; }
    }
}
