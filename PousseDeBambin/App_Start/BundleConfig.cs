using System.Web;
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
            "~/Scripts/jquery-ui-{version}.js",
            "~/Scripts/jquery.ui.datepicker-fr.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*",
                        "~/Scripts/localization/messages_fr.js",
                        "~/Scripts/localization/methods_fr.js"));

            bundles.Add(new ScriptBundle("~/bundles/carousel").Include(
                        "~/Scripts/carousel.js"));

            bundles.Add(new ScriptBundle("~/bundles/star").Include(
                        "~/Scripts/jquery.rateit.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Styles/css").Include(
                        "~/Content/css/main.css",
                        "~/Content/css/connexionBar.css",
                        "~/Content/css/header.css",
                        "~/Content/css/categories.css",
                        "~/Content/css/howItWorks.css",
                        "~/Content/css/footer.css",
                        "~/Content/css/createList.css",
                        "~/Content/css/searchList.css",
                        "~/Content/css/whoWeAre.css",
                        "~/Content/css/giftsList.css",
                        "~/Content/css/consultList.css",
                        "~/Content/css/modalBootstrap.css",
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
                        "~/Content/css/jquery.ui.theme.css",
                        "~/Content/css/bootstrap.css",
                        "~/Content/css/carousel.css",
                        "~/Content/rateit.css",
                        "~/Content/zopim.css"));

            bundles.IgnoreList.Clear();
        }
    }
}
