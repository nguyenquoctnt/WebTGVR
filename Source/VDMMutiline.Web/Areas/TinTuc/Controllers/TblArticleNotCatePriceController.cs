using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace VDMMutiline.WebBackEnd.Areas.TinTuc.Controllers
{
    public class TblArticleNotCatePriceController : BaseController
    {
        TblArticleBiz _biz = new TblArticleBiz();



        public ActionResult Index(int? TypeID)
        {
            return View(TypeID);
        }


        public ActionResult AjaxLoadList(GridFilterSetting<TblArticleInfo> gridFilterSetting, string keyseach, int? Types)
        {
            var pagininfo = new PagingInfo { RowStart = gridFilterSetting.iDisplayStart, PageSize = gridFilterSetting.iDisplayLength };
            var param = new TblArticleParam() { PagingInfo = pagininfo };
            var tblArticleFilter = new TblArticleFilter() { Search = keyseach, typeid = Types, ListUserName = GetparentUserDaiLy().Select(z => z.UserName).ToList() };
            param.TblArticleFilter = tblArticleFilter;
            _biz.Search(param);
            long count = pagininfo.RecordCount;
            return Json(new { aaData = param.TblArticleInfos, recordsTotal = count, recordsFiltered = count, amount = 0x2710 }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Create(int? id, int? typeid)
        {
            var model = new TblArticleParam();
            model.TblCategoryArticleInfos = new List<TblCategoryArticleInfo>();
            int? typevalue = 0;
            if (id.HasValue && id > 0)
            {
                model.Id = id.Value;
                _biz.SetupEditForm(model);
                model.TblCategoryArticleInfos = GetlistTblSanphamCategoryInfo(id.Value);
                typevalue = model.TblArticle.LoaiBaiviet;
            }
            else
            {
                var ck = new TblArticle { LoaiBaiviet = typeid, Id = 0 };
                model.TblArticle = ck;
                typevalue = typeid;
            }
            string value = "";
            // AddTree(ref value, model.TblCategoryArticleInfos, typevalue);
            ViewBag.conten = value;
            var bizcate = new SysCategoryBiz();
            var param = new SysCategoryParam() { };
            var sysCategoryFilter = new SysCategoryFilter() { type = typeid, ListUserName = GetparentUserDaiLy().Select(z => z.UserName).ToList() };
            param.SysCategoryFilter = sysCategoryFilter;
            bizcate.SearchListParam(param);
            var reparelist = param.SysCategoryInfos;
            model.SysCategoryInfos = reparelist;
            return View(model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(TblArticleParam modelInput, FormCollection inputcontrol)
        {
            try
            {
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
                    _biz.Updata(modelInput);
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
        public ActionResult Delete(int? id)
        {
            var param = new TblArticleParam { Id = id ?? 0 };
            _biz.SetupEditForm(param);
            TblArticle item = param.TblArticle;
            if (item == null)
            {
                ViewBag.Erro = Constant.NotItem;
                return View(new TblArticle());
            }
            else
                return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(TblArticle obj)
        {
            try
            {
                var list = new List<TblArticle> { obj };
                var paramDelete = new TblArticleParam { TblArticles = list };
                _biz.Delete(paramDelete);
                WriteLog("Delete thông tin TblArticle", Constant.LogType.Success);
                return Json(new { isSuccess = true, mess = Constant.Deletesuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                WriteLog("Delete thông tin TblArticle", Constant.LogType.Success);
                return Json(new { isSuccess = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        private string TeampleateRoot(int indexroot, SysCategoryInfo obj, bool check, List<TblCategoryArticleInfo> lstold)
        {
            var sb = new StringBuilder();
            var checkvalue = lstold.FirstOrDefault(z => z.IdCate == obj.Id);
            string inputcheck =
                string.Format("<input id='intputcate_{0}' name='intputcate_{1}' {2} type='checkbox'>", obj.Id, obj.Id, (checkvalue != null ? "checked='checked'" : ""));
            if (!check)
                inputcheck = "";
            sb.AppendFormat("<tr class=\"treegrid-{0}\">", indexroot);
            sb.AppendFormat("<td>{0}</td>", inputcheck + obj.TenHienthi);
            sb.Append("</tr>");
            return sb.ToString();

        }
        private string TeampleateParent(int indexroot, int indexparent, SysCategoryInfo obj, bool check, List<TblCategoryArticleInfo> lstold)
        {
            var sb = new StringBuilder();
            var checkvalue = lstold.FirstOrDefault(z => z.IdCate == obj.Id);
            string inputcheck =
                string.Format("<input id='intputcate_{0}' name='intputcate_{1}' {2} type='checkbox'>", obj.Id, obj.Id, (checkvalue != null ? "checked='checked'" : ""));
            if (!check)
                inputcheck = "";
            sb.AppendFormat("<tr class=\"treegrid-{0} treegrid-parent-{1}\">", indexroot, indexparent);
            sb.AppendFormat("<td>{0}</td>", inputcheck + obj.TenHienthi);
            sb.Append("</tr>");
            return sb.ToString();

        }
        private void AddTree(ref string valuestree, List<TblCategoryArticleInfo> lstold, int? type)
        {
            var list = Getlistcate(type);
            var categoryParent = list.Where(z => z.ParentId.HasValue == false).ToList();
            foreach (var iteam in categoryParent)
            {
                var checkparent = (list.Where(z => z.ParentId == iteam.Id).ToList().Count) == 0;
                valuestree += TeampleateRoot(iteam.Id, iteam, checkparent, lstold);
                getChildren(iteam.Id, ref valuestree, lstold, type);
            }
        }
        private void getChildren(int ParentID, ref string valuestree, List<TblCategoryArticleInfo> lstold, int? type)
        {
            var list = Getlistcate(type);
            var categoryChildren = list.Where(z => z.ParentId == ParentID).ToList();
            foreach (var iteam in categoryChildren)
            {
                var checkparent = (list.Where(z => z.ParentId == iteam.Id).ToList().Count) == 0;
                valuestree += TeampleateParent(iteam.Id, iteam.ParentId.Value, iteam, checkparent, lstold);
                getChildren(iteam.Id, ref valuestree, lstold, type);
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
        public List<SysCategoryInfo> Getlistcate(int? typeid)
        {
            var param = new SysCategoryParam() { SysCategoryFilter = new SysCategoryFilter() { type = typeid, ListUserName = GetparentUserDaiLy().Select(z => z.UserName).ToList() } };
            var biz = new SysCategoryBiz();
            biz.Search(param);
            return param.SysCategoryInfos;
        }
        public List<SysCategoryInfo> Getlistcatetree(int? typeid)
        {
            var param = new SysCategoryParam() { SysCategoryFilter = new SysCategoryFilter() { type = typeid, ListUserName = GetparentUserDaiLy().Select(z => z.UserName).ToList() } };
            var biz = new SysCategoryBiz();
            biz.SearchListParam(param);
            return param.SysCategoryInfos;
        }
    }
}