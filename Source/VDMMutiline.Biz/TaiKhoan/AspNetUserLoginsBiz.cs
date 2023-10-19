using System.Transactions;
using VDMMutiline.Dao;
using VDMMutiline.SharedComponent.Params;

namespace VDMMutiline.Biz
{
    public class AspNetUserLoginsBiz
    {
        public void Insert(AspNetUserLoginsParam param)
        {
            var dp = param.AspNetUserLogins;
           
            var dao = new AspNetUserLoginDao();
            dao.Insert(dp);
        }
        public void Update(AspNetUserLoginsParam param)
        {
            var endep = param.AspNetUserLogins;
            var dao = new AspNetUserLoginDao();
            dao.Update(endep);
        }
        public void Delete(AspNetUserLoginsParam param)
        {
            var dao = new AspNetUserLoginDao();
            using (var tran = new TransactionScope())
            {
                var paramDep = param.AspNetUserLoginss;
                foreach (var endep in paramDep)
                {
                    dao.Delete(endep);
                }
                tran.Complete();
            }
        }
        public void DeleteUpdate(AspNetUserLoginsParam param)
        {
            var dao = new AspNetUserLoginDao();
            using (var tran = new TransactionScope())
            {
                var paramDep = param.AspNetUserLoginss;
                foreach (var endep in paramDep)
                {
                    dao.DeleteUpdate(endep);
                }
                tran.Complete();
            }
        }
        public void SetupDisplayForm(AspNetUserLoginsParam param)
        {
            var dao = new AspNetUserLoginDao();
            param.AspNetUserLoginsInfo = dao.GetInfo(param.Id);

        }

        public void SetupEditForm(AspNetUserLoginsParam param)
        {
            var dao = new AspNetUserLoginDao();
            param.AspNetUserLogins = dao.GetbyId(param.Id);
        }
        public void Search(AspNetUserLoginsParam param)
        {
            var dao = new AspNetUserLoginDao();
            dao.Search(param);
        }
        public void GetAll(AspNetUserLoginsParam param)
        {
            var dao = new AspNetUserLoginDao();
            dao.GetAll(param);
        }
    }
}
