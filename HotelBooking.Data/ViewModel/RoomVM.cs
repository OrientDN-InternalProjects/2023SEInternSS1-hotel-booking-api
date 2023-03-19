using HotelBooking.Common.Enums;
using HotelBooking.Data.DTOs.Hotel;

namespace HotelBooking.Data.ViewModel
{
    public class RoomVM
    {
        public Guid? Id { get; set; }
        public RoomType? RoomType { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public IEnumerable<ServiceHotelModel> ExtraServices { get; set; }
        public IEnumerable<FacilityModel> Facilities { get; set; }
        public IEnumerable<string> Urls { get; set; }
        public double Price { get; set; }
    }
}
