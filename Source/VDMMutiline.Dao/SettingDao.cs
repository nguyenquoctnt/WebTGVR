using System;
using System.Collections.Generic;
using System.Linq;
using VDMMutiline.SharedComponent.Constants;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
using VDMMutiline.SharedComponent.Params;

namespace VDMMutiline.Dao
{
    public class SettingDao : BaseDao
    {

        #region Setting Group
        public void UpdataSettingGroup(SettingGroup item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.SettingGroups.FirstOrDefault(sitem => sitem.IDSetting == item.IDSetting && sitem.VaitroId == item.VaitroId);
                if (dbItem != null)
                {
                    dbItem.Value = item.Value;
                    dbItem.UpdateDate = DateTime.Now;
                    dbContext.SubmitChanges();
                }
                else
                {
                    item.UpdateDate = DateTime.Now;
                    dbContext.SettingGroups.InsertOnSubmit(item);
                    dbContext.SubmitChanges();
                }
            }
        }
        public SettingGroupInfo GetInfoSettingGroup(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.SettingGroups
                            where n.ID == id
                            select new SettingGroupInfo()
                            {
                                ID = n.ID,
                                VaitroId = n.VaitroId,
                                IDSetting = n.IDSetting,
                                Value = n.Value,
                                UpdateDate = n.UpdateDate,
                            };
                return query.FirstOrDefault();
            }
        }
        public void GetAllSettingGroup(SettingGroupParam param)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.SettingKeys
                            join groupSetting in dbContext.SettingGroups.Where(z => z.VaitroId == param.IdGroup && z.CreateUser == param.Userid) on n.Id equals groupSetting.IDSetting into p
                            from groupSetting in p.DefaultIfEmpty()
                            select new SettingGroupInfo()
                            {
                                IdKey = n.Id,
                                Name = n.Name,
                                Description = n.Description,
                                VaitroId = groupSetting.VaitroId,
                                IDSetting = groupSetting.IDSetting,
                                Value = groupSetting.Value,
                                UpdateDate = groupSetting.UpdateDate,
                            };
                //param.SettingGroupInfos = query.ToList();
                //var listid = param.SettingGroupInfos.Select(s => s.IdKey);
                //var query2 = from n in dbContext.SettingKeys
                //             where listid.Contains(n.Id) == false
                //             select new SettingGroupInfo()
                //             {
                //                 IdKey = n.Id,
                //                 Name = n.Name,
                //                 Description = n.Description,
                //             };
                // param.SettingGroupInfos.AddRange(query2);
                param.SettingGroupInfos = query.OrderBy(z => z.IdKey).ToList();
            }
        }

        #endregion
        #region Setting User
        public void UpdataSettingUser(SettingUser item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.SettingUsers.FirstOrDefault(sitem => sitem.IDSetting == item.IDSetting && sitem.UserID == item.UserID);
                if (dbItem != null)
                {
                    dbItem.Value = item.Value;
                    dbItem.UpdateDate = DateTime.Now;
                    dbContext.SubmitChanges();
                }
                else
                {
                    item.UpdateDate = DateTime.Now;
                    dbContext.SettingUsers.InsertOnSubmit(item);
                    dbContext.SubmitChanges();
                }
            }
        }
        public SettingUserInfo GetInfoSettingUser(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.SettingUsers
                            where n.ID == id
                            select new SettingUserInfo()
                            {
                                ID = n.ID,
                                UserID = n.UserID,
                                IDSetting = n.IDSetting,
                                Value = n.Value,
                                UpdateDate = n.UpdateDate,
                            };
                return query.FirstOrDefault();
            }
        }
        public void GetAllSettingUser(SettingUserParam param)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.SettingKeys
                            join groupSetting in dbContext.SettingUsers.Where(z => param.Userid.HasValue == false || z.UserID == param.Userid) on n.Id equals groupSetting.IDSetting into p
                            from groupSetting in p.DefaultIfEmpty()
                            select new SettingUserInfo()
                            {
                                IdKey = n.Id,
                                Name = n.Name,
                                Description = n.Description,
                                UserID = groupSetting.UserID,
                                IDSetting = groupSetting.IDSetting,
                                Value = groupSetting.Value,
                                UpdateDate = groupSetting.UpdateDate,
                                GroupID = n.GroupID
                            };
                param.SettingUserInfos = query.OrderBy(z => z.IdKey).ToList();
            }
        }
        public List<SettingUserInfo> GetAllSettingUser(int userId)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.SettingKeys
                            join groupSetting in dbContext.SettingUsers on n.Id equals groupSetting.IDSetting
                            where groupSetting.UserID == userId
                            select new SettingUserInfo()
                            {
                                IdKey = n.Id,
                                Name = n.Name,
                                Description = n.Description,
                                UserID = groupSetting.UserID,
                                IDSetting = groupSetting.IDSetting,
                                Value = groupSetting.Value,
                                UpdateDate = groupSetting.UpdateDate,
                            };
                return query.OrderBy(z => z.IdKey).ToList();
            }
        }
        public List<SettingUserInfo> GetAllSettingUser(string user)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.SettingKeys
                            join groupSetting in dbContext.SettingUsers on n.Id equals groupSetting.IDSetting
                            join users in dbContext.AspNetUsers on groupSetting.UserID equals users.Id
                            where users.UserName == user
                            select new SettingUserInfo()
                            {
                                IdKey = n.Id,
                                Name = n.Name,
                                Description = n.Description,
                                UserID = groupSetting.UserID,
                                IDSetting = groupSetting.IDSetting,
                                Value = groupSetting.Value,
                                UpdateDate = groupSetting.UpdateDate,
                            };
                return query.OrderBy(z => z.IdKey).ToList();
            }
        }
        #endregion
        #region setting system
        public void Update(SysSettingSystem item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.SysSettingSystems.FirstOrDefault(sitem => sitem.ID == item.ID);
                if (dbItem != null)
                {
                    dbItem.Giatri = item.Giatri;
                    dbItem.Ngaysua = DateTime.Now;
                    dbContext.SubmitChanges();
                }
            }
        }
        public SettingSystemInfo GetInfo(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.SysSettingSystems
                            where n.ID == id
                            select new SettingSystemInfo()
                            {
                                ID = n.ID,
                                Key = n.Key,
                                Giatri = n.Giatri,
                                Mota = n.Mota,
                                Ngaysua = n.Ngaysua,
                            };
                return query.FirstOrDefault();
            }
        }
        public SettingSystemInfo GetInfoBykey(string key)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.SysSettingSystems
                            where n.Key.Trim().ToUpper() == key.Trim().ToUpper()
                            select new SettingSystemInfo()
                            {
                                ID = n.ID,
                                Key = n.Key,
                                Giatri = n.Giatri,
                                Mota = n.Mota,
                                Ngaysua = n.Ngaysua,
                            };
                return query.FirstOrDefault();
            }
        }
        public void GetAll(SettingSystemParam param)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.SysSettingSystems
                            select new SettingSystemInfo
                            {
                                ID = n.ID,
                                Key = n.Key,
                                Giatri = n.Giatri,
                                Mota = n.Mota,
                                Ngaysua = n.Ngaysua,
                            };
                param.SettingSystemInfos = query.OrderBy(z => z.ID).ToList();
            }
        }
        #endregion

    }
}
