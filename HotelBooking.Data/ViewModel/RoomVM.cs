using HotelBooking.Common.Enums;

namespace HotelBooking.Data.ViewModel
{
    public class RoomVM
    {
        public RoomType? RoomType { get; set; }
        public RoomStatus? RoomStatus { get; set; }
        public string Description { get; set; }
        public IEnumerable<ServiceVM> ExtraServices { get; set; }
        public IEnumerable<FacilityVM> Facilities { get; set; }
    }
}
