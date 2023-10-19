using System.Transactions;
using VDMMutiline.Dao;
using VDMMutiline.SharedComponent.Params;

namespace VDMMutiline.Biz
{
    public class AspNetRolesBiz
    {
        public void Insert(AspNetRolesParam param)
        {
            var dp = param.AspNetRoles;
           
            var dao = new AspNetRoleDao();
            dao.Insert(dp);
        }
        public void Update(AspNetRolesParam param)
        {
            var endep = param.AspNetRoles;
            var dao = new AspNetRoleDao();
            dao.Update(endep);
        }
        public void Delete(AspNetRolesParam param)
        {
            var dao = new AspNetRoleDao();
            using (var tran = new TransactionScope())
            {
                var paramDep = param.AspNetRoless;
                foreach (var endep in paramDep)
                {
                    dao.Delete(endep);
                }
                tran.Complete();
            }
        }
        public void DeleteUpdate(AspNetRolesParam param)
        {
            var dao = new AspNetRoleDao();
            using (var tran = new TransactionScope())
            {
                var paramDep = param.AspNetRoless;
                foreach (var endep in paramDep)
                {
                    dao.DeleteUpdate(endep);
                }
                tran.Complete();
            }
        }
        public void SetupDisplayForm(AspNetRolesParam param)
        {
            var dao = new AspNetRoleDao();
            param.AspNetRolesInfo = dao.GetInfo(param.Id);

        }

        public void SetupEditForm(AspNetRolesParam param)
        {
            var dao = new AspNetRoleDao();
            param.AspNetRoles = dao.GetbyId(param.Id);
        }
        public void Search(AspNetRolesParam param)
        {
            var dao = new AspNetRoleDao();
            dao.Search(param);
        }
        public void GetAll(AspNetRolesParam param)
        {
            var dao = new AspNetRoleDao();
            dao.GetAll(param);
        }
    }
}
