using System;
using System.Text;
using System.IO;
using System.Net;
using System.Data;

namespace AirbnbGuestList
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Program program = new Program();
                DataTable dt_property = program.GetProperties();
                FileInfo fileInfo = new FileInfo("./log4net.config");
                log4net.Config.XmlConfigurator.Configure(fileInfo);
                foreach (DataRow dr in dt_property.Rows)
                {
                    string URL = Convert.ToString(dr[1]);
                    long ListingId = Convert.ToInt64(dr[0]);
                    string Email = Convert.ToString(dr[2]);
                    string Name = Convert.ToString(dr[3]);
                    string jsonContent = program.MakeRequests(URL);
                    GuestList guestList = new GuestList(ListingId, Email, Name);
                    guestList.ProcessIcal(jsonContent, ListingId);
                    guestList.UpdateTable();
                    guestList.SendMail().Wait();
                    guestList.SendMailASAP().Wait();
                    guestList.Request_account_pushed_co().Wait();
                    string listmsg = ListingId.ToString() + " processed.";
                    Logger.Info(listmsg);
                }
            }
            catch (FileNotFoundException)
            {
                FileInfo fileInfo = new FileInfo("LoggerFileNotFound.txt");
                FileStream fs = fileInfo.Create();
                fs.Close();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

        }

        public DataTable GetProperties()
        {
            return DataLayer.GetProperties();
        }

        public string MakeRequests(string URL)
        {
            HttpWebResponse response;
            string output = "";
            if (Request_www_airbnb_cal(out response, URL))
            {
                //var encoding = Encoding.GetEncoding(response.CharacterSet);
                var encoding = Encoding.UTF8;
                using (var responseStream = response.GetResponseStream())
                {
                    using (var reader = new StreamReader(responseStream, encoding))
                        output = reader.ReadToEnd();
                }
                response.Close();
            }
            return output;
        }
        private bool Request_www_airbnb_cal(out HttpWebResponse response, string URL)
        {
            response = null;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);

                request.KeepAlive = true;
                request.Headers.Add("Upgrade-Insecure-Requests", @"1");
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/67.0.3396.87 Safari/537.36";
                request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
                request.Headers.Set(HttpRequestHeader.AcceptEncoding, "gzip, deflate, br");
                request.Headers.Set(HttpRequestHeader.AcceptLanguage, "en-CA,en-GB;q=0.9,en-US;q=0.8,en;q=0.7");

                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException e)
            {
                Logger.Error("Ical Webexception Error", e);
                if (e.Status == WebExceptionStatus.ProtocolError) response = (HttpWebResponse)e.Response;
                else return false;
            }
            catch (Exception e)
            {
                Logger.Error("Ical Download Error", e);
                if (response != null) response.Close();
                return false;
            }

            return true;
        }


        ///<para>Request_www_airbnb_co_in method will not be used as it requires airbnb credentials.Instead use Request_www_airbnb_cal </para>
        [Obsolete]
        private bool Request_www_airbnb_co_in(out HttpWebResponse response)
        {
            response = null;

            try
            {
                DateTime Median = DateTime.Now;
                DateTime startdate = Median.AddMonths(-1);
                int month = startdate.Month;
                string startmonth = month < 10 ? "0" + Convert.ToString(month) : Convert.ToString(month);
                string startday = startdate.Day < 10 ? "0" + Convert.ToString(startdate.Day) : Convert.ToString(startdate.Day);
                string strstartdate = Convert.ToString(startdate.Year) + "-" + startmonth + "-" + startday;
                DateTime enddate = Median.AddMonths(1);
                month = enddate.Month;
                string endmonth = month < 10 ? "0" + Convert.ToString(month) : Convert.ToString(month);
                string endday = enddate.Day < 10 ? "0" + Convert.ToString(enddate.Day) : Convert.ToString(enddate.Day);
                string strenddate = Convert.ToString(enddate.Year) + "-" + endmonth + "-" + endday;
                string listingid = "16676839";
                string URL = @"https://www.airbnb.co.in/api/v2/calendar_days?key=d306zoyjsyarp7ifhu67rjxn52tv0t20&_format=host_calendar_detailed&listing_id={0}&start_date={1}&end_date={2}";
                string urlcalendardays = String.Format(URL, listingid, strstartdate, strenddate);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlcalendardays);
                request.Referer = "https://www.airbnb.co.in/multicalendar/16676839";
                request.Headers.Set(HttpRequestHeader.AcceptLanguage, "en-US,en-GB;q=0.7,en;q=0.3");
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 Edge/16.16299";
                request.Accept = "application/json";
                request.ContentType = "application/json";
                request.Headers.Add("x-csrf-token", @"V4$.airbnb.co.in$4awvCxg8buc$hONtn4u-atXTFSmYJKaO_S4RRjNq8SlkE0ohf4oI58w=");
                request.Headers.Set(HttpRequestHeader.AcceptEncoding, "gzip, deflate, br");
                request.Headers.Set(HttpRequestHeader.Cookie, @"_csrf_token=V4%24.airbnb.co.in%244awvCxg8buc%24hONtn4u-atXTFSmYJKaO_S4RRjNq8SlkE0ohf4oI58w%3D; flags=814874880; li=1; fbsr_138566025676=bm9we7Ce98JPBBhLllb8rY6OXZOtPlGYkCP8utB0JXw.eyJhbGdvcml0aG0iOiJITUFDLVNIQTI1NiIsImNvZGUiOiJBUUFSLURJbmtpWEZ1VWVPano4YzVnTDltVFdxQnlpOVhnN2ZWaTVsLWFsMmd5Qk83SDJQZ0FjaVo0bG41bF9COVFGamZOb1JNUUVVSXZsRktONVQydTlGdzY2MEpXbXBIS3kxM2Vnd18tRmdFTGxNcWpKcG9xRkFfQWhUYTRRN1lHcHBTU0N3d2w3Ry1mRndFRlktT2tRbGtoeFRBZUpZdFVKdU9DZ3hsUmpVenV5aFA0YnFyN1BRQzlQTVRWRi1jT0tOUUgwNlNFTjRBX2NjR1g5elhPY3A1dk94bXNlSHJnYl9EQWdsRndDTWpjTlZjd1NzT1ZpS0hGd1p0dWU2QV9BT2pDeU9SQkV5RExQcTFVSkNkaHZZeDdGbEVqV0VnYi1WU0lJMmd1d0lvRjhMcHdJV3RpWlliR2doWUJyTEFQQSIsImlzc3VlZF9hdCI6MTUyNjI5NDIwNSwidXNlcl9pZCI6IjEwMTU0ODcwOTA3OTEzNTk1In0; b3b78300e=treatment; jitney_client_session_created_at=1526294190; bev=1526280476_8pPVixfZNFu8b5Y0; sdid=; 07689523f=control; ftv=1526280477466; jitney_client_session_updated_at=1526294219; abb_fa2=%7B%22user_id%22%3A%2215%7C1%7CiO667MbQ3%2B%2FANDTq4DY0BIsbwW3kPhpZMXbAFwEDJ9X9iGrIbxj6d%2Bw%3D%22%7D; cbkp=3; _pt=1--WyI1MDA5OWYyMGRmNjlhZjRkMDEzMDJkYjdkZDVkNGRlYzc0ZWMyN2ZjIl0%3D--48d10f2841a5d986837d9fde282fac87b93979c8; fbm_138566025676=base_domain=.airbnb.co.in; 3b689aa21=treatment; _gid=GA1.3.430135272.1526280479; a405338e3=treatment; _ga=GA1.3.14036481.1526280479; rclmd=%7B%22144905227%22%3D%3E%22facebook%22%7D; hli=1; 70d75c19c=cereal; jitney_client_session_id=d06c6256-10e6-4020-80b7-10df850b8b7d; 0696b4d7c=treatment; _user_attributes=%7B%22curr%22%3A%22INR%22%2C%22guest_exchange%22%3A67.36%2C%22device_profiling_session_id%22%3A%221526280476--1efef9322f092b88e0156128%22%2C%22giftcard_profiling_session_id%22%3A%221526294191--d3084f0b242717b89a4e14da%22%2C%22reservation_profiling_session_id%22%3A%221526294191--beaaf40218c3d0cfb33c5a9a%22%2C%22id%22%3A144905227%2C%22hash_user_id%22%3A%2250099f20df69af4d01302db7dd5d4dec74ec27fc%22%2C%22eid%22%3A%22tH6hRKVvzEv5srnDF760PA%3D%3D%22%2C%22num_msg%22%3A53%2C%22num_notif%22%3A1%2C%22num_alert%22%3A1%2C%22num_h%22%3A0%2C%22num_pending_requests%22%3A0%2C%22num_trip_notif%22%3A0%2C%22name%22%3A%22Siva%22%2C%22num_action%22%3A0%2C%22is_admin%22%3Afalse%2C%22can_access_photography%22%3Afalse%2C%22referrals_info%22%3A%7B%22terms_and_conditions_link%22%3A%22https%3A%2F%2Fwww.airbnb.co.in%2Freferrals%2Fterms_and_conditions%3Foffer_name%3DIN_localized_test_v6%22%2C%22referrer_guest%22%3A%22%E2%82%B91%2C100%22%7D%7D; _airbed_session_id=7bb2c42cb83e0cedf0c8450d6014e780; AMP_TOKEN=%24NOT_FOUND; _gat=1; jlp3=true; roles=0; _uetsid=_uet36bd484c; fbs=connected; alfc=0; alfces=0");
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
