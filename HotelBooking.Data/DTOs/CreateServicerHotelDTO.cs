namespace HotelBooking.Data.DTOs
{
    public class CreateServiceHotelDTO
    {
        public string ServiceName { get; set; }
        public double ServicePrice { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
    }
}
