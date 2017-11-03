using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using MongoDataLayer;
using System.Security.Permissions;


namespace AirplusWcf
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {



        public Document FetchListings(string Month, string Listing)
        {
            return MongoCQRS.FetchListings(Month, Listing);
        }

        public void RegisterListings(string uname, string primaryListing, string[] listings)
        {
            MongoCQRS.RegisterListings(uname, primaryListing, listings);            
        }

        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        //public bool ValidateUser(string uname,string upass)
        //{
        //    return false;
        //}

        public CompositeType RegisterUser(string uname,string upass,string email,string question,string answer)
        {
            CompositeType ct = new CompositeType();
            if (uname == null || upass==null || email ==null || question == null|| answer==null)
            {
                ct.StringValue = "Null value cannot be accepted";
                ct.BoolValue = false;
            }
            else if (uname == "" || upass == "" || email == "" || question == "" || answer == "")
            {
                ct.StringValue = "Empty value cannot be accepted";
                ct.BoolValue = false;
            }
            else
            {
                bool mesg=MongoCQRS.RegisterUser(uname, upass, email, question, answer);
                if(mesg)
                {
                    ct.StringValue = "User Created";
                    ct.BoolValue = true;
                }
                else
                {
                    ct.StringValue = "User already exists with name : "+uname;
                    ct.BoolValue = false;
                }
            }
            return ct;
        }
    }
}
