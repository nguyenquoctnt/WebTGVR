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
    public class SysAirlineController : BaseController
    {
        AirlineBiz _biz = new AirlineBiz();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AjaxLoadList(GridFilterSetting<AirlineInfo> gridFilterSetting, string keyseach)
        {
            var pagininfo = new PagingInfo { RowStart = gridFilterSetting.iDisplayStart, PageSize = gridFilterSetting.iDisplayLength };
            var param = new AirlineParam() { PagingInfo = pagininfo };
            var AirlineFilter = new AirlineFilter()
            {
                
                Search = keyseach,
            
            };
            param.AirlineFilter = AirlineFilter;
            _biz.Search(param);
            long count = pagininfo.RecordCount;
            return Json(new { aaData = param.AirlineInfos, recordsTotal = count, recordsFiltered = count, amount = 0x2710 }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Create(string id, int? typeid)
        {
            var model = new AirlineParam();
            var _id = 0;
            if (Int32.TryParse(id, out _id))
            {
                model.Id = _id;
                _biz.SetupEditForm(model);
            }
            else
            {
                var ck = new tbl_airline();
                model.Airline = ck;
            }
            return View(model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(AirlineParam modelInput, FormCollection forminput)
        {
            try
            {
                if (modelInput != null)
                {
                    var Imagelink = UploadFile.UpLoadFile(Request.Files["Imageimput"], "Upload/Airlines/");
                   
                    if (!string.IsNullOrEmpty(Imagelink))
                    {
                        modelInput.Airline.Pictures = Imagelink;
                    }
                    modelInput.UserName = CurrentUser.UserName;

                    if (modelInput.Airline.Id > 0)
                    {
                        _biz.Update(modelInput);
                        WriteLog("Update thông tin Airline", Constant.LogType.Success);
                        return Json(new { isSuccess = true, mess = Constant.Updatesuccess }, JsonRequestBehavior.AllowGet);
                    }
                    _biz.Insert(modelInput);
                    WriteLog("Insert thông tin Airline", Constant.LogType.Success);
                    return Json(new { isSuccess = true, mess = Constant.Addnewsuccess }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { isSuccess = false, mess = Constant.AcactFail }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { isSuccess = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult Delete(int? id)
        {
            var param = new AirlineParam { Id = id ?? 0 };
            _biz.SetupEditForm(param);
            tbl_airline item = param.Airline;
            if (item == null)
            {
                ViewBag.Erro = Constant.NotItem;
                return View(new tbl_airline());
            }
            else
                return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(tbl_airline obj)
        {
            try
            {
                var list = new List<tbl_airline> { obj };
                var paramDelete = new AirlineParam { Airlines = list };
                _biz.Delete(paramDelete);
                WriteLog("Delete thông tin Airline", Constant.LogType.Success);
                return Json(new { isSuccess = true, mess = Constant.Deletesuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                WriteLog("Delete thông tin Airline", Constant.LogType.Success);
                return Json(new { isSuccess = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}