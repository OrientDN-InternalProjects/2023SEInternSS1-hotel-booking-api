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
        public void CreateAsync(Address address)
        {
            Add(address);
        }

        public void DeleteAsync(Address address)
        {
            Delete(address);
        }

        public async Task<Address> GetByIdAsync(Guid addressId)
        {
            return await GetByCondition(address => address.Id.Equals(addressId) && address.IsDeleted == false).FirstOrDefaultAsync();

        }
        public async Task<IEnumerable<Address>> GetAllAsync()
        {
            return await GetAll().OrderBy(address => address.Id).ToListAsync();
        }

        public void UpdateAsync(Address address)
        {
            Update(address);
        }
    }
}
