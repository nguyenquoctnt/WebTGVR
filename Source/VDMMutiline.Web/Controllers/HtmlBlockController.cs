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
using VDMMutiline.Core;
using VDMMutiline.Ultilities;

namespace VDMMutiline.WebBackEnd.Controllers
{
   
    public class HtmlBlockController : BaseController
    {
        HtmlBlockBiz _biz = new HtmlBlockBiz();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AjaxLoadList(GridFilterSetting<TblQuangCaoInfo> gridFilterSetting, string keyseach)
        {
            var pagininfo = new PagingInfo { RowStart = gridFilterSetting.iDisplayStart, PageSize = gridFilterSetting.iDisplayLength };
            var param = new HtmlBlockParam() { PagingInfo = pagininfo };
            var HtmlBlockFilter = new HtmlBlockFilter() {  ListUserName = GetparentUserDaiLy().Select(z => z.UserName).ToList() };
            param.HtmlBlockFilter = HtmlBlockFilter;
            _biz.Search(param);
            long count = pagininfo.RecordCount;
            return Json(new { aaData = param.HtmlBlockInfos, recordsTotal = count, recordsFiltered = count, amount = 0x2710 }, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult Create(string id)
        {

            var model = new HtmlBlockParam();
            int _id = 0;
            if (Int32.TryParse(id, out _id))
            {
                var param = new HtmlBlockParam { Id = _id };
                _biz.SetupEditForm(param);
                model = param;
            }
            else
            {
                var ck = new tbl_HtmlBlock
                {
                    ID = 0
                };
                model.HtmlBlock = ck;
            }
            return View(model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(HtmlBlockParam modelInput,FormCollection forminput)
        {
            try
            {
                if (modelInput != null)
                {
                    modelInput.UserName = CurrentUser.UserName;
                    if (modelInput.HtmlBlock.ID > 0)
                    {

                        modelInput.HtmlBlock.EditDay = DateTime.Now;
                        modelInput.HtmlBlock.EditBy =CurrentUser.UserName;
                        _biz.Update(modelInput);
                        WriteLog("Cập nhật thông tin html block", Constant.LogType.Success);
                        return Json(new { isSuccess = true, mess = Constant.Updatesuccess }, JsonRequestBehavior.AllowGet);


                    }


                    else
                    {
                        modelInput.HtmlBlock.EditDay = DateTime.Now;
                        modelInput.HtmlBlock.EditBy = CurrentUser.UserName;
                        modelInput.HtmlBlock.CreateDay = DateTime.Now;
                        modelInput.HtmlBlock.CreateBy = CurrentUser.UserName;
                        WriteLog("Thêm mới thông tin  html block", Constant.LogType.Success);
                        _biz.Insert(modelInput);
                    }
                    return Json(new { isSuccess = true, mess = Constant.Addnewsuccess }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { isSuccess = false, mess = Constant.AcactFail }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                WriteLog("Cập nhật thông tin htmblock", Constant.LogType.Success);
                return Json(new { isSuccess = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult Delete(int? id)
        {
            var param = new HtmlBlockParam { Id = id ?? 0 };
            _biz.SetupEditForm(param);
            tbl_HtmlBlock item = param.HtmlBlock;
            if (item == null)
            {
                ViewBag.Erro = Constant.NotItem;
                return View(new tbl_HtmlBlock());
            }
            else
                return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(tbl_HtmlBlock obj)
        {
            try
            {
                var list = new List<tbl_HtmlBlock> { obj };
                var paramDelete = new HtmlBlockParam { HtmlBlocks = list };
                _biz.Delete(paramDelete);
                WriteLog("Xóa thông tin html block", Constant.LogType.Success);
                return Json(new { isSuccess = true, mess = Constant.Deletesuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                WriteLog("Xóa thông tin html block", Constant.LogType.Success);
                return Json(new { isSuccess = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }




    }
}