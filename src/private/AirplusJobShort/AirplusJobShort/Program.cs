using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
using System.Net;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace AirplusJobShort
{
    class Program
    {
        static void Main(string[] args)
        {
            Program program = new Program();
            GuestList guestList = new GuestList();
            guestList.SendMail().Wait();
        }
    }
    public class Guest
    {

        public string FirstName { get; set; }
        public string FullName { get; set; }
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
    public class Property
    {

    }
    public class GuestList
    {
        public List<Guest> Guests;
        string apiKey = Environment.GetEnvironmentVariable("SENDSEND");
        SendGridClient client;
        public string Message
        {
            get
            {
                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Prod"].ConnectionString);
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

        EmailAddress from = new EmailAddress("siva@kustotech.in", "siva");
        public string subject = "DAILY STATUS Check In - Check Out";
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
        
        
    }
}
