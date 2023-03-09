using HotelBooking.Data.DTOs.Hotel;
using HotelBooking.Data.ViewModel;

namespace HotelBooking.Service.IServices
{
    public interface IHotelService
    {
        Task<Guid> AddHotelAsync(HotelRequest model);
        Task<IEnumerable<HotelModel>> GetHotelByAddressTypeRoomDuration(FilterHotelRequest model);
        Task<Guid> AddRoomAsync(RoomRequest model);

        Task<Guid> AddExtraServiceAsync(ServiceHotelModel model);
        Task<ServiceHotelModel> GetExtraServiceById(Guid id);
        Task<IEnumerable<ServiceHotelModel>> GetAllExtraService();
        Task<bool> UpdateExtraService(ServiceHotelModel model);
        Task<bool> DeleteExtraService(Guid id);


        Task<Guid> AddFacilityAsync(FacilityModel model);
        Task<bool> AddServiceAndFacilityToRoomAsync(EquipRoomRequest model);
        Task<IEnumerable<HotelModel>> GetHotelByName(string name);
    }
}
