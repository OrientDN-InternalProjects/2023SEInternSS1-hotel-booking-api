using HotelBooking.Data.Infrastructure;
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
            //var result = await GetByCondition(x => x.HotelName.ToLower().Contains(name.ToLower()) && x.IsDeleted == false)
            //.Include(hotel => hotel.Urls)
            //.Include(hotel => hotel.Rooms.Where(r => r.RoomStatus == RoomStatus.EMPTY)).ThenInclude(x => x.RoomServices)
            //.Include(hotel => hotel.Rooms.Where(r => r.RoomStatus == RoomStatus.EMPTY)).ThenInclude(x => x.RoomFacilities)
            //.Include(hotel => hotel.Address)
            //.ToListAsync();
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
                Rooms = x.Rooms.Where(x => x.RoomStatus == Common.Enums.RoomStatus.EMPTY).Select(x => new RoomVM
                {
                    RoomStatus = x.RoomStatus,
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
            //var listHotelVM = new List<HotelVM>();
            //if (result != null)
            //{

            //    foreach (var i in result)
            //    {
            //        listHotelVM.Add(
            //            new HotelVM
            //            {
            //                HotelName = i.HotelName,
            //                Rating = i.Rating,
            //                Description = i.Description,
            //                Urls = i.Urls.Select(x => x.ImageUrl).ToList()
            //            }
            //            );
            //    }

            //}

            //var result = await GetByCondition(x => x.HotelName.ToLower().Contains(name.ToLower()) && x.IsDeleted == false)
            //            .Select(x => new HotelVM()
            //            {
            //                HotelName = x.HotelName,
            //                Rating = x.Rating,
            //                Description = x.Description,
            //                Urls = x.Urls.Select(x=>x.ImageUrl).ToList(),
            //                Rooms = new RoomVM
            //                {
            //                    iD
            //                }
            //            })
            //            .ToListAsync();
            //return listHotelVM;
            //.Include(x => x.Rooms.Where(x => x.RoomStatus == RoomStatus.EMPTY).Select(y => new RoomVM
            //{


            //})).ToListAsync();
        }

        public void UpdateAsync(Hotel hotel)
        {
            Update(hotel);
        }
    }
}
