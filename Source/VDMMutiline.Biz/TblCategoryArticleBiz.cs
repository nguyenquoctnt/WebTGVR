using System.Transactions;
using VDMMutiline.Dao;
using VDMMutiline.SharedComponent.Params;

namespace VDMMutiline.Biz
{
    public class TblCategoryArticleBiz
    {
        public void Insert(TblCategoryArticleParam param)
        {
            var dp = param.TblCategoryArticle;
          
            var dao = new TblCategoryArticleDao();
            dao.Insert(dp);
        }
        public void Update(TblCategoryArticleParam param)
        {
            var endep = param.TblCategoryArticle;
            var dao = new TblCategoryArticleDao();
            dao.Update(endep);
        }
        public void Delete(TblCategoryArticleParam param)
        {
            var dao = new TblCategoryArticleDao();
            using (var tran = new TransactionScope())
            {
                var paramDep = param.TblCategoryArticles;
                foreach (var endep in paramDep)
                {
                    dao.Delete(endep);
                }
                tran.Complete();
            }
        }
        public void DeleteUpdate(TblCategoryArticleParam param)
        {
            var dao = new TblCategoryArticleDao();
            using (var tran = new TransactionScope())
            {
                var paramDep = param.TblCategoryArticles;
                foreach (var endep in paramDep)
                {
                    dao.DeleteUpdate(endep);
                }
                tran.Complete();
            }
        }
        public void SetupDisplayForm(TblCategoryArticleParam param)
        {
            var dao = new TblCategoryArticleDao();
            param.TblCategoryArticleInfo = dao.GetInfo(param.Id);

        }

        public void SetupEditForm(TblCategoryArticleParam param)
        {
            var dao = new TblCategoryArticleDao();
            param.TblCategoryArticle = dao.GetbyId(param.Id);
        }
        public void Search(TblCategoryArticleParam param)
        {
            var dao = new TblCategoryArticleDao();
            dao.Search(param);
        }
        public void GetAll(TblCategoryArticleParam param)
        {
            var dao = new TblCategoryArticleDao();
            dao.GetAll(param);
        }
    }
}
