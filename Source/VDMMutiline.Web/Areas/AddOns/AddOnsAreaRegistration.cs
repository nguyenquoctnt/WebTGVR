using System.Web.Mvc;

namespace VDMMutiline.WebBackEnd.Areas.AddOns
{
    public class AddOnsAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "AddOns";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "AddOns_default",
                "AddOns/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}