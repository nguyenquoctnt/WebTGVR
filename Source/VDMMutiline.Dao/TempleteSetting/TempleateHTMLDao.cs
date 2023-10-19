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
    public class TempleateHTMLDao : BaseDao
    {
        #region Action
        public int Insert(tbl_TempleateHTML item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                dbContext.tbl_TempleateHTMLs.InsertOnSubmit(item);
                dbContext.SubmitChanges();
                return item.Id;
            }
        }
        public void Update(tbl_TempleateHTML item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.tbl_TempleateHTMLs.FirstOrDefault(sitem => sitem.Id == item.Id);
                if (dbItem != null)
                {
                    dbItem.Id = item.Id;
                    dbItem.Name = item.Name;
                    dbItem.Note = item.Note;
                    dbItem.DefaultValue = item.DefaultValue;
                    dbItem.UpdateDate = item.UpdateDate;
                    dbItem.UpdateUser = item.UpdateUser;
                    dbContext.SubmitChanges();
                }
            }
        }
        public bool Delete(tbl_TempleateHTML item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.tbl_TempleateHTMLs.FirstOrDefault(en => en.Id == item.Id);
                if (dbItem != null)
                {
                    dbContext.tbl_TempleateHTMLs.DeleteOnSubmit(dbItem);
                    dbContext.SubmitChanges();
                }
            }
            return true;
        }
        #endregion
        #region Query
        public tbl_TempleateHTML GetbyId(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.tbl_TempleateHTMLs
                            where n.Id == id
                            select n;
                return query.FirstOrDefault();
            }
        }
        public TempleateHTMLInfo GetInfo(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.tbl_TempleateHTMLs
                            where n.Id == id
                            select new TempleateHTMLInfo()
                            {
                                Id = n.Id,
                                Name = n.Name,
                                Note = n.Note,
                                CreateDate = n.CreateDate,
                                CreateUserd = n.CreateUserd,
                                UpdateDate = n.UpdateDate,
                                UpdateUser = n.UpdateUser,
                                DefaultValue = n.DefaultValue
                            };
                return query.AsParallel().FirstOrDefault();
            }
        }
        public void Search(TempleateHTMLParam param)
        {
            var filter = param.TempleateHTMLFilter;
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.tbl_TempleateHTMLs
                            where (filter.Id.HasValue == false || n.Id == filter.Id)
                               && (filter.ItemId.HasValue == false || filter.ItemId == 0 )
                            && (string.IsNullOrEmpty(filter.Search) || n.Name.Contains(filter.Search))
                            select new TempleateHTMLInfo
                            {
                                Id = n.Id,
                                Name = n.Name,
                                Note = n.Note,
                                CreateDate = n.CreateDate,
                                CreateUserd = n.CreateUserd,
                                UpdateDate = n.UpdateDate,
                                UpdateUser = n.UpdateUser,
                                DefaultValue = n.DefaultValue
                            };

                if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.TempleateHTMLInfos = query.Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).AsParallel().ToList();
                }
                else
                {
                    param.TempleateHTMLInfos = query.AsParallel().ToList();

                }
            }
        }
        public void GetAll(TempleateHTMLParam param)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.tbl_TempleateHTMLs
                            select new TempleateHTMLInfo
                            {
                                Id = n.Id,
                                
                                Name = n.Name,
                                Note = n.Note,
                                CreateDate = n.CreateDate,
                                CreateUserd = n.CreateUserd,
                                UpdateDate = n.UpdateDate,
                                UpdateUser = n.UpdateUser,
                                DefaultValue = n.DefaultValue
                            };
                if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.TempleateHTMLInfos = query.Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).AsParallel().ToList();
                }
                else
                {
                    param.TempleateHTMLInfos = query.AsParallel().ToList();

                }
            }
        }
        #endregion
    }
}
