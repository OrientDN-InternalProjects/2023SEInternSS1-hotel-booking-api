using HotelBooking.Data.ViewModel;

namespace HotelBooking.Service.IServices
{
    public interface IBookingService
    {
        public Task<IEnumerable<HotelVM>> SearchHotelByName(string name);
    }
}
