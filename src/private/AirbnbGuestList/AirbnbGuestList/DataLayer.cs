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
                strbTemplate.Append(@"</Table>");
                return strbTemplate.ToString();
            }
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
                
                
                string Heading = "";
                string Template = "";
                string completeMsg = "";

                if (data.Rows.Count <= 0 && data_1.Rows.Count<=0)
                {
                    return "";
                }

                
                Heading = HeadingTemplate;
                Template = MessageTemplate;

                var ResultHeading = Engine.Razor.RunCompile(Heading, "MessageInKey", null, new { Heading = "Check In Details" });
                //return Result;
                completeMsg = ResultHeading;
                if (data.Rows.Count > 0)
                {
                    foreach (DataRow dr in data.Rows)
                    {
                        string statusOption = "Not Specified";
                        foreach (DataRow d in statuscode_data.Rows)
                        {
                            if (Convert.ToString(d[1]) == Convert.ToString(dr[9]))
                            {
                                statusOption = Convert.ToString(d[1]);
                                break;
                            }
                        }

                        var ResultCheckIn = Engine.Razor.RunCompile(Template, "MessageTemplate", null, new { Name = Convert.ToString(dr[3]), CheckIn = Convert.ToString(dr[7]), CheckOut = Convert.ToString(dr[8]), RCheckIn = (Convert.ToString(dr[5]) != "" ? Convert.ToDateTime(dr[5]).ToShortTimeString() : ""), RCheckOut = (Convert.ToString(dr[6]) != "" ? Convert.ToDateTime(dr[6]).ToShortTimeString() : ""), Remarks = Convert.ToString(dr[11]), CleanTiming = Convert.ToString(dr[10]) != "" ? Convert.ToDateTime(dr[10]).ToShortTimeString() : "", Status = statusOption });
                        completeMsg += ResultCheckIn;
                        //Result = Engine.Razor.Run("MessageKey",null,);
                    }
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
                foreach (DataRow dr in data_1.Rows)
                {
                    string statusOption = "Not Specified";
                    foreach (DataRow d in statuscode_data.Rows)
                    {
                        if (Convert.ToString(d[1]) == Convert.ToString(dr[9]))
                        {
                            statusOption = Convert.ToString(d[1]);
                            break;
                        }
                    }

                    var MsgCheckOut = Engine.Razor.RunCompile(Template, "MessageTemplateOut", null, new { Name = Convert.ToString(dr[3]), CheckIn = Convert.ToString(dr[7]), CheckOut = Convert.ToString(dr[8]), RCheckIn = (Convert.ToString(dr[5]) != "" ? Convert.ToDateTime(dr[5]).ToShortTimeString() : ""), RCheckOut = (Convert.ToString(dr[6]) != "" ? Convert.ToDateTime(dr[6]).ToShortTimeString() : ""), Remarks = Convert.ToString(dr[11]), CleanTiming = Convert.ToString(dr[10]) != "" ? Convert.ToDateTime(dr[10]).ToShortTimeString() : "", Status = statusOption });
                    completeMsg += MsgCheckOut;
                    //Result = Engine.Razor.Run("MessageKey",null,);
                }
                connection.Close();
                return completeMsg;
            }
            catch (Exception e)
            {
                connection.Close();
                string msg = "Error in DataLayer Message  for Day for Listing : "+ListingId;
                Logger.Error(msg,e);
                return "";
            }
            //strb.Append(@"</Table>");
            //strb.Append("<P>Check Out");
            //strb.Append("<Table>");
            //strb.Append("<tr>");
            //strb.Append(@"<td style='border:1px solid black;background-color:yellow;'>Guest Name</td>");
            //strb.Append(@"<td style='border:1px solid black;background-color:yellow;'>Check In</td>");
            //strb.Append(@"<td style='border:1px solid black;background-color:yellow;'>Check Out</td>");
            //strb.Append(@"<td style='border:1px solid black;background-color:yellow;'>Requested Check In</td>");
            //strb.Append(@"<td style='border:1px solid black;background-color:yellow;'>Requested Check Out</td>");
            //strb.Append(@"<td style='border:1px solid black;background-color:yellow;'>Remarks</td>");
            //strb.Append(@"<td style='border:1px solid black;background-color:yellow;'>Cleaning Timing</td>");
            //strb.Append(@"<td style='border:1px solid black;background-color:yellow;'>Status</td>");

            //foreach (DataRow dr in data_1.Rows)
            //{
            //    strb.Append("<tr>");
            //    strb.Append(@"<td style='border-bottom:1px solid black;border-right:1px solid black'>" + Convert.ToString(dr[3]) + @"</td>");
            //    strb.Append(@"<td style='border-bottom:1px solid black;border-right:1px solid black'>" + Convert.ToString(dr[7]) + @"</td>");
            //    strb.Append(@"<td style='border-bottom:1px solid black;border-right:1px solid black'>" + Convert.ToString(dr[8]) + @"</td>");

            //    if (Convert.ToString(dr[5]) != "")
            //    {
            //        strb.Append(@"<td style='border-bottom:1px solid black;border-right:1px solid black'>" + Convert.ToDateTime(dr[5]).ToShortTimeString() + @"</td>");
            //    }
            //    else
            //    {
            //        strb.Append(@"<td style='border-bottom:1px solid black;border-right:1px solid black'></td>");
            //    }

            //    if (Convert.ToString(dr[6]) != "")
            //    {
            //        strb.Append(@"<td style='border-bottom:1px solid black;border-right:1px solid black'>" + Convert.ToDateTime(dr[6]).ToShortTimeString() + @"</td>");
            //    }
            //    else
            //    {
            //        strb.Append(@"<td style='border-bottom:1px solid black;border-right:1px solid black'></td>");
            //    }


            //    strb.Append(@"<td style='border-bottom:1px solid black;border-right:1px solid black'>" + Convert.ToString(dr[11]) + @"</td>");
            //    if (Convert.ToString(dr[10]) != "")
            //    {
            //        strb.Append(@"<td style='border-bottom:1px solid black;border-right:1px solid black'>" + Convert.ToDateTime(dr[10]).ToShortTimeString() + @"</td>");
            //    }
            //    else
            //    {
            //        strb.Append(@"<td style='border-bottom:1px solid black;border-right:1px solid black'></td>");
            //    }
            //    bool isnotstatus = true;
            //    foreach (DataRow d in statuscode_data.Rows)
            //    {
            //        if (Convert.ToString(d[1]) == Convert.ToString(dr[9]))
            //        {
            //            strb.Append(@"<td style='border-bottom:1px solid black;border-right:1px solid black'>" + Convert.ToString(d[1]) + @"</td>");
            //            isnotstatus = false;
            //            break;
            //        }
            //    }
            //    if (isnotstatus)
            //    {
            //        strb.Append(@"<td style='border-bottom:1px solid black;border-right:1px solid black'>Not Specified</td>");
            //    }
            //    strb.Append(@"</tr>");
            //}
            //strb.Append(@"</Table>");

            
            //return strb.ToString();

        }
        public static string Message(long ListingId)
        {
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

                if (Convert.ToString(dr[5]) != "")
                {
                    strb.Append(@"<td style='border-bottom:1px solid black;border-right:1px solid black'>" + Convert.ToDateTime(dr[5]).ToShortTimeString() + @"</td>");
                }
                else
                {
                    strb.Append(@"<td style='border-bottom:1px solid black;border-right:1px solid black'></td>");
                }

                if (Convert.ToString(dr[6]) != "")
                {
                    strb.Append(@"<td style='border-bottom:1px solid black;border-right:1px solid black'>" + Convert.ToDateTime(dr[6]).ToShortTimeString() + @"</td>");
                }
                else
                {
                    strb.Append(@"<td style='border-bottom:1px solid black;border-right:1px solid black'></td>");
                }

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
            connection.Close();
            return strb.ToString();
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
