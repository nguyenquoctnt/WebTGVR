using System;
using System.Web;
using System.Web.Mvc;
using VDMMutiline.Core.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using VDMMutiline.Biz;
using VDMMutiline.SharedComponent.Params;
using static VDMMutiline.SharedComponent.Constants.Constant;
using VDMMutiline.SharedComponent.EntityInfo;
using VDMMutiline.SharedComponent.Entities;
using System.Collections.Generic;
using System.Linq;
using VDMMutiline.Core.Common;
namespace VDMMutiline.Core.UI
{
    public static class CommonUI
    {
        public static tbl_StyleSite GetstylebyDomain()
        {
            var obj = new tbl_StyleSite();
            var objuser = GetInforByDomain(SiteMuti.Getsitename());
            if (objuser != null)
            {
                var getliststyle = GetStyleSite(objuser.SiteId);
                var objstyle = getliststyle.FirstOrDefault(z => z.ID == objuser.StyleID);
                if (objstyle == null)
                    objstyle = getliststyle.FirstOrDefault(z => z.Isdefault == true);
                obj = objstyle;
            }
            return obj;
        }
        public static tbl_StyleSite GetstylebyDomain(AspNetUserInfo objuser)
        {
            var obj = new tbl_StyleSite();
            if (objuser != null)
            {
                var getliststyle = GetStyleSite(objuser.SiteId);
                var objstyle = getliststyle.FirstOrDefault(z => z.ID == objuser.StyleID);
                if (objstyle == null)
                    objstyle = getliststyle.FirstOrDefault(z => z.Isdefault == true);
                obj = objstyle;
            }
            return obj;
        }
        public static List<tbl_StyleSite> GetStyleSite(int? siteid)
        {
            var param = new SiteInfoParam { Id = siteid ?? 0 };
            var biz = new SiteInfoBiz();
            biz.GetStyleBySiteID(param);
            return param.StyleSites;
        }
        public static List<SettingUserInfo> GetSettingByUser(string UserName, bool AlwaysAgent)
        {
            var list = new List<SettingUserInfo>();
            var checklactv = GetlistCommon.CheckGroupUser(17, UserName);
            var param = new AspNetUsersParam();
            var biz = new AspNetUsersBiz();
            var objuser = biz.GetInfoByLoginName(UserName);
            if (objuser != null)
            {
                var iduser = objuser.Id;
                if (checklactv)
                {
                    if (AlwaysAgent && objuser.ParentId > 0)
                        iduser = objuser.ParentId ?? 0;
                }
                var bizseting = new SysSettingBiz();
                var paramsetting = new SettingUserParam() { Userid = iduser };
                bizseting.GetAllSettingUser(paramsetting);
                list = paramsetting.SettingUserInfos;
            }
            return list;
        }
        public static List<SettingUserInfo> GetSettingByDomain(string stringDomain, bool AlwaysAgent)
        {
            var list = new List<SettingUserInfo>();
            var param = new AspNetUsersParam() { UrlDomain = stringDomain };
            var biz = new AspNetUsersBiz();
            biz.GetInfobydomain(param);
            var objuserbydomain = param.AspNetUsersInfo;
            if (objuserbydomain != null)
            {
                var iduser = objuserbydomain.Id;
                if (AlwaysAgent && objuserbydomain.ParentId > 0)
                    iduser = objuserbydomain.ParentId ?? 0;
                var bizseting = new SysSettingBiz();
                var paramsetting = new SettingUserParam() { Userid = iduser };
                bizseting.GetAllSettingUser(paramsetting);
                list = paramsetting.SettingUserInfos;
            }
            return list;
        }
        public static List<SettingUserInfo> GetSettingByDomainAndUser(string stringDomain, VDMUser userInfo)
        {
            var list = new List<SettingUserInfo>();
            var param = new AspNetUsersParam() { UrlDomain = stringDomain };
            var biz = new AspNetUsersBiz();
            biz.GetInfobydomain(param);
            var objuserbydomain = param.AspNetUsersInfo;
            if (objuserbydomain != null)
            {
                var iduser = objuserbydomain.Id;
                if (userInfo != null)
                    if (userInfo.ParentId > 0)
                        iduser = userInfo.Id;
                var bizseting = new SysSettingBiz();
                var paramsetting = new SettingUserParam() { Userid = iduser };
                bizseting.GetAllSettingUser(paramsetting);
                list = paramsetting.SettingUserInfos;
            }
            return list;
        }
        public static AspNetUserInfo GetInforByDomain(string stringDomain)
        {
            var list = new List<SettingUserInfo>();
            var param = new AspNetUsersParam() { UrlDomain = stringDomain };
            var biz = new AspNetUsersBiz();
            biz.GetInfobydomain(param);
            var objuserbydomain = param.AspNetUsersInfo;
            return objuserbydomain != null ? objuserbydomain : new AspNetUserInfo();
        }
        public static AspNetUserInfo GetInforByUser(string username)
        {
            var biz = new AspNetUsersBiz();
            var obj = biz.GetInfoByLoginName(username);
            return obj != null ? obj : new AspNetUserInfo();
        }
        public static List<AspNetUserInfo> GetparentUserDaiLybyDomain()
        {
            var objuser = GetInforByDomain(SiteMuti.Getsitename());
            if (objuser != null)
            {
                var parentid = objuser.Id;
                var param = new AspNetUsersParam() { AspNetUsersFilter = new AspNetUsersFilter { Id = parentid } };
                var biz = new AspNetUsersBiz();
                biz.SearchListParam(param);
                return param.AspNetUsersInfos;
            }
            else return new List<AspNetUserInfo>();
        }
        public static List<VDMMutiline.SharedComponent.EntityInfo.TempleteSetting.TempleatePropertyUserInfo> GetlistTempleate(int? idgroup)
        {
            var objdomain = GetInforByDomain(SiteMuti.Getsitename());
            if (objdomain != null)
            {
                var param = new VDMMutiline.SharedComponent.Params.TempleteSetting.TempleatePropertyUserParam()
                {
                    TempleatePropertyUserFilter = new SharedComponent.Params.TempleteSetting.TempleatePropertyUserFilter
                    {
                        UserId = objdomain.Id,
                        GroupId = idgroup
                    },
                };
                var biz = new Biz.TempleteSetting.TempleatePropertyUserUserBiz();
                biz.SearchByUser(param);
                return param.TempleatePropertyUserInfos;
            }
            return new List<SharedComponent.EntityInfo.TempleteSetting.TempleatePropertyUserInfo>();
        }
        public static List<VDMMutiline.SharedComponent.EntityInfo.TempleteSetting.TempleatePropertyUserInfo> GetlistTempleate(int? idgroup, int? userID)
        {
            var param = new VDMMutiline.SharedComponent.Params.TempleteSetting.TempleatePropertyUserParam()
            {
                TempleatePropertyUserFilter = new SharedComponent.Params.TempleteSetting.TempleatePropertyUserFilter
                {
                    UserId = userID,
                    GroupId = idgroup
                },
            };
            var biz = new Biz.TempleteSetting.TempleatePropertyUserUserBiz();
            biz.SearchByUser(param);
            return param.TempleatePropertyUserInfos;
        }
        public static string Getsettingtempleatebykey(List<VDMMutiline.SharedComponent.EntityInfo.TempleteSetting.TempleatePropertyUserInfo> lst, string key)
        {
            var obj = lst.FirstOrDefault(z => z.key == key);
            if (obj != null)
            {
                return obj.Valued == null ? "" : obj.Valued;
            }
            return "";
        }
        public static string Customcss()
        {
            string template = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("~/TeampleateVe/cssmainTempleate.css"));
            var lstsettingtmpleate1 = GetlistTempleate(5);
            var lstsettingtmpleate2 = GetlistTempleate(6);
            var lstsettingtmpleate3 = GetlistTempleate(16);
            foreach (var item in lstsettingtmpleate1)
            {
                template = template.Replace(item.key, item.Valued);
            }
            foreach (var item in lstsettingtmpleate2)
            {
                template = template.Replace(item.key, item.Valued);
            }
            foreach (var item in lstsettingtmpleate3)
            {
                template = template.Replace(item.key, item.Valued);
            }
            return template;
        }
        public static AspNetUserInfo GetInforByUserOrUserParent(int? userID)
        {
            var biz = new AspNetUsersBiz();
            var objuserbydomain = biz.GetInfoById(userID);
            if (objuserbydomain != null)
            {
                if (objuserbydomain.ParentId > 0)
                {
                    return biz.GetInfoById(objuserbydomain.ParentId);
                }
                else
                    return objuserbydomain;
            }
            return null;
        }
        public static AspNetUserInfo GetInforByUserOrUserParent(string username)
        {
            var biz = new AspNetUsersBiz();
            var objuserbydomain = biz.GetInfoByLoginName(username);
            if (objuserbydomain != null)
            {
                if (objuserbydomain.ParentId > 0)
                {
                    return biz.GetInfoById(objuserbydomain.ParentId);
                }
                else
                    return objuserbydomain;
            }
            return null;
        }
    }
    public class PublishController : Controller
    {

        public static string Domainkey = System.Configuration.ConfigurationSettings.AppSettings.Get("Domainkey");
        public static string Domainwwwkey = System.Configuration.ConfigurationSettings.AppSettings.Get("Domainwwwkey");
        public static string Domainsys = System.Configuration.ConfigurationSettings.AppSettings.Get("Domainsys");
        EventLogBiz eventLog = new EventLogBiz();
        NotifyBiz notifySend = new NotifyBiz();
        /// <summary>
        /// Initializes a new instance of the <see cref="PublishController"/> class.
        /// </summary>
        public PublishController()
        {
            System.Reflection.AssemblyName curentAssembly = typeof(PublishController).Assembly.GetName();
            ViewBag.ProductVertion = string.Format("{0}.{1}.{2}", curentAssembly.Version.Major, curentAssembly.Version.Minor, curentAssembly.Version.Build);
        }
        /// <summary>
        /// Gets the current user.
        /// </summary>
        /// <value>
        /// The current user.
        /// </value>
        public VDMUser CurrentUser
        {
            get
            {
                var act = Session["CurrentUser"] != null ?
                    (VDMUser)Session["CurrentUser"] :
                    HttpContext.GetOwinContext().GetUserManager<VDMUserManager>().FindByName(HttpContext.User.Identity.Name);
                if (Session["CurrentUser"] == null)
                {
                    Session.Add("CurrentUser", act);
                }
                return act;
            }
            set
            {
                if (Session["CurrentUser"] != null)
                {
                    Session["CurrentUser"] = value;
                }
                else
                {
                    Session.Add("CurrentUser", value);
                }
            }
        }
        public static string GetDettingSystemKey(string key)
        {
            var param = new SettingSystemParam() { Key = key };
            var biz = new SysSettingBiz();
            biz.GetInfoBykey(param);
            if (param.SettingSystemInfo != null)
                return param.SettingSystemInfo.Giatri;
            return "";
        }
        public List<int> GetlistRole()
        {

            var groupbyuserparam = new AspNetUserGroupsParam() { userId = CurrentUser.Id };
            var bizgroup = new AspNetUserGroupsBiz();
            bizgroup.GetAllByusername(groupbyuserparam);
            var listidquyen = groupbyuserparam.AspNetUserGroupsInfos.Select(z => z.GroupId).ToList();
            return listidquyen;
        }
        public List<int> GetlistRole(int userID)
        {

            var groupbyuserparam = new AspNetUserGroupsParam() { userId = userID };
            var bizgroup = new AspNetUserGroupsBiz();
            bizgroup.GetAllByusername(groupbyuserparam);
            var listidquyen = groupbyuserparam.AspNetUserGroupsInfos.Select(z => z.GroupId).ToList();
            return listidquyen;
        }
        public List<AspNetUserInfo> GetparentUserDaiLybyDomain()
        {
            return CommonUI.GetparentUserDaiLybyDomain();
        }
        public List<AspNetUserInfo> GetparentUserDaiLy()
        {
            var parentid = CurrentUser.Id;
            if (GetlistRole().Contains(22))
            {
                parentid = CurrentUser.ParentId ?? 0;
            }
            var param = new AspNetUsersParam() { AspNetUsersFilter = new AspNetUsersFilter { Id = parentid } };
            var biz = new AspNetUsersBiz();
            biz.SearchListParam(param);
            return param.AspNetUsersInfos;
        }
        public List<AspNetUserInfo> GetparentUserDaiLy(int userid)
        {
            var parentid = userid;
            var param = new AspNetUsersParam() { AspNetUsersFilter = new AspNetUsersFilter { Id = parentid } };
            var biz = new AspNetUsersBiz();
            biz.SearchListParam(param);
            return param.AspNetUsersInfos;
        }
        public List<AspNetUserInfo> GetparentUserCTV()
        {
            var parentid = CurrentUser.Id;
            var param = new AspNetUsersParam() { AspNetUsersFilter = new AspNetUsersFilter { Id = parentid } };
            var biz = new AspNetUsersBiz();
            biz.SearchListParam(param);
            return param.AspNetUsersInfos;
        }
        public List<SettingUserInfo> GetSettingByUser(string UserName, bool AlwaysAgent)
        {
            return CommonUI.GetSettingByUser(UserName, AlwaysAgent);
        }
        public AspNetUserInfo GetInforByDomain(string domain)
        {
            return CommonUI.GetInforByDomain(domain);
        }
        public List<SettingUserInfo> GetSettingByDomain(string stringDomain, bool AlwaysAgent)
        {
            return CommonUI.GetSettingByDomain(stringDomain, AlwaysAgent);
        }
        public List<SettingUserInfo> GetSettingByDomainAndUser(string stringDomain)
        {
            return CommonUI.GetSettingByDomainAndUser(stringDomain, CurrentUser);
        }
        public AspNetUserInfo GetInforByUserOrUserParent(string username)
        {
            return CommonUI.GetInforByUserOrUserParent(username);
        }
        public bool IsAdmin()
        {
            var getvalue = GetlistRole().Where(z => z == 1);
            return getvalue.Count() > 0 ? true : false;
        }
        public bool IsAgency()
        {
            var getvalue = GetlistRole().Where(z => z == 18);
            return getvalue.Count() > 0 ? true : false;
        }
        #region Logs
        public void WriteLog(string message, LogType log, LogLevel lvel, string username)
        {
            var paraeventLog = new EventLogParam() { EventLogFilter = new EventLogFilter() { } };
            paraeventLog.EventLog = new SharedComponent.Entities.EventLog
            {
                Agent = Request.UserAgent,
                IPXForward = IPHelper.GetIPAddress(Request),
                IPDangnhap = Request.UserHostAddress,
                Nguoitao = username,
                Ngaytao = DateTime.Now,
                Ghichu = message,
                LogLevel = (int)lvel,
                LogType = (int)log
            };
            if ((Request.UserAgent != null) && Request.UserAgent.ToLower().Contains("iphone"))
            {
                paraeventLog.EventLog.Thietbi = "Iphone";
            }
            else if ((Request.UserAgent != null) && Request.UserAgent.ToLower().Contains("android"))
            {
                paraeventLog.EventLog.Thietbi = "Android";
            }
            else
            {
                paraeventLog.EventLog.Thietbi = "PC";
            }
            eventLog.Insert(paraeventLog);
        }
        /// <summary>
        /// Writes the log in.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="log">The log.</param>
        /// <param name="username">The username.</param>
        public void WriteLogIn(string message, LogType log, string username)
        {
            WriteLog(message, log, LogLevel.Login, username);
        }
        /// <summary>
        /// Writes the log.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="log">The log.</param>
        public void WriteLog(string message, LogType log)
        {
            WriteLog(message, log, LogLevel.Action, CurrentUser.UserName);
        }
        /// <summary>
        /// Writes the log.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="log">The log.</param>
        public void WriteLog(Exception message, LogType log)
        {
            WriteLog(message.Message, log);
        }
        /// <summary>
        /// Writes the log.
        /// </summary>
        /// <param name="message">The message.</param>
        public void WriteLog(Exception message)
        {
            WriteLog(message.Message, LogType.Error);
        }

        public void Notification(string subject, string content, MessageType msgType)
        {
            try
            {
                var notip = new NotifyParam() { NotifyFilter = new NotifyFilter() };
                notip.NotificationInfo = new SharedComponent.EntityInfo.NotificationInfo()
                {
                    Body = content,
                    Type = (int)msgType,
                    Daxoa = false,
                    ReadStatus = false,
                    SenderID = CurrentUser.Id,
                    SentDate = DateTime.Now,
                    SenderName = CurrentUser.DisplayName,
                    IsSendAll = true,
                    Subject = subject
                };
                notifySend.Insert(notip);
                WriteLog(content, LogType.Success);
            }
            catch (Exception ex)
            {
                WriteLog(ex);
            }
        }
        public void Notification(string content, MessageType msgType)
        {
            Notification("", content, msgType);
        }

        public string Getsettingkey(string key)
        {
            var param = new SettingSystemParam() { Key = key };
            var biz = new SysSettingBiz();
            //  biz.GetInfoBykey(param);
            if (param.SettingSystemInfo != null)
                return param.SettingSystemInfo.Giatri;
            return "";
        }

        //public static List<SettingValue> GetSeting()
        //{
        //    var dao = new SettingDao();
        //    if (HttpContext.Current.Session["SessionUserinfo"] != null)
        //    {
        //        var objuser = (UserInfo)HttpContext.Current.Session["SessionUserinfo"];
        //        if (objuser != null)
        //        {
        //            if (objuser.GroupId == 3 || objuser.GroupId == 2 || objuser.GroupId == 4 || objuser.GroupId == 1)
        //                return dao.GetSetingUsingUser(objuser.Id);
        //            else
        //                return dao.GetSetingUsingUser(0);
        //        }
        //        else
        //        {
        //            return dao.GetSetingUsingUser(0);
        //        }
        //    }
        //    else
        //    {
        //        return dao.GetSetingUsingUser(0);
        //    }
        //}
        #endregion
    }
}
