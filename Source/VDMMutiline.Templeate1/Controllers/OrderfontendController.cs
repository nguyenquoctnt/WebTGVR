using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using CaptchaMvc.Attributes;
using VDMMutiline.Core;
using VDMMutiline.Core.Common;
using VDMMutiline.Core.UI;
using VDMMutiline.Dao;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.Ultilities;

namespace VDMMutiline.Templeate1.Controllers
{
    public class OrderfontendController : PublishController
    {
        public ActionResult Index(int? idtt, string titeltt, string type)
        {
            var yeucaugoilai = new BK_Order();
            yeucaugoilai.IDtt = idtt;
            yeucaugoilai.Note = titeltt;
            yeucaugoilai.Dichvu = type;
            return View(yeucaugoilai);
        }

        [HttpPost]
        public ActionResult Index(FormCollection forminput, BK_Order ck)
        {
            try
            {
                var curentDomain = SiteMuti.Getsitename();
                var settingsite = GetSettingByDomain(curentDomain, true);
                var CurentUser = CommonUI.GetInforByDomain(curentDomain);
                var daoyeucaugoilai = new OlderDao();
                ck.CreatedDate = DateTime.Now;
                ck.SourceSite = curentDomain;
                ck.UserSite = CurentUser.UserName;
                daoyeucaugoilai.Insert(ck);
               
                string tieude = "Xác nhận đặt " + ck.Note + "  tại " + curentDomain.ToUpper() + " | | " + ck.Name;
                string noidung = "Xin chào " + ck.Name + " - " + ck.Phone +
                    ". Cám ơn bạn đã sử dụng dịch vụ của " + curentDomain.ToUpper() + " , chúng tôi đã nhận được yêu cầu của bạn và sẽ liên lạc lại ngay.";
                string noidung2 = "<br> Mọi thắc mắc xin liên hệ HotLine :<b> " + CurentUser.PhoneNumber + (!string.IsNullOrEmpty(CurentUser.PhoneNumber2) ? "-" + CurentUser.PhoneNumber2 : "") + "</b>";
                try
                {
                    var list = new List<MailAddress> { new MailAddress(ck.Mail) };
                    var fromAddress = UIUty.GetsettingByKey(settingsite, "PRT_EMAIL");
                    var fromPassword = UIUty.GetsettingByKey(settingsite, "PRT_EMAIL_Pass");
                    var port = UIUty.GetsettingByKey(settingsite, "EmailSender_SmtpPort");
                    var Host = UIUty.GetsettingByKey(settingsite, "EmailSender_SmtpClient");
                    MailUtily.sendmail(settingsite,tieude, noidung + noidung2, list, fromAddress, fromPassword, Utility.ConvertToInt(port), Host);
                }
                catch (Exception e)
                {

                }
                return Json(new { isSuccess = false, mess = "Thông tin đã được gửi đi" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { isSuccess = false, mess = "Thao tác thất bại" }, JsonRequestBehavior.AllowGet);
            }

        }

    }
}