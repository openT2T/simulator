using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Optimization;

namespace ContosoThings
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                "~/Scripts/jquery-1.10.2.min.js",
                "~/Scripts/angular.min.js",
                "~/Scripts/bootstrap.js",
                "~/Scripts/modernizr-*"
                ));

            bundles.Add(new ScriptBundle("~/bundles/app").Include(
                "~/Scripts/app/index.js"
                ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                 "~/Content/bootstrap.min.css",
                 "~/Content/Site.css"));
        }
    }
}
