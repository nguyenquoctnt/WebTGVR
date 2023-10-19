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
   
    public class MessageRoomController : BaseController
    {
        MessageRoomBiz _biz = new MessageRoomBiz();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AjaxLoadList(GridFilterSetting<MessageRoomInfo> gridFilterSetting, string keyseach)
        {
            var pagininfo = new PagingInfo { RowStart = gridFilterSetting.iDisplayStart, PageSize = gridFilterSetting.iDisplayLength };
            var param = new MessageRoomParam() { PagingInfo = pagininfo };
            var MessageRoomFilter = new MessageRoomFilter() { };
            param.MessageRoomFilter = MessageRoomFilter;
            _biz.Search(param);
            long count = pagininfo.RecordCount;
            return Json(new { aaData = param.MessageRoomInfos, recordsTotal = count, recordsFiltered = count, amount = 0x2710 }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Create(string id)
        {
            var model = new MessageRoomParam();
            var _id = 0;
            if (Int32.TryParse(id, out _id))
            {
                model.Id = _id;
                _biz.SetupEditForm(model);
            }
            else
            {
                var ck = new MessageRoom();
                model.MessageRoom = ck;
            }
            return View(model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(MessageRoomParam modelInput)
        {
            try
            {
                if (modelInput != null)
                {
                  
                    modelInput.UserName = User.Identity.Name;
                    if (modelInput.MessageRoom.Id > 0)
                    {
                      
                        _biz.Update(modelInput);
                        WriteLog("Cập nhật thông tin MessageRoom", Constant.LogType.Success);
                        return Json(new { isSuccess = true, mess=Constant.Updatesuccess }, JsonRequestBehavior.AllowGet);
                    }
                   
                    _biz.Insert(modelInput);
                    WriteLog("Add thông tin MessageRoom", Constant.LogType.Success);
                    return Json(new { isSuccess = true, mess = Constant.Addnewsuccess }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { isSuccess = false, mess = Constant.AcactFail }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                WriteLog("Cập nhật thông tin MessageRoom", Constant.LogType.Success);
                return Json(new { isSuccess = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            
        }
        public ActionResult Delete(int? id)
        {
            var param = new MessageRoomParam { Id = id??0 };
            _biz.SetupEditForm(param);
            MessageRoom item = param.MessageRoom;
            if (item == null)
            {
                ViewBag.Erro = Constant.NotItem;
                return View(new MessageRoom());
            }
            else
                return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(MessageRoom obj)
        {
            try
            {
                var list = new List<MessageRoom> { obj };
                var paramDelete = new MessageRoomParam { MessageRooms = list };
                _biz.Delete(paramDelete);
                WriteLog("Xóa thông tin MessageRoom", Constant.LogType.Success);
                return Json(new { isSuccess = true , mess = Constant.Deletesuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                WriteLog("Xóa thông tin MessageRoom", Constant.LogType.Success);
                return Json(new { isSuccess = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}