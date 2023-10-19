using System.Transactions;
using VDMMutiline.Dao;
using VDMMutiline.SharedComponent.Params;

namespace VDMMutiline.Biz
{
    public class AspNetGroupsBiz
    {
        public void Insert(AspNetGroupsParam param)
        {
            var dp = param.AspNetGroupsInfo;
            var dao = new AspNetGroupsDao();
            dao.Insert(dp);
        }
        public void Update(AspNetGroupsParam param)
        {
            var endep = param.AspNetGroupsInfo;
            var dao = new AspNetGroupsDao();
            dao.Update(endep);
        }
        public void Delete(AspNetGroupsParam param)
        {
            var dao = new AspNetGroupsDao();
            using (var tran = new TransactionScope())
            {
                var paramDep = param.AspNetGroupsInfos;
                foreach (var endep in paramDep)
                {
                    dao.Delete(endep);
                }
                tran.Complete();
            }
        }
        public void DeleteUpdate(AspNetGroupsParam param)
        {
            var dao = new AspNetGroupsDao();
            using (var tran = new TransactionScope())
            {
                var paramDep = param.AspNetGroupsInfos;
                foreach (var endep in paramDep)
                {
                    dao.Delete(endep);
                }
                tran.Complete();
            }
        }
        public void SetupDisplayForm(AspNetGroupsParam param)
        {
            var dao = new AspNetGroupsDao();
            param.AspNetGroupsInfo = dao.GetInfo(param.Id);

        }

        public void SetupEditForm(AspNetGroupsParam param)
        {
            var dao = new AspNetGroupsDao();
            param.AspNetGroupsInfo = dao.GetInfo(param.Id);
        }
        public void Search(AspNetGroupsParam param)
        {
            var dao = new AspNetGroupsDao();
            dao.Search(param);
        }
        public void GetAll(AspNetGroupsParam param)
        {
            var dao = new AspNetGroupsDao();
            dao.GetAll(param);
        }
    }
}
