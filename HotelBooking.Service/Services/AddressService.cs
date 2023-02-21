using AutoMapper;
using HotelBooking.Data.DTOs;
using HotelBooking.Data.Infrastructure;
using HotelBooking.Data.Interfaces;
using HotelBooking.Model.Entities;
using HotelBooking.Service.IServices;

namespace HotelBooking.Service.Services
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository addressRepository;
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;
        public AddressService(IAddressRepository addressRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            this.addressRepository = addressRepository;
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
        }
        public async Task<Guid> AddAsync(CreateAddressDTO model)
        {
            var address = mapper.Map<Address>(model);
            address.CreatedDate = DateTime.Now;
            addressRepository.Add(address);
            await unitOfWork.SaveAsync();
            return address.Id;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var address = addressRepository.GetByID(id);
            if (address == null || address.IsDeleted == true)
            {
                return false;
            }
            address.IsDeleted = true;
            address.DeletedDate = DateTime.Now;
            await unitOfWork.SaveAsync();
            return true;
        }

        public async Task<IEnumerable<Address>> GetAllAsync()
        {
            var result = new List<Address>();
            var objects = await addressRepository.GetAllAsync();
            foreach (var i in objects)
            {
                if (i.IsDeleted == false)
                {
                    result.Add(i);
                }
            }
            return result;
        }

        public async Task<Address> GetByIdAsync(Guid id)
        {
            var result = await addressRepository.GetByIdAsync(id);
            return result;
        }

        public async Task<bool> UpdateAsync(UpdateAddressDTO model)
        {
            var result = await addressRepository.GetByIdAsync(model.Id);
            if (result == null)
            {
                return false;
            }
            var addressResult = mapper.Map(model, result);
            result.UpdatedDate = DateTime.Now;
            addressRepository.UpdateAsync(result);
            await unitOfWork.SaveAsync();
            return true;
        }
    }
}
