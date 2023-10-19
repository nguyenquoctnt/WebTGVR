using System;
using System.Linq;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
using VDMMutiline.SharedComponent.Params;

namespace VDMMutiline.Dao
{
    public class HMTLBlockDao : BaseDao
    {
        #region Action
        public void Insert(tbl_HtmlBlock item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                item.CreateDay = DateTime.Now;
                dbContext.tbl_HtmlBlocks.InsertOnSubmit(item);
                dbContext.SubmitChanges();
            }
        }

        public void Update(tbl_HtmlBlock item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.tbl_HtmlBlocks.FirstOrDefault(sitem => sitem.ID == item.ID);
                if (dbItem != null)
                {


                    dbItem.HtmlBlockName = item.HtmlBlockName;
                    dbItem.HtmlblockConten = item.HtmlblockConten;
                    dbItem.IsSeoconten = item.IsSeoconten;
                    dbContext.SubmitChanges();
                }
            }
        }

        public bool Delete(tbl_HtmlBlock item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.tbl_HtmlBlocks.FirstOrDefault(sitem => sitem.ID == item.ID);

                if (dbItem != null)
                {
                    dbContext.tbl_HtmlBlocks.DeleteOnSubmit(dbItem);
                    dbContext.SubmitChanges();
                }
            }
            return true;
        }


        #endregion
        #region Query
        public tbl_HtmlBlock GetbyId(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.tbl_HtmlBlocks
                            where n.ID == id
                            select n;
                return query.FirstOrDefault();
            }
        }
        public tbl_HtmlBlock GetbyKey(string key)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.tbl_HtmlBlocks
                            where n.HlmlBlockKey == key
                            select n;
                return query.FirstOrDefault();
            }
        }
        public void Search(HtmlBlockParam param)
        {

            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from item in dbContext.tbl_HtmlBlocks
                            where param.HtmlBlockFilter.ListUserName.Count==0|| param.HtmlBlockFilter.ListUserName.Contains(item.CreateBy)
                            select new HtmlBlockInfo()
                            {
                                ID=item.ID,
                                HlmlBlockKey = item.HlmlBlockKey,
                                HtmlBlockName = item.HtmlBlockName,
                                HtmlblockConten = item.HtmlblockConten,
                                IsSeoconten = item.IsSeoconten,

                            };
                param.HtmlBlockInfos = query.ToList();
            }

        }

        #endregion
    }
}
