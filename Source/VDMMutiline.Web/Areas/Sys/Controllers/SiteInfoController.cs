using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using VDMMutiline.Biz;
using VDMMutiline.Core;
using VDMMutiline.Core.UI;
using VDMMutiline.SharedComponent;
using VDMMutiline.SharedComponent.Constants;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
using VDMMutiline.SharedComponent.Models;
using VDMMutiline.SharedComponent.Params;

namespace VDMMutiline.WebBackEnd.Areas.Sys.Controllers
{
    public class SiteInfoController : BaseController
    {
        SiteInfoBiz _biz = new SiteInfoBiz();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AjaxLoadList(GridFilterSetting<SysMenuInfo> gridFilterSetting, string keyseach,int? parentid)
        {
            var pagininfo = new PagingInfo { RowStart = gridFilterSetting.iDisplayStart, PageSize = gridFilterSetting.iDisplayLength };
            var param = new SiteInfoParam() { PagingInfo = pagininfo };
            var SysMenuFilter = new SiteInfoFilter() { Search = keyseach,ItemId = parentid };
            param.SiteInfoFilter = SysMenuFilter;
            _biz.Search(param);
            long count = pagininfo.RecordCount;
            return Json(new { aaData = param.SiteInfos, recordsTotal = count, recordsFiltered = count, amount = 0x2710 }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Create(string id)
        {
            var model = new SiteInfoParam();
            var _id = 0;
            if (Int32.TryParse(id, out _id))
            {
                model.Id = _id;
                _biz.SetupEditForm(model);
            }
            else
            {
                var ck = new tbl_SiteInfo();
                model.tblSiteInfo = ck;
            }
            return View(model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(SiteInfoParam modelInput)
        {
            try
            {
                var avatar = UploadFile.UpLoadFile(Request.Files["Thumbinput"], "Upload/SiteInfo/");
                if (!string.IsNullOrEmpty(avatar))
                {
                    modelInput.tblSiteInfo.ThumPic = avatar;
                }
                if (modelInput != null)
                {
                    modelInput.UserName = CurrentUser.UserName;
                    if (modelInput.tblSiteInfo.Id > 0)
                    {
                        modelInput.tblSiteInfo.EditDate = DateTime.Now;
                        modelInput.tblSiteInfo.UpdateUser = CurrentUser.UserName;
                        _biz.Update(modelInput);
                        WriteLog("Cập nhật  thông tin tblSiteInfo", Constant.LogType.Success);
                        return Json(new { isSuccess = true, mess = Constant.Updatesuccess }, JsonRequestBehavior.AllowGet);
                    }
                    modelInput.tblSiteInfo.EditDate = DateTime.Now;
                    modelInput.tblSiteInfo.UpdateUser = CurrentUser.UserName;
                    modelInput.tblSiteInfo.CreateDate = DateTime.Now;
                    modelInput.tblSiteInfo.CreateUser = CurrentUser.UserName;
                    _biz.Insert(modelInput);
                    WriteLog("ADD  thông tin tblSiteInfo", Constant.LogType.Success);
                    return Json(new { isSuccess = true, mess = Constant.Addnewsuccess }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { isSuccess = false, mess = Constant.AcactFail }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                WriteLog("Cập nhật  thông tin SiteInfo", Constant.LogType.Success);
                return Json(new { isSuccess = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult Delete(int? id)
        {
            var param = new SiteInfoParam { Id = id ?? 0 };
            _biz.SetupEditForm(param);
            tbl_SiteInfo item = param.tblSiteInfo;
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
        public ActionResult Delete(tbl_SiteInfo obj)
        {
            try
            {
                var list = new List<tbl_SiteInfo> { obj };
                var paramDelete = new SiteInfoParam { tblSiteInfos = list };
                _biz.Delete(paramDelete);
                WriteLog(" Delete thông tin SiteInfo", Constant.LogType.Success);
                return Json(new { isSuccess = true, mess = Constant.Deletesuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                WriteLog(" Delete thông tin SiteInfo", Constant.LogType.Success);
                return Json(new { isSuccess = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}