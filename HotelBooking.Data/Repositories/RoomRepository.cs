using HotelBooking.Data.Infrastructure;
using HotelBooking.Data.Interfaces;
using HotelBooking.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Data.Repositories
{
    public class RoomRepository : GenericRepository<Room>, IRoomRepository
    {
        public RoomRepository(BookingDbContext context) : base(context)
        {
        }

        public void CreateAsync(Room room)
        {
            Add(room);
        }

        public void DeleteAsync(Room room)
        {
            Delete(room);
        }

        public async Task<IEnumerable<Room>> GetAllAsync()
        {
            return await GetAll().OrderBy(x => x.Id).Where(x => x.IsDeleted == false).ToListAsync();
        }

        public IQueryable<Room> GetByIdAsync(Guid id)
        {
            return GetByCondition(x => x.Id.Equals(id) && x.IsDeleted == false);
        }

        public void UpdateAsync(Room room)
        {
            Update(room);
        }
    }
}
