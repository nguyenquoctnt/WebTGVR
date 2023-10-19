using System.Transactions;
using VDMMutiline.Dao;
using VDMMutiline.SharedComponent.Constants;
using VDMMutiline.SharedComponent.Params;

namespace VDMMutiline.Biz
{
    public class DmTaikhoanNganHangBiz
    {
        public void Insert(DmTaikhoanNganHangParam param)
        {
            var dp = param.DmTaikhoanNganHang;
            dp.Phienban = Constant.FirstVersion;
            dp.Daxoa = Constant.IsNotDeleted;
            var dao = new DmTaikhoanNganHangDao();
            dao.Insert(dp);
        }
        public void Update(DmTaikhoanNganHangParam param)
        {
            var endep = param.DmTaikhoanNganHang;
            var dao = new DmTaikhoanNganHangDao();
            dao.Update(endep);
        }
        public void Delete(DmTaikhoanNganHangParam param)
        {
            var dao = new DmTaikhoanNganHangDao();
            using (var tran = new TransactionScope())
            {
                var paramDep = param.DmTaikhoanNganHangs;
                foreach (var endep in paramDep)
                {
                    dao.Delete(endep);
                }
                tran.Complete();
            }
        }
        public void DeleteUpdate(DmTaikhoanNganHangParam param)
        {
            var dao = new DmTaikhoanNganHangDao();
            using (var tran = new TransactionScope())
            {
                var paramDep = param.DmTaikhoanNganHangs;
                foreach (var endep in paramDep)
                {
                    dao.DeleteUpdate(endep);
                }
                tran.Complete();
            }
        }
        public void SetupDisplayForm(DmTaikhoanNganHangParam param)
        {
            var dao = new DmTaikhoanNganHangDao();
            param.DmTaikhoanNganHangInfo = dao.GetInfo(param.Id);

        }

        public void SetupEditForm(DmTaikhoanNganHangParam param)
        {
            var dao = new DmTaikhoanNganHangDao();
            param.DmTaikhoanNganHang = dao.GetbyId(param.Id);
        }
        public void Search(DmTaikhoanNganHangParam param)
        {
            var dao = new DmTaikhoanNganHangDao();
            dao.Search(param);
        }
        public void GetAll(DmTaikhoanNganHangParam param)
        {
            var dao = new DmTaikhoanNganHangDao();
            dao.GetAll(param);
        }
    }
}
