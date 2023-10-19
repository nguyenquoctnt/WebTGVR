using System.Transactions;
using VDMMutiline.Dao.TempleteSetting;
using VDMMutiline.SharedComponent.Constants;
using VDMMutiline.SharedComponent.EntityInfo.TempleteSetting;
using VDMMutiline.SharedComponent.Params.TempleteSetting;
namespace VDMMutiline.Biz.TempleteSetting
{
    public class TempleateHTMLUserBiz : BaseBiz
    {
        public void Insert(TempleateHTMLUserParam param)
        {
            var dp = param.TempleateHTMLUser;
            var dao = new TempleateHTMLUserDao();
            dao.Insert(dp);
        }
        public void Update(TempleateHTMLUserParam param)
        {
            var endep = param.TempleateHTMLUser;
            var dao = new TempleateHTMLUserDao();
            dao.Update(endep);
        }
        public void Delete(TempleateHTMLUserParam param)
        {
            var dao = new TempleateHTMLUserDao();
            using (var tran = new TransactionScope())
            {
                var paramDep = param.TempleateHTMLUsers;
                foreach (var endep in paramDep)
                {
                    dao.Delete(endep);
                }
                tran.Complete();
            }
        }
        public TempleateHTMLUserInfo SetupDisplayForm(int? Idtempleate, int? IsUser)
        {
            var dao = new TempleateHTMLUserDao();
            return dao.GetInfo(Idtempleate, IsUser);
        }
        public void Updatedata(TempleateHTMLUserParam param)
        {
            var dp = param.TempleateHTMLUser;
            var dao = new TempleateHTMLUserDao();
            dao.Updatedata(dp);
        }
        public void SetupEditForm(TempleateHTMLUserParam param)
        {
            var dao = new TempleateHTMLUserDao();
            param.TempleateHTMLUser = dao.GetbyId(param.Id);
        }
        public void SearchByUser(TempleateHTMLUserParam param)
        {
            var dao = new TempleateHTMLUserDao();
            dao.SearchByUser(param);
        }
    }
}
