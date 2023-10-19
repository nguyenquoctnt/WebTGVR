using System.Collections.Generic;
using System.Linq;
using VDMMutiline.SharedComponent.Constants;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
using VDMMutiline.SharedComponent.Params;

namespace VDMMutiline.Dao
{
    public class AspNetUserDao : BaseDao
    {
        #region Action
        public int Insert(AspNetUser item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                dbContext.AspNetUsers.InsertOnSubmit(item);
                dbContext.SubmitChanges();
                return item.Id;
            }
        }
        public void Update(AspNetUserInfo item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.AspNetUsers.FirstOrDefault(sitem => sitem.Id == item.Id);
                if (dbItem != null)
                {

                    dbItem.Id = item.Id;
                    dbItem.Birthday = item.Birthday;
                    dbItem.Email = item.Email;
                    dbItem.PhoneNumber = item.PhoneNumber;
                    dbItem.TwoFactorEnabled = item.TwoFactorEnabled;
                    dbItem.PhoneNumber2 = item.PhoneNumber2;
                    dbItem.DisplayName = item.DisplayName;
                    dbItem.Location = item.Location;
                    dbItem.Avatar = item.Avatar;
                    dbItem.Website = item.Website;
                    dbItem.Gioitinh = item.Gioitinh;
                    dbItem.IsIssueTicket = item.IsIssueTicket;
                    dbContext.SubmitChanges();
                }
            }
        }
        public void UpdateSiteID(AspNetUserInfo item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.AspNetUsers.FirstOrDefault(sitem => sitem.Id == item.Id);
                if (dbItem != null)
                {

                    dbItem.Id = item.Id;
                    dbItem.SiteId = item.SiteId;
                    dbContext.SubmitChanges();
                }
            }
        }
        public void UpdateEditdomain(AspNetUserInfo itemparra)
        {
            var item = itemparra;
            using (var db = new ContextDataContext(ConnectionString))
            {
                var dbItem = db.AspNetUsers.FirstOrDefault(sitem => sitem.Id == item.Id);
                if (dbItem != null)
                {
                    dbItem.PhoneNumber = item.PhoneNumber;
                    dbItem.PhoneNumber2 = item.PhoneNumber2;
                    dbItem.Avatar = item.Avatar;
                    dbItem.LogoUrl = item.LogoUrl;
                    dbItem.LogoMbUrl = item.LogoMbUrl;
                    dbItem.Baner = item.Baner;
                    dbItem.Gmap = item.Gmap;
                    dbItem.Facebook = item.Facebook;
                    if (item.StyleID.HasValue)
                        dbItem.StyleID = item.StyleID;
                    if (dbItem.UrlDomain2 != item.UrlDomain2)
                        dbItem.UrlDomain2 = item.UrlDomain2.Trim().ToLower();
                    if (dbItem.UrlDomain3 != item.UrlDomain3)
                        dbItem.UrlDomain3 = item.UrlDomain3.Trim().ToLower();
                    dbItem.IsSSLDomain2 = item.IsSSLDomain2;
                    dbItem.IsSSLDomain3 = item.IsSSLDomain3;
                    dbItem.IsSSLDomain2MB = item.IsSSLDomain2MB;
                    dbItem.IsSSLDomain3MB = item.IsSSLDomain3MB;
                    dbItem.SeoDes = item.SeoDes;
                    dbItem.SeoKey = item.SeoKey;
                    dbItem.SeoTitle = item.SeoTitle;
                    db.SubmitChanges();
                }

            }
        }
        public AspNetUser CheckCode(string username, string code)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.AspNetUsers.FirstOrDefault(sitem => sitem.UserName == username && sitem.Code == code);
                if (dbItem != null)
                {
                    dbItem.IsActive = true;
                    dbContext.SubmitChanges();
                }
                return dbItem;
            }
        }
        public void Lock(int byID, string domain)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.AspNetUsers.FirstOrDefault(en => en.Id == byID);
                if (dbItem != null)
                {
                    dbItem.LockoutEnabled = !dbItem.LockoutEnabled;
                    if (!dbItem.LockoutEnabled)
                    {
                        if (dbItem.ParentId.HasValue)
                        {
                            //var objparent = dbContext.AspNetUsers.FirstOrDefault(z => z.Id == dbItem.ParentId);
                            //if (objparent != null)
                            //{

                            //    dbItem.UrlDomain1 = objparent.UrlDomain1;
                            //    dbItem.UrlDomain2 = objparent.UrlDomain2;
                            //    dbItem.UrlDomain3 = objparent.UrlDomain3;

                            //}
                        }
                        else
                        {
                            dbItem.UrlDomain1 = domain;
                        }
                    }

                    dbContext.SubmitChanges();
                }
            }
        }
        public bool Delete(AspNetUser item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.AspNetUsers.FirstOrDefault(en => en.Id == item.Id);

                if (dbItem != null)
                {
                    dbContext.AspNetUsers.DeleteOnSubmit(dbItem);
                    dbContext.SubmitChanges();
                }
            }
            return true;
        }
        public bool DeleteUpdate(AspNetUserInfo item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.AspNetUsers.FirstOrDefault(en => en.Id == item.Id);
                if (dbItem != null)
                {
                    dbItem.LockoutEnabled = false;
                    dbItem.Isdelete = true;
                    dbItem.UrlDomain2 = "";
                    dbItem.UrlDomain3 = "";
                    dbContext.SubmitChanges();
                }
            }
            return true;
        }
        #endregion
        #region Query

        public AspNetUser GetbyId(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.AspNetUsers
                            where n.Id == id
                            select n;
                return query.FirstOrDefault();
            }
        }
        public AspNetUserInfo GetInfobydomain(string UrlDomain)
        {
            using (var db = new ContextDataContext(ConnectionString))
            {
                var query = from n in db.AspNetUsers
                            where (n.UrlDomain1 == UrlDomain || n.UrlDomain2 == UrlDomain || n.UrlDomain3 == UrlDomain)
                            && n.ParentId.HasValue == false
                            select new AspNetUserInfo
                            {
                                Id = n.Id,
                                Birthday = n.Birthday,
                                Email = n.Email,
                                PhoneNumber = n.PhoneNumber,
                                SiteId = n.SiteId,
                                EmailConfirmed = n.EmailConfirmed,
                                PasswordHash = n.PasswordHash,
                                SecurityStamp = n.SecurityStamp,
                                PhoneNumberConfirmed = n.PhoneNumberConfirmed,
                                TwoFactorEnabled = n.TwoFactorEnabled,
                                LockoutEndDateUtc = n.LockoutEndDateUtc,
                                LockoutEnabled = n.LockoutEnabled,
                                AccessFailedCount = n.AccessFailedCount,
                                UserName = n.UserName,
                                PhoneNumber2 = n.PhoneNumber2,
                                RegisterIP = n.RegisterIP,
                                DisplayName = n.DisplayName,
                                Location = n.Location,
                                RegisterDate = n.RegisterDate,
                                Status = n.Status,
                                Avatar = n.Avatar,
                                LastLogin = n.LastLogin,
                                Website = n.Website,
                                Facebook = n.Facebook,
                                Twitter = n.Twitter,
                                Google = n.Google,
                                Gioitinh = n.Gioitinh,
                                UrlDomain1 = n.UrlDomain1,
                                UrlDomain2 = n.UrlDomain2,
                                UrlDomain3 = n.UrlDomain3,
                                Isdelete = n.Isdelete,
                                ParentId = n.ParentId,
                                Gmap = n.Gmap,
                                LogoUrl = n.LogoUrl,
                                Baner = n.Baner,
                                StyleID = n.StyleID,
                                IsSSLDomain2 = n.IsSSLDomain2,
                                IsSSLDomain3 = n.IsSSLDomain3,
                                IsSSLDomain2MB = n.IsSSLDomain2MB,
                                IsSSLDomain3MB = n.IsSSLDomain3MB,
                                SeoDes = n.SeoDes,
                                SeoKey = n.SeoKey,
                                SeoTitle = n.SeoTitle,
                                IsIssueTicket=n.IsIssueTicket,
                                LogoMbUrl=n.LogoMbUrl,
                            };
                return query.FirstOrDefault();
            }
        }
        public AspNetUserInfo GetInfoByLoginName(string username)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.AspNetUsers
                            where n.UserName == username
                            && n.Isdelete == Constant.IsNotDeleted
                            select new AspNetUserInfo()
                            {
                                Id = n.Id,
                                Birthday = n.Birthday,
                                Email = n.Email,
                                PhoneNumber = n.PhoneNumber,
                                SiteId = n.SiteId,
                                EmailConfirmed = n.EmailConfirmed,
                                PasswordHash = n.PasswordHash,
                                SecurityStamp = n.SecurityStamp,
                                PhoneNumberConfirmed = n.PhoneNumberConfirmed,
                                TwoFactorEnabled = n.TwoFactorEnabled,
                                LockoutEndDateUtc = n.LockoutEndDateUtc,
                                LockoutEnabled = n.LockoutEnabled,
                                AccessFailedCount = n.AccessFailedCount,
                                UserName = n.UserName,
                                PhoneNumber2 = n.PhoneNumber2,
                                RegisterIP = n.RegisterIP,
                                DisplayName = n.DisplayName,
                                Location = n.Location,
                                RegisterDate = n.RegisterDate,
                                Status = n.Status,
                                Avatar = n.Avatar,
                                LastLogin = n.LastLogin,
                                Website = n.Website,
                                Facebook = n.Facebook,
                                Twitter = n.Twitter,
                                Google = n.Google,
                                Gioitinh = n.Gioitinh,
                                UrlDomain1 = n.UrlDomain1,
                                UrlDomain2 = n.UrlDomain2,
                                UrlDomain3 = n.UrlDomain3,
                                Isdelete = n.Isdelete,
                                ParentId = n.ParentId,
                                Gmap = n.Gmap,
                                LogoUrl = n.LogoUrl,
                                Baner = n.Baner,
                                StyleID = n.StyleID,
                                IsSSLDomain2 = n.IsSSLDomain2,
                                IsSSLDomain3 = n.IsSSLDomain3,
                                IsSSLDomain2MB = n.IsSSLDomain2MB,
                                IsSSLDomain3MB = n.IsSSLDomain3MB,
                                SeoDes = n.SeoDes,
                                SeoKey = n.SeoKey,
                                SeoTitle = n.SeoTitle,
                                IsIssueTicket = n.IsIssueTicket,
                                LogoMbUrl = n.LogoMbUrl,
                                // RoleGrouplist = from r in dbContext.
                            };
                return query.FirstOrDefault();
            }
        }
        public AspNetUserInfo GetInfoById(int? Iduser)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.AspNetUsers
                            where n.Id == Iduser
                            && n.Isdelete == Constant.IsNotDeleted
                            select new AspNetUserInfo()
                            {
                                Id = n.Id,
                                Birthday = n.Birthday,
                                Email = n.Email,
                                PhoneNumber = n.PhoneNumber,
                                SiteId = n.SiteId,
                                EmailConfirmed = n.EmailConfirmed,
                                PasswordHash = n.PasswordHash,
                                SecurityStamp = n.SecurityStamp,
                                PhoneNumberConfirmed = n.PhoneNumberConfirmed,
                                TwoFactorEnabled = n.TwoFactorEnabled,
                                LockoutEndDateUtc = n.LockoutEndDateUtc,
                                LockoutEnabled = n.LockoutEnabled,
                                AccessFailedCount = n.AccessFailedCount,
                                UserName = n.UserName,
                                PhoneNumber2 = n.PhoneNumber2,
                                RegisterIP = n.RegisterIP,
                                DisplayName = n.DisplayName,
                                Location = n.Location,
                                RegisterDate = n.RegisterDate,
                                Status = n.Status,
                                Avatar = n.Avatar,
                                LastLogin = n.LastLogin,
                                Website = n.Website,
                                Facebook = n.Facebook,
                                Twitter = n.Twitter,
                                Google = n.Google,
                                Gioitinh = n.Gioitinh,
                                UrlDomain1 = n.UrlDomain1,
                                UrlDomain2 = n.UrlDomain2,
                                UrlDomain3 = n.UrlDomain3,
                                Isdelete = n.Isdelete,
                                ParentId = n.ParentId,
                                Gmap = n.Gmap,
                                LogoUrl = n.LogoUrl,
                                Baner = n.Baner,
                                StyleID = n.StyleID,
                                IsSSLDomain2 = n.IsSSLDomain2,
                                IsSSLDomain3 = n.IsSSLDomain3,
                                IsSSLDomain2MB = n.IsSSLDomain2MB,
                                IsSSLDomain3MB = n.IsSSLDomain3MB,
                                SeoDes = n.SeoDes,
                                SeoKey = n.SeoKey,
                                SeoTitle = n.SeoTitle,
                                IsIssueTicket = n.IsIssueTicket,
                                LogoMbUrl = n.LogoMbUrl,
                                // RoleGrouplist = from r in dbContext.
                            };
                return query.FirstOrDefault();
            }
        }
        public AspNetUserInfo GetInfoByEmail(string Email)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.AspNetUsers
                            where n.Email == Email.Trim()
                            && n.Isdelete == Constant.IsNotDeleted
                            select new AspNetUserInfo()
                            {
                                Id = n.Id,
                                Birthday = n.Birthday,
                                Email = n.Email,
                                PhoneNumber = n.PhoneNumber,
                                SiteId = n.SiteId,
                                EmailConfirmed = n.EmailConfirmed,
                                PasswordHash = n.PasswordHash,
                                SecurityStamp = n.SecurityStamp,
                                PhoneNumberConfirmed = n.PhoneNumberConfirmed,
                                TwoFactorEnabled = n.TwoFactorEnabled,
                                LockoutEndDateUtc = n.LockoutEndDateUtc,
                                LockoutEnabled = n.LockoutEnabled,
                                AccessFailedCount = n.AccessFailedCount,
                                UserName = n.UserName,
                                PhoneNumber2 = n.PhoneNumber2,
                                RegisterIP = n.RegisterIP,
                                DisplayName = n.DisplayName,
                                Location = n.Location,
                                RegisterDate = n.RegisterDate,
                                Status = n.Status,
                                Avatar = n.Avatar,
                                LastLogin = n.LastLogin,
                                Website = n.Website,
                                Facebook = n.Facebook,
                                Twitter = n.Twitter,
                                Google = n.Google,
                                Gioitinh = n.Gioitinh,
                                UrlDomain1 = n.UrlDomain1,
                                UrlDomain2 = n.UrlDomain2,
                                UrlDomain3 = n.UrlDomain3,
                                Isdelete = n.Isdelete,
                                ParentId = n.ParentId,
                                Gmap = n.Gmap,
                                LogoUrl = n.LogoUrl,
                                Baner = n.Baner,
                                StyleID = n.StyleID,
                                IsSSLDomain2 = n.IsSSLDomain2,
                                IsSSLDomain3 = n.IsSSLDomain3,
                                IsSSLDomain2MB = n.IsSSLDomain2MB,
                                IsSSLDomain3MB = n.IsSSLDomain3MB,
                                SeoDes = n.SeoDes,
                                SeoKey = n.SeoKey,
                                SeoTitle = n.SeoTitle,
                                IsIssueTicket = n.IsIssueTicket,
                                LogoMbUrl = n.LogoMbUrl,
                                // RoleGrouplist = from r in dbContext.
                            };
                return query.FirstOrDefault();
            }
        }
        public AspNetUser Checkdomain(string domain)
        {
            using (var db = new ContextDataContext(ConnectionString))
            {
                var dbItem = db.AspNetUsers.FirstOrDefault(sitem => sitem.UrlDomain1 == domain.Trim().ToLower() || sitem.UrlDomain2 == domain.Trim().ToLower() || sitem.UrlDomain3 == domain.Trim().ToLower());
                return dbItem;
            }
        }
        public AspNetUserInfo GetInfo(int? id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.AspNetUsers
                            where n.Id == id && n.Isdelete == Constant.IsNotDeleted
                            select new AspNetUserInfo()
                            {
                                Id = n.Id,
                                Birthday = n.Birthday,
                                Email = n.Email,
                                PhoneNumber = n.PhoneNumber,
                                SiteId = n.SiteId,
                                EmailConfirmed = n.EmailConfirmed,
                                PasswordHash = n.PasswordHash,
                                SecurityStamp = n.SecurityStamp,
                                PhoneNumberConfirmed = n.PhoneNumberConfirmed,
                                TwoFactorEnabled = n.TwoFactorEnabled,
                                LockoutEndDateUtc = n.LockoutEndDateUtc,
                                LockoutEnabled = n.LockoutEnabled,
                                AccessFailedCount = n.AccessFailedCount,
                                UserName = n.UserName,
                                PhoneNumber2 = n.PhoneNumber2,
                                RegisterIP = n.RegisterIP,
                                DisplayName = n.DisplayName,
                                Location = n.Location,
                                RegisterDate = n.RegisterDate,
                                Status = n.Status,
                                Avatar = n.Avatar,
                                LastLogin = n.LastLogin,
                                Website = n.Website,
                                Facebook = n.Facebook,
                                Twitter = n.Twitter,
                                Google = n.Google,
                                Gioitinh = n.Gioitinh,
                                UrlDomain1 = n.UrlDomain1,
                                UrlDomain2 = n.UrlDomain2,
                                UrlDomain3 = n.UrlDomain3,
                                Isdelete = n.Isdelete,
                                ParentId = n.ParentId,
                                Gmap = n.Gmap,
                                LogoUrl = n.LogoUrl,
                                Baner = n.Baner,
                                StyleID = n.StyleID,
                                IsSSLDomain2 = n.IsSSLDomain2,
                                IsSSLDomain3 = n.IsSSLDomain3,
                                IsSSLDomain2MB = n.IsSSLDomain2MB,
                                IsSSLDomain3MB = n.IsSSLDomain3MB,
                                SeoDes = n.SeoDes,
                                SeoKey = n.SeoKey,
                                SeoTitle = n.SeoTitle,
                                IsIssueTicket=n.IsIssueTicket,
                                LogoMbUrl = n.LogoMbUrl,
                                // RoleGrouplist = from r in dbContext.
                            };
                return query.FirstOrDefault();
            }
        }
        public void Search(AspNetUsersParam param)
        {
            var filter = param.AspNetUsersFilter;
            var listcontennotshow = System.Configuration.ConfigurationSettings.AppSettings.Get("Listcontenotshow").Split(';').ToList();
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.AspNetUsers
                            where (filter.Id.HasValue == false || n.Id == filter.Id) 
                            && (string.IsNullOrEmpty(filter.Search) || (n.UserName.Contains(filter.Search) || n.DisplayName.Contains(filter.Search) || n.Email.Contains(filter.Search) || n.PhoneNumber.Contains(filter.Search) || n.UrlDomain2.Contains(filter.Search)))
                             && n.Isdelete == Constant.IsNotDeleted
                             && (filter.UserList.Count == 0 || filter.UserList.Contains(n.UserName))
                             && (filter.ParentId.HasValue == false || (n.ParentId == filter.ParentId || n.Id == filter.ParentId))
                             && (listcontennotshow.Contains(n.UserName) == false)
                            select new AspNetUserInfo
                            {
                                Id = n.Id,
                                Birthday = n.Birthday,
                                Email = n.Email,
                                PhoneNumber = n.PhoneNumber,
                                SiteId = n.SiteId,
                                EmailConfirmed = n.EmailConfirmed,
                                PasswordHash = n.PasswordHash,
                                SecurityStamp = n.SecurityStamp,
                                PhoneNumberConfirmed = n.PhoneNumberConfirmed,
                                TwoFactorEnabled = n.TwoFactorEnabled,
                                LockoutEndDateUtc = n.LockoutEndDateUtc,
                                LockoutEnabled = n.LockoutEnabled,
                                AccessFailedCount = n.AccessFailedCount,
                                UserName = n.UserName,
                                PhoneNumber2 = n.PhoneNumber2,
                                RegisterIP = n.RegisterIP,
                                DisplayName = n.DisplayName,
                                Location = n.Location,
                                RegisterDate = n.RegisterDate,
                                Status = n.Status,
                                Avatar = n.Avatar,
                                LastLogin = n.LastLogin,
                                Website = n.Website,
                                Facebook = n.Facebook,
                                Twitter = n.Twitter,
                                Google = n.Google,
                                Gioitinh = n.Gioitinh,
                                UrlDomain1 = n.UrlDomain1,
                                UrlDomain2 = n.UrlDomain2,
                                UrlDomain3 = n.UrlDomain3,
                                Isdelete = n.Isdelete,
                                ParentId = n.ParentId,
                                Gmap = n.Gmap,
                                LogoUrl = n.LogoUrl,
                                Baner = n.Baner,
                                StyleID = n.StyleID,
                                IsSSLDomain2 = n.IsSSLDomain2,
                                IsSSLDomain3 = n.IsSSLDomain3,
                                IsSSLDomain2MB = n.IsSSLDomain2MB,
                                IsSSLDomain3MB = n.IsSSLDomain3MB,
                                SeoDes = n.SeoDes,
                                SeoKey = n.SeoKey,
                                SeoTitle = n.SeoTitle,
                                IsIssueTicket = n.IsIssueTicket,
                                LogoMbUrl = n.LogoMbUrl,
                            };

                if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.AspNetUsersInfos = query.Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).ToList();
                    foreach (var item in param.AspNetUsersInfos.Where(a => a.ParentId != null))
                    {
                        var objcha = dbContext.AspNetUsers.FirstOrDefault(z => z.Id == item.ParentId);
                        if (objcha != null)
                        {
                            item.Taikhoancha = objcha.UserName;
                        }
                    }
                }
                else
                {
                    param.AspNetUsersInfos = query.ToList();

                }
            }
        }
        public bool Checkuser(string UserName)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                return dbContext.AspNetUsers.Any(p => p.UserName.ToLower() == UserName.ToLower().Trim());
            }
        }
        public bool CheckMail(string Email, string userName)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var objuser = dbContext.AspNetUsers.FirstOrDefault(z => z.UserName == userName);
                if (objuser != null)
                {
                    if (objuser.Email != Email)
                        return dbContext.AspNetUsers.Any(p => p.Email.ToLower() == Email.ToLower().Trim());
                    else return false;
                }
                else
                    return dbContext.AspNetUsers.Any(p => p.Email.ToLower() == Email.ToLower().Trim());
            }
        }
        public bool CheckMail(string Email)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                return dbContext.AspNetUsers.Any(p => p.Email.ToLower() == Email.ToLower().Trim());
            }
        }
        public bool CheckPhone(string UserName)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                return dbContext.AspNetUsers.Any(p => p.PhoneNumber.ToLower() == UserName.ToLower().Trim());
            }
        }
        public void SearchListParam(AspNetUsersParam param)
        {
            var filter = param.AspNetUsersFilter;
            var listscate = new List<int>();
            if (filter.Id.HasValue)
            {
                listscate = Getsubcate(filter.Id.Value);
                listscate.Add(filter.Id.Value);
            }
            var contenotshow = System.Configuration.ConfigurationSettings.AppSettings.Get("Listcontenotshow");

            var listcontennotshow = !string.IsNullOrEmpty(contenotshow) ? contenotshow.Split(';').ToList() : new List<string>();

            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var list = (from n in dbContext.AspNetUsers
                            where n.Isdelete == Constant.IsNotDeleted
                            && (listscate.Count == 0 || listscate.Contains(n.Id))

                            select new AspNetUserInfo()
                            {
                                Id = n.Id,
                                Birthday = n.Birthday,
                                Email = n.Email,
                                PhoneNumber = n.PhoneNumber,
                                SiteId = n.SiteId,
                                EmailConfirmed = n.EmailConfirmed,
                                PasswordHash = n.PasswordHash,
                                SecurityStamp = n.SecurityStamp,
                                PhoneNumberConfirmed = n.PhoneNumberConfirmed,
                                TwoFactorEnabled = n.TwoFactorEnabled,
                                LockoutEndDateUtc = n.LockoutEndDateUtc,
                                LockoutEnabled = n.LockoutEnabled,
                                AccessFailedCount = n.AccessFailedCount,
                                UserName = n.UserName,
                                PhoneNumber2 = n.PhoneNumber2,
                                RegisterIP = n.RegisterIP,
                                DisplayName = n.DisplayName,
                                Location = n.Location,
                                RegisterDate = n.RegisterDate,
                                Status = n.Status,
                                Avatar = n.Avatar,
                                LastLogin = n.LastLogin,
                                Website = n.Website,
                                Facebook = n.Facebook,
                                Twitter = n.Twitter,
                                Google = n.Google,
                                Gioitinh = n.Gioitinh,
                                UrlDomain1 = n.UrlDomain1,
                                UrlDomain2 = n.UrlDomain2,
                                UrlDomain3 = n.UrlDomain3,
                                Isdelete = n.Isdelete,
                                ParentId = n.ParentId,
                                Gmap = n.Gmap,
                                LogoUrl = n.LogoUrl,
                                Baner = n.Baner,
                                StyleID = n.StyleID,
                                IsSSLDomain2 = n.IsSSLDomain2,
                                IsSSLDomain3 = n.IsSSLDomain3,
                                IsSSLDomain2MB = n.IsSSLDomain2MB,
                                IsSSLDomain3MB = n.IsSSLDomain3MB,
                                SeoDes = n.SeoDes,
                                SeoKey = n.SeoKey,
                                SeoTitle = n.SeoTitle,
                                IsIssueTicket = n.IsIssueTicket,
                                LogoMbUrl = n.LogoMbUrl,
                            }).ToList();
                param.AspNetUsersInfos = list.ToList();
            }
        }
        private List<int> Getsubcate(int parent)
        {
            var sublist = new List<int>();
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var querycatelist = (from n in dbContext.AspNetUsers
                                     where
                                         n.Isdelete == Constant.IsNotDeleted &&
                                         (n.ParentId == parent || (parent == 0 && n.ParentId.HasValue == false))
                                     select n.Id).ToList();
                foreach (var sub in querycatelist)
                {
                    sublist.AddRange(Getsubcate(sub));
                    sublist.Add(sub);
                }
            }
            return sublist;

        }
        #endregion
    }
}
