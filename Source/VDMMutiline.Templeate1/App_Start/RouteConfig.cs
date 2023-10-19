using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace VDMMutiline.Templeate1
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute("HomepageDefault", "", new { controller = "Home", action = "Index", name = UrlParameter.Optional });
            routes.MapRoute(name: "Sitemap", url: "sitemap.xml", defaults: new { controller = "Sitemap", action = "Index" });
            routes.MapRoute(name: "tracuuve", url: "tra-cuu-ve", defaults: new { controller = "KiemTraVe", action = "Index", code = UrlParameter.Optional });
            routes.MapRoute(name: "KhachSan", url: "tim-kiem-khach-san", defaults: new { controller = "KhachSan", action = "Index", key = UrlParameter.Optional });
            routes.MapRoute(name: "vedadat", url: "ve-da-dat", defaults: new { controller = "QuanLyVeDaDat", action = "Index", key = UrlParameter.Optional });
            routes.MapRoute(name: "Orderfontends", url: "Orderfontends", defaults: new { controller = "Orderfontend", action = "Index", key = UrlParameter.Optional });
            routes.MapRoute(name: "mailsubmit", url: "mailsubmitn", defaults: new { controller = "Control", action = "SubmitMail", key = UrlParameter.Optional });
            routes.MapRoute(name: "vesaphethan", url: "ve-sap-het-han", defaults: new { controller = "QuanLyVeDaDat", action = "Expired", key = UrlParameter.Optional });
            routes.MapRoute(name: "vedatthatbai", url: "ve-dat-that-bai", defaults: new { controller = "QuanLyVeDaDat", action = "Fail", key = UrlParameter.Optional });
            routes.MapRoute(name: "chitietve", url: "chi-tiet-ve", defaults: new { controller = "QuanLyVeDaDat", action = "Create", key = UrlParameter.Optional });
            routes.MapRoute(name: "timve", url: "tim-ve", defaults: new { controller = "SeachHome", action = "SeachMain" });
            routes.MapRoute(name: "khachsanlist", url: "khach-san", defaults: new { controller = "FontEndKhachsan", action = "Index", ChuyenMuc = UrlParameter.Optional });
            routes.MapRoute(name: "Bookticket", url: "dat-ve", defaults: new { controller = "SeachHome", action = "Bookticket" });
            routes.MapRoute(name: "book", url: "book", defaults: new { controller = "SeachHome", action = "Index" });
            routes.MapRoute(name: "FontEndnewchitiet", url: "bai-viet/{key}", defaults: new { controller = "FontEndnew", action = "ChiTiet", key = UrlParameter.Optional });
            routes.MapRoute(name: "listbaiviet", url: "ds-bai-viet/{ChuyenMuc}", defaults: new { controller = "FontEndnew", action = "Index", ChuyenMuc = UrlParameter.Optional });
        }
    }
}
