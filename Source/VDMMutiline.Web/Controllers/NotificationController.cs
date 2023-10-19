using VDMMutiline.Biz;
using VDMMutiline.Core.UI;
using VDMMutiline.SharedComponent;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
using VDMMutiline.SharedComponent.Params;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using VDMMutiline.Core;
using VDMMutiline.Core.Signalr;
using static VDMMutiline.SharedComponent.Constants.Constant;
using VDMMutiline.SharedComponent.Models;
using VDMMutiline.Ultilities;
using VDMMutiline.Dao;

namespace VDMMutiline.WebBackEnd.Controllers
{
    public class NotificationController : BaseController
    {
        NotifyBiz _notificationService = new NotifyBiz();
        private BK_BookingDao biz = new BK_BookingDao();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ChuyenBaySapToi()
        {
            var filter = new BK_BookingFilter
            { };
            if (IsAdmin() || GetlistRole().Contains(23))
            {
                filter.ListUserName = new List<string>();
            }
            else filter.ListUserName = GetparentUserDaiLy().Select(z => z.UserName).ToList();
            var param = new BK_BookingParam();
            param.BookingFilter = filter;
            biz.ChuyenBaySapToiView(param);
            return Json(new { list = param.BookingInfos.Take(10), Count = param.BookingInfos.Count }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Details(int? NotificationId)
        {
            if (!NotificationId.HasValue || (NotificationId.Value == 0))
            {
                return Json(new { SenderName = "", Subject = "Kh\x00f4ng t\x00ecm thấy th\x00f4ng b\x00e1o", Body = "" }, JsonRequestBehavior.AllowGet);
            }
            NotificationInfo byID = _notificationService.GetInfo(NotificationId.Value);
            if (byID == null)
            {
                return Json(new { SenderName = "", Subject = "Kh\x00f4ng t\x00ecm thấy th\x00f4ng b\x00e1o", Body = "" }, JsonRequestBehavior.AllowGet);
            }
            byID.ReadStatus = true;
            _notificationService.Update(byID);
            return Json(new { notification = byID }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Delete(int? NotificationId)
        {
            if (!NotificationId.HasValue || (NotificationId.Value == 0))
            {
                return Json(new { SenderName = "", Subject = "Kh\x00f4ng t\x00ecm thấy th\x00f4ng b\x00e1o", Body = "" }, JsonRequestBehavior.AllowGet);
            }
            NotificationInfo byID = _notificationService.GetInfo(NotificationId.Value);
            if (byID == null)
            {
                return Json(new { SenderName = "", Subject = "Kh\x00f4ng t\x00ecm thấy th\x00f4ng b\x00e1o", Body = "" }, JsonRequestBehavior.AllowGet);
            }
            _notificationService.Delete(byID);
            return Json(new { result = true, }, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public ActionResult NotifyForUser(int? Id)
        {
            try
            {
                Notification byID = _notificationService.GetbyId(Id.Value);
                {
                    int? nullable;
                    if (byID.ReceiverID.HasValue && ((byID.ReceiverID.Value == (nullable = byID.SenderID).GetValueOrDefault()) && nullable.HasValue))
                    {
                        //char[] separator = { ',' };
                        //string[] strArray = byID.ReceiverName.Split(separator);
                        //for (int i = 0; i < strArray.Length; i++)
                        //{
                        //    if (!string.IsNullOrEmpty(strArray[0]) && int.TryParse(strArray[i], result: out int num3))
                        //    {
                        //        new NotificationUser(userID: num3.ToString()).NotifyUser(byID.Subject, byID.Body, byID.Link);
                        //    }
                        //}
                    }
                    else if (byID.IsSendAll ?? false)
                    {
                        new NotificationAllUser().NotifyAllUser(byID.Subject, byID.Body, byID.Link);
                    }
                    else
                    {
                        new NotificationUser(byID.ReceiverID.ToString()).NotifyUser(byID.Subject, byID.Body, byID.Link);
                    }
                }
            }
            catch
            {
                return new EmptyResult();
            }
            return new EmptyResult();
        }

        [AllowAnonymous]

        public ActionResult NotifyUser()
        {
            string s = Request.Params["NotifyID"];
            try
            {
                //int.TryParse(s, result: out int result);
                //Notification byID = _notificationService.GetbyId(result);
                //{
                //    int? nullable;
                //    if (byID.ReceiverID.HasValue && ((byID.ReceiverID.Value == (nullable = byID.SenderID).GetValueOrDefault()) && nullable.HasValue))
                //    {
                //        char[] separator = { ',' };
                //        string[] strArray = byID.ReceiverName.Split(separator);
                //        for (int i = 0; i < strArray.Length; i++)
                //        {
                //            if (!string.IsNullOrEmpty(strArray[0]) && int.TryParse(strArray[i], result: out int num3))
                //            {
                //                new NotificationUser(num3.ToString()).NotifyUser(byID.Subject, byID.Body, byID.Link);
                //            }
                //        }
                //    }
                //    else if (byID.IsSendAll ?? false)
                //    {
                //        new NotificationAllUser().NotifyAllUser(byID.Subject, byID.Body, byID.Link);
                //    }
                //    else
                //    {
                //        new NotificationUser(byID.ReceiverID.ToString()).NotifyUser(byID.Subject, byID.Body, byID.Link);
                //    }
                //}
            }
            catch
            {
                return new EmptyResult();
            }
            return new EmptyResult();
        }
        [HttpPost]
        public ActionResult SendMessage(int? Id, string ReceiverName, string Subject, string Body)
        {
            if (((!Id.HasValue || string.IsNullOrWhiteSpace(Subject)) || string.IsNullOrWhiteSpace(ReceiverName)) || string.IsNullOrWhiteSpace(Body))
            {
                return Json(new { isSuccess = false, message = "Th\x00f4ng b\x00e1o chưa được gửi đi." });
            }
            NotificationInfo notification = new NotificationInfo();
            if (Id.Value == 0)
            {
                notification.ReceiverID = new int?(User.Identity.GetUserId<int>());
                notification.IsSendAll = true;
            }
            else
            {
                notification.ReceiverID = new int?(Id.Value);
                notification.IsSendAll = false;
            }
            notification.ReceiverName = ReceiverName;
            notification.SenderName = User.Identity.GetUserName();
            notification.SenderID = new int?(User.Identity.GetUserId<int>());
            notification.SentDate = new DateTime?(DateTime.Now);
            notification.ReadStatus = false;
            notification.Subject = Subject;
            notification.Body = Body;
            try
            {
                _notificationService.Insert(notification);
                NotifyHelper.NotifyUsers(false, notification);
            }
            catch (Exception exception)
            {
                WriteLog(exception, LogType.Error);
                return Json(new { isSuccess = false, message = "Lỗi trong qu\x00e1 tr\x00ecnh gửi th\x00f4ng b\x00e1o. Vui l\x00f2ng li\x00ean hệ kỹ thuật để được hỗ trợ." });
            }
            return Json(new { isSuccess = true, message = "Th\x00f4ng b\x00e1o đ\x00e3 được gửi đi." });
        }

        [HttpPost]
        public ActionResult SendSmsMessage(int? Id, string ReceiverName, string phone, string Body)
        {
            if ((!Id.HasValue || string.IsNullOrWhiteSpace(phone)) || string.IsNullOrWhiteSpace(Body))
            {
                return Json(new { isSuccess = false, message = "Th\x00f4ng b\x00e1o chưa được gửi đi." });
            }
            if (phone.Substring(0, 3) != "084")
            {
                return Json(new { isSuccess = false, message = "Số điện thoại kh\x00f4ng đ\x00fang định dạng" });
            }
            IdentityMessage message = new IdentityMessage
            {
                Body = Body,
                Subject = ReceiverName,
                Destination = phone
            };
            // string str = string.Empty;//this._smsServices.sendSMSMessage(obj.Body_Email, obj.phoneNumber);
            //if (_result.status != "success")
            //{
            //    return Json(new { isSuccess = false, message = "Tin nhắn chưa được gửi đi, vui l\x00f2ng kiểm tra lại số điện thoại người nhận" });
            //}
            NotificationInfo notification = new NotificationInfo();
            if (Id.Value == 0)
            {
                notification.ReceiverID = new int?(User.Identity.GetUserId<int>());
                notification.IsSendAll = true;
            }
            else
            {
                notification.ReceiverID = new int?(Id.Value);
                notification.IsSendAll = false;
            }
            notification.ReceiverName = ReceiverName;
            notification.SenderName = User.Identity.GetUserName();
            notification.SenderID = new int?(User.Identity.GetUserId<int>());
            notification.SentDate = new DateTime?(DateTime.Now);
            notification.ReadStatus = false;
            notification.Subject = "SMS";
            notification.Body = Body;
            try
            {
                _notificationService.Insert(notification);
                NotifyHelper.NotifyUsers(false, notification);
            }
            catch (Exception exception)
            {
                WriteLog(exception, LogType.Error);
                return Json(new { isSuccess = false, message = "Lỗi trong qu\x00e1 tr\x00ecnh gửi th\x00f4ng b\x00e1o. Vui l\x00f2ng li\x00ean hệ kỹ thuật để được hỗ trợ." });
            }
            return Json(new { isSuccess = true, message = "Th\x00f4ng b\x00e1o đ\x00e3 được gửi đi." });
        }

        public ActionResult AjaxGetNewNotifications()
        {
            int userId = User.Identity.GetUserId<int>();
            NotifyParam param = new NotifyParam();
            var finter = new NotifyFilter { UserId = userId };
            param.NotifyFilter = finter;
            _notificationService.GetAllUnRead(param);
            List<NotificationInfo> all = param.Notifications.ToList();
            long count = all.Count;
            var jsonResult = Json(new
            {
                entities = all,
                total = count
            }, JsonRequestBehavior.AllowGet); ;
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public ActionResult AjaxGetNotifications(BaseFilter gridFilterSetting, string keyword)
        {
            UpdateAllNotifications();
            NotifyParam _param = new NotifyParam();
            var finter = new NotifyFilter { UserId = User.Identity.GetUserId<int>() };
            var pagin = new PagingInfo();
            _param.NotifyFilter = finter;
            _param.PagingInfo = pagin;
            pagin.PageSize = gridFilterSetting.Paging.PageSize;
            _notificationService.GetAllIndex(_param);
            List<NotificationInfo> all = _param.Notifications.ToList();
            return Json(new { recordsTotal = pagin.RecordCount, recordsFiltered = pagin.RecordCount, aaData = all });
        }

        public ActionResult AjaxGetNotificationsByReceiver(GridFilterSetting<NotificationInfo> gridFilterSetting, int? receiverid)
        {
            var pagininfo = new PagingInfo { RowStart = gridFilterSetting.iDisplayStart, PageSize = gridFilterSetting.iDisplayLength };
            NotifyParam _param = new NotifyParam() { PagingInfo = pagininfo };
            var finter = new NotifyFilter() { UserId = receiverid };
            var pagin = new PagingInfo();
            _param.NotifyFilter = finter;
            _notificationService.GetListByReceiver(_param);
            long count = pagininfo.RecordCount;
            return Json(new { aaData = _param.Notifications, recordsTotal = count, recordsFiltered = count, amount = 0x2710 }, JsonRequestBehavior.AllowGet);
        }

        private void UpdateAllNotifications()
        {
            try
            {
                int userId = User.Identity.GetUserId<int>();
                NotifyParam param = new NotifyParam();
                var finter = new NotifyFilter { UserId = userId };
                param.NotifyFilter = finter;
                _notificationService.GetAllUnRead(param);
                List<NotificationInfo> list = param.Notifications;
                foreach (NotificationInfo notification in list)
                {
                    notification.ReadStatus = true;
                    _notificationService.Update(notification);
                }
            }
            catch (Exception exception)
            {
                WriteLog(exception, LogType.Error);
            }
        }
    }
}