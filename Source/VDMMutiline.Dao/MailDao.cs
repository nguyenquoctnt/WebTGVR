using System;
using System.Linq;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
using VDMMutiline.SharedComponent.Params;

namespace VDMMutiline.Dao
{
    public class MailDao : BaseDao
    {
        #region Action
        public void Insert(tbl_Mail item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                if (item.Mail != null)
                {
                    var dbItem =
                        dbContext.tbl_Mails.FirstOrDefault(
                            sitem => sitem.Mail.Trim().ToUpper() == item.Mail.Trim().ToUpper());
                    if (dbItem == null)
                    {
                        item.Created = DateTime.Now;
                        dbContext.tbl_Mails.InsertOnSubmit(item);
                        dbContext.SubmitChanges();
                    }
                }
            }
        }
        public bool Delete(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.tbl_Mails.FirstOrDefault(sitem => sitem.Id == id);
                if (dbItem != null)
                {
                    dbContext.tbl_Mails.DeleteOnSubmit(dbItem);
                    dbContext.SubmitChanges();
                }
            }
            return true;
        }
        #endregion
        #region Query
        public tbl_Mail GetbyId(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.tbl_Mails
                            where n.Id == id
                            select n;
                return query.FirstOrDefault();
            }
        }
        public void Search(MailParam param)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from item in dbContext.tbl_Mails
                            where param.listuserinsite.Contains(item.UserSite) 
                            && (string.IsNullOrEmpty(param.MailFilter.Search) || item.Mail.Contains(param.MailFilter.Search))
                            select new MailInfo()
                            {
                                Id = item.Id,
                                Mail = item.Mail,
                                Created = item.Created,
                                SourceSite=item.SourceSite,
                                UserSite=item.UserSite
                            };
                if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.MailInfos = query.OrderByDescending(z => z.Created).Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).ToList();
                }
                else
                {
                    param.MailInfos = query.OrderByDescending(z => z.Created).ToList();

                }
            }
        }
        #endregion
    }
}
