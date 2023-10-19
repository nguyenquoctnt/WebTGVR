using System.Web.Mvc;

namespace VDMMutiline.WebBackEnd.Areas.CancelTicket
{
    public class CancelTicketAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "CancelTicket";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "CancelTicket_default",
                "CancelTicket/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}