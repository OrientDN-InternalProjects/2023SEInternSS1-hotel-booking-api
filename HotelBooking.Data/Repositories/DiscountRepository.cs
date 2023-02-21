using HotelBooking.Data.Infrastructure;
using HotelBooking.Data.Interfaces;
using HotelBooking.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Data.Repository
{
    public class DiscountRepository : GenericRepository<Discount>, IDiscountRepository
    {
        public DiscountRepository(BookingDbContext context) : base(context)
        {
        }
        public void CreateDiscount(Discount discount)
        {
            Add(discount);
        }

        public void DeleteDiscount(Discount discount)
        {
            Delete(discount);
        }

        public async Task<IEnumerable<Discount>> GetAllDiscountAsync()
        {
            return await GetAll().OrderBy(discount => discount.Id).ToListAsync();
        }

        public async Task<Discount?> GetDiscountByIdAsync(Guid discountId)
        {
            return await GetByCondition(discount => discount.Id.Equals(discountId) && discount.IsDeleted == false).FirstOrDefaultAsync();
        }

        public void UpdateDiscount(Discount discount)
        {
            Update(discount);
        }
    }
}
