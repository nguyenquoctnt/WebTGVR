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
using VDMMutiline.SharedComponent.EntityInfo.Issue;
using VDMMutiline.SharedComponent.Params;
using VDMMutiline.Ultilities;
using VDMMutiline.SharedComponent.Entities;
using System.Web.Script.Serialization;
using VDMMutiline.SharedComponent.Params.Issue;
using VDMMutiline.SharedComponent.Params.Wallet;
using VDMMutiline.SharedComponent.Models;
using VDMMutiline.SharedComponent.EntityInfo;
using VDMMutiline.Biz;
using VDMMutiline.WebBackEnd.Common;

namespace VDMMutiline.WebBackEnd.Areas.Issue.Controllers
{
    public class IssueTicketController : BaseController
    {
        ApiClient.Common.ApiClient apiClient = new ApiClient.Common.ApiClient();
        BookingDTC booking = new BookingDTC();
        WalletBiz _bizWallet = new WalletBiz();
        IssueHistoryBiz _bizIssuaHistory = new IssueHistoryBiz();
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
        public ActionResult AjaxLoadList(GridFilterSetting<IssueHistorysInfo> gridFilterSetting, string keyseach, int? ParentId, string FromDate, string ToDate)
        {
            var param = new IssueParam()
            {
                IssueFilter = new IssueFilter()
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
                param.IssueFilter.UserList = new List<string>();
            else
            {
                param.IssueFilter.UserList = GetparentUserDaiLy().Select(z => z.UserName).ToList();
                if (param.IssueFilter.UserList.Count == 0)
                    param.IssueFilter.UserList.Add(CurrentUser.UserName);
            }
            if (ParentId > 0)
            {
                param.IssueFilter.ParentId = ParentId;
            }

            _bizIssuaHistory.Search(param);
            long count = param.PagingInfo.RecordCount;
            return Json(new { aaData = param.IssueHistorysInfos, recordsTotal = count, recordsFiltered = count, amount = 0x2710 }, JsonRequestBehavior.AllowGet);
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
                    var obj = booking.GetBookingByCode(BookingCode,Airline);
                    if (obj != null)
                    {
                        if (Airline != obj.AirlineCode)
                        {
                            ViewBag.erro = "Không tìm thấy booking.";
                        }
                        return RedirectToAction("ConfirmIssue", new { Airline = Airline, BookingCode = BookingCode });
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
        public ActionResult ConfirmIssue(string Airline, string BookingCode)
        {
            var param = new ConfirmIssueInfo();
            if (!string.IsNullOrEmpty(BookingCode))
            {
                if (Airline == Constant.AirlineDomestic.JQ)
                {

                }
                else
                {
                    var obj = booking.GetBookingByCode(BookingCode,Airline);
                    if (obj != null)
                    {
                        if (Airline != obj.AirlineCode)
                        {
                            ViewBag.erro = "Không tìm thấy booking.";
                        }
                        param.BookingInfos = obj;
                        param.IssueInfo = new IssueInfo() { BookingCode = BookingCode, AirlineCode = Airline };
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
        public ActionResult ConfirmIssue(ConfirmIssueInfo param)
        {
            if (CurrentUser.IsIssueTicket == true)
            {
                var obj = booking.GetBookingByCode(param.IssueInfo.BookingCode,param.IssueInfo.AirlineCode);
                var wallerParam = new WalletParam { UserID = CurrentUser.Id };
                _bizWallet.GetInfo(wallerParam);
                var walletInfo = wallerParam.WalletInfo;
                if (walletInfo == null)
                {
                    if (!IsAdmin())
                        return Json(new { isSuccess = false, mess = "Không tìm thấy thông tin ký quỹ." }, JsonRequestBehavior.AllowGet);
                }
                var checkAmount = _bizWallet.CheckKyQuy(CurrentUser.Id, (decimal)(obj.Booking.GrandTotal));
                if (!IsAdmin())
                {
                    if (!checkAmount)
                        return Json(new { isSuccess = false, mess = "Số dư không đủ xuất vé." }, JsonRequestBehavior.AllowGet);
                }
                var request = new IssueRequest()
                {
                    Airline = param.IssueInfo.AirlineCode,
                    BookingCode = param.IssueInfo.BookingCode,
                    HeaderUser = GetKeySetting.HeaderUser,
                    HeaderPass = GetKeySetting.HeaderPassword,
                    AgentAccount = GetKeySetting.Username,
                    AgentPassword = GetKeySetting.Password,
                };
                var res = apiClient.Issue(request);

                if (res.ListTicket == null)
                {
                    res.ListTicket = new List<Ticket>().ToArray();
                }
                if (res.Status && res.ListTicket.Length > 0)
                {
                    var TicketInforJson = new JavaScriptSerializer().Serialize(obj);
                    var RequestJson = new JavaScriptSerializer().Serialize(request);
                    var ResponseJson = new JavaScriptSerializer().Serialize(res);
                    var lst = new List<tblIssueHistory>();
                    foreach (var item in res.ListTicket)
                    {
                        var objIssueHistory = new tblIssueHistory()
                        {
                            BookingCode = item.BookingCode,
                            Status = res.Status ? Constant.IssueHistoryStatus.Success : Constant.IssueHistoryStatus.Fail,
                            Price = (decimal)item.TotalPrice,
                            Remark = param.IssueInfo.Remark,
                            TicketInforJson = TicketInforJson,
                            RequestJson = RequestJson,
                            ResponseJson = ResponseJson,
                            ErrorCode = res.ErrorCode,
                            Message = res.Message,
                            BookingImage = res.BookingImage,
                            CreateDate = DateTime.Now,
                            CreateUser = CurrentUser.UserName,
                            IdWallet = walletInfo.Id,
                            TicketNumber = item.TicketNumber,
                        };
                        lst.Add(objIssueHistory);

                    }
                    var issuaHistoryParam = new IssueParam
                    {
                        IssueHistorys = lst,
                        isAdmin = IsAdmin(),
                    };
                    _bizIssuaHistory.Insert(issuaHistoryParam);
                    return Json(new { isSuccess = true, mess = "Xuất vé thành công." }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { isSuccess = false, mess = res.ErrorCode + " - " + res.Message }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { isSuccess = false, mess = "Bạn không có quyền xuất vé." }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}