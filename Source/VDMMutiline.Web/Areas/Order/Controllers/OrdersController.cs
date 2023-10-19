using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VDMMutiline.Dao;
using VDMMutiline.SharedComponent;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.Models;
using VDMMutiline.SharedComponent.Params;
using VDMMutiline.Core.UI;
using VDMMutiline.SharedComponent.Constants;

namespace VDMMutiline.WebBackEnd.Areas.Order.Controllers
{
    public class OrdersController : BaseController
    {
        public ActionResult Index(int? page)
        {
            return View();
        }
        public ActionResult AjaxLoadList(GridFilterSetting<BK_Order> gridFilterSetting, string keyseach)
        {
            var dao = new OlderDao();
            var pagininfo = new PagingInfo { RowStart = gridFilterSetting.iDisplayStart, PageSize = gridFilterSetting.iDisplayLength };
            var param = new OlderParam() { Seach = keyseach, PagingInfo = pagininfo, ListUserSite = GetparentUserDaiLy().Select(z => z.UserName).ToList() };
            dao.Search(param);
            long count = pagininfo.RecordCount;
            return Json(new { aaData = param.BK_Orders, recordsTotal = count, recordsFiltered = count, amount = 0x2710 }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Delete(int id)
        {
            var biz = new OlderDao();
            var item = biz.GetbyId(id);
            if (item == null)
                return View("Bản ghi không tồn tại!");
            else
                return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(BK_Order item)
        {
            try
            {
                var biz = new OlderDao();
                biz.Delete(item.ID);
                WriteLog("Delete thông tin Order", Constant.LogType.Success);
                return Json(new { isSuccess = true, mess = Constant.Deletesuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                WriteLog("Delete thông tin Order", Constant.LogType.Success);
                return Json(new { isSuccess = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult CapNhapTrangThai(int? id)
        {
            var dao = new OlderDao();
            var obj = new BK_Order();
            obj.DeletedDate = DateTime.Now;
            obj.ID = id ?? 0;
            dao.Update(obj);
            WriteLog("Cập nhập trạng thái Order", Constant.LogType.Success);
            return Json(new { isSuccess = true, mess = Constant.Updatesuccess }, JsonRequestBehavior.AllowGet);
        }
    }
}