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

namespace VDMMutiline.WebBackEnd.Controllers
{
    public class TblCategoryArticleController : BaseController
    {
        TblCategoryArticleBiz _biz = new TblCategoryArticleBiz();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AjaxLoadList(GridFilterSetting<TblCategoryArticleInfo> gridFilterSetting, string keyseach)
        {
            var pagininfo = new PagingInfo { RowStart = gridFilterSetting.iDisplayStart, PageSize = gridFilterSetting.iDisplayLength };
            var param = new TblCategoryArticleParam() { PagingInfo = pagininfo };
            var TblCategoryArticleFilter = new TblCategoryArticleFilter() { };
            param.TblCategoryArticleFilter = TblCategoryArticleFilter;
            _biz.Search(param);
            long count = pagininfo.RecordCount;
            return Json(new { aaData = param.TblCategoryArticleInfos, recordsTotal = count, recordsFiltered = count, amount = 0x2710 }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Create(string id)
        {
            var model = new TblCategoryArticleParam();
            var _id = 0;
            if (Int32.TryParse(id, out _id))
            {
                model.Id = _id;
                _biz.SetupEditForm(model);
            }
            else
            {
                var ck = new TblCategoryArticle();
                model.TblCategoryArticle = ck;
            }
            return View(model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(TblCategoryArticleParam modelInput)
        {
            try
            {
                if (modelInput != null)
                {
                  
                    modelInput.UserName =  CurrentUser.UserName;
                    if (modelInput.TblCategoryArticle.Id > 0)
                    {
                     
                        _biz.Update(modelInput);
                        WriteLog("Cập nhật thông tin CategoryArticle", Constant.LogType.Success);
                        return Json(new { isSuccess = true, mess=Constant.Updatesuccess }, JsonRequestBehavior.AllowGet);
                    }
                   
                    _biz.Insert(modelInput);
                    WriteLog("Add thông tin CategoryArticle", Constant.LogType.Success);
                    return Json(new { isSuccess = true, mess = Constant.Addnewsuccess }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { isSuccess = false, mess = Constant.AcactFail }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                WriteLog("Cập nhật thông tin CategoryArticle", Constant.LogType.Success);
                return Json(new { isSuccess = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            
        }
        public ActionResult Delete(int? id)
        {
            var param = new TblCategoryArticleParam { Id = id??0 };
            _biz.SetupEditForm(param);
            TblCategoryArticle item = param.TblCategoryArticle;
            if (item == null)
            {
                ViewBag.Erro = Constant.NotItem;
                return View(new TblCategoryArticle());
            }
            else
                return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(TblCategoryArticle obj)
        {
            try
            {
                var list = new List<TblCategoryArticle> { obj };
                var paramDelete = new TblCategoryArticleParam { TblCategoryArticles = list };
                _biz.Delete(paramDelete);
                WriteLog("Xóa thông tin CategoryArticle", Constant.LogType.Success);
                return Json(new { isSuccess = true , mess = Constant.Deletesuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                WriteLog("Xóa thông tin CategoryArticle", Constant.LogType.Success);
                return Json(new { isSuccess = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}