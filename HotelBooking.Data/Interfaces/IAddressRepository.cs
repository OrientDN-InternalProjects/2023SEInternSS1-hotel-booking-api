using HotelBooking.Data.Infrastructure;
using HotelBooking.Model.Entities;

namespace HotelBooking.Data.Interfaces
{
    public interface IAddressRepository : IGenericRepository<Address>
    {
        Task<IEnumerable<Address>> GetAllAddressesAsync();
        Task<Address?> GetAddressByIdAsync(Guid addressId);
        void CreateAddress(Address address);
        void UpdateAddress(Address address);
        void DeleteAddress(Address address);
    }
}
