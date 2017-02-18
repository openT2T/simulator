using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Security;

namespace ContosoThings.Controllers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class SimpleAuthorizationAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            // forms auth
            if (actionContext.RequestContext.Principal.Identity.IsAuthenticated)
            {
                return;
            }

            string authKeyword = System.Configuration.ConfigurationManager.AppSettings["AuthKeyword"];
            if (!actionContext.Request.Headers.Contains("Authorization"))
            {
                HandleUnauthorizedRequest(actionContext);
            }

            IEnumerable<string> authHeaders = actionContext.Request.Headers.GetValues("Authorization");
            if (!authHeaders.ElementAt(0).Equals(authKeyword))
            {
                HandleUnauthorizedRequest(actionContext);
            }

        }

        protected override void HandleUnauthorizedRequest(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            var challengeMessage = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
            throw new HttpResponseException(challengeMessage);

        }
    }
}