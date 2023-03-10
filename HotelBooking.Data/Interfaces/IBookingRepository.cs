using HotelBooking.Data.Infrastructure;
using HotelBooking.Model.Entities;

namespace HotelBooking.Data.Interfaces
{
    public interface IBookingRepository : IGenericRepository<Booking>
    {
        Task<IEnumerable<Booking>> GetAllAsync();
        IQueryable<Booking> GetByIdAsync(Guid id);
        IQueryable<Booking> GetByUserEmail(string email);
        void CreateAsync(Booking booking);
        void UpdateAsync(Booking booking);
        void DeleteAsync(Booking booking);
    }
}
