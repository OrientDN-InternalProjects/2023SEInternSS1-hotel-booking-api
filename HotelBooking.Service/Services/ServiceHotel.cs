using AutoMapper;
using HotelBooking.Data.DTOs;
using HotelBooking.Data.Infrastructure;
using HotelBooking.Data.Interfaces;
using HotelBooking.Model.Entities;
using HotelBooking.Service.IServices;

namespace HotelBooking.Service.Services
{
    public class ServiceHotel : IServiceHotel
    {
        private readonly IServiceHotelRepository serviceRepository;
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;
        public ServiceHotel(IServiceHotelRepository serviceRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            this.serviceRepository = serviceRepository;
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
        }
        public async Task<Guid> AddAsync(CreateServiceHotelDTO model)
        {
            var service = mapper.Map<ExtraService>(model);
            service.CreatedDate = DateTime.Now;
            serviceRepository.Add(service);
            await unitOfWork.SaveAsync();
            return service.Id;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var service = serviceRepository.GetByID(id);
            if (service == null || service.IsDeleted == true)
            {
                return false;
            }
            service.IsDeleted = true;
            service.DeletedDate = DateTime.Now;
            await unitOfWork.SaveAsync();
            return true;
        }

        public async Task<IEnumerable<ExtraService>> GetAllAsync()
        {
            var result = new List<ExtraService>();
            var objects = await serviceRepository.GetAllAsync();
            foreach (var i in objects)
            {
                if (i.IsDeleted == false)
                {
                    result.Add(i);
                }
            }
            return result;
        }

        public async Task<ExtraService?> GetByIdAsync(Guid id)
        {
            var result = await serviceRepository.GetByIdAsync(id);
            return result;
        }

        public async Task<bool> UpdateAsync(UpdateServiceHotelDTO model)
        {
            var result = await serviceRepository.GetByIdAsync(model.Id);
            if (result == null)
            {
                return false;
            }
            var serviceResult = mapper.Map(model, result);
            result.UpdatedDate = DateTime.Now;
            serviceRepository.UpdateAsync(result);
            await unitOfWork.SaveAsync();
            return true;
        }
    }
}
