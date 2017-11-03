using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AirplusWeb
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpCookie cookie = Request.Cookies["Credentials"];
            AirPlusReference.Service1Client sc = new AirPlusReference.Service1Client();
            if (cookie == null)
            {
                Response.Redirect("Login.aspx");
            }
            else
            {
                //cookie = new HttpCookie("Credentials");
                sc.ClientCredentials.UserName.UserName = cookie["UserName"];
                sc.ClientCredentials.UserName.Password = cookie["Password"];
                try
                {
                    string[] listings = { "1", "2" };
                    sc.RegisterListings("vels527","1",listings);
                }
                catch
                {
                    Response.Redirect("Login.aspx");
                }
            }
        }

        protected void Chart1_Load(object sender, EventArgs e)
        {

        }
    }
}