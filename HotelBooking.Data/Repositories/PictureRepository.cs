using HotelBooking.Data.Infrastructure;
using HotelBooking.Data.Interfaces;
using HotelBooking.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Data.Repositories
{
    public class PictureRepository : GenericRepository<Image>, IPictureRepository
    {
        public PictureRepository(BookingDbContext context) : base(context)
        {
        }
        public void CreateImage(Image image)
        {
            Add(image);
        }

        public void DeleteImage(Image image)
        {
            Delete(image);
        }

        public async Task<IEnumerable<Image>> GetAllImagesAsync()
        {
            return await GetAll().OrderBy(image => image.Id).Where(image => image.IsDeleted == false).ToListAsync();
        }

        public async Task<Image> GetImageByIdAsync(Guid ImageId)
        {
            return await GetByCondition(image => image.Id.Equals(ImageId) && image.IsDeleted == false).FirstOrDefaultAsync();
        }
        public void UpdateImage(Image image)
        {
            Update(image);
        }
    }
}
