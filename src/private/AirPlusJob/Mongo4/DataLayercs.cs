using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System;

namespace Mongo4
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
        public static void SaveToDB(long ListingID, DateTime calendardate, bool isAvailable, decimal price)
        {
            connection.Open();
            SqlCommand cmd = new SqlCommand("SAVEAVAILABILITY", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SQLParameter("@LISTINGID", ListingID));
            cmd.Parameters.Add(new SQLParameter("@CALENDARDATE", calendardate));
            cmd.Parameters.Add(new SQLParameter("@ISAVAILABLE", isAvailable));
            cmd.Parameters.Add(new SQLParameter("@PRICE", price));
            cmd.ExecuteNonQuery();
            connection.Close();
        }
    }
}
