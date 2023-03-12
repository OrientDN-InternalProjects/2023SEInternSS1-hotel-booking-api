using HotelBooking.Data.Infrastructure;
using HotelBooking.Data.Interfaces;
using HotelBooking.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Data.Repositories
{
    public class BookingRepository : GenericRepository<Booking>, IBookingRepository
    {
        public BookingRepository(BookingDbContext context) : base(context)
        {
        }

        public void CreateAsync(Booking booking)
        {
            Add(booking);
        }

        public void DeleteAsync(Booking booking)
        {
            Delete(booking);
        }

        public async Task<IEnumerable<Booking>> GetAllAsync()
        {
            return await GetAll().OrderBy(b => b.Id).Where(b => b.IsDeleted == false).ToListAsync();
        }

        public IQueryable<Booking> GetAllBookings()
        {
            return GetAll();
        }
        public IQueryable<Booking> GetByIdAsync(Guid id)
        {
            return GetByCondition(b => b.Id.Equals(id) && b.IsDeleted == false);
        }

        public IQueryable<Booking> GetByUserEmail(string email)
        {
            return GetByCondition(b => b.IsDeleted == false && b.Email.Equals(email));
        }

        public void UpdateAsync(Booking booking)
        {
            Update(booking);
        }
    }
}
