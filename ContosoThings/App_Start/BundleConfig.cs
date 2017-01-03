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
                "~/Scripts/bootstrap.js",
                "~/Scripts/angular.js",
                "~/Scripts/angular-ui/ui-bootstrap-tpls.js",
                "~/Scripts/modernizr-*",
                "~/Scripts/jquery.signalR-2.2.1.min.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/app").Include(
                ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                 "~/Content/bootstrap.min.css",
                 "~/Content/font-awesome.min.css",
                 "~/Content/Site.css"
                 ));
        }
    }
}
