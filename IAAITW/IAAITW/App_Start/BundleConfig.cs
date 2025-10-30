using System.Web;
using System.Web.Optimization;

namespace IAAITW
{
    public class BundleConfig
    {
        // 如需統合的詳細資訊，請瀏覽 https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // 使用開發版本的 Modernizr 進行開發並學習。然後，當您
            // 準備好可進行生產時，請使用 https://modernizr.com 的建置工具，只挑選您需要的測試。
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new Bundle("~/bundles/bootstrap").Include(
                      //"~/Scripts/bootstrap.js"
                      ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      //"~/Content/bootstrap.css",
                      "~/Content/site.css"));

            //前台css
            bundles.Add(new StyleBundle("~/Front/css").Include(
                      "~/css/bootstrap.min.css","~/css/style.css",
                      "~/css/responsive.css", 
                      "~/css/animate.css","~/css/owl.carousel.min.css",
                      "~/css/owl.theme.css",
                      
                      "~/css/colorbox.css"
                      
                      ));

            //前台js
            bundles.Add(new ScriptBundle("~/bundles/Front").Include(
                      "~/js/jquery.js",
                      "~/js/bootstrap.min.js",
                      "~/js/owl.carousel.min.js",
                      "~/js/jquery.counterup.min.js",
                      "~/js/waypoints.min.js",
                      "~/js/html5shiv.js",
                      "~/js/jquery.colorbox.js",
                      "~/js/isotope.js",
                      "~/js/ini.isotope.js",
                      "~/js/gmap3.min.js",
                      "~/js/custom.js", 
                      "~/js/respond.min.js",
                      "~/js/wow.min.js"
                      ));

            // 後台 JavaScript bundle
            bundles.Add(new ScriptBundle("~/bundles/back").Include(
                "~/Back/vendors/js/vendor.bundle.base.js",
                "~/Back/js/off-canvas.js",
                "~/Back/js/template.js",
                "~/Back/js/settings.js",
                "~/Back/js/todolist.js"
            ));

            // 後台 Plugin JavaScript bundle
            bundles.Add(new ScriptBundle("~/bundles/back-plugins").Include(
                "~/Back/vendors/chart.js/chart.umd.js",
                "~/Back/vendors/datatables.net/jquery.dataTables.js",
                "~/Back/vendors/datatables.net-bs5/dataTables.bootstrap5.js",
                "~/Back/js/dataTables.select.min.js",
                "~/Back/js/jquery.cookie.js",
                "~/Back/js/dashboard.js"
            ));
        }
    }
}
