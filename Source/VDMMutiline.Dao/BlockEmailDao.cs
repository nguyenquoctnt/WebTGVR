using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.Params;

namespace VDMMutiline.Dao
{
    public class BlockEmailDao : BaseDao
    {
        #region Action
        public void Insert(BlockMail item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                dbContext.BlockMails.InsertOnSubmit(item);
                dbContext.SubmitChanges();
            }
        }

        public void Update(BlockMail item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.BlockMails.FirstOrDefault(sitem => sitem.Id == item.Id);
                if (dbItem != null)
                {
                    dbItem.Mail = item.Mail;
                    dbItem.GhiChu = item.GhiChu;
                    dbContext.SubmitChanges();
                }
            }
        }

        public bool Delete(BlockMail item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.BlockMails.FirstOrDefault(sitem => sitem.Id == item.Id);

                if (dbItem != null)
                {
                    dbContext.BlockMails.DeleteOnSubmit(dbItem);
                    dbContext.SubmitChanges();
                }
            }
            return true;
        }


        #endregion
        #region Query
        public BlockMail GetbyId(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.BlockMails
                            where n.Id == id
                            select n;
                return query.FirstOrDefault();
            }
        }
        public bool Checktrung(string mail)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.BlockMails
                            where n.Mail == mail
                            select n;
                return query.FirstOrDefault() == null;
            }
        }

        public bool Checkblock(string Mail)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.BlockMails
                            where n.Mail == Mail.Trim()
                            select n;
                if (query.Count() == 0 )
                    return true;
                else
                {
                    return false;
                }
            }
        }
      
        public void Search(BlockMailParam param)
        {

            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from item in dbContext.BlockMails
                            select item;
                param.BlockMails = query.OrderByDescending(z => z.Id).Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).ToList();
            }
        }

        #endregion
    }
}
