using System.Transactions;
using VDMMutiline.Dao;
using VDMMutiline.SharedComponent.Params;
namespace VDMMutiline.Biz
{
    public class AirlineBiz
    {
        public void Insert(AirlineParam param)
        {
            var dp = param.Airline;
            var dao = new AirlineDao();
            dao.Insert(dp);
        }
        public void Update(AirlineParam param)
        {
            var endep = param.Airline;
            var dao = new AirlineDao();
            dao.Update(endep);
        }
        public void Delete(AirlineParam param)
        {
            var dao = new AirlineDao();
            using (var tran = new TransactionScope())
            {
                var paramDep = param.Airlines;
                foreach (var endep in paramDep)
                {
                    dao.Delete(endep);
                }
                tran.Complete();
            }
        }
        public void SetupDisplayForm(AirlineParam param)
        {
            var dao = new AirlineDao();
            param.AirlineInfo = dao.GetInfo(param.Id);

        }

        public void SetupEditForm(AirlineParam param)
        {
            var dao = new AirlineDao();
            param.Airline = dao.GetbyID(param.Id);
        }
        public void Search(AirlineParam param)
        {
            var dao = new AirlineDao();
            dao.Search(param);
        }

    }
}
