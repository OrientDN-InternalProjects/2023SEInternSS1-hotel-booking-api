using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelBooking.Data.Infrastructure;
using HotelBooking.Model.Entities;

namespace HotelBooking.Data.Interfaces
{
    public interface IBookingRepository : IGenericRepository<Booking>
    {
        Task<IEnumerable<Booking>> GetAllAsync();
        Task<Booking> GetByIdAsync(Guid id);
        void CreateAsync(Booking booking);
        void UpdateAsync(Booking booking);
        void DeleteAsync(Booking booking);
    }
}
