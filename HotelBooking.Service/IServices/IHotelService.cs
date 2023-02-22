using HotelBooking.Data.DTOs;
using HotelBooking.Model.Entities;

namespace HotelBooking.Service.IServices
{
    public interface IHotelService
    {
        Task<Guid> AddAsync(CreateHotelDTO model);
        Task<bool> UpdateAsync(UpdateHotelDTO model);
        Task<bool> DeleteAsync(Guid id);
        Task<IEnumerable<Hotel>> GetAllAsync();
        Task<Hotel> GetByIdAsync(Guid id);
    }
}
