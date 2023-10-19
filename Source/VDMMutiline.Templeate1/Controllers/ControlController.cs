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
using VDMMutiline.Dao;
using VDMMutiline.SharedComponent.Entities;

namespace VDMMutiline.Templeate1.Controllers
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
            var param = new SysCategoryParam()
            {
                SysCategoryFilter = new SysCategoryFilter
                {
                    Isfooter = true,
                    type = Constant.CateType.NoiDung,
                    ListUserName = VDMMutiline.Core.UI.CommonUI.GetparentUserDaiLybyDomain().Select(z => z.UserName).ToList()
                },
            };
            var biz = new SysCategoryBiz();
            biz.Search(param);
            return View(param.SysCategoryInfos);
        }
        public ActionResult NhanVienHotro()
        {
            return View(GetlistTinTake(2, Constant.CateType.NhanVienHoTro, null));
        }
        public ActionResult home_page_services()
        {
            return View();
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
        public ActionResult SubmitMail()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SubmitMail(FormCollection forminput)
        {
            if (forminput.GetValue("Savemail") != null)
            {
                var curentDomain = SiteMuti.Getsitename();
                var CurentUser = CommonUI.GetInforByDomain(curentDomain);
                var dao = new MailDao();
                var ck = new tbl_Mail();
                var mailinput = forminput.GetValue("sendMail");
                if (mailinput != null)
                    ck.Mail = mailinput.AttemptedValue;
                ck.UserSite = CurentUser.UserName;
                ck.SourceSite = curentDomain;
                dao.Insert(ck);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }
    }
}