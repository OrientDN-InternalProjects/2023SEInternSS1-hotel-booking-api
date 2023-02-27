using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using HotelBooking.Service.IServices;
using Microsoft.AspNetCore.Http;

namespace HotelBooking.Service.Services
{
    public class PictureService : IPictureService
    {
        public async Task<string> UploadImageAsync(IFormFile image)
        {
            Account account = new Account(
                     "dgs9vyh4n",
                     "759658434427383",
                     "oobrP1pOzKOb9q7E9vB_jBQqQHY");
            Cloudinary cloudinary = new Cloudinary(account);
            cloudinary.Api.Secure = true;

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(image.FileName, image.OpenReadStream())
            };
            var uploadResult = await cloudinary.UploadAsync(uploadParams);
            return uploadResult.Url.ToString();
        }
    }
}
