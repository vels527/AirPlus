using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebAirplus;

namespace WebAirplus
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            GridView1.DataSource = Datalayer.GetUserList(Convert.ToString(Session["UserName"]));            
            GridView1.DataBind();
        }
    }
}