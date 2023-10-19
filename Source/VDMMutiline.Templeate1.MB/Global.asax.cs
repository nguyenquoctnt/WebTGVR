using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using VDMMutiline.Biz;
using VDMMutiline.SharedComponent.EntityInfo;
using VDMMutiline.SharedComponent.Params;
using VDMMutiline.Ultilities;

namespace VDMMutiline.Templeate1.MB
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            var url = Request.Url.AbsoluteUri;
            Uri myUri = new Uri(url);
            var ip = System.Net.Dns.GetHostAddresses(myUri.Host).Length > 0 ? System.Net.Dns.GetHostAddresses(myUri.Host)[0].ToString() : "";
            if (ip != "127.0.0.1")
            {
                var dao = new Dao.HeThong.AliarDao();
                var obj = dao.checkIP();
                if (obj == null)
                {
                    if (!HttpContext.Current.Request.Url.Host.Contains("localhost") && !HttpContext.Current.Request.Url.AbsoluteUri.Contains("Indexedit"))
                    {
                        HttpContext.Current.RewritePath("/license.aspx");
                    }
                }
                else
                {
                    var domain = obj.Value;
                    var domainon = obj.KeyOn;
                    try
                    {
                        if (!string.IsNullOrEmpty(domain))
                            domain = Utility.Decrypt(domain, true);
                        if (!string.IsNullOrEmpty(domainon))
                            domainon = Utility.Decrypt(domainon, true);
                    }
                    catch
                    {

                    }
                    if (ip != domain && !HttpContext.Current.Request.Url.Host.Contains("localhost") && !HttpContext.Current.Request.Url.AbsoluteUri.Contains("Indexedit"))
                    {
                        HttpContext.Current.RewritePath("/license.aspx");
                    }
                }
            }
            var cookie = Request.Cookies["Language"];
            if (cookie == null)
            {
                Utility.SetCookie("Language", "Vi", 30);
            }
            string lang = Utility.GetCookie("Language");
            string culture = string.Empty;
            string curencode = "";
            if (lang.ToLower().CompareTo("Vi") == 0 || string.IsNullOrEmpty(culture))
            {
                culture = "Vi";
            }
            if (lang.ToLower().CompareTo("en-us") == 0)
            {
                culture = "en-us";
            }
            curencode = culture;

            var list = new List<SettingUserInfo>();
            var param = new AspNetUsersParam() { UrlDomain = Core.Common.SiteMuti.Getsitename() };
            var biz = new AspNetUsersBiz();
            biz.GetInfobydomain(param);
            var objuserbydomain = param.AspNetUsersInfo;
            if (objuserbydomain != null)
            {
                if (!Request.IsSecureConnection &&
                                        Request.Url.AbsoluteUri.Contains("localhost") == false)
                {
                    if ((Core.Common.SiteMuti.Getsitename() == objuserbydomain.UrlDomain2 && objuserbydomain.IsSSLDomain2MB == true))
                    {
                        Response.Redirect(Request.Url.AbsoluteUri.Replace("http://", "https://"));
                    }
                    else if (Core.Common.SiteMuti.Getsitename() == objuserbydomain.UrlDomain3 && objuserbydomain.IsSSLDomain3MB == true)
                    {
                        Response.Redirect(Request.Url.AbsoluteUri.Replace("http://", "https://"));
                    }

                }
            }
        }
        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            Response.Clear();
            HttpException httpException = exception as HttpException;
            //if (HttpContext.Current.Request.Url.AbsoluteUri == "https://thegioivere.net/lam-dai-ly-ve-may-bay-cap-2")
            //    HttpContext.Current.Response.Redirect("https://thegioivere.net/lam-dai-ly-ve-may-bay-cap-2-1398");
            //else
            //{

            //}
            //string a = HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.PathAndQuery, "");
            //HttpContext.Current.Response.Redirect(a);

        }
    }
}
