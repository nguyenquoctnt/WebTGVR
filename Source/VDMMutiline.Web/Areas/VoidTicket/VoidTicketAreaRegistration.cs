using System.Web.Mvc;

namespace VDMMutiline.WebBackEnd.Areas.VoidTicket
{
    public class VoidTicketAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "VoidTicket";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "VoidTicket_default",
                "VoidTicket/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}