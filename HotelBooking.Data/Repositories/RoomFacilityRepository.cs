using HotelBooking.Data.Infrastructure;
using HotelBooking.Data.Interfaces;
using HotelBooking.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Data.Repositories
{
    public class RoomFacilityRepository : GenericRepository<RoomFacility>, IRoomFacility
    {
        public RoomFacilityRepository(BookingDbContext context) : base(context)
        {
        }

        public void CreateAsync(RoomFacility model)
        {
            Add(model);
        }

        public void DeleteAsync(RoomFacility model)
        {
            Delete(model);
        }

        public async Task<IEnumerable<RoomFacility>> GetAllAsync()
        {
            return await GetAll().OrderBy(x => x.Id).Where(x => x.IsDeleted == false).ToListAsync();
        }

        public async Task<RoomFacility> GetByIdAsync(Guid id)
        {
            return await GetByCondition(x => x.Id.Equals(id) && x.IsDeleted == false).FirstOrDefaultAsync();
        }

        public void UpdateAsync(RoomFacility model)
        {
            Update(model);
        }
    }
}
