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

namespace VDMMutiline.WebBackEnd.Controllers
{
    public class MessageController : BaseController
    {
        MessageBiz _biz = new MessageBiz();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AjaxLoadList(GridFilterSetting<MessageInfo> gridFilterSetting, string keyseach)
        {
            var pagininfo = new PagingInfo { RowStart = gridFilterSetting.iDisplayStart, PageSize = gridFilterSetting.iDisplayLength };
            var param = new MessageParam() { PagingInfo = pagininfo };
            var MessageFilter = new MessageFilter() { };
            param.MessageFilter = MessageFilter;
            _biz.Search(param);
            long count = pagininfo.RecordCount;
            return Json(new { aaData = param.MessageInfos, recordsTotal = count, recordsFiltered = count, amount = 0x2710 }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Create(string id)
        {
            var model = new MessageParam();
            var _id = 0;
            if (Int32.TryParse(id, out _id))
            {
                model.Id = _id;
                _biz.SetupEditForm(model);
            }
            else
            {
                var ck = new Message();
                model.Message = ck;
            }
            return View(model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(MessageParam modelInput)
        {
            try
            {
                if (modelInput != null)
                {
                  
                    modelInput.UserName = User.Identity.Name;
                    if (modelInput.Message.Id > 0)
                    {
                       
                        _biz.Update(modelInput);
                        WriteLog("Cập nhật thông tin Message", Constant.LogType.Success);
                        return Json(new { isSuccess = true, mess=Constant.Updatesuccess }, JsonRequestBehavior.AllowGet);
                    }
                  
                    _biz.Insert(modelInput);
                    WriteLog("ADD  thông tin Message", Constant.LogType.Success);
                    return Json(new { isSuccess = true, mess = Constant.Addnewsuccess }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { isSuccess = false, mess = Constant.AcactFail }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                WriteLog("Cập nhật thông tin Message", Constant.LogType.Success);
                return Json(new { isSuccess = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            
        }
        public ActionResult Delete(int? id)
        {
            var param = new MessageParam { Id = id??0 };
            _biz.SetupEditForm(param);
            Message item = param.Message;
            if (item == null)
            {
                ViewBag.Erro = Constant.NotItem;
                return View(new Message());
            }
            else
                return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Message obj)
        {
            try
            {
                var list = new List<Message> { obj };
                var paramDelete = new MessageParam { Messages = list };
                _biz.Delete(paramDelete);
                WriteLog("Xóa thông tin Message", Constant.LogType.Success);
                return Json(new { isSuccess = true , mess = Constant.Deletesuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                WriteLog("Xóa thông tin Message", Constant.LogType.Success);
                return Json(new { isSuccess = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult AjaxGetNewMessage()
        {
            int userId = CurrentUser.Id;
            var pagininfo = new PagingInfo {};
            var param = new MessageParam() {};
            var MessageFilter = new MessageFilter() { Id=userId };
            param.MessageFilter = MessageFilter;
            _biz.GetUnRead(param);
            List<MessageInfo> all = param.MessageInfos.ToList();
            long count = all.Count;
            var jsonResult = Json(new
            {
                entities = all,
                total = count
            }, JsonRequestBehavior.AllowGet); ;
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
    }
}