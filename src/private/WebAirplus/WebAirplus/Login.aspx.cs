using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace WebAirplus
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (Request.Browser.IsMobileDevice)
                MasterPageFile = "~/Site.Mobile.Master";
        }
        protected void Submit_btn_Click(object sender, EventArgs e)
        {
            if (User_Txt.Text == "" || Pass_Txt.Text == "")
            {
                error_lbl.Text = "Please enter all necessary details";
                return;
            }
            UserAuthenticate Authenticated = Datalayer.Authenticate(User_Txt.Text, Pass_Txt.Text);
            
            if (Authenticated.Authenticated)
            {
                DataTable dt = Authenticated.UserData.Tables[1];
                DataRow row = dt.Rows[0];
                Session["Authenticate"] = true;
                Session["UserName"] = row["UserName"];
                Session["Email"] = row["Email"];
                if (Request.Browser.IsMobileDevice)
                {
                    Response.Redirect("Default_Mobile.aspx");
                }
                else
                {
                    Response.Redirect("Default.aspx");
                }
            }
        }
    }
}
