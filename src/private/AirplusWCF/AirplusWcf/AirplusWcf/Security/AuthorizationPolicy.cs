using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IdentityModel.Policy;
using System.IdentityModel.Claims;
using System.IdentityModel;
using System.Security.Principal;

namespace AirplusWcf.Security
{
    public class AuthorizationPolicy : IAuthorizationPolicy
    {
        public ClaimSet Issuer;

        //public string Id ="";
        Guid _id = Guid.NewGuid();
        public string id;
        public bool Evaluate(EvaluationContext evaluationContext, ref object state)
        {
            // get the authenticated client identity
            IIdentity client = GetClientIdentity(evaluationContext);
            // set the custom principal
            evaluationContext.Properties["Principal"] = new CustomPrincipal(client);
            return true;
            
        }
        private IIdentity GetClientIdentity(EvaluationContext evaluationContext)
        {
            object obj;
            if (!evaluationContext.Properties.TryGetValue("Identities", out obj))
                throw new Exception("No Identity found");
            IList<IIdentity> identities = obj as IList<IIdentity>;
            if (identities == null || identities.Count <= 0)
                throw new Exception("No Identity found");
            return identities[0];
        }
    }
}