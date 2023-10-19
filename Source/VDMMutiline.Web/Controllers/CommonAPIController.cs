using System.Linq;
using System.Web.Mvc;
using VDMMutiline.Core;

namespace VDMMutiline.WebBackEnd.Controllers
{
    [AllowAnonymous]
    public class CommonAPIController : Controller
    {
        public ActionResult GetTinhthanhbyQuocgia(int? _Idquocgia)
        {
            var list = GetlistCommon.GetlisttinhthanhByQuocgia(_Idquocgia ?? 0);
            var listvalue = (from n in list
                             select new
                             {
                                 Value = n.Id,
                                 Text = n.Tentinh,
                             }).ToList();
            return Json(new { listvalue }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Gethuyenthibytinhthanh(int? _Idtinhthanh)
        {
            var list = GetlistCommon.Gethuyenthibytinhthanh(_Idtinhthanh ?? 0);
            var listvalue = (from n in list
                             select new
                             {
                                 Value = n.Id,
                                 Text = n.Tenhuyen,
                             }).ToList();
            return Json(new { listvalue }, JsonRequestBehavior.AllowGet);
        }
      
    }
}