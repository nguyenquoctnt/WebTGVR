using System.Linq;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
using VDMMutiline.SharedComponent.Params;

namespace VDMMutiline.Dao
{
    public class AspNetUserClaimDao : BaseDao
    {
        #region Action
        public int Insert(AspNetUserClaim item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                dbContext.AspNetUserClaims.InsertOnSubmit(item);
                dbContext.SubmitChanges();
                return item.Id;
            }
        }
        public void Update(AspNetUserClaim item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.AspNetUserClaims.FirstOrDefault(sitem => sitem.Id == item.Id);
                if (dbItem != null)
                {

                    dbItem.Id = item.Id;
                    dbItem.UserId = item.UserId;
                    dbItem.ClaimType = item.ClaimType;
                    dbItem.ClaimValue = item.ClaimValue;

                    dbContext.SubmitChanges();
                }
            }
        }

        public bool Delete(AspNetUserClaim item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.AspNetUserClaims.FirstOrDefault(en => en.Id == item.Id);

                if (dbItem != null)
                {
                    dbContext.AspNetUserClaims.DeleteOnSubmit(dbItem);
                    dbContext.SubmitChanges();
                }
            }
            return true;
        }
        public bool DeleteUpdate(AspNetUserClaim item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.AspNetUserClaims.FirstOrDefault(en => en.Id == item.Id);
                if (dbItem != null)
                {

                    dbContext.SubmitChanges();
                }
            }
            return true;
        }
        #endregion
        #region Query
        public AspNetUserClaim GetbyId(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.AspNetUserClaims
                            where n.Id == id
                            select n;
                return query.FirstOrDefault();
            }
        }

        public AspNetUserClaimsInfo GetInfo(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.AspNetUserClaims
                            where n.Id == id
                            select new AspNetUserClaimsInfo()
                            {
                                Id = n.Id,
                                UserId = n.UserId,
                                ClaimType = n.ClaimType,
                                ClaimValue = n.ClaimValue,

                            };
                return query.FirstOrDefault();
            }
        }
        public void Search(AspNetUserClaimsParam param)
        {
            var filter = param.AspNetUserClaimsFilter;
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.AspNetUserClaims
                            where (filter.Id.HasValue == false || n.Id == filter.Id)

                            select new AspNetUserClaimsInfo
                            {
                                Id = n.Id,
                                UserId = n.UserId,
                                ClaimType = n.ClaimType,
                                ClaimValue = n.ClaimValue,

                            };

                if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.AspNetUserClaimsInfos = query.Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).ToList();
                }
                else
                {
                    param.AspNetUserClaimsInfos = query.ToList();

                }
            }
        }
        public void GetAll(AspNetUserClaimsParam param)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.AspNetUserClaims

                            select new AspNetUserClaimsInfo
                            {
                                Id = n.Id,
                                UserId = n.UserId,
                                ClaimType = n.ClaimType,
                                ClaimValue = n.ClaimValue,

                            };
                if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.AspNetUserClaimsInfos = query.Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).ToList();
                }
                else
                {
                    param.AspNetUserClaimsInfos = query.ToList();
                }
            }
        }
        #endregion
    }
}
