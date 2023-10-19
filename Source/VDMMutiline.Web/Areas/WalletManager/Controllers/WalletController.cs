using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VDMMutiline.Biz;
using VDMMutiline.Biz.Wallet;
using VDMMutiline.Core.UI;
using VDMMutiline.SharedComponent.Constants;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
using VDMMutiline.SharedComponent.EntityInfo.Wallet;
using VDMMutiline.SharedComponent.Models;
using VDMMutiline.SharedComponent.Params.Wallet;
using VDMMutiline.Ultilities;

namespace VDMMutiline.WebBackEnd.Areas.WalletManager.Controllers
{
    public class WalletController : BaseController
    {
        AspNetUsersBiz aspnetUsersBiz = new AspNetUsersBiz();
        WalletBiz _biz = new WalletBiz();
        // GET: WalletManager/Wallet
        public ActionResult Index()
        {
            ViewBag.Username = CurrentUser.UserName;
            var param = new VDMMutiline.SharedComponent.Params.AspNetUsersParam()
            {
                AspNetUsersFilter = new VDMMutiline.SharedComponent.Params.AspNetUsersFilter() { },
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
        public ActionResult AjaxLoadList(GridFilterSetting<WalletInfo> gridFilterSetting, string keyseach, int? ParentId)
        {
            var param = new VDMMutiline.SharedComponent.Params.Wallet.WalletParam()
            {
                WalletFilter = new VDMMutiline.SharedComponent.Params.Wallet.WalletFilter() { Search = keyseach },
                PagingInfo = new VDMMutiline.SharedComponent.PagingInfo()
                {
                    PageSize = gridFilterSetting.iDisplayLength,
                    RowStart = gridFilterSetting.iDisplayStart
                }
            };
            if (IsAdmin())
                param.WalletFilter.UserList = new List<string>();
            else
            {
                param.WalletFilter.UserList = GetparentUserDaiLy().Select(z => z.UserName).ToList();
                if (param.WalletFilter.UserList.Count == 0)
                    param.WalletFilter.UserList.Add(CurrentUser.UserName);
            }
            if (ParentId > 0)
            {
                param.WalletFilter.ParentId = ParentId;
            }
            _biz.SearchWallet(param);
            long count = param.PagingInfo.RecordCount;
            return Json(new { aaData = param.WalletInfos, recordsTotal = count, recordsFiltered = count, amount = 0x2710 }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Create(string IdUser)
        {
            var param = new WalletParam();
            param.AspNetUserInfos = GetLstUser();
            param.AspNetUserInfos = param.AspNetUserInfos.Where(z => z.Id != CurrentUser.Id).ToList();
            param.WalletHistorysInfo = new WalletHistorysInfo()
            {
            };
            return View(param);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(WalletParam modelInput)
        {
            try
            {

                if (modelInput != null)
                {
                    var Amount = Utility.ConvertToDecimal(modelInput.WalletHistorysInfo.AmountStr.Replace(",", ""));
                    var bizUser = new AspNetUsersBiz();
                    var objUser = bizUser.GetInfoById(modelInput.WalletHistorysInfo.IdUser);
                    if (objUser == null)
                        return Json(new { isSuccess = false, mess = "Không tìm thấy tài khoản." }, JsonRequestBehavior.AllowGet);
                    if (!IsAdmin())
                    {
                        if (objUser.ParentId != CurrentUser.Id)
                            return Json(new { isSuccess = false, mess = "Chỉ được phép nạp tiền cho thành viên của bạn." }, JsonRequestBehavior.AllowGet);
                        var checkAmount = _biz.CheckKyQuy(CurrentUser.Id, Amount);
                        if (!checkAmount)
                            return Json(new { isSuccess = false, mess = "Không được kỹ quỹ vượt quá số tiền." }, JsonRequestBehavior.AllowGet);
                        if (Amount < 0)
                        {
                            var checkAmountAm = _biz.CheckKyQuy(modelInput.WalletHistorysInfo.IdUser, Math.Abs(Amount ?? 0));
                            if (!checkAmountAm)
                                return Json(new { isSuccess = false, mess = "Ký quỹ âm, số dư của thành viên không đủ." }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        if (objUser.ParentId.HasValue)
                            return Json(new { isSuccess = false, mess = "Chỉ được phép kỹ quỹ cho thành viên của bạn." }, JsonRequestBehavior.AllowGet);
                    }
                    modelInput.UserName = CurrentUser.UserName;
                    modelInput.UserID = modelInput.WalletHistorysInfo.IdUser ?? 0;
                    modelInput.IsAdmin = IsAdmin();
                    modelInput.UserIDSource = CurrentUser.Id;
                    var obj = new tblWalletHistory
                    {
                        Datetime = DateTime.Now,
                        Amount = Amount,
                        NumberOfDocuments = modelInput.WalletHistorysInfo.NumberOfDocuments,
                        Note = modelInput.WalletHistorysInfo.Note,
                        CreateDate = DateTime.Now,
                        CreateUser = CurrentUser.UserName,
                        UpdateDate = DateTime.Now,
                        UpdateUser = CurrentUser.UserName
                    };
                    modelInput.WalletHistory = obj;
                    _biz.Update(modelInput);
                    WriteLog("Nạp tiền vào Wallet", Constant.LogType.Success);
                    return Json(new { isSuccess = true, mess = Constant.Addnewsuccess }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { isSuccess = false, mess = Constant.AcactFail }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                WriteLog("Nạp tiền vào Wallet", Constant.LogType.Success);
                return Json(new { isSuccess = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Historys()
        {
            ViewBag.Username = CurrentUser.UserName;
            var param = new VDMMutiline.SharedComponent.Params.AspNetUsersParam()
            {
                AspNetUsersFilter = new VDMMutiline.SharedComponent.Params.AspNetUsersFilter() { },
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
            aspnetUsersBiz.Search(param);
            if (IsAdmin())
            {
                param.AspNetUsersInfos = param.AspNetUsersInfos.Where(z => z.ParentId == null).ToList();
                ViewBag.IshowAddNew = true;
            }
            else
            {
                param.AspNetUsersInfos = param.AspNetUsersInfos.Where(z => z.ParentId == CurrentUser.Id).ToList();
            }
            param.AspNetUsersInfos.Insert(0, new AspNetUserInfo() { UserName = "[Chọn đại lý]", Id = 0 });
            var reparelist = (param.AspNetUsersInfos.Select(n => new SelectListItem() { Text = n.UserName, Value = n.Id.ToString() })).ToList();
            ViewBag.DataTree = reparelist;
            return View();
        }
        public ActionResult AjaxHistoryLoadList(GridFilterSetting<WalletHistorysInfo> gridFilterSetting, string keyseach, int? ParentId, string FromDate, string ToDate)
        {
            var param = new VDMMutiline.SharedComponent.Params.Wallet.WalletParam()
            {
                WalletFilter = new VDMMutiline.SharedComponent.Params.Wallet.WalletFilter()
                {
                    NumberOfDocuments = keyseach,
                    FromDate = Utility.ConvertToDate(FromDate),
                    ToDate = Utility.ConvertToDate(ToDate)
                },
                PagingInfo = new VDMMutiline.SharedComponent.PagingInfo()
                {
                    PageSize = gridFilterSetting.iDisplayLength,
                    RowStart = gridFilterSetting.iDisplayStart
                }
            };
            if (IsAdmin())
                param.WalletFilter.UserList = new List<string>();
            else
            {
                param.WalletFilter.UserList = GetparentUserDaiLy().Select(z => z.UserName).ToList();
                if (param.WalletFilter.UserList.Count == 0)
                    param.WalletFilter.UserList.Add(CurrentUser.UserName);
            }
            if (ParentId > 0)
            {
                //param.WalletFilter.ParentId = ParentId;
                param.WalletFilter.UserIdCreate = ParentId;
                var bizUser = new AspNetUsersBiz();
                var objUser = bizUser.GetInfoById(ParentId);
                if (objUser != null)
                {
                    param.WalletFilter.UserCreate = objUser.UserName;
                }
                _biz.SearchHistorys(param);
            }
            _biz.SearchHistorys(param);
            long count = param.PagingInfo.RecordCount;
            return Json(new { aaData = param.WalletHistorysInfos, recordsTotal = count, recordsFiltered = count, amount = 0x2710 }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult HistorysByUser(int? UserId)
        {
            var param = new WalletParam { UserID = UserId ?? 0 };
            _biz.GetInfo(param);
            if (param.WalletInfo != null)
            {
                param.WalletInfo.AmountString = (param.WalletInfo.Amount ?? 0).ToString("n0");
            }
            return View(param.WalletInfo);
        }
        public ActionResult AjaxHistoryByUserLoadList(GridFilterSetting<WalletHistorysInfo> gridFilterSetting, int? UserId)
        {
            var param = new VDMMutiline.SharedComponent.Params.Wallet.WalletParam()
            {
                WalletFilter = new VDMMutiline.SharedComponent.Params.Wallet.WalletFilter()
                {

                },
                PagingInfo = new VDMMutiline.SharedComponent.PagingInfo()
                {
                    PageSize = gridFilterSetting.iDisplayLength,
                    RowStart = gridFilterSetting.iDisplayStart
                }
            };
            param.WalletFilter.UserList = new List<string>();
            if (UserId.HasValue == false || UserId == 0)
                UserId = -1;
            param.WalletFilter.UserIdCreate = UserId;
            var bizUser = new AspNetUsersBiz();
            var objUser = bizUser.GetInfoById(UserId);
            if (objUser != null)
            {
                param.WalletFilter.UserCreate = objUser.UserName;
            }
            _biz.SearchHistorys(param);
            long count = param.PagingInfo.RecordCount;
            return Json(new { aaData = param.WalletHistorysInfos, recordsTotal = count, recordsFiltered = count, amount = 0x2710 }, JsonRequestBehavior.AllowGet);
        }
        private List<AspNetUserInfo> GetLstUser()
        {
            var listUser = new List<AspNetUserInfo>();
            var paramUser = new VDMMutiline.SharedComponent.Params.AspNetUsersParam()
            {
                AspNetUsersFilter = new VDMMutiline.SharedComponent.Params.AspNetUsersFilter() { UserList = new List<string>() },
            };
            aspnetUsersBiz.Search(paramUser);
            if (IsAdmin())
            {
                listUser = paramUser.AspNetUsersInfos.Where(z => (z.ParentId == null || z.ParentId.HasValue == false)).ToList();
            }
            else if (IsAgency())
            {
                listUser = paramUser.AspNetUsersInfos.Where(z => z.ParentId == CurrentUser.Id).ToList();
            }
            listUser.Insert(0, new AspNetUserInfo() { UserName = "[Chọn đại lý]", Id = 0 });
            return listUser;
        }
    }
}