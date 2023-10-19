using ApiClient.Common;
using ApiClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VDMMutiline.Biz.Wallet;
using VDMMutiline.Biz.Issue;
using VDMMutiline.Core.UI;
using VDMMutiline.Dao.Issue;
using VDMMutiline.SharedComponent.Constants;
using VDMMutiline.SharedComponent.EntityInfo.VoidTicket;
using VDMMutiline.SharedComponent.Params;
using VDMMutiline.Ultilities;
using VDMMutiline.SharedComponent.Entities;
using System.Web.Script.Serialization;
using VDMMutiline.SharedComponent.Params.CancelTicket;
using VDMMutiline.SharedComponent.Models;
using VDMMutiline.SharedComponent.EntityInfo;
using VDMMutiline.Biz;
using VDMMutiline.Biz.VoidTicketHistory;
using VDMMutiline.WebBackEnd.Common;

namespace VDMMutiline.WebBackEnd.Areas.CancelTicket.Controllers
{
    public class CancelController : BaseController
    {
        ApiClient.Common.ApiClient apiClient = new ApiClient.Common.ApiClient();
        BookingDTC booking = new BookingDTC();
        CancelTicketHistoryBiz _bizVoidTicketHistory = new CancelTicketHistoryBiz();
        AspNetUsersBiz aspnetUsersBiz = new AspNetUsersBiz();
        public ActionResult Index()
        {
            ViewBag.Username = CurrentUser.UserName;
            var param = new AspNetUsersParam()
            {
                AspNetUsersFilter = new AspNetUsersFilter() { },
            };
            if (IsAdmin())
                param.AspNetUsersFilter.UserList = new List<string>();
            else
            {
                param.AspNetUsersFilter.UserList = GetparentUserDaiLy().Select(z => z.UserName).ToList();
                if (param.AspNetUsersFilter.UserList.Count == 0)
                    param.AspNetUsersFilter.UserList.Add(CurrentUser.UserName);
            }
            ViewBag.isadmin = IsAdmin();
            ViewBag.IshowAddNew = (CurrentUser.ParentId.HasValue ? false : true);
            if (IsAdmin())
            {
                ViewBag.IshowAddNew = true;
            }
            aspnetUsersBiz.Search(param);
            param.AspNetUsersInfos = param.AspNetUsersInfos.Where(z => z.ParentId == null).ToList();
            param.AspNetUsersInfos.Insert(0, new AspNetUserInfo() { UserName = "[Chọn đại lý]", Id = 0 });
            var reparelist = (param.AspNetUsersInfos.Select(n => new SelectListItem() { Text = n.UserName, Value = n.Id.ToString() })).ToList();
            ViewBag.DataTree = reparelist;
            return View();
        }
        public ActionResult AjaxLoadList(GridFilterSetting<CancelTicketHistorysInfo> gridFilterSetting, string keyseach, int? ParentId, string FromDate, string ToDate)
        {
            var param = new CancelTicketHistoryParam()
            {
                CancelTicketHistoryFilter = new CancelTicketHistoryFilter()
                {
                    Search = keyseach,
                    FromDate = Utility.ConvertToDate(FromDate),
                    ToDate = Utility.ConvertToDate(ToDate)
                },
                PagingInfo = new SharedComponent.PagingInfo()
                {
                    PageSize = gridFilterSetting.iDisplayLength,
                    RowStart = gridFilterSetting.iDisplayStart
                }
            };
            if (IsAdmin())
                param.CancelTicketHistoryFilter.UserList = new List<string>();
            else
            {
                param.CancelTicketHistoryFilter.UserList = GetparentUserDaiLy().Select(z => z.UserName).ToList();
                if (param.CancelTicketHistoryFilter.UserList.Count == 0)
                    param.CancelTicketHistoryFilter.UserList.Add(CurrentUser.UserName);
            }
            if (ParentId > 0)
            {
                param.CancelTicketHistoryFilter.ParentId = ParentId;
            }

            _bizVoidTicketHistory.Search(param);
            long count = param.PagingInfo.RecordCount;
            return Json(new { aaData = param.CancelTicketHistorysInfos, recordsTotal = count, recordsFiltered = count, amount = 0x2710 }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CheckCode()
        {

            return View();
        }
        [HttpPost]
        public ActionResult CheckCode(string Airline, string BookingCode)
        {

            if (!string.IsNullOrEmpty(BookingCode))
            {
                if (Airline == Constant.AirlineDomestic.JQ)
                {

                }
                else
                {
                    var obj = booking.GetBookingByCode(BookingCode, Airline);
                    if (obj != null)
                    {
                        if (Airline != obj.AirlineCode)
                        {
                            ViewBag.erro = "Không tìm thấy booking.";
                        }
                        return RedirectToAction("CancelIssue", new { Airline = Airline, BookingCode = BookingCode });
                    }
                    else
                    {
                        ViewBag.erro = "Không tìm thấy booking.";
                    }

                }
            }
            else
            {
                ViewBag.erro = "Không tìm thấy booking.";
            }
            return View();
        }
        public ActionResult CancelIssue(string Airline, string BookingCode)
        {
            var param = new ConfirmCancelTicketInfo();
            if (!string.IsNullOrEmpty(BookingCode))
            {
                if (Airline == Constant.AirlineDomestic.JQ)
                {

                }
                else
                {
                    var obj = booking.GetBookingByCode(BookingCode, Airline);
                    if (obj != null)
                    {
                        if (Airline != obj.AirlineCode)
                        {
                            ViewBag.erro = "Không tìm thấy booking.";
                        }
                        param.BookingInfo = obj;
                        param.CancelTicketInfo = new CancelTicketInfo() { BookingCode = BookingCode, AirlineCode = Airline };
                    }
                    else
                    {
                        ViewBag.erro = "Không tìm thấy booking.";
                    }

                }
            }
            else
            {
                ViewBag.erro = "Không tìm thấy booking.";
            }
            return View(param);
        }
        [HttpPost]
        public ActionResult CancelIssue(ConfirmCancelTicketInfo param)
        {
            var obj = booking.GetBookingByCode(param.CancelTicketInfo.BookingCode, param.CancelTicketInfo.AirlineCode);
            if (obj == null)
            {
                return Json(new { isSuccess = false, mess = "Hủy hành trình không thành công." }, JsonRequestBehavior.AllowGet);
            }
            var request = new CancelRequest()
            {
                Airline = param.CancelTicketInfo.AirlineCode,
                BookingCode = param.CancelTicketInfo.BookingCode,
                HeaderUser = GetKeySetting.HeaderUser,
                HeaderPass = GetKeySetting.HeaderPassword,
                AgentAccount = GetKeySetting.Username,
                AgentPassword = GetKeySetting.Password,
            };
            var res = apiClient.Cancel(request);
            var RequestJson = new JavaScriptSerializer().Serialize(request);
            if (res.Status)
            {

                var ticket = obj.Tickets.FirstOrDefault();
                if (ticket == null)
                {
                    return Json(new { isSuccess = false, mess = "Hủy hành trình không thành công." }, JsonRequestBehavior.AllowGet);
                }
                var TicketInforJson = new JavaScriptSerializer().Serialize(obj);
             
                var ResponseJson = new JavaScriptSerializer().Serialize(res);
                var objIssueHistory = new tblCancelTicketHistory()
                {
                    BookingCode = param.CancelTicketInfo.BookingCode,
                    Status = res.Status ? Constant.CancelHistoryStatus.Success : Constant.CancelHistoryStatus.Fail,
                    Remark = param.CancelTicketInfo.Remark,
                    TicketInforJson = TicketInforJson,
                    RequestJson = RequestJson,
                    ResponseJson = ResponseJson,
                    ErrorCode = res.ErrorCode,
                    Message = res.Message,
                    BookingImage = res.BookingImage,
                    CreateDate = DateTime.Now,
                    CreateUser = CurrentUser.UserName,
                };
                var cancelTicketHistoryParam = new CancelTicketHistoryParam
                {
                    CancelTicketHistory = objIssueHistory,
                    isAdmin = IsAdmin(),
                };
                _bizVoidTicketHistory.Insert(cancelTicketHistoryParam);
                return Json(new { isSuccess = true, mess = "Hủy hành trình thành công." }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { isSuccess = false, mess = res.ErrorCode + " - " + res.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}