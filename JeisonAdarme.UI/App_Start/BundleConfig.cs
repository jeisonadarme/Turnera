using System.Web;
using System.Web.Optimization;

namespace JeisonAdarme.UI
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(

                "~/Scripts/Unobtrusive/jquery.unobtrusive*",
                "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Content/js/bootstrap.js",
                      "~/Content/js/Loading/jquery.showLoading.js",
                      "~/Scripts/jquery.qrcode-0.12.0.js"
                      ));


            bundles.Add(new ScriptBundle("~/bundles/free").Include(
                       "~/Content/js/classie.js",
                      "~/Content/js/cbpAnimatedHeader.js",
                      "~/Content/js/jqBootstrapValidation.js",
                      "~/Content/js/freelancer.js"
                ));

            bundles.Add(new StyleBundle("~/Content/Styles").Include(
                      "~/Content/css/bootstrap.css",
                      "~/Content/font-awesome/css/font-awesome.css",
                      "~/Content/css/freelancer.css",
                      "~/Content/css/Loading/showLoading.css",
                      "~/Content/css/jquery-ui.css"
                      ));
        }
    }
}
