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
                "~/Scripts/jquery-1.12.4.js",
                "~/Scripts/jquery-ui-1.12.1.js",
                "~/Scripts/bootstrap.js",
                "~/Scripts/angular.js",
                "~/Scripts/angular-ui/ui-bootstrap-tpls.js",
                "~/Scripts/angular-ui.js",
                "~/Scripts/modernizr-*",
                "~/Scripts/jquery.signalR-2.2.1.min.js",
                "~/Scripts/slider.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/app").Include(
                ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                 "~/Content/themes/base/jquery-ui.min.css",
                 "~/Content/themes/base/*.css",
                 "~/Content/bootstrap.min.css",
                 "~/Content/font-awesome.min.css",
                 "~/Content/ui-slider.css",
                 "~/Content/Site.css"
                 ));
        }
    }
}
