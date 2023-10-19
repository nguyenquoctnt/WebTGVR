using System;
using System.Collections.Generic;
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

namespace VDMMutiline.WebBackEnd.Areas.DanhMuc.Controllers
{
 
    public class DmXaphuongController : BaseController
    {
        DmXaphuongBiz _biz = new DmXaphuongBiz();
        public ActionResult Index()
        {
            var param = new DmXaphuongParam();
            param.DmQuocgiaInfos = GetlistCommon.Getlistquocgia();
            param.DmTinhthanhInfos = new List<DmTinhthanhInfo>();
            param.DmHuyenthiInfos = new List<DmHuyenthiInfo>();
            return View(param);
        }
        public ActionResult AjaxLoadList(GridFilterSetting<DmXaphuongInfo> gridFilterSetting, string keyseach, int? HuyenID)
        {
            var pagininfo = new PagingInfo { RowStart = gridFilterSetting.iDisplayStart, PageSize = gridFilterSetting.iDisplayLength };
            var param = new DmXaphuongParam() { PagingInfo = pagininfo };
            var DmXaphuongFilter = new DmXaphuongFilter() {IdHuyen = HuyenID };
            param.DmXaphuongFilter = DmXaphuongFilter;
            _biz.Search(param);
            long count = pagininfo.RecordCount;
            return Json(new { aaData = param.DmXaphuongInfos, recordsTotal = count, recordsFiltered = count, amount = 0x2710 }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Create(string id)
        {
            var model = new DmXaphuongParam();
            var _id = 0;
            if (Int32.TryParse(id, out _id))
            {
                model.Id = _id;
                _biz.SetupEditForm(model);
            }
            else
            {
                var ck = new DmXaphuong();
                model.DmXaphuong = ck;
            }
            model.DmQuocgiaInfos = GetlistCommon.Getlistquocgia();
            model.DmTinhthanhInfos = new List<DmTinhthanhInfo>();
            model.DmHuyenthiInfos = new List<DmHuyenthiInfo>();
            return View(model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(DmXaphuongParam modelInput)
        {
            try
            {
                if (modelInput != null)
                {

                    modelInput.UserName = CurrentUser.UserName;
                    if (modelInput.DmXaphuong.Id > 0)
                    {
                        modelInput.DmXaphuong.NgaySua = DateTime.Now;
                        modelInput.DmXaphuong.NguoiSua = CurrentUser.UserName;
                        _biz.Update(modelInput);
                        WriteLog("Cập nhật thông tin DmXaphuong", Constant.LogType.Success);
                        return Json(new { isSuccess = true, mess = Constant.Updatesuccess }, JsonRequestBehavior.AllowGet);
                    }
                    modelInput.DmXaphuong.NgayTao = DateTime.Now;
                    modelInput.DmXaphuong.Nguoitao = CurrentUser.UserName;
                    modelInput.DmXaphuong.NgaySua = DateTime.Now;
                    modelInput.DmXaphuong.NguoiSua = CurrentUser.UserName;
                    _biz.Insert(modelInput);
                    WriteLog("ADD thông tin DmXaphuong", Constant.LogType.Success);
                    return Json(new { isSuccess = true, mess = Constant.Addnewsuccess }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { isSuccess = false, mess = Constant.AcactFail }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                WriteLog("Cập nhật thông tin DmXaphuong", Constant.LogType.Success);
                return Json(new { isSuccess = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult Delete(int? id)
        {
            var param = new DmXaphuongParam { Id = id ?? 0 };
            _biz.SetupEditForm(param);
            DmXaphuong item = param.DmXaphuong;
            if (item == null)
            {
                ViewBag.Erro = Constant.NotItem;
                return View(new DmXaphuong());
            }
            else
                return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(DmXaphuong obj)
        {
            try
            {
                var list = new List<DmXaphuong> { obj };
                var paramDelete = new DmXaphuongParam { DmXaphuongs = list };
                _biz.Delete(paramDelete);
                WriteLog("Delete thông tin DmXaphuong", Constant.LogType.Success);
                return Json(new { isSuccess = true, mess = Constant.Deletesuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                WriteLog("Delete thông tin DmXaphuong", Constant.LogType.Success);
                return Json(new { isSuccess = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}