using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using AirplusWeb.AirPlusReference;
using System.Net;

namespace AirplusWeb
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //HttpCookie cookie = Request.Cookies["Credentials"];
            //AirPlusReference.Service1Client sc = new AirPlusReference.Service1Client();
            //if (cookie == null)
            //{
                
            //}
            //else
            //{                
            //    sc.ClientCredentials.UserName.UserName = cookie["UserName"];
            //    sc.ClientCredentials.UserName.Password = cookie["Password"];
            //    try
            //    {
            //        var a = sc.GetData(5);
            //        Response.Redirect("Default.aspx");
            //    }
            //    catch
            //    {

            //    }
            //}
        }

        protected void CreateUserWizard1_CreatedUser(object sender, EventArgs e)
        {

        }

        protected void ContinueButton_Click(object sender, EventArgs e)
        {

        }

        protected void StepNextButton_Click(object sender, EventArgs e)
        {
            //string uname = CreateUserWizard1.UserName;
            //string upass = CreateUserWizard1.Password;
            //string email = CreateUserWizard1.Email;
            //string squestion = CreateUserWizard1.Question;
            //string sanswer = CreateUserWizard1.Answer;
            //AirPlusReference.IService1 svc = new AirPlusReference.Service1Client();
            //var ct = svc.RegisterUser(uname, upass, email, squestion, sanswer);
            //if (ct.BoolValue)
            //{
            //    CreateUserWizard1.DuplicateUserNameErrorMessage = "User Name Already present";
            //    CreateUserWizard1.UserName = "";
            //}

        }

        protected void UserName_TextChanged(object sender, EventArgs e)
        {

        }
        protected void Loginbtn_click(object sender,EventArgs e)
        {
            AirPlusReference.Service1Client sc = new AirPlusReference.Service1Client();
            sc.ClientCredentials.UserName.UserName =LoginUsertxt.Text;
            sc.ClientCredentials.UserName.Password = LoginPasswordtxt.Text;
            HttpCookie cookie = Request.Cookies["Credentials"];
            if (cookie == null)
            {
                cookie = new HttpCookie("Credentials");
                cookie["UserName"] = LoginUsertxt.Text;
                cookie["Password"] = LoginPasswordtxt.Text;
                
            }
            try
            {
                var a = sc.GetData(5);
                Response.Cookies.Add(cookie);
                Response.Redirect("Default.aspx");
            }
            catch(Exception exp)
            {
                throw exp;
            }
        }

        protected void Button_Register_Click(object sender, EventArgs e)
        {
            string uname = TextBox_User.Text;
            string upass = TextBox_Password.Text;
            string email = TextBox_Email.Text;
            string squestion = TextBox_Question.Text;
            string sanswer = TextBox_Answer.Text;
            AirPlusReference.Service1Client svc = new AirPlusReference.Service1Client();
            //svc.ClientCredentials.UserName.UserName = "siva05";
            //svc.ClientCredentials.UserName.Password = "billyPo0$";
            var ct = svc.RegisterUser(uname, upass, email, squestion, sanswer);
            if (ct.BoolValue)
            {
                Label_Result.Text = "User Registered";
            }
            else
            {
                TextBox_User.Text = "";
                Label_Result.Text = "User Name already present";
            }
        }
    }
}