using HotelBooking.Data.DTOs;
using HotelBooking.Model.Entities;

namespace HotelBooking.Service.IServices
{
    public interface IServiceHotel
    {
        Task<Guid> AddAsync(CreateServiceHotelDTO model);
        Task<bool> UpdateAsync(UpdateServiceHotelDTO model);
        Task<bool> DeleteAsync(Guid id);
        Task<IEnumerable<ExtraService>> GetAllAsync();
        Task<ExtraService?> GetByIdAsync(Guid id);
    }
}
