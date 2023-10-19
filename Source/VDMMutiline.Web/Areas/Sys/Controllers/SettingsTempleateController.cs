using Microsoft.Web.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VDMMutiline.Biz;
using VDMMutiline.Biz.TempleteSetting;
using VDMMutiline.Core;
using VDMMutiline.Core.Common;
using VDMMutiline.Core.UI;
using VDMMutiline.SharedComponent.Constants;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
using VDMMutiline.SharedComponent.Params;
using VDMMutiline.SharedComponent.Params.TempleteSetting;

namespace VDMMutiline.WebBackEnd.Areas.Sys.Controllers
{
    public class SettingsTempleateController : BaseController
    {
        public ActionResult Index(int? IdUser)
        {
            TempleatePropertyGroupBiz _biz = new TempleatePropertyGroupBiz();
            var param = new TempleatePropertyGroupParam() { };
            var TempleatePropertyGroupFilter = new TempleatePropertyGroupFilter() { };
            param.TempleatePropertyGroupFilter = TempleatePropertyGroupFilter;
            _biz.Search(param);
            ViewBag.Data = param.TempleatePropertyGroupInfos;
            return View(IdUser);
        }

        public ActionResult UpdateSetting(int? Idgroup)
        {
            var param = new TempleatePropertyUserParam()
            {
                TempleatePropertyUserFilter = new TempleatePropertyUserFilter { UserId = CurrentUser.Id, GroupId = Idgroup },
            };
            var biz = new TempleatePropertyUserUserBiz();
            biz.SearchByUser(param);
            return View(param);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult UpdateSetting(TempleatePropertyUserParam param)
        {
            try
            {
                var biz = new TempleatePropertyUserUserBiz();
                foreach (var item in param.TempleatePropertyUserInfos)
                {
                    TempleatePropertyUserParam model = new TempleatePropertyUserParam();
                    model.TempleatePropertyUser = new tbl_TempleatePropertyUser
                    {
                        IdProperty = item.IdProperty,
                        IdUser = CurrentUser.Id,
                        Valued = item.Valued,
                        CreateDate = DateTime.Now,
                        CreateUserd = CurrentUser.UserName,
                        UpdateDate = DateTime.Now,
                        UpdateUser = CurrentUser.UserName,
                    };
                    biz.Updatedata(model);
                }
                WriteLog("Update  TempleatePropertyUser", Constant.LogType.Success);
                return Json(new { isSuccess = true, mess = Constant.Addnewsuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { isSuccess = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}