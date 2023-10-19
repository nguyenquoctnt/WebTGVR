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
   
    public class DmTinhthanhController : BaseController
    {
        DmTinhthanhBiz _biz = new DmTinhthanhBiz();
        public ActionResult Index()
        {
            var param = new DmTinhthanhParam();
            param.DmQuocgiaInfos = GetlistCommon.Getlistquocgia();
            return View(param);
        }
        public ActionResult AjaxLoadList(GridFilterSetting<DmTinhthanhInfo> gridFilterSetting, string keyseach, int? quocgia)
        {
            var pagininfo = new PagingInfo { RowStart = gridFilterSetting.iDisplayStart, PageSize = gridFilterSetting.iDisplayLength };
            var param = new DmTinhthanhParam() { PagingInfo = pagininfo };
            var DmTinhthanhFilter = new DmTinhthanhFilter() {Search = keyseach, Idquocgia = quocgia};
            param.DmTinhthanhFilter = DmTinhthanhFilter;
            _biz.Search(param);
            long count = pagininfo.RecordCount;
            return Json(new { aaData = param.DmTinhthanhInfos, recordsTotal = count, recordsFiltered = count, amount = 0x2710 }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Create(string id)
        {
            var model = new DmTinhthanhParam();
            var _id = 0;
            if (Int32.TryParse(id, out _id))
            {
                model.Id = _id;
                _biz.SetupEditForm(model);
            }
            else
            {
                var ck = new DmTinhthanh();
                model.DmTinhthanh = ck;
            }
            model.DmQuocgiaInfos = GetlistCommon.Getlistquocgia();
            return View(model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(DmTinhthanhParam modelInput)
        {
            try
            {
                if (modelInput != null)
                {
                  
                    modelInput.UserName =  CurrentUser.UserName;
                    if (modelInput.DmTinhthanh.Id > 0)
                    {
                        modelInput.DmTinhthanh.NgaySua = DateTime.Now;
                         modelInput.DmTinhthanh.NguoiSua =  CurrentUser.UserName;
                        _biz.Update(modelInput);
                        WriteLog("Cập nhật thông tin DmTinhthanh", Constant.LogType.Success);
                        return Json(new { isSuccess = true, mess=Constant.Updatesuccess }, JsonRequestBehavior.AllowGet);
                    }
                    modelInput.DmTinhthanh.NgayTao = DateTime.Now;
                     modelInput.DmTinhthanh.Nguoitao =  CurrentUser.UserName;
                    modelInput.DmTinhthanh.NgaySua = DateTime.Now;
                    modelInput.DmTinhthanh.NguoiSua =  CurrentUser.UserName;
                    _biz.Insert(modelInput);
                    WriteLog("ADD thông tin DmTinhthanh", Constant.LogType.Success);
                    return Json(new { isSuccess = true, mess = Constant.Addnewsuccess }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { isSuccess = false, mess = Constant.AcactFail }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                WriteLog("Cập nhật thông tin DmTinhthanh", Constant.LogType.Success);
                return Json(new { isSuccess = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            
        }
        public ActionResult Delete(int? id)
        {
            var param = new DmTinhthanhParam { Id = id??0 };
            _biz.SetupEditForm(param);
            DmTinhthanh item = param.DmTinhthanh;
            if (item == null)
            {
                ViewBag.Erro = Constant.NotItem;
                return View(new DmTinhthanh());
            }
            else
                return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(DmTinhthanh obj)
        {
            try
            {
                var list = new List<DmTinhthanh> { obj };
                var paramDelete = new DmTinhthanhParam { DmTinhthanhs = list };
                _biz.Delete(paramDelete);
                WriteLog("Delete thông tin DmTinhthanh", Constant.LogType.Success);
                return Json(new { isSuccess = true , mess = Constant.Deletesuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                WriteLog("Delete thông tin DmTinhthanh", Constant.LogType.Success);
                return Json(new { isSuccess = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}