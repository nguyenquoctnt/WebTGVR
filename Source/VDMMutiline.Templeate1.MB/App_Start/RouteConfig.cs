using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace VDMMutiline.Templeate1.MB
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute("HomepageDefault", "", new { controller = "Home", action = "Index", name = UrlParameter.Optional });
            routes.MapRoute(name: "Sitemap", url: "sitemap.xml", defaults: new { controller = "Sitemap", action = "Index" });

            routes.MapRoute(
      name: "guilienhe",
      url: "gui-lien-he",
      defaults: new { controller = "About", action = "Index", key = UrlParameter.Optional }
      );
            routes.MapRoute(
     name: "KhachSan",
     url: "tim-kiem-khach-san",
     defaults: new { controller = "KhachSan", action = "Index", key = UrlParameter.Optional }
     );

            routes.MapRoute(
         name: "vedadat",
         url: "ve-da-dat",
         defaults: new { controller = "QuanLyVeDaDat", action = "Index", key = UrlParameter.Optional }
         );
            routes.MapRoute(
       name: "GuiLaiHanhTrinh",
       url: "gui-lai-hanh-trinh",
       defaults: new { controller = "KiemTraVe", action = "GuiLaiHanhTrinh", code = UrlParameter.Optional }
       );
            routes.MapRoute(
     name: "kiemtrave",
     url: "kiem-tra-ve",
     defaults: new { controller = "KiemTraVe", action = "Index", code = UrlParameter.Optional }
     );
            routes.MapRoute(
          name: "vesaphethan",
          url: "ve-sap-het-han",
          defaults: new { controller = "QuanLyVeDaDat", action = "Expired", key = UrlParameter.Optional }
          );

            routes.MapRoute(
          name: "vedatthatbai",
          url: "ve-dat-that-bai",
          defaults: new { controller = "QuanLyVeDaDat", action = "Fail", key = UrlParameter.Optional }
          );
            routes.MapRoute(
       name: "chitietve",
       url: "chi-tiet-ve",
       defaults: new { controller = "QuanLyVeDaDat", action = "Create", key = UrlParameter.Optional }
       );

            routes.MapRoute(
              name: "lstbaiviet",
              url: "ds-bai-viet/{ChuyenMuc}",
              defaults: new { controller = "FontEndnew", action = "Index", ChuyenMuc = UrlParameter.Optional }
          );

            routes.MapRoute(
             name: "timve",
             url: "tim-ve",
             defaults: new
             {
                 controller = "SeachHome",
                 action = "SeachMain"
             }
             );
            routes.MapRoute(
            name: "Bookticket",
            url: "dat-ve",
            defaults: new
            {
                controller = "SeachHome",
                action = "Bookticket"
            }
            );

            routes.MapRoute(
          name: "book",
          url: "book",
          defaults: new
          {
              controller = "SeachHome",
              action = "Index"
          }
          );

            routes.MapRoute(
     name: "FontEndnewchitiet",
     url: "bai-viet/{key}",
     defaults: new { controller = "FontEndnew", action = "ChiTiet", key = UrlParameter.Optional }
     );
        }
    }
}
