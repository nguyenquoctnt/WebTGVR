using System.Web;
using System.Web.Mvc;

namespace VDMMutiline.Templeate1.MB
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
