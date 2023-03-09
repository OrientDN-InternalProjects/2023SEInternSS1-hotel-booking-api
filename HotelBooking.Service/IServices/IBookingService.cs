using HotelBooking.Common.Base;
using HotelBooking.Data.ViewModel;

namespace HotelBooking.Service.IServices
{
    public interface IBookingService
    {
        public Task<IEnumerable<HotelModel>> SearchHotelByName(string name);
        Task<ResponseModel> AddBookingAsync(BookingVM model);
        Task<bool> CheckValidationDurationForRoom(DurationVM model, Guid roomId);
        Task<bool> UpdateBookingAsync(BookingVM model, Guid id);
        Task<bool> DeleteBookingAsync(Guid id);
    }
}
