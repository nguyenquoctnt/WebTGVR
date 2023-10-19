using System.Collections.Generic;
using System.Linq;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
using VDMMutiline.SharedComponent.Params;
using VDMMutiline.SharedComponent.Constants;
using VDMMutiline.SharedComponent.EntityInfo.TempleteSetting;
using VDMMutiline.SharedComponent.Params.TempleteSetting;

namespace VDMMutiline.Dao.TempleteSetting
{
    public class TempleatePropertyUserDao : BaseDao
    {
        #region Action
        public int Insert(tbl_TempleatePropertyUser item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                dbContext.tbl_TempleatePropertyUsers.InsertOnSubmit(item);
                dbContext.SubmitChanges();
                return item.Id;
            }
        }
        public void Update(tbl_TempleatePropertyUser item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.tbl_TempleatePropertyUsers.FirstOrDefault(sitem => sitem.Id == item.Id);
                if (dbItem != null)
                {
                    dbItem.Id = item.Id;
                    dbItem.IdProperty = item.IdProperty;
                    dbItem.IdUser = item.IdUser;
                    dbItem.Valued = item.Valued;
                    dbItem.CreateDate = item.CreateDate;
                    dbItem.UpdateDate = item.UpdateDate;
                    dbItem.UpdateUser = item.UpdateUser;
                    dbContext.SubmitChanges();
                }
            }
        }
        public bool Delete(tbl_TempleatePropertyUser item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.tbl_TempleatePropertyUsers.FirstOrDefault(en => en.Id == item.Id);
                if (dbItem != null)
                {
                    dbContext.tbl_TempleatePropertyUsers.DeleteOnSubmit(dbItem);
                    dbContext.SubmitChanges();
                }
            }
            return true;
        }
        public int Updatedata(tbl_TempleatePropertyUser item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var check = dbContext.tbl_TempleatePropertyUsers.FirstOrDefault(z => z.IdProperty == item.IdProperty && z.IdUser == item.IdUser);
                if (check == null)
                {
                    dbContext.tbl_TempleatePropertyUsers.InsertOnSubmit(item);
                    dbContext.SubmitChanges();
                }
                else
                {
                    check.Valued = item.Valued;
                    check.UpdateDate = item.UpdateDate;
                    check.UpdateUser = item.UpdateUser;
                    dbContext.SubmitChanges();
                }
                return 0;
            }
        }
        #endregion
        #region Query
        public tbl_TempleatePropertyUser GetbyId(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.tbl_TempleatePropertyUsers
                            where n.Id == id
                            select n;
                return query.FirstOrDefault();
            }
        }
        public TempleatePropertyUserInfo GetInfo(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.tbl_TempleatePropertyUsers
                            where n.Id == id
                            select new TempleatePropertyUserInfo()
                            {
                                Id = n.Id,
                                IdProperty = n.IdProperty,
                                IdUser = n.IdUser,
                                Valued = n.Valued,
                                CreateDate = n.CreateDate,
                                CreateUserd = n.CreateUserd,
                                UpdateDate = n.UpdateDate,
                                UpdateUser = n.UpdateUser,
                            };
                return query.AsParallel().FirstOrDefault();
            }
        }
        public void SearchByUser(TempleatePropertyUserParam param)
        {
            var filter = param.TempleatePropertyUserFilter;
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.tbl_TempleateProperties
                            join p in dbContext.tbl_TempleatePropertyUsers.Where(z => z.IdUser == filter.UserId) on n.Id equals p.IdProperty into s
                            from p in s.DefaultIfEmpty()
                            where (filter.Id.HasValue == false || n.Id == filter.Id)
                            && (filter.GroupId.HasValue == false || n.IdGroup == filter.GroupId)
                            select new TempleatePropertyUserInfo
                            {
                                Id = n.Id,
                                IdProperty = n.Id,
                                IdUser = p.IdUser,
                                Typecontrol = n.TypeControl,
                                Valued = (p.IdProperty>0) ? p.Valued : n.DefaultValue,
                                CreateDate = n.CreateDate,
                                CreateUserd = n.CreateUserd,
                                UpdateDate = n.UpdateDate,
                                UpdateUser = n.UpdateUser,
                                Name = n.Name,
                                Note = n.Note,
                                key=n.Keyd,
                            };

                if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.TempleatePropertyUserInfos = query.Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).AsParallel().ToList();
                }
                else
                {
                    param.TempleatePropertyUserInfos = query.AsParallel().ToList();
                }
            }
        }
        #endregion
    }
}
