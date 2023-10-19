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
using VDMMutiline.SharedComponent.Params.VoidTicket;
using VDMMutiline.SharedComponent.Params.Wallet;
using VDMMutiline.SharedComponent.Models;
using VDMMutiline.SharedComponent.EntityInfo;
using VDMMutiline.Biz;
using VDMMutiline.Biz.VoidTicketHistory;
using VDMMutiline.WebBackEnd.Common;

namespace VDMMutiline.WebBackEnd.Areas.VoidTicket.Controllers
{
    public class VoidController : BaseController
    {
        ApiClient.Common.ApiClient apiClient = new ApiClient.Common.ApiClient();
        BookingDTC booking = new BookingDTC();
        VoidTicketHistoryBiz _bizVoidTicketHistory = new VoidTicketHistoryBiz();
        AspNetUsersBiz aspnetUsersBiz = new AspNetUsersBiz();
        WalletBiz _bizWallet = new WalletBiz();
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
        public ActionResult AjaxLoadList(GridFilterSetting<VoidTicketHistorysInfo> gridFilterSetting, string keyseach, int? ParentId, string FromDate, string ToDate)
        {
            var param = new VoidTicketHistoryParam()
            {
                VoidTicketHistoryFilter = new VoidTicketHistoryFilter()
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
                param.VoidTicketHistoryFilter.UserList = new List<string>();
            else
            {
                param.VoidTicketHistoryFilter.UserList = GetparentUserDaiLy().Select(z => z.UserName).ToList();
                if (param.VoidTicketHistoryFilter.UserList.Count == 0)
                    param.VoidTicketHistoryFilter.UserList.Add(CurrentUser.UserName);
            }
            if (ParentId > 0)
            {
                param.VoidTicketHistoryFilter.ParentId = ParentId;
            }

            _bizVoidTicketHistory.Search(param);
            long count = param.PagingInfo.RecordCount;
            return Json(new { aaData = param.VoidTicketHistorysInfos, recordsTotal = count, recordsFiltered = count, amount = 0x2710 }, JsonRequestBehavior.AllowGet);
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
                        return RedirectToAction("voidIssue", new { Airline = Airline, BookingCode = BookingCode });
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
        public ActionResult voidIssue(string Airline, string BookingCode)
        {
            var param = new ConfirmVoidInfo();
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
                        param.VoidInfo = new VoidInfo() { BookingCode = BookingCode, AirlineCode = Airline };
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
        public ActionResult voidIssue(ConfirmVoidInfo param)
        {
            var obj = booking.GetBookingByCode(param.VoidInfo.BookingCode, param.VoidInfo.AirlineCode);
            if(obj==null)
            {
                return Json(new { isSuccess = false, mess = "Hủy vé không thành công." }, JsonRequestBehavior.AllowGet);
            }    
            var wallerParam = new WalletParam { UserID = CurrentUser.Id };
            _bizWallet.GetInfo(wallerParam);
            var walletInfo = wallerParam.WalletInfo;
            if (walletInfo == null)
            {
                if (!IsAdmin())
                    return Json(new { isSuccess = false, mess = "Không tìm thấy thông tin ký quỹ." }, JsonRequestBehavior.AllowGet);
            }
            var request = new VoidRequest()
            {
                Airline = param.VoidInfo.AirlineCode,
                BookingCode = param.VoidInfo.BookingCode,
                HeaderUser = GetKeySetting.HeaderUser,
                HeaderPass = GetKeySetting.HeaderPassword,
                AgentAccount = GetKeySetting.Username,
                AgentPassword = GetKeySetting.Password,
                CancelBooking = true,
            };
            var res = apiClient.Void(request);
            if (res.Status )
            {

                var ticket = obj.Tickets.FirstOrDefault();
                if(ticket==null)
                {
                    return Json(new { isSuccess = false, mess = "Hủy vé không thành công." }, JsonRequestBehavior.AllowGet);
                }    
                var TicketInforJson = new JavaScriptSerializer().Serialize(obj);
                var RequestJson = new JavaScriptSerializer().Serialize(request);
                var ResponseJson = new JavaScriptSerializer().Serialize(res);
                var objIssueHistory = new tblVoidTicketHistory()
                {
                    BookingCode = param.VoidInfo.BookingCode,
                    Status = res.Status ? Constant.VoidHistoryStatus.Success : Constant.VoidHistoryStatus.Fail,
                    Price = (decimal)obj.Booking.GrandTotal,
                    Remark = param.VoidInfo.Remark,
                    TicketInforJson = TicketInforJson,
                    RequestJson = RequestJson,
                    ResponseJson = ResponseJson,
                    ErrorCode = res.ErrorCode,
                    Message = res.Message,
                    BookingImage = res.BookingImage,
                    CreateDate = DateTime.Now,
                    CreateUser = CurrentUser.UserName,
                    IdWallet = walletInfo.Id,
                };
                var voidTicketHistoryParam = new VoidTicketHistoryParam
                {
                    VoidTicketHistory = objIssueHistory,
                    isAdmin = IsAdmin(),
                };
                _bizVoidTicketHistory.Insert(voidTicketHistoryParam);
                return Json(new { isSuccess = true, mess = "Hủy vé thành công." }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { isSuccess = false, mess = res.ErrorCode + " - " + res.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}