using HotelBooking.Data.Infrastructure;
using HotelBooking.Model.Entities;

namespace HotelBooking.Data.Interfaces
{
    public interface IDiscountRepository : IGenericRepository<Discount>
    {
        Task<IEnumerable<Discount>> GetAllDiscountAsync();
        Task<Discount?> GetDiscountByIdAsync(Guid discountId);
        void CreateDiscount();
        void UpdateDiscount();
        void DeleteDiscount();
    }
}
