﻿namespace HotelBooking.Data.DTOs.Hotel
{
    public class EquipRoomRequest
    {
        public Guid RoomId { get; set; }
        public IEnumerable<string> ServiceIds { get; set; }
        public IEnumerable<string> FacilityIds { get; set; }
    }
}
