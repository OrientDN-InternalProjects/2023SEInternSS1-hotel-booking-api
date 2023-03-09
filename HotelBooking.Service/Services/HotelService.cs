using AutoMapper;
using HotelBooking.Data.DTOs.Hotel;
using HotelBooking.Data.Helpers;
using HotelBooking.Data.Infrastructure;
using HotelBooking.Data.Interfaces;
using HotelBooking.Data.ViewModel;
using HotelBooking.Model.Entities;
using HotelBooking.Service.IServices;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Service.Services
{
    public class HotelService : IHotelService
    {
        private readonly IHotelRepository hotelRepository;
        private readonly IAddressRepository addressRepository;
        private readonly IPictureRepository pictureRepository;
        private readonly IPictureService pictureService;
        private readonly ICheckDurationValidationService checkDurationValidationService;

        private readonly IPriceQuotationRepository priceQuotationRepository;
        private readonly IServiceHotelRepository serviceHotelRepository;
        private readonly IFacilityRepository facilityRepository;
        private readonly IRoomRepository roomRepository;
        private readonly IRoomService roomServiceRepository;
        private readonly IRoomFacility roomFacilityRepository;

        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;
        public HotelService(IHotelRepository hotelRepository, IAddressRepository addressRepository,
            IPictureRepository pictureRepository, IPictureService pictureService,
            IPriceQuotationRepository priceQuotationRepository, IServiceHotelRepository serviceHotelRepository,
            IFacilityRepository facilityRepository, IRoomRepository roomRepository,
            IRoomService roomServiceRepository, IRoomFacility roomFacilityRepository,
            IMapper mapper, IUnitOfWork unitOfWork, ICheckDurationValidationService checkDurationValidationService)
        {
            this.hotelRepository = hotelRepository;
            this.addressRepository = addressRepository;
            this.pictureRepository = pictureRepository;
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.pictureService = pictureService;
            this.priceQuotationRepository = priceQuotationRepository;
            this.serviceHotelRepository = serviceHotelRepository;
            this.facilityRepository = facilityRepository;
            this.roomRepository = roomRepository;
            this.roomFacilityRepository = roomFacilityRepository;
            this.roomServiceRepository = roomServiceRepository;
            this.checkDurationValidationService = checkDurationValidationService;
        }

        public async Task<Guid> AddHotelAsync(HotelRequest model)
        {
            var address = mapper.Map<Address>(model.Address);
            address.CreatedDate = DateTime.Now;
            addressRepository.CreateAsync(address);
            var hotel = new Hotel
            {
                HotelName = model.HotelName,
                Rating = model.Rating,
                Description = model.Description,
                Address = address,
                CreatedDate = DateTime.Now,
            };
            hotelRepository.CreateAsync(hotel);
            if (model.Images != null)
            {
                foreach (var i in model.Images)
                {
                    var url = await pictureService.UploadImageAsync(i);
                    var image = new Image
                    {
                        ImageUrl = url,
                        Description = "Lorem ipsum",
                        CreatedDate = DateTime.Now,
                        Hotel = hotel
                    };
                    pictureRepository.CreateImage(image);
                }
            }
            await unitOfWork.SaveAsync();
            return hotel.Id;
        }
        public async Task<Guid> AddRoomAsync(RoomRequest model)
        {
            var priceQuotation = mapper.Map<PriceQuotation>(model.Price);
            priceQuotation.CreatedDate = DateTime.Now;
            priceQuotationRepository.CreatePriceQuotation(priceQuotation);
            var room = mapper.Map<Room>(model);
            room.CreatedDate = DateTime.Now;
            roomRepository.CreateAsync(room);
            await unitOfWork.SaveAsync();
            return room.Id;
        }

        public async Task<Guid> AddExtraServiceAsync(ServiceHotelModel model)
        {
            var service = mapper.Map<ExtraService>(model);
            service.CreatedDate = DateTime.Now;
            serviceHotelRepository.Add(service);
            await unitOfWork.SaveAsync();
            return service.Id;
        }

        public async Task<Guid> AddFacilityAsync(FacilityModel model)
        {
            var facility = mapper.Map<Facility>(model);
            facility.CreatedDate = DateTime.Now;
            facilityRepository.Add(facility);
            await unitOfWork.SaveAsync();
            return facility.Id;
        }
        public async Task<bool> AddServiceAndFacilityToRoomAsync(EquipRoomRequest model)
        {
            var room = await roomRepository.GetByIdAsync(model.RoomId);
            if (room == null) return false;
            if (model.FacilityIds != null)
            {
                foreach (var i in model.FacilityIds)
                {
                    var item = await facilityRepository.GetFacilityByIdAsync(new Guid(i));
                    if (item != null)
                    {
                        var roomFacility = new RoomFacility()
                        {
                            Room = room,
                            Facility = item
                        };
                        roomFacilityRepository.CreateAsync(roomFacility);
                    }
                }
            }
            if (model.ServiceIds != null)
            {
                foreach (var i in model.ServiceIds)
                {
                    var item = await serviceHotelRepository.GetByIdAsync(new Guid(i));
                    if (item != null)
                    {
                        var roomService = new RoomService()
                        {
                            Room = room,
                            Service = item
                        };
                        roomServiceRepository.CreateAsync(roomService);
                    }
                }
            }
            await unitOfWork.SaveAsync();
            return true;
        }

        public async Task<IEnumerable<HotelModel>> GetHotelByAddressTypeRoomDuration(FilterHotelRequest model)
        {
            var dataSet = hotelRepository.GetAllHotels();
            var result = await dataSet.ApplyFilterByAddress(model.City)
                                      .ApplyFilterByRoomType(model.RoomType)
                                      .Include(x => x.Address)
                                      .Include(x => x.Urls)
                                      .Include(x => x.Rooms).ThenInclude(x => x.RoomFacilities).ThenInclude(x => x.Facility)
                                      .Include(x => x.Rooms).ThenInclude(x => x.RoomServices).ThenInclude(x => x.Service)
                                      .ToListAsync();
            if (!result.Any()) return default;
            var hotels = new List<Hotel>();
            foreach (var hotel in result)
            {
                var rooms = await roomRepository.GetByCondition(x => x.HotelId.Equals(hotel.Id) && x.RoomType == model.RoomType).ToListAsync();
                if (!rooms.Any()) break;
                foreach (var room in rooms)
                {
                    var checkResult = await checkDurationValidationService.CheckValidationDurationForRoom(model.Duration, room.Id);
                    if (checkResult)
                    {
                        hotels.Add(hotel);
                        break;
                    }
                }
            }
            return mapper.Map<IEnumerable<HotelModel>>(hotels);
        }

    }
}
