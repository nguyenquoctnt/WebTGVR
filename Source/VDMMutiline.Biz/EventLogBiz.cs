using System.Transactions;
using VDMMutiline.Dao;
using VDMMutiline.SharedComponent.Params;

namespace VDMMutiline.Biz
{
    public class EventLogBiz
    {
        public void Insert(EventLogParam param)
        {
            var dp = param.EventLog;
           
            var dao = new EventLogDao();
            dao.Insert(dp);
        }
        public void Update(EventLogParam param)
        {
            var endep = param.EventLog;
            var dao = new EventLogDao();
            dao.Update(endep);
        }
        public void Delete(EventLogParam param)
        {
            var dao = new EventLogDao();
            using (var tran = new TransactionScope())
            {
                var paramDep = param.EventLogs;
                foreach (var endep in paramDep)
                {
                    dao.Delete(endep);
                }
                tran.Complete();
            }
        }
        public void SetupDisplayForm(EventLogParam param)
        {
            var dao = new EventLogDao();
            param.EventLogInfo = dao.GetInfo(param.Id);

        }

        public void SetupEditForm(EventLogParam param)
        {
            var dao = new EventLogDao();
            param.EventLog = dao.GetbyId(param.Id);
        }
        public void Search(EventLogParam param)
        {
            var dao = new EventLogDao();
            dao.Search(param);
        }
        public void GetAll(EventLogParam param)
        {
            var dao = new EventLogDao();
            dao.GetAll(param);
        }
    }
}
