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
        public IQueryable<Hotel> GetAllHotels()
        {
            return GetAll();
        }
        public async Task<IEnumerable<Hotel>> GetAllAsync()
        {
            return await GetAll().OrderBy(x => x.Id).Where(x => x.IsDeleted == false).ToListAsync();
        }

        public IQueryable<Hotel> GetByIdAsync(Guid hotelId)

          => GetByCondition(x => x.Id.Equals(hotelId) && x.IsDeleted == false);

        public void UpdateAsync(Hotel hotel)
        {
            Update(hotel);
        }
    }
}
