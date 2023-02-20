using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        Task<Address?> GetByIdAsync(Guid id);
    }
}
