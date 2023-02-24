using HotelBooking.Data.Infrastructure;
using HotelBooking.Model.Entities;

namespace HotelBooking.Data.Interfaces
{
    public interface IRoomFacility : IGenericRepository<RoomFacility>
    {
        Task<IEnumerable<RoomFacility>> GetAllAsync();
        Task<RoomFacility> GetByIdAsync(Guid id);
        void CreateAsync(RoomFacility model);
        void UpdateAsync(RoomFacility model);
        void DeleteAsync(RoomFacility model);
    }
}
