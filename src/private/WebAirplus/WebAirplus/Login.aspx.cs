using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebAirplus
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Submit_btn_Click(object sender, EventArgs e)
        {
            if (User_Txt.Text == "" || Pass_Txt.Text == "")
            {
                error_lbl.Text = "Please enter all necessary details";
                return;
            }
            bool Authenticated = Datalayer.Authenticate(User_Txt.Text, Pass_Txt.Text);
            if (Authenticated)
            {
                Session["Autheenticate"] = Authenticated;
                Session["UserName"] = User_Txt.Text;
                Response.Redirect("Default.aspx");
            }
        }
    }
}
