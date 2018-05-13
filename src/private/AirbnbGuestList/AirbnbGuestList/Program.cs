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
        SendGridClient client;
        public string FirstName { get; set; }
        public string FullName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Message { get; set; }
        EmailAddress from = new EmailAddress("siva@kustotech.in", "siva");
        string subject = "Morning Airplus Calendar Month";
        EmailAddress to = new EmailAddress("saran@kustotech.in", "saran");
        EmailAddress cc = new EmailAddress("siva@kustotech.in", "siva");
        public Guest()
        {
            client = new SendGridClient(apiKey);
        }
        public bool SendMail()
        {
            var msg = MailHelper.CreateSingleEmail(from, to, subject, "Hi", Message);
            msg.AddCc(cc);
            var response = client.SendEmailAsync(msg);
            if (response.IsCompleted)
            {
                return true;
            }
            return false;
        }
    }
    public class GuestList
    {
        public List<Guest> Guests { get; set; } 
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
                    if (tomorrowStr == startDate)
                    {
                        Guest guest = new Guest();
                        guest.FullName = doc["reservation"]["full_name"];
                        guest.FirstName =doc["reservation"]["first_name"];
                        guest.Message = "Send Check in Mail for " + guest.FullName + ".He is checking in on "+ tomorrowStr;
                        //guest.CheckOutMail = "";
                        Guests.Add(guest);
                    }
                    else if (tomorrowStr == endDate)
                    {
                        Guest guest = new Guest();
                        guest.FullName = doc["reservation"]["full_name"];
                        guest.FirstName = doc["reservation"]["first_name"];
                        //guest.CheckInMail = "";
                        guest.Message = "Send Check out Mail for " + guest.FullName + ".He is checking out on " + tomorrowStr;
                        Guests.Add(guest);
                    }
                    if (Guests.Count > 1)
                    {
                        break;
                    }
                }
            }
            if (Guests.Count < 1)
            {
                return false;
            }
            else
            {
                foreach (Guest g in Guests)
                {
                    g.SendMail();
                }
            }
            return true;
        }
    }
}
