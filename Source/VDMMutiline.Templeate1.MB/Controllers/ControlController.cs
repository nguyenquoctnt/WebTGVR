using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VDMMutiline.SharedComponent.Params;
using VDMMutiline.SharedComponent.Constants;
using VDMMutiline.Ultilities;
using VDMMutiline.Core.Common;
using VDMMutiline.Core.UI;
using VDMMutiline.Biz;
using VDMMutiline.SharedComponent.EntityInfo;

namespace VDMMutiline.Templeate1.MB.Controllers
{
    public class ControlController : PublishController
    {

        public ActionResult Header()
        {
            var param = new SysCategoryParam()
            {
                SysCategoryFilter = new SysCategoryFilter
                {
                    IsMenu = true,
                    type = Constant.CateType.NoiDung,
                    ListUserName = VDMMutiline.Core.UI.CommonUI.GetparentUserDaiLybyDomain().Select(z => z.UserName).ToList()
                },
            };
            var biz = new SysCategoryBiz();
            biz.Search(param);
            return View(param.SysCategoryInfos);
        }

        public ActionResult Footer()
        {
            return View(GetlistTinTake(5, Constant.CateType.NoiDung, true));
        }
        public ActionResult HeaderDetail()
        {
            return View();
        }
        public ActionResult NhanVienHotro()
        {
            return View(GetlistTinTake(5, Constant.CateType.NhanVienHoTro, null));
        }
        private List<VDMMutiline.SharedComponent.EntityInfo.TblArticleInfo> GetlistTinTake(int? take, int type, bool? showmenu)
        {
            var filter = new TblArticleFilter
            {
                Search = string.Empty,
                typeid = type,
                IsShowMenu = showmenu,
                ListUserName = GetparentUserDaiLybyDomain().Select(z => z.UserName).ToList()
            };
            var param = new TblArticleParam { TblArticleFilter = filter, Take = take };
            var biz = new TblArticleBiz();
            biz.Search(param);
            return param.TblArticleInfos;
        }
    }
}