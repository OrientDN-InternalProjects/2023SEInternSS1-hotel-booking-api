using HotelBooking.Common.Base;
using HotelBooking.Data.DTOs.Account;
using HotelBooking.Model.Entities;

namespace HotelBooking.Data.Interfaces
{
    public interface IAccountRepository
    {
        Task<ResponseModel> RegisterAsync(RegisterRequest model);
        Task<ResponseModel> LoginAsync(LoginRequest model);
        Task<ResponseModel> ForgetPassword(ForgetPasswordRequest model);
        Task<ResponseModel> ResetPassword(ResetPasswordRequest model);
        Task<ResponseModel> ChangePassword(string email, ChangePasswordRequest model);
        Task<User> GetUserById(string id);
        Task<User> GetUserByEmail(string email);
    }
}
