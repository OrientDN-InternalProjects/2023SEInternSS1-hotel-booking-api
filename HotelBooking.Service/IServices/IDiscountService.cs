using HotelBooking.Data.DTOs;
using HotelBooking.Model.Entities;

namespace HotelBooking.Service.IServices
{
    public interface IDiscountService
    {
        Task<Guid> AddAsync(CreateDiscountDTO model);
        Task<bool> UpdateAsync(UpdateDiscountDTO model);
        Task<bool> DeleteAsync(Guid id);
        Task<IEnumerable<Discount>> GetAllAsync();
        Task<Discount?> GetByIdAsync(Guid id);
    }
}
