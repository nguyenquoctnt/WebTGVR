using System.Transactions;
using VDMMutiline.Dao;
using VDMMutiline.SharedComponent.Params;

namespace VDMMutiline.Biz
{
    public class AspNetUserRolesBiz
    {
        public void Insert(AspNetUserRolesParam param)
        {
            var dp = param.AspNetUserRoles;
          
            var dao = new AspNetUserRoleDao();
            dao.Insert(dp);
        }
        public void Update(AspNetUserRolesParam param)
        {
            var endep = param.AspNetUserRoles;
            var dao = new AspNetUserRoleDao();
            dao.Update(endep);
        }
        public void Delete(AspNetUserRolesParam param)
        {
            var dao = new AspNetUserRoleDao();
            using (var tran = new TransactionScope())
            {
                var paramDep = param.AspNetUserRoless;
                foreach (var endep in paramDep)
                {
                    dao.Delete(endep);
                }
                tran.Complete();
            }
        }
        public void DeleteUpdate(AspNetUserRolesParam param)
        {
            var dao = new AspNetUserRoleDao();
            using (var tran = new TransactionScope())
            {
                var paramDep = param.AspNetUserRoless;
                foreach (var endep in paramDep)
                {
                    dao.DeleteUpdate(endep);
                }
                tran.Complete();
            }
        }
        public void SetupDisplayForm(AspNetUserRolesParam param)
        {
            var dao = new AspNetUserRoleDao();
            param.AspNetUserRolesInfo = dao.GetInfo(param.Id);

        }

        public void SetupEditForm(AspNetUserRolesParam param)
        {
            var dao = new AspNetUserRoleDao();
            param.AspNetUserRoles = dao.GetbyId(param.Id);
        }
        public void Search(AspNetUserRolesParam param)
        {
            var dao = new AspNetUserRoleDao();
            dao.Search(param);
        }
        public void GetAll(AspNetUserRolesParam param)
        {
            var dao = new AspNetUserRoleDao();
            dao.GetAll(param);
        }
    }
}
