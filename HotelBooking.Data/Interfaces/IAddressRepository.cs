using HotelBooking.Data.Infrastructure;
using HotelBooking.Model.Entities;

namespace HotelBooking.Data.Interfaces
{
    public interface IAddressRepository : IGenericRepository<Address>
    {
        Task<IEnumerable<Address>> GetAllAsync();
        Task<Address> GetByIdAsync(Guid addressId);
        void CreateAsync(Address address);
        void UpdateAsync(Address address);
        void DeleteAsync(Address address);
    }
}
