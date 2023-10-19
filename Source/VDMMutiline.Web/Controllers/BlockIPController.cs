using System;
using System.Collections.Generic;
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
    public class BlockIPController : BaseController
    {
        BlockIPDao biz = new BlockIPDao();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AjaxLoadList(GridFilterSetting<BlockIP> gridFilterSetting, string keyseach)
        {
            var pagininfo = new PagingInfo { RowStart = gridFilterSetting.iDisplayStart, PageSize = gridFilterSetting.iDisplayLength };
            var param = new BlockIPParam() { PagingInfo = pagininfo };
            var filter = new BlockIPFilter() { Search = keyseach };
            param.BlockIPFilter = filter;
            biz.Search(param);
            long count = pagininfo.RecordCount;
            return Json(new { aaData = param.BlockIPs, recordsTotal = count, recordsFiltered = count, amount = 0x2710 }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Create(string id)
        {
            var model = new BlockIPParam()
            {
                BlockIP = new BlockIP()
            };
            var _id = 0;
            if (Int32.TryParse(id, out _id))
            {
                model.BlockIP = biz.GetbyId(_id);
            }
            return View(model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(BlockIPParam modelInput, FormCollection forminput)
        {
            try
            {
                if (modelInput != null)
                {
                    modelInput.UserName = CurrentUser.UserName;
                    if (modelInput.BlockIP.Id > 0)
                    {

                        biz.Update(modelInput.BlockIP);
                        return Json(new { isSuccess = true, mess = Constant.Updatesuccess }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        biz.Insert(modelInput.BlockIP);
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
            BlockIP item = biz.GetbyId(id??0);
            if (item == null)
            {
                ViewBag.Erro = Constant.NotItem;
                return View(new BlockIP());
            }
            else
                return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(BlockIP obj)
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