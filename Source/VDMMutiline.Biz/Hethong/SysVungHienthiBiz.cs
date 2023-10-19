using System.Transactions;
using VDMMutiline.Dao;
using VDMMutiline.SharedComponent.Constants;
using VDMMutiline.SharedComponent.Params;

namespace VDMMutiline.Biz
{
    public class SysVungHienthiBiz
    {
        public void Insert(SysVungHienthiParam param)
        {
            var dp = param.SysVungHienthi;
          
            dp.Daxoa = Constant.IsNotDeleted;
            var dao = new SysVungHienthiDao();
            dao.Insert(dp);
        }
        public void Update(SysVungHienthiParam param)
        {
            var endep = param.SysVungHienthi;
            var dao = new SysVungHienthiDao();
            dao.Update(endep);
        }
        public void Delete(SysVungHienthiParam param)
        {
            var dao = new SysVungHienthiDao();
            using (var tran = new TransactionScope())
            {
                var paramDep = param.SysVungHienthis;
                foreach (var endep in paramDep)
                {
                    dao.Delete(endep);
                }
                tran.Complete();
            }
        }
        public void DeleteUpdate(SysVungHienthiParam param)
        {
            var dao = new SysVungHienthiDao();
            using (var tran = new TransactionScope())
            {
                var paramDep = param.SysVungHienthis;
                foreach (var endep in paramDep)
                {
                    dao.DeleteUpdate(endep);
                }
                tran.Complete();
            }
        }
        public void SetupDisplayForm(SysVungHienthiParam param)
        {
            var dao = new SysVungHienthiDao();
            param.SysVungHienthiInfo = dao.GetInfo(param.Id);

        }

        public void SetupEditForm(SysVungHienthiParam param)
        {
            var dao = new SysVungHienthiDao();
            param.SysVungHienthi = dao.GetbyId(param.Id);
        }
        public void Search(SysVungHienthiParam param)
        {
            var dao = new SysVungHienthiDao();
            dao.Search(param);
        }
        public void GetAll(SysVungHienthiParam param)
        {
            var dao = new SysVungHienthiDao();
            dao.GetAll(param);
        }
    }
}
