using System;
using System.Collections.Generic;
using System.Web.Mvc;
using VDMMutiline.Biz;
using VDMMutiline.Core.UI;
using VDMMutiline.SharedComponent;
using VDMMutiline.SharedComponent.Constants;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
using VDMMutiline.SharedComponent.Models;
using VDMMutiline.SharedComponent.Params;

namespace VDMMutiline.WebBackEnd.Areas.Sys.Controllers
{
   
    public class SysVungHienthiController : BaseController
    {
        SysVungHienthiBiz _biz = new SysVungHienthiBiz();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AjaxLoadList(GridFilterSetting<SysVungHienthiInfo> gridFilterSetting, string keyseach)
        {
            var pagininfo = new PagingInfo { RowStart = gridFilterSetting.iDisplayStart, PageSize = gridFilterSetting.iDisplayLength };
            var param = new SysVungHienthiParam() { PagingInfo = pagininfo };
            var SysVungHienthiFilter = new SysVungHienthiFilter() { };
            param.SysVungHienthiFilter = SysVungHienthiFilter;
            _biz.Search(param);
            long count = pagininfo.RecordCount;
            return Json(new { aaData = param.SysVungHienthiInfos, recordsTotal = count, recordsFiltered = count, amount = 0x2710 }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Create(string id)
        {
            var model = new SysVungHienthiParam();
            var _id = 0;
            if (Int32.TryParse(id, out _id))
            {
                model.Id = _id;
                _biz.SetupEditForm(model);
            }
            else
            {
                var ck = new SysVungHienthi();
                model.SysVungHienthi = ck;
            }
            return View(model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(SysVungHienthiParam modelInput)
        {
            try
            {
                if (modelInput != null)
                {
                  
                    modelInput.UserName =  CurrentUser.UserName;
                    if (modelInput.SysVungHienthi.Id > 0)
                    {
                        modelInput.SysVungHienthi.NgaySua = DateTime.Now;
                        // modelInput.SysVungHienthi.NguoiSua =  CurrentUser.UserName;
                        _biz.Update(modelInput);
                        WriteLog("Update thông tin SysVungHienthi", Constant.LogType.Success);
                        return Json(new { isSuccess = true, mess=Constant.Updatesuccess }, JsonRequestBehavior.AllowGet);
                    }
                    modelInput.SysVungHienthi.NgayTao = DateTime.Now;
                    // modelInput.SysVungHienthi.Nguoitao =  CurrentUser.UserName;
                    modelInput.SysVungHienthi.NgaySua = DateTime.Now;
                    //modelInput.SysVungHienthi.NguoiSua =  CurrentUser.UserName;
                    _biz.Insert(modelInput);
                    WriteLog("Insert thông tin SysVungHienthi", Constant.LogType.Success);
                    return Json(new { isSuccess = true, mess = Constant.Addnewsuccess }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { isSuccess = false, mess = Constant.AcactFail }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                WriteLog("Cập nhật thông tin SysVungHienthi", Constant.LogType.Success);
                return Json(new { isSuccess = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            
        }
        public ActionResult Delete(int? id)
        {
            var param = new SysVungHienthiParam { Id = id??0 };
            _biz.SetupEditForm(param);
            SysVungHienthi item = param.SysVungHienthi;
            if (item == null)
            {
                ViewBag.Erro = Constant.NotItem;
                return View(new SysVungHienthi());
            }
            else
                return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(SysVungHienthi obj)
        {
            try
            {
                var list = new List<SysVungHienthi> { obj };
                var paramDelete = new SysVungHienthiParam { SysVungHienthis = list };
                _biz.Delete(paramDelete);
                WriteLog("Delete thông tin SysVungHienthi", Constant.LogType.Success);
                return Json(new { isSuccess = true , mess = Constant.Deletesuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                WriteLog("Delete thông tin SysVungHienthi", Constant.LogType.Success);
                return Json(new { isSuccess = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}