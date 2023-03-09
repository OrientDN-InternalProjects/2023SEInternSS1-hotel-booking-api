using HotelBooking.Data.ViewModel;

namespace HotelBooking.Service.IServices
{
    public interface ICheckDurationValidationService
    {
        Task<bool> CheckValidationDurationForRoom(DurationVM newDuration, Guid roomId);
    }
}
