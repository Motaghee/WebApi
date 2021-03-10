using System.Web;
using System.Web.Optimization;
using BundleTransformer.Core.Builders;
using BundleTransformer.Core.Bundles;
using BundleTransformer.Core.Orderers;
using BundleTransformer.Core.Resolvers;

namespace QCManagement
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery")
                .Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.min.js",
                      "~/Scripts/mysidebar.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/mysidebar.css",
                      "~/Content/mystyles.css",
                       "~/Content/css/main.css"
                      ));

            //bundles.Add(new StyleBundle("~/Content/css").Include(
            //          "~/Content/bootstrap.css",
            //          "~/Content/mysidebar.css",
            //          "~/Content/mystyles.css",
            //          "~/Content/css/main.css"
            //          ));



            //Style Bundles
            bundles.Add(new StyleBundle("~/bundles/skin").Include(
          "~/Scripts/skins.js"));


            bundles.Add(
                new Bundle("~/bundles/LoginJS")
                    .Include("~/vendor/jquery/jquery-3.2.1.min.js")
                    .Include("~/vendor/animsition/js/animsition.min.js")
                    .Include("~/vendor/bootstrap/js/popper.js")
                    .Include("~/vendor/bootstrap/js/bootstrap.min.js")
                    .Include("~/vendor/select2/select2.min.js")
                    .Include("~/vendor/daterangepicker/moment.min.js")
                    .Include("~/vendor/daterangepicker/daterangepicker.js")
                    .Include("~/vendor/countdowntime/countdowntime.js")
                    .Include("~/Scripts/login.js")
                );

            bundles.Add(new StyleBundle("~/Content/LoginCSS").Include(
                "~/Content/fonts/font-awesome-4.7.0/css/font-awesome.min.css",
                "~/Content/fonts/Linearicons-Free-v1.0.0/icon-font.min.css",
                "~/vendor/animate/animate.css",
                "~/vendor/css-hamburgers/hamburgers.min.css",
                "~/vendor/animsition/css/animsition.min.css",
                "~/vendor/select2/select2.min.css",
                "~/vendor/daterangepicker/daterangepicker.css",
                "~/Content/login.css",
                "~/Content/util.css"
            ));

  
        }
    }
}
