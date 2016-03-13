﻿using System.Web;
using System.Web.Optimization;

namespace Driver.WebSite
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Resources/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Resources/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Resources/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Resources/Scripts/bootstrap.js",
                      "~/Resources/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/knockout").Include("~/Resources/Scripts/knockout-3.4.0.js"));

            bundles.Add(new ScriptBundle("~/bundles/utils").Include("~/Scripts/Utils.js"));


            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Resources/Styles/bootstrap.css",
                      "~/Styles/site.css"));
        }
    }
}
