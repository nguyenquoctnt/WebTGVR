using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using VDMMutiline.Biz;
using VDMMutiline.SharedComponent.EntityInfo;
using VDMMutiline.SharedComponent.Params;
namespace VDMMutiline.Core.Common
{
    public class SiteMuti
    {
        public static string Getsitename()
        {
            string site = HttpContext.Current.Request.Url.Host;
            if (site.StartsWith("m."))
            {
                var value = site.Remove(0, 2);
                return value;
            }
            return site;
        }
        public static AspNetUserInfo getinfouserbydomain()
        {
            var param = new AspNetUsersParam() { UrlDomain= Getsitename() };
            var biz = new AspNetUsersBiz();
            biz.GetInfobydomain(param);
            return param.AspNetUsersInfo;
        }
    }
}
