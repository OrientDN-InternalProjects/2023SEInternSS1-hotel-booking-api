﻿using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace HotelBooking.Data.Extensions;

public class MailSender : IMailSender
{
    public async Task<bool> SendMailToResetPassword(string toEmail, string resetToken)
    {
        // Create email message
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse("girlmilk123@gmail.com"));
        email.To.Add(MailboxAddress.Parse(toEmail));
        email.Subject = "Confirm EMail";
        email.Body = new TextPart(TextFormat.Plain) { Text = resetToken };

        // Send email
        using var smtp = new SmtpClient();
        smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
        smtp.Authenticate("girlmilk123@gmail.com", "nlxqkuqkgtmerqog");
        var res = await smtp.SendAsync(email);
        smtp.Disconnect(true);
        return true;
    }
}