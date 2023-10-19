using VDMMutiline.Dao;
using VDMMutiline.SharedComponent.Params;
using System.Transactions;
using VDMMutiline.SharedComponent.EntityInfo;
using VDMMutiline.SharedComponent.Entities;

namespace VDMMutiline.Biz
{
    public class NotifyBiz : BaseBiz
    {
        public void Insert(NotifyParam param)
        {
            var dp = param.Notification;
            var dao = new NotifyDao();
            dao.Insert(dp);
        }
        public void Update(NotifyParam param)
        {
            var endep = param.Notification;
            var dao = new NotifyDao();
            dao.Update(endep);
        }
        public void Delete(NotifyParam param)
        {
            var dao = new NotifyDao();
            using (var tran = new TransactionScope())
            {
                var paramDep = param.Notifications;
                foreach (var endep in paramDep)
                {
                    dao.Delete(endep);
                }
                tran.Complete();
            }
        }
        public void DeleteUpdate(NotifyParam param)
        {
            var dao = new NotifyDao();
            using (var tran = new TransactionScope())
            {
                var paramDep = param.Notifications;
                foreach (var endep in paramDep)
                {
                    dao.Delete(endep);
                }
                tran.Complete();
            }
        }

        public void Update(NotificationInfo byID)
        {
            //var endep = param.Notification;
            var dao = new NotifyDao();
            dao.Update(byID);
        }

        public void SetupDisplayForm(NotifyParam param)
        {
            var dao = new NotifyDao();
            param.NotificationInfo = dao.GetInfo(param.NotifyFilter.ItemId);

        }

        public void SetupEditForm(NotifyParam param)
        {
            var dao = new NotifyDao();
            param.Notification = dao.GetbyId(param.NotifyFilter.ItemId);
        }

        public NotificationInfo GetInfo(int value)
        {
            var notifyDao = new NotifyDao();
            return notifyDao.GetInfo(value);
        }

        public void Search(NotifyParam param)
        {
            var dao = new NotifyDao();
            dao.Search(param);
        }

        public void Delete(NotificationInfo byID)
        {
            var dao = new NotifyDao();
            dao.Delete(byID);
        }

        public void GetAll(NotifyParam param)
        {
            var dao = new NotifyDao();
            dao.GetAll(param);
        }

        public Notification GetbyId(int value)
        {
            var notifyDao = new NotifyDao();
            return notifyDao.GetbyId(value);
        }

        public void Insert(Notification notification)
        {
            var notifyDao = new NotifyDao();
            notifyDao.Insert(notification);
        }

        public void GetAllUnRead(NotifyParam param)
        {
            var notifyDao = new NotifyDao();
            notifyDao.GetAllUnRead(param);
        }

        public void GetAllIndex(NotifyParam param)
        {
            var notifyDao = new NotifyDao();
            notifyDao.GetAllIndex(param);
        }
        public void GetListByReceiver(NotifyParam param)
        {
            var notifyDao = new NotifyDao();
            notifyDao.GetListByReceiver(param);
        }
    }
}
