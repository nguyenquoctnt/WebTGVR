using System.Linq;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
using VDMMutiline.SharedComponent.Params;

namespace VDMMutiline.Dao
{
    public class AspNetOnlineUsersDao : BaseDao
    {
        #region Action
        public int Insert(AspNetOnlineUser item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                dbContext.AspNetOnlineUsers.InsertOnSubmit(item);
                dbContext.SubmitChanges();
                return item.UserId;
            }
        }
        public void Update(AspNetOnlineUser item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.AspNetOnlineUsers.FirstOrDefault(sitem => sitem.UserId == item.UserId);
                if (dbItem != null)
                {

                    dbItem.UserId = item.UserId;
                    dbItem.SessionId = item.SessionId;
                    dbItem.LastActivity = item.LastActivity;
                    dbItem.Device = item.Device;

                    dbContext.SubmitChanges();
                }
            }
        }

        public bool Delete(AspNetOnlineUser item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.AspNetOnlineUsers.FirstOrDefault(en => en.UserId == item.UserId);

                if (dbItem != null)
                {
                    dbContext.AspNetOnlineUsers.DeleteOnSubmit(dbItem);
                    dbContext.SubmitChanges();
                }
            }
            return true;
        }
        public bool DeleteUpdate(AspNetOnlineUser item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.AspNetOnlineUsers.FirstOrDefault(en => en.UserId == item.UserId);
                if (dbItem != null)
                {

                    dbContext.SubmitChanges();
                }
            }
            return true;
        }
        #endregion
        #region Query
        public AspNetOnlineUser GetbyId(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.AspNetOnlineUsers
                            where n.UserId == id
                            select n;
                return query.FirstOrDefault();
            }
        }

        public AspNetOnlineUsersInfo GetInfo(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.AspNetOnlineUsers
                            where n.UserId == id
                            select new AspNetOnlineUsersInfo()
                            {
                                UserId = n.UserId,
                                SessionId = n.SessionId,
                                LastActivity = n.LastActivity,
                                Device = n.Device,

                            };
                return query.FirstOrDefault();
            }
        }
        public void Search(AspNetOnlineUsersParam param)
        {
            var filter = param.AspNetOnlineUsersFilter;
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.AspNetOnlineUsers
                            where (filter.Id.HasValue == false || n.UserId == filter.Id)
                            select new AspNetOnlineUsersInfo
                            {
                                UserId = n.UserId,
                                SessionId = n.SessionId,
                                LastActivity = n.LastActivity,
                                Device = n.Device,

                            };

                if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.AspNetOnlineUsersInfos = query.Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).ToList();
                }
                else
                {
                    param.AspNetOnlineUsersInfos = query.ToList();

                }
            }
        }
        public void GetAll(AspNetOnlineUsersParam param)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.AspNetOnlineUsers

                            select new AspNetOnlineUsersInfo
                            {
                                UserId = n.UserId,
                                SessionId = n.SessionId,
                                LastActivity = n.LastActivity,
                                Device = n.Device,

                            };
                if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.AspNetOnlineUsersInfos = query.Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).ToList();
                }
                else
                {
                    param.AspNetOnlineUsersInfos = query.ToList();

                }
            }
        }


        #endregion
    }
}
