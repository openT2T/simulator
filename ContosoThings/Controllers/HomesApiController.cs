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
        
    }
}
