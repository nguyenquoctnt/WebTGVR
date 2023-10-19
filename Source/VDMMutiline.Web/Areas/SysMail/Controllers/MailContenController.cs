using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VDMMutiline.Dao;
using VDMMutiline.SharedComponent;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.Models;
using VDMMutiline.SharedComponent.Params;
using VDMMutiline.Core.UI;
using VDMMutiline.SharedComponent.Constants;
using System.Net.Mail;
using VDMMutiline.Core;
using VDMMutiline.Ultilities;

namespace VDMMutiline.WebBackEnd.Areas.SysMail.Controllers
{
    public class MailContenController : BaseController
    {
        MailContendDao _biz = new MailContendDao();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AjaxLoadList(GridFilterSetting<tbl_MailContend> gridFilterSetting, string keyseach, int? stinhid)
        {
            var pagininfo = new PagingInfo { RowStart = gridFilterSetting.iDisplayStart, PageSize = gridFilterSetting.iDisplayLength };
            var param = new MailContenParam() { PagingInfo = pagininfo, userNameList = GetparentUserDaiLy().Select(z => z.UserName).ToList() };
            var MailContenFilter = new MailContenFilter() { Search = keyseach };
            param.MailContenFilter = MailContenFilter;
            _biz.Search(param);
            long count = pagininfo.RecordCount;
            return Json(new { aaData = param.MailContendInfos, recordsTotal = count, recordsFiltered = count, amount = 0x2710 }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Create(string id)
        {
            var model = new MailContenParam();
            var _id = 0;
            if (Int32.TryParse(id, out _id))
            {
                model.Id = _id;
                model.MailContend = _biz.GetbyId(_id);
            }
            else
            {
                var ck = new tbl_MailContend { ID = 0 };
                model.MailContend = ck;
            }
            return View(model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(MailContenParam modelInput, FormCollection forminput)
        {
            try
            {
                if (modelInput != null)
                {
                    modelInput.UserName = CurrentUser.UserName;
                    if (modelInput.MailContend.ID > 0)
                    {
                        _biz.Update(modelInput.MailContend);
                        WriteLog("Cập nhật thông tin MailContend", Constant.LogType.Success);
                        return Json(new { isSuccess = true, mess = Constant.Updatesuccess }, JsonRequestBehavior.AllowGet);
                    }
                    modelInput.MailContend.NgayTao = DateTime.Now;
                    modelInput.MailContend.NguoiTao = CurrentUser.UserName;
                    _biz.Insert(modelInput.MailContend);
                    WriteLog("ADD thông tin MailContend", Constant.LogType.Success);
                    //if (forminput.GetValue("Savesend") != null)
                    //{
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
                            MailUtily.sendmail(listseting, modelInput.MailContend.TieuDe, modelInput.MailContend.Contend, list, fromAddress, fromPassword, Utility.ConvertToInt(port), Host);

                        }
                    //}
                    return Json(new { isSuccess = true, mess = Constant.Addnewsuccess }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { isSuccess = false, mess = Constant.AcactFail }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                WriteLog("Cập nhật thông tin MailContend", Constant.LogType.Success);
                return Json(new { isSuccess = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Delete(int? id)
        {
            tbl_MailContend item = _biz.GetbyId(id ?? 0);
            if (item == null)
            {
                ViewBag.Erro = Constant.NotItem;
                return View(new tbl_MailContend());
            }
            else
                return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(tbl_MailContend obj)
        {
            try
            {
                var biz = new MailContendDao();
                biz.Delete(obj.ID);
                WriteLog("Delete thông tin tbl_MailContend", Constant.LogType.Success);
                return Json(new { isSuccess = true, mess = Constant.Deletesuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                WriteLog("Delete thông tin tbl_MailContend", Constant.LogType.Success);
                return Json(new { isSuccess = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}