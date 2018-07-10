using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;


namespace WebAirplus
{
    public class UserAuthenticate
    {
        public bool Authenticated=false;
        public DataSet UserData;
    }
    public static class Datalayer
    {
        private static SqlConnection conn;
        
        static Datalayer()
        {
#if DEBUG
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["GuestConnectionString"].ConnectionString);
#else
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["ProdConnectionString"].ConnectionString);
#endif            
        }
        public static UserAuthenticate Authenticate(string username,string password)
        {
            conn.Open();
            DataSet ds = new DataSet("User");
            SqlDataAdapter da = new SqlDataAdapter();
            SqlCommand cmd = new SqlCommand("AuthenticateUser", conn);
            UserAuthenticate user = new UserAuthenticate();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@username", SqlDbType.VarChar, 100));
            cmd.Parameters.Add(new SqlParameter("@pass", SqlDbType.VarChar, 100));
            cmd.Parameters[0].Value = username;
            cmd.Parameters[1].Value = password;
            da.SelectCommand = cmd;
            da.Fill(ds);
            user.UserData = ds;
            if(Convert.ToInt32(ds.Tables[0].Rows[0][0])<1)
            {
                user.Authenticated = false;
            }
            else
            {
                user.Authenticated = true;
            }

            conn.Close();

            return user;
        }
        public static DataSet GetUserList(string user)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds=new DataSet("Users");
            conn.Open();
            SqlCommand cmd = new SqlCommand("GetGuestsList", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@user", SqlDbType.VarChar, 100));
            cmd.Parameters[0].Value = user;
            da.SelectCommand = cmd;
            da.Fill(ds);
            conn.Close();

            return ds;
        }
        public static DataSet GetUserListHistory(string user)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet("Users");
            conn.Open();
            SqlCommand cmd = new SqlCommand("GetGuestsListHistory", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@user", SqlDbType.VarChar, 100));
            cmd.Parameters[0].Value = user;
            da.SelectCommand = cmd;
            da.Fill(ds);
            conn.Close();
            return ds;
        }
        public static DataSet GetSettings(string user)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet("Settings");
            conn.Open();
            SqlCommand cmd = new SqlCommand("GetSettings", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@user", SqlDbType.VarChar, 100));
            cmd.Parameters[0].Value = user;
            da.SelectCommand = cmd;
            da.Fill(ds);
            conn.Close();
            return ds;
        }
        public static void UpdateSettings(Host host)
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("UpdateUserSettings", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@FullName", SqlDbType.VarChar, 250));
            cmd.Parameters[0].Value = host.FullName;
            cmd.Parameters.Add(new SqlParameter("@FirstName", SqlDbType.VarChar, 150));
            cmd.Parameters[1].Value = host.FirstName;
            cmd.Parameters.Add(new SqlParameter("@LastName", SqlDbType.VarChar, 150));
            cmd.Parameters[2].Value = host.LastName;
            cmd.Parameters.Add(new SqlParameter("@username", SqlDbType.VarChar, 100));
            cmd.Parameters[3].Value = host.username;
            cmd.Parameters.Add(new SqlParameter("@Age", SqlDbType.Int));
            cmd.Parameters[4].Value = host.Age;
            cmd.Parameters.Add(new SqlParameter("@Email", SqlDbType.VarChar, 250));
            cmd.Parameters[5].Value = host.Email;
            cmd.Parameters.Add(new SqlParameter("@Phone", SqlDbType.VarChar, 250));
            cmd.Parameters[6].Value = host.Phone;
            DataTable dt = new DataTable("LISTTYPETABLE");
            DataColumn column;

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.Int32");
            column.ColumnName = "Property_Id";
            dt.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.Int32");
            column.ColumnName = "ListingId";
            dt.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "PropertyAddress";
            dt.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "ICSURL";
            dt.Columns.Add(column);

            foreach (Listing L in host.Listings)
            {
                DataRow row = dt.NewRow();
                row["Property_Id"]=L.Id;
                row["ListingId"] = L.ListingId;
                if(L.PropertyAddress == null)
                {
                    row["PropertyAddress"] = DBNull.Value;
                }
                else
                {
                    row["PropertyAddress"] = L.PropertyAddress;
                }
                if (L.IcalUrl == null)
                {
                    row["ICSURL"] = DBNull.Value;
                }
                else
                {
                    row["ICSURL"] = L.IcalUrl;
                }
                dt.Rows.Add(row);
            }
            cmd.Parameters.Add(new SqlParameter("@List", SqlDbType.Structured));
            cmd.Parameters[7].Value = dt;
            cmd.Parameters[7].TypeName = "dbo.LISTTYPETABLE";
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        public static void UpdateGuestProperty(DataTable dt)
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("UpdateGuestProperty", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlParameter param = cmd.Parameters.AddWithValue("@Guest",dt);
            param.SqlDbType = SqlDbType.Structured;
            param.TypeName = "dbo.GUESTTYPETABLE";
            cmd.ExecuteNonQuery();
            conn.Close();
            //return false;
        }
    }
}