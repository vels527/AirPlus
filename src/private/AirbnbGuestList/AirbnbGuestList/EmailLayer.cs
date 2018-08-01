using System;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace AirbnbGuestList
{
    static class EmailLayer
    {
        static string apiKey;
        static SendGridClient client;

        static EmailLayer()
        {
            apiKey = Environment.GetEnvironmentVariable("SENDSEND");
            client = new SendGridClient(apiKey);
        }

        public static async Task SendMail(string from,string to,string cc,string subject,string message)
        {
                var fromEmail = new EmailAddress(from);
                var toEmail = new EmailAddress(to);
                var ccEmail = new EmailAddress(cc);
                var msg = MailHelper.CreateSingleEmail(fromEmail, toEmail, subject, "Airplus", message);
                msg.AddCc(cc);
                var response = await client.SendEmailAsync(msg);
        }
    }
}
