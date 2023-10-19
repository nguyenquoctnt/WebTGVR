using System.Transactions;
using VDMMutiline.Dao;
using VDMMutiline.SharedComponent.Constants;
using VDMMutiline.SharedComponent.Params;

namespace VDMMutiline.Biz
{
    public class DmXaphuongBiz
    {
        public void Insert(DmXaphuongParam param)
        {
            var dp = param.DmXaphuong;
            dp.Phienban = Constant.FirstVersion;
            dp.Daxoa = Constant.IsNotDeleted;
            var dao = new DmXaphuongDao();
            dao.Insert(dp);
        }
        public void Update(DmXaphuongParam param)
        {
            var endep = param.DmXaphuong;
            var dao = new DmXaphuongDao();
            dao.Update(endep);
        }
        public void Delete(DmXaphuongParam param)
        {
            var dao = new DmXaphuongDao();
            using (var tran = new TransactionScope())
            {
                var paramDep = param.DmXaphuongs;
                foreach (var endep in paramDep)
                {
                    dao.Delete(endep);
                }
                tran.Complete();
            }
        }
        public void DeleteUpdate(DmXaphuongParam param)
        {
            var dao = new DmXaphuongDao();
            using (var tran = new TransactionScope())
            {
                var paramDep = param.DmXaphuongs;
                foreach (var endep in paramDep)
                {
                    dao.DeleteUpdate(endep);
                }
                tran.Complete();
            }
        }
        public void SetupDisplayForm(DmXaphuongParam param)
        {
            var dao = new DmXaphuongDao();
            param.DmXaphuongInfo = dao.GetInfo(param.Id);

        }

        public void SetupEditForm(DmXaphuongParam param)
        {
            var dao = new DmXaphuongDao();
            param.DmXaphuong = dao.GetbyId(param.Id);
        }
        public void Search(DmXaphuongParam param)
        {
            var dao = new DmXaphuongDao();
            dao.Search(param);
        }
        public void GetAll(DmXaphuongParam param)
        {
            var dao = new DmXaphuongDao();
            dao.GetAll(param);
        }
    }
}
