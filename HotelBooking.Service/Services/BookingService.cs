using System.Net;
using AutoMapper;
using CloudinaryDotNet;
using HotelBooking.Data.Extensions;
using HotelBooking.Data.Infrastructure;
using HotelBooking.Data.Interfaces;
using HotelBooking.Data.ViewModel;
using HotelBooking.Model.Entities;
using HotelBooking.Service.IServices;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Service.Services
{
    public class BookingService : IBookingService
    {
        private readonly IHotelRepository hotelRepository;
        private readonly IBookingRepository bookingRepository;
        private readonly IBookedRoom bookedRoomRepository;
        private readonly IRoomRepository roomRepository;
        private readonly IRoomService roomServiceRepository;
        private readonly IMailSender mailSender;
        private readonly IPriceQuotationRepository priceQuotation;

        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;
        public BookingService(IHotelRepository hotelRepository,
            IBookedRoom bookedRoomRepository, IBookingRepository bookingRepository,
            IRoomRepository roomRepository, IMapper mapper, IUnitOfWork unitOfWork,
            IRoomService roomServiceRepository, IMailSender mailSender, IPriceQuotationRepository priceQuotation)
        {
            this.hotelRepository = hotelRepository;
            this.bookedRoomRepository = bookedRoomRepository;
            this.bookingRepository = bookingRepository;
            this.roomRepository = roomRepository;
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.roomServiceRepository = roomServiceRepository;
            this.mailSender = mailSender;
            this.priceQuotation = priceQuotation;
        }

        public async Task<bool> AddBookingAsync(BookingVM model)
        {
            var booking = new Booking()
            {
                PaymentStatus = model.PaymentStatus,
                From = model.Duration.From,
                To = model.Duration.To,
                Email = model.Email,
                Amount = CalculateFee(model.RoomIds)
            };
            if(model.UserId!= null)
            {
                booking.UserId = model.UserId;
            }
            bookingRepository.CreateAsync(booking);
            foreach (var i in model.RoomIds)
            {
                var bookedRoom = new BookedRoom()
                {
                    Room = await roomRepository.GetByIdAsync(i),
                    Booking = booking,
                };
                bookedRoomRepository.CreateAsync(bookedRoom);
            }
            await unitOfWork.SaveAsync();
            var responseEmail = new InforBookingResponse
            {
                Id = booking.Id,
                Amount = booking.Amount,
                From = booking.From,
                To = booking.To,
                Rooms = booking.BookedRooms.Select(x=> new RoomVM
                {
                    RoomType = x.Room.RoomType,
                    Description = x.Room.Description
                }).ToList()
            };
            await mailSender.SendInforOfBooking(model.Email, responseEmail);
            return true;
        }

        public async Task<bool> CheckValidationDurationForRoom(DurationVM model, Guid roomId)
        {
            return await bookedRoomRepository.CheckValidation(model, roomId);
        }

        public async Task<bool> DeleteBookingAsync(Guid Id)
        {
            var res = await bookingRepository.GetByIdAsync(Id);

            if (res == null) return false;

            res.DeletedDate = DateTime.Now;
            res.IsDeleted= true;
            await unitOfWork.SaveAsync();

            return true;
        }

        public async Task<IEnumerable<HotelVM>> SearchHotelByName(string name)
        {
            var result = await hotelRepository.GetByNameAync(name);
            return result;
        }

        public async Task<bool> UpdateBookingAsync(BookingVM model,Guid Id)
        {
            var res = await bookingRepository.GetByIdAsync(Id);
            if(res == null) return false;
            var booking = mapper.Map<Booking>(res);
            booking.Amount = CalculateFee(model.RoomIds);
            booking.From = model.Duration.From;
            booking.To = model.Duration.To;
            booking.UpdatedDate = DateTime.Now;
            bookingRepository.Update(booking);

            var booked_room = await bookedRoomRepository.GetByCondition(x=>x.BookingId.Equals(Id)).ToListAsync();
            foreach(var i in booked_room)
            {
                bookedRoomRepository.Delete(i);
            }
            
            foreach (var i in model.RoomIds)
            {
                var bookedRoom = new BookedRoom()
                {
                    Room = await roomRepository.GetByIdAsync(i),
                    Booking = booking,
                };
                bookedRoomRepository.CreateAsync(bookedRoom);
            }
            await unitOfWork.SaveAsync();
            return true;
        }

        private double CalculateFee(IEnumerable<Guid> roomIds)
        {
            double sum = 0;
            
            foreach (var roomId in roomIds)
            {
                var res = roomServiceRepository.GetByCondition(x => x.IsDeleted == false && x.RoomId == roomId).Select(x => x.Service).ToList().Sum(x => x.ServicePrice);
                var room_price = roomRepository.GetByCondition(x => x.IsDeleted== false && x.Id== roomId).Select(x => x.Price.Price).FirstOrDefault();
                sum += res + room_price;
            }

            return sum;
        }
    }
}
