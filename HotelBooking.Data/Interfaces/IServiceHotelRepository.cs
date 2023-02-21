using HotelBooking.Data.Infrastructure;
using HotelBooking.Model.Entities;

namespace HotelBooking.Data.Interfaces
{
    public interface IServiceHotelRepository : IGenericRepository<ExtraService>
    {
        Task<IEnumerable<ExtraService>> GetAllServiceHotelesAsync();
        Task<ExtraService?> GetServiceHotelByIdAsync(Guid ServiceHotelId);
        void CreateServiceHotel(ExtraService ServiceHotel);
        void UpdateServiceHotel(ExtraService ServiceHotel);
        void DeleteServiceHotel(ExtraService ServiceHotel);
    }
}
