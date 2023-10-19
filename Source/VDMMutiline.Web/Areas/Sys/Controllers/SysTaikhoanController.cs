using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Collections.Generic;
using VDMMutiline.Core;
using VDMMutiline.Core.UI;
using VDMMutiline.Core.Models;
using VDMMutiline.SharedComponent.Models;
using VDMMutiline.SharedComponent.Constants;
using System;
using VDMMutiline.SharedComponent.EntityInfo;
using VDMMutiline.Biz;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.Params;
using System.Threading.Tasks;
using Microsoft.Web.Administration;
using System.Net.Mail;
using VDMMutiline.Core.Common;

namespace VDMMutiline.WebBackEnd.Areas.Sys.Controllers
{
    public class SysTaikhoanController : BaseController
    {
        private ActSignInManager _signInManager;
        private VDMUserManager _userManager;
        private AspNetUsersBiz aspnetUsersBiz = new AspNetUsersBiz();
        public ActSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ActSignInManager>();
            }

            private set
            {
                _signInManager = value;
            }
        }
        private VDMUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<VDMUserManager>();
            }

            set
            {
                _userManager = value;
            }
        }
        // GET: HeThong/SysTaikhoan
        public ActionResult Index()
        {
            ViewBag.Username = CurrentUser.UserName;

            var param = new VDMMutiline.SharedComponent.Params.AspNetUsersParam()
            {
                AspNetUsersFilter = new VDMMutiline.SharedComponent.Params.AspNetUsersFilter() { },
            };
            if (IsAdmin())
                param.AspNetUsersFilter.UserList = new List<string>();
            else
            {
                param.AspNetUsersFilter.UserList = GetparentUserDaiLy().Select(z => z.UserName).ToList();
                if (param.AspNetUsersFilter.UserList.Count == 0)
                    param.AspNetUsersFilter.UserList.Add(CurrentUser.UserName);
            }
            ViewBag.isadmin = IsAdmin();


            aspnetUsersBiz.Search(param);
            param.AspNetUsersInfos = param.AspNetUsersInfos.Where(z => z.ParentId == null).ToList();
            param.AspNetUsersInfos.Insert(0, new AspNetUserInfo() { UserName = "[Chọn đại lý]", Id = 0 });
            var reparelist = (param.AspNetUsersInfos.Select(n => new SelectListItem() { Text = n.UserName, Value = n.Id.ToString() })).ToList();
            ViewBag.DataTree = reparelist;
            return View();
        }
        public ActionResult AjaxLoadList(GridFilterSetting<AspNetUserInfo> gridFilterSetting, string keyseach, int? ParentId)
        {
            var param = new VDMMutiline.SharedComponent.Params.AspNetUsersParam()
            {
                AspNetUsersFilter = new VDMMutiline.SharedComponent.Params.AspNetUsersFilter() { Search = keyseach },
                PagingInfo = new VDMMutiline.SharedComponent.PagingInfo()
                {
                    PageSize = gridFilterSetting.iDisplayLength,
                    RowStart = gridFilterSetting.iDisplayStart
                }
            };
            if (IsAdmin())
                param.AspNetUsersFilter.UserList = new List<string>();
            else
            {
                param.AspNetUsersFilter.UserList = GetparentUserDaiLy().Select(z => z.UserName).ToList();
                if (param.AspNetUsersFilter.UserList.Count == 0)
                    param.AspNetUsersFilter.UserList.Add(CurrentUser.UserName);
            }
            if (ParentId > 0)
            {
                param.AspNetUsersFilter.ParentId = ParentId;
            }
            aspnetUsersBiz.Search(param);
            long count = param.PagingInfo.RecordCount;
            return Json(new { aaData = param.AspNetUsersInfos, recordsTotal = count, recordsFiltered = count, amount = 0x2710 }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Create(int? id)
        {
            var param = new VDMMutiline.SharedComponent.Params.AspNetUsersParam() { AspNetUsersFilter = new VDMMutiline.SharedComponent.Params.AspNetUsersFilter() { Id = id } };
            aspnetUsersBiz.SetupEditForm(param);
            if (param.AspNetUsersInfo == null)
            {
                param.AspNetUsersInfo = new AspNetUserInfo();
            }
            var bizgroupuser = new AspNetUserGroupsBiz();
            var paramgroupuser = new AspNetUserGroupsParam() { userId = id ?? 0 };
            bizgroupuser.GetAll(paramgroupuser);
            param.AspNetUserGroupsInfos = paramgroupuser.AspNetUserGroupsInfos;
            var bizgroup = new AspNetGroupsBiz();
            var paramgroup = new AspNetGroupsParam();
            bizgroup.GetAll(paramgroup);
            param.AspNetGroupsInfos = paramgroup.AspNetGroupsInfos;
            if (id == null)
            {
                param.AspNetUserGroupsInfos = new List<AspNetUserGroupsInfo>();
            }
            if (!IsAdmin())
            {
                param.AspNetGroupsInfos = param.AspNetGroupsInfos.Where(z => z.Id != 1 && z.Id != 18 && z.Id != 23).ToList();
            }
            param.Id = param.AspNetUsersInfo.Id;
            return View(param);
        }
        [HttpPost, ValidateInput(false)]
        public JsonResult Create(AspNetUsersParam param, string password)
        {
            try
            {
                var avatar = UploadFile.UpLoadFile(Request.Files["Thumbinput"], "Upload/Avatar/");
                if (!string.IsNullOrEmpty(avatar))
                {
                    param.AspNetUsersInfo.Avatar = avatar;
                }
                var model = param.AspNetUsersInfo;
                if (model != null && model.Id > 0)
                {
                    var checkemail = aspnetUsersBiz.CheckMail(param.AspNetUsersInfo.Email, param.AspNetUsersInfo.UserName);
                    if (!checkemail)
                    {
                        var parauser = new AspNetUsersParam() { AspNetUsersFilter = new AspNetUsersFilter() };
                        parauser.AspNetUsersInfo = model;
                        aspnetUsersBiz.Update(parauser);
                        var biz = new AspNetUserGroupsBiz();
                        var paramgroup = new AspNetUserGroupsParam() { Id = model.Id };
                        biz.GetUserByGroup(paramgroup);
                        biz.Delete(paramgroup);
                        var listgroup = new List<int?>();
                        foreach (var item in Request.Form.AllKeys.Where(p => p.StartsWith("group_")))
                        {
                            if (Request.Form.GetValues(item).Length > 0 && Request.Form.GetValues(item)[0] == "true")
                            {
                                var results = item.Replace("group_", "");
                                int roleid = 0;
                                if (int.TryParse(results, out roleid))
                                {
                                    AspNetUserGroup sitem = new AspNetUserGroup();
                                    param.AspNetUserRoles = new List<AspNetUserRole>();
                                    sitem.GroupId = roleid;
                                    sitem.UserId = parauser.AspNetUsersInfo.Id;
                                    paramgroup.AspNetUserGroups = sitem;
                                    biz.Insert(paramgroup);
                                    listgroup.Add(roleid);
                                }
                            }
                        }
                        WriteLog("Update tài khoản thành công", Constant.LogType.Success);
                        return Json(new { isSuccess = true, mess = Constant.Updatesuccess }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { isSuccess = false, mess = "Email đã tồn tại." }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    var user = new VDMUser
                    {
                        UserName = model.UserName,
                        Email = model.Email,
                        RegisterDate = DateTime.Now,
                        DisplayName = model.DisplayName,
                        RegisterIP = IPHelper.GetIPAddress(Request),
                        Website = model.Website,
                        Facebook = model.Facebook,
                        Twitter = model.Twitter,
                        Google = model.Google,
                        PhoneNumber = model.PhoneNumber,
                    };
                    var result = UserManager.Create(user, model.Password);
                    if (!result.Succeeded)
                    {
                        string erros = "";
                        foreach (var erro in result.Errors)
                        {
                            erros += "<br>" + erro;
                        }
                        return Json(new { isSuccess = false, mess = erros }, JsonRequestBehavior.AllowGet);
                    }
                    var objuser = UserManager.FindByName(user.UserName);
                    if (objuser != null && result.Succeeded)
                    {
                        var listgroup = new List<int?>();
                        foreach (var item in Request.Form.AllKeys.Where(p => p.StartsWith("group_")))
                        {
                            if (Request.Form.GetValues(item).Length > 0 && Request.Form.GetValues(item)[0] == "true")
                            {
                                var results = item.Replace("group_", "");
                                int roleid = 0;
                                if (int.TryParse(results, out roleid))
                                {
                                    AspNetUserGroup sitem = new AspNetUserGroup();
                                    param.AspNetUserRoles = new List<AspNetUserRole>();
                                    sitem.GroupId = roleid;
                                    sitem.UserId = user.Id;
                                    var biz = new AspNetUserGroupsBiz();
                                    var paramgroup = new AspNetUserGroupsParam();
                                    paramgroup.AspNetUserGroups = sitem;
                                    biz.Insert(paramgroup);
                                    listgroup.Add(roleid);
                                }
                            }
                        }
                        WriteLog("Tạo tài khoản thành công", Constant.LogType.Success);
                        return Json(new { isSuccess = true, mess = Constant.Addnewsuccess }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { isSuccess = false, mess = Constant.AcactFail }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog(ex);
                return Json(new { isSuccess = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Delete(int? id)
        {
            var param = new AspNetUsersParam { Id = id ?? 0, AspNetUsersFilter = new AspNetUsersFilter { Id = id ?? 0 } };
            aspnetUsersBiz.SetupEditForm(param);
            AspNetUserInfo item = param.AspNetUsersInfo;
            if (item == null)
            {
                ViewBag.Erro = Constant.NotItem;
                return View(new AspNetGroupsInfo());
            }
            else
                return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(AspNetUserInfo obj)
        {
            try
            {

                var param = new AspNetUsersParam() { AspNetUsersFilter = new AspNetUsersFilter { Id = obj.Id } };
                aspnetUsersBiz.SetupEditForm(param);
                var userobj = param.AspNetUsersInfo;
                if (userobj != null)
                {
                    if (userobj.ParentId.HasValue == false && userobj.SiteId > 0)
                    {
                        if (!string.IsNullOrEmpty(userobj.UrlDomain1))
                        {
                            try
                            {
                                RemoveIISWebsite("m." + userobj.UrlDomain1);
                                RemoveIISWebsite(userobj.UrlDomain1);
                                deletePool(userobj.UrlDomain1);
                            }
                            catch
                            {
                            }
                        }
                    }
                }

                var list = new List<AspNetUserInfo> { obj };
                var paramDelete = new AspNetUsersParam { AspNetUsersInfos = list };
                aspnetUsersBiz.DeleteUpdate(paramDelete);

                WriteLog("Xóa thông tin AspNetUser", Constant.LogType.Success);
                return Json(new { isSuccess = true, mess = Constant.Deletesuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                WriteLog("Xóa thông tin AspNetGroups", Constant.LogType.Success);
                return Json(new { isSuccess = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult LockUser(int Id)
        {
            try
            {
                var param = new AspNetUsersParam() { AspNetUsersFilter = new AspNetUsersFilter { Id = Id } };
                aspnetUsersBiz.SetupEditForm(param);
                var obj = param.AspNetUsersInfo;
                if (obj != null)
                {
                    var listrole = GetlistRole(obj.Id);
                    if (obj.ParentId.HasValue == false && obj.SiteId > 0 && listrole.Contains(22) == false)
                    {
                        if (!string.IsNullOrEmpty(obj.UrlDomain1))
                        {
                            try
                            {
                                RemoveIISWebsite("m." + obj.UrlDomain1);
                                RemoveIISWebsite(obj.UrlDomain1);
                                deletePool(obj.UrlDomain1);
                            }
                            catch
                            {
                            }
                        }
                    }
                    aspnetUsersBiz.Lock(Id, "");
                }
                else
                {
                    return Json(new
                    {
                        isSuccess = false,
                        message = "Không tìm thấy tài khoản trong hệ thống."
                    });
                }
                WriteLog("Khóa tài khoản thành công", Constant.LogType.Success);
                return Json(new
                {
                    isSuccess = true,
                    message = "Đ\x00e3 kh\x00f3a th\x00e0nh vi\x00ean!"
                });
            }
            catch (Exception exception)
            {
                WriteLog(exception, Constant.LogType.Error);
                return Json(new
                {
                    isSuccess = false,
                    message = "Lỗi xảy ra trong qu\x00e1 tr\x00ecnh thao t\x00e1c."
                });
            }
        }
        public JsonResult UnLockUser(int Id)
        {
            try
            {
                var check = "";
                var param = new AspNetUsersParam() { AspNetUsersFilter = new AspNetUsersFilter { Id = Id } };
                aspnetUsersBiz.SetupEditForm(param);
                var obj = param.AspNetUsersInfo;
                if (obj != null)
                {
                    var domain = "";
                    var listrole = GetlistRole(obj.Id);
                    if (obj.ParentId.HasValue == false && listrole.Contains(22) == false)
                    {

                        if (obj.SiteId > 0)
                        {
                            try
                            {


                                domain = obj.UserName.ToLower() + "." + Domainkey;
                                string poolname = addPool(domain);
                                AddBindings(domain, poolname);
                                AddBindingsMobile("m." + domain, poolname);
                                check = adddomain(domain, obj.UrlDomain2, obj.UrlDomain3);
                                sendmail(obj.Email, obj.DisplayName, obj.UserName);
                            }
                            catch (Exception ex)
                            {
                                return Json(new
                                {
                                    isSuccess = false,
                                    message = ex.Message
                                });
                            }
                        }
                        else
                        {
                            return Json(new
                            {
                                isSuccess = false,
                                message = "Tài khoản này chưa chọn site mẫu chưa thể kích hoạt."
                            });
                        }
                    }
                    aspnetUsersBiz.Lock(Id, domain);
                    WriteLog("Mở khóa tài khoản thành công", Constant.LogType.Success);
                    return Json(new
                    {
                        isSuccess = true,
                        message = check + "Đ\x00e3 mở kh\x00f3a th\x00e0nh vi\x00ean!"
                    });
                }
                else
                {
                    return Json(new
                    {
                        isSuccess = false,
                        message = "Không tìm thấy tài khoản trong hệ thống."
                    });
                }
            }
            catch (Exception exception)
            {
                WriteLog(exception, Constant.LogType.Error);
                return Json(new
                {
                    isSuccess = false,
                    message = "Lỗi xảy ra trong qu\x00e1 tr\x00ecnh thao t\x00e1c."
                });
            }
        }
        #region Sendmail
        public void sendmail(string mail, string hoten, string taikhoan)
        {
            try
            {
                var list = new List<MailAddress>();
                list.Add(new MailAddress(mail));
                var tieude = Domainkey + " kích hoạt tài khoản ";
                string urlfull = Request.Url.AbsoluteUri;
                string path = Request.Url.AbsolutePath;
                string url = urlfull.Replace(path, "") + "/System";
                string conten = string.Format(@"<p>Xin chào {0},</p>
<p></p>

<p>Tài khoản {1}<p>
<p></p>
<p>Truy cập vào link dưới đây để đăng nhập hệ thống. </p>
<p>{2}</p>
<p> </p>
", hoten, taikhoan, url);
                if (list.Count > 0)
                    MailUtily.sendmail(tieude, conten, list);
            }
            catch
            {

            }
        }
        #endregion
        #region IIS
        public bool IsWebsiteExists(string strWebsitename, ServerManager serverMgr)
        {
            Boolean flagset = false;
            SiteCollection sitecollection = serverMgr.Sites;
            foreach (Site site in sitecollection)
            {
                if (site.Name == strWebsitename.ToString())
                {
                    flagset = true;
                    break;
                }
                else
                {
                    flagset = false;
                }
            }
            return flagset;
        }
        public void AddBindings(string hostname, string poolname)
        {
            var path = System.Configuration.ConfigurationSettings.AppSettings.Get("PathPC");
            var VirtualDirectoriesUpLoad = System.Configuration.ConfigurationSettings.AppSettings.Get("VirtualDirectoriesUpLoad");
            ServerManager serverMgr = new ServerManager();
            string strWebsitename = hostname;
            string strApplicationPool = poolname;
            string strhostname = hostname;
            string stripaddress = "*";
            string bindinginfo = stripaddress + ":80:" + strhostname;
            Boolean bWebsite = IsWebsiteExists(strWebsitename, serverMgr);
            if (!bWebsite)
            {
                Site mySite = serverMgr.Sites.Add(strWebsitename.ToString(), "http", bindinginfo, path);
                mySite.ApplicationDefaults.ApplicationPoolName = strApplicationPool;
                mySite.TraceFailedRequestsLogging.Enabled = true;
                mySite.TraceFailedRequestsLogging.Directory = path;
                mySite.ServerAutoStart = true;
                Application rootApp = mySite.Applications.First(a => a.Path == "/");
                rootApp.VirtualDirectories.Add("/Upload", VirtualDirectoriesUpLoad);
                serverMgr.CommitChanges();
            }
        }
        private string Checkpooll(string hostname, ServerManager mgr)
        {
            ApplicationPoolCollection sitecollection = mgr.ApplicationPools;
            foreach (ApplicationPool site in sitecollection)
            {
                if (site.Name == hostname)
                {
                    return hostname;
                }
            }
            return "";
        }
        private string addPool(string hostname)
        {
            ServerManager mgr = new ServerManager();
            if (string.IsNullOrEmpty(Checkpooll(hostname, mgr)))
            {
                ApplicationPool newPool = mgr.ApplicationPools.Add(hostname);
                newPool.ManagedRuntimeVersion = "v4.0";
                newPool.Enable32BitAppOnWin64 = true;
                ManagedPipelineMode mode = ManagedPipelineMode.Integrated;
                newPool.ManagedPipelineMode = mode;
                mgr.CommitChanges();
                //ApplicationPool myAppPool = mgr.ApplicationPools.Add(hostname);
                //myAppPool.AutoStart = true;
                //myAppPool.ProcessModel.IdleTimeout = TimeSpan.FromMinutes(10);
                //myAppPool.Enable32BitAppOnWin64 = true;
                //myAppPool.ManagedPipelineMode = ManagedPipelineMode.Integrated;
                //myAppPool.ManagedRuntimeVersion = "V4.0";
                //myAppPool.QueueLength = 1000;
                //myAppPool.Cpu.Limit = 0;
                //myAppPool.Cpu.Action = ProcessorAction.NoAction;
                //myAppPool.Cpu.ResetInterval = TimeSpan.FromMinutes(5);
                //myAppPool.Cpu.SmpAffinitized = false;
                //myAppPool.Cpu.SmpProcessorAffinityMask = 4294967295;
                //myAppPool.ProcessModel.LoadUserProfile = false;
                //myAppPool.ProcessModel.MaxProcesses = 1;
                //myAppPool.ProcessModel.PingingEnabled = true;
                //myAppPool.ProcessModel.PingResponseTime = TimeSpan.FromSeconds(90);
                //myAppPool.ProcessModel.ShutdownTimeLimit = TimeSpan.FromSeconds(90);
                //myAppPool.ProcessModel.StartupTimeLimit = TimeSpan.FromSeconds(90);
                //myAppPool.ProcessModel.IdentityType = ProcessModelIdentityType.LocalSystem;
                //mgr.CommitChanges();
                //myAppPool.Start();
                //mgr.CommitChanges();
            }
            return hostname;
        }
        private void deletePool(string hostname)
        {
            ServerManager mgr = new ServerManager();
            if (!string.IsNullOrEmpty(Checkpooll(hostname, mgr)))
            {
                ApplicationPool appPool = mgr.ApplicationPools[hostname];
                if (appPool != null)
                {
                    ApplicationPoolCollection appColl = mgr.ApplicationPools;
                    appColl.Remove(appPool);
                    mgr.CommitChanges();
                }
            }
        }
        public void AddBindingsMobile(string hostname, string poolname)
        {
            var path = System.Configuration.ConfigurationSettings.AppSettings.Get("PathMB");
            var VirtualDirectoriesUpLoad = System.Configuration.ConfigurationSettings.AppSettings.Get("VirtualDirectoriesUpLoad");
            ServerManager serverMgr = new ServerManager();
            string strWebsitename = hostname;
            string strApplicationPool = poolname;
            string strhostname = hostname;
            string stripaddress = "*";
            string bindinginfo = stripaddress + ":80:" + strhostname;
            Boolean bWebsite = IsWebsiteExists(strWebsitename, serverMgr);
            if (!bWebsite)
            {
                Site mySite = serverMgr.Sites.Add(strWebsitename.ToString(), "http", bindinginfo, path);
                mySite.ApplicationDefaults.ApplicationPoolName = strApplicationPool;
                mySite.TraceFailedRequestsLogging.Enabled = true;
                mySite.TraceFailedRequestsLogging.Directory = path;
                mySite.ServerAutoStart = true;
                Application rootApp = mySite.Applications.First(a => a.Path == "/");
                rootApp.VirtualDirectories.Add("/Upload", VirtualDirectoriesUpLoad);
                serverMgr.CommitChanges();

            }
        }
        public void RemoveIISWebsite(string sitename)
        {
            ServerManager serverMgr = new ServerManager();
            Site site = serverMgr.Sites[sitename];
            bool blnSuccess = false;
            blnSuccess = IsWebsiteExists(sitename, serverMgr);
            if (blnSuccess)
            {
                serverMgr.Sites.Remove(site);
                serverMgr.CommitChanges();
            }
        }
        private string adddomain(string UrlDomain1, string domain2, string domain3)
        {
            string check = "";
            try
            {
                var changeiis = false;
                var domain = SiteMuti.Getsitename();
                domain2 = !string.IsNullOrEmpty(domain2) ? domain2.ToLower().Trim() : "";
                domain3 = !string.IsNullOrEmpty(domain3) ? domain3.ToLower().Trim() : "";
                ServerManager serverMgr = new ServerManager();
                if (domain.Trim().ToUpper() == Domainsys.Trim().ToUpper())
                {
                    check = "Check domain sys - ";
                    Site mySite = serverMgr.Sites[UrlDomain1];
                    Site mySitemobile = serverMgr.Sites["m." + UrlDomain1];
                    if (!string.IsNullOrEmpty(domain2))
                    {
                        check += "Check domain 2 rong - ";
                        try
                        {
                            var objhost = mySite.Bindings.FirstOrDefault(z => z.Host == domain2);
                            if (objhost != null)
                                mySite.Bindings.Remove(objhost);

                            var objhostmobile = mySitemobile.Bindings.FirstOrDefault(z => z.Host == ("m." + domain2));
                            if (objhostmobile != null)
                                mySitemobile.Bindings.Remove(objhostmobile);
                            check += "xoa domain 2  - ";
                            changeiis = true;
                        }
                        catch (Exception ex)
                        {
                            check += ex.Message;
                            throw new Exception(domain2 + ":" + ex.Message);
                            // ViewBag.erro += "- loi xoa domain 2" + ex.Message;
                        }
                    }
                    if (!string.IsNullOrEmpty(domain3))
                    {
                        check += "Check domain 3 rong - ";
                        try
                        {
                            var objhost = mySite.Bindings.FirstOrDefault(z => z.Host == domain3);
                            if (objhost != null)
                                mySite.Bindings.Remove(objhost);
                            var objhostmobile = mySitemobile.Bindings.FirstOrDefault(z => z.Host == ("m." + domain3));
                            if (objhostmobile != null)
                                mySitemobile.Bindings.Remove(objhostmobile);
                            check += "xoa domain 3 - ";
                            changeiis = true;
                        }
                        catch (Exception ex)
                        {
                            check += ex.Message;
                            throw new Exception(domain3 + ":" + ex.Message);
                            //ViewBag.erro += "- loi xoa domain 3" + ex.Message;
                        }
                    }
                    if (!string.IsNullOrEmpty(domain2))
                    {

                        check += "Check domain 2 rong add - ";
                        var objhost = mySite.Bindings.FirstOrDefault(z => z.Host == domain2);
                        if (objhost == null)
                        {
                            check += "add domain 2  - ";
                            mySite.Bindings.Add("*:80:" + domain2, "http");
                        }
                        var objhostmobile = mySitemobile.Bindings.FirstOrDefault(z => z.Host == ("m." + domain2));
                        if (objhostmobile == null)
                        {
                            check += "add domain 2 mb - ";
                            mySitemobile.Bindings.Add("*:80:" + "m." + domain2, "http");
                        }
                        changeiis = true;

                    }
                    if (!string.IsNullOrEmpty(domain3))
                    {
                        check += "Check domain 3 add - ";
                        var objhost = mySite.Bindings.FirstOrDefault(z => z.Host == domain3);
                        if (objhost == null)
                        {
                            check += "add domain 3 - ";
                            mySite.Bindings.Add("*:80:" + domain3, "http");
                        }
                        var objhostmobile = mySitemobile.Bindings.FirstOrDefault(z => z.Host == ("m." + domain3));
                        if (objhostmobile == null)
                        {
                            check += "add 3 mb - ";
                            mySitemobile.Bindings.Add("*:80:" + "m." + domain3, "http");
                        }
                        changeiis = true;
                    }
                }
                if (changeiis)
                {
                    ViewBag.erro += "-CommitChanges sv ";
                    serverMgr.CommitChanges();
                }
            }
            catch (Exception ex)
            {
                check += ex.Message;
                throw new Exception(ex.Message);
                //ViewBag.erro += ex.Message;
            }
            return check;
        }
        private bool checkdomain(string UrlDomain)
        {
            var aspnetuserparam = new AspNetUsersParam() { UrlDomain = UrlDomain };
            var aspnetuserbiz = new AspNetUsersBiz();
            return aspnetuserbiz.Checkdomain(aspnetuserparam);
        }
        #endregion
        #region Changepass 
        public ActionResult ResetUserPassword(string userId)
        {
            var param = new VDMMutiline.SharedComponent.Params.AspNetUsersParam() { AspNetUsersFilter = new VDMMutiline.SharedComponent.Params.AspNetUsersFilter() { Id = VDMMutiline.Ultilities.Utility.ConvertToInt(userId) ?? 0 } };
            aspnetUsersBiz.SetupEditForm(param);
            ResetPasswordViewModel model = new ResetPasswordViewModel();
            model.Username = param.AspNetUsersInfo.UserName.ToString();
            model.UserId = param.AspNetUsersInfo.Id;
            return View(model);
        }
        [HttpPost]
        public async Task<ActionResult> ResetUserPassword(ResetPasswordViewModel model)
        {
            try
            {
                if (UserManager.HasPassword(model.UserId))
                {
                    var code = await UserManager.GeneratePasswordResetTokenAsync(model.UserId);
                    var ps = await UserManager.ResetPasswordAsync(model.UserId, code, model.ConfirmPassword);
                    if (!ps.Succeeded)
                    {
                        string erro = "";
                        foreach (var item in ps.Errors)
                        {
                            erro += item;
                        }
                        WriteLog(erro, Constant.LogType.Error, Constant.LogLevel.System, CurrentUser.UserName);
                        return Json(new { isSuccess = false, mess = erro }, JsonRequestBehavior.AllowGet);
                    }
                }
                WriteLog("Reset Password thành công", Constant.LogType.Success);
                return Json(new { isSuccess = true, mess = Constant.Updatesuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                WriteLog(ex);
                return Json(new { isSuccess = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }
}