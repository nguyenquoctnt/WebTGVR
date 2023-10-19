using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VDMMutiline.Biz;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.Params;

namespace VDMMutiline.WebBackEnd.Areas.Sys.Controllers
{
  //  [eCommerce.Core.Attribute.eCommerceAuthorize(Roles = "Settingsystem")]
    public class SettingSystemController : Controller
    {
        public ActionResult Settingsystem()
        {
            var dao = new SysSettingBiz();
            var param = new SettingSystemParam();
            dao.GetAll(param);
            return View(param);
        }
        [HttpPost]
        public ActionResult Settingsystem(SettingSystemParam param)
        {
            try
            {
                var dao = new SysSettingBiz();
                foreach (var item in param.SettingSystemInfos)
                {
                  
                    var ck = new SysSettingSystem()
                    {
                        Giatri = item.Giatri,
                        Ngaysua = DateTime.Now,
                        ID = item.ID,
                    };
                    param.SysSettingSystem = ck;
                    dao.Update(param);
                }
                dao.GetAll(param);
                return View(param);
            }
            catch
            {
                return View(param);
            }
        }

    }
}