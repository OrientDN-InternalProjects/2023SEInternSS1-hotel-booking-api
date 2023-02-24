using HotelBooking.Data.Infrastructure;
using HotelBooking.Data.Interfaces;
using HotelBooking.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Data.Repository
{
    public class PriceQuotationRepository : GenericRepository<PriceQuotation>, IPriceQuotationRepository
    {
        public PriceQuotationRepository(BookingDbContext context) : base(context)
        {
        }

        public void DeleteAsync(PriceQuotation priceQuotation)
        {
            Delete(priceQuotation);
        }

        public async Task<IEnumerable<PriceQuotation>> GetAllPriceQuotationAsync()
        {
            return await GetAll().OrderBy(x => x.Id).Where(x => x.IsDeleted == false).ToListAsync();
        }

        public async Task<PriceQuotation> GetPriceQuotationByIdAsync(Guid PriceQuotationId)
        {
            return await GetByCondition(p => p.Id.Equals(PriceQuotationId) && p.IsDeleted == false).FirstOrDefaultAsync();
        }

        public void CreatePriceQuotation(PriceQuotation PriceQuotation)
        {
            Add(PriceQuotation);
        }

        public void UpdatePriceQuotation(PriceQuotation PriceQuotation)
        {
            Update(PriceQuotation);
        }

        public void DeletePriceQuotation(PriceQuotation PriceQuotation)
        {
            Delete(PriceQuotation);
        }
    }
}
