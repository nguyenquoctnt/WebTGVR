using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VDMMutiline.WebBackEnd.Controllers
{
    public class AliarController : Controller
    {
        [AllowAnonymous]
        public ActionResult Indexedit()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Indexedit(string input1, string input2, string input3, string input4)
        {
            var dao = new Dao.HeThong.AliarDao();
            dao.update(input1, input2, input3, input4);
            return Redirect("/");
        }
    }
}