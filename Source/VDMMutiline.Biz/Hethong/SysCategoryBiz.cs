using System.Transactions;
using VDMMutiline.Dao;
using VDMMutiline.SharedComponent.Constants;
using VDMMutiline.SharedComponent.Params;

namespace VDMMutiline.Biz
{
    public class SysCategoryBiz
    {
        public void Insert(SysCategoryParam param)
        {
            var dp = param.SysCategory;
            dp.Phienban = Constant.FirstVersion;
            dp.Daxoa = Constant.IsNotDeleted;
            var dao = new SysCategoryDao();
            dao.Insert(dp);
        }
        public void Update(SysCategoryParam param)
        {
            var endep = param.SysCategory;
            var dao = new SysCategoryDao();
            dao.Update(endep);
        }
        public void Delete(SysCategoryParam param)
        {
            var dao = new SysCategoryDao();
            using (var tran = new TransactionScope())
            {
                var paramDep = param.SysCategorys;
                foreach (var endep in paramDep)
                {
                    dao.Delete(endep);
                }
                tran.Complete();
            }
        }
        public void DeleteUpdate(SysCategoryParam param)
        {
            var dao = new SysCategoryDao();
            using (var tran = new TransactionScope())
            {
                var paramDep = param.SysCategorys;
                foreach (var endep in paramDep)
                {
                    dao.DeleteUpdate(endep);
                }
                tran.Complete();
            }
        }
        public void SetupDisplayForm(SysCategoryParam param)
        {
            var dao = new SysCategoryDao();
            param.SysCategoryInfo = dao.GetInfo(param.Id);

        }
        public void GetInfoBykey(SysCategoryParam param)
        {
            var dao = new SysCategoryDao();
            param.SysCategoryInfo = dao.GetInfoBykey(param.SysCategoryFilter.SeoUrl);

        }
        

        public void SetupEditForm(SysCategoryParam param)
        {
            var dao = new SysCategoryDao();
            param.SysCategory = dao.GetbyId(param.Id);
        }
        public void Search(SysCategoryParam param)
        {
            var dao = new SysCategoryDao();
            dao.Search(param);
        }
        public void SearchListParam(SysCategoryParam param)
        {
            var dao = new SysCategoryDao();
            dao.SearchListParam(param);
        }
        public void GetAll(SysCategoryParam param)
        {
            var dao = new SysCategoryDao();
            dao.GetAll(param);
        }
        public void Getparentbycate(SysCategoryParam param)
        {
            var dao = new SysCategoryDao();
            dao.Getparentbycate(param);
        }
    }
}
