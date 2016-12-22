using ContosoThingsCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ContosoThings.Controllers
{
    public class HomesApiController : ApiController
    {
        // GET: api/HomesApi
        public IEnumerable<Hub> Get()
        {
            return HomeManager.Instance.Homes;
        }

        public object GetHub(string id)
        {
            return HomeManager.Instance.GetHub(id);
        }

        public object GetService(string id, string deviceId, string serviceName)
        {
            Hub h = HomeManager.Instance.GetHub(id);
            return h.GetServiceValue(deviceId, serviceName);
        }

        [HttpPost]
        public object SetValue(HttpRequestMessage request)
        {
            string body = request.Content.ReadAsStringAsync().Result;
            dynamic toChange = Newtonsoft.Json.JsonConvert.DeserializeObject(body);

            string t = String.Format("home {0}, device {1}, setting {2} to {3}", toChange.id, toChange.deviceId, toChange.propertyName, toChange.value);

            System.Diagnostics.Debug.WriteLine(t);
            
            Hub h = HomeManager.Instance.GetHub(toChange.id.ToString());
            ThingsBase newThing = h.Control(toChange.deviceId.ToString(), toChange.propertyName.ToString(), toChange.value.Value);
            return newThing;
        }
    }
}
