using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAirplus
{
    public class Host
    {
        public string FullName;
        public string FirstName;
        public string LastName;
        public int Age;
        public string Email;
        public string Phone;
        public string username;
        public List<Listing> Listings;
        public Host()
        {
            Listings = new List<Listing>();
        }
    }
    public class Listing
    {
        public int Id;
        public string ListingId;
        public string PropertyAddress;
        public string IcalUrl;
    }
}