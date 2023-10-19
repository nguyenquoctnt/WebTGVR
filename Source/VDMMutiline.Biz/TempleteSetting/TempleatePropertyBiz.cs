using System.Transactions;
using VDMMutiline.Dao.TempleteSetting;
using VDMMutiline.SharedComponent.Constants;
using VDMMutiline.SharedComponent.Params.TempleteSetting;
namespace VDMMutiline.Biz.TempleteSetting
{
    public class TempleatePropertyBiz : BaseBiz
    {
        public void Insert(TempleatePropertyParam param)
        {
            var dp = param.TempleateProperty;
            var dao = new TempleatePropertyDao();
            dao.Insert(dp);
        }
        public void Update(TempleatePropertyParam param)
        {
            var endep = param.TempleateProperty;
            var dao = new TempleatePropertyDao();
            dao.Update(endep);
        }
        public void Delete(TempleatePropertyParam param)
        {
            var dao = new TempleatePropertyDao();
            using (var tran = new TransactionScope())
            {
                var paramDep = param.TempleatePropertys;
                foreach (var endep in paramDep)
                {
                    dao.Delete(endep);
                }
                tran.Complete();
            }
        }
        public void SetupDisplayForm(TempleatePropertyParam param)
        {
            var dao = new TempleatePropertyDao();
            param.TempleatePropertyInfo = dao.GetInfo(param.Id);

        }

        public void SetupEditForm(TempleatePropertyParam param)
        {
            var dao = new TempleatePropertyDao();
            param.TempleateProperty = dao.GetbyId(param.Id);
        }
        public void Search(TempleatePropertyParam param)
        {
            var dao = new TempleatePropertyDao();
            dao.Search(param);
        }
        public void GetAll(TempleatePropertyParam param)
        {
            var dao = new TempleatePropertyDao();
            dao.GetAll(param);
        }
        
    }
}
