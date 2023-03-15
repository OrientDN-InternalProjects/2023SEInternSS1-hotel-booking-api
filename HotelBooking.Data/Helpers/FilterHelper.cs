using HotelBooking.Common.Enums;
using HotelBooking.Model.Entities;
using FuzzySharp;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Data.Helpers
{
    public static class FilterHelper
    {
        public static IQueryable<Hotel> ApplyFilterByAddress(
            this IQueryable<Hotel> hotelQuery,
            string city)

        => string.IsNullOrEmpty(city) ? hotelQuery : hotelQuery.Where(x => x.Address.City.Equals(city));

        public static IQueryable<Hotel> ApplyFilterByRoomType(
            this IQueryable<Hotel> hotelQuery,
            RoomType? roomType)
        => roomType is not null ?  hotelQuery.Where(x => x.Rooms.Any(x => x.RoomType.Equals(roomType))) : hotelQuery;

    }
}
