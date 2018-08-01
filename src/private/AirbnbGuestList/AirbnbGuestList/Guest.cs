using System;

namespace AirbnbGuestList
{
    public class Guest
    {

        public string FirstName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime RequestedStartDate { get; set; }
        public DateTime RequestedEndDate { get; set; }
        public long AirplusId { get; set; }
        public long ListingId { get; set; }

        public Guest()
        {


        }
        public Guest(string Summary, string Description, DateTime startDate, DateTime endDate, long listingID)
        {
            try
            {
                string[] Names = Summary.Split(' ');
                if (!String.IsNullOrEmpty(Names[0]))
                {
                    FirstName = Names[0];
                }
                for (int i = 0; i < Names.Length - 1; i++)
                {
                    FullName += Names[i];
                    if (i == Names.Length - 1)
                    {
                        break;
                    }
                    FullName += " ";
                }
                //Take PHONE and EMAIL
                string[] values = Description.Split('\n');
                bool phoneCheck = false;
                bool emailCheck = false;
                foreach (string s in values)
                {
                    if (s.IndexOf(":") > -1)
                    {
                        string[] keyvalue = s.Split(':');
                        switch (keyvalue[0].ToLower())
                        {
                            case "phone":
                                if (!String.IsNullOrEmpty(keyvalue[1]) && keyvalue[1].TrimStart(' ').IndexOf("+") == 0)
                                {
                                    Phone = keyvalue[1].Trim();
                                }
                                phoneCheck = true;
                                break;
                            case "email":
                                if (!String.IsNullOrEmpty(keyvalue[1]) && keyvalue[1].Trim().IndexOf("@") > 0)
                                {
                                    Email = keyvalue[1].Trim();
                                }
                                emailCheck = true;
                                break;
                            default:
                                continue;
                        }
                        if (phoneCheck && emailCheck) break;
                    }
                }
                StartDate = startDate;
                EndDate = endDate;
                ListingId = listingID;
            }
            catch (Exception e)
            {
                string mesg = "Problem in listing: " + listingID + " with Name " + Summary;
                Logger.Error(mesg, e);
            }
        }
    }
}
