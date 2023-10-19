using System.Web.Optimization;

namespace VDMMutiline.WebBackEnd
{
    public static class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.UseCdn = true;   //enable CDN support


            bundles.Add(new StyleBundle("~/bundles/logincss").Include(
                 "~/Content/app-assets/css/vendors.css",
                 "~/Content/app-assets/vendors/css/forms/icheck/icheck.css",
                 "~/Content/app-assets/vendors/css/forms/icheck/custom.css",
                 "~/Content/app-assets/css/app.css",
                 "~/Content/app-assets/css/core/menu/menu-types/vertical-menu.css",
                 "~/Content/app-assets/css/core/colors/palette-gradient.css",
                 "~/Content/app-assets/css/pages/login-register.css",
                 "~/Content/assets/css/style.css"

                ));
            bundles.Add(new ScriptBundle("~/bundles/loginjs").Include(
              "~/Content/app-assets/vendors/js/vendors.min.js",
              "~/Content/app-assets/vendors/js/forms/validation/jqBootstrapValidation.js",
              "~/Content/app-assets/vendors/js/forms/icheck/icheck.min.js",
              "~/Content/app-assets/js/core/app-menu.js",
               "~/Content/app-assets/js/core/app.js",
                "~/Content/app-assets/js/scripts/forms/form-login-register.js"
                ));

            bundles.Add(new StyleBundle("~/bundles/css").Include(
                  "~/Content/app-assets/css/vendors.css",
                  "~/Content/app-assets/vendors/css/extensions/unslider.css",
                  "~/Content/app-assets/vendors/css/weather-icons/climacons.min.css",
                  "~/Content/app-assets/css/app.css",
                  "~/Content/app-assets/css/core/menu/menu-types/vertical-menu.css",
                  "~/Content/app-assets/css/core/colors/palette-gradient.css",
                  "~/Content/app-assets/css/core/colors/palette-gradient.css",
                  "~/Content/app-assets/css/core/menu/menu-types/vertical-menu.css",
                  "~/Content/app-assets/css/core/menu/menu-types/vertical-overlay-menu.css",
                  "~/Content/app-assets/css/plugins/calendars/clndr.css",
                    "~/Content/app-assets/fonts/meteocons/style.min.css",
                     "~/Content/app-assets/vendors/css/forms/icheck/icheck.css",
                          "~/Content/app-assets/fonts/icomoon.css",
                       "~/Content/assets/css/style.css",
                      "~/Content/bootstrap-toastr/toastr.min.css",
                  "~/Content/Selecttize/selectize.bootstrap3.css",

                  "~/Content/css/bootstrap-datepicker3.min.css",
                  "~/Content/css/bootstrap-timepicker.min.css",
                   //"~/Content/css/daterangepicker.min.css",
                   "~/Content/app-assets/css/plugins/forms/wizard.css",
                   "~/Content/app-assets/css/plugins/forms/wizard.min.css",
                     "~/Content/Popup/Popubcss.css"


                 ));
            bundles.Add(new ScriptBundle("~/bundles/Corejquery").Include(
                  "~/Content/app-assets/js/core/libraries/jquery.min.js"
                  ));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(

                     "~/Content/app-assets/vendors/js/vendors.min.js",
                     "~/Content/app-assets/js/core/libraries/bootstrap.min.js",
                      "~/Content/js/moment.min.js",
                    "~/Content/app-assets/vendors/js/extensions/underscore-min.js",
                    "~/Content/app-assets/js/core/app-menu.js",
                     "~/Content/app-assets/js/core/app.js",
                       "~/Content/app-assets/vendors/js/forms/icheck/icheck.min.js",
                    "~/Content/app-assets/js/scripts/customizer.js",
                     "~/Content/form-parsley/parsley.js",
                       "~/Scripts/VDMComon.js",
                         "~/Scripts/format.js",
                           "~/Content/js/bootstrap.min.js",
                     // "~/Content/js/bootstrap-datepicker.min.js",
                     //"~/Content/js/bootstrap-datetimepicker.min.js",
                    "~/Content/js/bootstrap-timepicker.min.js",
                     "~/Scripts/ckeditor/ckeditor.js",
                         "~/Content/Popup/jquery.bpopup.js",
                         "~/Scripts/jquery.number.min.js"



            ));
            bundles.Add(new ScriptBundle("~/bundles/datatable").Include(

             "~/Content/DataTables/DataTables-1.10.16/js/jquery.dataTables.min.js",
             "~/Content/DataTables/DataTables-1.10.16/js/dataTables.bootstrap.js"

         ));
            bundles.Add(new ScriptBundle("~/bundles/toastr").Include(
              "~/Content/bootstrap-toastr/toastr.min.js",
              "~/Content/bootstrap-toastr/ui-toastr.js",
              "~/Content/ion.sound/ion.sound.js",
              "~/Scripts/metronic.js"));
          
        }
    }
}
