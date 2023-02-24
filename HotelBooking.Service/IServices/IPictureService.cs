using Microsoft.AspNetCore.Http;

namespace HotelBooking.Service.IServices
{
    public interface IPictureService
    {
        Task<string> UploadImageAsync(IFormFile image);
    }
}
