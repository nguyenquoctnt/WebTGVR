using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VDMMutiline.Core.UI;

namespace VDMMutiline.WebBackEnd.Controllers
{
    public class NavbarController : BaseController
    {
        // GET: Navbar
        public ActionResult Index()
        {
            ViewBag.isadmin = IsAdmin();
            ViewBag.isbooker = GetlistRole().Contains(22);
            ViewBag.isbookerLL = GetlistRole().Contains(23);
            ViewBag.ParentIdHasValue = CurrentUser.ParentId.HasValue;
            return View(CurrentUser);
        }
    }
}