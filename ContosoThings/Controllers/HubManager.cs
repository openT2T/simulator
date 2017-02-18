using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ContosoThingsCore;
using ContosoThingsCore.Providers;
using System.Web.Http;
using System.Web.Mvc;

namespace ContosoThings.Controllers
{
    public class HubManager : Controller
    {
        static HubManager instance = new HubManager();
        public static HubManager Instance { get { return instance; } }

        internal Microsoft.AspNet.SignalR.IHubContext hub;
        private HubManager()
        {
            // load the signalr hub which is used to signal to the webui something has changed
            hub = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext("NotificationHub");

            Refresh();
        }

        /// <summary>
        /// Refreshes the in memory cache with the data in table storage
        /// </summary>
        public void Refresh()
        {
            Refresh(TimeSpan.Zero);
        }

        /// <summary>
        /// Refreshes the in memory cache with the data in table storage
        /// </summary>
        public void RefreshDefaultWait()
        {
            // by default allow only two minute old data
            Refresh(TimeSpan.FromMinutes(2));
        }

        /// <summary>
        /// Refreshes the in memory cache with the data in table storage
        /// </summary>
        /// <param name="ageAllowed"></param>
        public void Refresh(TimeSpan ageAllowed)
        {
            if (LastLoaded + ageAllowed < DateTime.Now)
            {
                this.hubs.Clear();

                List<ContosoThingsCore.Hub> hubs = TableStorageProvider.GetAllHubs();
                this.hubs.AddRange(hubs);

                LastLoaded = DateTime.Now;

                // refresh the web ui
                hub.Clients.All.refresh();
            }
        }

        public DateTime LastLoaded = DateTime.MinValue;

        protected List<Hub> hubs = new List<Hub>();
        public List<Hub> Hubs
        {
            get
            {
                RefreshDefaultWait();
                return this.hubs;
            }
        }

        public Hub GetHub(string hubId)
        {
            foreach (Hub h in Hubs)
            {
                if (h.Id == hubId)
                {
                    return h;
                }
            }

            return null;
        }

        public void Save(Hub h)
        {
            TableStorageProvider.AddHub(h);
        }
    }
}