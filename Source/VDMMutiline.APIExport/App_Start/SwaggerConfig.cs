using System.Web.Http;
using WebActivatorEx;
using VDMMutiline.APIExport;
using Swashbuckle.Application;
using System.Web;
using System;
using System.Net.Http;

[assembly: System.Web.PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace VDMMutiline.APIExport
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;
            GlobalConfiguration.Configuration
                                 .EnableSwagger(c =>
                                 {
                                     c.SingleApiVersion("v1", "VDM API");
                                     c.RootUrl(ResolveBasePath);
                                     c.IncludeXmlComments(string.Format(@"{0}\bin\VDMMutiline.APIExport.xml",
                                     System.AppDomain.CurrentDomain.BaseDirectory));
                                     c.DescribeAllEnumsAsStrings();
                                     //c.ApiKey("apiKey") //First ApiKey
                                     //    .Description("API Key Authentication")
                                     //    .Name("UserKey")
                                     //    .In("header");
                                 })
                                .EnableSwaggerUi(c => { c.DisableValidator();
                                    //c.EnableApiKeySupport("UserKey", "header");
                                });

        }
        private static string ResolveBasePath(HttpRequestMessage message)
        {
            var strPathAndQuery = HttpContext.Current.Request.Url.PathAndQuery;
            var strUrl = HttpContext.Current.Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");
            var virtualPathRoot = message.GetRequestContext().VirtualPathRoot;
            return new Uri(new Uri(strUrl, UriKind.Absolute), virtualPathRoot).AbsoluteUri;
        }

    }
}
