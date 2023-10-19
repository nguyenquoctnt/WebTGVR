using System.Transactions;
using VDMMutiline.Dao;
using VDMMutiline.SharedComponent.Constants;
using VDMMutiline.SharedComponent.Params;

namespace VDMMutiline.Biz
{
    public class DmQuocgiaBiz
    {
        public void Insert(DmQuocgiaParam param)
        {
            var dp = param.DmQuocgia;
            dp.Phienban = Constant.FirstVersion;
            dp.Daxoa = Constant.IsNotDeleted;
            var dao = new DmQuocgiaDao();
            dao.Insert(dp);
        }
        public void Update(DmQuocgiaParam param)
        {
            var endep = param.DmQuocgia;
            var dao = new DmQuocgiaDao();
            dao.Update(endep);
        }
        public void Delete(DmQuocgiaParam param)
        {
            var dao = new DmQuocgiaDao();
            using (var tran = new TransactionScope())
            {
                var paramDep = param.DmQuocgias;
                foreach (var endep in paramDep)
                {
                    dao.Delete(endep);
                }
                tran.Complete();
            }
        }
        public void DeleteUpdate(DmQuocgiaParam param)
        {
            var dao = new DmQuocgiaDao();
            using (var tran = new TransactionScope())
            {
                var paramDep = param.DmQuocgias;
                foreach (var endep in paramDep)
                {
                    dao.DeleteUpdate(endep);
                }
                tran.Complete();
            }
        }
        public void SetupDisplayForm(DmQuocgiaParam param)
        {
            var dao = new DmQuocgiaDao();
            param.DmQuocgiaInfo = dao.GetInfo(param.Id);

        }

        public void SetupEditForm(DmQuocgiaParam param)
        {
            var dao = new DmQuocgiaDao();
            param.DmQuocgia = dao.GetbyId(param.Id);
        }
        public void Search(DmQuocgiaParam param)
        {
            var dao = new DmQuocgiaDao();
            dao.Search(param);
        }
        public void GetAll(DmQuocgiaParam param)
        {
            var dao = new DmQuocgiaDao();
            dao.GetAll(param);
        }
    }
}
