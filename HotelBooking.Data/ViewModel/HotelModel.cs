using System.ComponentModel.DataAnnotations;
using HotelBooking.Data.DTOs;

namespace HotelBooking.Data.ViewModel
{
    public class HotelModel
    {
        public Guid Id { get; set; }

        [StringLength(100)]
        public string HotelName { get; set; }
        public double? Rating { get; set; }
        [StringLength(250)]
        public string Description { get; set; }
        public AddressModel Address { get; set; }
        public IEnumerable<string> Urls { get; set; }
        public IEnumerable<RoomVM> Rooms { get; set; }
    }
}
