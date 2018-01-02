using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;
using System.Net;
using System.Net.Mail;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Text.RegularExpressions;
using System.Web;
//using System.Windows.Forms;
using System.Xml;
using MongoDB;
using MongoDB.Driver;
using MongoDB.Driver.Core;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Mongo4
{

    class Program
    {
        static async Task Execute()
        {
            codeEvaler cde = new codeEvaler();
            cde.Eval();
            var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("siva@kustotech.in", "siva");
            var subject = "Morning Airplus Calendar Month";
            var to = new EmailAddress("siva@kustotech.in", "saran");
            //var plainTextContent = "and easy to do anywhere, even with C#";
            var htmlContent = cde.MailBody; ;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, "Hi", htmlContent);
            var response = await client.SendEmailAsync(msg);
        }
        static void Main(string[] args)
        {
            Execute().Wait();
        }
    }
    [BsonDiscriminator("Price")]
    public class Price
    {
        [BsonElement("DateAvailable")]
        public BsonDateTime date { get; set; }

        [BsonElement("LocalCurrency")]
        public BsonString local_currency { get; set; }

        [BsonElement("LocalPrice")]
        public BsonDouble local_price { get; set; }

        [BsonElement("NativeCurrency")]
        public BsonString native_currency { get; set; }

        [BsonElement("NativePrice")]
        public BsonDouble native_price { get; set; }

        [BsonElement("type")]
        public BsonString type { get; set; }
    }
    [BsonDiscriminator("Day")]
    public class Day
    {
        [BsonElement("Available")]
        public BsonBoolean available { get; set; }
        [BsonElement("Date")]
        public BsonDateTime date { get; set; }

        [BsonElement("Price")]
        public Price price { get; set; }
    }
    [BsonDiscriminator("Listing")]
    public class Document
    {
        [BsonElement("Month")]
        public BsonString Month { get; set; }

        [BsonElement("Day")]
        public List<Day> days;

        [BsonElement("DateTimeStamp")]
        public BsonDateTime datetaken;

        [BsonElement("Listing")]
        public BsonString ListingId { get; set; }
        public Document()
        {
            days = new List<Day>();
        }
    }

    public class Mail
    {
        public int Month { get; set; }

        public List<DayName> days;

        public DateTime datetaken;

        public string ListingId { get; set; }
        public void populate(string content)
        {
            DateTime dtx = new DateTime();
            dtx = DateTime.Now;

            dynamic json = JsonConvert.DeserializeObject(content);
            for (int i = 0; i < json["calendar_months"].Count; i++)
            {
                var oneDoc = json["calendar_months"];
                this.datetaken = DateTime.Now;
                for (int j = 0; j < json["calendar_months"][i].days.Count; j++)
                {
                    DayName oneday = new DayName();
                    oneday.available = Convert.ToBoolean(json["calendar_months"][i].days[j]["available"].Value);
                    oneday.date = Convert.ToDateTime(json["calendar_months"][i].days[j]["date"].Value);
                    days.Add(oneday);
                }
            }
        }
        public Mail()
        {
            days = new List<DayName>();
        }
    }
    public class MailOut
    {
        public List<Mail> MailListings= new List<Mail>();
        public string RetString()
        {
            StringBuilder strb = new StringBuilder();
            strb.Append("<Table>");
            strb.Append("<tr>");
            strb.Append("<td style='border:1px solid black'>");
            strb.Append(@"</td>");
            string monthNom = "";
            foreach (Mail m in MailListings)
            {                
                foreach (var dates in m.days)
                {
                    if (monthNom != MonthName(dates.date.Month).mon)
                    {
                        monthNom = MonthName(dates.date.Month).mon;
                    }
                    else{
                        continue;
                    }
                    strb.Append("<td style='border:1px solid black'>" + monthNom+@"</td>");
                    for (int i = 1; i <= MonthName(dates.date.Month).days; i++)
                    {
                        strb.Append("<td style='border:1px solid black'>" + Convert.ToString(i) + @"</td>");
                    }
                }             
            }
            strb.Append(@"</tr>");
            strb.Append(@"<tr></tr>");
            foreach (Mail m in MailListings)
            {
                strb.Append("<tr>");
                strb.Append("<td style='border-bottom:1px solid black'>" + m.ListingId+@"</td><td></td>");
                foreach (var dates in m.days)
                {
                    EachMonth ec = MonthName(dates.date.Month);                    
                    for (int i = 1; i <= ec.days; i++)
                    {
                        strb.Append("<td style='border-bottom:1px solid black'>" + Convert.ToString(dates.available) + @"</td>");
                    }
                }
                strb.Append(@"</tr>");
            }
            strb.Append(@"</Table>");
            return strb.ToString();
        }
        private struct EachMonth
        {
            public string mon;
            public int days;
        }
        private EachMonth MonthName(int mon)
        {
            EachMonth moneach=new EachMonth();
            moneach.days = 0;
            moneach.mon = "";
            switch(mon)
            {
                case 1:
                    moneach.mon = "January";
                    moneach.days = 31;
                    return moneach;
                case 2:
                    moneach.mon = "February";
                    moneach.days = 28;
                    return moneach;                    
                case 3:
                    moneach.mon = "March";
                    moneach.days = 31;
                    return moneach; 
                case 4:
                    moneach.mon = "April";
                    moneach.days = 30;
                    return moneach;
                case 5:
                    moneach.mon = "May";
                    moneach.days = 31;
                    return moneach;                    
                case 6:
                    moneach.mon = "June";
                    moneach.days = 30;
                    return moneach;
                case 7:
                    moneach.mon = "July";
                    moneach.days = 31;
                    return moneach;
                case 8:
                    moneach.mon = "August";
                    moneach.days = 31;
                    return moneach;
                case 9:
                    moneach.mon = "September";
                    moneach.days = 30;
                    return moneach;
                case 10:
                    moneach.mon = "October";
                    moneach.days = 31;
                    return moneach;
                case 11:
                    moneach.mon = "November";
                    moneach.days = 30;
                    return moneach;
                case 12:
                    moneach.mon = "December";
                    moneach.days = 31;
                    return moneach;
            }
            return moneach;
        }
    }

    public class DayName
    {        
        public Boolean available { get; set; }
        
        public DateTime date { get; set; }
    }

    public class codeEvaler
    {

        //string urlTemplate =
//"https://www.airbnb.com/api/v2/calendar_months?key=d306zoyjsyarp7ifhu67rjxn52tv0t20&currency=USD&locale=en&listing_id={0}&month={1}&year={2}&count=6&_format=with_conditions&guests={3}";
        string urlTemplate = "https://www.airbnb.com/api/v2/calendar_months?_format=for_price_calculator_date_picker&count={0}&listing_id={1}&month={2}&year={3}&key=d306zoyjsyarp7ifhu67rjxn52tv0t20&currency=INR&locale=en";

        //Cooper home - 16676839
        private List<string> listingIds = new List<string>() { "16676839", "9199361", "8175972", "10593515", "13625030", "12891710", "11950530", "12742037", "9547197", "18395377", "4925118", "5601452", "13591122" };
        //private List<string> listingIds = new List<string>()        {"7939975" };
        private StringBuilder bodyBuilder = new StringBuilder();
        private StringBuilder headerBuilder = new StringBuilder();
        public string MailBody { get; set; }

        public void DownloadMain(List<string> listingIds, string urlTemplate)
        {
            var month = 12;
            var year = 2017;
            var guests = 3;
            MailOut mout = new MailOut();                            
            foreach (var listingId in listingIds)
            {
                //string url = string.Format(urlTemplate, listingId, month, year, guests);
                string output;
                string url = string.Format(urlTemplate, guests,listingId,month,year);
                DownloadFromAirbnb(listingId, url,out output);
                if(output=="")
                {
                    continue;
                }
                Mail Mone = new Mail();
                Mone.ListingId = listingId;
                Mone.Month = month;
                Mone.populate(output);
                mout.MailListings.Add(Mone);
                Task t = MyMethodAsync();      
            }
            MailBody = mout.RetString();
        }

        public async Task MyMethodAsync()
        {
            Task<int> longRunningTask = LongRunningOperationAsync();
            // independent work which doesn't need the result of LongRunningOperationAsync can be done here

            //and now we call await on the task 
            int result = await longRunningTask;
            //use the result 
            //Console.WriteLine(result);
        }

        public async Task<int> LongRunningOperationAsync() // assume we return an int from this long running operation 
        {
            await Task.Delay(5000); //2 seconds delay
            return 1;
        }

        public void Eval()
        {
            DownloadMain(listingIds, urlTemplate);
            //ProcessMain(listingIds, rootFolder);
        }

        private void DownloadFromAirbnb(string listingId, string url,out string output)
        {

            HttpWebResponse response;
            output = "";

            //string url = "https://www.airbnb.com/api/v2/calendar_months?key=d306zoyjsyarp7ifhu67rjxn52tv0t20&currency=USD&locale=en&listing_id=16676839&month=6&year=2017&count=3&_format=with_conditions";
            //string queryString = new System.Uri(url).Query;
            //var queryDictionary = System.Web.HttpUtility.ParseQueryString(queryString);
            //var listingId = queryDictionary["listing_id"];

            if (Request_www_airbnb_com(url, out response))
            {

                var encoding = Encoding.GetEncoding(response.CharacterSet);
                using (var responseStream = response.GetResponseStream()) { 
                using (var reader = new StreamReader(responseStream, encoding))
                    output= reader.ReadToEnd();
                }

            response.Close();
            }

        }

        public void Save(string listingId, string content)
        {
   
            DateTime dtx = new DateTime();
            dtx = DateTime.Now;
            var client = new MongoClient("mongodb://airplusmongo:7ABjdtYSDQ4Zn3oB4zMPF82xIiK5JgRkTgYjgB7kylxE1jSkaOPnY0qYgtrheNKmOTKFGy98Ao4OKxLm00SQkg==@airplusmongo.documents.azure.com:10255/?ssl=true&replicaSet=globaldb");
            var database = client.GetDatabase("airbnb");
            //database.CreateCollection("listings");
            var collection = database.GetCollection<BsonDocument>("listings");
            dynamic json = JsonConvert.DeserializeObject(content);
            for(int i=0;i<json["calendar_months"].Count;i++)
            {
                var oneDoc = json["calendar_months"];
                Document d = new Document();
                d.Month = new BsonString(oneDoc[i].abbr_name.Value);
                d.ListingId = new BsonString(listingId);
                d.datetaken = new BsonDateTime(DateTime.Now);
                for(int j=0;j< json["calendar_months"][i].days.Count;j++)
                {
                    Day oneday = new Day();
                    oneday.available = new BsonBoolean(json["calendar_months"][i].days[j]["available"].Value);
                    oneday.date = BsonDateTime.Create(json["calendar_months"][i].days[j]["date"].Value);
                    oneday.price = new Price();
                    oneday.price.date = BsonDateTime.Create(json["calendar_months"][i].days[j]["price"]["date"].Value);
                    oneday.price.local_currency = new BsonString(json["calendar_months"][i].days[j]["price"]["local_currency"].Value);
                    oneday.price.local_price = new BsonDouble(json["calendar_months"][i].days[j]["price"]["local_price"].Value);
                    oneday.price.native_currency = new BsonString(json["calendar_months"][i].days[j]["price"]["native_currency"].Value);
                    double dnative_price = json["calendar_months"][i].days[j]["price"]["native_price"].Value;
                    oneday.price.native_price = new BsonDouble(dnative_price);
                    oneday.price.type = new BsonString(json["calendar_months"][i].days[j]["price"]["type"].Value);
                    d.days.Add(oneday);
               }
                var bsonDocument = d.ToBsonDocument();
                collection.InsertOneAsync(bsonDocument).Wait();
            }
            
            //var abELement=new BsonElement()
            //var abListing = new BsonDocument();

            //File.WriteAllText(path, content);
        }
        private bool Request_www_airbnb_com(string url, out HttpWebResponse response)
        {
            response = null;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                request.KeepAlive = true;
                request.Accept = "application/json, text/javascript, */*; q=0.01";
                request.Headers.Add("X-CSRF-Token", @"V4$.airbnb.com$fHjCyefp62k$vfFofoysz0krccodT-NTPwi6dY6Oc8TZYu9oNNS353E=");
                request.Headers.Add("X-Requested-With", @"XMLHttpRequest");
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.84 Safari/537.36";
                request.Referer = "https://www.airbnb.com/rooms/16676839?location=Los%20Angeles%2C%20CA%2C%20United%20States&s=SjTF6t4i";
                request.Headers.Set(HttpRequestHeader.AcceptEncoding, "gzip, deflate, br");
                request.Headers.Set(HttpRequestHeader.AcceptLanguage, "en-CA,en-GB;q=0.9,en-US;q=0.8,en;q=0.7");
                request.Headers.Set(HttpRequestHeader.Cookie, @"bev=1514549109_4pSmBrr6dJvFlt5Z; _csrf_token=V4%24.airbnb.com%24fHjCyefp62k%24vfFofoysz0krccodT-NTPwi6dY6Oc8TZYu9oNNS353E%3D; jitney_client_session_id=28b348e1-cf9f-4beb-ad87-d3bb8568938b; jitney_client_session_created_at=1514549109; _user_attributes=%7B%22curr%22%3A%22INR%22%2C%22guest_exchange%22%3A63.979525%2C%22device_profiling_session_id%22%3A%221514549110--ce3066d7bc2195821f25c54f%22%2C%22giftcard_profiling_session_id%22%3A%221514549110--674e7d23b8eb2365dd714f53%22%2C%22reservation_profiling_session_id%22%3A%221514549110--4525b9b3fb9d43091729e1ef%22%7D; flags=268697600; b47c37150=control; mdr_browser=desktop; sdid=; ftv=1514549107780; cbkp=3; AMP_TOKEN=%24NOT_FOUND; _ga=GA1.2.241556216.1514549109; _gid=GA1.2.873486276.1514549109; __ssid=00c16da8-4467-4d88-8e9e-053f75961d16; jitney_client_session_updated_at=1514549119; has_predicted_user_langs=1; _gat=1; _gat_UA-2725447-23=1; _uetsid=_uet27d678c9; jitney_client_session_updated_at=1514549508");
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    response = (HttpWebResponse)e.Response;
                    return false;
                }
                else return false;
            }
            catch (Exception)
            {
                if (response != null) {
                    response.Close();
                }
                
                return false;
            }

            return true;
        }

    }
}