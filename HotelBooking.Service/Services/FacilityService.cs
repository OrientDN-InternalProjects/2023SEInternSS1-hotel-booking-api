using AutoMapper;
using HotelBooking.Data.DTOs;
using HotelBooking.Data.Infrastructure;
using HotelBooking.Data.Interfaces;
using HotelBooking.Model.Entities;
using HotelBooking.Service.IServices;

namespace HotelBooking.Service.Services
{
    public class FacilityService : IFacilityService
    {
        private readonly IFacilityRepository facilityRepository;
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;
        public FacilityService(IFacilityRepository facilityRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            this.facilityRepository = facilityRepository;
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
        }
        public async Task<Guid> AddAsync(CreateFacilityDTO model)
        {
            var facility = mapper.Map<Facility>(model);
            facility.CreatedDate = DateTime.Now;
            facilityRepository.Add(facility);
            await unitOfWork.SaveAsync();
            return facility.Id;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var facility = facilityRepository.GetByID(id);
            if (facility == null || facility.IsDeleted == true)
            {
                return false;
            }
            facility.IsDeleted = true;
            facility.DeletedDate = DateTime.Now;
            await unitOfWork.SaveAsync();
            return true;
        }

        public async Task<IEnumerable<Facility>> GetAllAsync()
        {
            var result = new List<Facility>();
            var objects = await facilityRepository.GetAllFacilityAsync();
            foreach (var i in objects)
            {
                if (i.IsDeleted == false)
                {
                    result.Add(i);
                }
            }
            return result;
        }

        public async Task<Facility?> GetByIdAsync(Guid id)
        {
            var result = await facilityRepository.GetFacilityByIdAsync(id);
            return result;
        }

        public async Task<bool> UpdateAsync(UpdateFacilityDTO model)
        {
            var result = await facilityRepository.GetFacilityByIdAsync(model.Id);
            if (result == null)
            {
                return false;
            }
            var facilityResult = mapper.Map(model, result);
            result.UpdatedDate = DateTime.Now;
            facilityRepository.UpdateFacility(result);
            await unitOfWork.SaveAsync();
            return true;
        }
    }
}
