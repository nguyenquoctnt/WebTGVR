using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VDMMutiline.Ultilities;

namespace VDMMutiline.WebBackEnd.Controllers
{
    public class APIController : Controller
    {
        // GET: API
        public ActionResult SearchAirport(string key)
        {
            var list = Station.SearchAirport(key);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }
}