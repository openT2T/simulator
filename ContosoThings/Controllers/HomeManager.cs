using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ContosoThingsCore;
using System.Web.Http;
using System.Web.Mvc;

namespace ContosoThings.Controllers
{
    public class HomeManager : Controller
    {
        static HomeManager instance = new HomeManager();
        public static HomeManager Instance { get { return instance; } }

        private HomeManager()
        {
            Homes = new List<Hub>();
            
            string defaultHomeFileName = System.Web.HttpContext.Current.Server.MapPath("~/Content/testdata/home.json");
            string defaultHomeJson = System.IO.File.ReadAllText(defaultHomeFileName);
            Hub h = Hub.Load(defaultHomeJson);
            Homes.Add(h);
        }

        public List<Hub> Homes { get; }

        public Hub GetHub(string hubId)
        {
            foreach (Hub h in Homes)
            {
                if (h.Id == hubId)
                {
                    return h;
                }
            }

            return null;
        }
    }
}