using System.Linq;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
using VDMMutiline.SharedComponent.Params;

namespace VDMMutiline.Dao
{
    public class AspNetGroupsDao : BaseDao
    {
        #region Action
        public int Insert(AspNetGroupsInfo item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = new AspNetGroup();
                dbItem.Name = item.Name;
                dbItem.Description = item.Description;
                dbContext.AspNetGroups.InsertOnSubmit(dbItem);
                dbContext.SubmitChanges();
                //if (item.RoleAssigned.Count > 0)
                //{
                //    foreach (var asss in item.RoleAssigned)
                //    {
                //        asss.GroupId = dbItem.Id;
                //        dbContext.AspNetRoleGroups.InsertOnSubmit(asss);
                //    }
                //    dbContext.SubmitChanges();
                //}
                return dbItem.Id;
            }
        }
        public void Update(AspNetGroupsInfo item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.AspNetGroups.FirstOrDefault(sitem => sitem.Id == item.Id);
                if (dbItem != null)
                {
                    dbItem.Name = item.Name;
                    dbItem.Description = item.Description;
                    //var listAssignInDB = (from d in dbContext.AspNetRoleGroups where d.GroupId == dbItem.Id select d).ToList();
                    //foreach (var asss in item.RoleAssigned)
                    //{
                    //    if (!listAssignInDB.Any(p => p.RoleId == asss.RoleId))
                    //    {
                    //        asss.GroupId = dbItem.Id;
                    //        dbContext.AspNetRoleGroups.InsertOnSubmit(asss);
                    //    }
                    //}
                    //foreach (var asss in listAssignInDB)
                    //{
                    //    if (!item.RoleAssigned.Any(p => p.RoleId == asss.RoleId))
                    //    {
                    //        dbContext.AspNetRoleGroups.DeleteOnSubmit(asss);
                    //    }
                    //}
                    dbContext.SubmitChanges();
                }
            }
        }

        public bool Delete(AspNetGroupsInfo item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.AspNetGroups.FirstOrDefault(en => en.Id == item.Id);

                if (dbItem != null)
                {
                    dbContext.AspNetGroups.DeleteOnSubmit(dbItem);
                    dbContext.SubmitChanges();
                }
            }
            return true;
        }
        public bool DeleteUpdate(AspNetGroupsInfo item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.AspNetGroups.FirstOrDefault(en => en.Id == item.Id);
                if (dbItem != null)
                {

                    dbContext.SubmitChanges();
                }
            }
            return true;
        }
        #endregion
        #region Query
        public AspNetGroup GetbyId(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.AspNetGroups
                            where n.Id == id
                            select n;
                return query.FirstOrDefault();
            }
        }

        public AspNetGroupsInfo GetInfo(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.AspNetGroups
                            where n.Id == id
                            select new AspNetGroupsInfo()
                            {
                                Id = n.Id,
                                Name = n.Name,
                                Description = n.Description,
                                RoleAssigned = (from r in dbContext.AspNetRoleGroups where r.GroupId == n.Id select r).ToList()
                            };
                return query.FirstOrDefault();
            }
        }
        public void Search(AspNetGroupsParam param)
        {
            var filter = param.AspNetGroupsFilter;
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.AspNetGroups
                            where (filter.Id.HasValue == false || n.Id == filter.Id)
                            select new AspNetGroupsInfo
                            {
                                Id = n.Id,
                                Name = n.Name,
                                Description = n.Description,

                            };

                if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.AspNetGroupsInfos = query.Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).ToList();
                }
                else
                {
                    param.AspNetGroupsInfos = query.ToList();
                }
            }
        }
        public void GetAll(AspNetGroupsParam param)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.AspNetGroups

                            select new AspNetGroupsInfo
                            {
                                Id = n.Id,
                                Name = n.Name,
                                Description = n.Description,

                            };
                if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.AspNetGroupsInfos = query.Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).ToList();
                }
                else
                {
                    param.AspNetGroupsInfos = query.ToList();

                }
            }
        }


        #endregion
    }
}
