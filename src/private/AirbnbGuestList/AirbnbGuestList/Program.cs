﻿using System;
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
using System.Net;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Ical.Net;
using Ical.Net.CalendarComponents;

namespace AirbnbGuestList
{
    class Program
    {
        static void Main(string[] args)
        {
            Program program = new Program();
            DataTable dt_property = program.GetProperties();
            foreach(DataRow dr in dt_property.Rows)
            {
                string URL = Convert.ToString(dr[1]);
                long ListingId = Convert.ToInt64(dr[0]);
                string jsonContent = program.MakeRequests(URL);
                GuestList guestList = new GuestList();
                guestList.ProcessIcal(jsonContent,ListingId);
                //guestList.processJSON(jsonContent);
                guestList.UpdateTable();
                //guestList.SendMail().Wait();
                //guestList.SendMailASAP().Wait();
                guestList.Request_account_pushed_co().Wait();
            }

        }
        public DataTable GetProperties()
        {
            SqlConnection connection = new SqlConnection(ConnString);
            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet("PropertyList");
            SqlCommand cmd = new SqlCommand("GetPropertyList", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            da.SelectCommand = cmd;
            da.Fill(ds);
            return ds.Tables[0];
        }
        public string ConnString
        {
            get
            {
#if DEBUG
                return ConfigurationManager.ConnectionStrings["Dev"].ConnectionString;
#else
            return ConfigurationManager.ConnectionStrings["Prod"].ConnectionString;
#endif
            }
        }
        public string MakeRequests(string URL)
        {
            HttpWebResponse response;
            string output="";
            if (Request_www_airbnb_cal(out response,URL))
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
        private bool Request_www_airbnb_cal(out HttpWebResponse response,string URL)
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
        private bool Request_www_airbnb_co_in(out HttpWebResponse response)
        {
            response = null;

            try
            {
                DateTime Median = DateTime.Now;
                DateTime startdate = Median.AddMonths(-1);
                int month = startdate.Month;
                string startmonth = month < 10 ? "0" + Convert.ToString(month) : Convert.ToString(month);
                string startday = startdate.Day < 10 ? "0" + Convert.ToString(startdate.Day) :Convert.ToString(startdate.Day);
                string strstartdate = Convert.ToString(startdate.Year) + "-" + startmonth + "-" +startday;
                DateTime enddate = Median.AddMonths(1);
                month = enddate.Month;
                string endmonth= month < 10 ? "0" + Convert.ToString(month) : Convert.ToString(month);
                string endday= enddate.Day < 10 ? "0" + Convert.ToString(enddate.Day) : Convert.ToString(enddate.Day);
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

    public class Guest
    {

        public string FirstName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime RequestedStartDate { get; set; }
        public DateTime RequestedEndDate { get; set; }
        public long AirplusId { get; set; }
        public long ListingId { get; set; }
        
        public Guest()
        {
            

        }
    }

    public class GuestList
    {
        public List<Guest> Guests;
        string apiKey = Environment.GetEnvironmentVariable("SENDSEND");
        SendGridClient client;
        public string ConnString
        {
            get
            {
#if DEBUG
                return ConfigurationManager.ConnectionStrings["Dev"].ConnectionString;
#else
            return ConfigurationManager.ConnectionStrings["Prod"].ConnectionString;
#endif
            }
        }
        public DataTable GuestTable
        {
            get
            {
                DataTable dt = new DataTable("GUESTPROPERTYTYPETABLE");
                DataColumn columnFullName = new DataColumn("FullName", Type.GetType("System.String"));
                dt.Columns.Add(columnFullName);
                DataColumn columnFirstName = new DataColumn("FirstName", Type.GetType("System.String"));
                dt.Columns.Add(columnFirstName);
                DataColumn columnEmail = new DataColumn("Email", Type.GetType("System.String"));
                dt.Columns.Add(columnEmail);
                DataColumn columnPhone = new DataColumn("Phone", Type.GetType("System.String"));
                dt.Columns.Add(columnPhone);
                DataColumn columnListing = new DataColumn("ListingID", Type.GetType("System.Int32"));
                dt.Columns.Add(columnListing);
                DataColumn columnCheckIn = new DataColumn("CHECKIN", Type.GetType("System.DateTime"));
                dt.Columns.Add(columnCheckIn);
                DataColumn columnCheckOut = new DataColumn("CHECKOUT", Type.GetType("System.DateTime"));
                dt.Columns.Add(columnCheckOut);
                foreach (Guest g in Guests)
                {
                    DataRow dr = dt.NewRow();
                    dr["FullName"]=g.FullName;
                    dr["FirstName"] = g.FirstName;
                    dr["ListingID"] = g.ListingId;
                    dr["CHECKIN"]= g.StartDate;
                    dr["CHECKOUT"] = g.EndDate;
                    if (g.Email is null)
                    {
                        dr["Email"]= DBNull.Value;
                    }
                    else
                    {
                        dr["Email"] = g.Email;
                    }
                    if (g.Phone is null)
                    {
                        dr["Phone"] = DBNull.Value;
                    }
                    else
                    {
                        dr["Phone"] = g.Phone;
                    }
                    dt.Rows.Add(dr);
                }
                return dt;
            }
        }
        public string MessageForNotification
        {
            get
            {
                SqlConnection connection = new SqlConnection(ConnString);
                connection.Open();
                SqlDataAdapter da = new SqlDataAdapter();
                DataSet ds = new DataSet("Users");
                SqlCommand cmd = new SqlCommand("GetGuestsListForToday", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@user", SqlDbType.VarChar, 100));
                cmd.Parameters[0].Value = "saran";
                da.SelectCommand = cmd;
                da.Fill(ds);
                DataSet dataset = ds;
                DataTable data = dataset.Tables[0];
                DataTable data_1 = dataset.Tables[1];
                StringBuilder strb = new StringBuilder();
                strb.Append("Check IN for ");
                
                foreach (DataRow dr in data.Rows)
                {
                    strb.Append(Convert.ToString(dr[3]));
                    strb.Append(" on ");
                    strb.Append(Convert.ToString(dr[7]));
                    strb.Append(" check out on ");
                    strb.Append(Convert.ToString(dr[8]));
                    strb.Append(" with cleaning at ");
                    if (Convert.ToString(dr[10]) != "")
                        strb.Append(Convert.ToDateTime(dr[10]).ToShortTimeString());
                    strb.Append(" Remarks: ");
                    strb.Append(Convert.ToString(dr[11]));
                    break;
                }
                strb.Append("\n");
                strb.Append("Check Out for ");


                foreach (DataRow dr in data_1.Rows)
                {
                    strb.Append(Convert.ToString(dr[3]));
                    strb.Append(" on ");
                    strb.Append(Convert.ToString(dr[7]));
                    strb.Append(" check out on ");
                    strb.Append(Convert.ToString(dr[8]));
                    strb.Append(" with cleaning at ");
                    if (Convert.ToString(dr[10]) != "")
                        strb.Append(Convert.ToDateTime(dr[10]).ToShortTimeString());
                    strb.Append(" Remarks: ");
                    strb.Append(Convert.ToString(dr[11]));
                    break;
                }
                strb.Append("\n");
                return strb.ToString();
            }
        }
        public string MessageForDay
        {
            get
            {
                SqlConnection connection = new SqlConnection(ConnString);
                connection.Open();
                SqlDataAdapter da = new SqlDataAdapter();
                DataSet ds = new DataSet("Users");
                SqlCommand cmd = new SqlCommand("GetGuestsListForToday", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@user", SqlDbType.VarChar, 100));
                cmd.Parameters[0].Value = "saran";
                da.SelectCommand = cmd;
                da.Fill(ds);
                DataSet dataset = ds;
                DataTable data = dataset.Tables[0];
                DataTable data_1 = dataset.Tables[1];
                DataTable statuscode_data = dataset.Tables[2];
                StringBuilder strb = new StringBuilder();
                strb.Append("<P>Check IN");
                strb.Append("<Table>");
                strb.Append("<tr>");
                strb.Append(@"<td style='border:1px solid black;background-color:yellow;'>Guest Name</td>");
                strb.Append(@"<td style='border:1px solid black;background-color:yellow;'>Check In</td>");
                strb.Append(@"<td style='border:1px solid black;background-color:yellow;'>Check Out</td>");
                strb.Append(@"<td style='border:1px solid black;background-color:yellow;'>Requested Check In</td>");
                strb.Append(@"<td style='border:1px solid black;background-color:yellow;'>Requested Check Out</td>");
                strb.Append(@"<td style='border:1px solid black;background-color:yellow;'>Remarks</td>");
                strb.Append(@"<td style='border:1px solid black;background-color:yellow;'>Cleaning Timing</td>");
                strb.Append(@"<td style='border:1px solid black;background-color:yellow;'>Status</td>");

                foreach (DataRow dr in data.Rows)
                {
                    strb.Append("<tr>");
                    strb.Append(@"<td style='border-bottom:1px solid black;border-right:1px solid black'>" + Convert.ToString(dr[3]) + @"</td>");
                    strb.Append(@"<td style='border-bottom:1px solid black;border-right:1px solid black'>" + Convert.ToString(dr[7]) + @"</td>");
                    strb.Append(@"<td style='border-bottom:1px solid black;border-right:1px solid black'>" + Convert.ToString(dr[8]) + @"</td>");
                    strb.Append(@"<td style='border-bottom:1px solid black;border-right:1px solid black'>" + Convert.ToDateTime(dr[5]).ToShortTimeString() + @"</td>");
                    strb.Append(@"<td style='border-bottom:1px solid black;border-right:1px solid black'>" + Convert.ToDateTime(dr[6]).ToShortTimeString() + @"</td>");
                    strb.Append(@"<td style='border-bottom:1px solid black;border-right:1px solid black'>" + Convert.ToString(dr[11]) + @"</td>");
                    if (Convert.ToString(dr[10]) != "")
                    {
                        strb.Append(@"<td style='border-bottom:1px solid black;border-right:1px solid black'>" + Convert.ToDateTime(dr[10]).ToShortTimeString() + @"</td>");
                    }
                    else
                    {
                        strb.Append(@"<td style='border-bottom:1px solid black;border-right:1px solid black'></td>");
                    }
                    bool isnotstatus = true;
                    foreach (DataRow d in statuscode_data.Rows)
                    {
                        if (Convert.ToString(d[1]) == Convert.ToString(dr[9]))
                        {
                            strb.Append(@"<td style='border-bottom:1px solid black;border-right:1px solid black'>" + Convert.ToString(d[1]) + @"</td>");
                            isnotstatus = false;
                            break;
                        }
                    }
                    if (isnotstatus)
                    {
                        strb.Append(@"<td style='border-bottom:1px solid black;border-right:1px solid black'>Not Specified</td>");
                    }
                    strb.Append(@"</tr>");
                }
                strb.Append(@"</Table>");
                strb.Append("<P>Check Out");
                strb.Append("<Table>");
                strb.Append("<tr>");
                strb.Append(@"<td style='border:1px solid black;background-color:yellow;'>Guest Name</td>");
                strb.Append(@"<td style='border:1px solid black;background-color:yellow;'>Check In</td>");
                strb.Append(@"<td style='border:1px solid black;background-color:yellow;'>Check Out</td>");
                strb.Append(@"<td style='border:1px solid black;background-color:yellow;'>Requested Check In</td>");
                strb.Append(@"<td style='border:1px solid black;background-color:yellow;'>Requested Check Out</td>");
                strb.Append(@"<td style='border:1px solid black;background-color:yellow;'>Remarks</td>");
                strb.Append(@"<td style='border:1px solid black;background-color:yellow;'>Cleaning Timing</td>");
                strb.Append(@"<td style='border:1px solid black;background-color:yellow;'>Status</td>");

                foreach (DataRow dr in data_1.Rows)
                {
                    strb.Append("<tr>");
                    strb.Append(@"<td style='border-bottom:1px solid black;border-right:1px solid black'>" + Convert.ToString(dr[3]) + @"</td>");
                    strb.Append(@"<td style='border-bottom:1px solid black;border-right:1px solid black'>" + Convert.ToString(dr[7]) + @"</td>");
                    strb.Append(@"<td style='border-bottom:1px solid black;border-right:1px solid black'>" + Convert.ToString(dr[8]) + @"</td>");
                    strb.Append(@"<td style='border-bottom:1px solid black;border-right:1px solid black'>" + Convert.ToDateTime(dr[5]).ToShortTimeString() + @"</td>");
                    strb.Append(@"<td style='border-bottom:1px solid black;border-right:1px solid black'>" + Convert.ToDateTime(dr[6]).ToShortTimeString() + @"</td>");
                    strb.Append(@"<td style='border-bottom:1px solid black;border-right:1px solid black'>" + Convert.ToString(dr[11]) + @"</td>");
                    if (Convert.ToString(dr[10]) != "")
                    {
                        strb.Append(@"<td style='border-bottom:1px solid black;border-right:1px solid black'>" + Convert.ToDateTime(dr[10]).ToShortTimeString() + @"</td>");
                    }
                    else
                    {
                        strb.Append(@"<td style='border-bottom:1px solid black;border-right:1px solid black'></td>");
                    }
                    bool isnotstatus = true;
                    foreach (DataRow d in statuscode_data.Rows)
                    {
                        if (Convert.ToString(d[1]) == Convert.ToString(dr[9]))
                        {
                            strb.Append(@"<td style='border-bottom:1px solid black;border-right:1px solid black'>" + Convert.ToString(d[1]) + @"</td>");
                            isnotstatus = false;
                            break;
                        }
                    }
                    if (isnotstatus)
                    {
                        strb.Append(@"<td style='border-bottom:1px solid black;border-right:1px solid black'>Not Specified</td>");
                    }
                    strb.Append(@"</tr>");
                }
                strb.Append(@"</Table>");
                return strb.ToString();
            }
        }
        public string Message {
            get {
                SqlConnection connection = new SqlConnection(ConnString);
                connection.Open();
                SqlDataAdapter da = new SqlDataAdapter();
                DataSet ds = new DataSet("Users");
                SqlCommand cmd = new SqlCommand("GetGuestsList", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@user", SqlDbType.VarChar, 100));
                cmd.Parameters[0].Value = "saran";
                da.SelectCommand = cmd;
                da.Fill(ds);
                DataSet dataset = ds;
                DataTable data = dataset.Tables[0];
                DataTable statuscode_data = dataset.Tables[1];
                StringBuilder strb = new StringBuilder();
                strb.Append("<Table>");
                strb.Append("<tr>");
                strb.Append(@"<td style='border:1px solid black;background-color:yellow;'>Guest Name</td>");
                strb.Append(@"<td style='border:1px solid black;background-color:yellow;'>Check In</td>");
                strb.Append(@"<td style='border:1px solid black;background-color:yellow;'>Check Out</td>");
                strb.Append(@"<td style='border:1px solid black;background-color:yellow;'>Requested Check In</td>");
                strb.Append(@"<td style='border:1px solid black;background-color:yellow;'>Requested Check Out</td>");
                strb.Append(@"<td style='border:1px solid black;background-color:yellow;'>Remarks</td>");
                strb.Append(@"<td style='border:1px solid black;background-color:yellow;'>Cleaning Timing</td>");
                strb.Append(@"<td style='border:1px solid black;background-color:yellow;'>Status</td>");

                foreach (DataRow dr in data.Rows)
                {
                    strb.Append("<tr>");
                    strb.Append(@"<td style='border-bottom:1px solid black;border-right:1px solid black'>" + Convert.ToString(dr[3]) + @"</td>");
                    strb.Append(@"<td style='border-bottom:1px solid black;border-right:1px solid black'>" + Convert.ToString(dr[7]) + @"</td>");
                    strb.Append(@"<td style='border-bottom:1px solid black;border-right:1px solid black'>" + Convert.ToString(dr[8]) + @"</td>");
                    strb.Append(@"<td style='border-bottom:1px solid black;border-right:1px solid black'>" + Convert.ToDateTime(dr[5]).ToShortTimeString() + @"</td>");
                    strb.Append(@"<td style='border-bottom:1px solid black;border-right:1px solid black'>" + Convert.ToDateTime(dr[6]).ToShortTimeString() + @"</td>");
                    strb.Append(@"<td style='border-bottom:1px solid black;border-right:1px solid black'>" + Convert.ToString(dr[11]) + @"</td>");
                    if (Convert.ToString(dr[10]) != "")
                    {
                        strb.Append(@"<td style='border-bottom:1px solid black;border-right:1px solid black'>" + Convert.ToDateTime(dr[10]).ToShortTimeString() + @"</td>");
                    }
                    else
                    {
                        strb.Append(@"<td style='border-bottom:1px solid black;border-right:1px solid black'></td>");
                    }
                    bool isnotstatus = true;
                    foreach (DataRow d in statuscode_data.Rows)
                    {
                        if (Convert.ToString(d[1]) == Convert.ToString(dr[9]))
                        {
                            strb.Append(@"<td style='border-bottom:1px solid black;border-right:1px solid black'>" + Convert.ToString(d[1]) + @"</td>");
                            isnotstatus = false;
                            break;
                        }
                    }
                    if (isnotstatus)
                    {
                        strb.Append(@"<td style='border-bottom:1px solid black;border-right:1px solid black'>Not Specified</td>");
                    }
                    strb.Append(@"</tr>");
                }
                strb.Append(@"</Table>");
                return strb.ToString();
            } }

        EmailAddress from = new EmailAddress("airplus@kustotech.in", "airplus");
        public string subject = "Reminder Check In - Check Out";
        EmailAddress to = new EmailAddress("saran@kustotech.in", "saran");
        EmailAddress cc = new EmailAddress("siva@kustotech.in", "siva");
        public GuestList()
        {
            Guests = new List<Guest>();
            client = new SendGridClient(apiKey);
        }
        public async Task SendMail()
        {
            var msg = MailHelper.CreateSingleEmail(from, to, subject, "Hi", Message);
            msg.AddCc(cc);
            var response = await client.SendEmailAsync(msg);
        }

        public async Task SendMailASAP()
        {
            var msg = MailHelper.CreateSingleEmail(from, to, "DAILY Check In - Check Out", "Hi", MessageForDay);
            msg.AddCc(cc);
            var response = await client.SendEmailAsync(msg);
        }
        public void UpdateTable()
        {
            if (Guests.Count() > 0)
            {
                try
                {
                    SqlConnection connection = new SqlConnection(ConnString);
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("InsertUpdateGuestList", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlParameter param = cmd.Parameters.AddWithValue("@Guest", GuestTable);
                    param.SqlDbType = SqlDbType.Structured;
                    param.TypeName = "dbo.GUESTPROPERTYTYPETABLE";
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
                catch (Exception e)
                {
                    //Logging
                }
            }
        }
        public void ProcessIcal(string calendarInfo,long ListingId)
        {
            var calobjects = Calendar.Load(calendarInfo);
            var events = calobjects.Events;
            foreach(CalendarEvent e in events)
            {
                if(e.Location is null || e.DtStart.Date < DateTime.Now.AddMonths(-2))   //If No Guest or Guest stay was before 2 months from now
                {
                    continue;
                }
                Guest g = new Guest();
                g.StartDate= e.DtStart.Date;
                g.EndDate = e.DtEnd.Date;
                
                //Take only valid part of name
                string[] Names = e.Summary.Split(' '); 
                if (!String.IsNullOrEmpty(Names[0]))
                {
                    g.FirstName = Names[0];
                }
                for(int i = 0; i < Names.Length - 1; i++)
                {
                    g.FullName += Names[i];
                    if (i == Names.Length - 1)
                    {
                        break;
                    }
                    g.FullName += " ";
                }

                //Take PHONE and EMAIL
                string[] values = e.Description.Split('\n');
                bool phoneCheck = false;
                bool emailCheck = false;
                foreach (string s in values)
                {
                    if (s.IndexOf(":") > -1)
                    {
                        string[] keyvalue = s.Split(':');
                        switch (keyvalue[0].ToLower())
                        {
                            case "phone":
                                if (!String.IsNullOrEmpty(keyvalue[1])  && keyvalue[1].TrimStart(' ').IndexOf("+")==0)
                                {
                                    g.Phone = keyvalue[1].Trim();
                                }
                                phoneCheck = true;
                                break;
                            case "email":
                                if (!String.IsNullOrEmpty(keyvalue[1]) && keyvalue[1].Trim().IndexOf("@") >0)
                                {
                                    g.Email = keyvalue[1].Trim();
                                }
                                emailCheck = true;
                                break;
                            default:
                                continue;
                        }
                        if (phoneCheck && emailCheck) break;
                    }
                }

                g.ListingId = ListingId;
                Guests.Add(g);
            }
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
                if (docAvailable=="False")
                {
                    string startDate = doc["reservation"]["start_date"];
                    string endDate = doc["reservation"]["end_date"];
                    Guest guest = new Guest();
                    guest.FullName = doc["reservation"]["guest"]["full_name"];
                    guest.FirstName = doc["reservation"]["guest"]["first_name"];
                    guest.AirplusId = Convert.ToInt64(doc["reservation"]["guest"]["id"]);
                    guest.ListingId= Convert.ToInt64(doc["reservation"]["hosting_id"]);
                    guest.StartDate = Convert.ToDateTime(startDate);
                    guest.EndDate = Convert.ToDateTime(endDate);
                    guest.RequestedStartDate = guest.StartDate.AddHours(16);
                    guest.RequestedEndDate = guest.EndDate.AddHours(11);
                    
                    Guest another = new Guest();
                    another.AirplusId = 0;
                    if (Guests.Count()==0)
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
                HttpResponseMessage response = await client.PostAsync("https://api.pushed.co/1/push", requestContent);

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
            }

            catch (Exception)
            {
            }
        }
    }
}
