using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelBooking.Data.Infrastructure;
using HotelBooking.Model.Entities;

namespace HotelBooking.Data.Interfaces
{
    public interface IPictureRepository : IGenericRepository<Image>
    {
        Task<IEnumerable<Image>> GetAllImagesAsync();
        Task<Image> GetImageByIdAsync(Guid imageId);
        void CreateImage(Image image);
        void UpdateImage(Image image);
        void DeleteImage(Image image);
    }
}
