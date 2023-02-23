namespace HotelBooking.Data.Extensions;

public interface IMailSender
{
    Task<bool> SendMailToResetPassword(string toEmail, string resetToken);
}
