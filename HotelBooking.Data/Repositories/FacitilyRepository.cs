using HotelBooking.Data.Infrastructure;
using HotelBooking.Data.Interfaces;
using HotelBooking.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Data.Repository
{

    public class FacilityRepository : GenericRepository<Facility>, IFacilityRepository
    {
        public FacilityRepository(BookingDbContext context) : base(context)
        {
        }

        public void CreateFacility(Facility facility)
        {
            Add(facility);
        }

        public void DeleteFacility(Facility facility)
        {
            Delete(facility.Id);
        }

        public async Task<IEnumerable<Facility>> GetAllFacilityAsync()
        {
            return await GetAll().OrderBy(Facility => Facility.Id).ToListAsync();
        }

        public async Task<Facility> GetFacilityByIdAsync(Guid FacilityId)
        {
            return await GetByCondition(Facility => Facility.Id.Equals(FacilityId) && Facility.IsDeleted == false).FirstOrDefaultAsync();
        }

        public void UpdateFacility(Facility facility)
        {
            Update(facility);
        }
    }
}
