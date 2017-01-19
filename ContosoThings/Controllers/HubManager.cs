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

        private HubManager()
        {
            Hubs = new List<Hub>();

            Refresh();
        }

        public void Refresh()
        {
            Hubs.Clear();

            List<Hub> hubs = TableStorageProvider.GetAllHubs();
            Hubs.AddRange(hubs);
        }

        public List<Hub> Hubs { get; }

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