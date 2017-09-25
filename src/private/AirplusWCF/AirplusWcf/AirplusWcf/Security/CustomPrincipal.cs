using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Principal;
using System.Security;

namespace AirplusWcf.Security
{
    public class CustomPrincipal : IPrincipal
    {
        public IIdentity Identity;

        public CustomPrincipal(IIdentity client)
        {

        }

        public bool IsInRole(string role)
        {
            return true;
        }
    }
}