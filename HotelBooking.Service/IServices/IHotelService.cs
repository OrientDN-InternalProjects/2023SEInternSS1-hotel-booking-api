﻿using HotelBooking.Data.DTOs.Hotel;
using HotelBooking.Data.ViewModel;

namespace HotelBooking.Service.IServices
{
    public interface IHotelService
    {
        Task<Guid> AddHotelAsync(HotelRequest model);
        Task<IEnumerable<HotelModel>> GetHotelByAddressTypeRoomDuration(FilterHotelRequest model);
        Task<Guid> AddRoomAsync(RoomRequest model);
        Task<Guid> AddExtraServiceAsync(ServiceHotelModel model);

        Task<Guid> AddFacilityAsync(FacilityModel model);
        Task<FacilityModel> GetFacilityById(Guid id);
        Task<IEnumerable<FacilityModel>> GetAllFacilities();
        Task<bool> UpdateFacilityAsync(FacilityModel model);
        Task<bool> DeleteFacilityAsync(Guid id);

        Task<bool> AddServiceAndFacilityToRoomAsync(EquipRoomRequest model);
    }
}
