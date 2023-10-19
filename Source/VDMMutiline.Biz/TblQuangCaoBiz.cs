using System.Transactions;
using VDMMutiline.Dao;
using VDMMutiline.SharedComponent.Constants;
using VDMMutiline.SharedComponent.Params;

namespace VDMMutiline.Biz
{
    public class TblQuangCaoBiz
    {
        public void Insert(TblQuangCaoParam param)
        {
            var dp = param.TblQuangCao;
            dp.Daxoa = Constant.IsNotDeleted;
            dp.Trangthai = Constant.TrangthaiBanghi.Draft;
            var dao = new TblQuangCaoDao();
            dao.Insert(dp);
        }
        public void Update(TblQuangCaoParam param)
        {
            var endep = param.TblQuangCao;
            var dao = new TblQuangCaoDao();
            dao.Update(endep);
        }
        public void Delete(TblQuangCaoParam param)
        {
            var dao = new TblQuangCaoDao();
            using (var tran = new TransactionScope())
            {
                var paramDep = param.TblQuangCaos;
                foreach (var endep in paramDep)
                {
                    dao.Delete(endep);
                }
                tran.Complete();
            }
        }
        public void DeleteUpdate(TblQuangCaoParam param)
        {
            var dao = new TblQuangCaoDao();
            using (var tran = new TransactionScope())
            {
                var paramDep = param.TblQuangCaos;
                foreach (var endep in paramDep)
                {
                    dao.DeleteUpdate(endep);
                }
                tran.Complete();
            }
        }
        public void SetupDisplayForm(TblQuangCaoParam param)
        {
            var dao = new TblQuangCaoDao();
            param.TblQuangCaoInfo = dao.GetInfo(param.Id);

        }

        public void SetupEditForm(TblQuangCaoParam param)
        {
            var dao = new TblQuangCaoDao();
            param.TblQuangCao = dao.GetbyId(param.Id);
        }
        public void Search(TblQuangCaoParam param)
        {
            var dao = new TblQuangCaoDao();
            dao.Search(param);
        }
        public void GetAll(TblQuangCaoParam param)
        {
            var dao = new TblQuangCaoDao();
            dao.GetAll(param);
        }

        public void GetKhoaVung ( TblQuangCaoParam param)
        {
            var dao = new TblQuangCaoDao();
            dao.GetKhoaVung(param);
        }

    }
}
