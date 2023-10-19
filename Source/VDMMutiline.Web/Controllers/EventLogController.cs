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

namespace VDMMutiline.WebBackEnd.Controllers
{
    public class EventLogController : BaseController
    {
        EventLogBiz _biz = new EventLogBiz();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AjaxLoadList(GridFilterSetting<EventLogInfo> gridFilterSetting, string keyseach,int? level)
        {
            var pagininfo = new PagingInfo { RowStart = gridFilterSetting.iDisplayStart, PageSize = gridFilterSetting.iDisplayLength };
            var param = new EventLogParam() { PagingInfo = pagininfo };
            var EventLogFilter = new EventLogFilter() { ItemId = level };
            param.EventLogFilter = EventLogFilter;
            _biz.Search(param);
            long count = pagininfo.RecordCount;
            return Json(new { aaData = param.EventLogInfos, recordsTotal = count, recordsFiltered = count, amount = 0x2710 }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Create(string id)
        {
            var model = new EventLogParam();
            var _id = 0;
            if (Int32.TryParse(id, out _id))
            {
                model.Id = _id;
                _biz.SetupEditForm(model);
            }
            else
            {
                var ck = new EventLog();
                model.EventLog = ck;
            }
            return View(model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(EventLogParam modelInput)
        {
            try
            {
                if (modelInput != null)
                {
                  
                    modelInput.UserName = User.Identity.Name;
                    if (modelInput.EventLog.ID > 0)
                    {
                       
                        _biz.Update(modelInput);
                        WriteLog("Cập nhật thông tin EventLog", Constant.LogType.Success);
                        return Json(new { isSuccess = true, mess=Constant.Updatesuccess }, JsonRequestBehavior.AllowGet);
                    }
                
                    _biz.Insert(modelInput);
                    WriteLog("Add thông tin EventLog", Constant.LogType.Success);
                    return Json(new { isSuccess = true, mess = Constant.Addnewsuccess }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { isSuccess = false, mess = Constant.AcactFail }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                WriteLog("Cập nhật thông tin EventLog", Constant.LogType.Success);
                return Json(new { isSuccess = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            
        }
        public ActionResult Delete(int? id)
        {
            var param = new EventLogParam { Id = id??0 };
            _biz.SetupEditForm(param);
            EventLog item = param.EventLog;
            if (item == null)
            {
                ViewBag.Erro = Constant.NotItem;
                return View(new EventLog());
            }
            else
                return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(EventLog obj)
        {
            try
            {
                var list = new List<EventLog> { obj };
                var paramDelete = new EventLogParam { EventLogs = list };
                _biz.Delete(paramDelete);
                WriteLog("Xóa thông tin EventLog", Constant.LogType.Success);
                return Json(new { isSuccess = true , mess = Constant.Deletesuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                WriteLog("Xóa thông tin EventLog", Constant.LogType.Success);
                return Json(new { isSuccess = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}