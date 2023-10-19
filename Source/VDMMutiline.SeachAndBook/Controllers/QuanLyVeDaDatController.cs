using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VDMMutiline.Biz;
using VDMMutiline.SharedComponent;
using VDMMutiline.SharedComponent.EntityInfo;
using VDMMutiline.SharedComponent.Params;
using VDMMutiline.Core.UI;
using System.Net.Mail;
using VDMMutiline.Core;
using VDMMutiline.Ultilities;
using VDMMutiline.Dao;
using PagedList;
using VDMMutiline.SharedComponent.Constants;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SendMailAndSMS.TeampleateVe;
using System.IO;
using System.Net.Mime;
using VDMMutiline.SeachAndBook.Models;

namespace VDMMutiline.SeachAndBook.Controllers
{
    public class QuanLyVeDaDatController : BaseController
    {
        public ActionResult Menu(string key)
        {
            return View(key);
        }
        private BK_BookingDao biz = new BK_BookingDao();
        // GET: QuanLyVeDaDat
        public ActionResult Index(int? page, string thoigiandatcho, string booker, int? trangthaive, string hangvanchuyen, string seach)
        {
            TempData["keymenu"] = "Index";
            // TempData["keymenu"] = "Index";
            var filter = new BK_BookingFilter
            {
                Search = seach
            };
            if (IsAdmin())
            {
                filter.ListUserName = new List<string>();
            }
            else filter.ListUserName = GetparentUserDaiLy().Select(z => z.UserName).ToList();
            if (!string.IsNullOrEmpty(thoigiandatcho))
            {
                var tungaydenngay = thoigiandatcho.Split('-');
                if (tungaydenngay.Count() > 1)
                {
                    filter.Fromdate = Utility.ConvertToDate(tungaydenngay[0].Trim());
                    filter.Todate = Utility.ConvertToDate(tungaydenngay[1].Trim());
                }
            }
            if (trangthaive == 0)
                filter.StatusVe = null;
            else
                filter.StatusVe = trangthaive;
            var param = new BK_BookingParam();
            param.UserInfos = GetlistUser();
            int pageNumber = (page ?? 1);
            int pageSize = 100;
            var pInfo = new PagingInfo(pageNumber, pageSize);
            param.BookingFilter = filter;
            param.PagingInfo = pInfo;
            param.UserInfos.Insert(0, new AspNetUserInfo { UserName = "Tất cả" });
            if (param.UserInfos.Count > 1)
                param.UserInfos.Insert(1, new AspNetUserInfo { UserName = Constant.KL });
            else param.UserInfos.Add(new AspNetUserInfo { UserName = Constant.KL });
            param.AirlineInfos = GetAirline();
            biz.Search(param);
            ViewBag.userinfor = CurrentUser;
            ViewBag.showchonuser = (IsAdmin() && CurrentUser.ParentId.HasValue == false);
            param.PagedList = new StaticPagedList<BK_BookingInfo>(param.BookingInfos, param.PagingInfo.PageIndex, param.PagingInfo.PageSize, param.PagingInfo.RecordCount);
            return View(param);
        }
        [HttpPost]
        public ActionResult Index(BK_BookingParam param, int? page)
        {
            TempData["keymenu"] = "Index";
            var filter = param.BookingFilter;
            if (IsAdmin() || GetlistRole().Contains(23))
            {
                filter.ListUserName = new List<string>();
            }
            else filter.ListUserName = GetparentUserDaiLy().Select(z => z.UserName).ToList();
            if (!string.IsNullOrEmpty(filter.TuNgayDenNgay))
            {
                var tungaydenngay = filter.TuNgayDenNgay.Split('-');
                if (tungaydenngay.Count() > 1)
                {
                    filter.Fromdate = Utility.ConvertToDate(tungaydenngay[0].Trim());
                    filter.Todate = Utility.ConvertToDate(tungaydenngay[1].Trim());
                }
            }
            if (filter.StatusVe == 0)
                filter.StatusVe = null;
            if (filter.TaiKhoan == "Tất cả")
                filter.TaiKhoan = "";
            param.UserInfos = GetlistUser();
            int pageNumber = (page ?? 1);
            int pageSize = 100;
            var pInfo = new PagingInfo(pageNumber, pageSize);
            param.BookingFilter = filter;
            param.PagingInfo = pInfo;
            param.UserInfos.Insert(0, new AspNetUserInfo { UserName = "Tất cả" });
            if (param.UserInfos.Count > 1)
                param.UserInfos.Insert(1, new AspNetUserInfo { UserName = Constant.KL });
            else param.UserInfos.Add(new AspNetUserInfo { UserName = Constant.KL });
            param.AirlineInfos = GetAirline();
            biz.Search(param);
            ViewBag.userinfor = CurrentUser;
            ViewBag.showchonuser = (IsAdmin() && CurrentUser.ParentId.HasValue == false);
            param.PagedList = new StaticPagedList<BK_BookingInfo>(param.BookingInfos, param.PagingInfo.PageIndex, param.PagingInfo.PageSize, param.PagingInfo.RecordCount);
            return View(param);
        }
        public ActionResult Expired(int? page, string booker, string hangvanchuyen, string seach)
        {
            TempData["keymenu"] = "Expired";
            var filter = new BK_BookingFilter
            {
                Search = seach
            };
            if (IsAdmin())
            {
                filter.ListUserName = new List<string>();
            }
            else filter.ListUserName = GetparentUserDaiLy().Select(z => z.UserName).ToList();
            filter.StatusVe = Constant.StatusVe.DangGiuCho;
            filter.Vehethangiucho = true;
            var param = new BK_BookingParam();
            param.UserInfos = GetlistUser();
            int pageNumber = (page ?? 1);
            int pageSize = 100;
            var pInfo = new PagingInfo(pageNumber, pageSize);
            param.BookingFilter = filter;
            param.PagingInfo = pInfo;
            param.UserInfos.Insert(0, new AspNetUserInfo { UserName = "Tất cả" });
            if (param.UserInfos.Count > 1)
                param.UserInfos.Insert(1, new AspNetUserInfo { UserName = Constant.KL });
            else param.UserInfos.Add(new AspNetUserInfo { UserName = Constant.KL });
            param.AirlineInfos = GetAirline();
            biz.Search(param);
            ViewBag.userinfor = CurrentUser;
            ViewBag.showchonuser = (IsAdmin() && CurrentUser.ParentId.HasValue == false);
            param.PagedList = new StaticPagedList<BK_BookingInfo>(param.BookingInfos, param.PagingInfo.PageIndex, param.PagingInfo.PageSize, param.PagingInfo.RecordCount);
            return View(param);
        }
        [HttpPost]
        public ActionResult Expired(BK_BookingParam param, int? page)
        {
            TempData["keymenu"] = "Expired";
            var filter = param.BookingFilter;
            if (IsAdmin())
            {
                filter.ListUserName = new List<string>();
            }
            else filter.ListUserName = GetparentUserDaiLy().Select(z => z.UserName).ToList();
            filter.StatusVe = Constant.StatusVe.DangGiuCho;
            filter.Vehethangiucho = true;
            param.UserInfos = GetlistUser();
            int pageNumber = (page ?? 1);
            int pageSize = 100;
            var pInfo = new PagingInfo(pageNumber, pageSize);
            param.BookingFilter = filter;
            param.PagingInfo = pInfo;
            param.UserInfos.Insert(0, new AspNetUserInfo { UserName = "Tất cả" });
            if (param.UserInfos.Count > 1)
                param.UserInfos.Insert(1, new AspNetUserInfo { UserName = Constant.KL });
            else param.UserInfos.Add(new AspNetUserInfo { UserName = Constant.KL });
            param.AirlineInfos = GetAirline();
            biz.Search(param);
            ViewBag.userinfor = CurrentUser;
            ViewBag.showchonuser = (IsAdmin() && CurrentUser.ParentId.HasValue == false);
            param.PagedList = new StaticPagedList<BK_BookingInfo>(param.BookingInfos, param.PagingInfo.PageIndex, param.PagingInfo.PageSize, param.PagingInfo.RecordCount);
            return View(param);
        }
        public ActionResult Fail(int? page, string booker, string hangvanchuyen, string seach)
        {
            TempData["keymenu"] = "Fail";
            var filter = new BK_BookingFilter
            {
                Search = seach
            };
            if (IsAdmin())
            {
                filter.ListUserName = new List<string>();
            }
            else filter.ListUserName = GetparentUserDaiLy().Select(z => z.UserName).ToList();
            filter.StatusVe = Constant.StatusVe.DangGiuCho;
            filter.DatVeThatBai = true;
            var param = new BK_BookingParam();
            param.UserInfos = GetlistUser();
            int pageNumber = (page ?? 1);
            int pageSize = 100;
            var pInfo = new PagingInfo(pageNumber, pageSize);
            param.BookingFilter = filter;
            param.PagingInfo = pInfo;
            param.UserInfos.Insert(0, new AspNetUserInfo { UserName = "Tất cả" });
            if (param.UserInfos.Count > 1)
                param.UserInfos.Insert(1, new AspNetUserInfo { UserName = Constant.KL });
            else param.UserInfos.Add(new AspNetUserInfo { UserName = Constant.KL });
            param.AirlineInfos = GetAirline();
            biz.Search(param);
            ViewBag.userinfor = CurrentUser;
            ViewBag.showchonuser = (IsAdmin() && CurrentUser.ParentId.HasValue == false);
            param.PagedList = new StaticPagedList<BK_BookingInfo>(param.BookingInfos, param.PagingInfo.PageIndex, param.PagingInfo.PageSize, param.PagingInfo.RecordCount);
            return View(param);
        }
        [HttpPost]
        public ActionResult Fail(BK_BookingParam param, int? page)
        {
            TempData["keymenu"] = "Fail";
            var filter = param.BookingFilter;
            if (IsAdmin())
            {
                filter.ListUserName = new List<string>();
            }
            else filter.ListUserName = GetparentUserDaiLy().Select(z => z.UserName).ToList();
            filter.StatusVe = Constant.StatusVe.DangGiuCho;
            filter.DatVeThatBai = true;
            param.UserInfos = GetlistUser();
            int pageNumber = (page ?? 1);
            int pageSize = 100;
            var pInfo = new PagingInfo(pageNumber, pageSize);
            param.BookingFilter = filter;
            param.PagingInfo = pInfo;
            param.UserInfos.Insert(0, new AspNetUserInfo { UserName = "Tất cả" });
            if (param.UserInfos.Count > 1)
                param.UserInfos.Insert(1, new AspNetUserInfo { UserName = Constant.KL });
            else param.UserInfos.Add(new AspNetUserInfo { UserName = Constant.KL });
            param.AirlineInfos = GetAirline();
            biz.Search(param);
            ViewBag.userinfor = CurrentUser;
            ViewBag.showchonuser = (IsAdmin() && CurrentUser.ParentId.HasValue == false);
            param.PagedList = new StaticPagedList<BK_BookingInfo>(param.BookingInfos, param.PagingInfo.PageIndex, param.PagingInfo.PageSize, param.PagingInfo.RecordCount);
            return View(param);
        }
        public ActionResult DanhsachTicket(int? page)
        {
            TempData["keymenu"] = "DanhsachTicket";
            int pageNumber = (page ?? 1);
            int pageSize = 20;
            var pInfo = new PagingInfo(pageNumber, pageSize);
            var param = new BK_TicketParamParam { PagingInfo = pInfo };
            if (IsAdmin())
            {
                param.ListUserName = new List<string>();
            }
            else param.ListUserName = GetparentUserDaiLy().Select(z => z.UserName).ToList();
            var biz = new BK_TicketDao();
            var list = biz.GetAlltake200(param);
            param.PagedList = new StaticPagedList<BK_TicketInfo>(list, param.PagingInfo.PageIndex, param.PagingInfo.PageSize, param.PagingInfo.RecordCount);

            return View(param);
        }
        [HttpPost]
        public ActionResult DanhsachTicket(BK_TicketParamParam param, int? page)
        {
            TempData["keymenu"] = "DanhsachTicket";
            var biz = new BK_TicketDao();
            if (!string.IsNullOrEmpty(param.TuNgayDenNgay))
            {
                var tungaydenngay = param.TuNgayDenNgay.Split('-');
                if (tungaydenngay.Count() > 1)
                {
                    param.Fromdate = Utility.ConvertToDate(tungaydenngay[0].Trim());
                    param.Todate = Utility.ConvertToDate(tungaydenngay[1].Trim());
                }
            }
            if (IsAdmin())
            {
                param.ListUserName = new List<string>();
            }
            else param.ListUserName = GetparentUserDaiLy().Select(z => z.UserName).ToList();
            int pageNumber = (page ?? 1);
            int pageSize = 30;
            var pInfo = new PagingInfo(pageNumber, pageSize);
            param.PagingInfo = pInfo;
            var list = biz.GetAlltake200(param);
            param.PagedList = new StaticPagedList<BK_TicketInfo>(list, param.PagingInfo.PageIndex, param.PagingInfo.PageSize, param.PagingInfo.RecordCount);
            return View(param);

        }
        public ActionResult seachCreate(string code)
        {
            var biz = new BK_BookingDao();
            var param = biz.GetDetail(code);
            if (param != null)
                return RedirectToAction("Create", new { id = param.ID });
            else
            {
                return RedirectToAction("Create", new { id = 0, codes = code });
            }
        }

