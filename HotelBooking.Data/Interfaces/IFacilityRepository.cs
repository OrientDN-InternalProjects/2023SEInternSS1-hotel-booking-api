using HotelBooking.Data.Infrastructure;
using HotelBooking.Model.Entities;

namespace HotelBooking.Data.Interfaces
{
    public interface IFacilityRepository : IGenericRepository<Facility>
    {
        Task<IEnumerable<Facility>> GetAllFacilityAsync();
        Task<Facility?> GetFacilityByIdAsync(Guid? FacilityId);
        void CreateFacility(Facility facility);
        void UpdateFacility(Facility facility);
        void DeleteFacility(Facility facility);
    }
}
