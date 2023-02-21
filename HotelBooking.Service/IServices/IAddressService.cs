using HotelBooking.Data.DTOs;
using HotelBooking.Model.Entities;

namespace HotelBooking.Service.IServices
{
    public interface IAddressService
    {
        Task<Guid> AddAsync(CreateAddressDTO model);
        Task<bool> UpdateAsync(UpdateAddressDTO model);
        Task<bool> DeleteAsync(Guid id);
        Task<IEnumerable<Address>> GetAllAsync();
        Task<Address> GetByIdAsync(Guid id);
    }
}
