using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using VDMMutiline.Biz;
using VDMMutiline.Core.UI;
using VDMMutiline.SharedComponent;
using VDMMutiline.SharedComponent.Constants;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
using VDMMutiline.SharedComponent.Models;
using VDMMutiline.SharedComponent.Params;
using VDMMutiline.Core;
using VDMMutiline.Ultilities;

namespace VDMMutiline.WebBackEnd.Controllers
{
   
    public class TblQuangCaoController : BaseController
    {
        TblQuangCaoBiz _biz = new TblQuangCaoBiz();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AjaxLoadList(GridFilterSetting<TblQuangCaoInfo> gridFilterSetting, string keyseach)
        {
            var pagininfo = new PagingInfo { RowStart = gridFilterSetting.iDisplayStart, PageSize = gridFilterSetting.iDisplayLength };
            var param = new TblQuangCaoParam() { PagingInfo = pagininfo };
            var TblQuangCaoFilter = new TblQuangCaoFilter() {  ListUserName = GetparentUserDaiLy().Select(z => z.UserName).ToList() };
            param.TblQuangCaoFilter = TblQuangCaoFilter;
            _biz.Search(param);
            long count = pagininfo.RecordCount;
            return Json(new { aaData = param.TblQuangCaoInfos, recordsTotal = count, recordsFiltered = count, amount = 0x2710 }, JsonRequestBehavior.AllowGet);
        }


        public List<SysVungHienthiInfo> Getvunghienthi()
        {
            var param = new SysVungHienthiParam();
            var biz = new SysVungHienthiBiz();
            biz.GetAll(param);
            return param.SysVungHienthiInfos.ToList();
        }
        public ActionResult Create(string id)
        {

            var model = new TblQuangCaoParam();
            int _id = 0;
            if (Int32.TryParse(id, out _id))
            {
                var param = new TblQuangCaoParam { Id = _id };
                param.SysVungHienthiInfos = Getvunghienthi();
                var biz = new TblQuangCaoBiz();
                biz.SetupEditForm(param);
                model = param;
            }
            else
            {
                model.SysVungHienthiInfos = Getvunghienthi();
                var ck = new TblQuangCao
                {
                    Id = 0
                };
                model.TblQuangCao = ck;
            }
            return View(model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(TblQuangCaoParam modelInput,FormCollection forminput)
        {
            try
            {
                if (modelInput != null)
                {
                    var ThoigianTu = forminput.GetValue("ThoigianTu");
                    if (ThoigianTu != null)
                        modelInput.TblQuangCao.ThoigianTu = Utility.ConvertToDate(ThoigianTu.AttemptedValue);

                    var ThoigianDen = forminput.GetValue("ThoigianDen");
                    if (ThoigianDen != null)
                        modelInput.TblQuangCao.ThoigianDen = Utility.ConvertToDate(ThoigianDen.AttemptedValue);

                    var Imagelink = UploadFile.UpLoadFile(Request.Files["Imageimput"], "Upload/QuangCao/");
                    modelInput.UserName = CurrentUser.UserName;
                    if (modelInput.TblQuangCao.Id > 0)
                    {

                        if (!string.IsNullOrEmpty(Imagelink) && Imagelink != modelInput.TblQuangCao.Path)
                        {

                            modelInput.TblQuangCao.Path = Imagelink;
                         
                        }
                        modelInput.TblQuangCao.NgaySua = DateTime.Now;
                        modelInput.TblQuangCao.NguoiSua = CurrentUser.UserName;
                        _biz.Update(modelInput);
                        WriteLog("Cập nhật thông tin quảng cáo", Constant.LogType.Success);
                        return Json(new { isSuccess = true, mess = Constant.Updatesuccess }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        modelInput.TblQuangCao.Path = Imagelink;
                        modelInput.TblQuangCao.NgaySua = DateTime.Now;
                        modelInput.TblQuangCao.NguoiSua = CurrentUser.UserName;
                        modelInput.TblQuangCao.NgayTao = DateTime.Now;
                        modelInput.TblQuangCao.Nguoitao = CurrentUser.UserName;
                        WriteLog("Thêm mới thông tin quảng cáo", Constant.LogType.Success);
                        _biz.Insert(modelInput);
                    }



                    // modelInput.TblQuangCao.Nguoitao =  CurrentUser.UserName;

                    //modelInput.TblQuangCao.NguoiSua =  CurrentUser.UserName;

                    return Json(new { isSuccess = true, mess = Constant.Addnewsuccess }, JsonRequestBehavior.AllowGet);
                }


                return Json(new { isSuccess = false, mess = Constant.AcactFail }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                WriteLog("Cập nhật thông tin quảng cáo", Constant.LogType.Success);
                return Json(new { isSuccess = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult Delete(int? id)
        {
            var param = new TblQuangCaoParam { Id = id ?? 0 };
            _biz.SetupEditForm(param);
            TblQuangCao item = param.TblQuangCao;
            if (item == null)
            {
                ViewBag.Erro = Constant.NotItem;
                return View(new TblQuangCao());
            }
            else
                return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(TblQuangCao obj)
        {
            try
            {
                var list = new List<TblQuangCao> { obj };
                var paramDelete = new TblQuangCaoParam { TblQuangCaos = list };
                _biz.Delete(paramDelete);
                WriteLog("Xóa thông tin quảng cáo", Constant.LogType.Success);
                return Json(new { isSuccess = true, mess = Constant.Deletesuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                WriteLog("Xóa thông tin quảng cáo", Constant.LogType.Success);
                return Json(new { isSuccess = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }




    }
}