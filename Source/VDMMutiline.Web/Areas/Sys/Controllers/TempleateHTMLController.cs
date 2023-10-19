using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using VDMMutiline.Biz.TempleteSetting;
using VDMMutiline.Core.UI;
using VDMMutiline.SharedComponent;
using VDMMutiline.SharedComponent.Constants;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo.TempleteSetting;
using VDMMutiline.SharedComponent.Models;
using VDMMutiline.SharedComponent.Params.TempleteSetting;

namespace VDMMutiline.WebBackEnd.Areas.Sys.Controllers
{
    public class TempleateHTMLController : BaseController
    {
        TempleateHTMLBiz _biz = new TempleateHTMLBiz();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AjaxLoadList(GridFilterSetting<TempleateHTMLInfo> gridFilterSetting, string keyseach, int? parentid)
        {
            var pagininfo = new PagingInfo { RowStart = gridFilterSetting.iDisplayStart, PageSize = gridFilterSetting.iDisplayLength };
            var param = new TempleateHTMLParam() { PagingInfo = pagininfo };
            var TempleateHTMLFilter = new TempleateHTMLFilter() { Search = keyseach, ItemId = parentid };
            param.TempleateHTMLFilter = TempleateHTMLFilter;
            _biz.Search(param);
            long count = pagininfo.RecordCount;
            return Json(new { aaData = param.TempleateHTMLInfos, recordsTotal = count, recordsFiltered = count, amount = 0x2710 }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Create(string id)
        {
            var model = new TempleateHTMLParam();
            var _id = 0;
            if (Int32.TryParse(id, out _id))
            {
                model.Id = _id;
                _biz.SetupEditForm(model);
            }
            else
            {
                var ck = new tbl_TempleateHTML();
                model.TempleateHTML = ck;
            }
            return View(model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(TempleateHTMLParam modelInput)
        {
            try
            {
                if (modelInput != null)
                {
                    modelInput.UserName = CurrentUser.UserName;
                    if (modelInput.TempleateHTML.Id > 0)
                    {
                        modelInput.TempleateHTML.UpdateDate = DateTime.Now;
                        modelInput.TempleateHTML.UpdateUser = CurrentUser.UserName;
                        _biz.Update(modelInput);
                        WriteLog("Cập nhật  thông tin TempleateHTML", Constant.LogType.Success);
                        return Json(new { isSuccess = true, mess = Constant.Updatesuccess }, JsonRequestBehavior.AllowGet);
                    }
                    modelInput.TempleateHTML.UpdateDate = DateTime.Now;
                    modelInput.TempleateHTML.UpdateUser = CurrentUser.UserName;
                    modelInput.TempleateHTML.CreateDate = DateTime.Now;
                    modelInput.TempleateHTML.CreateUserd = CurrentUser.UserName;
                    _biz.Insert(modelInput);
                    WriteLog("ADD  thông tin TempleateHTML", Constant.LogType.Success);
                    return Json(new { isSuccess = true, mess = Constant.Addnewsuccess }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { isSuccess = false, mess = Constant.AcactFail }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                WriteLog("Cập nhật  thông tin TempleateHTML", Constant.LogType.Success);
                return Json(new { isSuccess = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult Delete(int? id)
        {
            var param = new TempleateHTMLParam { Id = id ?? 0 };
            _biz.SetupEditForm(param);
            tbl_TempleateHTML item = param.TempleateHTML;
            if (item == null)
            {
                ViewBag.Erro = Constant.NotItem;
                return View(new SysMenu());
            }
            else
                return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(tbl_TempleateHTML obj)
        {
            try
            {
                var list = new List<tbl_TempleateHTML> { obj };
                var paramDelete = new TempleateHTMLParam { TempleateHTMLs = list };
                _biz.Delete(paramDelete);
                WriteLog(" Delete thông tin TempleateHTML", Constant.LogType.Success);
                return Json(new { isSuccess = true, mess = Constant.Deletesuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                WriteLog(" Delete thông tin TempleateHTML", Constant.LogType.Success);
                return Json(new { isSuccess = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}