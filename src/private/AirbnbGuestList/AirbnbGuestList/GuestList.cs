using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net;
using System.Data;
using Ical.Net;
using System.Configuration;

namespace AirbnbGuestList
{
    public class GuestList
    {
        public List<Guest> Guests;
        private string from = ConfigurationManager.AppSettings["fromEmailAddress"];
        private string cc = ConfigurationManager.AppSettings["ccEmailAddress"];
        private string to;

        public long ListingId
        {
            get; set;
        }

        public DataTable GuestTable
        {
            get
            {
                return DataLayer.GuestTable(Guests);
            }
        }

        public string MessageForNotification
        {
            get
            {
                return DataLayer.MessageForNotification(this.ListingId);
            }
        }

        public string MessageForDay
        {
            get
            {
                return DataLayer.MessageForDay(this.ListingId);
            }
        }

        public string Message
        {
            get
            {
                return DataLayer.Message(this.ListingId);
            }
        }



        public GuestList(long listingId, string email, string fName)
        {
            Guests = new List<Guest>();
            ListingId = listingId;
            to = email;
        }
        public async Task SendMail()
        {
            try
            {
                string subject = ConfigurationManager.AppSettings["RegularSubject"];
                await EmailLayer.SendMail(from, to, cc, subject, Message);
            }
            catch (Exception ex)
            {
                string Err = "Error when sending mail -" + this.ListingId;
                Logger.Error(Err, ex);
            }
        }

        public async Task SendMailASAP()
        {
            try
            {
                if (MessageForDay == "")
                {
                    string msg = "ListingID " + this.ListingId + " has no check In or check Out for the day .";
                    Logger.Info(msg);
                }
                else
                {
                    string subject = ConfigurationManager.AppSettings["DailySubject"];
                    await EmailLayer.SendMail(from, to, cc, subject, MessageForDay);
                }
            }
            catch (Exception ex)
            {
                string Err = "Error when sending mail -" + this.ListingId;
                Logger.Error(Err, ex);
            }
        }
        public void UpdateTable()
        {
            if (Guests.Count() > 0)
            {
                try
                {
                    DataLayer.UpdateTable(GuestTable);
                }
                catch (Exception e)
                {
                    Logger.Error("Error when inserting or updating guests", e);
                }
            }
        }
        public void ProcessIcal(string calendarInfo, long ListingId)
        {
            try
            {
                var calobjects = Calendar.Load(calendarInfo);
                var events = calobjects.Events;
                var guestFromLINQ = from e in events where (e.Location != null && e.DtStart.Date >= DateTime.Now.AddMonths(-2)) select new Guest(e.Summary, e.Description, e.DtStart.Date, e.DtEnd.Date, ListingId);
                Guests = guestFromLINQ.ToList<Guest>();
            }
            catch (Exception e)
            {
                string exMsg = "Ical processing error for " + ListingId;
                Logger.Error(exMsg, e);
            }
        }


        ///<para>processJSON is obsolete as this flow uses airbnb crdentials.Instead use airbnbcal</para>        
        [Obsolete]
        public bool processJSON(string JSON)
        {
            DateTime today = DateTime.Now;
            DateTime tomorrow = today.AddDays(1);
            dynamic jsonGuest = JsonConvert.DeserializeObject(JSON);
            var jsonDays = jsonGuest["calendar_days"];
            for (int i = 0; i < jsonDays.Count; i++)
            {
                var doc = jsonDays[i];
                string docDate = doc["date"];
                string docAvailable = doc["available"];
                if (docAvailable == "False")
                {
                    string startDate = doc["reservation"]["start_date"];
                    string endDate = doc["reservation"]["end_date"];
                    Guest guest = new Guest();
                    guest.FullName = doc["reservation"]["guest"]["full_name"];
                    guest.FirstName = doc["reservation"]["guest"]["first_name"];
                    guest.AirplusId = Convert.ToInt64(doc["reservation"]["guest"]["id"]);
                    guest.ListingId = Convert.ToInt64(doc["reservation"]["hosting_id"]);
                    guest.StartDate = Convert.ToDateTime(startDate);
                    guest.EndDate = Convert.ToDateTime(endDate);
                    guest.RequestedStartDate = guest.StartDate.AddHours(16);
                    guest.RequestedEndDate = guest.EndDate.AddHours(11);

                    Guest another = new Guest();
                    another.AirplusId = 0;
                    if (Guests.Count() == 0)
                    {
                        Guests.Add(guest);
                    }
                    else
                    {
                        another = Guests.Find(e => (e.AirplusId == Convert.ToInt64(doc["reservation"]["guest"]["id"])));
                        if (another is null)
                        {
                            Guests.Add(guest);
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
            }

            return true;
        }
        public async Task Request_account_pushed_co()
        {
            try
            {
                var client = new HttpClient();

                var requestContent = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string, string>("app_key", "P9cVh1ypGxkd1ISzstkg"),
                new KeyValuePair<string, string>("app_secret", "bgcMm994HNegQpjSMGhNX0PHMBsuReNj8w035o7pnRuACuChniX7LsoYcoB1Xeqb"),
                new KeyValuePair<string, string>("target_type", "app"),
                new KeyValuePair<string, string>("content", this.MessageForNotification)
                });

                // Get the response.
                HttpResponseMessage response = await client.PostAsync(@"https://api.pushed.co/1/push", requestContent);

                //delay
                System.Threading.Thread.Sleep(2000);

                // Get the response content.
                HttpContent responseContent = response.Content;
                // Get the stream of the content.

                using (var reader = new StreamReader(await responseContent.ReadAsStreamAsync()))
                {
                    // Write the output.
                    Console.WriteLine(await reader.ReadToEndAsync());
                }
            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                }
                Logger.Error("Protocol Error in push notification", e);
            }

            catch (Exception ex)
            {
                Logger.Error("Error in push notification", ex);
            }
        }
    }
}
