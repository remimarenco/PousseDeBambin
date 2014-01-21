﻿using System.Web;
using System.Web.Optimization;

namespace PousseDeBambin
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
            "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*",
                        "~/Scripts/localization/messages_fr.js",
                        "~/Scripts/localization/methods_fr.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/css/jquery.ui.core.css",
                        "~/Content/css/jquery.ui.resizable.css",
                        "~/Content/css/jquery.ui.selectable.css",
                        "~/Content/css/jquery.ui.accordion.css",
                        "~/Content/css/jquery.ui.autocomplete.css",
                        "~/Content/css/jquery.ui.button.css",
                        "~/Content/css/jquery.ui.dialog.css",
                        "~/Content/css/jquery.ui.slider.css",
                        "~/Content/css/jquery.ui.tabs.css",
                        "~/Content/css/jquery.ui.datepicker.css",
                        "~/Content/css/jquery.ui.progressbar.css",
                        "~/Content/css/jquery.ui.theme.css"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/main.css"));
        }
    }
}
