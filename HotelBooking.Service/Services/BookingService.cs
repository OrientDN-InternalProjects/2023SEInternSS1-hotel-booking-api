using HotelBooking.Data.Interfaces;
using HotelBooking.Data.ViewModel;
using HotelBooking.Service.IServices;

namespace HotelBooking.Service.Services
{
    public class BookingService : IBookingService
    {
        private readonly IHotelRepository hotelRepository;
        public BookingService(IHotelRepository hotelRepository)
        {
            this.hotelRepository = hotelRepository;
        }

        public async Task<IEnumerable<HotelVM>> SearchHotelByName(string name)
        {
            var result = await hotelRepository.GetByNameAync(name);

            return result;
        }
    }
}
