using HotelBooking.Data.DTOs.Hotel;

namespace HotelBooking.Service.IServices
{
    public interface IHotelService
    {
        Task<Guid> AddHotelAsync(HotelRequest model);
        Task<Guid> AddRoomAsync(RoomRequest model);
        Task<Guid> AddExtraServiceAsync(ServiceHotelRequest model);
        Task<Guid> AddFacilityAsync(FacilityRequest model);
        Task<bool> AddServiceAndFacilityToRoomAsync(EquipRoomRequest model);
    }
}
