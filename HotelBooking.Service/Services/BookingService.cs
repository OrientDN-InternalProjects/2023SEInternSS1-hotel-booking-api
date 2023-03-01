using AutoMapper;
using HotelBooking.Data.Infrastructure;
using HotelBooking.Data.Interfaces;
using HotelBooking.Data.ViewModel;
using HotelBooking.Model.Entities;
using HotelBooking.Service.IServices;

namespace HotelBooking.Service.Services
{
    public class BookingService : IBookingService
    {
        private readonly IHotelRepository hotelRepository;
        private readonly IBookingRepository bookingRepository;
        private readonly IBookedRoom bookedRoomRepository;
        private readonly IRoomRepository roomRepository;

        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;
        public BookingService(IHotelRepository hotelRepository,
            IBookedRoom bookedRoomRepository, IBookingRepository bookingRepository,
            IRoomRepository roomRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            this.hotelRepository = hotelRepository;
            this.bookedRoomRepository = bookedRoomRepository;
            this.bookingRepository = bookingRepository;
            this.roomRepository = roomRepository;
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
        }

        public async Task<bool> AddBookingAsync(BookingVM model)
        {
            var booki = new Booking()
            {
                PaymentStatus = model.PaymentStatus,
                From = model.Duration.From,
                To = model.Duration.To,

            };
            bookingRepository.CreateAsync(booki);
            foreach (var i in model.RoomIds)
            {
                var bookedRoom = new BookedRoom()
                {
                    Room = await roomRepository.GetByIdAsync(new Guid(i)),
                    Booking = booki,
                };
                bookedRoomRepository.CreateAsync(bookedRoom);
                await unitOfWork.SaveAsync();
            }
            return true;
        }

        public async Task<bool> CheckValidationDurationForRoom(DurationVM model, string roomId)
        {
            return await bookedRoomRepository.CheckValidation(model, new Guid(roomId));
        }

        public async Task<IEnumerable<HotelVM>> SearchHotelByName(string name)
        {
            var result = await hotelRepository.GetByNameAync(name);
            return result;
        }

    }
}
