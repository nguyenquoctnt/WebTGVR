using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using VDMMutiline.Dao;
using VDMMutiline.SharedComponent.Constants;
using VDMMutiline.SharedComponent.EntityInfo;
using VDMMutiline.SharedComponent.Params;
using VDMMutiline.Core.UI;
using VDMMutiline.Biz;
namespace VDMMutiline.Templeate1.MB.Controllers
{
    public class FontEndTaiKhoanNganHangController : PublishController
    {
        public ActionResult Index(int? page, string ChuyenMuc)
        {
            var param = new DmTaikhoanNganHangParam();
            var finter = new DmTaikhoanNganHangFilter() { ListUserName = GetparentUserDaiLybyDomain().Select(z => z.UserName).ToList() };
            param.DmTaikhoanNganHangFilter = finter;
            var biz = new DmTaikhoanNganHangBiz();
            biz.Search(param);
            return View(param.DmTaikhoanNganHangInfos);
        }


    }
}