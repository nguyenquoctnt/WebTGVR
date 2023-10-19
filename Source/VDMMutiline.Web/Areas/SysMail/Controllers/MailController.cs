using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using VDMMutiline.Core;
using VDMMutiline.Core.UI;
using VDMMutiline.Dao;
using VDMMutiline.SharedComponent;
using VDMMutiline.SharedComponent.Constants;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
using VDMMutiline.SharedComponent.Models;
using VDMMutiline.SharedComponent.Params;
using VDMMutiline.Ultilities;

namespace VDMMutiline.WebBackEnd.Areas.SysMail.Controllers
{
    public class MailController : PublishController
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AjaxLoadList(GridFilterSetting<MailInfo> gridFilterSetting, string keyseach, int? stinhid)
        {
            var dao = new MailDao();
            var pagininfo = new PagingInfo { RowStart = gridFilterSetting.iDisplayStart, PageSize = gridFilterSetting.iDisplayLength };
            var param = new MailParam() { PagingInfo = pagininfo,listuserinsite= GetparentUserDaiLy().Select(z => z.UserName).ToList() };
            var MailFilter = new MailFilter() {  Search = keyseach };
            param.MailFilter = MailFilter;
            dao.Search(param);
            long count = pagininfo.RecordCount;
            return Json(new { aaData = param.MailInfos, recordsTotal = count, recordsFiltered = count, amount = 0x2710 }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SendMail()
        {
            return View();
        }
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult SendMail(string txtSubject, string txtBody)
        {
            try
            {
                var dao = new MailDao();
                var param = new MailParam();
                param.listuserinsite = GetparentUserDaiLy().Select(z => z.UserName).ToList();
                var MailFilter = new MailFilter() { Search = "" };
                param.MailFilter = MailFilter;
                dao.Search(param);
                var list = new List<MailAddress>();
                foreach (var VARIABLE in param.MailInfos)
                {
                    list.Add(new MailAddress(VARIABLE.Mail));
                }
                if (list.Count > 0)
                {
                    var listseting = GetSettingByUser(CurrentUser.UserName, true);
                    var fromAddress = UIUty.GetsettingByKey(listseting, "PRT_EMAIL");
                    var fromPassword = UIUty.GetsettingByKey(listseting, "PRT_EMAIL_Pass");
                    var port = UIUty.GetsettingByKey(listseting, "EmailSender_SmtpPort");
                    var Host = UIUty.GetsettingByKey(listseting, "EmailSender_SmtpClient");
                    MailUtily.sendmail(listseting, txtSubject, txtBody, list, fromAddress, fromPassword, Utility.ConvertToInt(port), Host);
                }
                return View();
            }
            catch (Exception ex)
            {
                WriteLog("Send mail MailController", Constant.LogType.Success);
                return View();
            }
        }
        public ActionResult Delete(int? id)
        {
            var param = new MailParam { Id = id ?? 0 };
            var biz = new MailDao();
            var item = biz.GetbyId(id??0);
            if (item == null)
            {
                ViewBag.Erro = Constant.NotItem;
                return View(new tbl_Mail());
            }
            else
                return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(tbl_Mail obj)
        {
            try
            {
                var biz = new MailDao();
                biz.Delete(obj.Id);

                WriteLog("Delete thông tin tbl_Mail", Constant.LogType.Success);
                return Json(new { isSuccess = true, mess = Constant.Deletesuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                WriteLog("Delete thông tin tbl_Mail", Constant.LogType.Success);
                return Json(new { isSuccess = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}