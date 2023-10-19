using System;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using VDMMutiline.Core.UI;
using VDMMutiline.Dao;
using VDMMutiline.SharedComponent;
using VDMMutiline.SharedComponent.Constants;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.Models;
using VDMMutiline.SharedComponent.Params;

namespace VDMMutiline.WebBackEnd.Controllers
{
    public class BlockEmailController : BaseController
    {
        BlockEmailDao biz = new BlockEmailDao();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AjaxLoadList(GridFilterSetting<BlockMail> gridFilterSetting, string keyseach)
        {
            var pagininfo = new PagingInfo { RowStart = gridFilterSetting.iDisplayStart, PageSize = gridFilterSetting.iDisplayLength };
            var param = new BlockMailParam() { PagingInfo = pagininfo };
            var filter = new BlockMailFilter() { Search = keyseach };
            param.BlockMailFilter = filter;
            biz.Search(param);
            long count = pagininfo.RecordCount;
            return Json(new { aaData = param.BlockMails, recordsTotal = count, recordsFiltered = count, amount = 0x2710 }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Create(string id)
        {
            var model = new BlockMailParam()
            {
                BlockMail = new BlockMail()
            };
            var _id = 0;
            if (Int32.TryParse(id, out _id))
            {
                model.BlockMail = biz.GetbyId(_id);
            }
            return View(model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(BlockMailParam modelInput, FormCollection forminput)
        {
            try
            {
                if (modelInput != null)
                {
                    modelInput.UserName = CurrentUser.UserName;
                    if (modelInput.BlockMail.Id > 0)
                    {

                        biz.Update(modelInput.BlockMail);
                        return Json(new { isSuccess = true, mess = Constant.Updatesuccess }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        biz.Insert(modelInput.BlockMail);
                    }
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
            BlockMail item = biz.GetbyId(id ?? 0);
            if (item == null)
            {
                ViewBag.Erro = Constant.NotItem;
                return View(new BlockMail());
            }
            else
                return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(BlockMail obj)
        {
            try
            {
                biz.Delete(obj);
                return Json(new { isSuccess = true, mess = Constant.Deletesuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { isSuccess = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}