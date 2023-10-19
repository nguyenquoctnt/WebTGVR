using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VDMMutiline.Dao;
using VDMMutiline.SharedComponent;
using VDMMutiline.SharedComponent.Models;
using VDMMutiline.SharedComponent.Params;
using VDMMutiline.Core.UI;
using System.Net.Mail;
using VDMMutiline.Core;
using VDMMutiline.Ultilities;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.Constants;

namespace VDMMutiline.WebBackEnd.Areas.Sys.Controllers
{
    public class VoucherCodeController : PublishController
    {
        VoucherCodeDao _biz = new VoucherCodeDao();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AjaxLoadList(GridFilterSetting<VoucherCodeInfo> gridFilterSetting, string keyseach, int? trangthai)
        {
            var pagininfo = new PagingInfo { RowStart = gridFilterSetting.iDisplayStart, PageSize = gridFilterSetting.iDisplayLength };
            var param = new VoucherCodeParam() { PagingInfo = pagininfo };
            var VoucherCodeFilter = new VoucherCodeFilter() { ListuserinSite = GetparentUserDaiLy().Select(z => z.UserName).ToList(), StatusInt = trangthai };
            param.VoucherCodeFilter = VoucherCodeFilter;
            _biz.Search(param);
            long count = pagininfo.RecordCount;
            return Json(new { aaData = param.VoucherCodeInfos, recordsTotal = count, recordsFiltered = count, amount = 0x2710 }, JsonRequestBehavior.AllowGet);
        }
        public void sendmail(string mail, string code, DateTime EXPRIED, decimal value)
        {
            try
            {
                var list = new List<MailAddress>();
                list.Add(new MailAddress(mail));
                var tieude = "Voucher Code [" + EXPRIED.ToString("dd/MM/yyyy") + " - " + value.ToString("n0") + "|" + mail;
                if (list.Count > 0)
                {
                    var listseting = GetSettingByUser(CurrentUser.UserName, true);
                    var fromAddress = UIUty.GetsettingByKey(listseting, "PRT_EMAIL");
                    var fromPassword = UIUty.GetsettingByKey(listseting, "PRT_EMAIL_Pass");
                    var port = UIUty.GetsettingByKey(listseting, "EmailSender_SmtpPort");
                    var Host = UIUty.GetsettingByKey(listseting, "EmailSender_SmtpClient");
                    MailUtily.sendmail(listseting, tieude, code, list, fromAddress, fromPassword, Utility.ConvertToInt(port), Host);
                }
            }
            catch
            {
            }
        }
        public ActionResult Manual()
        {
            var param = new VoucherCodeParam { VoucherCode = new tbl_VoucherCode() };
            return View(param);
        }
        [HttpPost]
        public ActionResult Manual(VoucherCodeParam param, FormCollection forminput)
        {
            if (param != null)
            {
                var fromdate = forminput.GetValue("fromdate");
                var fromdatevalue = fromdate != null ? Utility.ConvertToDate(fromdate.AttemptedValue) : null;
                var Todate = forminput.GetValue("Todate");
                var Todatevalue = Todate != null ? Utility.ConvertToDate(Todate.AttemptedValue) : null;
                var mailspl = param.VoucherCode.Email.Split(';');
                var valued = forminput.GetValue("valued");
                var valuedvalue = valued != null ? Utility.ConvertToDecimal(valued.AttemptedValue) : null;
                var dao = new VoucherCodeDao();
                foreach (var mail in mailspl)
                {
                    if (!string.IsNullOrEmpty(mail))
                    {
                        var ck = new tbl_VoucherCode
                        {
                            Status = false,
                            FormDate = fromdatevalue,
                            ToDate = Todatevalue,
                            Email = mail,
                            Valued = valuedvalue,
                            UserCreate = CurrentUser.UserName,
                            CreateDate = DateTime.Now
                        };
                        var code = dao.Insert(ck);
                        sendmail(mail, code, Todatevalue ?? DateTime.Now, valuedvalue ?? 0);
                    }
                }
            }
            WriteLog("Insert thông tin tbl_VoucherCode", Constant.LogType.Success);
            return Json(new { isSuccess = true, mess = Constant.Addnewsuccess }, JsonRequestBehavior.AllowGet);
        }
    }
}