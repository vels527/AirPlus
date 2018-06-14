using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebAirplus
{
    public partial class LogOut : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["Authenticate"] = false;
            Session["UserName"] = null;
            Session["Email"] = null;
            Response.Redirect("Login.aspx");
        }
    }
}