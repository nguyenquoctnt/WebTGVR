using System.Linq;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
using VDMMutiline.SharedComponent.Params;

namespace VDMMutiline.Dao
{
    public class AspNetUserLoginDao : BaseDao
    {
        #region Action
        public int Insert(AspNetUserLogin item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                dbContext.AspNetUserLogins.InsertOnSubmit(item);
                dbContext.SubmitChanges();
                return item.UserId;
            }
        }
        public void Update(AspNetUserLogin item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.AspNetUserLogins.FirstOrDefault(sitem => sitem.UserId == item.UserId);
                if (dbItem != null)
                {

                    dbItem.LoginProvider = item.LoginProvider;
                    dbItem.ProviderKey = item.ProviderKey;
                    dbItem.UserId = item.UserId;

                    dbContext.SubmitChanges();
                }
            }
        }

        public bool Delete(AspNetUserLogin item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.AspNetUserLogins.FirstOrDefault(en => en.UserId == item.UserId);

                if (dbItem != null)
                {
                    dbContext.AspNetUserLogins.DeleteOnSubmit(dbItem);
                    dbContext.SubmitChanges();
                }
            }
            return true;
        }
        public bool DeleteUpdate(AspNetUserLogin item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.AspNetUserLogins.FirstOrDefault(en => en.UserId == item.UserId);
                if (dbItem != null)
                {

                    dbContext.SubmitChanges();
                }
            }
            return true;
        }
        #endregion
        #region Query
        public AspNetUserLogin GetbyId(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.AspNetUserLogins
                            where n.UserId == id
                            select n;
                return query.FirstOrDefault();
            }
        }

        public AspNetUserLoginsInfo GetInfo(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.AspNetUserLogins
                            where n.UserId == id
                            select new AspNetUserLoginsInfo()
                            {
                                LoginProvider = n.LoginProvider,
                                ProviderKey = n.ProviderKey,
                                UserId = n.UserId,

                            };
                return query.FirstOrDefault();
            }
        }
        public void Search(AspNetUserLoginsParam param)
        {
            var filter = param.AspNetUserLoginsFilter;
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.AspNetUserLogins
                            where (filter.Id.HasValue == false || n.UserId == filter.Id)
                          
                            select new AspNetUserLoginsInfo
                            {
                                LoginProvider = n.LoginProvider,
                                ProviderKey = n.ProviderKey,
                                UserId = n.UserId,

                            };

                if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.AspNetUserLoginsInfos = query.Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).ToList();
                }
                else
                {
                    param.AspNetUserLoginsInfos = query.ToList();

                }
            }
        }
        public void GetAll(AspNetUserLoginsParam param)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.AspNetUserLogins
                            select new AspNetUserLoginsInfo
                            {
                                LoginProvider = n.LoginProvider,
                                ProviderKey = n.ProviderKey,
                                UserId = n.UserId,

                            };
                if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.AspNetUserLoginsInfos = query.Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).ToList();
                }
                else
                {
                    param.AspNetUserLoginsInfos = query.ToList();

                }
            }
        }


        #endregion
    }
}
