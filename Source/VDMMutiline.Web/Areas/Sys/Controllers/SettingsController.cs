using Microsoft.Web.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VDMMutiline.Biz;
using VDMMutiline.Core;
using VDMMutiline.Core.Common;
using VDMMutiline.Core.UI;
using VDMMutiline.SharedComponent.Constants;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
using VDMMutiline.SharedComponent.Params;

namespace VDMMutiline.WebBackEnd.Areas.Sys.Controllers
{
    public class SettingsController : BaseController
    {
        // GET: Sys/Settings
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult SettingGroup(int? Idgroup)
        {
            var param = new SettingGroupParam();
            param.IdGroup = Idgroup;
            param.SettingGroupInfos = GetSettingGroup(Idgroup, CurrentUser.Id);
            return View(param);
        }
        [HttpPost]
        public ActionResult SettingGroup(SettingGroupParam param)
        {
            try
            {
                var dao = new SysSettingBiz();
                foreach (var item in param.SettingGroupInfos)
                {
                    var ck = new SettingGroup
                    {
                        VaitroId = param.IdGroup.HasValue ? param.IdGroup.Value : 0,
                        IDSetting = item.IdKey,
                        CreateUser = CurrentUser.Id,
                        Value = item.Value,
                        UpdateDate = DateTime.Now
                    };
                    param.SettingGroup = ck;
                    dao.UpdataSettingGroup(param);
                }
                param.SettingGroupInfos = GetSettingGroup(param.IdGroup, CurrentUser.Id);
                return View(param);
            }
            catch
            {
                return View(param);
            }
        }
        public ActionResult Settinguser(int? IdUser)
        {
            var aspnetuserparam = new AspNetUsersParam() { AspNetUsersFilter = new AspNetUsersFilter { Id = IdUser } };
            var aspnetuserbiz = new AspNetUsersBiz();
            aspnetuserbiz.SetupEditForm(aspnetuserparam);
            aspnetuserparam.SettingUserInfos = GetSetting(IdUser);

            if (aspnetuserparam.AspNetUsersInfo.ParentId.HasValue)
                aspnetuserparam.SettingUserInfos = aspnetuserparam.SettingUserInfos.Where(z => z.GroupID != "QT").ToList();
            ViewBag.checkdaily = aspnetuserparam.AspNetUsersInfo.ParentId.HasValue == false;
            //aspnetuserparam.StyleSites = GetStyleSite(CurrentUser.SiteId);
            //if (aspnetuserparam.StyleSites.Count == 0)
            //    aspnetuserparam.StyleSites = new List<tbl_StyleSite>();

            //if (aspnetuserparam.AspNetUsersInfo != null)
            //{
            //    var parammenu = new MenuHomeParam();
            //    var bizmenu = new MenuHomeBiz();
            //    parammenu.UserName = aspnetuserparam.AspNetUsersInfo.UserName;
            //    bizmenu.GetlistMenuHome(parammenu);
            //    aspnetuserparam.SysMenuHomeInfos = parammenu.SysMenuHomeInfos;
            //}

            return View(aspnetuserparam);
        }
        [HttpPost]
        public ActionResult Settinguser(AspNetUsersParam param)
        {
            var aspnetuserparam = new AspNetUsersParam() { AspNetUsersFilter = new AspNetUsersFilter { Id = param.AspNetUsersInfo.Id } };
            var aspnetuserbiz = new AspNetUsersBiz();
            aspnetuserbiz.SetupEditForm(aspnetuserparam);
            ViewBag.erro = "";
            try
            {
                ViewBag.checkdaily = aspnetuserparam.AspNetUsersInfo.ParentId.HasValue == false;
                if (aspnetuserparam.AspNetUsersInfo != null)
                {
                    var objuser = aspnetuserparam.AspNetUsersInfo;
                    var domain = SiteMuti.Getsitename();
                    var changeiis = false;
                    param.AspNetUsersInfo.Id = param.AspNetUsersInfo.Id;
                    param.AspNetUsersInfo.UrlDomain2 = !string.IsNullOrEmpty(param.AspNetUsersInfo.UrlDomain2) ? param.AspNetUsersInfo.UrlDomain2.Trim().ToLower() : "";
                    param.AspNetUsersInfo.UrlDomain3 = !string.IsNullOrEmpty(param.AspNetUsersInfo.UrlDomain3) ? param.AspNetUsersInfo.UrlDomain3.Trim().ToLower() : "";
                    ServerManager serverMgr = new ServerManager();
                    try
                    {
                        if (domain.Trim().ToUpper() == Domainsys.Trim().ToUpper())
                        {
                            Site mySite = serverMgr.Sites[objuser.UrlDomain1];
                            Site mySitemobile = serverMgr.Sites["m." + objuser.UrlDomain1];
                            if (objuser.UrlDomain2 != param.AspNetUsersInfo.UrlDomain2 && !string.IsNullOrEmpty(objuser.UrlDomain2))
                            {
                                try
                                {
                                    var objhost = mySite.Bindings.FirstOrDefault(z => z.Host == objuser.UrlDomain2);
                                    if (objhost != null)
                                        mySite.Bindings.Remove(objhost);

                                    var objhostmobile = mySitemobile.Bindings.FirstOrDefault(z => z.Host == ("m." + objuser.UrlDomain2));
                                    if (objhostmobile != null)
                                        mySitemobile.Bindings.Remove(objhostmobile);
                                    changeiis = true;
                                }
                                catch (Exception ex)
                                {
                                    ViewBag.erro += "- loi xoa domain 2" + ex.Message;
                                }
                            }
                            if (objuser.UrlDomain3 != param.AspNetUsersInfo.UrlDomain3 && !string.IsNullOrEmpty(objuser.UrlDomain3))
                            {
                                try
                                {
                                    var objhost = mySite.Bindings.FirstOrDefault(z => z.Host == objuser.UrlDomain3);
                                    if (objhost != null)
                                        mySite.Bindings.Remove(objhost);
                                    var objhostmobile = mySitemobile.Bindings.FirstOrDefault(z => z.Host == ("m." + objuser.UrlDomain3));
                                    if (objhostmobile != null)
                                        mySitemobile.Bindings.Remove(objhostmobile);
                                    changeiis = true;
                                }
                                catch (Exception ex)
                                {
                                    ViewBag.erro += "- loi xoa domain 3" + ex.Message;
                                }
                            }
                            if (!string.IsNullOrEmpty(param.AspNetUsersInfo.UrlDomain2))
                            {
                                ViewBag.erro += "- check  rỗng 2 ok";
                                var checkdomain2 = checkdomain(param.AspNetUsersInfo.UrlDomain2);
                                if (!checkdomain2 && param.AspNetUsersInfo.UrlDomain2 != objuser.UrlDomain2)
                                {
                                    ViewBag.erro = ("Tên miền 2 đã tồn tại trong hệ thống " + "- " + checkdomain2.ToString() + " - " + (param.AspNetUsersInfo.UrlDomain2 != objuser.UrlDomain2).ToString() + " - " + param.AspNetUsersInfo.UrlDomain2 + "- " + objuser.UrlDomain2);
                                    param.SettingUserInfos = GetSetting(objuser.Id);
                                    return View(param);
                                }
                                var objhost = mySite.Bindings.FirstOrDefault(z => z.Host == param.AspNetUsersInfo.UrlDomain2);
                                if (objhost == null)
                                {
                                    mySite.Bindings.Add("*:80:" + param.AspNetUsersInfo.UrlDomain2, "http");
                                }
                                var objhostmobile = mySitemobile.Bindings.FirstOrDefault(z => z.Host == ("m." + param.AspNetUsersInfo.UrlDomain2));
                                if (objhostmobile == null)
                                {
                                    mySitemobile.Bindings.Add("*:80:" + "m." + param.AspNetUsersInfo.UrlDomain2, "http");
                                }
                                changeiis = true;
                            }
                            if (!string.IsNullOrEmpty(param.AspNetUsersInfo.UrlDomain3))
                            {
                                var checkdomain3 = checkdomain(param.AspNetUsersInfo.UrlDomain3);
                                if (!checkdomain3 && param.AspNetUsersInfo.UrlDomain3 != objuser.UrlDomain3)
                                {
                                    param.SettingUserInfos = GetSetting(objuser.Id);
                                    return View(param);
                                }
                                var objhost = mySite.Bindings.FirstOrDefault(z => z.Host == param.AspNetUsersInfo.UrlDomain3);
                                if (objhost == null)
                                {
                                    mySite.Bindings.Add("*:80:" + param.AspNetUsersInfo.UrlDomain3, "http");
                                }
                                var objhostmobile = mySitemobile.Bindings.FirstOrDefault(z => z.Host == ("m." + param.AspNetUsersInfo.UrlDomain3));
                                if (objhostmobile == null)
                                {
                                    mySitemobile.Bindings.Add("*:80:" + "m." + param.AspNetUsersInfo.UrlDomain3, "http");
                                }
                                changeiis = true;
                            }
                        }


                    }
                    catch (Exception ex)
                    {
                        ViewBag.erro += ex.Message;
                    }
                    var bizsetting = new SysSettingBiz();
                    var paramsetting = new SettingUserParam();
                    foreach (var item in param.SettingUserInfos)
                    {
                        var ck = new SettingUser()
                        {
                            IDSetting = item.IdKey,
                            UserID = objuser.Id,
                            Value = item.Value,
                        };
                        paramsetting.SettingUser = ck;
                        bizsetting.UpdataSettingUser(paramsetting);
                    }
                    param.SettingUserInfos = GetSetting(objuser.Id);
                    var linkfile = UploadFile.UpLoadFile(Request.Files["fileAvatar"], "Upload/avtaruser/");
                    if (!string.IsNullOrEmpty(linkfile) && linkfile != param.AspNetUsersInfo.Avatar)
                    {
                        param.AspNetUsersInfo.Avatar = linkfile;
                    }
                    string linklogo = UploadFile.UpLoadFile(Request.Files["fileLogoUrl"], "Upload/logosite/");
                    if (!string.IsNullOrEmpty(linklogo) && linklogo != param.AspNetUsersInfo.LogoUrl)
                    {
                        param.AspNetUsersInfo.LogoUrl = linklogo;
                    }
                    string filebaner = UploadFile.UpLoadFile(Request.Files["filebaner"], "Upload/Banersite/");
                    if (!string.IsNullOrEmpty(filebaner) && filebaner != param.AspNetUsersInfo.Baner)
                    {
                        param.AspNetUsersInfo.Baner = filebaner;
                    }
                    var fileLogoMbUrl = UploadFile.UpLoadFile(Request.Files["fileLogoMbUrl"], "Upload/logosite/");
                    if (!string.IsNullOrEmpty(fileLogoMbUrl) && fileLogoMbUrl != param.AspNetUsersInfo.LogoMbUrl)
                    {
                        param.AspNetUsersInfo.LogoMbUrl = fileLogoMbUrl;
                    }

                    var bizuser = new AspNetUsersBiz();
                    bizuser.UpdateEditdomain(param);

                    //var parammenu = new MenuHomeParam();
                    //var bizmenu = new MenuHomeBiz();
                    //parammenu.SysMenuHomeInfos = param.SysMenuHomeInfos;
                    //parammenu.UserName = aspnetuserparam.AspNetUsersInfo.UserName;
                    //bizmenu.Updata(parammenu);

                    if (changeiis)
                    {
                        ViewBag.erro += "-CommitChanges sv ";
                        serverMgr.CommitChanges();
                    }
                    return RedirectToAction("Settinguser", "Settings", new { IdUser = objuser.Id });
                }
            }
            catch (Exception ex)
            {
                ViewBag.erro += ex.Message;
            }
            aspnetuserparam.SettingUserInfos = GetSetting(param.AspNetUsersInfo.Id);
            return View(aspnetuserparam);
        }
        private bool checkdomain(string UrlDomain)
        {
            var aspnetuserparam = new AspNetUsersParam() { UrlDomain = UrlDomain };
            var aspnetuserbiz = new AspNetUsersBiz();
            return aspnetuserbiz.Checkdomain(aspnetuserparam);
        }
        private List<SettingUserInfo> GetSetting(int? IdUser)
        {
            var biz = new SysSettingBiz();
            var param = new SettingUserParam() { Userid = IdUser };
            biz.GetAllSettingUser(param);
            return param.SettingUserInfos;
        }
        private List<SettingGroupInfo> GetSettingGroup(int? Idgroup, int? UserID)
        {
            var biz = new SysSettingBiz();
            var param = new SettingGroupParam() { IdGroup = Idgroup, Userid = UserID };
            biz.GetAllSettingGroup(param);
            return param.SettingGroupInfos;
        }
        private List<tbl_StyleSite> GetStyleSite(int? siteid)
        {
            var param = new SiteInfoParam { Id = siteid ?? 0 };
            var biz = new SiteInfoBiz();
            biz.GetStyleBySiteID(param);
            return param.StyleSites;
        }
    }
}