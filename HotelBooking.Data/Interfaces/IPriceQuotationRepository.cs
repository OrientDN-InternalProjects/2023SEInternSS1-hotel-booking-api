using HotelBooking.Data.Infrastructure;
using HotelBooking.Model.Entities;

namespace HotelBooking.Data.Interfaces
{
    public interface IPriceQuotationRepository : IGenericRepository<PriceQuotation>
    {
        Task<IEnumerable<PriceQuotation>> GetAllPriceQuotationAsync();
        Task<PriceQuotation?> GetPriceQuotationByIdAsync(Guid PriceQuotationId);
        void CreatePriceQuotation(PriceQuotation PriceQuotation);
        void UpdatePriceQuotation(PriceQuotation PriceQuotation);
        void DeletePriceQuotation(PriceQuotation PriceQuotation);
    }
}
