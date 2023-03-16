using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using HotelBooking.Data.Infrastructure;
using HotelBooking.Data.Interfaces;
using HotelBooking.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Data.Repositories
{


    public class RoomPicture : GenericRepository<RoomImage>,IRoomPicture 
    {
        public RoomPicture(BookingDbContext context) : base(context)
        {
        }

        public void CreateRoomImage(RoomImage roomImage)
        {
            Add(roomImage);
        }

        public void DeleteRoomImage(RoomImage roomImage)
        {
            Delete(roomImage);
        }

        public async Task<IEnumerable<RoomImage>> GetAllRoomImagesAsync()
        {
            return await GetAll().OrderBy(image => image.Id).Where(image => image.IsDeleted == false).ToListAsync();
        }

        public async Task<RoomImage> GetRoomImageByIdAsync(Guid RoomImageId)
        {
            return await GetByCondition(image => image.Id.Equals(RoomImageId) && image.IsDeleted == false).FirstOrDefaultAsync();
        }

        public void UpdateRoomImage(RoomImage roomImage)
        {
            Update(roomImage);
        }
    }
}
