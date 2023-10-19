using ApiClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using VDMMutiline.Biz;
using VDMMutiline.Biz.AddOnServiceHistory;
using VDMMutiline.Biz.Issue;
using VDMMutiline.Biz.Wallet;
using VDMMutiline.Core.UI;
using VDMMutiline.Dao;
using VDMMutiline.SharedComponent.Constants;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
using VDMMutiline.SharedComponent.EntityInfo.AddOnService;
using VDMMutiline.SharedComponent.Models;
using VDMMutiline.SharedComponent.Params;
using VDMMutiline.SharedComponent.Params.AddOnService;
using VDMMutiline.SharedComponent.Params.Wallet;
using VDMMutiline.Ultilities;
using VDMMutiline.WebBackEnd.Common;

namespace VDMMutiline.WebBackEnd.Areas.AddOns.Controllers
{
    public class AddOnServiceController : BaseController
    {
        ApiClient.Common.ApiClient apiClient = new ApiClient.Common.ApiClient();
        BookingDTC booking = new BookingDTC();
        AspNetUsersBiz aspnetUsersBiz = new AspNetUsersBiz();
        AddOnServiceHistoryBiz _biz = new AddOnServiceHistoryBiz();
        BaggageDao bagBiz = new BaggageDao();
        IssueHistoryBiz issueHistoryBiz = new IssueHistoryBiz();
        WalletBiz _bizWallet = new WalletBiz();
        // GET: AddOns/Add
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
        public ActionResult AjaxLoadList(GridFilterSetting<AddOnServiceHistorysInfo> gridFilterSetting, string keyseach, int? ParentId, string FromDate, string ToDate)
        {
            var param = new AddOnServiceParam()
            {
                AddOnServiceHistoryFilter = new AddOnServiceHistoryFilter()
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
                param.AddOnServiceHistoryFilter.UserList = new List<string>();
            else
            {
                param.AddOnServiceHistoryFilter.UserList = GetparentUserDaiLy().Select(z => z.UserName).ToList();
                if (param.AddOnServiceHistoryFilter.UserList.Count == 0)
                    param.AddOnServiceHistoryFilter.UserList.Add(CurrentUser.UserName);
            }
            if (ParentId > 0)
            {
                param.AddOnServiceHistoryFilter.ParentId = ParentId;
            }

            _biz.Search(param);
            long count = param.PagingInfo.RecordCount;
            return Json(new { aaData = param.AddOnServiceHistorysInfos, recordsTotal = count, recordsFiltered = count, amount = 0x2710 }, JsonRequestBehavior.AllowGet);
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
                        return RedirectToAction("AddOnService", new { Airline = Airline, BookingCode = BookingCode });
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
        public ActionResult AddOnService(string Airline, string BookingCode)
        {
            var param = new ConfirmAddOnServiceInfo();
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
                        param.PassgerInfo = new List<PassgerInfo>();
                        var listPass = new List<PassgerInfo>();
                        foreach (var ticket in obj.Tickets)
                        {
                            foreach (var flight in ticket.ListFlight)
                            {
                                foreach (var passger in obj.Passengers)
                                {
                                    var objPassger = new PassgerInfo
                                    {
                                        Id = passger.Id,
                                        Index = passger.Index,
                                        FirstName = passger.FirstName,
                                        LastName = passger.LastName,
                                        Type = passger.Type,
                                        Gender = passger.Gender,
                                        Birthday = passger.Birthday,
                                        Nationality = passger.Nationality,
                                        PassportNumber = passger.PassportNumber,
                                        TicketLeg = flight.Leg,
                                        TicketRoute = flight.StartPoint + flight.EndPoint,
                                    };
                                    if (obj.ListBookingBaggage != null)
                                    {
                                        var objBaggageOld = obj.ListBookingBaggage.FirstOrDefault(z => z.PassengerId == passger.Id && z.Route == (flight.StartPoint + flight.EndPoint));
                                        if (objBaggageOld != null)
                                        {
                                            objPassger.BaggageValue = objBaggageOld.Value;
                                            objPassger.BaggageCode = objBaggageOld.Value;
                                            objPassger.BaggagePrice = objBaggageOld.Price;
                                            objPassger.BaggageRoute = objBaggageOld.Route;
                                        }
                                    }
                                    listPass.Add(objPassger);
                                }
                            }
                        }
                        param.PassgerInfo = listPass;
                        param.AddOnServiceInfo = new AddOnServiceInfo() { BookingCode = BookingCode, AirlineCode = Airline };
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
        public ActionResult AddOnService(ConfirmAddOnServiceInfo param)
        {
            var obj = booking.GetBookingByCode(param.AddOnServiceInfo.BookingCode, param.AddOnServiceInfo.AirlineCode);
            if (obj == null)
            {
                return Json(new { isSuccess = false, mess = "Mua hành lý không thành công." }, JsonRequestBehavior.AllowGet);
            }
            var issueStatus = issueHistoryBiz.GetInfoByCode(param.AddOnServiceInfo.BookingCode);
            if (issueStatus == null)
            {
                return Json(new { isSuccess = false, mess = "Chỉ áp dụng với vé đã xuất." }, JsonRequestBehavior.AllowGet);
            }
            var request = new AddOnServiceRequest()
            {
                Airline = param.AddOnServiceInfo.AirlineCode,
                BookingCode = param.AddOnServiceInfo.BookingCode,
                HeaderUser = GetKeySetting.HeaderUser,
                HeaderPass = GetKeySetting.HeaderPassword,
                AgentAccount = GetKeySetting.Username,
                AgentPassword = GetKeySetting.Password,
            };
            request.ListPassenger = new List<Passenger>();

            foreach (var item in param.PassgerInfo)
            {
                if (!string.IsNullOrEmpty(item.BaggageCode))
                {
                    var objbag = bagBiz.GetbyCode(item.BaggageCode);
                    if (objbag != null)
                    {
                        var passengerobj = new Passenger()
                        {

                            Index = item.Index,
                        };
                        var listbagger = new List<Baggage>();
                        listbagger.Add(new Baggage
                        {
                            Airline = param.AddOnServiceInfo.AirlineCode,
                            Route = item.TicketRoute,
                            Code = objbag.Code,
                            Price = (double)(objbag.Price ?? 0),
                            Value = objbag.Value
                        });
                        passengerobj.ListBaggage = listbagger.ToArray();
                        request.ListPassenger.Add(passengerobj);
                    }
                }
            }
            if (request.ListPassenger.Count > 0)
            {
                var TotalPrice = request.ListPassenger.Select(z => z.ListBaggage.Sum(x => x.Price)).Sum();
                var wallerParam = new WalletParam { UserID = CurrentUser.Id };
                _bizWallet.GetInfo(wallerParam);
                var walletInfo = wallerParam.WalletInfo;
                if (walletInfo == null)
                {
                    if (!IsAdmin())
                        return Json(new { isSuccess = false, mess = "Không tìm thấy thông tin ký quỹ." }, JsonRequestBehavior.AllowGet);
                }
                var checkAmount = _bizWallet.CheckKyQuy(CurrentUser.Id, (decimal)(TotalPrice));
                if (!IsAdmin())
                {
                    if (!checkAmount)
                        return Json(new { isSuccess = false, mess = "Số dư không đủ." }, JsonRequestBehavior.AllowGet);
                }
                var res = apiClient.Addonservice(request);
                var RequestJson = new JavaScriptSerializer().Serialize(request);
                if (res.Status)
                {
                    var ticket = obj.Tickets.FirstOrDefault();
                    if (ticket == null)
                    {
                        return Json(new { isSuccess = false, mess = "Mua hành lý không thành công." }, JsonRequestBehavior.AllowGet);
                    }
                    var TicketInforJson = new JavaScriptSerializer().Serialize(obj);
                    var ResponseJson = new JavaScriptSerializer().Serialize(res);
                    var lsttblAddOnServiceHistory = new List<tblAddOnServiceHistory>();
                    foreach (var item in request.ListPassenger)
                    {
                        var objbagage = item.ListBaggage.FirstOrDefault();
                        if (objbagage != null)
                        {
                            var objIssueHistory = new tblAddOnServiceHistory()
                            {
                                BookingCode = param.AddOnServiceInfo.BookingCode,
                                Status = res.Status ? Constant.CancelHistoryStatus.Success : Constant.CancelHistoryStatus.Fail,
                                FullName = (item.FirstName + " " + item.LastName),
                                Remark = param.AddOnServiceInfo.Remark,
                                CodeBaggage = objbagage.Code,
                                ValueBaggage = objbagage.Value,
                                Price = (decimal)objbagage.Price,
                                TicketInforJson = TicketInforJson,
                                RequestJson = RequestJson,
                                ResponseJson = ResponseJson,
                                ErrorCode = res.ErrorCode,
                                Message = res.Message,
                                CreateDate = DateTime.Now,
                                CreateUser = CurrentUser.UserName,
                                IdWallet= walletInfo.Id,
                            };
                            lsttblAddOnServiceHistory.Add(objIssueHistory);
                        }
                       
                    }
                    var addOnServiceParam = new AddOnServiceParam
                    {
                        AddOnServiceHistorys = lsttblAddOnServiceHistory,
                        isAdmin = IsAdmin(),
                    };
                    _biz.Insert(addOnServiceParam);
                    return Json(new { isSuccess = true, mess = "Thêm hành lý thành công." }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { isSuccess = false, mess = res.ErrorCode + " - " + res.Message }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { isSuccess = false, mess = "Mua hành lý thất bại" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}