using AutoMapper;
using FuzzySharp;
using HotelBooking.Common.Enums;
using HotelBooking.Common.Models;
using HotelBooking.Data.DTOs.Booking;
using HotelBooking.Data.DTOs.Hotel;
using HotelBooking.Data.Helpers;
using HotelBooking.Data.Infrastructure;
using HotelBooking.Data.Interfaces;
using HotelBooking.Data.Repositories;
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

        public async Task<IEnumerable<HotelModel>> GetAllHotel()
        {
            var result = await hotelRepository.GetAllHotels()
                                              .Include(x => x.Address)
                                              .Include(x => x.Urls).ToListAsync();
            return mapper.Map<IEnumerable<HotelModel>>(result);
        }

        public async Task<bool> UpdateHotel(HotelRequest model, Guid id)
        {
            var hotel = await hotelRepository.GetByIdAsync(id).FirstOrDefaultAsync();
            if (hotel == null) return false;
            hotel.HotelName = model.HotelName;
            model.Description = model.Description;
            model.Rating = model.Rating;
            hotel.UpdatedDate = DateTime.Now;
            var address = addressRepository.GetByCondition(x => x.Id.Equals(hotel.AddressId)).FirstOrDefault();
            address = mapper.Map(model.Address, address);
            address.UpdatedDate = DateTime.Now;
            addressRepository.Update(address);
            hotelRepository.UpdateAsync(hotel);

            var listImage = await pictureRepository.GetByCondition(x => x.HotelId.Equals(hotel.Id)).ToListAsync();
            if (listImage.Any())
            {
                foreach (var image in listImage)
                {
                    pictureRepository.DeleteImage(image);
                }
            }

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
            return true;
        }

        public async Task<bool> DeleteHotel(Guid id)
        {
            var hotel = await hotelRepository.GetByIdAsync(id).FirstOrDefaultAsync();
            if (hotel == null) return false;
            hotel.DeletedDate = DateTime.Now;
            hotel.IsDeleted = true;

            var address = await addressRepository.GetByCondition(x => x.Id.Equals(hotel.AddressId)).FirstOrDefaultAsync();
            if (address != null)
            {
                address.IsDeleted = true;
                address.DeletedDate = DateTime.Now;
            }

            var images = await pictureRepository.GetByCondition(x => x.HotelId.Equals(hotel.Id)).ToListAsync();
            if (images.Any())
            {
                foreach (var i in images)
                {
                    pictureRepository.Delete(i);
                }
            }
            await unitOfWork.SaveAsync();
            return true;
        }

        public async Task<IEnumerable<RoomVM>> GetAllRoomAvailable(Guid idHotel, DurationVM duration)
        {
            var rooms = await roomRepository.GetByCondition(x => x.HotelId.Equals(idHotel) && x.IsDeleted == false)
                                            .Include(x => x.RoomFacilities).ThenInclude(x => x.Facility)
                                            .Include(x => x.RoomServices).ThenInclude(x => x.Service)
                                            .ToListAsync();
            if (rooms.Any())
            {
                var returnRooms = new List<Room>();
                foreach (var i in rooms)
                {
                    var checkResult = await checkDurationValidationService.CheckValidationDurationForRoom(duration, i.Id);
                    if (checkResult == true)
                    {
                        returnRooms.Add(i);
                    }
                }
                return mapper.Map<IEnumerable<RoomVM>>(returnRooms);
            }
            return default;
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

        public async Task<bool> UpdateRoomEquipment(EquipRoomRequest model)
        {
            var room = await roomRepository.GetByIdAsync(model.RoomId).FirstOrDefaultAsync();
            if (room == null) return false;
            var services = await roomServiceRepository.GetByCondition(x => x.RoomId.Equals(model.RoomId)).ToListAsync();
            if (services.Any())
            {
                foreach (var i in services)
                {
                    roomServiceRepository.Delete(i);
                }
            }
            var facilities = await roomFacilityRepository.GetByCondition(x => x.RoomId.Equals(model.RoomId)).ToListAsync();
            if (facilities.Any())
            {
                foreach (var i in facilities)
                {
                    roomFacilityRepository.Delete(i);
                }
            }
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

        public async Task<bool> DeleteRoom(Guid id)
        {
            var room = await roomRepository.GetByIdAsync(id).FirstOrDefaultAsync();
            if (room == null) return false;
            room.IsDeleted = true;
            room.DeletedDate = DateTime.Now;
            var services = await roomServiceRepository.GetByCondition(x => x.RoomId.Equals(id)).ToListAsync();
            if (services.Any())
            {
                foreach (var i in services)
                {
                    roomServiceRepository.Delete(i);
                }
            }
            var facilities = await roomFacilityRepository.GetByCondition(x => x.RoomId.Equals(id)).ToListAsync();
            if (facilities.Any())
            {
                foreach (var i in facilities)
                {
                    roomFacilityRepository.Delete(i);
                }
            }
            await unitOfWork.SaveAsync();
            return true;
        }

        public async Task<Guid> AddExtraServiceAsync(ServiceHotelModel model)
        {
            var service = mapper.Map<ExtraService>(model);
            service.CreatedDate = DateTime.Now;
            serviceHotelRepository.Add(service);
            await unitOfWork.SaveAsync();
            return service.Id;
        }

        public async Task<ServiceHotelModel> GetExtraServiceById(Guid id)
        {
            var service = await serviceHotelRepository.GetByIdAsync(id);
            return mapper.Map<ServiceHotelModel>(service);
        }

        public async Task<IEnumerable<ServiceHotelModel>> GetAllExtraService()
        {
            var services = await serviceHotelRepository.GetAllAsync();
            return mapper.Map<IEnumerable<ServiceHotelModel>>(services);
        }

        public async Task<bool> UpdateExtraService(ServiceHotelModel model)
        {
            if(model.Id == null) return false;
            var service = await serviceHotelRepository.GetByIdAsync(model.Id);
            if (service == null) return false;
            service.ServicePrice = model.ServicePrice;
            service.ServiceName = model.ServiceName;
            service.UpdatedDate = DateTime.Now;
            serviceHotelRepository.UpdateAsync(service);
            await unitOfWork.SaveAsync();
            return true;
        }

        public async Task<bool> DeleteExtraService(Guid id)
        {
            var service = await serviceHotelRepository.GetByIdAsync(id);
            if (service == null) return false;
            service.DeletedDate = DateTime.Now;
            service.IsDeleted = true;
            await unitOfWork.SaveAsync();
            return true;
        }

        public async Task<Guid> AddFacilityAsync(FacilityModel model)
        {
            var facility = mapper.Map<Facility>(model);
            facility.CreatedDate = DateTime.Now;
            facilityRepository.Add(facility);
            await unitOfWork.SaveAsync();
            return facility.Id;
        }

        public async Task<FacilityModel> GetFacilityById(Guid id)
        {
            var facility = await facilityRepository.GetFacilityByIdAsync(id);
            if (facility != null)
            {
                return mapper.Map<FacilityModel>(facility);
            }
            return default;
        }

        public async Task<IEnumerable<FacilityModel>> GetAllFacilities()
        {
            var facilities = await facilityRepository.GetAllFacilityAsync();
            return mapper.Map<IEnumerable<FacilityModel>>(facilities);
        }

        public async Task<bool> UpdateFacilityAsync(FacilityModel model)
        {
            if (model.Id == null) return false;
            var facility = await facilityRepository.GetFacilityByIdAsync(model.Id);
            if (facility == null) return false;
            facility.FacilityName = model.FacilityName;
            facility.UpdatedDate = DateTime.Now;
            facilityRepository.UpdateFacility(facility);
            await unitOfWork.SaveAsync();
            return true;
        }

        public async Task<bool> DeleteFacilityAsync(Guid id)
        {
            var facility = await facilityRepository.GetFacilityByIdAsync(id);
            if (facility == null) return false;
            facility.DeletedDate = DateTime.Now;
            facility.IsDeleted = true;
            await unitOfWork.SaveAsync();
            return true;
        }

        public async Task<bool> AddServiceAndFacilityToRoomAsync(EquipRoomRequest model)
        {
            var room = await roomRepository.GetByIdAsync(model.RoomId).FirstOrDefaultAsync();
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

        public async Task<IEnumerable<HotelModel>> SearchHotel(string name, DateTime? from, DateTime? to, string city, RoomType? roomType)
        {
           
            var dataSet = hotelRepository.GetAllHotels();
            var result = await dataSet.ApplyFilterByAddress(city)
                                      .ApplyFilterByRoomType(roomType)
                                      .Include(x => x.Address)
                                      .Include(x => x.Urls)
                                      .ToListAsync();
            if (!result.Any()) return default;
            var hotels = new List<Hotel>();

            if (!string.IsNullOrEmpty(name))
            {
                foreach (var item in result)
                {
                    if (Fuzz.Ratio(name.ToLower(), item.HotelName.ToLower()) > 50)
                    {
                        hotels.Add(item);
                    }
                }
            }
            else
            {
                hotels = result;
            }

            if (from == null || to  == null) return mapper.Map<IEnumerable<HotelModel>>(hotels);
            var returnHotels = new List<Hotel>();
            var newDuration = new DurationVM { From = (DateTime) from, To = (DateTime) to };
            foreach (var hotel in hotels)
            {
                var rooms = await roomRepository.GetByCondition(x => x.HotelId.Equals(hotel.Id) && x.RoomType == roomType).ToListAsync();
                if (!rooms.Any()) break;
                foreach (var room in rooms)
                {
                    var checkResult = await checkDurationValidationService.CheckValidationDurationForRoom(newDuration, room.Id);
                    if (checkResult)
                    {
                        returnHotels.Add(hotel);
                        break;
                    }
                }
            }
            return mapper.Map<IEnumerable<HotelModel>>(returnHotels);
        }

        public async Task<HotelModel> GetHotelByIdAsync(Guid id)
        {
            var result = await hotelRepository.GetByIdAsync(id)
                                      .Include(x => x.Address)
                                      .Include(x => x.Urls)
                                      .Include(x => x.Rooms).ThenInclude(x => x.RoomFacilities).ThenInclude(x => x.Facility)
                                      .Include(x => x.Rooms).ThenInclude(x => x.RoomServices).ThenInclude(x => x.Service)
                                      .FirstOrDefaultAsync();
            if (result != null)
            {
                return mapper.Map<HotelModel>(result);
            }
            return default;
        }

        public async Task<RoomVM> GetRoomByIdAsync(Guid id)
        {
            var room = await roomRepository.GetByIdAsync(id)
                            .Include(x => x.RoomFacilities).ThenInclude(x => x.Facility)
                            .Include(x => x.RoomServices).ThenInclude(x => x.Service)
                            .FirstOrDefaultAsync();
            if (room != null)
            {
                return mapper.Map<RoomVM>(room);
            }
            return default;
        }

        public async Task<PagedList<HotelModel>> GetHotelPagedList(PagedListRequest request)
        {
            var allHotels = await hotelRepository.GetAllHotels()
                                      .Include(x => x.Address)
                                      .Include(x => x.Urls)
                                      .Include(x => x.Rooms).ThenInclude(x => x.RoomFacilities).ThenInclude(x => x.Facility)
                                      .Include(x => x.Rooms).ThenInclude(x => x.RoomServices).ThenInclude(x => x.Service)
                                      .ToListAsync();
            var returnObjects = mapper.Map<IEnumerable<HotelModel>>(allHotels);
            var hotels = PagedList<HotelModel>.ToPagedList(returnObjects.AsQueryable(), request.PageNumber, request.PageSize);
            return hotels;
        }
    }


}  

