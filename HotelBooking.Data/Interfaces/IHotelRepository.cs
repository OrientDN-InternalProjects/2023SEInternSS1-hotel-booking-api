using HotelBooking.Model.Entities;

namespace HotelBooking.Data.Interfaces
{
    public interface IHotelRepository
    {
        Task<IEnumerable<Hotel>> GetAllAsync();
        IQueryable<Hotel> GetByIdAsync(Guid hotelId);
        void CreateAsync(Hotel hotel);
        void UpdateAsync(Hotel hotel);
        void DeleteAsync(Hotel hotel);
        IQueryable<Hotel> GetAllHotels();
    }
}
