using HotelBooking.Data.ViewModel;

namespace HotelBooking.Data.Extensions;

public interface IMailSender
{
    Task<bool> SendMailToResetPassword(string toEmail, string resetToken);
    Task<bool> SendInforOfBooking(string email, InforBookingResponse model);
}
