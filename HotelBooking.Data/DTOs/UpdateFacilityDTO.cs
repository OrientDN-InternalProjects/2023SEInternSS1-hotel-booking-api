namespace HotelBooking.Data.DTOs
{
    public class UpdateFacilityDTO
    {
        public Guid Id { get; set; }
        public string FacilityName { get; set; }
        public DateTime? UpdatedDate { get; set; } = DateTime.Now;
    }
}
