using HotelBooking.Data.DTOs;
using HotelBooking.Model.Entities;

namespace HotelBooking.Service.IServices
{
    public interface IPriceQuotationService
    {
        Task<Guid> AddAsync(CreatePriceQuotationDTO model);
        Task<bool> UpdateAsync(UpdatePriceQuotationDTO model);
        Task<bool> DeleteAsync(Guid id);
        Task<IEnumerable<PriceQuotation>> GetAllAsync();
        Task<PriceQuotation> GetByIdAsync(Guid id);
    }
}
