using HotelBooking.Data.Infrastructure;
using HotelBooking.Data.Interfaces;
using HotelBooking.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Data.Repository
{
    public class AddressRepository : GenericRepository<Address>, IAddressRepository
    {
        public AddressRepository(BookingDbContext context) : base(context)
        {
        }
        public void CreateAddress(Address address)
        {
            Add(address);
        }

        public void DeleteAddress(Address address)
        {
            Delete(address);
        }

        public async Task<Address?> GetAddressByIdAsync(Guid addressId)
        {
            return await GetByCondition(address => address.Id.Equals(addressId) && address.IsDeleted == false).FirstOrDefaultAsync();
            
        } 
        public async Task<IEnumerable<Address>> GetAllAddressesAsync()
        {
            return await GetAll().OrderBy(address => address.Id).ToListAsync();
        }

        public void UpdateAddress(Address address)
        {
            Update(address);
        }
    }
}
