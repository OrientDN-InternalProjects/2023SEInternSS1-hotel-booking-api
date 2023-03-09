using HotelBooking.Common.Enums;
using HotelBooking.Model.Entities;

namespace HotelBooking.Data.Helpers
{
    public static class FilterHelper
    {
        public static IQueryable<Hotel> ApplyFilterByAddress(
            this IQueryable<Hotel> hotelQuery,
            string city)

        => hotelQuery.Where(x => x.Address.City.Equals(city) && x.IsDeleted == false);

        public static IQueryable<Hotel> ApplyFilterByRoomType(
            this IQueryable<Hotel> hotelQuery,
            RoomType roomType)
        => hotelQuery.Where(x => x.Rooms.Any(x => x.RoomType.Equals(roomType)) == x.IsDeleted == false);

        public static IQueryable<Hotel> ApplyFilterByName(
            this IQueryable<Hotel> hotelQuery,
            string name)
        => hotelQuery.Where(x => x.HotelName.ToLower().Contains(name.ToLower()) && x.IsDeleted == false);


    }
}
