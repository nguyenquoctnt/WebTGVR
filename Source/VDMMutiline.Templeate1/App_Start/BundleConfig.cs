using System.Web;
using System.Web.Optimization;

namespace VDMMutiline.Templeate1
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/FontEnd/jquery").Include(
                   "~/Content/FontEnd/js/jquery-1.9.1.js",
                       "~/Content/FontEnd/Seach/js/jquery-ui-1.10.3.custom.js",
                  "~/Content/FontEnd/js/bootstrap.min.js"
                  ));

            bundles.Add(new StyleBundle("~/Content/FontEnd/css").Include(
                    "~/Content/FontEnd/css/bootstrap.min.css",
                    "~/Content/FontEnd/css/default.css",
                    "~/Content/FontEnd/css/font-awesome.min.css",
                    "~/Content/FontEnd/engine0/style.css"
                    ));
        }
    }
}
