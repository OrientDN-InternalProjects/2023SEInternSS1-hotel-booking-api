using AutoMapper;
using HotelBooking.Common.Base;
using HotelBooking.Common.Models;
using HotelBooking.Data.DTOs.Booking;
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
        private readonly IAccountRepository accountRepository;
        private readonly ICheckDurationValidationService checkDurationValidationService;

        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;
        public BookingService(IHotelRepository hotelRepository,
            IBookedRoom bookedRoomRepository, IBookingRepository bookingRepository,
            IRoomRepository roomRepository, IMapper mapper, IUnitOfWork unitOfWork,
            IRoomService roomServiceRepository, IMailSender mailSender,
            IAccountRepository accountRepository, ICheckDurationValidationService checkDurationValidationService)
        {
            this.hotelRepository = hotelRepository;
            this.bookedRoomRepository = bookedRoomRepository;
            this.bookingRepository = bookingRepository;
            this.roomRepository = roomRepository;
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.roomServiceRepository = roomServiceRepository;
            this.mailSender = mailSender;
            this.accountRepository = accountRepository;
            this.checkDurationValidationService = checkDurationValidationService;
        }

        public async Task<ResponseModel> AddBookingAsync(BookingRequest model)
        {
            if (DateTime.Compare(model.Duration.From, model.Duration.To) > 0)
                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    IsSuccess = false,
                    Message = "Duration you chosen is not valid"
                };
            var booking = new Booking()
            {
                PaymentStatus = false,
                From = model.Duration.From,
                To = model.Duration.To,
                Email = model.Email,
                Amount = CalculateFee(model.RoomIds)
            };
            if (model.UserId != null)
            {
                booking.UserId = model.UserId;
            }
            bookingRepository.CreateAsync(booking);
            foreach (var i in model.RoomIds)
            {
                var checkValidation = await checkDurationValidationService.CheckValidationDurationForRoom(model.Duration, i);
                if (!checkValidation)
                {
                    return new ResponseModel
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        IsSuccess = false,
                        Message = "You can't book at this duration"
                    };
                }
                var room = await roomRepository.GetByIdAsync(i).FirstOrDefaultAsync();
                if (room == null)
                    return new ResponseModel
                    {
                        StatusCode = System.Net.HttpStatusCode.NotFound,
                        IsSuccess = false,
                    };
                var bookedRoom = new BookedRoom()
                {
                    Room = room,
                    Booking = booking,
                    From = booking.From,
                    To = booking.To
                };
                bookedRoomRepository.CreateAsync(bookedRoom);
            }
            await unitOfWork.SaveAsync();

            //await mailSender.SendInforOfBooking(model.Email, booking.Id.ToString());
            return new ResponseModel
            {
                StatusCode = System.Net.HttpStatusCode.Created,
                IsSuccess = true,
                Data = new
                {
                    Guid = booking.Id,
                }
            };
        }

        public async Task<bool> DeleteBookingAsync(Guid Id)
        {
            var res = await bookingRepository.GetByIdAsync(Id).FirstOrDefaultAsync();

            if (res == null) return false;

            res.DeletedDate = DateTime.Now;
            res.IsDeleted = true;
            await unitOfWork.SaveAsync();

            return true;
        }

        public async Task<bool> UpdateBookingAsync(BookingRequest model, Guid Id)
        {
            var res = await bookingRepository.GetByIdAsync(Id).FirstOrDefaultAsync();
            if (res == null) return false;
            var booking = mapper.Map<Booking>(res);
            booking.Amount = CalculateFee(model.RoomIds);
            booking.From = model.Duration.From;
            booking.To = model.Duration.To;
            booking.UpdatedDate = DateTime.Now;
            bookingRepository.Update(booking);

            var booked_room = await bookedRoomRepository.GetByCondition(x => x.BookingId.Equals(Id)).ToListAsync();
            foreach (var i in booked_room)
            {
                bookedRoomRepository.Delete(i);
            }

            foreach (var i in model.RoomIds)
            {
                var bookedRoom = new BookedRoom()
                {
                    Room = await roomRepository.GetByIdAsync(i).FirstOrDefaultAsync(),
                    Booking = booking,
                    From = booking.From,
                    To = booking.To
                };
                bookedRoomRepository.CreateAsync(bookedRoom);
            }
            await unitOfWork.SaveAsync();
            return true;
        }

        public async Task<IEnumerable<BookingResponse>> GetAllBookingsByUser(string email)
        {
            var bookings = await bookingRepository.GetByUserEmail(email)
                           .Include(x => x.BookedRooms).ThenInclude(x => x.Room).ThenInclude(x => x.RoomFacilities).ThenInclude(x => x.Facility)
                           .Include(x => x.BookedRooms).ThenInclude(x => x.Room).ThenInclude(x => x.RoomServices).ThenInclude(x => x.Service)
                           .ToListAsync();
            if (bookings.Any())
            {
                return mapper.Map<IEnumerable<BookingResponse>>(bookings);
            }
            return default;
        }

        public async Task<BookingResponse> GetBookingById(Guid id)
        {
            var booking = await bookingRepository.GetByIdAsync(id)
                                .Include(x => x.BookedRooms).ThenInclude(x => x.Room).ThenInclude(x => x.RoomFacilities).ThenInclude(x => x.Facility)
                                .Include(x => x.BookedRooms).ThenInclude(x => x.Room).ThenInclude(x => x.RoomServices).ThenInclude(x => x.Service)
                                .FirstOrDefaultAsync();
            if (booking != null)
            {
                var res = mapper.Map<BookingResponse>(booking);
                if (booking.UserId != null)
                {
                    var customer = await accountRepository.GetUserById(booking.UserId);
                    res.User = mapper.Map<UserModel>(customer);
                }
                return res;
            }
            return default;
        }

        public async Task<PagedList<BookingResponse>> GetBookingPagedList(PagedListRequest request)
        {
            var allBookings = await bookingRepository.GetAllBookings()
                           .Include(x => x.BookedRooms).ThenInclude(x => x.Room).ThenInclude(x => x.RoomFacilities).ThenInclude(x => x.Facility)
                           .Include(x => x.BookedRooms).ThenInclude(x => x.Room).ThenInclude(x => x.RoomServices).ThenInclude(x => x.Service)
                           .ToListAsync();
            var returnObjects = mapper.Map<IEnumerable<BookingResponse>>(allBookings);
            var bookings = PagedList<BookingResponse>.ToPagedList(returnObjects.AsQueryable(), request.PageNumber, request.PageSize);
            return bookings;
        }

        private double CalculateFee(IEnumerable<Guid> roomIds)
        {
            double sum = 0;

            foreach (var roomId in roomIds)
            {
                var res = roomServiceRepository.GetByCondition(x => x.IsDeleted == false && x.RoomId == roomId).Select(x => x.Service).ToList().Sum(x => x.ServicePrice);
                var room_price = roomRepository.GetByCondition(x => x.IsDeleted == false && x.Id == roomId).Select(x => x.Price.Price).FirstOrDefault();
                sum += res + room_price;
            }

            return sum;
        }
    }
}
