using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using RazorEngine;
using RazorEngine.Templating;

namespace AirbnbGuestList
{
    public static class DataLayer
    {
        static SqlConnection connection;

        static DataLayer()
        {
            connection = new SqlConnection(ConnString);
        }
        private static string ConnString
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
        public static DataTable GetProperties()
        {
            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet("PropertyList");
            SqlCommand cmd = new SqlCommand("GetPropertyList", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            da.SelectCommand = cmd;
            da.Fill(ds);
            connection.Close();
            return ds.Tables[0];
        }
        public static DataTable GuestTable(List<Guest> Guests)
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
                dr["FullName"] = g.FullName;
                dr["FirstName"] = g.FirstName;
                dr["ListingID"] = g.ListingId;
                dr["CHECKIN"] = g.StartDate;
                dr["CHECKOUT"] = g.EndDate;

                dr["Email"] = g.Email ?? (Object)DBNull.Value;

                dr["Phone"] = g.Phone ?? (Object)DBNull.Value;

                dt.Rows.Add(dr);
            }
            return dt;
        }
        public static string MessageForNotification(long ListingId)
        {
            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet("Users");
            SqlCommand cmd = new SqlCommand("GetGuestsListForToday", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@Listing", SqlDbType.Int));
            cmd.Parameters[0].Value = ListingId;
            da.SelectCommand = cmd;
            da.Fill(ds);
            DataSet dataset = ds;
            DataTable data = dataset.Tables[0];
            DataTable data_1 = dataset.Tables[1];
            StringBuilder strb = new StringBuilder();

            if (data.Rows.Count > 0) //If empty no message
            {
                strb.Append("Check IN for ");
            }


            foreach (DataRow dr in data.Rows)
            {
                strb.Append(Convert.ToString(dr[3]));
                strb.Append(" on ");
                strb.Append(Convert.ToString(dr[7]));
                strb.Append(" check out on ");
                strb.Append(Convert.ToString(dr[8]));

                if (Convert.ToString(dr[10]) != "")
                {
                    strb.Append(" with cleaning at ");
                    strb.Append(Convert.ToDateTime(dr[10]).ToShortTimeString());
                }

                if (Convert.ToString(dr[11]) != "")
                {
                    strb.Append(" Remarks: ");
                    strb.Append(Convert.ToString(dr[11]));
                }
                strb.Append("\n");
            }
            if (data_1.Rows.Count > 0) //If empty no message
            {
                strb.Append("Check Out for ");
            }


            foreach (DataRow dr in data_1.Rows)
            {
                strb.Append(Convert.ToString(dr[3]));
                strb.Append(" on ");
                strb.Append(Convert.ToString(dr[8]));

                strb.Append(" whose check in was on ");
                strb.Append(Convert.ToString(dr[7]));

                if (Convert.ToString(dr[10]) != "")
                {
                    strb.Append(" with cleaning at ");
                    strb.Append(Convert.ToDateTime(dr[10]).ToShortTimeString());
                }

                if (Convert.ToString(dr[11]) != "")
                {
                    strb.Append(" Remarks: ");
                    strb.Append(Convert.ToString(dr[11]));
                }
                strb.Append("\n");
            }

            connection.Close();
            return strb.ToString();
        }
        public static string HeadingTemplate
        {
            get
            {
                StringBuilder strbHeading = new StringBuilder();
                strbHeading.Append("<P>@Model.Heading");
                strbHeading.Append("<Table>");
                strbHeading.Append("<tr>");
                strbHeading.Append(@"<td style='border:1px solid black;background-color:yellow;'>Guest Name</td>");
                strbHeading.Append(@"<td style='border:1px solid black;background-color:yellow;'>Check In</td>");
                strbHeading.Append(@"<td style='border:1px solid black;background-color:yellow;'>Check Out</td>");
                strbHeading.Append(@"<td style='border:1px solid black;background-color:yellow;'>Requested Check In</td>");
                strbHeading.Append(@"<td style='border:1px solid black;background-color:yellow;'>Requested Check Out</td>");
                strbHeading.Append(@"<td style='border:1px solid black;background-color:yellow;'>Remarks</td>");
                strbHeading.Append(@"<td style='border:1px solid black;background-color:yellow;'>Cleaning Timing</td>");
                strbHeading.Append(@"<td style='border:1px solid black;background-color:yellow;'>Status</td>");
                strbHeading.Append(@"</tr>");
                return strbHeading.ToString();
            }
        }
        public static string MessageTemplate
        {
            get
            {
                StringBuilder strbTemplate = new StringBuilder();
                strbTemplate.Append("<tr>");
                strbTemplate.Append(@"<td style='border-bottom:1px solid black;border-right:1px solid black'>@Model.Name</td>");
                strbTemplate.Append(@"<td style='border-bottom:1px solid black;border-right:1px solid black'>@Model.CheckIn</td>");
                strbTemplate.Append(@"<td style='border-bottom:1px solid black;border-right:1px solid black'>@Model.CheckOut</td>");
                strbTemplate.Append(@"<td style='border-bottom:1px solid black;border-right:1px solid black'>@Model.RCheckIn</td>");
                strbTemplate.Append(@"<td style='border-bottom:1px solid black;border-right:1px solid black'>@Model.RCheckOut</td>");
                strbTemplate.Append(@"<td style='border-bottom:1px solid black;border-right:1px solid black'>@Model.Remarks</td>");
                strbTemplate.Append(@"<td style='border-bottom:1px solid black;border-right:1px solid black'>@Model.CleanTiming</td>");
                strbTemplate.Append(@"<td style='border-bottom:1px solid black;border-right:1px solid black'>@Model.Status</td>");
                strbTemplate.Append(@"</tr>");               
                return strbTemplate.ToString();
            }
        }
        private static int counter = 0;
        public static string GuestToObject(DataTable dataSet,DataTable StatusTable)
        {
            string GuestMessage = "";
            int i = counter++;
            foreach (DataRow dr in dataSet.Rows)
            {
                string statusOption = "Not Specified";
                foreach (DataRow d in StatusTable.Rows)
                {
                    if (Convert.ToString(d[1]) == Convert.ToString(dr[9]))
                    {
                        statusOption = Convert.ToString(d[1]);
                        break;
                    }
                }

                Object GuestDetail =  new { Name = Convert.ToString(dr[3]), CheckIn = Convert.ToString(dr[7]), CheckOut = Convert.ToString(dr[8]), RCheckIn = (Convert.ToString(dr[5]) != "" ? Convert.ToDateTime(dr[5]).ToShortTimeString() : ""), RCheckOut = (Convert.ToString(dr[6]) != "" ? Convert.ToDateTime(dr[6]).ToShortTimeString() : ""), Remarks = Convert.ToString(dr[11]), CleanTiming = Convert.ToString(dr[10]) != "" ? Convert.ToDateTime(dr[10]).ToShortTimeString() : "", Status = statusOption };
                GuestMessage += Engine.Razor.RunCompile(MessageTemplate, "MessageInKey" + i.ToString(), null, GuestDetail);
            }
            GuestMessage += @"</Table>";
            return GuestMessage;
        }
        public static string MessageForDay(long ListingId)
        {
            try
            {
                connection.Open();
                SqlDataAdapter da = new SqlDataAdapter();
                DataSet ds = new DataSet("Users");
                SqlCommand cmd = new SqlCommand("GetGuestsListForToday", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@Listing", SqlDbType.Int));
                cmd.Parameters[0].Value = ListingId;
                da.SelectCommand = cmd;
                da.Fill(ds);
                DataSet dataset = ds;
                DataTable data = dataset.Tables[0];
                DataTable data_1 = dataset.Tables[1];
                DataTable statuscode_data = dataset.Tables[2];
                connection.Close();
                
                string Heading = "";
                string completeMsg = "";

                if (data.Rows.Count <= 0 && data_1.Rows.Count<=0)
                {
                    return "";
                }

                
                Heading = HeadingTemplate;


                var ResultHeading = Engine.Razor.RunCompile(Heading, "MessageInKey", null, new { Heading = "Check In Details" });
                completeMsg = ResultHeading;

                if (data.Rows.Count > 0)
                {
                    completeMsg += GuestToObject(data,statuscode_data);
                }
                else
                {
                    completeMsg = "";
                }
                if (data_1.Rows.Count <= 0)
                {
                    return completeMsg;
                }
                var ResultCheckOut= Engine.Razor.RunCompile(Heading, "MessageOutKey", null, new { Heading = "Check Out Details" }); ;
                completeMsg += ResultCheckOut;
                completeMsg += GuestToObject(data_1,statuscode_data);
                return completeMsg;
            }
            catch (Exception e)
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }                
                string msg = "Error in DataLayer Message  for Day for Listing : "+ListingId;
                Logger.Error(msg,e);
                return "";
            }
        }
        public static string Message(long ListingId)
        {
            try{
                connection.Open();
                SqlDataAdapter da = new SqlDataAdapter();
                DataSet ds = new DataSet("Users");
                SqlCommand cmd = new SqlCommand("GetGuestsList", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@Listing", SqlDbType.Int));
                cmd.Parameters[0].Value = ListingId;
                da.SelectCommand = cmd;
                da.Fill(ds);
                DataSet dataset = ds;
                DataTable data = dataset.Tables[0];
                DataTable statuscode_data = dataset.Tables[1];
                StringBuilder strb = new StringBuilder();
                connection.Close();
                string Heading = "";
                string completeMsg = "";

                Heading = HeadingTemplate;

                var ResultHeading = Engine.Razor.RunCompile(Heading, "MessageKey", null, new { Heading = "Guest Details" });
                completeMsg = ResultHeading;
                completeMsg += GuestToObject(data,statuscode_data);
                return completeMsg;
            }
            catch(Exception ex)
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                string msg = "Error in DataLayer Message for Listing : " + ListingId;
                Logger.Error(msg, ex);
                return "";
            }
        }

        public static void UpdateTable(DataTable GuestTable)
        {
            connection.Open();
            SqlCommand cmd = new SqlCommand("InsertUpdateGuestList", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlParameter param = cmd.Parameters.AddWithValue("@Guest", GuestTable);
            param.SqlDbType = SqlDbType.Structured;
            param.TypeName = "dbo.GUESTPROPERTYTYPETABLE";
            cmd.ExecuteNonQuery();
            connection.Close();
        }

    }
}
