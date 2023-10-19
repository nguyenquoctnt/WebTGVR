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
using System.Linq;
namespace VDMMutiline.WebBackEnd.Areas.DanhMuc.Controllers
{
    public class DmTaikhoanNganHangController : BaseController
    {
        DmTaikhoanNganHangBiz _biz = new DmTaikhoanNganHangBiz();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AjaxLoadList(GridFilterSetting<DmTaikhoanNganHangInfo> gridFilterSetting, string keyseach)
        {
            var pagininfo = new PagingInfo { RowStart = gridFilterSetting.iDisplayStart, PageSize = gridFilterSetting.iDisplayLength };
            var param = new DmTaikhoanNganHangParam() { PagingInfo = pagininfo };
            var DmTaikhoanNganHangFilter = new DmTaikhoanNganHangFilter() { Search = keyseach, ListUserName = GetparentUserDaiLy().Select(z => z.UserName).ToList() };
            param.DmTaikhoanNganHangFilter = DmTaikhoanNganHangFilter;
            _biz.Search(param);
            long count = pagininfo.RecordCount;
            return Json(new { aaData = param.DmTaikhoanNganHangInfos, recordsTotal = count, recordsFiltered = count, amount = 0x2710 }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Create(string id)
        {
            var model = new DmTaikhoanNganHangParam();
            var _id = 0;
            if (Int32.TryParse(id, out _id))
            {
                model.Id = _id;
                _biz.SetupEditForm(model);
            }
            else
            {
                var ck = new DmTaikhoanNganHang();
                model.DmTaikhoanNganHang = ck;
            }
            return View(model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(DmTaikhoanNganHangParam modelInput)
        {
            try
            {
                if (modelInput != null)
                {
                    var iconlink = UploadFile.UpLoadFile(Request.Files["Imageimput"], "Upload/Articles/");
                    if (!string.IsNullOrEmpty(iconlink))
                    {
                        modelInput.DmTaikhoanNganHang.Logo = iconlink;
                    }
                    modelInput.UserName = CurrentUser.UserName;
                    if (modelInput.DmTaikhoanNganHang.Id > 0)
                    {
                        modelInput.DmTaikhoanNganHang.NgaySua = DateTime.Now;
                        modelInput.DmTaikhoanNganHang.NguoiSua = CurrentUser.UserName;
                        _biz.Update(modelInput);
                        WriteLog("Cập nhật thông tin DmTaikhoanNganHang", Constant.LogType.Success);
                        return Json(new { isSuccess = true, mess = Constant.Updatesuccess }, JsonRequestBehavior.AllowGet);
                    }
                    modelInput.DmTaikhoanNganHang.NgayTao = DateTime.Now;
                    modelInput.DmTaikhoanNganHang.Nguoitao = CurrentUser.UserName;
                    modelInput.DmTaikhoanNganHang.NgaySua = DateTime.Now;
                    modelInput.DmTaikhoanNganHang.NguoiSua = CurrentUser.UserName;
                    _biz.Insert(modelInput); WriteLog("Add thông tin DmTaikhoanNganHang", Constant.LogType.Success); WriteLog("Xóa thông tin DmQuocgia", Constant.LogType.Success);
                    return Json(new { isSuccess = true, mess = Constant.Addnewsuccess }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { isSuccess = false, mess = Constant.AcactFail }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                WriteLog("Cập nhật thông tin DmTaikhoanNganHang", Constant.LogType.Success);
                return Json(new { isSuccess = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult Delete(int? id)
        {
            var param = new DmTaikhoanNganHangParam { Id = id ?? 0 };
            _biz.SetupEditForm(param);
            DmTaikhoanNganHang item = param.DmTaikhoanNganHang;
            if (item == null)
            {
                ViewBag.Erro = Constant.NotItem;
                return View(new DmTaikhoanNganHang());
            }
            else
                return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(DmTaikhoanNganHang obj)
        {
            try
            {
                var list = new List<DmTaikhoanNganHang> { obj };
                var paramDelete = new DmTaikhoanNganHangParam { DmTaikhoanNganHangs = list };
                _biz.Delete(paramDelete);
                WriteLog("Delete thông tin DmTaikhoanNganHang", Constant.LogType.Success);
                return Json(new { isSuccess = true, mess = Constant.Deletesuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                WriteLog("Delete thông tin DmTaikhoanNganHang", Constant.LogType.Success);
                return Json(new { isSuccess = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}