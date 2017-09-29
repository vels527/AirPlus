using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using AirplusWeb.AirPlusReference;

namespace AirplusWeb
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

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
            try
            {
                var a = sc.GetData(5);
            }
            catch
            {

            }
        }
    }
}