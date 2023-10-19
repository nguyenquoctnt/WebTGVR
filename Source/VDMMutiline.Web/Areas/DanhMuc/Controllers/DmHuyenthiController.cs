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

namespace VDMMutiline.WebBackEnd.Areas.DanhMuc.Controllers
{
    
    public class DmHuyenthiController : BaseController
    {
        DmHuyenthiBiz _biz = new DmHuyenthiBiz();
        public ActionResult Index()
        {
            var param = new DmHuyenthiParam();
            param.DmQuocgiaInfos = GetlistCommon.Getlistquocgia();
            param.DmTinhthanhInfos= new List<DmTinhthanhInfo>();
            return View(param);
        }
        public ActionResult AjaxLoadList(GridFilterSetting<DmHuyenthiInfo> gridFilterSetting, string keyseach, int? stinhid)
        {
            var pagininfo = new PagingInfo { RowStart = gridFilterSetting.iDisplayStart, PageSize = gridFilterSetting.iDisplayLength };
            var param = new DmHuyenthiParam() { PagingInfo = pagininfo };
            var DmHuyenthiFilter = new DmHuyenthiFilter() {Idtinhthanh = stinhid , Search = keyseach };
            param.DmHuyenthiFilter = DmHuyenthiFilter;
            _biz.Search(param);
            long count = pagininfo.RecordCount;
            return Json(new { aaData = param.DmHuyenthiInfos, recordsTotal = count, recordsFiltered = count, amount = 0x2710 }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Create(string id)
        {
            var model = new DmHuyenthiParam();
            var _id = 0;
            if (Int32.TryParse(id, out _id))
            {
                model.Id = _id;
                _biz.SetupEditForm(model);
            }
            else
            {
                var ck = new DmHuyenthi();
                model.DmHuyenthi = ck;
            }
            model.DmQuocgiaInfos = GetlistCommon.Getlistquocgia();
            model.DmTinhthanhInfos = new List<DmTinhthanhInfo>();
            return View(model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(DmHuyenthiParam modelInput)
        {
            try
            {
                if (modelInput != null)
                {
                  
                    modelInput.UserName =  CurrentUser.UserName;
                    if (modelInput.DmHuyenthi.Id > 0)
                    {
                        modelInput.DmHuyenthi.NgaySua = DateTime.Now;
                        // modelInput.DmHuyenthi.NguoiSua =  CurrentUser.UserName;
                        _biz.Update(modelInput);
                        WriteLog("Cập nhật thông tin DmHuyenthi", Constant.LogType.Success);
                        return Json(new { isSuccess = true, mess=Constant.Updatesuccess }, JsonRequestBehavior.AllowGet);
                    }
                    modelInput.DmHuyenthi.NgayTao = DateTime.Now;
                    // modelInput.DmHuyenthi.Nguoitao =  CurrentUser.UserName;
                    modelInput.DmHuyenthi.NgaySua = DateTime.Now;
                    //modelInput.DmHuyenthi.NguoiSua =  CurrentUser.UserName;
                    _biz.Insert(modelInput);
                    WriteLog("ADD thông tin DmHuyenthi", Constant.LogType.Success);
                    return Json(new { isSuccess = true, mess = Constant.Addnewsuccess }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { isSuccess = false, mess = Constant.AcactFail }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                WriteLog("Cập nhật thông tin DmHuyenthi", Constant.LogType.Success);
                return Json(new { isSuccess = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            
        }
        public ActionResult Delete(int? id)
        {
            var param = new DmHuyenthiParam { Id = id??0 };
            _biz.SetupEditForm(param);
            DmHuyenthi item = param.DmHuyenthi;
            if (item == null)
            {
                ViewBag.Erro = Constant.NotItem;
                return View(new DmHuyenthi());
            }
            else
                return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(DmHuyenthi obj)
        {
            try
            {
                var list = new List<DmHuyenthi> { obj };
                var paramDelete = new DmHuyenthiParam { DmHuyenthis = list };
                _biz.Delete(paramDelete);
                WriteLog("Delete thông tin DmHuyenthi", Constant.LogType.Success);
                return Json(new { isSuccess = true , mess = Constant.Deletesuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                WriteLog("Delete thông tin DmHuyenthi", Constant.LogType.Success);
                return Json(new { isSuccess = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
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
    }
}