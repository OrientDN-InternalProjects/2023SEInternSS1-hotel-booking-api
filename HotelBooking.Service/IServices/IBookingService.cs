using HotelBooking.Data.DTOs.Hotel;
using HotelBooking.Data.ViewModel;

namespace HotelBooking.Service.IServices
{
    public interface IBookingService
    {
        public Task<IEnumerable<HotelVM>> SearchHotelByName(string name);
        Task<bool> AddBookingAsync(BookingVM model);
        Task<bool> CheckValidationDurationForRoom(DurationVM model, Guid roomId);
        Task<bool> UpdateBookingAsync(BookingVM model, Guid id);
        Task<bool> DeleteBookingAsync(Guid id);
    }
}
