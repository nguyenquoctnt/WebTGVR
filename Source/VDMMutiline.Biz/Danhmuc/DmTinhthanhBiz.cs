using System.Transactions;
using VDMMutiline.Dao;
using VDMMutiline.SharedComponent.Constants;
using VDMMutiline.SharedComponent.Params;

namespace VDMMutiline.Biz
{
    public class DmTinhthanhBiz
    {
        public void Insert(DmTinhthanhParam param)
        {
            var dp = param.DmTinhthanh;
            dp.Phienban = Constant.FirstVersion;
            dp.Daxoa = Constant.IsNotDeleted;
            var dao = new DmTinhthanhDao();
            dao.Insert(dp);
        }
        public void Update(DmTinhthanhParam param)
        {
            var endep = param.DmTinhthanh;
            var dao = new DmTinhthanhDao();
            dao.Update(endep);
        }
        public void Delete(DmTinhthanhParam param)
        {
            var dao = new DmTinhthanhDao();
            using (var tran = new TransactionScope())
            {
                var paramDep = param.DmTinhthanhs;
                foreach (var endep in paramDep)
                {
                    dao.Delete(endep);
                }
                tran.Complete();
            }
        }
        public void DeleteUpdate(DmTinhthanhParam param)
        {
            var dao = new DmTinhthanhDao();
            using (var tran = new TransactionScope())
            {
                var paramDep = param.DmTinhthanhs;
                foreach (var endep in paramDep)
                {
                    dao.DeleteUpdate(endep);
                }
                tran.Complete();
            }
        }
        public void SetupDisplayForm(DmTinhthanhParam param)
        {
            var dao = new DmTinhthanhDao();
            param.DmTinhthanhInfo = dao.GetInfo(param.Id);

        }

        public void SetupEditForm(DmTinhthanhParam param)
        {
            var dao = new DmTinhthanhDao();
            param.DmTinhthanh = dao.GetbyId(param.Id);
        }
        public void Search(DmTinhthanhParam param)
        {
            var dao = new DmTinhthanhDao();
            dao.Search(param);
        }
        public void GetAll(DmTinhthanhParam param)
        {
            var dao = new DmTinhthanhDao();
            dao.GetAll(param);
        }
    }
}
