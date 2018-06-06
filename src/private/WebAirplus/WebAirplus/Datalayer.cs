using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;


namespace WebAirplus
{
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
        public static bool Authenticate(string username,string password)
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("AuthenticateUser", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@username", SqlDbType.VarChar, 100));
            cmd.Parameters.Add(new SqlParameter("@pass", SqlDbType.VarChar, 100));
            cmd.Parameters[0].Value = username;
            cmd.Parameters[1].Value = password;
            var returnParameter = cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
            returnParameter.Direction = ParameterDirection.ReturnValue;
            cmd.ExecuteNonQuery();
            int authvalue=(int)returnParameter.Value;

            conn.Close();
            if (authvalue > 0)
            {
               
                return true;
            }
            return false;
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