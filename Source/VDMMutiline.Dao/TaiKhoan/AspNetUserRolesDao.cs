using System.Linq;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
using VDMMutiline.SharedComponent.Params;

namespace VDMMutiline.Dao
{
    public class AspNetUserRoleDao : BaseDao
    {
        #region Action
        public int Insert(AspNetUserRole item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                dbContext.AspNetUserRoles.InsertOnSubmit(item);
                dbContext.SubmitChanges();
                return item.UserId;
            }
        }
        public void Update(AspNetUserRole item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.AspNetUserRoles.FirstOrDefault(sitem => sitem.UserId == item.UserId);
                if (dbItem != null)
                {

                    dbItem.UserId = item.UserId;
                    dbItem.RoleId = item.RoleId;

                    dbContext.SubmitChanges();
                }
            }
        }

        public bool Delete(AspNetUserRole item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.AspNetUserRoles.FirstOrDefault(en => en.UserId == item.UserId);

                if (dbItem != null)
                {
                    dbContext.AspNetUserRoles.DeleteOnSubmit(dbItem);
                    dbContext.SubmitChanges();
                }
            }
            return true;
        }
        public bool DeleteUpdate(AspNetUserRole item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.AspNetUserRoles.FirstOrDefault(en => en.UserId == item.UserId);
                if (dbItem != null)
                {
                    dbContext.SubmitChanges();
                }
            }
            return true;
        }
        #endregion
        #region Query
        public AspNetUserRole GetbyId(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.AspNetUserRoles
                            where n.UserId == id
                            select n;
                return query.FirstOrDefault();
            }
        }

        public AspNetUserRolesInfo GetInfo(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.AspNetUserRoles
                            where n.UserId == id
                            select new AspNetUserRolesInfo()
                            {
                                UserId = n.UserId,
                                RoleId = n.RoleId,

                            };
                return query.FirstOrDefault();
            }
        }
        public void Search(AspNetUserRolesParam param)
        {
            var filter = param.AspNetUserRolesFilter;
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.AspNetUserRoles
                            where (filter.Id.HasValue == false || n.UserId == filter.Id)
                            select new AspNetUserRolesInfo
                            {
                                UserId = n.UserId,
                                RoleId = n.RoleId,

                            };

                if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.AspNetUserRolesInfos = query.Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).ToList();
                }
                else
                {
                    param.AspNetUserRolesInfos = query.ToList();

                }
            }
        }
        public void GetAll(AspNetUserRolesParam param)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.AspNetUserRoles
                            select new AspNetUserRolesInfo
                            {
                                UserId = n.UserId,
                                RoleId = n.RoleId,

                            };
                if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.AspNetUserRolesInfos = query.Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).ToList();
                }
                else
                {
                    param.AspNetUserRolesInfos = query.ToList();

                }
            }
        }


        #endregion
    }
}
