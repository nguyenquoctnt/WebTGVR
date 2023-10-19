using System.Transactions;
using VDMMutiline.Dao.TempleteSetting;
using VDMMutiline.SharedComponent.Constants;
using VDMMutiline.SharedComponent.Params.TempleteSetting;
namespace VDMMutiline.Biz.TempleteSetting
{
    public class TempleatePropertyUserUserBiz : BaseBiz
    {
        public void Insert(TempleatePropertyUserParam param)
        {
            var dp = param.TempleatePropertyUser;
            var dao = new TempleatePropertyUserDao();
            dao.Insert(dp);
        }
        public void Update(TempleatePropertyUserParam param)
        {
            var endep = param.TempleatePropertyUser;
            var dao = new TempleatePropertyUserDao();
            dao.Update(endep);
        }
        public void Delete(TempleatePropertyUserParam param)
        {
            var dao = new TempleatePropertyUserDao();
            using (var tran = new TransactionScope())
            {
                var paramDep = param.TempleatePropertyUsers;
                foreach (var endep in paramDep)
                {
                    dao.Delete(endep);
                }
                tran.Complete();
            }
        }
        public void SetupDisplayForm(TempleatePropertyUserParam param)
        {
            var dao = new TempleatePropertyUserDao();
            param.TempleatePropertyUserInfo = dao.GetInfo(param.Id);

        }
        public void Updatedata(TempleatePropertyUserParam param)
        {
            var dp = param.TempleatePropertyUser;
            var dao = new TempleatePropertyUserDao();
            dao.Updatedata(dp);
        }
        
        public void SetupEditForm(TempleatePropertyUserParam param)
        {
            var dao = new TempleatePropertyUserDao();
            param.TempleatePropertyUser = dao.GetbyId(param.Id);
        }
        public void SearchByUser(TempleatePropertyUserParam param)
        {
            var dao = new TempleatePropertyUserDao();
            dao.SearchByUser(param);
        }
      
        
    }
}
