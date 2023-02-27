using HotelBooking.Data.Infrastructure;
using HotelBooking.Data.Interfaces;
using HotelBooking.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Data.Repositories
{
    public class RoomServiceRepository : GenericRepository<RoomService>, IRoomService
    {
        public RoomServiceRepository(BookingDbContext context) : base(context)
        {
        }

        public void CreateAsync(RoomService model)
        {
            Add(model);
        }

        public void DeleteAsync(RoomService model)
        {
            Delete(model);
        }

        public async Task<IEnumerable<RoomService>> GetAllAsync()
        {
            return await GetAll().OrderBy(x => x.Id).Where(x => x.IsDeleted == false).ToListAsync();
        }

        public async Task<RoomService> GetByIdAsync(Guid id)
        {
            return await GetByCondition(x => x.Id.Equals(id) && x.IsDeleted == false).FirstOrDefaultAsync();
        }

        public void UpdateAsync(RoomService model)
        {
            Update(model);
        }
    }
}
