using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using VDMMutiline.Biz;
using VDMMutiline.Core;
using VDMMutiline.Core.UI;
using VDMMutiline.SharedComponent;
using VDMMutiline.SharedComponent.Constants;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
using VDMMutiline.SharedComponent.Models;
using VDMMutiline.SharedComponent.Params;
using VDMMutiline.Ultilities;

namespace VDMMutiline.WebBackEnd.Areas.Sys.Controllers
{
    public class SysCategoryController : BaseController
    {
        SysCategoryBiz _biz = new SysCategoryBiz();
        public ActionResult Index(int? TypeID)
        {
            var param = new SysCategoryParam() { };
            var sysCategoryFilter = new SysCategoryFilter() { type = TypeID, ListUserName = GetparentUserDaiLy().Select(z => z.UserName).ToList() };
            param.SysCategoryFilter = sysCategoryFilter;
            _biz.SearchListParam(param);
            var reparelist = (param.SysCategoryInfos.Select(n => new SelectListItem() { Text = n.TenHienthi, Value = n.Id.ToString() })).ToList();
            ViewBag.DataTree = reparelist;
            return View(TypeID);
        }
        public ActionResult AjaxLoadList(GridFilterSetting<SysCategoryInfo> gridFilterSetting, string keyseach, int? Types, int? parentcateFillter)
        {
            var pagininfo = new PagingInfo { RowStart = gridFilterSetting.iDisplayStart, PageSize = gridFilterSetting.iDisplayLength };
            var param = new SysCategoryParam() { PagingInfo = pagininfo };
            var SysCategoryFilter = new SysCategoryFilter()
            {
                type = Types,
                Search = keyseach,
                Id = parentcateFillter,
                ListUserName = GetparentUserDaiLy().Where(z=>z.ParentId.HasValue ==false || z.ParentId==0).Select(z => z.UserName).ToList()
            };
            param.SysCategoryFilter = SysCategoryFilter;
            _biz.Search(param);
            long count = pagininfo.RecordCount;
            return Json(new { aaData = param.SysCategoryInfos, recordsTotal = count, recordsFiltered = count, amount = 0x2710 }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Create(string id, int? typeid)
        {
            var model = new SysCategoryParam();
            var _id = 0;
            if (Int32.TryParse(id, out _id))
            {
                model.Id = _id;
                _biz.SetupEditForm(model);
            }
            else
            {
                var ck = new SysCategory();
                ck.LoaiCate = typeid;
                model.SysCategory = ck;
            }
            var SysCategoryFilter = new SysCategoryFilter() { type = typeid, ListUserName = GetparentUserDaiLy().Select(z => z.UserName).ToList() };
            model.SysCategoryFilter = SysCategoryFilter;
            _biz.SearchListParam(model);
            model.SysCategoryInfos.Insert(0, new SysCategoryInfo() { Id = 0, TenHienthi = "Chuyên mục gốc" });
            if (Int32.TryParse(id, out _id))
                model.SysCategoryInfos = model.SysCategoryInfos.Where(z => z.Id != _id).ToList();
            return View(model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(SysCategoryParam modelInput, FormCollection forminput)
        {
            try
            {
                if (modelInput != null)
                {
                    var iconlink = UploadFile.UpLoadFile(Request.Files["Thumbinput"], "Upload/Categorys/");
                    var Imagelink = UploadFile.UpLoadFile(Request.Files["Imageimput"], "Upload/Categorys/");
                    var alcontrol = forminput.AllKeys.Any(p => p == "tags") ? forminput.GetValue("tags").AttemptedValue : null;
                    if (!string.IsNullOrEmpty(iconlink))
                    {
                        modelInput.SysCategory.Thumb = iconlink;
                    }
                    if (!string.IsNullOrEmpty(Imagelink))
                    {
                        modelInput.SysCategory.Image = Imagelink;
                    }
                    if (modelInput.SysCategory.ParentId == 0)
                        modelInput.SysCategory.ParentId = null;
                    modelInput.UserName = CurrentUser.UserName;

                    if (modelInput.SysCategory.Id > 0)
                    {
                        modelInput.SysCategory.NgaySua = DateTime.Now;
                        modelInput.SysCategory.NguoiSua = CurrentUser.UserName;
                        _biz.Update(modelInput);
                        WriteLog("Update thông tin SysCategory", Constant.LogType.Success);
                        return Json(new { isSuccess = true, mess = Constant.Updatesuccess }, JsonRequestBehavior.AllowGet);
                    }

                    modelInput.SysCategory.NgayTao = DateTime.Now;
                    modelInput.SysCategory.Nguoitao = CurrentUser.UserName;
                    modelInput.SysCategory.NgaySua = DateTime.Now;
                    modelInput.SysCategory.NguoiSua = CurrentUser.UserName;
                    modelInput.SysCategory.SourceSite = CurrentUser.UrlDomain1;
                    _biz.Insert(modelInput);
                    WriteLog("Insert thông tin SysCategory", Constant.LogType.Success);
                    return Json(new { isSuccess = true, mess = Constant.Addnewsuccess }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { isSuccess = false, mess = Constant.AcactFail }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { isSuccess = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult Delete(int? id)
        {
            var param = new SysCategoryParam { Id = id ?? 0 };
            _biz.SetupEditForm(param);
            SysCategory item = param.SysCategory;
            if (item == null)
            {
                ViewBag.Erro = Constant.NotItem;
                return View(new SysCategory());
            }
            else
                return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(SysCategory obj)
        {
            try
            {
                var list = new List<SysCategory> { obj };
                var paramDelete = new SysCategoryParam { SysCategorys = list };
                _biz.Delete(paramDelete);
                WriteLog("Delete thông tin SysCategory", Constant.LogType.Success);
                return Json(new { isSuccess = true, mess = Constant.Deletesuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                WriteLog("Delete thông tin SysCategory", Constant.LogType.Success);
                return Json(new { isSuccess = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult CreateArtical(int? id,int? cateid)
        {
            TblArticleBiz biz = new TblArticleBiz();
            var model = new TblArticleParam();
            model.TblCategoryArticleInfos = new List<TblCategoryArticleInfo>();
            int? typevalue = 0;
            if (id.HasValue && id > 0)
            {
                model.Id = id.Value;
                biz.SetupEditForm(model);
                model.TblCategoryArticleInfos = GetlistTblSanphamCategoryInfo(id.Value);
                typevalue = model.TblArticle.LoaiBaiviet;
            }
            else
            {
                var ck = new TblArticle { LoaiBaiviet = Constant.CateType.NoiDung, Id = 0 };
                model.TblArticle = ck;
                model.TblArticle.ChuyenMuc = cateid;
                typevalue = Constant.CateType.NoiDung;
                
            }
            string value = "";
            // AddTree(ref value, model.TblCategoryArticleInfos, typevalue);
            ViewBag.conten = value;
            var bizcate = new SysCategoryBiz();
            var param = new SysCategoryParam() { };
            var sysCategoryFilter = new SysCategoryFilter() { type = Constant.CateType.NoiDung, ListUserName = GetparentUserDaiLy().Select(z => z.UserName).ToList() };
            param.SysCategoryFilter = sysCategoryFilter;
            bizcate.SearchListParam(param);
            var reparelist = param.SysCategoryInfos;
            model.SysCategoryInfos = reparelist;
            return View(model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult CreateArtical(TblArticleParam modelInput, FormCollection inputcontrol)
        {
            try
            {
                TblArticleBiz biz = new TblArticleBiz();
                if (modelInput != null)
                {
                    var iconlink = UploadFile.UpLoadFile(Request.Files["Thumbinput"], "Upload/Articles/");
                    var Imagelink = UploadFile.UpLoadFile(Request.Files["Imageimput"], "Upload/Articles/");
                    if (!string.IsNullOrEmpty(iconlink))
                    {
                        modelInput.TblArticle.Thumb = iconlink;
                    }
                    if (!string.IsNullOrEmpty(Imagelink))
                    {
                        modelInput.TblArticle.Image = Imagelink;
                    }
                    string alter = Constant.Updatesuccess;
                    if (modelInput.TblArticle.Id <= 0)
                    {
                        modelInput.TblArticle.Nguoitao = CurrentUser.UserName;
                        modelInput.TblArticle.NgayTao = DateTime.Now;
                        alter = Constant.Addnewsuccess;
                        
                    }
                    modelInput.TblArticle.NgaySua = DateTime.Now;
                    modelInput.TblArticle.NguoiSua = CurrentUser.UserName;
                    modelInput.Listcate = Listcategoryinput(inputcontrol);
                    biz.Updata(modelInput);
                    biz.UpdateCateID(modelInput.TblArticle.ChuyenMuc, modelInput.Id);
                    WriteLog("Cập nhật thông tin TblArticle", Constant.LogType.Success);
                    return Json(new { isSuccess = true, mess = alter }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { isSuccess = false, mess = Constant.AcactFail }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                WriteLog("Cập nhật thông tin TblArticle", Constant.LogType.Success);
                return Json(new { isSuccess = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        private List<TblCategoryArticleInfo> GetlistTblSanphamCategoryInfo(int ArticalID)
        {
            var param = new TblCategoryArticleParam();
            var Filter = new TblCategoryArticleFilter() { ArticalId = ArticalID };
            param.TblCategoryArticleFilter = Filter;
            var biz = new TblCategoryArticleBiz();
            biz.Search(param);
            return param.TblCategoryArticleInfos;
        }
        private List<int> Listcategoryinput(FormCollection inputControl)
        {
            var list = new List<int>();
            var subcontrol = inputControl.AllKeys.Where(z => z.StartsWith("intputcate_"));
            foreach (var controlname in subcontrol)
            {
                var control = inputControl.GetValue(controlname);
                var name = controlname.Split('_');
                if (control != null && name.Length > 1)
                {
                    if (control.AttemptedValue.ToUpper() == "ON")
                        if (Utility.ConvertToInt(name[1]).HasValue)
                            list.Add(Utility.ConvertToInt(name[1]).Value);
                }
            }
            return list;
        }
    }
}