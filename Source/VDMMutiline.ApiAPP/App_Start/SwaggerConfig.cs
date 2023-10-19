using System.Web.Http;
using Swashbuckle.Application;
using System.Net.Http;
using System;
using System.Web;
using VDMMutiline.ApiAPP;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace VDMMutiline.ApiAPP
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;
           
            GlobalConfiguration.Configuration
                                 .EnableSwagger(c =>
                                {
                                    c.SingleApiVersion("v1", "VDMMutilineAPI");
                                  //  c.RootUrl(ResolveBasePath);
                                    c.IncludeXmlComments(string.Format(@"{0}\bin\VDMMutiline.ApiAPP.xml",
                                    System.AppDomain.CurrentDomain.BaseDirectory));
                                    c.DescribeAllEnumsAsStrings();
                                })
                                .EnableSwaggerUi(c => c.DisableValidator());
           
        }
        private static string ResolveBasePath(HttpRequestMessage message)
        {
            var virtualPathRoot = message.GetRequestContext().VirtualPathRoot;
            var schemeAndHost = "https://" + message.RequestUri.Host;
            return new Uri(new Uri(schemeAndHost, UriKind.Absolute), virtualPathRoot).AbsoluteUri;
        }
    }
}
