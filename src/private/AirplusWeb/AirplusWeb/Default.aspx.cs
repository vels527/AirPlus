using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataModel;
using MongoDB;
using MongoDB.Driver;
using MongoDB.Driver.Core;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

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
                    string Month;
                    switch (DateTime.Now.Month)
                    {
                        case 1:
                            Month = "Jan";
                            break;
                        case 2:
                            Month = "Feb";
                            break;
                        case 3:
                            Month = "Mar";
                            break;
                        case 4:
                            Month = "Apr";
                            break;
                        case 5:
                            Month = "May";
                            break;
                        case 6:
                            Month = "Jun";
                            break;
                        case 7:
                            Month = "Jul";
                            break;
                        case 8:
                            Month = "Aug";
                            break;
                        case 9:
                            Month = "Sep";
                            break;
                        case 10:
                            Month = "Oct";
                            break;
                        case 11:
                            Month = "Nov";
                            break;
                        case 12:
                            Month = "Dec";
                            break;
                        default:
                            Month = null;
                            break;
                    }
                    AirPlusReference.Document document = sc.FetchListings(Month, "8175972");
                    
                    

                    //string[] listings = { "1", "2" };
                    //sc.RegisterListings("vels527","1",listings);
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