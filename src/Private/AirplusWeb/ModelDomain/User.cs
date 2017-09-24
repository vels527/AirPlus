using System;
using System.Collections.Generic;
using System.Text;

namespace ModelDomain
{
    public class User
    {
        public int userid { get; set; }
        public string uname { get; set; }
        public string upass { get; set; }
        public string email { get; set; }
        //secret q and answer
        public string squestion { get; set; }
        public string sanswer { get; set; }
    }
}