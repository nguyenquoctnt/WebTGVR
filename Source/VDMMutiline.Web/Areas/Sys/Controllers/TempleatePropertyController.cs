using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using VDMMutiline.Biz.TempleteSetting;
using VDMMutiline.Core.UI;
using VDMMutiline.SharedComponent;
using VDMMutiline.SharedComponent.Constants;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo.TempleteSetting;
using VDMMutiline.SharedComponent.Models;
using VDMMutiline.SharedComponent.Params.TempleteSetting;

namespace VDMMutiline.WebBackEnd.Areas.Sys.Controllers
{
    public class TempleatePropertyController : BaseController
    {
        TempleatePropertyBiz _biz = new TempleatePropertyBiz();
        public ActionResult Index()
        {
            var param = new TempleatePropertyGroupParam() { };
            var TempleatePropertyGroupFilter = new TempleatePropertyGroupFilter() { };
            param.TempleatePropertyGroupFilter = TempleatePropertyGroupFilter;
            TempleatePropertyGroupBiz biz = new TempleatePropertyGroupBiz();
            biz.Prepare(param);
            var reparelist = (param.TempleatePropertyGroupInfos.Select(n => new SelectListItem() { Text = n.Name, Value = n.Id.ToString() })).ToList();
            ViewBag.DataTree = reparelist;
            return View();
        }
        public ActionResult AjaxLoadList(GridFilterSetting<TempleatePropertyGroupInfo> gridFilterSetting, string keyseach, int? parentid)
        {
            var pagininfo = new PagingInfo { RowStart = gridFilterSetting.iDisplayStart, PageSize = gridFilterSetting.iDisplayLength };
            var param = new TempleatePropertyParam() { PagingInfo = pagininfo };
            var TempleatePropertyFilter = new TempleatePropertyFilter() { Search = keyseach, ItemId = parentid };
            param.TempleatePropertyFilter = TempleatePropertyFilter;
            _biz.Search(param);
            long count = pagininfo.RecordCount;
            return Json(new { aaData = param.TempleatePropertyInfos, recordsTotal = count, recordsFiltered = count, amount = 0x2710 }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Create(string id)
        {
            var model = new TempleatePropertyParam();
            var TempleatePropertyFilter = new TempleatePropertyFilter();
            var param = new TempleatePropertyGroupParam() { };
            var TempleatePropertyGroupFilter = new TempleatePropertyGroupFilter() { };
            param.TempleatePropertyGroupFilter = TempleatePropertyGroupFilter;
            TempleatePropertyGroupBiz biz = new TempleatePropertyGroupBiz();
            biz.SearchListParam(param);
            model.TempleatePropertyGroupInfos = param.TempleatePropertyGroupInfos;
            var _id = 0;
            if (Int32.TryParse(id, out _id))
            {
                model.Id = _id;
                _biz.SetupEditForm(model);
            }
            else
            {
                var ck = new tbl_TempleateProperty();
                model.TempleateProperty = ck;
            }
            return View(model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(TempleatePropertyParam modelInput)
        {
            try
            {
                if (modelInput != null)
                {
                    modelInput.UserName = CurrentUser.UserName;
                    if (modelInput.TempleateProperty.Id > 0)
                    {
                        modelInput.TempleateProperty.UpdateDate = DateTime.Now;
                        modelInput.TempleateProperty.UpdateUser = CurrentUser.UserName;
                        _biz.Update(modelInput);
                        WriteLog("Cập nhật  thông tin TempleateProperty", Constant.LogType.Success);
                        return Json(new { isSuccess = true, mess = Constant.Updatesuccess }, JsonRequestBehavior.AllowGet);
                    }
                    modelInput.TempleateProperty.UpdateDate = DateTime.Now;
                    modelInput.TempleateProperty.UpdateUser = CurrentUser.UserName;
                    modelInput.TempleateProperty.CreateDate = DateTime.Now;
                    modelInput.TempleateProperty.CreateUserd = CurrentUser.UserName;
                    _biz.Insert(modelInput);
                    WriteLog("ADD  thông tin TempleateProperty", Constant.LogType.Success);
                    return Json(new { isSuccess = true, mess = Constant.Addnewsuccess }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { isSuccess = false, mess = Constant.AcactFail }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                WriteLog("Cập nhật  thông tin TempleatePropertyGroup", Constant.LogType.Success);
                return Json(new { isSuccess = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult Delete(int? id)
        {
            var param = new TempleatePropertyParam { Id = id ?? 0 };
            _biz.SetupEditForm(param);
            tbl_TempleateProperty item = param.TempleateProperty;
            if (item == null)
            {
                ViewBag.Erro = Constant.NotItem;
                return View(new SysMenu());
            }
            else
                return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(tbl_TempleateProperty obj)
        {
            try
            {
                var list = new List<tbl_TempleateProperty> { obj };
                var paramDelete = new TempleatePropertyParam { TempleatePropertys = list };
                _biz.Delete(paramDelete);
                WriteLog(" Delete thông tin TempleateProperty", Constant.LogType.Success);
                return Json(new { isSuccess = true, mess = Constant.Deletesuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                WriteLog(" Delete thông tin TempleateProperty", Constant.LogType.Success);
                return Json(new { isSuccess = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}