using HotelBooking.Data.DTOs.Hotel;

namespace HotelBooking.Service.IServices
{
    public interface IHotelService
    {
        //Task<Guid> AddAsync(CreateHotelDTO model);
        //Task<bool> UpdateAsync(UpdateHotelDTO model, string id);
        //Task<bool> DeleteAsync(Guid id);
        //Task<IEnumerable<Hotel>> GetAllAsync();
        //Task<Hotel> GetByIdAsync(Guid id);

        Task<Guid> AddHotelAsync(CreateHotelDTO model);
        Task<Guid> AddRoomAsync(CreateRoomDTO model);
        Task<Guid> AddExtraServiceAsync(CreateServiceHotelDTO model);
        Task<Guid> AddFacilityAsync(CreateFacilityDTO model);
        Task<bool> AddServiceAndFacilityToRoomAsync(EquipRoomDTO model);
    }
}
