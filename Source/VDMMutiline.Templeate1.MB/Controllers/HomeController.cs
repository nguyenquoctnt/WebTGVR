using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VDMMutiline.Core.UI;
using VDMMutiline.Biz;
using VDMMutiline.SharedComponent.Params;
using VDMMutiline.SharedComponent.Constants;

namespace VDMMutiline.Templeate1.MB.Controllers
{
    public class HomeController : PublishController
    {
        public ActionResult Index()
        {
            return RedirectToAction("Seach", "SeachHome");
            //return View();
        }
        public ActionResult SliderHome()
        {
            var dao = new TblQuangCaoBiz();
            var finter = new TblQuangCaoFilter()
            {
                Vung = 1,
                ListUserName = GetparentUserDaiLybyDomain().Select(z => z.UserName).ToList()
            };
            var param = new TblQuangCaoParam() { TblQuangCaoFilter = finter };
            dao.GetKhoaVung(param);
            return View(param);
        }
        public ActionResult KhuyenMai()
        {
            return View(GetlistTinTake(10, Constant.CateType.KhuyenMai));
        }
        public ActionResult TinTuc()
        {
            return View(GetlistTinTake(10, Constant.CateType.TinTuc));
        }
        public ActionResult Doitac()
        {
            return View(GetlistImageTake(30, Constant.CateType.DoiTac));
        }
        private List<VDMMutiline.SharedComponent.EntityInfo.TblArticleInfo> GetlistImageTake(int? take, int type)
        {
            var filter = new TblArticleFilter
            {
                Search = string.Empty,
                typeid = type,
                ListUserName = GetparentUserDaiLybyDomain().Select(z => z.UserName).ToList()
            };
            var param = new TblArticleParam { TblArticleFilter = filter, Take = take };
            var biz = new TblArticleBiz();
            biz.Search(param);
            return param.TblArticleInfos;
        }
        private List<VDMMutiline.SharedComponent.EntityInfo.TblArticleInfo> GetlistTinTake(int? take, int type)
        {
            var filter = new TblArticleFilter
            {
                Search = string.Empty,
                typeid = type,
                Ishot = true,
                ListUserName = GetparentUserDaiLybyDomain().Select(z => z.UserName).ToList()
            };
            var param = new TblArticleParam { TblArticleFilter = filter, Take = take };
            var biz = new TblArticleBiz();
            biz.Search(param);
            return param.TblArticleInfos;
        }
    }
}