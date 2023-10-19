using System.Linq;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
using VDMMutiline.SharedComponent.Params;

namespace VDMMutiline.Dao
{
    public class SysSmsTemplateDao : BaseDao
    {
        #region Action
        public int Insert(SysSmsTemplate item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                dbContext.SysSmsTemplates.InsertOnSubmit(item);
                dbContext.SubmitChanges();
                return item.Id;
            }
        }
        public void Update(SysSmsTemplate item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.SysSmsTemplates.FirstOrDefault(sitem => sitem.Id == item.Id);
                if (dbItem != null)
                {
                   
                    dbItem.Id = item.Id;
dbItem.Body = item.Body;

                    dbContext.SubmitChanges();
                }
            }
        }

        public bool Delete(SysSmsTemplate item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.SysSmsTemplates.FirstOrDefault(en => en.Id == item.Id);

                if (dbItem != null)
                {
                    dbContext.SysSmsTemplates.DeleteOnSubmit(dbItem);
                    dbContext.SubmitChanges();
                }
            }
            return true;
        }
		 public bool DeleteUpdate(SysSmsTemplate item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.SysSmsTemplates.FirstOrDefault(en => en.Id == item.Id);
                if (dbItem != null)
                {
                    
                    dbContext.SubmitChanges();
                }
            }
            return true;
        }
        #endregion
        #region Query
        public SysSmsTemplate GetbyId(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.SysSmsTemplates
                            where n.Id == id
                            select n;
                return query.FirstOrDefault();
            }
        }

        public SysSmsTemplateInfo GetInfo(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.SysSmsTemplates
                            where n.Id == id
                            select new SysSmsTemplateInfo()
                            {
                                Id = n.Id,
Body = n.Body,

                            };
                return query.FirstOrDefault();
            }
        }
        public void Search(SysSmsTemplateParam param)
        {
            var filter = param.SysSmsTemplateFilter;
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.SysSmsTemplates
                            where (filter.Id.HasValue == false || n.Id == filter.Id)
                            select new SysSmsTemplateInfo
                            {
                                Id = n.Id,
Body = n.Body,

                            };
              
				  if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.SysSmsTemplateInfos = query.Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).ToList();
                }
                else
                {
                    param.SysSmsTemplateInfos = query.ToList();

                }
            }
        }
        public void GetAll(SysSmsTemplateParam param)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.SysSmsTemplates
                            select new SysSmsTemplateInfo
                            {
								Id = n.Id,
Body = n.Body,

                            };
              if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.SysSmsTemplateInfos = query.Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).ToList();
                }
                else
                {
                    param.SysSmsTemplateInfos = query.ToList();

                }
            }
        }

       
        #endregion
    }
}
