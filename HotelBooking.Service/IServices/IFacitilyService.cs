using HotelBooking.Data.DTOs;
using HotelBooking.Model.Entities;

namespace HotelBooking.Service.IServices
{
    public interface IFacilityService
    {
        Task<Guid> AddAsync(CreateFacilityDTO model);
        Task<bool> UpdateAsync(UpdateFacilityDTO model);
        Task<bool> DeleteAsync(Guid id);
        Task<IEnumerable<Facility>> GetAllAsync();
        Task<Facility?> GetByIdAsync(Guid id);
    }
}
