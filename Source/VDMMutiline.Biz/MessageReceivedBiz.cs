using System.Transactions;
using VDMMutiline.Dao;
using VDMMutiline.SharedComponent.Params;

namespace VDMMutiline.Biz
{
    public class MessageReceivedBiz
    {
        public void Insert(MessageReceivedParam param)
        {
            var dp = param.MessageReceived;
           
            var dao = new MessageReceivedDao();
            dao.Insert(dp);
        }
        public void Update(MessageReceivedParam param)
        {
            var endep = param.MessageReceived;
            var dao = new MessageReceivedDao();
            dao.Update(endep);
        }
        public void Delete(MessageReceivedParam param)
        {
            var dao = new MessageReceivedDao();
            using (var tran = new TransactionScope())
            {
                var paramDep = param.MessageReceiveds;
                foreach (var endep in paramDep)
                {
                    dao.Delete(endep);
                }
                tran.Complete();
            }
        }
        public void DeleteUpdate(MessageReceivedParam param)
        {
            var dao = new MessageReceivedDao();
            using (var tran = new TransactionScope())
            {
                var paramDep = param.MessageReceiveds;
                foreach (var endep in paramDep)
                {
                    dao.DeleteUpdate(endep);
                }
                tran.Complete();
            }
        }
        public void SetupDisplayForm(MessageReceivedParam param)
        {
            var dao = new MessageReceivedDao();
            param.MessageReceivedInfo = dao.GetInfo(param.Id);

        }

        public void SetupEditForm(MessageReceivedParam param)
        {
            var dao = new MessageReceivedDao();
            param.MessageReceived = dao.GetbyId(param.Id);
        }
        public void Search(MessageReceivedParam param)
        {
            var dao = new MessageReceivedDao();
            dao.Search(param);
        }
        public void GetAll(MessageReceivedParam param)
        {
            var dao = new MessageReceivedDao();
            dao.GetAll(param);
        }
    }
}
