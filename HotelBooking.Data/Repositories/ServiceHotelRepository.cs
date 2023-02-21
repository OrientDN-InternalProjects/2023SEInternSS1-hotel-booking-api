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
        public void CreateServiceHotel(ExtraService ServiceHotel)
        {
            Add(ServiceHotel);
        }

        public void DeleteServiceHotel(ExtraService ServiceHotel)
        {
            Delete(ServiceHotel);
        }

        public async Task<ExtraService?> GetServiceHotelByIdAsync(Guid ServiceHotelId)
        {
            return await GetByCondition(ServiceHotel => ServiceHotel.Id.Equals(ServiceHotelId) && ServiceHotel.IsDeleted == false).FirstOrDefaultAsync();

        }
        public async Task<IEnumerable<ExtraService>> GetAllServiceHotelesAsync()
        {
            return await GetAll().OrderBy(ServiceHotel => ServiceHotel.Id).ToListAsync();
        }

        public void UpdateServiceHotel(ExtraService ServiceHotel)
        {
            Update(ServiceHotel);
        }
    }
}
