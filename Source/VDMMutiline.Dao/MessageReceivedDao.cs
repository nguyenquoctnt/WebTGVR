using System.Linq;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
using VDMMutiline.SharedComponent.Params;

namespace VDMMutiline.Dao
{
    public class MessageReceivedDao : BaseDao
    {
        #region Action
        public int Insert(MessageReceived item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                dbContext.MessageReceiveds.InsertOnSubmit(item);
                dbContext.SubmitChanges();
                return item.Id;
            }
        }
        public void Update(MessageReceived item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.MessageReceiveds.FirstOrDefault(sitem => sitem.Id == item.Id);
                if (dbItem != null)
                {
                   
                    dbItem.Id = item.Id;
dbItem.MsgId = item.MsgId;
dbItem.IdReceiver = item.IdReceiver;
dbItem.Ngaynhan = item.Ngaynhan;

                    dbContext.SubmitChanges();
                }
            }
        }

        public bool Delete(MessageReceived item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.MessageReceiveds.FirstOrDefault(en => en.Id == item.Id);

                if (dbItem != null)
                {
                    dbContext.MessageReceiveds.DeleteOnSubmit(dbItem);
                    dbContext.SubmitChanges();
                }
            }
            return true;
        }
		 public bool DeleteUpdate(MessageReceived item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.MessageReceiveds.FirstOrDefault(en => en.Id == item.Id);
                if (dbItem != null)
                {
                    
                    dbContext.SubmitChanges();
                }
            }
            return true;
        }
        #endregion
        #region Query
        public MessageReceived GetbyId(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.MessageReceiveds
                            where n.Id == id
                            select n;
                return query.FirstOrDefault();
            }
        }

        public MessageReceivedInfo GetInfo(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.MessageReceiveds
                            where n.Id == id
                            select new MessageReceivedInfo()
                            {
                                Id = n.Id,
MsgId = n.MsgId,
IdReceiver = n.IdReceiver,
Ngaynhan = n.Ngaynhan,

                            };
                return query.FirstOrDefault();
            }
        }
        public void Search(MessageReceivedParam param)
        {
            var filter = param.MessageReceivedFilter;
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.MessageReceiveds
                            where (filter.Id.HasValue == false || n.Id == filter.Id)
                            select new MessageReceivedInfo
                            {
                                Id = n.Id,
MsgId = n.MsgId,
IdReceiver = n.IdReceiver,
Ngaynhan = n.Ngaynhan,

                            };
              
				  if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.MessageReceivedInfos = query.OrderByDescending(z => z.Id).Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).ToList();
                }
                else
                {
                    param.MessageReceivedInfos = query.OrderByDescending(z => z.Id).ToList();

                }
            }
        }
        public void GetAll(MessageReceivedParam param)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.MessageReceiveds
                            select new MessageReceivedInfo
                            {
								Id = n.Id,
MsgId = n.MsgId,
IdReceiver = n.IdReceiver,
Ngaynhan = n.Ngaynhan,

                            };
              if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.MessageReceivedInfos = query.OrderByDescending(z => z.Id).Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).ToList();
                }
                else
                {
                    param.MessageReceivedInfos = query.OrderByDescending(z => z.Id).ToList();

                }
            }
        }

       
        #endregion
    }
}
