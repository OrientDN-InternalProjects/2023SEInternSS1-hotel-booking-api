using HotelBooking.Data.Infrastructure;
using HotelBooking.Model.Entities;

namespace HotelBooking.Data.Interfaces
{
    public interface IServiceHotelRepository : IGenericRepository<ExtraService>
    {
        Task<IEnumerable<ExtraService>> GetAllAsync();
        Task<ExtraService> GetByIdAsync(Guid ServiceHotelId);
        void CreateAsync(ExtraService extraService);
        void UpdateAsync(ExtraService extraService);
        void DeleteAsync(ExtraService extraService);
    }
}
