using System.Linq;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
using VDMMutiline.SharedComponent.Params;

namespace VDMMutiline.Dao
{
   public class SysMenuGroupDao:BaseDao
    {
        #region Action
        public int Insert(SysMenuGroup item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                dbContext.SysMenuGroups.InsertOnSubmit(item);
                dbContext.SubmitChanges();
                return item.Id;
            }
        }
        public void Update(SysMenuGroup item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.SysMenuGroups.FirstOrDefault(sitem => sitem.Id == item.Id);
                if (dbItem != null)
                {
                    dbItem.GroupId = item.GroupId;
                    dbItem.MenuId = item.MenuId;
                    dbContext.SubmitChanges();
                }
            }
        }
        public bool DeleteByidMenu(int idmenu)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.SysMenuGroups.Where(en => en.MenuId == idmenu);

                if (dbItem != null)
                {
                    dbContext.SysMenuGroups.DeleteAllOnSubmit(dbItem);
                    dbContext.SubmitChanges();
                }
            }
            return true;
        }
        public bool Delete(SysMenuGroup item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.SysMenuGroups.FirstOrDefault(en => en.Id == item.Id);

                if (dbItem != null)
                {
                    dbContext.SysMenuGroups.DeleteOnSubmit(dbItem);
                    dbContext.SubmitChanges();
                }
            }
            return true;
        }
        public bool DeleteUpdate(SysMenuGroup item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.SysMenuGroups.FirstOrDefault(en => en.Id == item.Id);
                if (dbItem != null)
                {
                    dbContext.SubmitChanges();
                }
            }
            return true;
        }
        #endregion
        #region Query
        public SysMenuGroup GetbyId(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.SysMenuGroups
                            where n.Id == id
                            select n;
                return query.FirstOrDefault();
            }
        }
        public SysMenuGroupInfo GetInfo(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.SysMenuGroups
                            where n.Id == id
                            select new SysMenuGroupInfo()
                            {
                                Id = n.Id,
                                MenuId = n.MenuId,
                                GroupId = n.GroupId,
                            };
                return query.FirstOrDefault();
            }
        }
        public void Search(SysMenuGroupParam param)
        {
            var filter = param.SysMenuGroupFilter;
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.SysMenuGroups
                            select new SysMenuGroupInfo()
                            {
                                Id = n.Id,
                                MenuId = n.MenuId,
                                GroupId = n.GroupId,
                            };

                if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.SysMenuGroupInfos = query.OrderByDescending(z => z.Id).Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).ToList();
                }
                else
                {
                    param.SysMenuGroupInfos = query.OrderByDescending(z => z.Id).ToList();

                }
            }
        }
        public void GetAll(SysMenuGroupParam param)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.SysMenuGroups
                            where (param.MenuId.HasValue == false ||n.MenuId== param.MenuId)
                            select new SysMenuGroupInfo()
                            {
                                Id = n.Id,
                                MenuId = n.MenuId,
                                GroupId = n.GroupId,
                            };
                if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.SysMenuGroupInfos = query.OrderByDescending(z => z.Id).Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).ToList();
                }
                else
                {
                    param.SysMenuGroupInfos = query.OrderByDescending(z => z.Id).ToList();

                }
            }
        }
        #endregion
    }
}
