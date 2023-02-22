using AutoMapper;
using HotelBooking.Data.DTOs;
using HotelBooking.Data.Infrastructure;
using HotelBooking.Data.Interfaces;
using HotelBooking.Model.Entities;
using HotelBooking.Service.IServices;

namespace HotelBooking.Service.Services
{
    public class HotelService : IHotelService
    {
        private readonly IHotelRepository hotelRepository;
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;
        public HotelService(IHotelRepository hotelRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            this.hotelRepository = hotelRepository;
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Guid> AddAsync(CreateHotelDTO model)
        {
            var item = mapper.Map<Hotel>(model);
            hotelRepository.CreateAsync(item);
            await unitOfWork.SaveAsync();
            return item.Id;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var hotel = await hotelRepository.GetByIdAsync(id);
            if (hotel == null || hotel.IsDeleted == true)
            {
                return false;
            }
            hotel.IsDeleted = true;
            hotel.DeletedDate = DateTime.Now;
            await unitOfWork.SaveAsync();
            return true;
        }

        public async Task<IEnumerable<Hotel>> GetAllAsync()
        {
            var result = new List<Hotel>();
            var objects = await hotelRepository.GetAllAsync();
            foreach (var i in objects)
            {
                if (i.IsDeleted == false)
                {
                    result.Add(i);
                }
            }
            return result;
        }

        public async Task<Hotel> GetByIdAsync(Guid id)
        {
            return await hotelRepository.GetByIdAsync(id);
        }

        public async Task<bool> UpdateAsync(UpdateHotelDTO model)
        {
            var result = await hotelRepository.GetByIdAsync(model.Id);
            if (result == null)
            {
                return false;
            }
            var addressResult = mapper.Map(model, result);
            result.UpdatedDate = DateTime.Now;
            hotelRepository.UpdateAsync(result);
            await unitOfWork.SaveAsync();
            return true;
        }
    }
}
