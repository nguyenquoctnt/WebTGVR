using System.Web.Mvc;

namespace VDMMutiline.WebBackEnd.Areas.WalletManager
{
    public class WalletManagerAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "WalletManager";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "WalletManager_default",
                "WalletManager/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}