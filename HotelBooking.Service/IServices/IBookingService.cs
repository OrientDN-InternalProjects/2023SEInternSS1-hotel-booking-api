using HotelBooking.Common.Base;
using HotelBooking.Data.DTOs.Booking;
using HotelBooking.Data.ViewModel;

namespace HotelBooking.Service.IServices
{
    public interface IBookingService
    {
        Task<ResponseModel> AddBookingAsync(BookingRequest model);
        Task<bool> UpdateBookingAsync(BookingRequest model, Guid id);
        Task<bool> DeleteBookingAsync(Guid id);
        Task<BookingResponse> GetBookingById(Guid id);
        Task<IEnumerable<BookingResponse>> GetAllBookingsByUser(string email);
    }
}
