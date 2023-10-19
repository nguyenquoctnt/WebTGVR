using System.Transactions;
using VDMMutiline.Dao;
using VDMMutiline.SharedComponent.Constants;
using VDMMutiline.SharedComponent.Params;
using VDMMutiline.SharedComponent.EntityInfo;

namespace VDMMutiline.Biz
{
    public class MessageRoomBiz
    {
        public void Insert(MessageRoomParam param)
        {
            var dp = param.MessageRoom;

            dp.Daxoa = Constant.IsNotDeleted;
            var dao = new MessageRoomDao();
            dao.Insert(dp);
        }
        public void Update(MessageRoomParam param)
        {
            var endep = param.MessageRoom;
            var dao = new MessageRoomDao();
            dao.Update(endep);
        }
        public void Delete(MessageRoomParam param)
        {
            var dao = new MessageRoomDao();
            using (var tran = new TransactionScope())
            {
                var paramDep = param.MessageRooms;
                foreach (var endep in paramDep)
                {
                    dao.Delete(endep);
                }
                tran.Complete();
            }
        }
        public void DeleteUpdate(MessageRoomParam param)
        {
            var dao = new MessageRoomDao();
            using (var tran = new TransactionScope())
            {
                var paramDep = param.MessageRooms;
                foreach (var endep in paramDep)
                {
                    dao.DeleteUpdate(endep);
                }
                tran.Complete();
            }
        }
        public void SetupDisplayForm(MessageRoomParam param)
        {
            var dao = new MessageRoomDao();
            param.MessageRoomInfo = dao.GetInfo(param.Id);

        }

        public void SetupEditForm(MessageRoomParam param)
        {
            var dao = new MessageRoomDao();
            param.MessageRoom = dao.GetbyId(param.Id);
        }
        public void Search(MessageRoomParam param)
        {
            var dao = new MessageRoomDao();
            dao.Search(param);
        }
        public void GetAll(MessageRoomParam param)
        {
            var dao = new MessageRoomDao();
            dao.GetAll(param);
        }
        public MessageRoomInfo GetDefaultRoomInfo()
        {
            var dao = new MessageRoomDao();
            return dao.GetDefaultRoomInfo();
        }
    }
}
