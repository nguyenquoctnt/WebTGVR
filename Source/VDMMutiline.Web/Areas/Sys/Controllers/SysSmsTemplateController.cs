using System;
using System.Collections.Generic;
using System.Web.Mvc;
using VDMMutiline.Biz;
using VDMMutiline.Core.UI;
using VDMMutiline.SharedComponent;
using VDMMutiline.SharedComponent.Constants;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
using VDMMutiline.SharedComponent.Models;
using VDMMutiline.SharedComponent.Params;

namespace VDMMutiline.WebBackEnd.Areas.Sys.Controllers
{
   
    public class SysSmsTemplateController : BaseController
    {
        SysSmsTemplateBiz _biz = new SysSmsTemplateBiz();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AjaxLoadList(GridFilterSetting<SysSmsTemplateInfo> gridFilterSetting, string keyseach)
        {
            var pagininfo = new PagingInfo { RowStart = gridFilterSetting.iDisplayStart, PageSize = gridFilterSetting.iDisplayLength };
            var param = new SysSmsTemplateParam() { PagingInfo = pagininfo };
            var SysSmsTemplateFilter = new SysSmsTemplateFilter() { };
            param.SysSmsTemplateFilter = SysSmsTemplateFilter;
            _biz.Search(param);
            long count = pagininfo.RecordCount;
            return Json(new { aaData = param.SysSmsTemplateInfos, recordsTotal = count, recordsFiltered = count, amount = 0x2710 }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Create(string id)
        {
            var model = new SysSmsTemplateParam();
            var _id = 0;
            if (Int32.TryParse(id, out _id))
            {
                model.Id = _id;
                _biz.SetupEditForm(model);
            }
            else
            {
                var ck = new SysSmsTemplate();
                model.SysSmsTemplate = ck;
            }
            return View(model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(SysSmsTemplateParam modelInput)
        {
            try
            {
                if (modelInput != null)
                {
                  
                    modelInput.UserName =  CurrentUser.UserName;
                    if (modelInput.SysSmsTemplate.Id > 0)
                    {
                       
                        _biz.Update(modelInput);
                        WriteLog("Cập nhật thông tin SysSmsTemplate", Constant.LogType.Success);
                        return Json(new { isSuccess = true, mess=Constant.Updatesuccess }, JsonRequestBehavior.AllowGet);
                    }
                  
                    _biz.Insert(modelInput);
                    WriteLog("Insert thông tin SysSmsTemplate", Constant.LogType.Success);
                    return Json(new { isSuccess = true, mess = Constant.Addnewsuccess }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { isSuccess = false, mess = Constant.AcactFail }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                WriteLog("Cập nhật thông tin SysSmsTemplate", Constant.LogType.Success);
                return Json(new { isSuccess = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            
        }
        public ActionResult Delete(int? id)
        {
            var param = new SysSmsTemplateParam { Id = id??0 };
            _biz.SetupEditForm(param);
            SysSmsTemplate item = param.SysSmsTemplate;
            if (item == null)
            {
                ViewBag.Erro = Constant.NotItem;
                return View(new SysSmsTemplate());
            }
            else
                return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(SysSmsTemplate obj)
        {
            try
            {
                var list = new List<SysSmsTemplate> { obj };
                var paramDelete = new SysSmsTemplateParam { SysSmsTemplates = list };
                _biz.Delete(paramDelete);
                WriteLog("Delete thông tin SysSmsTemplate", Constant.LogType.Success);
                return Json(new { isSuccess = true , mess = Constant.Deletesuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                WriteLog("Delete thông tin SysSmsTemplate", Constant.LogType.Success);
                return Json(new { isSuccess = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}