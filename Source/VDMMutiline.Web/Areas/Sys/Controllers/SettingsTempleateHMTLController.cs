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
using VDMMutiline.SharedComponent;
using VDMMutiline.SharedComponent.Constants;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
using VDMMutiline.SharedComponent.EntityInfo.TempleteSetting;
using VDMMutiline.SharedComponent.Models;
using VDMMutiline.SharedComponent.Params;
using VDMMutiline.SharedComponent.Params.TempleteSetting;

namespace VDMMutiline.WebBackEnd.Areas.Sys.Controllers
{
    public class SettingsTempleateHMTLController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AjaxLoadList(GridFilterSetting<TempleateHTMLUserInfo> gridFilterSetting, string keyseach, int? parentid)
        {
            TempleateHTMLUserBiz _biz = new TempleateHTMLUserBiz();
            var param = new TempleateHTMLUserParam() { };
            var TempleateHTMLUserFilter = new TempleateHTMLUserFilter() { UserId=CurrentUser.Id};
            param.TempleateHTMLUserFilter = TempleateHTMLUserFilter;
            _biz.SearchByUser(param);
            long count = param.TempleateHTMLUserInfos.Count;
            return Json(new { aaData = param.TempleateHTMLUserInfos, recordsTotal = count, recordsFiltered = count, amount = 0x2710 }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UpdateSetting(int? IdTempleate)
        {
            var biz = new TempleateHTMLUserBiz();
            var obj = biz.SetupDisplayForm(IdTempleate, CurrentUser.Id);
            return View(obj);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult UpdateSetting(TempleateHTMLUserInfo info)
        {
            try
            {
                var biz = new TempleateHTMLUserBiz();
               
                    TempleateHTMLUserParam model = new TempleateHTMLUserParam();
                    model.TempleateHTMLUser = new tbl_TempleateHTMLUser
                    {
                        IdTempleate = info.IdTempleate,
                        IdUser = CurrentUser.Id,
                        Value = info.Value,
                        CreateDate = DateTime.Now,
                        CreateUserd = CurrentUser.UserName,
                        UpdateDate = DateTime.Now,
                        UpdateUser = CurrentUser.UserName,
                    };
                    biz.Updatedata(model);
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