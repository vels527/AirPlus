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