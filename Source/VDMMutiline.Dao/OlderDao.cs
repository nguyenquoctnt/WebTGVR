using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
using VDMMutiline.SharedComponent.Params;
using VDMMutiline.SharedComponent.Constants;

namespace VDMMutiline.Dao
{
    public class OlderDao : BaseDao
    {
        #region Action
        public void Insert(BK_Order item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                dbContext.BK_Orders.InsertOnSubmit(item);
                dbContext.SubmitChanges();
            }
        }
        public void Update(BK_Order item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.BK_Orders.FirstOrDefault(sitem => sitem.ID == item.ID);
                if (dbItem != null)
                {
                    dbItem.IsDeleted = true;
                    dbContext.SubmitChanges();
                }
            }
        }

        public bool Delete(int item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.BK_Orders.FirstOrDefault(en => en.ID == item);

                if (dbItem != null)
                {
                    dbContext.BK_Orders.DeleteOnSubmit(dbItem);
                    dbContext.SubmitChanges();
                }
            }
            return true;
        }
        #endregion
        #region Query
        public BK_Order GetbyId(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.BK_Orders
                            where n.ID == id
                            select n;
                return query.FirstOrDefault();
            }
        }
        public void Search(OlderParam param)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from item in dbContext.BK_Orders
                            where param.ListUserSite.Contains(item.UserSite)
                            && (string.IsNullOrEmpty(param.Seach) || (item.Name.Contains(param.Seach) || item.Phone.Contains(param.Seach) || item.Mail.Contains(param.Seach)))
                            select item;
                if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.BK_Orders = query.OrderByDescending(z => z.CreatedDate).Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).ToList();
                }
                else
                {
                    param.BK_Orders = query.OrderByDescending(z => z.CreatedDate).ToList();
                }
            }
        }
        #endregion
    }
}
