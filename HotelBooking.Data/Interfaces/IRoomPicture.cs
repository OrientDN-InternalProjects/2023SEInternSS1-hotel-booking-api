using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelBooking.Data.Infrastructure;
using HotelBooking.Model.Entities;

namespace HotelBooking.Data.Interfaces
{
    public interface IRoomPicture : IGenericRepository<RoomImage>
    {
        Task<IEnumerable<RoomImage>> GetAllRoomImagesAsync();
        Task<RoomImage> GetRoomImageByIdAsync(Guid RoomImageId);
        void CreateRoomImage(RoomImage roomImage);
        void UpdateRoomImage(RoomImage roomImage);
        void DeleteRoomImage(RoomImage roomImage);
    }
}
