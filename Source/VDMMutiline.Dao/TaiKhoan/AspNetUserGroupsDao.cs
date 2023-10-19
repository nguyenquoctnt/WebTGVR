using System.Collections.Generic;
using System.Linq;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
using VDMMutiline.SharedComponent.Params;

namespace VDMMutiline.Dao
{
    public class AspNetUserGroupDao : BaseDao
    {
        #region Action
        public int Insert(AspNetUserGroup item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                dbContext.AspNetUserGroups.InsertOnSubmit(item);
                dbContext.SubmitChanges();
                return item.Id;
            }
        }
        public void Update(AspNetUserGroup item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.AspNetUserGroups.FirstOrDefault(sitem => sitem.Id == item.Id);
                if (dbItem != null)
                {

                    dbItem.Id = item.Id;
                    dbItem.UserId = item.UserId;
                    dbItem.GroupId = item.GroupId;

                    dbContext.SubmitChanges();
                }
            }
        }

        public bool Delete(AspNetUserGroup item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.AspNetUserGroups.FirstOrDefault(en => en.Id == item.Id);

                if (dbItem != null)
                {
                    dbContext.AspNetUserGroups.DeleteOnSubmit(dbItem);
                    dbContext.SubmitChanges();
                }
            }
            return true;
        }
        public bool DeleteUpdate(AspNetUserGroup item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.AspNetUserGroups.FirstOrDefault(en => en.Id == item.Id);
                if (dbItem != null)
                {

                    dbContext.SubmitChanges();
                }
            }
            return true;
        }
        #endregion
        #region Query
        public AspNetUserGroup GetbyId(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.AspNetUserGroups
                            where n.Id == id
                            select n;
                return query.FirstOrDefault();
            }
        }
        public List<AspNetUserGroup>  GetbyGroup(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.AspNetUserGroups
                    where n.UserId == id
                    select n;
                return query.ToList();
            }
        }
        public AspNetUserGroupsInfo GetInfo(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.AspNetUserGroups
                            where n.Id == id
                            select new AspNetUserGroupsInfo()
                            {
                                Id = n.Id,
                                UserId = n.UserId,
                                GroupId = n.GroupId,

                            };
                return query.FirstOrDefault();
            }
        }
        public void Search(AspNetUserGroupsParam param)
        {
            var filter = param.AspNetUserGroupsFilter;
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.AspNetUserGroups
                            where (filter.Id.HasValue == false || n.Id == filter.Id)
                          
                            select new AspNetUserGroupsInfo
                            {
                                Id = n.Id,
                                UserId = n.UserId,
                                GroupId = n.GroupId,

                            };

                if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.AspNetUserGroupsInfos = query.Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).ToList();
                }
                else
                {
                    param.AspNetUserGroupsInfos = query.ToList();

                }
            }
        }
        public void GetAll(AspNetUserGroupsParam param)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.AspNetUserGroups
                            where param.userId==0 || n.UserId== param.userId
                            select new AspNetUserGroupsInfo
                            {
                                Id = n.Id,
                                UserId = n.UserId,
                                GroupId = n.GroupId,

                            };
                if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.AspNetUserGroupsInfos = query.Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).ToList();
                }
                else
                {
                    param.AspNetUserGroupsInfos = query.ToList();

                }
            }
        }
        public void GetAllByusername(AspNetUserGroupsParam param)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.AspNetUserGroups
                            where n.UserId== param.userId
                            select new AspNetUserGroupsInfo
                            {
                                Id = n.Id,
                                UserId = n.UserId,
                                GroupId = n.GroupId,

                            };
                if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.AspNetUserGroupsInfos = query.Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).ToList();
                }
                else
                {
                    param.AspNetUserGroupsInfos = query.ToList();

                }
            }
        }


        #endregion
    }
}
