using System.Linq;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
using VDMMutiline.SharedComponent.Params;

namespace VDMMutiline.Dao
{
    public class AspNetRoleGroupsDao : BaseDao
    {
        #region Action
        public int Insert(AspNetRoleGroup item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                dbContext.AspNetRoleGroups.InsertOnSubmit(item);
                dbContext.SubmitChanges();
                return item.Id;
            }
        }
        public void Update(AspNetRoleGroup item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.AspNetRoleGroups.FirstOrDefault(sitem => sitem.Id == item.Id);
                if (dbItem != null)
                {

                    dbItem.Id = item.Id;
                    dbItem.RoleId = item.RoleId;
                    dbItem.GroupId = item.GroupId;

                    dbContext.SubmitChanges();
                }
            }
        }

        public bool Delete(AspNetRoleGroup item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.AspNetRoleGroups.FirstOrDefault(en => en.Id == item.Id);

                if (dbItem != null)
                {
                    dbContext.AspNetRoleGroups.DeleteOnSubmit(dbItem);
                    dbContext.SubmitChanges();
                }
            }
            return true;
        }
        public bool DeleteUpdate(AspNetRoleGroup item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.AspNetRoleGroups.FirstOrDefault(en => en.Id == item.Id);
                if (dbItem != null)
                {

                    dbContext.SubmitChanges();
                }
            }
            return true;
        }
        #endregion
        #region Query
        public AspNetRoleGroup GetbyId(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.AspNetRoleGroups
                            where n.Id == id
                            select n;
                return query.FirstOrDefault();
            }
        }

        public AspNetRoleGroupsInfo GetInfo(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.AspNetRoleGroups
                            where n.Id == id
                            select new AspNetRoleGroupsInfo()
                            {
                                Id = n.Id,
                                RoleId = n.RoleId,
                                GroupId = n.GroupId,

                            };
                return query.FirstOrDefault();
            }
        }
        public void Search(AspNetRoleGroupsParam param)
        {
            var filter = param.AspNetRoleGroupsFilter;
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.AspNetRoleGroups
                            where (filter.Id.HasValue == false || n.Id == filter.Id)

                            select new AspNetRoleGroupsInfo
                            {
                                Id = n.Id,
                                RoleId = n.RoleId,
                                GroupId = n.GroupId,

                            };

                if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.AspNetRoleGroupsInfos = query.Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).ToList();
                }
                else
                {
                    param.AspNetRoleGroupsInfos = query.ToList();

                }
            }
        }
        public void GetAll(AspNetRoleGroupsParam param)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.AspNetRoleGroups
                    join gro in dbContext.AspNetGroups on n.GroupId equals gro.Id 
                            select new AspNetRoleGroupsInfo
                            {
                                Id = n.Id,
                                RoleId = n.RoleId,
                                GroupId = n.GroupId,
                                TenNhomQuyen = gro.Name
                            };
                if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.AspNetRoleGroupsInfos = query.Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).ToList();
                }
                else
                {
                    param.AspNetRoleGroupsInfos = query.ToList();

                }
            }
        }


        #endregion
    }
}
