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
    public class SysMenuController : BaseController
    {
        SysMenuBiz _biz = new SysMenuBiz();
        public ActionResult Index()
        {
            var param = new SysMenuParam() { };
            var SysMenuFilter = new SysMenuFilter() { };
            param.SysMenuFilter = SysMenuFilter;
            _biz.Prepare(param);
          
            var reparelist = (param.SysMenuInfos.Select(n => new SelectListItem() { Text = n.TenHienthi, Value = n.Id.ToString() })).ToList();
            ViewBag.DataTree = reparelist;
            return View();
        }
        public ActionResult AjaxLoadList(GridFilterSetting<SysMenuInfo> gridFilterSetting, string keyseach, int? parentid)
        {
            var pagininfo = new PagingInfo { RowStart = gridFilterSetting.iDisplayStart, PageSize = gridFilterSetting.iDisplayLength };
            var param = new SysMenuParam() { PagingInfo = pagininfo };
            var SysMenuFilter = new SysMenuFilter() { Search = keyseach, ItemId = parentid };
            param.SysMenuFilter = SysMenuFilter;
            _biz.Search(param);
            long count = pagininfo.RecordCount;
            return Json(new { aaData = param.SysMenuInfos, recordsTotal = count, recordsFiltered = count, amount = 0x2710 }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Create(string id)
        {
            var param = new SysMenuParam();
            var sysMenuFilter = new SysMenuFilter();
            param.SysMenuFilter = sysMenuFilter;
            _biz.SearchListParam(param);
            param.SysMenuInfos.Insert(0, new SysMenuInfo() { Id = 0, TenHienthi = "Chuyên mục gốc" });
            var model = new SysMenuParam();
            var bizgrouproles = new AspNetGroupsBiz();
            var paramgrouproles = new AspNetGroupsParam();
            bizgrouproles.GetAll(paramgrouproles);
            model.AspNetGroupsInfos = paramgrouproles.AspNetGroupsInfos;
            var bizmenugroup = new SysMenuGroupBiz();
            var parammenugroup = new SysMenuGroupParam();
            var _id = 0;
            if (Int32.TryParse(id, out _id))
            {
                model.Id = _id;
                _biz.SetupEditForm(model);
                parammenugroup.MenuId = _id;
            }
            else
            {
                var ck = new SysMenu();
                model.SysMenu = ck;
            }
            bizmenugroup.GetAll(parammenugroup);
            model.SysMenuGroupInfos = parammenugroup.SysMenuGroupInfos;
            model.SysMenuInfos = param.SysMenuInfos;

            var bizsite = new SiteInfoBiz();
            var paramsite = new SiteInfoParam() { SiteInfoFilter = new SiteInfoFilter() };
            bizsite.Search(paramsite);
            model.SiteInfos = paramsite.SiteInfos;
            model.SiteInfos.Insert(0, new SiteInfo { Id = 0, SiteName = "Không thuộc site" });
            return View(model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(SysMenuParam modelInput)
        {
            try
            {
                if (modelInput != null)
                {
                    if (modelInput.SysMenu.ParentId == 0)
                        modelInput.SysMenu.ParentId = null;
                    modelInput.UserName = CurrentUser.UserName;
                    if (modelInput.SysMenu.SiteID == 0)
                        modelInput.SysMenu.SiteID = null;
                    if (modelInput.SysMenu.Id > 0)
                    {
                        modelInput.SysMenu.NgaySua = DateTime.Now;
                        modelInput.SysMenu.NguoiSua = CurrentUser.UserName;
                        _biz.Update(modelInput);
                        WriteLog("Cập nhật  thông tin SysMenu", Constant.LogType.Success);
                        var SysMenuGroupBiz = new SysMenuGroupBiz();
                        var paramdeletebymenuid = new SysMenuGroupParam() { MenuId = modelInput.SysMenu.Id };
                        SysMenuGroupBiz.DeleteByidMenu(paramdeletebymenuid);
                        foreach (var item in Request.Form.AllKeys.Where(p => p.StartsWith("group_")))
                        {
                            if (Request.Form.GetValues(item).Length > 0 && Request.Form.GetValues(item)[0] == "true")
                            {
                                var results = item.Replace("group_", "");
                                int roleid = 0;
                                if (int.TryParse(results, out roleid))
                                {
                                    SysMenuGroup sitem = new SysMenuGroup();
                                    modelInput.SysMenuGroups = new List<SysMenuGroup>();
                                    sitem.MenuId = modelInput.SysMenu.Id;
                                    sitem.GroupId = roleid;
                                    var parammenugroup = new SysMenuGroupParam();
                                    parammenugroup.SysMenuGroup = sitem;
                                    SysMenuGroupBiz.Insert(parammenugroup);
                                }
                            }
                        }
                        return Json(new { isSuccess = true, mess = Constant.Updatesuccess }, JsonRequestBehavior.AllowGet);
                    }
                    modelInput.SysMenu.NgayTao = DateTime.Now;
                    modelInput.SysMenu.Nguoitao = CurrentUser.UserName;
                    modelInput.SysMenu.NgaySua = DateTime.Now;
                    modelInput.SysMenu.NguoiSua = CurrentUser.UserName;
                    _biz.Insert(modelInput);
                    WriteLog("ADD  thông tin SysMenu", Constant.LogType.Success);
                    foreach (var item in Request.Form.AllKeys.Where(p => p.StartsWith("group_")))
                    {
                        if (Request.Form.GetValues(item).Length > 0 && Request.Form.GetValues(item)[0] == "true")
                        {
                            var results = item.Replace("group_", "");
                            int roleid = 0;
                            if (int.TryParse(results, out roleid))
                            {
                                SysMenuGroup sitem = new SysMenuGroup();
                                modelInput.SysMenuGroups = new List<SysMenuGroup>();
                                sitem.MenuId = modelInput.SysMenu.Id;
                                sitem.GroupId = roleid;
                                var biz = new SysMenuGroupBiz();
                                var parammenugroup = new SysMenuGroupParam();
                                parammenugroup.SysMenuGroup = sitem;
                                biz.Insert(parammenugroup);
                            }
                        }
                    }
                    return Json(new { isSuccess = true, mess = Constant.Addnewsuccess }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { isSuccess = false, mess = Constant.AcactFail }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                WriteLog("Cập nhật  thông tin SysMenu", Constant.LogType.Success);
                return Json(new { isSuccess = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult Delete(int? id)
        {
            var param = new SysMenuParam { Id = id ?? 0 };
            _biz.SetupEditForm(param);
            SysMenu item = param.SysMenu;
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
        public ActionResult Delete(SysMenu obj)
        {
            try
            {
                var list = new List<SysMenu> { obj };
                var paramDelete = new SysMenuParam { SysMenus = list };
                _biz.Delete(paramDelete);
                WriteLog(" Delete thông tin SysMenu", Constant.LogType.Success);
                return Json(new { isSuccess = true, mess = Constant.Deletesuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                WriteLog(" Delete thông tin SysMenu", Constant.LogType.Success);
                return Json(new { isSuccess = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}