using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VDMMutiline.Biz;
using VDMMutiline.Core;
using VDMMutiline.Core.UI;
using VDMMutiline.Dao;
using VDMMutiline.SendMailAndSMS.TeampleateVe;
using VDMMutiline.SharedComponent;
using VDMMutiline.SharedComponent.Constants;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
using VDMMutiline.SharedComponent.Params;
using VDMMutiline.Ultilities;
using VDMMutiline.WebBackEnd.Models;
namespace VDMMutiline.WebBackEnd.Areas.Report.Controllers
{
    public class DoanhsoController : BaseController
    {
        private BK_BookingDao biz = new BK_BookingDao();
        // GET: Report/Doanhso
        public ActionResult Index(string TuNgayDenNgay, int? page)
        {
            var biz = new BK_BookingDao();
            var filter = new BK_BookingFilter
            {
                Search = string.Empty
            };
            filter.TuNgayDenNgay = TuNgayDenNgay;
            if (string.IsNullOrEmpty(filter.TuNgayDenNgay))
            {
                filter.TuNgayDenNgay = UIUty.GetFirstDayOfMonth(DateTime.Now).ToString("dd/MM/yyyy") + " - " + UIUty.GetLastDayOfMonth(DateTime.Now).ToString("dd/MM/yyyy");
            }
            if (!string.IsNullOrEmpty(filter.TuNgayDenNgay))
            {
                var tungaydenngay = filter.TuNgayDenNgay.Split('-');
                if (tungaydenngay.Count() > 1)
                {
                    filter.Fromdate = UIUty.GetFirstDayOfMonth(DateTime.Now);
                    filter.Todate = UIUty.GetLastDayOfMonth(DateTime.Now);
                }
            }
            var param = new BK_BookingParam { };
            if (IsAdmin() || GetlistRole().Contains(23))
            {
                filter.ListUserName = new List<string>();
            }
            else filter.ListUserName = GetparentUserDaiLy().Select(z => z.UserName).ToList();
            param.UserInfos = GetlistUser();
            param.UserInfos.Insert(0, new AspNetUserInfo { UserName = "Tất cả" });
            if (param.UserInfos.Count > 1)
                param.UserInfos.Insert(1, new AspNetUserInfo { UserName = Constant.KL });
            else param.UserInfos.Add(new AspNetUserInfo { UserName = Constant.KL });
            param.BookingFilter = filter;
            biz.Doanhso(param);
            return View(param);
        }
        [HttpPost]
        public ActionResult Index(BK_BookingParam param, int? page)
        {
            var biz = new BK_BookingDao();
            var filter = param.BookingFilter;
            if (string.IsNullOrEmpty(filter.TuNgayDenNgay))
            {
                filter.TuNgayDenNgay = UIUty.GetFirstDayOfMonth(DateTime.Now).ToString("dd/MM/yyyy") + " - " + UIUty.GetLastDayOfMonth(DateTime.Now).ToString("dd/MM/yyyy");
            }
            if (!string.IsNullOrEmpty(filter.TuNgayDenNgay))
            {
                var tungaydenngay = filter.TuNgayDenNgay.Split('-');
                if (tungaydenngay.Count() > 1)
                {
                    filter.Fromdate = Utility.ConvertToDate(tungaydenngay[0].Trim());
                    filter.Todate = Utility.ConvertToDate(tungaydenngay[1].Trim());
                }
            }
            if (filter.TaiKhoan == "Tất cả")
                filter.TaiKhoan = "";

            if (IsAdmin() || GetlistRole().Contains(23))
            {
                filter.ListUserName = new List<string>();
            }
            else filter.ListUserName = GetparentUserDaiLy().Select(z => z.UserName).ToList();
            param.UserInfos = GetlistUser();
            param.UserInfos.Insert(0, new AspNetUserInfo { UserName = "Tất cả" });
            if (param.UserInfos.Count > 1)
                param.UserInfos.Insert(1, new AspNetUserInfo { UserName = Constant.KL });
            else param.UserInfos.Add(new AspNetUserInfo { UserName = Constant.KL });
            param.BookingFilter = filter;
            biz.Doanhso(param);
            return View(param);
        }
        private List<AspNetUserInfo> GetlistUser()
        {
            AspNetUsersBiz aspnetUsersBiz = new AspNetUsersBiz();
            var param = new VDMMutiline.SharedComponent.Params.AspNetUsersParam()
            {
                AspNetUsersFilter = new VDMMutiline.SharedComponent.Params.AspNetUsersFilter() { },
            };
            if (IsAdmin() || GetlistRole().Contains(23))
                param.AspNetUsersFilter.UserList = new List<string>();
            else if (GetlistRole().Contains(22))
            {
                param.AspNetUsersFilter.UserList = GetparentUserDaiLy(CurrentUser.ParentId ?? 0).Select(z => z.UserName).ToList();
            }
            else param.AspNetUsersFilter.UserList = GetparentUserDaiLy().Select(z => z.UserName).ToList();
            aspnetUsersBiz.Search(param);
            return param.AspNetUsersInfos;
        }


        public ActionResult ThongKebooking()
        {
            var filter = new BK_BookingFilter
            {
            };
            if (IsAdmin() || GetlistRole().Contains(23))
            {
                filter.ListUserName = new List<string>();
            }
            else filter.ListUserName = GetparentUserDaiLy().Select(z => z.UserName).ToList();
            var param = new BK_BookingParam();
            param.BookingFilter = filter;
             var paramcount = param;
            biz.SearchCount(paramcount);
            param.TicketCountInfos = paramcount.TicketCountInfos;
            return View(param);
        }
    }
}