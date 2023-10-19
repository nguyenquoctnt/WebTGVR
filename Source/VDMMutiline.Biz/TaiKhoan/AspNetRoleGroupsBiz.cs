using System.Transactions;
using VDMMutiline.Dao;
using VDMMutiline.SharedComponent.Params;

namespace VDMMutiline.Biz
{
    public class AspNetRoleGroupsBiz
    {
        public void Insert(AspNetRoleGroupsParam param)
        {
            var dp = param.AspNetRoleGroups;
         
            var dao = new AspNetRoleGroupsDao();
            dao.Insert(dp);
        }
        public void Update(AspNetRoleGroupsParam param)
        {
            var endep = param.AspNetRoleGroups;
            var dao = new AspNetRoleGroupsDao();
            dao.Update(endep);
        }
        public void Delete(AspNetRoleGroupsParam param)
        {
            var dao = new AspNetRoleGroupsDao();
            using (var tran = new TransactionScope())
            {
                var paramDep = param.AspNetRoleGroupss;
                foreach (var endep in paramDep)
                {
                    dao.Delete(endep);
                }
                tran.Complete();
            }
        }
        public void DeleteUpdate(AspNetRoleGroupsParam param)
        {
            var dao = new AspNetRoleGroupsDao();
            using (var tran = new TransactionScope())
            {
                var paramDep = param.AspNetRoleGroupss;
                foreach (var endep in paramDep)
                {
                    dao.DeleteUpdate(endep);
                }
                tran.Complete();
            }
        }
        public void SetupDisplayForm(AspNetRoleGroupsParam param)
        {
            var dao = new AspNetRoleGroupsDao();
            param.AspNetRoleGroupsInfo = dao.GetInfo(param.Id);

        }

        public void SetupEditForm(AspNetRoleGroupsParam param)
        {
            var dao = new AspNetRoleGroupsDao();
            param.AspNetRoleGroups = dao.GetbyId(param.Id);
        }
        public void Search(AspNetRoleGroupsParam param)
        {
            var dao = new AspNetRoleGroupsDao();
            dao.Search(param);
        }
        public void GetAll(AspNetRoleGroupsParam param)
        {
            var dao = new AspNetRoleGroupsDao();
            dao.GetAll(param);
        }
    }
}
