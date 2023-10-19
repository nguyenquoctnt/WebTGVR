using System.Transactions;
using VDMMutiline.Dao;
using VDMMutiline.SharedComponent.Params;

namespace VDMMutiline.Biz
{
    public class AspNetUserGroupsBiz
    {
        public void Insert(AspNetUserGroupsParam param)
        {
            var dp = param.AspNetUserGroups;
           
            var dao = new AspNetUserGroupDao();
            dao.Insert(dp);
        }
        public void Update(AspNetUserGroupsParam param)
        {
            var endep = param.AspNetUserGroups;
            var dao = new AspNetUserGroupDao();
            dao.Update(endep);
        }
        public void Delete(AspNetUserGroupsParam param)
        {
            var dao = new AspNetUserGroupDao();
            using (var tran = new TransactionScope())
            {
                var paramDep = param.AspNetUserGroupss;
                foreach (var endep in paramDep)
                {
                    dao.Delete(endep);
                }
                tran.Complete();
            }
        }
        public void DeleteUpdate(AspNetUserGroupsParam param)
        {
            var dao = new AspNetUserGroupDao();
            using (var tran = new TransactionScope())
            {
                var paramDep = param.AspNetUserGroupss;
                foreach (var endep in paramDep)
                {
                    dao.DeleteUpdate(endep);
                }
                tran.Complete();
            }
        }
        public void SetupDisplayForm(AspNetUserGroupsParam param)
        {
            var dao = new AspNetUserGroupDao();
            param.AspNetUserGroupsInfo = dao.GetInfo(param.Id);

        }

        public void SetupEditForm(AspNetUserGroupsParam param)
        {
            var dao = new AspNetUserGroupDao();
            param.AspNetUserGroups = dao.GetbyId(param.Id);
        }
        public void GetUserByGroup(AspNetUserGroupsParam param)
        {
            var dao = new AspNetUserGroupDao();
            param.AspNetUserGroupss = dao.GetbyGroup(param.Id);
        }
        public void Search(AspNetUserGroupsParam param)
        {
            var dao = new AspNetUserGroupDao();
            dao.Search(param);
        }
        public void GetAll(AspNetUserGroupsParam param)
        {
            var dao = new AspNetUserGroupDao();
            dao.GetAll(param);
        }
        public void GetAllByusername(AspNetUserGroupsParam param)
        {
            var dao = new AspNetUserGroupDao();
            dao.GetAllByusername(param);
        }
        
    }
}
