using CaptchaMvc.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using VDMMutiline.Core;
using VDMMutiline.Core.Common;
using VDMMutiline.Core.UI;
using VDMMutiline.Ultilities;

namespace VDMMutiline.Templeate1.MB.Controllers
{
    public class AboutController : PublishController
    {
        // GET: About
        public ActionResult Index()
        {
            ViewBag.menu = "about";
            return View();
        }
        [HttpPost, CaptchaVerify("Sai mã bảo vệ!")]
        [ValidateAntiForgeryToken]
        public ActionResult Index(FormCollection input)
        {
            ViewBag.menu = "about";
            if (ModelState.IsValid)
            {
                var strName = input.GetValue("txtName");
                var strFromEmail = input.GetValue("txtEmail");
                var strMess = input.GetValue("txtMess");
                if (strName != null && strFromEmail != null && strMess != null)
                {
                    string value = strName.AttemptedValue + "<br/> " + strFromEmail.AttemptedValue + "<br/> " + strMess.AttemptedValue;
                    sendmail(value);

                }
            }
            else
            {
                var errors = ModelState.Select(x => x.Value.Errors)
                    .Where(y => y.Count > 0)
                    .ToList();
                string er = "";
                foreach (var item in errors)
                {
                    er += item[0].ErrorMessage + "<br>";
                }
                ViewBag.Erro = er;
            }
            return View("Index");
        }
        public void sendmail(string code)
        {
            try
            {
                var curentDomain = SiteMuti.Getsitename();
                var settingsite = GetSettingByDomain(curentDomain, true);
                var tieude = "Liên Hệ Mới";
                try
                {
                    var fromAddress = UIUty.GetsettingByKey(settingsite, "PRT_EMAIL");
                    var fromPassword = UIUty.GetsettingByKey(settingsite, "PRT_EMAIL_Pass");
                    var port = UIUty.GetsettingByKey(settingsite, "EmailSender_SmtpPort");
                    var Host = UIUty.GetsettingByKey(settingsite, "EmailSender_SmtpClient");
                    var list = new List<MailAddress>();
                    list.Add(new MailAddress(fromAddress));
                    MailUtily.sendmail(settingsite, tieude, code, list, fromAddress, fromPassword, Utility.ConvertToInt(port), Host);
                }
                catch (Exception e)
                {

                }
            }
            catch
            {

            }
        }
    }
}