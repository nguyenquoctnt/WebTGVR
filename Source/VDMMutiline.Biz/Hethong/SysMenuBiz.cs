using System.Transactions;
using VDMMutiline.Dao;
using VDMMutiline.SharedComponent.Constants;
using VDMMutiline.SharedComponent.Params;

namespace VDMMutiline.Biz
{
    public class SysMenuBiz
    {
        public void Insert(SysMenuParam param)
        {
            var dp = param.SysMenu;
            dp.Phienban = Constant.FirstVersion;
            dp.Daxoa = Constant.IsNotDeleted;
            var dao = new SysMenuDao();
            dao.Insert(dp);
        }
        public void Update(SysMenuParam param)
        {
            var endep = param.SysMenu;
            var dao = new SysMenuDao();
            dao.Update(endep);
        }
        public void Delete(SysMenuParam param)
        {
            var dao = new SysMenuDao();
            using (var tran = new TransactionScope())
            {
                var paramDep = param.SysMenus;
                foreach (var endep in paramDep)
                {
                    dao.Delete(endep);
                }
                tran.Complete();
            }
        }
        public void DeleteUpdate(SysMenuParam param)
        {
            var dao = new SysMenuDao();
            using (var tran = new TransactionScope())
            {
                var paramDep = param.SysMenus;
                foreach (var endep in paramDep)
                {
                    dao.DeleteUpdate(endep);
                }
                tran.Complete();
            }
        }
        public void SetupDisplayForm(SysMenuParam param)
        {
            var dao = new SysMenuDao();
            param.SysMenuInfo = dao.GetInfo(param.Id);

        }

        public void SetupEditForm(SysMenuParam param)
        {
            var dao = new SysMenuDao();
            param.SysMenu = dao.GetbyId(param.Id);
        }
        public void Search(SysMenuParam param)
        {
            var dao = new SysMenuDao();
            dao.Search(param);
        }
        public void SearchAllMenu(SysMenuParam param)
        {
            var dao = new SysMenuDao();
            dao.SearchAllMenu(param);
        }
        
        public void SearchListParam(SysMenuParam param)
        {
            var dao = new SysMenuDao();
            dao.SearchListParam(param);
        }

        public void GetAll(SysMenuParam param)
        {
            var dao = new SysMenuDao();
            dao.GetAll(param);
        }
        public void Prepare(SysMenuParam param)
        {
            var dao = new SysMenuDao();
            dao.PrepareTree(param);
        }
    }
}
