using AutoMapper;
using HotelBooking.Data.DTOs;
using HotelBooking.Data.Infrastructure;
using HotelBooking.Data.Interfaces;
using HotelBooking.Model.Entities;
using HotelBooking.Service.IServices;

namespace HotelBooking.Service.Services
{
    public class PriceQuotationService : IPriceQuotationService
    {
        private readonly IPriceQuotationRepository priceQuotationRepository;
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;
        public PriceQuotationService(IPriceQuotationRepository priceQuotationRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            this.priceQuotationRepository = priceQuotationRepository;
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Guid> AddAsync(CreatePriceQuotationDTO model)
        {
            var priceQuotation = mapper.Map<PriceQuotation>(model);
            priceQuotation.CreatedDate = DateTime.Now;
            priceQuotationRepository.Add(priceQuotation);
            await unitOfWork.SaveAsync();
            return priceQuotation.Id;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var priceQuotation = priceQuotationRepository.GetByID(id);
            if (priceQuotation == null || priceQuotation.IsDeleted == true)
            {
                return false;
            }
            priceQuotation.IsDeleted = true;
            priceQuotation.DeletedDate = DateTime.Now;
            await unitOfWork.SaveAsync();
            return true;
        }

        public async Task<IEnumerable<PriceQuotation>> GetAllAsync()
        {

            return await priceQuotationRepository.GetAllPriceQuotationAsync();
        }

        public async Task<PriceQuotation> GetByIdAsync(Guid id)
        {
            return await priceQuotationRepository.GetPriceQuotationByIdAsync(id);
        }

        public async Task<bool> UpdateAsync(UpdatePriceQuotationDTO model)
        {
            var result = await priceQuotationRepository.GetPriceQuotationByIdAsync(model.Id);
            if (result == null)
            {
                return false;
            }
            var priceQuotation = mapper.Map(model, result);
            result.UpdatedDate = DateTime.Now;
            priceQuotationRepository.UpdatePriceQuotation(result);
            await unitOfWork.SaveAsync();
            return true;
        }
    }
}
