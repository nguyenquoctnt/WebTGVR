using System.Transactions;
using VDMMutiline.Dao;
using VDMMutiline.SharedComponent.Constants;
using VDMMutiline.SharedComponent.Params;
using VDMMutiline.SharedComponent.Entities;

namespace VDMMutiline.Biz
{
    public class MessageBiz
    {
        public void Insert(MessageParam param)
        {
            var dp = param.Message;
            dp.Daxoa = Constant.IsNotDeleted;
            var dao = new MessageDao();
            dao.Insert(dp);
        }
        public void Insert(Message param)
        {
            var dao = new MessageDao();
            dao.Insert(param);
        }
        public void Update(MessageParam param)
        {
            var endep = param.Message;
            var dao = new MessageDao();
            dao.Update(endep);
        }
        public void Delete(MessageParam param)
        {
            var dao = new MessageDao();
            using (var tran = new TransactionScope())
            {
                var paramDep = param.Messages;
                foreach (var endep in paramDep)
                {
                    dao.Delete(endep);
                }
                tran.Complete();
            }
        }
        public void DeleteUpdate(MessageParam param)
        {
            var dao = new MessageDao();
            using (var tran = new TransactionScope())
            {
                var paramDep = param.Messages;
                foreach (var endep in paramDep)
                {
                    dao.DeleteUpdate(endep);
                }
                tran.Complete();
            }
        }
        public void SetupDisplayForm(MessageParam param)
        {
            var dao = new MessageDao();
            param.MessageInfo = dao.GetInfo(param.Id);

        }

        public void SetupEditForm(MessageParam param)
        {
            var dao = new MessageDao();
            param.Message = dao.GetbyId(param.Id);
        }
        public void Search(MessageParam param)
        {
            var dao = new MessageDao();
            dao.Search(param);
        }
        public void GetAll(MessageParam param)
        {
            var dao = new MessageDao();
            dao.GetAll(param);
        }
        public void GetUnRead(MessageParam param)
        {
            var dao = new MessageDao();
            dao.GetUnRead(param);
        }
    }
}
