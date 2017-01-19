using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace ContosoThings.Controllers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class SimpleAuthorizationAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            //ContosoThingsCore.Hub h = new ContosoThingsCore.Hub(((System.Web.HttpContextWrapper)actionContext.Request.Properties["MS_HttpContext"]).Request.UserHostName);
            //ContosoThingsCore.Providers.TableStorageProvider.AddHub(h);

            //// local requests are ok
            //if (actionContext.RequestContext.IsLocal || ((System.Web.HttpContextWrapper)actionContext.Request.Properties["MS_HttpContext"]).Request.UserHostName.Contains("contosothings.azurewebsites.net"))
            //{
            //    return;
            //}

            //string authKeyword = System.Configuration.ConfigurationManager.AppSettings["AuthKeyword"];
            //if (!actionContext.Request.Headers.Contains("Authorization"))
            //{
            //    HandleUnauthorizedRequest(actionContext);
            //}

            //IEnumerable<string> authHeaders = actionContext.Request.Headers.GetValues("Authorization");
            //if (!authHeaders.ElementAt(0).Equals(authKeyword))
            //{
            //    HandleUnauthorizedRequest(actionContext);
            //}

            //System.Diagnostics.Debug.WriteLine(actionContext.ToString());
            ////return true;
        }

        protected override void HandleUnauthorizedRequest(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            var challengeMessage = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
            challengeMessage.Headers.Add("WWW-Authenticate", "Basic");
            throw new HttpResponseException(challengeMessage);

        }
    }
}