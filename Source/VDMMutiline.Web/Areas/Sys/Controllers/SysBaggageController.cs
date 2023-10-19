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
    public class SysBaggageController : BaseController
    {
        BaggageBiz _biz = new BaggageBiz();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AjaxLoadList(GridFilterSetting<BaggageInfo> gridFilterSetting, string keyseach)
        {
            var pagininfo = new PagingInfo { RowStart = gridFilterSetting.iDisplayStart, PageSize = gridFilterSetting.iDisplayLength };
            var param = new BaggageParam() { PagingInfo = pagininfo };
            var BaggageFilter = new BaggageFilter()
            {
                
                Search = keyseach,
            
            };
            param.BaggageFilter = BaggageFilter;
            _biz.Search(param);
            long count = pagininfo.RecordCount;
            return Json(new { aaData = param.BaggageInfos, recordsTotal = count, recordsFiltered = count, amount = 0x2710 }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Create(string id, int? typeid)
        {
            var model = new BaggageParam();
            var _id = 0;
            if (Int32.TryParse(id, out _id))
            {
                model.Id = _id;
                _biz.SetupEditForm(model);
            }
            else
            {
                var ck = new tblBaggage();
                model.Baggage = ck;
            }
            return View(model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(BaggageParam modelInput, FormCollection forminput)
        {
            try
            {
                if (modelInput != null)
                {
                 
                    modelInput.UserName = CurrentUser.UserName;
                   ;
                    if (modelInput.Baggage.Id > 0)
                    {
                        modelInput.Baggage.UpdateBy = CurrentUser.UserName;
                        modelInput.Baggage.UpdateDate = DateTime.Now;
                        _biz.Update(modelInput);
                        WriteLog("Update thông tin Baggage", Constant.LogType.Success);
                        return Json(new { isSuccess = true, mess = Constant.Updatesuccess }, JsonRequestBehavior.AllowGet);
                    }
                    modelInput.Baggage.CreateBy = CurrentUser.UserName;
                    modelInput.Baggage.CreateDate = DateTime.Now;
                    _biz.Insert(modelInput);
                    WriteLog("Insert thông tin Baggage", Constant.LogType.Success);
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
            var param = new BaggageParam { Id = id ?? 0 };
            _biz.SetupEditForm(param);
            tblBaggage item = param.Baggage;
            if (item == null)
            {
                ViewBag.Erro = Constant.NotItem;
                return View(new tblBaggage());
            }
            else
                return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(tblBaggage obj)
        {
            try
            {
                var list = new List<tblBaggage> { obj };
                var paramDelete = new BaggageParam { Baggages = list };
                _biz.Delete(paramDelete);
                WriteLog("Delete thông tin Baggages", Constant.LogType.Success);
                return Json(new { isSuccess = true, mess = Constant.Deletesuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                WriteLog("Delete thông tin Baggages", Constant.LogType.Success);
                return Json(new { isSuccess = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}