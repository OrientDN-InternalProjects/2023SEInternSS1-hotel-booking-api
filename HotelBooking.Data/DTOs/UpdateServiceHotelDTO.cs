namespace HotelBooking.Data.DTOs
{
    public class UpdateServiceHotelDTO
    {
        public string ServiceName { get; set; }
        public double ServicePrice { get; set; }
        public DateTime? UpdatedDate { get; set; } = DateTime.Now;
        public Guid Id { get; set; }
    }
}
