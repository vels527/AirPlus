//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Security.Principal;
//using System.Security;

//namespace AirplusWcf
//{
//    public class CustomPrincipal : IPrincipal
//    {
//        public IIdentity Identity {
//            get { return _identity; }
//        }
//        private IIdentity _identity;
//        public CustomPrincipal(IIdentity client)
//        {
//            _identity = client;
//        }

//        public bool IsInRole(string role)
//        {
//            var _roles = new string[1];
//            if (Identity.Name == "test1")
//                _roles = new string[1] { "ADMIN" };
//            else
//                _roles = new string[1] { "USER" };
//            return _roles.Contains(role);
//        }
//    }
//}