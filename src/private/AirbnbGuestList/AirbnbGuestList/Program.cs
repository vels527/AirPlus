using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading;
using System.Net.Http;

namespace AirbnbGuestList
{
    class Program
    {
        static void Main(string[] args)
        {
            string jsonContent = File.ReadAllText(@"C:\Calendar_Days.json");
            GuestList guestList = new GuestList();
            guestList.processJSON(jsonContent);
        }
    }
    public class Guest
    {
        string apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
        public static int i = 0;
        SendGridClient client;
        public string FirstName { get; set; }
        public string FullName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Message { get; set; }

        EmailAddress from = new EmailAddress("siva@kustotech.in", "siva");
        public string subject = "Reminder Check In - Check Out";
        EmailAddress to = new EmailAddress("sivanathanb@gmail.com", "saran");
        EmailAddress cc = new EmailAddress("siva@kustotech.in", "siva");
        public Guest()
        {
            client = new SendGridClient(apiKey);
            i++;
            if (i > 2)
            {
                return;
            }
        }
        public async Task SendMail()
        {
            var msg = MailHelper.CreateSingleEmail(from, to, subject, "Hi", Message);
            msg.AddCc(cc);
            var response = await client.SendEmailAsync(msg);

        }
    }
    public class GuestList
    {
        public List<Guest> Guests { get; set; }
        public Guest checkOutGuest { get; set; }
        public Guest checkInGuest { get; set; }
        public GuestList()
        {
            checkInGuest = new Guest();
            checkOutGuest = new Guest();
        }
        public bool processJSON(string JSON)
        {
            DateTime today = DateTime.Now;
            DateTime tomorrow = today.AddDays(1);
            dynamic jsonGuest = JsonConvert.DeserializeObject(JSON);
            var jsonDays = jsonGuest["calendar_days"];
            for(int i = 0; i < jsonDays.Count; i++)
            {
                var doc = jsonDays[i];
                string docDate = doc["date"];
                string docAvailable = doc["available"];
                string tomorrowStr = Convert.ToString(tomorrow.Year) + "-" + (tomorrow.Month > 9 ? Convert.ToString(tomorrow.Month) : ("0" + Convert.ToString(tomorrow.Month))) + "-" + (tomorrow.Day > 9 ? Convert.ToString(tomorrow.Day) : ("0" + Convert.ToString(tomorrow.Day)));
                if (docAvailable=="False")
                {
                    string startDate = doc["reservation"]["start_date"];
                    string endDate = doc["reservation"]["end_date"];
                    if (tomorrowStr == startDate && checkInGuest.Message is null)
                    {
                        Guest guest = new Guest();
                        guest.FullName = doc["reservation"]["guest"]["full_name"];
                        guest.FirstName =doc["reservation"]["guest"]["first_name"];
                        guest.Message = "Send Check in Mail for " + guest.FullName + ".He is checking in on "+ tomorrowStr;
                        guest.subject = "Check in Mail Reminder";
                        //guest.CheckOutMail = "";
                        checkInGuest = guest;
                    }
                    else if (tomorrowStr == endDate && checkOutGuest.Message is null)
                    {
                        Guest guest = new Guest();
                        guest.FullName = doc["reservation"]["guest"]["full_name"];
                        guest.FirstName = doc["reservation"]["guest"]["first_name"];
                        //guest.CheckInMail = "";
                        guest.subject = "Check out Mail Reminder";
                        guest.Message = "Send Check out Mail for " + guest.FullName + ".He is checking out on " + tomorrowStr;
                        checkOutGuest = guest;
                    }
                    if (!(checkOutGuest.Message is null) && !(checkInGuest.Message is null))
                    {
                        break;
                    }
                }
            }
            if (checkOutGuest.Message is null
                && checkInGuest.Message is null)
            {
                Guest guest = new Guest();
                guest.FullName = "";
                guest.FirstName = "";
                guest.Message = "Today no checkout or check in mail.";
                guest.SendMail().Wait();
                return false;
            }
            else if(!(checkInGuest.Message is null))
            {
                checkInGuest.SendMail().Wait();
            }
            else if (!(checkOutGuest.Message is null))
            {
                checkOutGuest.SendMail().Wait();
            }
            return true;
        }
    }
}
