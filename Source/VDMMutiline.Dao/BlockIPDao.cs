using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.Params;

namespace VDMMutiline.Dao
{
    public class BlockIPDao:BaseDao
    {
        #region Action
        public void Insert(BlockIP item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                dbContext.BlockIPs.InsertOnSubmit(item);
                dbContext.SubmitChanges();
            }
        }

        public void Update(BlockIP item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.BlockIPs.FirstOrDefault(sitem => sitem.Id == item.Id);
                if (dbItem != null)
                {
                    dbItem.IP = item.IP;
                    dbItem.GhiChu = item.GhiChu;
                    dbContext.SubmitChanges();
                }
            }
        }

        public bool Delete(BlockIP item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.BlockIPs.FirstOrDefault(sitem => sitem.Id == item.Id);

                if (dbItem != null)
                {
                    dbContext.BlockIPs.DeleteOnSubmit(dbItem);
                    dbContext.SubmitChanges();
                }
            }
            return true;
        }


        #endregion
        #region Query
        public BlockIP GetbyId(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.BlockIPs
                            where n.Id == id
                            select n;
                return query.FirstOrDefault();
            }
        }
        public bool Checktrung(string IP)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.BlockIPs
                            where n.IP == IP
                            select n;
                return query.FirstOrDefault() == null;
            }
        }

        public bool Checkblock(string IP)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.BlockIPs
                            where n.IP == IP.Trim()
                            select n;
                if (query.Count() == 0 )
                    return true;
                else
                {
                    return false;
                }
            }
        }
   
        public void Search(BlockIPParam param)
        {

            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from item in dbContext.BlockIPs
                            select item;
                param.BlockIPs = query.OrderByDescending(z => z.Id).Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).ToList();
            }
        }

        #endregion
    }
}
