using System.Linq;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
using VDMMutiline.SharedComponent.Params;

namespace VDMMutiline.Dao
{
    public class AspNetRoleDao : BaseDao
    {
        #region Action
        public int Insert(AspNetRole item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                dbContext.AspNetRoles.InsertOnSubmit(item);
                dbContext.SubmitChanges();
                return item.Id;
            }
        }
        public void Update(AspNetRole item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.AspNetRoles.FirstOrDefault(sitem => sitem.Id == item.Id);
                if (dbItem != null)
                {

                    dbItem.Id = item.Id;
                    dbItem.Description = item.Description;
                    dbItem.Name = item.Name;

                    dbContext.SubmitChanges();
                }
            }
        }

        public bool Delete(AspNetRole item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.AspNetRoles.FirstOrDefault(en => en.Id == item.Id);

                if (dbItem != null)
                {
                    dbContext.AspNetRoles.DeleteOnSubmit(dbItem);
                    dbContext.SubmitChanges();
                }
            }
            return true;
        }
        public bool DeleteUpdate(AspNetRole item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.AspNetRoles.FirstOrDefault(en => en.Id == item.Id);
                if (dbItem != null)
                {
                    dbContext.SubmitChanges();
                }
            }
            return true;
        }
        #endregion
        #region Query
        public AspNetRole GetbyId(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.AspNetRoles
                            where n.Id == id
                            select n;
                return query.FirstOrDefault();
            }
        }

        public AspNetRolesInfo GetInfo(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.AspNetRoles
                            where n.Id == id
                            select new AspNetRolesInfo()
                            {
                                Id = n.Id,
                                Description = n.Description,
                                Name = n.Name,

                            };
                return query.FirstOrDefault();
            }
        }
        public void Search(AspNetRolesParam param)
        {
            var filter = param.AspNetRolesFilter;
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.AspNetRoles
                            where (filter.Id.HasValue == false || n.Id == filter.Id)
                            select new AspNetRolesInfo
                            {
                                Id = n.Id,
                                Description = n.Description,
                                Name = n.Name,

                            };

                if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.AspNetRolesInfos = query.Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).ToList();
                }
                else
                {
                    param.AspNetRolesInfos = query.ToList();

                }
            }
        }
        public void GetAll(AspNetRolesParam param)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.AspNetRoles
                            select new AspNetRolesInfo
                            {
                                Id = n.Id,
                                Description = n.Description,
                                Name = n.Name,
                                RoleCategory = n.RoleCategory
                            };
                if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.AspNetRolesInfos = query.Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).ToList();
                }
                else
                {
                    param.AspNetRolesInfos = query.ToList();

                }
            }
        }


        #endregion
    }
}
