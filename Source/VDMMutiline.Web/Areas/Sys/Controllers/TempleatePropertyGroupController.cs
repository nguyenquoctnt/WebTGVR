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
    public class TempleatePropertyGroupController : BaseController
    {
        TempleatePropertyGroupBiz _biz = new TempleatePropertyGroupBiz();
        public ActionResult Index()
        {
            var param = new TempleatePropertyGroupParam() { };
            var TempleatePropertyGroupFilter = new TempleatePropertyGroupFilter() { };
            param.TempleatePropertyGroupFilter = TempleatePropertyGroupFilter;
            _biz.Prepare(param);
            var reparelist = (param.TempleatePropertyGroupInfos.Select(n => new SelectListItem() { Text = n.Name, Value = n.Id.ToString() })).ToList();
            ViewBag.DataTree = reparelist;
            return View();
        }
        public ActionResult AjaxLoadList(GridFilterSetting<TempleatePropertyGroupInfo> gridFilterSetting, string keyseach, int? parentid)
        {
            var pagininfo = new PagingInfo { RowStart = gridFilterSetting.iDisplayStart, PageSize = gridFilterSetting.iDisplayLength };
            var param = new TempleatePropertyGroupParam() { PagingInfo = pagininfo };
            var TempleatePropertyGroupFilter = new TempleatePropertyGroupFilter() { Search = keyseach, ItemId = parentid };
            param.TempleatePropertyGroupFilter = TempleatePropertyGroupFilter;
            _biz.Search(param);
            long count = pagininfo.RecordCount;
            return Json(new { aaData = param.TempleatePropertyGroupInfos, recordsTotal = count, recordsFiltered = count, amount = 0x2710 }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Create(string id)
        {
            var model = new TempleatePropertyGroupParam();
            var param = new TempleatePropertyGroupParam();
            var TempleatePropertyGroupFilter = new TempleatePropertyGroupFilter();
            param.TempleatePropertyGroupFilter = TempleatePropertyGroupFilter;
            _biz.SearchListParam(param);
            param.TempleatePropertyGroupInfos.Insert(0, new TempleatePropertyGroupInfo() { Id = 0, Name = "Nhóm gốc" });
            model.TempleatePropertyGroupInfos = param.TempleatePropertyGroupInfos;
            var _id = 0;
            if (Int32.TryParse(id, out _id))
            {
                model.Id = _id;
                _biz.SetupEditForm(model);
            }
            else
            {
                var ck = new tbl_TempleatePropertyGroup();
                model.TempleatePropertyGroup = ck;
            }
            return View(model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(TempleatePropertyGroupParam modelInput)
        {
            try
            {
                if (modelInput != null)
                {
                    if (modelInput.TempleatePropertyGroup.ThuoctinhCha == 0)
                        modelInput.TempleatePropertyGroup.ThuoctinhCha = null;
                    modelInput.UserName = CurrentUser.UserName;
                    if (modelInput.TempleatePropertyGroup.Id > 0)
                    {
                        modelInput.TempleatePropertyGroup.UpdateDate = DateTime.Now;
                        modelInput.TempleatePropertyGroup.UpdateUser = CurrentUser.UserName;
                        _biz.Update(modelInput);
                        WriteLog("Cập nhật  thông tin TempleatePropertyGroup", Constant.LogType.Success);
                        return Json(new { isSuccess = true, mess = Constant.Updatesuccess }, JsonRequestBehavior.AllowGet);
                    }
                    modelInput.TempleatePropertyGroup.UpdateDate = DateTime.Now;
                    modelInput.TempleatePropertyGroup.UpdateUser = CurrentUser.UserName;
                    modelInput.TempleatePropertyGroup.CreateDate = DateTime.Now;
                    modelInput.TempleatePropertyGroup.CreateUserd = CurrentUser.UserName;
                    _biz.Insert(modelInput);
                    WriteLog("ADD  thông tin TempleatePropertyGroup", Constant.LogType.Success);
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
            var param = new TempleatePropertyGroupParam { Id = id ?? 0 };
            _biz.SetupEditForm(param);
            tbl_TempleatePropertyGroup item = param.TempleatePropertyGroup;
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
        public ActionResult Delete(tbl_TempleatePropertyGroup obj)
        {
            try
            {
                var list = new List<tbl_TempleatePropertyGroup> { obj };
                var paramDelete = new TempleatePropertyGroupParam { TempleatePropertyGroups = list };
                _biz.Delete(paramDelete);
                WriteLog(" Delete thông tin TempleatePropertyGroup", Constant.LogType.Success);
                return Json(new { isSuccess = true, mess = Constant.Deletesuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                WriteLog(" Delete thông tin TempleatePropertyGroup", Constant.LogType.Success);
                return Json(new { isSuccess = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}