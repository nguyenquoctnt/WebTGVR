using Microsoft.AspNet.SignalR;
using System;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using VDMMutiline.Ultilities;

namespace VDMMutiline.WebBackEnd
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // Make long polling connections wait a maximum of 110 seconds for a
            // response. When that time expires, trigger a timeout command and
            // make the client reconnect.
            GlobalHost.Configuration.ConnectionTimeout = TimeSpan.FromSeconds(110);

            // Wait a maximum of 30 seconds after a transport connection is lost
            // before raising the Disconnected event to terminate the SignalR connection.
            GlobalHost.Configuration.DisconnectTimeout = TimeSpan.FromSeconds(60);

            // For transports other than long polling, send a keepalive packet every
            // 10 seconds. 
            // This value must be no more than 1/3 of the DisconnectTimeout value.
            GlobalHost.Configuration.KeepAlive = TimeSpan.FromSeconds(10);



            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            if (!HttpContext.Current.Request.Url.Host.Contains("localhost"))
            {
                var url = Request.Url.AbsoluteUri;
                Uri myUri = new Uri(url);
                var ip = Dns.GetHostAddresses(myUri.Host).Length > 0 ? Dns.GetHostAddresses(myUri.Host)[0].ToString() : "";

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

            //UriBuilder builder = new UriBuilder(Request.Url);
            //if (!Request.Url.Host.StartsWith("www") && !Request.Url.IsLoopback 
            //    && Request.Url.Host.ToLower() == "timvenhanh.com" 
            //    && builder.Host.ToLower().Contains("https"))
            //{
            //    builder.Host = "www." + Request.Url.Host;
            //    Response.Redirect(builder.ToString().Replace("https", "http").Replace(":443", ""), true);
            //}
        }
    }
}
