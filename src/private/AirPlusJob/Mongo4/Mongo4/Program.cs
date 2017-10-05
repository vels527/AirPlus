using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;
using System.Net;
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
        static void Main(string[] args)
        {
            codeEvaler cde = new codeEvaler();
            cde.Eval();
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
    
    public class codeEvaler
    {

        string urlTemplate =
"https://www.airbnb.com/api/v2/calendar_months?key=d306zoyjsyarp7ifhu67rjxn52tv0t20&currency=USD&locale=en&listing_id={0}&month={1}&year={2}&count=6&_format=with_conditions&guests={3}";

        //Cooper home - 16676839
        private List<string> listingIds = new List<string>() { "16676839", "9199361", "8175972", "10593515", "13625030", "12891710", "11950530", "12742037", "9547197", "18395377", "4925118", "5601452", "13591122" };
        private StringBuilder bodyBuilder = new StringBuilder();
        private StringBuilder headerBuilder = new StringBuilder();

        public void DownloadMain(List<string> listingIds, string urlTemplate)
        {
            var month = 9;
            var year = 2017;
            var guests = 6;
            foreach (var listingId in listingIds)
            {
                string url = string.Format(urlTemplate, listingId, month, year, guests);
                DownloadFromAirbnb(listingId, url);
                Task t = MyMethodAsync();      
            }

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
            await Task.Delay(2000); //2 seconds delay
            return 1;
        }

        public void Eval()
        {
            DownloadMain(listingIds, urlTemplate);
            //ProcessMain(listingIds, rootFolder);
        }

        private void DownloadFromAirbnb(string listingId, string url)
        {

            HttpWebResponse response;

            //string url = "https://www.airbnb.com/api/v2/calendar_months?key=d306zoyjsyarp7ifhu67rjxn52tv0t20&currency=USD&locale=en&listing_id=16676839&month=6&year=2017&count=3&_format=with_conditions";
            //string queryString = new System.Uri(url).Query;
            //var queryDictionary = System.Web.HttpUtility.ParseQueryString(queryString);
            //var listingId = queryDictionary["listing_id"];

            if (Request_www_airbnb_com(url, out response))
            {

                var encoding = UTF8Encoding.UTF8;
                using (var reader = new System.IO.StreamReader(response.GetResponseStream(), encoding))
                {
                    string responseText = reader.ReadToEnd();                    
                    Save(listingId, responseText);
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
                //WebProxy proxy = new WebProxy("127.0.0.1", 8888);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                //16676839
                //request.Proxy = proxy;
                request.Accept = "application/json, text/javascript, */*; q=0.01";
                request.Headers.Add("x-csrf-token", @"V4$.airbnb.com$Im1tM2jDiT0$SZ20E7bTT7HmdsWUqFGPd9QgPQRYGrqQrdj9xHPFyD8=");
                request.Headers.Add("x-requested-with", @"XMLHttpRequest");
                request.Referer = "https://www.airbnb.com/rooms/9199361?location=Santa%20Clara%2C%20CA%2C%20United%20States&s=-n-9qTJp";
                request.Headers.Set(HttpRequestHeader.AcceptLanguage, "en-US,en;q=0.5");
                request.Headers.Set(HttpRequestHeader.AcceptEncoding, "gzip, deflate, peerdist");
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; Touch; rv:11.0) like Gecko";
                request.Headers.Set(HttpRequestHeader.Cookie, @"bev=1426028442_4xz%2BO1yaJqtluowu; _ga=GA1.2.187075194.1426028443; __ssid=2d331962-738a-4d51-9899-058862682edc; sdid=; ftv=1486940561242; vr_coupon=control; p2_aa_cdn_test_v2=treatment; i18n_google_nmt=treatment; p3_interactive_book_it_loading=treatment; p3_pdp_ia_reduce_redundancy=treatment; p3_pdp_ia_consolidate=control; p3_pdp_ia_location=control; mdr_browser=desktop; _gid=GA1.2.83740503.1496446037; p3_pdp_ia_nav=treatment; _csrf_token=V4%24.airbnb.com%24Im1tM2jDiT0%24SZ20E7bTT7HmdsWUqFGPd9QgPQRYGrqQrdj9xHPFyD8%3D; _user_attributes=%7B%22curr%22%3A%22USD%22%2C%22guest_exchange%22%3A1.0%2C%22device_profiling_session_id%22%3A%221496460251--0630b94545ed3cdd0e841d25%22%2C%22giftcard_profiling_session_id%22%3A%221496460251--53261e7d5e1f2c1edb45c8d3%22%2C%22reservation_profiling_session_id%22%3A%221496460251--2bae832bdbc8a3f9c50b5dec%22%7D; flags=268435456; _airbed_session_id=2b6674beff36f953ee1421a25431b0b9; _gat=1; _uetsid=_uetba689884");
                request.Headers.Add("X-P2P-PeerDist", @"Version=1.1");
                request.Headers.Add("X-P2P-PeerDistEx", @"MinContentInformation=1.0, MaxContentInformation=2.0");
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
};