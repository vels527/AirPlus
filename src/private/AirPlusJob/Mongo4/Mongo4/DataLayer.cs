using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

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
        public static void SaveToDB(long ListingID,DateTime dateTime,bool IsAvailable,decimal price,int propertyId)
        {
            connection.Open();
            SqlCommand command = new SqlCommand("SAVEAVAILABILITY", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@LISTINGID", ListingID));
            command.Parameters.Add(new SqlParameter("@CALENDARDATE", dateTime));
            command.Parameters.Add(new SqlParameter("@ISAVAILABLE", IsAvailable));
            command.Parameters.Add(new SqlParameter("@PRICE", price));
            command.Parameters.Add(new SqlParameter("@PROPERTYID",propertyId));
            command.ExecuteNonQuery();
            connection.Close();
        }
        public static DataTable GetListings()
        {
            DataSet dataSet_Listings = new DataSet();
            connection.Open();
            SqlDataAdapter daListings = new SqlDataAdapter();
            SqlCommand command = new SqlCommand("GetListings",connection);
            command.CommandType = CommandType.StoredProcedure;
            //command.EndExecuteNonQuery
            daListings.SelectCommand = command;
            daListings.Fill(dataSet_Listings);
            connection.Close();
            return dataSet_Listings.Tables[0];
        }
    }

}
