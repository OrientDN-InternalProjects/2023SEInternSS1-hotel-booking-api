using AutoMapper;
using HotelBooking.Data.DTOs;
using HotelBooking.Data.Infrastructure;
using HotelBooking.Data.Interfaces;
using HotelBooking.Model.Entities;
using HotelBooking.Service.IServices;

namespace HotelBooking.Service.Services
{
    public class DiscontService : IDiscountService
    {
        private readonly IDiscountRepository dicountRepository;
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;
        public DiscontService(IDiscountRepository dicountRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            this.dicountRepository = dicountRepository;
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
        }
        public async Task<Guid> AddAsync(CreateDiscountDTO model)
        {
            var discount = mapper.Map<Discount>(model);
            discount.CreatedDate = DateTime.Now;
            dicountRepository.Add(discount);
            await unitOfWork.SaveAsync();
            return discount.Id;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var discount = dicountRepository.GetByID(id);
            if (discount == null || discount.IsDeleted == true)
            {
                return false;
            }
            discount.IsDeleted = true;
            discount.DeletedDate = DateTime.Now;
            await unitOfWork.SaveAsync();
            return true;
        }

        public async Task<IEnumerable<Discount>> GetAllAsync()
        {
            var result = new List<Discount>();
            var objects = await dicountRepository.GetAllDiscountAsync();
            foreach (var i in objects)
            {
                if (i.IsDeleted == false)
                {
                    result.Add(i);
                }
            }
            return result;
        }

        public async Task<Discount?> GetByIdAsync(Guid id)
        {
            var result = await dicountRepository.GetDiscountByIdAsync(id);
            return result;
        }

        public async Task<bool> UpdateAsync(UpdateDiscountDTO model)
        {
            var result = await dicountRepository.GetDiscountByIdAsync(model.Id);
            if (result == null)
            {
                return false;
            }
            var discountResult = mapper.Map(model, result);
            result.UpdatedDate = DateTime.Now;
            dicountRepository.UpdateDiscount(result);
            await unitOfWork.SaveAsync();
            return true;
        }
    }
}
