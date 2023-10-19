using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VDMMutiline.SharedComponent.Constants;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SendMailAndSMS.TeampleateVe;
using VDMMutiline.Biz;
using VDMMutiline.SharedComponent;
using VDMMutiline.SharedComponent.EntityInfo;
using VDMMutiline.SharedComponent.Params;
using VDMMutiline.Core.UI;
using System.Net.Mail;
using VDMMutiline.Core;
using VDMMutiline.Ultilities;
using VDMMutiline.Dao;
namespace VDMMutiline.SeachAndBook.Controllers
{
    public class KiemTraVeController : PublishController
    {
        public ActionResult Index(string code)
        {
            BK_BookingParam param = new BK_BookingParam();
            param.Code = code;
            var dao = new BK_BookingDao();
            if (string.IsNullOrEmpty(code))
                code = "";
            else
            {
                param.BookingInfo = dao.GetInfoALL(param.Code);
            }
            return View(param);
        }
        [HttpPost]
        public ActionResult Index(BK_BookingParam param, FormCollection forminput)
        {
            if(param==null )
            {
                param = new BK_BookingParam { };
                var check_code_ticket = forminput.GetValue("check_code_ticket");
                if(check_code_ticket!=null)
                {
                    param.Code = check_code_ticket.AttemptedValue;
                }
            }
            else if(string.IsNullOrEmpty(param.Code))
            {
                param = new BK_BookingParam { };
                var check_code_ticket = forminput.GetValue("check_code_ticket");
                if (check_code_ticket != null)
                {
                    param.Code = check_code_ticket.AttemptedValue;
                }
            }
            var dao = new BK_BookingDao();
            param.BookingInfo = dao.GetInfoALL(param.Code);
            ViewBag.Erro = "";
            if (param.BookingInfo == null)
            {
                ViewBag.Erro = "Không tìm thấy kết quả.";
            }
            return View(param);
        }
        public ActionResult GuiLaiHanhTrinh(string code)
        {
            BK_BookingParam param = new BK_BookingParam();
            var dao = new BK_BookingDao();
            param.Code = code;
            param.BookingInfo = dao.GetInfoALL(param.Code);
            var inforbk = param.BookingInfo;
            string mail = "";
            if (inforbk != null)
            {
                //if (param.BookingInfo.UserId == Constant.KL)
                //{
                //    mail = param.BookingInfo.Email;
                //}
                //else
                //{
                //    var listseting = GetSettingByUser(inforbk.UserCreate, true);
                //    var EMAIL = UIUty.GetsettingByKey(listseting, "PRT_EMAIL");
                //    mail = (!string.IsNullOrWhiteSpace(EMAIL) ? EMAIL : param.BookingInfo.Email);
                //}
                var obj = param.BookingInfo;
                if (obj.Status == Constant.StatusVe.DaXuatVe)
                {
                    var listseting = GetSettingByUser(inforbk.UserCreate, true);
                    var tieude = "Đã kích hoạt vé | mã xác nhận [" + inforbk.BookCode + "] | " +
                                 inforbk.Name +
                                 " | " + inforbk.FromCity + " " + inforbk.ToCity;
                    var list = new List<MailAddress> { new MailAddress(inforbk.Email) };
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
                }
                if (obj.Status == Constant.StatusVe.DaHuyVe)
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
                        MailUtily.sendmail(listseting, tieude, html
                        , list,
                        att, fromAddress, fromPassword, Utility.ConvertToInt(port), Host);
                    }
                }
                else
                {
                    var userSendMail = GetInforByUserOrUserParent(inforbk.UserCreate);
                    if (userSendMail != null)
                    {

                        var listseting = GetSettingByUser(inforbk.UserCreate, true);
                        var tieude = "Xác nhận đặt vé | mã đặt chỗ [" + inforbk.BookCode + "] | " +
                                     inforbk.Name +
                                     " | " +
                                     inforbk.FromCity + " " + inforbk.ToCity;
                        var list = new List<MailAddress> { new MailAddress(inforbk.Email) };
                        var html = RenderTeamPletave.Teampleateve(inforbk, listseting, Constant.TemPleateHTMLID.KichHoatVe, userSendMail.Id);
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
                }
            }
            return RedirectToAction("Index", new { code = code });
        }
    }
}