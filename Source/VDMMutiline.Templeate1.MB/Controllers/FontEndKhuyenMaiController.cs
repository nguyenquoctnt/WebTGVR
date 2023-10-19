using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using VDMMutiline.Dao;
using VDMMutiline.Biz;
using VDMMutiline.SharedComponent.Constants;
using VDMMutiline.SharedComponent.EntityInfo;
using VDMMutiline.SharedComponent.Params;
using VDMMutiline.Core.UI;
using VDMMutiline.SharedComponent;

namespace VDMMutiline.Templeate1.MB.Controllers
{
    public class FontEndKhuyenMaiController : PublishController
    {
        public ActionResult Index(int? page, string ChuyenMuc)
        {
            var list = new List<TblArticleInfo>();
            int pageNumber = (page ?? 1);
            int pageSize = 10;
            var pInfo = new PagingInfo(pageNumber, pageSize);
            var bizTinTuc = new TblArticleBiz();
            var filter = new TblArticleFilter()
            {
                typeid = Constant.CateType.KhuyenMai,
                ListUserName = GetparentUserDaiLybyDomain().Select(z => z.UserName).ToList()
            };
            var paramTinTuc = new TblArticleParam { PagingInfo = pInfo, TblArticleFilter = filter };
            bizTinTuc.SearchHome(paramTinTuc);
            paramTinTuc.PagedList = new StaticPagedList<TblArticleInfo>(paramTinTuc.TblArticleInfos, paramTinTuc.PagingInfo.PageIndex, paramTinTuc.PagingInfo.PageSize, paramTinTuc.PagingInfo.RecordCount);
            return View(paramTinTuc);
        }



    }
}