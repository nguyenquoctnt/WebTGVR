using System.Transactions;
using VDMMutiline.Dao;
using VDMMutiline.SharedComponent.Params;

namespace VDMMutiline.Biz
{
    public class AspNetOnlineUsersBiz
    {
        public void Insert(AspNetOnlineUsersParam param)
        {
            var dp = param.AspNetOnlineUsers;
            var dao = new AspNetOnlineUsersDao();
            dao.Insert(dp);
        }
        public void Update(AspNetOnlineUsersParam param)
        {
            var endep = param.AspNetOnlineUsers;
            var dao = new AspNetOnlineUsersDao();
            dao.Update(endep);
        }
        public void Delete(AspNetOnlineUsersParam param)
        {
            var dao = new AspNetOnlineUsersDao();
            using (var tran = new TransactionScope())
            {
                var paramDep = param.AspNetOnlineUserss;
                foreach (var endep in paramDep)
                {
                    dao.Delete(endep);
                }
                tran.Complete();
            }
        }
        public void DeleteUpdate(AspNetOnlineUsersParam param)
        {
            var dao = new AspNetOnlineUsersDao();
            using (var tran = new TransactionScope())
            {
                var paramDep = param.AspNetOnlineUserss;
                foreach (var endep in paramDep)
                {
                    dao.DeleteUpdate(endep);
                }
                tran.Complete();
            }
        }
        public void SetupDisplayForm(AspNetOnlineUsersParam param)
        {
            var dao = new AspNetOnlineUsersDao();
            param.AspNetOnlineUsersInfo = dao.GetInfo(param.Id);

        }

        public void SetupEditForm(AspNetOnlineUsersParam param)
        {
            var dao = new AspNetOnlineUsersDao();
            param.AspNetOnlineUsers = dao.GetbyId(param.Id);
        }
        public void Search(AspNetOnlineUsersParam param)
        {
            var dao = new AspNetOnlineUsersDao();
            dao.Search(param);
        }
        public void GetAll(AspNetOnlineUsersParam param)
        {
            var dao = new AspNetOnlineUsersDao();
            dao.GetAll(param);
        }
    }
}
