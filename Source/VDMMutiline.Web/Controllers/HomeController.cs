using System;
using VDMMutiline.Core.UI;
using System.Web.Mvc;

using VDMMutiline.Core;
using VDMMutiline.SharedComponent;
using VDMMutiline.SharedComponent.Constants;

using VDMMutiline.SharedComponent.Models;


namespace VDMMutiline.WebBackEnd.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Booking", new { area = "QuanLyVe" });
        }
      
    }
}