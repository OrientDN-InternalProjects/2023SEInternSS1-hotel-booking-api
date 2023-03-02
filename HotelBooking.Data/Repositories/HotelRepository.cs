﻿using HotelBooking.Data.Infrastructure;
using HotelBooking.Data.Interfaces;
using HotelBooking.Data.ViewModel;
using HotelBooking.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Data.Repositories
{
    public class HotelRepository : GenericRepository<Hotel>, IHotelRepository
    {
        public HotelRepository(BookingDbContext context) : base(context)
        {
        }
        public void CreateAsync(Hotel hotel)
        {
            Add(hotel);
        }

        public void DeleteAsync(Hotel hotel)
        {
            Delete(hotel);
        }

        public async Task<IEnumerable<Hotel>> GetAllAsync()
        {
            return await GetAll().OrderBy(x => x.Id).Where(x => x.IsDeleted == false).ToListAsync();
        }

        public async Task<Hotel> GetByIdAsync(Guid hotelId)
        {
            return await GetByCondition(x => x.Id.Equals(hotelId) && x.IsDeleted == false).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<HotelVM>> GetByNameAync(string name)
        {
            var result = await GetByCondition(x => x.HotelName.ToLower().Contains(name.ToLower()) && x.IsDeleted == false).Select(x => new HotelVM
            {
                HotelName = x.HotelName,
                Description = x.Description,
                Rating = x.Rating,
                Id = x.Id,
                Address = new AddressVM()
                {
                    Building = x.Address.Building,
                    City = x.Address.City,
                    District = x.Address.District,
                    StreetNumber = x.Address.StreetNumber,
                    PinCode = x.Address.PinCode
                },
                Urls = x.Urls.Select(x => x.ImageUrl).ToList(),
                Rooms = x.Rooms.Select(x => new RoomVM
                {
                    RoomType = x.RoomType,
                    Description = x.Description,
                    ExtraServices = x.RoomServices.Select(x => new ServiceVM
                    {
                        ServiceName = x.Service.ServiceName,
                        ServicePrice = x.Service.ServicePrice
                    }).ToList(),
                    Facilities = x.RoomFacilities.Select(x => new FacilityVM
                    {
                        FacilityName = x.Facility.FacilityName
                    }).ToList(),


                }).ToList()

            }).ToListAsync();
            return result;
        }

        public void UpdateAsync(Hotel hotel)
        {
            Update(hotel);
        }
    }
}
