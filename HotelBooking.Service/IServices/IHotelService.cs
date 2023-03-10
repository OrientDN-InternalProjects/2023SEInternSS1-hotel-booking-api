using HotelBooking.Common.Models;
using HotelBooking.Data.DTOs.Booking;
using HotelBooking.Data.DTOs.Hotel;
using HotelBooking.Data.ViewModel;

namespace HotelBooking.Service.IServices
{
    public interface IHotelService
    {
        Task<Guid> AddHotelAsync(HotelRequest model);
        Task<IEnumerable<HotelModel>> GetAllHotel();
        Task<PagedList<HotelModel>> GetHotelPagedList(PagedListRequest request);
        Task<bool> UpdateHotel(HotelRequest model, Guid id);
        Task<bool> DeleteHotel(Guid id);

        Task<IEnumerable<HotelModel>> GetHotelByAddressTypeRoomDuration(FilterHotelRequest model);
        Task<IEnumerable<RoomVM>> GetAllRoomAvailable(Guid idHotel, DurationVM duration);
        Task<Guid> AddRoomAsync(RoomRequest model);

        Task<Guid> AddExtraServiceAsync(ServiceHotelModel model);
        Task<ServiceHotelModel> GetExtraServiceById(Guid id);
        Task<IEnumerable<ServiceHotelModel>> GetAllExtraService();
        Task<bool> UpdateExtraService(ServiceHotelModel model);
        Task<bool> DeleteExtraService(Guid id);


        Task<Guid> AddFacilityAsync(FacilityModel model);
        Task<FacilityModel> GetFacilityById(Guid id);
        Task<IEnumerable<FacilityModel>> GetAllFacilities();
        Task<bool> UpdateFacilityAsync(FacilityModel model);
        Task<bool> DeleteFacilityAsync(Guid id);

        Task<bool> AddServiceAndFacilityToRoomAsync(EquipRoomRequest model);
        Task<HotelModel> GetHotelByIdAsync(Guid id);
        Task<IEnumerable<HotelModel>> GetHotelByName(string name);
        Task<RoomVM> GetRoomByIdAsync(Guid id);
    }
}
