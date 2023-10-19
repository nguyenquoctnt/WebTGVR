using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using VDMMutiline.Dao;
using VDMMutiline.SharedComponent;
using VDMMutiline.SharedComponent.Constants;
using VDMMutiline.SharedComponent.EntityInfo;
using VDMMutiline.SharedComponent.Params;
using VDMMutiline.Ultilities;
using VDMMutiline.Core.UI;
using VDMMutiline.Biz;
namespace VDMMutiline.Templeate1.Controllers
{
    public class FontEndnewController : PublishController
    {
        public ActionResult ChiTiet(string key)
        {
            var param = new TblArticleParam() { key = key };
            var bizsp = new TblArticleBiz();
            bizsp.SetupDisplaybykey(param);
            return View(param.TblArticleInfo);
        }
        public ActionResult Index(int? page, string ChuyenMuc)
        {
            var list = new List<TblArticleInfo>();
            int pageNumber = (page ?? 1);
            int pageSize = 10;
            var pInfo = new PagingInfo(pageNumber, pageSize);
            var bizTinTuc = new TblArticleBiz();
            var filter = new TblArticleFilter()
            {
                typeid = Constant.CateType.NoiDung,
                ListUserName = GetparentUserDaiLybyDomain().Select(z => z.UserName).ToList()
            };
            var paramTinTuc = new TblArticleParam { PagingInfo = pInfo  };
            var objchuyenmuc = Getcatebykey(ChuyenMuc);
            if (objchuyenmuc != null)
            {
                filter.ChuyenMucID = objchuyenmuc.Id;
                paramTinTuc.SysCategoryInfo = objchuyenmuc;
            }
            paramTinTuc.TblArticleFilter = filter;


            bizTinTuc.SearchHome(paramTinTuc);
            paramTinTuc.PagedList = new StaticPagedList<TblArticleInfo>(paramTinTuc.TblArticleInfos, paramTinTuc.PagingInfo.PageIndex, paramTinTuc.PagingInfo.PageSize, paramTinTuc.PagingInfo.RecordCount);
            return View(paramTinTuc);
        }
        private SysCategoryInfo Getcatebykey(string SeoUrl)
        {
            var param = new SysCategoryParam { SysCategoryFilter = new SysCategoryFilter { SeoUrl = SeoUrl } };
            var biz = new SysCategoryBiz();
            biz.GetInfoBykey(param);
            return param.SysCategoryInfo;
        }
    }
}