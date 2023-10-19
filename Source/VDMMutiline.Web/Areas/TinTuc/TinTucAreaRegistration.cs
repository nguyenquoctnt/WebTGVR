using System.Web.Mvc;

namespace VDMMutiline.WebBackEnd.Areas.TinTuc
{
    public class TinTucAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "TinTuc";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "TinTuc_default",
                "TinTuc/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}