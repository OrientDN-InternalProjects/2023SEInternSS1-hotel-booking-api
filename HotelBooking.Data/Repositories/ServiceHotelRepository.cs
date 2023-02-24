using HotelBooking.Data.Infrastructure;
using HotelBooking.Data.Interfaces;
using HotelBooking.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Data.Repository
{
    public class ServiceHotelRepository : GenericRepository<ExtraService>, IServiceHotelRepository
    {
        public ServiceHotelRepository(BookingDbContext context) : base(context)
        {
        }

        public void CreateAsync(ExtraService extraService)
        {
            Add(extraService);
        }

        public void DeleteAsync(ExtraService extraService)
        {
            Delete(extraService);
        }

        public async Task<IEnumerable<ExtraService>> GetAllAsync()
        {
            return await GetAll().OrderBy(s => s.Id).Where(s => s.IsDeleted == false).ToListAsync();
        }

        public async Task<ExtraService> GetByIdAsync(Guid ServiceHotelId)
        {
            return await GetByCondition(s => s.Id.Equals(ServiceHotelId) && s.IsDeleted == false).FirstOrDefaultAsync();
        }

        public void UpdateAsync(ExtraService extraService)
        {
            Update(extraService);
        }
    }
}
