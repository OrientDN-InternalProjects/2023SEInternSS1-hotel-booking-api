using HotelBooking.Data.DTOs.Hotel;

namespace HotelBooking.Service.IServices
{
    public interface IHotelService
    {
        Task<Guid> AddHotelAsync(HotelRequest model);
        Task<Guid> AddRoomAsync(CreateRoomDTO model);
        Task<Guid> AddExtraServiceAsync(CreateServiceHotelDTO model);
        Task<Guid> AddFacilityAsync(FacilityRequest model);
        Task<bool> AddServiceAndFacilityToRoomAsync(EquipRoomRequest model);
    }
}
