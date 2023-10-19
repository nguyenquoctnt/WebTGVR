using System;
using System.Linq;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
using VDMMutiline.SharedComponent.Params;

namespace VDMMutiline.Dao
{
    public class MailContendDao : BaseDao
    {
        #region Action
        public void Insert(tbl_MailContend item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                item.NgayTao = DateTime.Now;
                dbContext.tbl_MailContends.InsertOnSubmit(item);
                dbContext.SubmitChanges();
            }
        }

        public void Update(tbl_MailContend item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.tbl_MailContends.FirstOrDefault(sitem => sitem.ID == item.ID);
                if (dbItem != null)
                {
                    dbItem.TieuDe = item.TieuDe;
                    dbItem.Contend = item.Contend;
                    dbItem.NgayTao = DateTime.Now;
                    dbContext.SubmitChanges();
                }
            }
        }

        public bool Delete(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.tbl_MailContends.FirstOrDefault(sitem => sitem.ID == id);
                if (dbItem != null)
                {
                    dbContext.tbl_MailContends.DeleteOnSubmit(dbItem);
                    dbContext.SubmitChanges();
                }
            }
            return true;
        }


        #endregion
        #region Query
        public tbl_MailContend GetbyId(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.tbl_MailContends
                            where n.ID == id
                            select n;
                return query.FirstOrDefault();
            }
        }
        public void Search(MailContenParam param)
        {

            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from item in dbContext.tbl_MailContends
                            where param.userNameList.Contains(item.NguoiTao)
                                 
                            select new MailContendInfo()
                            {
                                ID = item.ID,
                                TieuDe = item.TieuDe,
                                Contend = item.Contend,
                                NgayTao = item.NgayTao,
                                NguoiTao = item.NguoiTao,
                                SourceSite = item.SourceSite
                            };
                param.MailContendInfos = query.ToList();
                if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.MailContendInfos = query.OrderByDescending(z => z.NgayTao).Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).ToList();
                }
                else
                {
                    param.MailContendInfos = query.OrderByDescending(z => z.NgayTao).ToList();

                }
            }
        }
        #endregion
    }
}
