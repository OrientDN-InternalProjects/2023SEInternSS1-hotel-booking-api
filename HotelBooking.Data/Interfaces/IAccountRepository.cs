using HotelBooking.Common.Base;
using HotelBooking.Data.DTOs.Account;

namespace HotelBooking.Data.Interfaces
{
    public interface IAccountRepository
    {
        Task<ResponseModel> RegisterAsync(RegisterRequest model);
        Task<ResponseModel> LoginAsync(LoginRequest model);
        Task<ResponseModel> ForgetPassword(ForgetPasswordRequest model);
        Task<ResponseModel> ResetPassword(ResetPasswordRequest model);
        Task<ResponseModel> ChangePassword(string email, ChangePasswordRequest model);
    }
}
