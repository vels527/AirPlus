using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
//using System.Windows.Forms;
using System.Xml;
using System.Linq;
using System.Web.Script.Serialization;

namespace CodeEvaler
{
    public class Price
    {
        public string date { get; set; }
        public string local_currency { get; set; }
        public int local_price { get; set; }
        public string native_currency { get; set; }
        public int native_price { get; set; }
        public string type { get; set; }
    }

    public class Day
    {
        public bool available { get; set; }
        public DateTime date { get; set; }
        public Price price { get; set; }
    }

    public class Conditions
    {
        public bool closed_to_arrival { get; set; }
        public bool closed_to_departure { get; set; }
        public int min_nights { get; set; }
        public int max_nights { get; set; }
    }

    public class ConditionRange
    {
        public string start_date { get; set; }
        public string end_date { get; set; }
        public Conditions conditions { get; set; }
    }

    public class CalendarMonth
    {
        public string abbr_name { get; set; }
        public List<string> day_names { get; set; }
        public List<Day> days { get; set; }
        public string dynamic_pricing_updated_at { get; set; }
        public int month { get; set; }
        public string name { get; set; }
        public int year { get; set; }
        public List<ConditionRange> condition_ranges { get; set; }
    }

    public class Metadata
    {
    }

    public class RootObject
    {
        public List<CalendarMonth> calendar_months { get; set; }
        public Metadata metadata { get; set; }
    } 

    public static class Program
    {
        public static void Main()
        {
            (new CodeEvaler()).Eval();
        }
    }

    public class CodeEvaler
    {
        private string rootFolder = @"c:\users\sivanathan82\Downloads\";
        string urlTemplate =
        "https://www.airbnb.com/api/v2/calendar_months?key=d306zoyjsyarp7ifhu67rjxn52tv0t20&currency=USD&locale=en&listing_id={0}&month={1}&year={2}&count=6&_format=with_conditions&guests={3}";

        //Cooper home - 16676839
        private List<string> listingIds = new List<string>() { "16676839", "9199361", "8175972", "10593515", "13625030", "12891710", "11950530", "12742037", "9547197", "18395377", "4925118", "5601452", "13591122" };
        private StringBuilder bodyBuilder = new StringBuilder();
        private StringBuilder headerBuilder = new StringBuilder();

        public void Eval()
        {
            DownloadMain(listingIds, urlTemplate);
            ProcessMain(listingIds, rootFolder);
        }

        public void ProcessMain(List<string> listingIds, string rootFolder)
        {
            //Get Latest from ListingId folders    
            bool firstListing = true;
            foreach (var listingId in listingIds)
            {
                string listingIdFolder = Path.Combine(rootFolder, listingId);
                var directory = new DirectoryInfo(listingIdFolder);
                var latestFile = directory.GetFiles().OrderByDescending(f => f.LastWriteTime).First();
                var content = File.ReadAllText(latestFile.FullName);
                if (firstListing)
                {
                    BuildHeader(content);
                    firstListing = false;
                }

                BuildBody(listingId, content);
                //DisplayConsole(listingId, content);
            }

            string n = string.Format("Report-{0:yyyy-MM-dd_hh-mm-ss-tt}.csv", DateTime.Now);
            string path = Path.Combine(rootFolder, n);
            string reportContent = headerBuilder.ToString() + "\r\n" + bodyBuilder.ToString();
            File.WriteAllText(path ,reportContent);
        }

        public void BuildHeader(string content)
        {
            string delimiter = ",";
            headerBuilder.Append(delimiter);
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            RootObject root = json_serializer.Deserialize<RootObject>(content);
            if (root == null || root.calendar_months == null)
                return;

            foreach (var calendar in root.calendar_months.OrderBy(y => y.year).OrderBy(m => m.month).ToList())
            {
                //Console.WriteLine("     " + calendar.name + ":");
                var sortedDays = calendar.days.OrderBy(d => d.date).ToList();
                headerBuilder.Append(calendar.name + delimiter);
                foreach (var dayItem in sortedDays)
                {
                    if (dayItem.date.Month != calendar.month)
                        continue;

                    var prefix = dayItem.date.Day < 10 ? "0" : "";
                    headerBuilder.Append(prefix + dayItem.date.Day + delimiter);
                }
            }
        }

        public void BuildBody(string listingId, string content)
        {
            string delimiter = ",";
            bodyBuilder.AppendLine();
            bodyBuilder.Append(listingId + delimiter);
            var json_serializer = new JavaScriptSerializer();
            RootObject root = json_serializer.Deserialize<RootObject>(content);
            if (root == null || root.calendar_months==null)
                return;
            foreach (var calendar in root.calendar_months.OrderBy(y => y.year).OrderBy(m => m.month).ToList())
            {
                var sortedDays = calendar.days.OrderBy(d => d.date).ToList();
                bodyBuilder.Append(delimiter);
                foreach (var dayItem in sortedDays)
                {
                    if (dayItem.date.Month != calendar.month)
                        continue;

                    bodyBuilder.Append(dayItem.price.local_price + ":" + dayItem.available + delimiter);
                }
            }
        }

        public void DisplayConsole(string listingId, string content)
        {
            Console.WriteLine("-------------------------------------------------------------");
            Console.WriteLine(listingId);
            Console.WriteLine("-------------------------------------------------------------");
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            RootObject root = json_serializer.Deserialize<RootObject>(content);
            foreach (var calendar in root.calendar_months)
            {
                Console.WriteLine("     " + calendar.name + ":");
                var sortedDays = calendar.days.OrderBy(d => d.date).ToList();
                foreach (var dayItem in sortedDays)
                {
                    if (dayItem.date.Month != calendar.month)
                        continue;

                    if (dayItem.available)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                    }

                    var prefix = dayItem.date.Day < 10 ? "0" : "";
                    Console.WriteLine("   " + prefix + dayItem.date.Day + " : " + dayItem.price.local_price);
                }

                Console.WriteLine();
                Console.WriteLine();

                Console.ResetColor();
            }

            Console.WriteLine("-------------------------------------------------------------");
                
        }

        public void Save(string listingId, string content)
        {
            string listingIdFolder = Path.Combine(rootFolder, listingId);
            Directory.CreateDirectory(listingIdFolder);

            string n = string.Format("{0}-{1:yyyy-MM-dd_hh-mm-ss-tt}.txt", listingId, DateTime.Now);
            string path = Path.Combine(listingIdFolder, n);
            File.WriteAllText(path, content);
        }

        public void DownloadMain(List<string> listingIds, string urlTemplate)
        {
            var month = 7;
            var year = 2017;
            var guests = 6;
            foreach (var listingId in listingIds)
            {
                string url = string.Format(urlTemplate, listingId, month, year, guests);
                DownloadFromAirbnb(listingId, url);    
            }
            
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
                if (e.Status == WebExceptionStatus.ProtocolError) response = (HttpWebResponse)e.Response;
                else return false;
            }
            catch (Exception)
            {
                if (response != null) response.Close();
                return false;
            }

            return true;
        }

    }
}