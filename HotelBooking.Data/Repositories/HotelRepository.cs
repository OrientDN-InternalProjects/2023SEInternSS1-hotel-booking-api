using HotelBooking.Data.Infrastructure;
using HotelBooking.Data.Interfaces;
using HotelBooking.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Data.Repositories
{
    public class HotelRepository : GenericRepository<Hotel>, IHotelRepository
    {
        public HotelRepository(BookingDbContext context) : base(context)
        {
        }

        public void CreateAsync(Hotel hotel)
        {
            Add(hotel);
        }

        public void DeleteAsync(Hotel hotel)
        {
            Delete(hotel);
        }

        public async Task<IEnumerable<Hotel>> GetAllAsync()
        {
            return await GetAll().OrderBy(x => x.Id).ToListAsync();
        }

        public async Task<Hotel> GetByIdAsync(Guid hotelId)
        {
            return await GetByCondition(x => x.Id.Equals(hotelId) && x.IsDeleted == false).FirstOrDefaultAsync();
        }

        public void UpdateAsync(Hotel hotel)
        {
            Update(hotel);
        }
    }
}
