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
    public class TempleateHTMLUserDao : BaseDao
    {
        #region Action
        public int Insert(tbl_TempleateHTMLUser item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                dbContext.tbl_TempleateHTMLUsers.InsertOnSubmit(item);
                dbContext.SubmitChanges();
                return item.Id;
            }
        }
        public void Update(tbl_TempleateHTMLUser item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.tbl_TempleateHTMLUsers.FirstOrDefault(sitem => sitem.Id == item.Id);
                if (dbItem != null)
                {
                    dbItem.Id = item.Id;
                    dbItem.IdTempleate = item.IdTempleate;
                    dbItem.IdUser = item.IdUser;
                    dbItem.Value = item.Value;
                    dbItem.CreateDate = item.CreateDate;
                    dbItem.UpdateDate = item.UpdateDate;
                    dbItem.UpdateUser = item.UpdateUser;
                    dbContext.SubmitChanges();
                }
            }
        }
        public bool Delete(tbl_TempleateHTMLUser item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.tbl_TempleateHTMLUsers.FirstOrDefault(en => en.Id == item.Id);
                if (dbItem != null)
                {
                    dbContext.tbl_TempleateHTMLUsers.DeleteOnSubmit(dbItem);
                    dbContext.SubmitChanges();
                }
            }
            return true;
        }
        public int Updatedata(tbl_TempleateHTMLUser item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var check = dbContext.tbl_TempleateHTMLUsers.FirstOrDefault(z => z.IdTempleate == item.IdTempleate && z.IdUser == item.IdUser);
                if (check == null)
                {
                    dbContext.tbl_TempleateHTMLUsers.InsertOnSubmit(item);
                    dbContext.SubmitChanges();
                }
                else
                {
                    check.Value = item.Value;
                    check.UpdateDate = item.UpdateDate;
                    check.UpdateUser = item.UpdateUser;
                    dbContext.SubmitChanges();
                }
                return 0;
            }
        }
        #endregion
        #region Query
        public tbl_TempleateHTMLUser GetbyId(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.tbl_TempleateHTMLUsers
                            where n.Id == id
                            select n;
                return query.FirstOrDefault();
            }
        }
        public TempleateHTMLUserInfo GetInfo(int? Idtempleate,int? user)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.tbl_TempleateHTMLs
                            join p in dbContext.tbl_TempleateHTMLUsers.Where(z => z.IdUser == user) on n.Id equals p.IdTempleate into s
                            from p in s.DefaultIfEmpty()
                            where n.Id== Idtempleate
                            select new TempleateHTMLUserInfo
                            {
                                Id = n.Id,
                                IdTempleate = n.Id,
                                IdUser = p.IdUser,
                                Value = (p.IdTempleate>0) ? p.Value : n.DefaultValue,
                                CreateDate = n.CreateDate,
                                CreateUserd = n.CreateUserd,
                                UpdateDate = n.UpdateDate,
                                UpdateUser = n.UpdateUser,
                                Name=n.Name,
                                Note=n.Note
                            };
                return query.AsParallel().FirstOrDefault();
            }
        }
        public void SearchByUser(TempleateHTMLUserParam param)
        {
            var filter = param.TempleateHTMLUserFilter;
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.tbl_TempleateHTMLs
                            join p in dbContext.tbl_TempleateHTMLUsers.Where(z => z.IdUser == filter.UserId) on n.Id equals p.IdTempleate into s
                            from p in s.DefaultIfEmpty()
                            where (filter.Id.HasValue == false || n.Id == filter.Id)
                            select new TempleateHTMLUserInfo
                            {
                                Id = n.Id,
                                IdTempleate = n.Id,
                                IdUser = p.IdUser,
                                Value = (p.Value == null || p.Value == "") ? n.DefaultValue : p.Value,
                                CreateDate = n.CreateDate,
                                CreateUserd = n.CreateUserd,
                                UpdateDate = n.UpdateDate,
                                UpdateUser = n.UpdateUser,
                                Name = n.Name,
                                Note = n.Note
                            };

                if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.TempleateHTMLUserInfos = query.Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).AsParallel().ToList();
                }
                else
                {
                    param.TempleateHTMLUserInfos = query.AsParallel().ToList();
                }
            }
        }
        #endregion
    }
}
