using System;
using System.Collections.Generic;
using System.Linq;
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
    public class GroupsController : BaseController
    {
        AspNetGroupsBiz _biz = new AspNetGroupsBiz();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AjaxLoadList(GridFilterSetting<AspNetGroupsInfo> gridFilterSetting, string keyseach)
        {
            var pagininfo = new PagingInfo { RowStart = gridFilterSetting.iDisplayStart, PageSize = gridFilterSetting.iDisplayLength };
            var param = new AspNetGroupsParam() { PagingInfo = pagininfo };
            var AspNetGroupsFilter = new AspNetGroupsFilter() { };
            param.AspNetGroupsFilter = AspNetGroupsFilter;
            _biz.Search(param);
            long count = pagininfo.RecordCount;
            return Json(new { aaData = param.AspNetGroupsInfos, recordsTotal = count, recordsFiltered = count, amount = 0x2710 }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Create(string id)
        {
            var model = new AspNetGroupsParam();
            var _id = 0;
            if (Int32.TryParse(id, out _id))
            {
                model.Id = _id;
                _biz.SetupEditForm(model);
            }
            else
            {
                var ck = new AspNetGroupsInfo();
                ck.RoleAssigned=new List<AspNetRoleGroup>();
                model.AspNetGroupsInfo = ck;
            }
            AspNetRolesBiz bizrolegroup = new AspNetRolesBiz();
            var bzrgParam = new AspNetRolesParam() { AspNetRolesFilter = new AspNetRolesFilter() };
            bizrolegroup.GetAll(bzrgParam);
            model.RoleList = bzrgParam.AspNetRolesInfos;
            return View(model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(AspNetGroupsParam modelInput)
        {
            try
            {
                if (modelInput != null)
                {
                    modelInput.UserName = User.Identity.Name;
                    if (modelInput.AspNetGroupsInfo.Id > 0)
                    {
                        _biz.Update(modelInput);
                        WriteLog("Cập nhật thông tin AspNetGroups", Constant.LogType.Success);
                        return Json(new { isSuccess = true, mess = Constant.Updatesuccess }, JsonRequestBehavior.AllowGet);
                    }
                    _biz.Insert(modelInput);
                    WriteLog("ADD thông tin AspNetGroups", Constant.LogType.Success);
                    return Json(new { isSuccess = true, mess = Constant.Addnewsuccess }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { isSuccess = false, mess = Constant.AcactFail }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                WriteLog("Cập nhật thông tin AspNetGroups", Constant.LogType.Success);
                return Json(new { isSuccess = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult Delete(int? id)
        {
            var param = new AspNetGroupsParam { Id = id ?? 0 };
            _biz.SetupEditForm(param);
            AspNetGroupsInfo item = param.AspNetGroupsInfo;
            if (item == null)
            {
                ViewBag.Erro = Constant.NotItem;
                return View(new AspNetGroupsInfo());
            }
            else
                return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(AspNetGroupsInfo obj)
        {
            try
            {
                var list = new List<AspNetGroupsInfo> { obj };
                var paramDelete = new AspNetGroupsParam { AspNetGroupsInfos = list };
                _biz.Delete(paramDelete);
                WriteLog("Xóa thông tin AspNetGroups", Constant.LogType.Success);
                return Json(new { isSuccess = true, mess = Constant.Deletesuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                WriteLog("Xóa thông tin AspNetGroups", Constant.LogType.Success);
                return Json(new { isSuccess = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}