using System.Transactions;
using VDMMutiline.Dao;
using VDMMutiline.SharedComponent.Constants;
using VDMMutiline.SharedComponent.Params;

namespace VDMMutiline.Biz
{
    public class DmHuyenthiBiz
    {
        public void Insert(DmHuyenthiParam param)
        {
            var dp = param.DmHuyenthi;
            dp.Phienban = Constant.FirstVersion;
            dp.Daxoa = Constant.IsNotDeleted;
            var dao = new DmHuyenthiDao();
            dao.Insert(dp);
        }
        public void Update(DmHuyenthiParam param)
        {
            var endep = param.DmHuyenthi;
            var dao = new DmHuyenthiDao();
            dao.Update(endep);
        }
        public void Delete(DmHuyenthiParam param)
        {
            var dao = new DmHuyenthiDao();
            using (var tran = new TransactionScope())
            {
                var paramDep = param.DmHuyenthis;
                foreach (var endep in paramDep)
                {
                    dao.Delete(endep);
                }
                tran.Complete();
            }
        }
        public void DeleteUpdate(DmHuyenthiParam param)
        {
            var dao = new DmHuyenthiDao();
            using (var tran = new TransactionScope())
            {
                var paramDep = param.DmHuyenthis;
                foreach (var endep in paramDep)
                {
                    dao.DeleteUpdate(endep);
                }
                tran.Complete();
            }
        }
        public void SetupDisplayForm(DmHuyenthiParam param)
        {
            var dao = new DmHuyenthiDao();
            param.DmHuyenthiInfo = dao.GetInfo(param.Id);

        }

        public void SetupEditForm(DmHuyenthiParam param)
        {
            var dao = new DmHuyenthiDao();
            param.DmHuyenthi = dao.GetbyId(param.Id);
        }
        public void Search(DmHuyenthiParam param)
        {
            var dao = new DmHuyenthiDao();
            dao.Search(param);
        }
        public void GetAll(DmHuyenthiParam param)
        {
            var dao = new DmHuyenthiDao();
            dao.GetAll(param);
        }
    }
}
