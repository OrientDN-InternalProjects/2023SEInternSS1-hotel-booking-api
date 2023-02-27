using HotelBooking.Data.ViewModel;
using HotelBooking.Model.Entities;

namespace HotelBooking.Data.Interfaces
{
    public interface IHotelRepository
    {
        Task<IEnumerable<Hotel>> GetAllAsync();
        Task<Hotel> GetByIdAsync(Guid hotelId);
        void CreateAsync(Hotel hotel);
        void UpdateAsync(Hotel hotel);
        void DeleteAsync(Hotel hotel);
        Task<IEnumerable<HotelVM>> GetByNameAync(string name);
    }
}
