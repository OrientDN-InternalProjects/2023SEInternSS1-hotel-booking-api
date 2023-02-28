using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelBooking.Data.Infrastructure;
using HotelBooking.Data.ViewModel;
using HotelBooking.Model.Entities;

namespace HotelBooking.Data.Interfaces
{
    public interface IBookedRoom : IGenericRepository<BookedRoom>
    {
        Task<IEnumerable<BookedRoom>> GetAllAsync();
        Task<BookedRoom> GetByIdAsync(Guid id);
        void CreateAsync(BookedRoom model);
        void UpdateAsync(BookedRoom model);
        void DeleteAsync(BookedRoom model);
        Task<bool> CheckValidation (DurationVM newDuration,Guid roomId);
    }
}
