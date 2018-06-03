using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAirplus
{
    public class GuestInsert
    {
        public int GuestId;
        public int PropertyId;
        public int HostId;
        public int status;
        public string Remarks;
        public DateTime? RequestedCheckIn;
        public DateTime? RequestedCheckOut; 
        public DateTime? CheckOutCleaning;
    }
}