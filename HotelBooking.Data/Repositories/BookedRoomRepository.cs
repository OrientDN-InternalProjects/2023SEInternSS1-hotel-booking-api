using HotelBooking.Data.Infrastructure;
using HotelBooking.Data.Interfaces;
using HotelBooking.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Data.Repositories
{
    public class BookedRoomRepository : GenericRepository<BookedRoom>, IBookedRoom
    {
        public BookedRoomRepository(BookingDbContext context) : base(context)
        {
        }

        public void CreateAsync(BookedRoom model)
        {
            Add(model);
        }

        public void DeleteAsync(BookedRoom model)
        {
            Delete(model);
        }

        public async Task<IEnumerable<BookedRoom>> GetAllAsync()
        {
            return await GetAll().OrderBy(br => br.Id).Where(br => br.IsDeleted == false).ToListAsync();
        }
        public async Task<BookedRoom> GetByIdAsync(Guid id)
        {
            return await GetByCondition(br => br.Id.Equals(id) && br.IsDeleted == false).FirstOrDefaultAsync();
        }

        public void UpdateAsync(BookedRoom model)
        {
            Update(model);
        }

    }
}
