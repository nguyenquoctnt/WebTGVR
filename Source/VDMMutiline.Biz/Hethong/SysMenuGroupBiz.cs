using System.Transactions;
using VDMMutiline.Dao;
using VDMMutiline.SharedComponent.Params;

namespace VDMMutiline.Biz
{
   public class SysMenuGroupBiz
    {

        public void Insert(SysMenuGroupParam param)
        {
            var dp = param.SysMenuGroup;
            var dao = new SysMenuGroupDao();
            dao.Insert(dp);
        }
        public void Update(SysMenuGroupParam param)
        {
            var endep = param.SysMenuGroup;
            var dao = new SysMenuGroupDao();
            dao.Update(endep);
        }
        public void Delete(SysMenuGroupParam param)
        {
            var dao = new SysMenuGroupDao();
            using (var tran = new TransactionScope())
            {
                var paramDep = param.SysMenuGroups;
                foreach (var endep in paramDep)
                {
                    dao.Delete(endep);
                }
                tran.Complete();
            }
        }
        public void DeleteByidMenu(SysMenuGroupParam param)
        {
            var dao = new SysMenuGroupDao();
            dao.DeleteByidMenu(param.MenuId ?? 0);
        }
        
        public void DeleteUpdate(SysMenuGroupParam param)
        {
            var dao = new SysMenuGroupDao();
            using (var tran = new TransactionScope())
            {
                var paramDep = param.SysMenuGroups;
                foreach (var endep in paramDep)
                {
                    dao.DeleteUpdate(endep);
                }
                tran.Complete();
            }
        }
        public void SetupDisplayForm(SysMenuGroupParam param)
        {
            var dao = new SysMenuGroupDao();
            param.SysMenuGroupInfo = dao.GetInfo(param.Id??0);

        }

        public void SetupEditForm(SysMenuGroupParam param)
        {
            var dao = new SysMenuGroupDao();
            param.SysMenuGroup = dao.GetbyId(param.Id??0);
        }
        public void Search(SysMenuGroupParam param)
        {
            var dao = new SysMenuGroupDao();
            dao.Search(param);
        }
        public void GetAll(SysMenuGroupParam param)
        {
            var dao = new SysMenuGroupDao();
            dao.GetAll(param);
        }
    }
}
