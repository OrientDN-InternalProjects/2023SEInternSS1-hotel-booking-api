using HotelBooking.Data.Infrastructure;
using HotelBooking.Model.Entities;

namespace HotelBooking.Data.Interfaces
{
    public interface IRoomRepository : IGenericRepository<Room>
    {
        Task<IEnumerable<Room>> GetAllAsync();
        IQueryable<Room> GetByIdAsync(Guid id);
        void CreateAsync(Room room);
        void UpdateAsync(Room room);
        void DeleteAsync(Room room);
    }
}
