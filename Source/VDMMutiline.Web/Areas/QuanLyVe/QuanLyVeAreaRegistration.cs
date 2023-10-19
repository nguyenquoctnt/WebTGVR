using System.Web.Mvc;

namespace VDMMutiline.WebBackEnd.Areas.QuanLyVe
{
    public class QuanLyVeAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "QuanLyVe";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "QuanLyVe_default",
                "QuanLyVe/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}