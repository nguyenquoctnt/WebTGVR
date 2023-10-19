using System.Linq;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
using VDMMutiline.SharedComponent.Params;

namespace VDMMutiline.Dao
{
    public class TblCategoryArticleDao : BaseDao
    {
        #region Action
        public decimal Insert(TblCategoryArticle item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                dbContext.TblCategoryArticles.InsertOnSubmit(item);
                dbContext.SubmitChanges();
             
                return item.Id;
            }
        }
        public void Update(TblCategoryArticle item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.TblCategoryArticles.FirstOrDefault(sitem => sitem.Id == item.Id);
                if (dbItem != null)
                {

                    dbItem.Id = item.Id;
                    dbItem.IdCate = item.IdCate;
                    dbItem.IdArticle = item.IdArticle;

                    dbContext.SubmitChanges();
                }
            }
        }

        public bool Delete(TblCategoryArticle item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.TblCategoryArticles.FirstOrDefault(en => en.Id == item.Id);

                if (dbItem != null)
                {
                    dbContext.TblCategoryArticles.DeleteOnSubmit(dbItem);
                    dbContext.SubmitChanges();
                }
            }
            return true;
        }
        public bool DeleteUpdate(TblCategoryArticle item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.TblCategoryArticles.FirstOrDefault(en => en.Id == item.Id);
                if (dbItem != null)
                {

                    dbContext.SubmitChanges();
                }
            }
            return true;
        }
        #endregion
        #region Query
        public TblCategoryArticle GetbyId(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.TblCategoryArticles
                            where n.Id == id
                            select n;
                return query.FirstOrDefault();
            }
        }

        public TblCategoryArticleInfo GetInfo(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.TblCategoryArticles
                            where n.Id == id
                            select new TblCategoryArticleInfo()
                            {
                                Id = n.Id,
                                IdCate = n.IdCate,
                                IdArticle = n.IdArticle,

                            };
                return query.FirstOrDefault();
            }
        }
        public void Search(TblCategoryArticleParam param)
        {
            var filter = param.TblCategoryArticleFilter;
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.TblCategoryArticles
                            where (filter.Id.HasValue == false || n.Id == filter.Id)
                                    && (filter.ArticalId.HasValue == false || n.IdArticle == filter.ArticalId)
                            select new TblCategoryArticleInfo
                            {
                                Id = n.Id,
                                IdCate = n.IdCate,
                                IdArticle = n.IdArticle,
                            };

                if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.TblCategoryArticleInfos = query.Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).ToList();
                }
                else
                {
                    param.TblCategoryArticleInfos = query.ToList();

                }
            }
        }
        public void GetAll(TblCategoryArticleParam param)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.TblCategoryArticles
                            select new TblCategoryArticleInfo
                            {
                                Id = n.Id,
                                IdCate = n.IdCate,
                                IdArticle = n.IdArticle,

                            };
                if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.TblCategoryArticleInfos = query.Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).ToList();
                }
                else
                {
                    param.TblCategoryArticleInfos = query.ToList();

                }
            }
        }


        #endregion
    }
}
