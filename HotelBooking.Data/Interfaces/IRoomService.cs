using HotelBooking.Data.Infrastructure;
using HotelBooking.Model.Entities;

namespace HotelBooking.Data.Interfaces
{
    public interface IRoomService : IGenericRepository<RoomService>
    {
        Task<IEnumerable<RoomService>> GetAllAsync();
        Task<RoomService> GetByIdAsync(Guid id);
        void CreateAsync(RoomService model);
        void UpdateAsync(RoomService model);
        void DeleteAsync(RoomService model);
    }
}