        #region Edit Vé
        public ActionResult Delete(int id, string ReturnUrl)
        {
            var biz = new BK_BookingDao();
            var param = new BK_BookingParam();
            param.Booking = biz.GetbyId(id);
            param.Id = id;
            param.RetunUrl = ReturnUrl;
            return View(param);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(BK_BookingParam param, FormCollection collection)
        {
            try
            {
                var biz = new BK_BookingDao();
                var itemdelete = biz.GetbyId(param.Id);
                itemdelete.DeletedUser = CurrentUser.UserName;
                var bokingdao = new BK_PassengerDao();
                try
                {
                    LogMain.LogBooking(param.Id, CurrentUser.UserName, "", "", Constant.LogBookingType.Xoabooking);
                }
                catch
                {
                }
                biz.Delete(itemdelete);
                if (!string.IsNullOrEmpty(param.RetunUrl))
                    return new RedirectResult(Server.UrlDecode(param.RetunUrl));
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        public ActionResult Create(int id, string codes)
        {
            var biz = new BK_BookingDao();
            var param = biz.GetDetail(id);
            if (param != null)
            {
                if (string.IsNullOrWhiteSpace(param.BookCode))
                    param.BookCode = param.FailCode;
            }
            ViewBag.codes = codes;
            ViewBag.userinfor = CurrentUser;
            return View(param);
        }
        public ActionResult CapNhapghichu(int? bookingid, string note)
        {
            var param = new BK_BookingParam { Id = bookingid ?? 0, note = note };
            return View(param);
        }
        [HttpPost]
        public ActionResult CapNhapghichu(BK_BookingParam param)
        {
            var ticketdao = new BK_BookingDao();
            var objold = ticketdao.GetbyId(param.Id);
            ticketdao.Updateghichu(param.Id, param.note);
            try
            {
                if (objold != null)
                {
                    string noidungcu = "";
                    noidungcu += "Ghi chú: " + objold.Note;
                    string noidungmoi = "";
                    noidungmoi += "Ghi chú: " + param.note;
                    LogMain.LogBooking(param.Id, CurrentUser.UserName, noidungcu, noidungmoi, Constant.LogBookingType.capnhapghichu);
                }
            }
            catch
            {

            }
            return RedirectToAction("Create", "QuanLyVeDaDat", new { id = param.Id });
        }
        public ActionResult Inve(int bookingid)
        {
            string filename = "Inve.pdf";
            var dao = new BK_BookingDao();
            var inforbk = dao.GetInfo(bookingid);
            if (inforbk != null)
            {

                var userInfo = GetInforByUserOrUserParent(inforbk.UserCreate);
                var listseting = GetSettingByUser(inforbk.UserCreate, true);
                string html = RenderTeamPletave.TeampleatevePDF(inforbk, listseting, UIUty.GetIdPdfTempleate(inforbk), userInfo.Id);
                filename = inforbk.CreatedDate.HasValue ? inforbk.CreatedDate.Value.ToString("dd/MM/yyyy").Replace('/', '_') : "";
                filename += "_" + inforbk.BookCode;
                filename += "_" + Utility.ConvertToUnsign(inforbk.Name).Replace(' ', '_');
                filename += "_" + inforbk.FromCity + inforbk.ToCity;
                var export = new ExportPDF();
                var file = export.ExportPdf(html, filename);
                return file;
            }
            return RedirectToAction("Create", "DanhSachVe", new { id = bookingid });
        }
        public ActionResult Yeucauxuathoadon(int? bookingid, string Hotennguoixuly, string ngayxuly, bool? Daxuly)
        {
            var param = new BK_BookingParam();
            param.bookingid = bookingid;
            param.HoTenNguoixuly = Hotennguoixuly;
            param.Daxuly = Daxuly;
            param.Ngayxuly = ngayxuly;
            return View(param);
        }
        [HttpPost]
        public ActionResult Yeucauxuathoadon(BK_BookingParam param)
        {
            try
            {
                var ticketdao = new BK_BookingDao();
                var obj = ticketdao.UpdateXuatHoaDon(param.bookingid, CurrentUser.UserName, param.HoTenNguoixuly, param.Daxuly ?? false, Utility.ConvertToDate(param.Ngayxuly));
            }
            catch { }
            return RedirectToAction("Create", "QuanLyVeDaDat", new { id = param.bookingid });
        }
        public ActionResult Updatehinhthucthanhtoan(int? bookingid)
        {
            var dao = new BK_BookingDao();
            var obj = dao.GetInfoThanhToan(bookingid);
            if (obj == null)
            {
                obj = new tbl_PaymentInfo();
                obj.IdBooking = bookingid;
                obj.IdHinhThuc = Constant.Hinhthucthanhtoav2.chuyenkhoan;
            }
            return View(obj);
        }
        [HttpPost]
        public ActionResult Updatehinhthucthanhtoan(tbl_PaymentInfo modelinput)
        {
            try
            {
                var BookingDao = new BK_BookingDao();
                BookingDao.UpdatePayment(modelinput);
            }
            catch
            {
            }
            return RedirectToAction("Create", "QuanLyVeDaDat", new { id = modelinput.IdBooking });
        }
        public ActionResult sendsms(int bookingid)
        {
            var dao = new BK_BookingDao();
            var inforbk = dao.GetInfo(bookingid);
            if (inforbk != null)
            {
               // ViewBag.usercreadInfo = GetSettingByUser(inforbk.UserCreate, true);
                //voidsendsms(inforbk);
                // return RedirectToAction("Create", "DanhSachVe", new { id = inforbk.ID });
            }
            return View(inforbk);
        }
        [HttpPost]
        public ActionResult sendsms(BK_BookingInfo BookingInfo)
        {
            var dao = new BK_BookingDao();
            var inforbk = dao.GetInfo(BookingInfo.ID);
            if (inforbk != null)
            {
                string phoneold = inforbk.Phone;
                dao.UpdateSdt(BookingInfo.ID, BookingInfo.Phone);
                inforbk = dao.GetInfo(BookingInfo.ID);
                // voidsendsms(inforbk, "");
                try
                {
                    LogMain.LogBooking(BookingInfo.ID, CurrentUser.UserName, phoneold, inforbk.Phone, Constant.LogBookingType.sendsms);
                }
                catch
                {

                }
                return RedirectToAction("Create", "QuanLyVeDaDat", new { id = inforbk.ID });
            }
            return View();
        }
        [HttpGet]
        public ActionResult sendmail(int bookingid, string Email)
        {
            var mail = new MailInfo { Bookingid = bookingid, Mail = Email };
            return View(mail);
        }
        [HttpPost]
        public ActionResult sendmail(MailInfo Mailinfo)
        {
            var dao = new BK_BookingDao();
            var inforbk = dao.GetInfo(Mailinfo.Bookingid);
            if (inforbk != null)
            {
                var listseting = GetSettingByUser(inforbk.UserCreate, true);

                var tieude = "Xác nhận đặt vé | mã đặt chỗ " + inforbk.BookCode + " | " + inforbk.Name + " | " + inforbk.FromCity + " " + inforbk.ToCity;
                var list = new List<MailAddress> { new MailAddress(Mailinfo.Mail.Trim()) };

                var fromAddress = UIUty.GetsettingByKey(listseting, "PRT_EMAIL");
                var fromPassword = UIUty.GetsettingByKey(listseting, "PRT_EMAIL_Pass");
                var port = UIUty.GetsettingByKey(listseting, "EmailSender_SmtpPort");
                var Host = UIUty.GetsettingByKey(listseting, "EmailSender_SmtpClient");
                // string urlsource = base.Server.MapPath("~/TeampleateVe/TeampletaveSendmail.html");

                var userInfo = GetInforByUserOrUserParent(inforbk.UserCreate);
                string html = RenderTeamPletave.Teampleateve(inforbk, listseting, Constant.TemPleateHTMLID.VeXacNhanHanhTrinh, userInfo.Id);
                if (inforbk.Status == Constant.StatusVe.DaXuatVe)
                {
                    html = RenderTeamPletave.Teampleateve(inforbk, listseting, Constant.TemPleateHTMLID.KichHoatVe, userInfo.Id);
                    tieude = "Đã kích hoạt vé | mã xác nhận [" + inforbk.BookCode + "] | " +
                                            inforbk.Name +
                                            " | " + inforbk.FromCity + " " + inforbk.ToCity;
                }
                else if (inforbk.Status == Constant.StatusVe.DaHuyVe)
                {
                    html = RenderTeamPletave.Teampleateve(inforbk, listseting, Constant.TemPleateHTMLID.HuyVe, userInfo.Id);
                    tieude = "Hủy | [" + inforbk.BookCode + "] | " +
                                           inforbk.Name +
                                           " | " +
                                           inforbk.FromCity + " " + inforbk.ToCity;
                }
                string filename = "";
                filename = inforbk.CreatedDate.HasValue ? inforbk.CreatedDate.Value.ToString("dd/MM/yyyy").Replace('/', '_') : "";
                filename += "_" + inforbk.BookCode;
                filename += "_" + Utility.ConvertToUnsign(inforbk.Name).Replace(' ', '_');
                filename += "_" + inforbk.FromCity + inforbk.ToCity;
                filename += ".pdf";
                var export = new ExportPDF();
                string htmlPDF = RenderTeamPletave.TeampleatevePDF(inforbk, listseting, UIUty.GetIdPdfTempleate(inforbk), userInfo.Id);
                var file = export.ExportPdfReturnStream(htmlPDF);
                var att = new Attachment(file, filename + ".pdf", "application/pdf");
                /// write log
                try
                {
                    LogMain.LogBooking(inforbk.ID, CurrentUser.UserName, inforbk.Email, inforbk.Email, Constant.LogBookingType.sendmail);
                }
                catch
                {

                }

                MailUtily.sendmail(listseting, tieude, html, list, att,
                   fromAddress, fromPassword, Utility.ConvertToInt(port), Host);
            }
            return RedirectToAction("Create", "QuanLyVeDaDat", new { id = Mailinfo.Bookingid });
        }
        public ActionResult CapNhapTrangThai(int? bookingid, string Code, int? TrangThai, string TaiKhoan, string ngay, string gio)
        {
            var param = new BK_BookingParam();
            param.bookingid = bookingid;
            param.Code = Code;
            param.TrangThai = TrangThai;
            param.TaiKhoan = TaiKhoan;
            param.UserInfos = GetlistUser();
            if (param.UserInfos.Count > 1)
                param.UserInfos.Insert(1, new AspNetUserInfo { UserName = Constant.KL });
            else param.UserInfos.Add(new AspNetUserInfo { UserName = Constant.KL });
            param.Ngay = ngay;
            param.Gio = gio;
            return View(param);
        }
        [HttpPost]
        public ActionResult CapNhapTrangThai(BK_BookingParam param)
        {

            var ticketdao = new BK_BookingDao();
            string giatringaybatdau = "";
            string giatrigiobatdau = "";
            var ngaybatdaulv = param.Ngay.Split('/');
            var giobatdauvl = param.Gio.Split(':');
            if (param.Ngay.Length >= 3)
            {
                giatringaybatdau = (ngaybatdaulv[0].Length == 1 ? ("0" + ngaybatdaulv[0]) : ngaybatdaulv[0]) + "/" + (ngaybatdaulv[1].Length == 1 ? ("0" + ngaybatdaulv[1]) : ngaybatdaulv[1]) + "/" + ngaybatdaulv[2];
            }
            if (giobatdauvl.Length >= 2)
            {
                giatrigiobatdau = (giobatdauvl[0].Length == 1 ? ("0" + giobatdauvl[0]) : giobatdauvl[0]) + ":" + (giobatdauvl[1].Length == 1 ? ("0" + giobatdauvl[1]) : giobatdauvl[1]);
            }
            DateTime? han = Utility.ConvertToDatetime(giatringaybatdau + " " + giatrigiobatdau);
            var objcheck = ticketdao.GetInfo(param.bookingid.HasValue ? param.bookingid.Value : 0);
            if (objcheck != null)
            {
                var obj = ticketdao.UpdateTrangThai(param.bookingid, param.TrangThai, param.Code, param.TaiKhoan, han, objcheck.Sendmailgannhat);
                /// write log
                try
                {
                    string noidungcu = "";
                    noidungcu += "Code: " + objcheck.BookCode;
                    noidungcu += "</br>Trạng thái: " + Utility.GetDictionaryValue(Constant.StatusVe.dctName, objcheck.Status);
                    noidungcu += "</br>Booker: " + objcheck.UserId;
                    noidungcu += "</br>Thời gian giữ chỗ: " + Utility.GetDateMinuteString(objcheck.Expireddate);

                    string noidungmoi = "";
                    noidungmoi += "Code: " + obj.BookCode;
                    noidungmoi += "</br>Trạng thái: " + Utility.GetDictionaryValue(Constant.StatusVe.dctName, obj.Status);
                    noidungmoi += "</br>Booker: " + obj.UserId;
                    noidungmoi += "</br>Thời gian giữ chỗ: " + Utility.GetDateMinuteString(obj.Expireddate);
                    LogMain.LogBooking(objcheck.ID, CurrentUser.UserName, noidungcu, noidungmoi, Constant.LogBookingType.capnhaptrangthai);
                }
                catch
                {

                }
                if (obj != null)
                {
                    if (objcheck.Sendmailgannhat.HasValue == false ||
                        objcheck.Sendmailgannhat < DateTime.Now.AddMinutes(-1))
                    {

                        if (obj.Status == Constant.StatusVe.DaXuatVe)
                        {
                            var dao = new BK_BookingDao();
                            var inforbk = dao.GetInfo(obj.ID);
                            if (inforbk != null)
                            {
                                var listseting = GetSettingByUser(inforbk.UserCreate, true);
                                var tieude = "Đã kích hoạt vé | mã xác nhận [" + inforbk.BookCode + "] | " +
                                             inforbk.Name +
                                             " | " + inforbk.FromCity + " " + inforbk.ToCity;
                                var list = new List<MailAddress> { new MailAddress(inforbk.Email) };
                                //  string urlsource =Server.MapPath("~/TeampleateVe/TeampletaveSendmailXacNhan.html");
                                var userSendMail = GetInforByUserOrUserParent(inforbk.UserCreate);
                                if (userSendMail != null)
                                {
                                    string html = RenderTeamPletave.Teampleateve(inforbk, listseting, Constant.TemPleateHTMLID.KichHoatVe, userSendMail.Id);
                                    string filename = "";
                                    filename = inforbk.CreatedDate.HasValue ? inforbk.CreatedDate.Value.ToString("dd/MM/yyyy").Replace('/', '_') : "";
                                    filename += "_" + inforbk.BookCode;
                                    filename += "_" + Utility.ConvertToUnsign(inforbk.Name).Replace(' ', '_');
                                    filename += "_" + inforbk.FromCity + inforbk.ToCity;
                                    filename += ".pdf";
                                    var export = new ExportPDF();
                                    string htmlPDF = RenderTeamPletave.TeampleatevePDF(inforbk, listseting, UIUty.GetIdPdfTempleate(inforbk), userSendMail.Id);
                                    var file = export.ExportPdfReturnStream(htmlPDF);

                                    var att = new Attachment(file, filename, "application/pdf");
                                    var fromAddress = UIUty.GetsettingByKey(listseting, "PRT_EMAIL");
                                    var fromPassword = UIUty.GetsettingByKey(listseting, "PRT_EMAIL_Pass");
                                    var port = UIUty.GetsettingByKey(listseting, "EmailSender_SmtpPort");
                                    var Host = UIUty.GetsettingByKey(listseting, "EmailSender_SmtpClient");


                                    MailUtily.sendmail(listseting, tieude, html
                                  , list,
                                  att, fromAddress, fromPassword, Utility.ConvertToInt(port), Host);
                                }
                                else
                                {
                                    WriteLog("CapNhapTrangThai Không tìm thấy templeate", Constant.LogType.Info);
                                }

                                //if (inforbk.UserId == Constant.KL)
                                //    voidsendsms(inforbk, "DA XAC NHAN");
                                Sendyeucaunhanhoadon(inforbk, listseting);

                            }
                        }
                        if (obj.Status == Constant.StatusVe.DaHuyVe)
                        {

                            var dao = new BK_BookingDao();
                            var inforbk = dao.GetInfo(obj.ID);
                            if (inforbk != null)
                            {
                                var userSendMail = GetInforByUserOrUserParent(inforbk.UserCreate);
                                if (userSendMail != null)
                                {
                                    //if (checkgui(inforbk.UserId))
                                    //{
                                    var listseting = GetSettingByUser(inforbk.UserCreate, true);
                                    var tieude = "Hủy | [" + inforbk.BookCode + "] | " +
                                                 inforbk.Name +
                                                 " | " +
                                                 inforbk.FromCity + " " + inforbk.ToCity;
                                    var list = new List<MailAddress> { new MailAddress(inforbk.Email) };
                                    // string urlsource = base.Server.MapPath("~/TeampleateVe/TeampletaveSendmail.html");
                                    var html = RenderTeamPletave.Teampleateve(inforbk, listseting, Constant.TemPleateHTMLID.VeXacNhanHanhTrinh, userSendMail.Id);
                                    string filename = "";
                                    filename = inforbk.CreatedDate.HasValue ? inforbk.CreatedDate.Value.ToString("dd/MM/yyyy").Replace('/', '_') : "";
                                    filename += "_" + inforbk.BookCode;
                                    filename += "_" + Utility.ConvertToUnsign(inforbk.Name).Replace(' ', '_');
                                    filename += "_" + inforbk.FromCity + inforbk.ToCity;
                                    filename += ".pdf";
                                    var export = new ExportPDF();
                                    string htmlPDF = RenderTeamPletave.TeampleatevePDF(inforbk, listseting, UIUty.GetIdPdfTempleate(inforbk), userSendMail.Id);
                                    var file = export.ExportPdfReturnStream(htmlPDF);

                                    var att = new Attachment(file, filename, "application/pdf");
                                    var fromAddress = UIUty.GetsettingByKey(listseting, "PRT_EMAIL");
                                    var fromPassword = UIUty.GetsettingByKey(listseting, "PRT_EMAIL_Pass");
                                    var port = UIUty.GetsettingByKey(listseting, "EmailSender_SmtpPort");
                                    var Host = UIUty.GetsettingByKey(listseting, "EmailSender_SmtpClient");


                                    MailUtily.sendmail(listseting, tieude,html
                                    , list,
                                    att, fromAddress, fromPassword, Utility.ConvertToInt(port), Host);
                                }
                                else
                                {
                                    WriteLog("CapNhapTrangThai Không tìm thấy templeate", Constant.LogType.Info);
                                }

                                //}
                            }
                        }
                    }
                }
            }
            return RedirectToAction("Create", "QuanLyVeDaDat", new { id = param.bookingid });
        }
        public ActionResult HoanVe(int bookingid)
        {
            return View();
        }
        [HttpPost]
        public ActionResult HoanVe(FormCollection input)
        {
            var bookingid = input.GetValue("bookingid");
            var code = input.GetValue("code");
            var nguoicanhoan = input.GetValue("nguoicanhoan");
            var changcanhoan = input.GetValue("changcanhoan");
            var kieuhoan = input.GetValue("kieuhoan");
            var bookingidvl = Utility.ConvertToInt(bookingid.AttemptedValue);
            var bokingdao = new BK_BookingDao();
            try
            {

                var obj = bokingdao.GetbyId(bookingidvl.HasValue ? bookingidvl.Value : 0);
                if (obj != null)
                {
                    bokingdao.UpdateTrangThai(obj.ID, 4, obj.BookCode, obj.UserId, obj.Expireddate, null);
                    var userdao = new AspNetUsersBiz();
                    var objuser = userdao.GetInfoByLoginName(obj.UserCreate);
                    string mailuser = objuser != null ? objuser.Email : "";
                    string nguoiyeucau = objuser != null ? (objuser.DisplayName + " - " + objuser.PhoneNumber) : "";
                    string conten = string.Format(@"<p>Code/TE: {0},</p>
<p></p>

<p>Người cần hoàn: {1}<p>
<p></p>

<p>Chặng cần hoàn:{2}</p>
<p> </p>
<p>Kiểu hoàn: {3}</p>
<p> </p>
<p>Vui lòng báo lại giúp phí hoàn và tình trạng vào mail: {4}</p>
<p> </p>
<p>Người yêu cầu: {5}</p>
", code != null ? code.AttemptedValue : "",
     nguoicanhoan != null ? nguoicanhoan.AttemptedValue : "",
      changcanhoan != null ? changcanhoan.AttemptedValue : "",
     kieuhoan != null ? kieuhoan.AttemptedValue : "",
     mailuser,
     nguoiyeucau);
                    var setting = GetSettingByUser(obj.UserCreate, true); ;
                    var list = new List<MailAddress>();
                    list.Add(new MailAddress(UIUty.GetsettingByKey(setting, "Email_HoanVe")));
                    var fromAddress = UIUty.GetsettingByKey(setting, "PRT_EMAIL");
                    var fromPassword = UIUty.GetsettingByKey(setting, "PRT_EMAIL_Pass");
                    var port = UIUty.GetsettingByKey(setting, "EmailSender_SmtpPort");
                    var Host = UIUty.GetsettingByKey(setting, "EmailSender_SmtpClient");
                    MailUtily.sendmail(setting,
                        "Yêu cầu hoàn vé | " + (code != null ? code.AttemptedValue : "") + " | " +
                        (nguoicanhoan != null ? nguoicanhoan.AttemptedValue : "") + " | " +
                        (changcanhoan != null ? changcanhoan.AttemptedValue : ""), conten, list, fromAddress, fromPassword, Utility.ConvertToInt(port), Host);
                    try
                    {
                        string noidungmoi = "";
                        noidungmoi += "Code: " + code != null ? code.AttemptedValue : "";
                        noidungmoi += "Người cần hoàn: " + nguoicanhoan != null ? nguoicanhoan.AttemptedValue : "";
                        noidungmoi += "Chặng cần hoàn: " + changcanhoan != null ? changcanhoan.AttemptedValue : "";
                        noidungmoi += "Kiểu hoàn: " + kieuhoan != null ? kieuhoan.AttemptedValue : "";
                        noidungmoi += "Người yêu cầu: " + nguoiyeucau;
                        LogMain.LogBooking(obj.ID, CurrentUser.UserName, "", noidungmoi, Constant.LogBookingType.Hoanve);
                    }
                    catch
                    {

                    }
                }
            }
            catch (Exception)
            {

            }
            return RedirectToAction("Create", "QuanLyVeDaDat", new { id = bookingidvl.HasValue ? bookingidvl.Value : 0 });

        }
        public ActionResult UpdateGia(int? veid, double? sotien)
        {
            var ticketdao = new BK_TicketDao();
            var objold = ticketdao.GetbyId(veid ?? 0);
            ticketdao.UpdateGia(veid.HasValue ? veid.Value : 0, sotien.HasValue ? sotien.Value : 0);
            try
            {

                if (objold != null)
                {
                    var objnew = ticketdao.GetbyId(veid ?? 0);
                    if (objnew != null)
                    {
                        string noidungcu = "";
                        noidungcu += "Số hiệu: " + objold.FlightNo;
                        noidungcu += "</br>Lượt: " + Utility.GetDictionaryValue(Constant.Typeve.dctName, objold.TypeID);
                        noidungcu += "</br>Giá vé: " + objold.Price;
                        string noidungmoi = "";
                        noidungmoi += "Số hiệu: " + objnew.FlightNo;
                        noidungmoi += "</br>Lượt: " + Utility.GetDictionaryValue(Constant.Typeve.dctName, objnew.TypeID);
                        noidungmoi += "</br>Giá vé: " + objnew.Price;
                        LogMain.LogBooking(objold.BookingID, CurrentUser.UserName, noidungcu, noidungmoi, Constant.LogBookingType.capnhapgia);
                    }
                }

            }
            catch
            {

            }

            return Json("");
        }
        [HttpPost]
        public ActionResult Capnhapgiobay(FormCollection input)
        {

            var bookingid = input.GetValue("bookingid");
            var ticketid = input.GetValue("ticketid");
            var ngaybatdau = input.GetValue("ngaybatdau" + ticketid.AttemptedValue);
            var giobatdau = input.GetValue("giobatdau" + ticketid.AttemptedValue);
            var ngaykethuc = input.GetValue("ngaykethuc" + ticketid.AttemptedValue);
            var giokethuc = input.GetValue("giokethuc" + ticketid.AttemptedValue);
            var FromCity = input.GetValue("CapnhapFromCity" + ticketid.AttemptedValue);
            var ToCity = input.GetValue("CapnhapToCity" + ticketid.AttemptedValue);
            var airlineCode = input.GetValue("CapnhapairlineCode" + ticketid.AttemptedValue);
            var flight = input.GetValue("Capnhapflight" + ticketid.AttemptedValue);
            var Class = input.GetValue("CapnhapClass" + ticketid.AttemptedValue);
            var bookingidvl = Utility.ConvertToInt(bookingid.AttemptedValue);
            if (bookingid != null && ticketid != null && ngaybatdau != null && giobatdau != null && ngaykethuc != null &&
                giokethuc != null && FromCity != null && ToCity != null && airlineCode != null & flight != null && Class != null)
            {
                try
                {
                    var ticketidvl = Utility.ConvertToInt(ticketid.AttemptedValue);
                    var dao = new BK_TicketDao();
                    var objold = dao.GetTicketDetailbyId(ticketidvl ?? 0);

                    var ngaybatdaulv = ngaybatdau.AttemptedValue.Split('/');
                    var giobatdauvl = giobatdau.AttemptedValue.Split(':');
                    var ngaykethucvl = ngaykethuc.AttemptedValue.Split('/');
                    var giokethucvl = giokethuc.AttemptedValue.Split(':');
                    string giatringaybatdau = "";
                    string giatrigiobatdau = "";
                    string giatringaykethuc = "";
                    string giatrigiokethuc = "";
                    if (ngaybatdaulv.Length >= 3)
                    {
                        giatringaybatdau = (ngaybatdaulv[0].Length == 1 ? ("0" + ngaybatdaulv[0]) : ngaybatdaulv[0]) + "/" + (ngaybatdaulv[1].Length == 1 ? ("0" + ngaybatdaulv[1]) : ngaybatdaulv[1]) + "/" + ngaybatdaulv[2];
                    }
                    if (ngaykethucvl.Length >= 3)
                    {
                        giatringaykethuc = (ngaykethucvl[0].Length == 1 ? ("0" + ngaykethucvl[0]) : ngaykethucvl[0]) + "/" + (ngaykethucvl[1].Length == 1 ? ("0" + ngaykethucvl[1]) : ngaykethucvl[1]) + "/" + ngaykethucvl[2];
                    }
                    if (giobatdauvl.Length >= 2)
                    {
                        giatrigiobatdau = (giobatdauvl[0].Length == 1 ? ("0" + giobatdauvl[0]) : giobatdauvl[0]) + ":" + (giobatdauvl[1].Length == 1 ? ("0" + giobatdauvl[1]) : giobatdauvl[1]);
                    }
                    if (giokethucvl.Length >= 2)
                    {
                        giatrigiokethuc = (giokethucvl[0].Length == 1 ? ("0" + giokethucvl[0]) : giokethucvl[0]) + ":" + (giokethucvl[1].Length == 1 ? ("0" + giokethucvl[1]) : giokethucvl[1]);
                    }
                    var ngaybatdaucv = Utility.ConvertToDatetime(giatringaybatdau + " " + giatrigiobatdau);
                    var ngayktcv = Utility.ConvertToDatetime(giatringaykethuc + " " + giatrigiokethuc);
                    var ck = new BK_Ticket
                    {
                        ID = ticketidvl.HasValue ? ticketidvl.Value : 0,
                        StartDate = ngaybatdaucv,
                        EndDate = ngayktcv,
                        FromCity = FromCity.AttemptedValue,
                        ToCity = ToCity.AttemptedValue,
                        Code = airlineCode.AttemptedValue,
                        FlightNo = flight.AttemptedValue,
                        Class = Class.AttemptedValue,
                    };
                    dao.Updatengaygio(ck);
                    try
                    {

                        if (objold != null)
                        {
                            var objnew = dao.GetTicketDetailbyId(ticketidvl ?? 0);
                            if (objnew != null)
                            {
                                string noidungcu = "";
                                noidungcu += "Số hiệu: " + objold.flight;
                                noidungcu += "</br>Giờ khởi hành: " + Utility.GetDateMinuteString(objold.StartDate);
                                noidungcu += "</br>Giờ giờ kết thúc: " + Utility.GetDateMinuteString(objold.EndDate);
                                noidungcu += "</br>Từ: " + objold.FromCity;
                                noidungcu += "</br>Đến: " + objold.ToCity;
                                noidungcu += "</br>Hãng bay: " + objold.airlineCode;
                                noidungcu += "</br>Hạng vé: " + objold.Class;
                                string noidungmoi = "";
                                noidungmoi += "Số hiệu: " + objnew.flight;
                                noidungmoi += "</br>Giờ khởi hành: " + Utility.GetDateMinuteString(objnew.StartDate);
                                noidungmoi += "</br>Giờ giờ kết thúc: " + Utility.GetDateMinuteString(objnew.EndDate);
                                noidungmoi += "</br>Từ: " + objnew.FromCity;
                                noidungmoi += "</br>Đến: " + objnew.ToCity;
                                noidungmoi += "</br>Hãng bay: " + objnew.airlineCode;
                                noidungmoi += "</br>Hạng vé: " + objnew.Class;
                                LogMain.LogBooking(bookingidvl, CurrentUser.UserName, noidungcu, noidungmoi, Constant.LogBookingType.thaydoingaygiobay);
                            }
                        }
                    }
                    catch { }
                }
                catch { }
            }
            return RedirectToAction("Create", "QuanLyVeDaDat", new { id = bookingidvl });
        }
        public ActionResult deletevechitiet(int bookingid, int ticketid)
        {
            var dao = new BK_TicketDao();
            try
            {
                var objnew = dao.GetTicketDetailbyId(ticketid);
                if (objnew != null)
                {
                    string noidungmoi = "";
                    noidungmoi += "Số hiệu: " + objnew.flight;
                    LogMain.LogBooking(bookingid, CurrentUser.UserName, "", noidungmoi, Constant.LogBookingType.xoave);
                }
            }
            catch { }
            dao.xoa(bookingid, ticketid);
            return RedirectToAction("Create", "QuanLyVeDaDat", new { id = bookingid });
        }
        public ActionResult DoiTenHK()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DoiTenHK(FormCollection input)
        {
            var bookingid = input.GetValue("bookingid");
            var ticketid = input.GetValue("ticketid");
            var Ho = input.GetValue("Ho");
            var Ten = input.GetValue("Ten");
            var Birthday = input.GetValue("hkBirthday");
            var hkBaggageDepartValue = input.GetValue("hkBaggageDepartValue");
            var hkBaggageDepartPrice = input.GetValue("hkBaggageDepartPrice");
            var hkBaggageReturnValue = input.GetValue("hkBaggageReturnValue");
            var hkBaggageReturnPrice = input.GetValue("hkBaggageReturnPrice");
            var hkgioitinh = input.GetValue("hkgioitinh");
            var bookingidvl = Utility.ConvertToInt(bookingid.AttemptedValue);
            var ticketidvl = Utility.ConvertToInt(ticketid.AttemptedValue);
            var birthday = Utility.ConvertToDate(Birthday != null ? Birthday.AttemptedValue : "");
            var bokingdao = new BK_PassengerDao();
            if (ticketidvl.HasValue)
            {
                var objold = bokingdao.GetbyId(ticketidvl ?? 0);
                bokingdao.updateTen(ticketidvl.Value, (Ho != null ? Utility.ConvertToUnsign(Ho.AttemptedValue).ToUpper() : ""),
                    (Ten != null ? Utility.ConvertToUnsign(Ten.AttemptedValue).ToUpper() : ""),
                    hkBaggageDepartValue != null ? hkBaggageDepartValue.AttemptedValue.Replace("Kg", "").Replace("kg", "") : "",
                    Utility.ConvertToDecimal(hkBaggageDepartPrice != null ? hkBaggageDepartPrice.AttemptedValue.Replace(",", "").Replace(".", "").Trim() : ""),
                    hkBaggageReturnValue != null ? hkBaggageReturnValue.AttemptedValue.Replace("Kg", "").Replace("kg", "") : "",
                      Utility.ConvertToDecimal(hkBaggageReturnPrice != null ? hkBaggageReturnPrice.AttemptedValue.Replace(",", "").Replace(".", "").Trim() : ""),
                       hkgioitinh != null ? hkgioitinh.AttemptedValue : "", bookingidvl, birthday
                 );

                try
                {
                    if (objold != null)
                    {
                        var objnew = bokingdao.GetbyId(ticketidvl.Value);
                        if (objnew != null)
                        {
                            string noidungcu = "";
                            noidungcu += "Tên: " + objold.FirstName.ToUpper() + " " + objold.Name.ToUpper();
                            noidungcu += "<br>Hành lý chiều đi: " + objold.BaggageDepartValue + " - " + objold.BaggageDepartPrice;
                            noidungcu += "<br>Hành lý chiều về: " + objold.BaggageReturnValue + " " + objold.BaggageReturnPrice;
                            string noidungmoi = "";
                            noidungmoi += "Tên: " + objnew.FirstName.ToUpper() + " " + objnew.Name.ToUpper();
                            noidungmoi += "<br>Hành lý chiều đi: " + objnew.BaggageDepartValue + " - " + objnew.BaggageDepartPrice;
                            noidungmoi += "<br>Hành lý chiều về: " + objnew.BaggageReturnValue + " " + objnew.BaggageReturnPrice;
                            LogMain.LogBooking(bookingidvl, CurrentUser.UserName, noidungcu, noidungmoi, Constant.LogBookingType.doitenhanhkhach);
                        }
                    }
                }
                catch { }
                return RedirectToAction("Create", "QuanLyVeDaDat", new { id = bookingidvl.HasValue ? bookingidvl.Value : 0 });
            }
            return View();
        }
        public ActionResult XoaHanhKhach(int? IDHanhKhach, int? bookingid)
        {
            var ticketdao = new BK_PassengerDao();
            var bokingdao = new BK_PassengerDao();
            if (IDHanhKhach.HasValue)
            {
                var objold = bokingdao.GetbyId(IDHanhKhach ?? 0);
                try
                {
                    if (objold != null)
                    {
                        string noidungmoi = "";
                        noidungmoi += "Tên: " + objold.FirstName.ToUpper() + " " + objold.Name.ToUpper();
                        LogMain.LogBooking(bookingid, CurrentUser.UserName, "", noidungmoi, Constant.LogBookingType.Xoahanhkhach);
                    }
                }
                catch { }
            }
            ticketdao.DeleteById(IDHanhKhach.HasValue ? IDHanhKhach.Value : 0,
                bookingid.HasValue ? bookingid.Value : 0);
            return Json("");
        }
        public ActionResult AddPass(int? BookingId)
        {
            var model = new CreatePassengerModel();
            model.Idbooking = BookingId;
            var bkdao = new BK_TicketDao();
            var lstticket = bkdao.GetListByBooking(BookingId ?? 0);
            var checkchieuve = lstticket.Count(z => z.TypeID == Constant.Typeve.LuotVe);
            model.IsReturn = false;
            if (checkchieuve > 0)
            {
                model.IsReturn = true;
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult AddPass(CreatePassengerModel model)
        {
            try
            {
                var bkdao = new BK_TicketDao();
                if (string.IsNullOrEmpty(model.BaggageReturnPrice))
                {
                    model.BaggageReturnPrice = "";
                }
                if (string.IsNullOrEmpty(model.BaggageDepartPrice))
                {
                    model.BaggageDepartPrice = "";
                }
                if (string.IsNullOrEmpty(model.TotalPriceDepart))
                {
                    model.TotalPriceDepart = "";
                }
                if (string.IsNullOrEmpty(model.TotalPriceReturn))
                {
                    model.TotalPriceReturn = "";
                }
                var passdao = new BK_PassengerDao();
                var objpass = new BK_Passenger
                {
                    TypeID = model.TypeID,
                    Name = model.Name,
                    Sex = model.Sex,
                    FirstName = model.FirstName,
                    CreateUser = CurrentUser.UserName,
                    BaggageDepartID = model.BaggageDepartID,
                    BaggageDepartValue = model.BaggageDepartValue,
                    BaggageDepartPrice = Utility.ConvertToDecimal(model.BaggageDepartPrice.Replace(",", "").Replace(".", "")),
                    BaggageReturnID = model.BaggageReturnID,
                    BaggageReturnValue = model.BaggageReturnValue,
                    BaggageReturnPrice = Utility.ConvertToDecimal(model.BaggageReturnPrice.Replace(",", "").Replace(".", "")),

                };
                if (model.TypeID == Constant.TypePassenger.NguoiLon)
                {
                    objpass.Birthday = Utility.ConvertToDate(model.Birthday);
                }
                passdao.InsertNewPassengerBackEnd(objpass, model.Idbooking ?? 0,
                     (Utility.ConvertTodouble(model.TotalPriceDepart.Replace(",", "").Replace(".", "")) ?? 0),
                    (Utility.ConvertTodouble(model.TotalPriceReturn.Replace(",", "").Replace(".", "")) ?? 0)
                   );
                string noidungmoi = "";
                noidungmoi += "Tên: " + model.FirstName.ToUpper() + " " + model.Name.ToUpper();
                noidungmoi += "<br>Hành lý chiều đi: " + model.BaggageDepartValue + " - " + model.BaggageDepartPrice;
                noidungmoi += "<br>Hành lý chiều về: " + model.BaggageReturnValue + " " + model.BaggageReturnPrice;
                LogMain.LogBooking(model.Idbooking, CurrentUser.UserName, "", noidungmoi, Constant.LogBookingType.ThemHanhKhach);
                return Json(new { isSuccess = true, mess = Constant.Addnewsuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { isSuccess = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region Danh mục
        private void Sendyeucaunhanhoadon(BK_BookingInfo inforbk, List<SettingUserInfo> listsetting)
        {
            try
            {
                if (inforbk != null)
                {
                    if (!string.IsNullOrEmpty(inforbk.xhdtencongty) || !string.IsNullOrEmpty(inforbk.xhdnguoinhanhd) || !string.IsNullOrEmpty(inforbk.xhddiachi))
                    {
                        string body = @"<table style='width: 100%;font-size: 18px;' border='0' cellpadding='3' cellspacing='3'>
                                                <tbody>";
                        body += @"<tr> <td><span id='lblName'>Người đặt:<b> " + inforbk.Name + " - " + inforbk.Email + " - " + inforbk.Phone + " </b></span></td></tr>";
                        body += @"<tr> <td><span id='lblName'>Tổng giá:<b> " + (inforbk.TotalPrice.HasValue ? inforbk.TotalPrice.Value.ToString("n0") : "") + " </b></span></td></tr>";
                        body += @"<tr> <td><span id='lblName'><b> Nơi nhận hóa đơn: </b></span>  </td> </tr>";
                        body += @"<tr> <td><span id='lblName'>Tên công ty:<b> " + inforbk.xhdtencongty + " </b></span></td></tr>";
                        body += @"<tr><td><span id='lblEmail'>Địa chỉ:<b>" + inforbk.xhddiachi + "</b></span></td></tr>";
                        body += @"<tr><td><span id='lblEmail'>Thành phố:<b>" + inforbk.xhdthanhpho + "</b></span></td></tr>";
                        body += @"<tr><td><span id='lblEmail'>Người nhận:<b>" + inforbk.xhdnguoinhanhd + "</b></span></td></tr>";
                        body += @"<tr><td><span id='lblEmail'>Mã số thuế VAT:<b>" + inforbk.xhdvat + "</b></span></td></tr>";
                        if (!string.IsNullOrEmpty(inforbk.xhdktencongty))
                        {
                            body += @"<tr><td><span id='lblEmail'><b> Nơi nhận hóa đơn khác </b></span></td></tr>";
                            body += @"<tr><td><span id='lblEmail'>Tên công ty:<b>" + inforbk.xhdktencongty + "</b></span></td></tr>";
                            body += @"<tr><td><span id='lblEmail'>Địa chỉ:<b>" + inforbk.xhdkdiachi + "</b></span></td></tr>";
                            body += @"<tr><td><span id='lblEmail'>Thành phố:<b>" + inforbk.xhdktp + "</b></span></td></tr>";
                            body += @"<tr><td><span id='lblEmail'>Người nhận:<b>" + inforbk.xhdknguoinhanhd + "</b></span></td></tr>";
                        }
                        body += @"  </tbody> </table>";
                        var tieude = "Yêu cầu xuất hóa đơn | mã xác nhận [" + inforbk.BookCode + "] | " +
                                                    inforbk.Name +
                                                    " | " + inforbk.FromCity + " " + inforbk.ToCity;
                        var mailsend = UIUty.GetsettingByKey(listsetting, "MailYeuCauHoaDon");
                        if (!string.IsNullOrEmpty(mailsend))
                        {
                            var list = new List<MailAddress> { new MailAddress(mailsend) };
                            var fromAddress = UIUty.GetsettingByKey(listsetting, "PRT_EMAIL");
                            var fromPassword = UIUty.GetsettingByKey(listsetting, "PRT_EMAIL_Pass");
                            var port = UIUty.GetsettingByKey(listsetting, "EmailSender_SmtpPort");
                            var Host = UIUty.GetsettingByKey(listsetting, "EmailSender_SmtpClient");
                            MailUtily.sendmail(listsetting,
                               tieude, body, list, fromAddress, fromPassword, Utility.ConvertToInt(port), Host);

                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
        private List<AspNetUserInfo> GetlistUser()
        {
            AspNetUsersBiz aspnetUsersBiz = new AspNetUsersBiz();
            var param = new VDMMutiline.SharedComponent.Params.AspNetUsersParam()
            {
                AspNetUsersFilter = new VDMMutiline.SharedComponent.Params.AspNetUsersFilter() { },
            };
            if (IsAdmin())
                param.AspNetUsersFilter.UserList = new List<string>();
            else param.AspNetUsersFilter.UserList = GetparentUserDaiLy().Select(z => z.UserName).ToList();
            aspnetUsersBiz.Search(param);
            return param.AspNetUsersInfos;
        }
        private List<AirlineInfo> GetAirline()
        {
            var filter = new AirlineFilter()
            {
                Search = string.Empty
            };
            var param = new AirlineParam { AirlineFilter = filter };
            var biz = new AirlineBiz();
            biz.Search(param);
            param.AirlineInfos.Insert(0, new AirlineInfo() { Code = "", Name = "Tất cả" });
            return param.AirlineInfos;
        }
        #endregion



    }
}