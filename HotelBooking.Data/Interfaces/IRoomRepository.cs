using HotelBooking.Model.Entities;

namespace HotelBooking.Data.Interfaces
{
    public interface IRoomRepository
    {
        Task<IEnumerable<Room>> GetAllAsync();
        Task<Room> GetByIdAsync(Guid id);
        void CreateAsync(Room room);
        void UpdateAsync(Room room);
        void DeleteAsync(Room room);
    }
}
