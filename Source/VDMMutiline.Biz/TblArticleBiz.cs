using System;
using System.Transactions;
using VDMMutiline.Dao;
using VDMMutiline.SharedComponent.Constants;
using VDMMutiline.SharedComponent.Params;

namespace VDMMutiline.Biz
{
    public class TblArticleBiz
    {
        public void Insert(TblArticleParam param)
        {
            var dp = param.TblArticle;
            dp.Phienban = Constant.FirstVersion;
            dp.Daxoa = Constant.IsNotDeleted;
            // dp.Trangthai = Constant.RecordStatus.Draft;
            var dao = new TblArticleDao();
            dao.Insert(dp);
        }
        public void Update(TblArticleParam param)
        {
            var endep = param.TblArticle;
            var dao = new TblArticleDao();
            dao.Update(endep);
        }
        public void Delete(TblArticleParam param)
        {
            var dao = new TblArticleDao();
            using (var tran = new TransactionScope())
            {
                var paramDep = param.TblArticles;
                foreach (var endep in paramDep)
                {
                    dao.Delete(endep);
                }
                tran.Complete();
            }
        }
        public void UpdateCateID(int? cateid, decimal? articalID)
        {
            var dao = new TblArticleDao();
            dao.UpdateCateID(cateid, articalID);
        }
        public void Updata(TblArticleParam param)
        {
            var dao = new TblArticleDao();
            using (var tran = new TransactionScope())
            {
                try
                {
                    param.Id = dao.Updata(param.TblArticle, param.Listcate);
                    tran.Complete();
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }

            }
        }
        public void DeleteUpdate(TblArticleParam param)
        {
            var dao = new TblArticleDao();
            using (var tran = new TransactionScope())
            {
                var paramDep = param.TblArticles;
                foreach (var endep in paramDep)
                {
                    dao.DeleteUpdate(endep);
                }
                tran.Complete();
            }
        }
        public void SetupDisplayForm(TblArticleParam param)
        {
            var dao = new TblArticleDao();
            param.TblArticleInfo = dao.GetInfo(param.Id);

        }
        public void SetupDisplaybykey(TblArticleParam param)
        {
            var dao = new TblArticleDao();
            param.TblArticleInfo = dao.GetInfo(param.key);
        }
        public void SetupEditForm(TblArticleParam param)
        {
            var dao = new TblArticleDao();
            param.TblArticle = dao.GetbyId(param.Id);
        }
        public void Search(TblArticleParam param)
        {
            var dao = new TblArticleDao();
            dao.Search(param);
        }
        public void SearchHome(TblArticleParam param)
        {
            var dao = new TblArticleDao();
            dao.SearchHome(param);
        }

    }
}
