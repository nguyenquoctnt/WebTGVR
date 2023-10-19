using System.Transactions;
using VDMMutiline.Dao;
using VDMMutiline.SharedComponent.Params;
namespace VDMMutiline.Biz
{
    public class BaggageBiz
    {
        public void Insert(BaggageParam param)
        {
            var dp = param.Baggage;
            var dao = new BaggageDao();
            dao.Insert(dp);
        }
        public void Update(BaggageParam param)
        {
            var endep = param.Baggage;
            var dao = new BaggageDao();
            dao.Update(endep);
        }
        public void Delete(BaggageParam param)
        {
            var dao = new BaggageDao();
            using (var tran = new TransactionScope())
            {
                var paramDep = param.Baggages;
                foreach (var endep in paramDep)
                {
                    dao.Delete(endep);
                }
                tran.Complete();
            }
        }
        public void SetupDisplayForm(BaggageParam param)
        {
            var dao = new BaggageDao();
            param.BaggageInfo = dao.GetInfo(param.Id);

        }
        public void SetupEditForm(BaggageParam param)
        {
            var dao = new BaggageDao();
            param.Baggage = dao.GetbyID(param.Id);
        }
        public void Search(BaggageParam param)
        {
            var dao = new BaggageDao();
            dao.Search(param);
        }
    }
}
