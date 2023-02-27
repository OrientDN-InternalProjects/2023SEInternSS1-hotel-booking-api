using HotelBooking.Data.DTOs.Account;
using HotelBooking.Data.ViewModel;

namespace HotelBooking.Data.Interfaces
{
    public interface IAccountRepository
    {
        Task<AuthenicationModel> RegisterAsync(RegisterDTO model);
        Task<AuthenicationModel> LoginAsync(LoginDTO model);
        Task<AuthenicationModel> ForgetPassword(ForgetPasswordDTO model);
        Task<AuthenicationModel> ResetPassword(ResetPasswordDTO model);
        Task<AuthenicationModel> ChangePassword(string email, ChangePasswordDTO model);
    }
}
