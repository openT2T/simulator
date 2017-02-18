using ContosoThingsCore;
using ContosoThingsCore.Providers;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
namespace ContosoThings.Controllers
{
    [SimpleAuthorization]
    public class HubsApiController : ApiController
    {
        internal IHubContext hub;
        public HubsApiController() : base()
        {
            // load the signalr hub which is used to signal to the webui something has changed
            hub = GlobalHost.ConnectionManager.GetHubContext("NotificationHub");
        }

        /// <summary>
        /// Forces the in memory cache to load the data in table storage
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/forceRefresh")]
        public HttpResponseMessage ForceRefresh()
        {
            HubManager.Instance.Refresh();

            string hostName = ((System.Web.HttpContextWrapper)this.Request.Properties["MS_HttpContext"]).Request.UserHostName;

            return Request.CreateResponse(hostName);
        }

        // GET: api/HubsApi
        public IEnumerable<ContosoThingsCore.Hub> Get()
        {
            return HubManager.Instance.Hubs;
        }

        public object GetHub(string id)
        {
            return HubManager.Instance.GetHub(id);
        }

        public object GetThing(string id, string deviceId)
        {
            ContosoThingsCore.Hub h = HubManager.Instance.GetHub(id);
            return h.GetDevice(deviceId);
        }

        //public object GetService(string id, string deviceId, string serviceName)
        //{
        //    ContosoThingsCore.Hub h = HubManager.Instance.GetHub(id);
        //    return h.GetServiceValue(deviceId, serviceName);
        //}

        [HttpPost]
        [Route("api/addThing")]
        public void AddThing(HttpRequestMessage request)
        {
            string body = request.Content.ReadAsStringAsync().Result;
            dynamic toAdd = Newtonsoft.Json.JsonConvert.DeserializeObject(body);

            ContosoThingsCore.Hub h = HubManager.Instance.GetHub(toAdd.hubId.Value);

            ThingsBase thingToAdd = null;
            if (toAdd.thingType == 0)
            {
                thingToAdd = new ContosoSwitch(toAdd.name.Value);
            }
            else if (toAdd.thingType == 1)
            {
                thingToAdd = new ContosoLight(toAdd.name.Value);
            }
            else if (toAdd.thingType == 2)
            {
                thingToAdd = new ContosoLightDimmable(toAdd.name.Value);
            }
            else if (toAdd.thingType == 3)
            {
                thingToAdd = new ContosoLightColor(toAdd.name.Value);
            }
            else if (toAdd.thingType == 4)
            {
                thingToAdd = new ContosoThermostat(toAdd.name.Value);
            }

            // add new thing to hub
            h.AddThing(thingToAdd);

            // save the hub to table storage
            HubManager.Instance.Save(h);

            // refresh the web ui
            hub.Clients.All.refresh();
        }

        [HttpPost]
        [Route("api/removeThing")]
        public void RemoveThing(HttpRequestMessage request)
        {
            string body = request.Content.ReadAsStringAsync().Result;
            dynamic toRemove = Newtonsoft.Json.JsonConvert.DeserializeObject(body);

            ContosoThingsCore.Hub h = HubManager.Instance.GetHub(toRemove.hubId.Value);

            // remove the thing from the hub
            h.RemoveThing(toRemove.thingId.Value);

            // save the hub to table storage
            HubManager.Instance.Save(h);

            // refresh the web ui
            hub.Clients.All.refresh();
        }

        /// <summary>
        /// Takes in a request object with json content to update the value.
        /// postData = {
        ///     id: hubId,
        ///     deviceId: thing.Id,
        ///     propertyName: propertyName,
        ///     value: value
        /// };
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public object SetValue(HttpRequestMessage request)
        {
            string body = request.Content.ReadAsStringAsync().Result;
            dynamic toChange = Newtonsoft.Json.JsonConvert.DeserializeObject(body);

            string t = String.Format("hub {0}, device {1}, setting {2} to {3}", toChange.id, toChange.deviceId, toChange.propertyName, toChange.value);
            System.Diagnostics.Debug.WriteLine(t);

            // update the hub
            ContosoThingsCore.Hub h = HubManager.Instance.GetHub(toChange.id.ToString());
            ThingsBase newThing = h.Control(toChange.deviceId.ToString(), toChange.propertyName.ToString(), toChange.value.Value);

            // refresh the web ui
            hub.Clients.All.refresh();

            // update hub in storage
            TableStorageProvider.AddHub(h);

            return newThing;
        }
    }
}
