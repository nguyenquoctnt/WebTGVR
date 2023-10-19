using System.Transactions;
using VDMMutiline.Dao.TempleteSetting;
using VDMMutiline.SharedComponent.Constants;
using VDMMutiline.SharedComponent.Params.TempleteSetting;
namespace VDMMutiline.Biz.TempleteSetting
{
    public class TempleateHTMLBiz : BaseBiz
    {
        public void Insert(TempleateHTMLParam param)
        {
            var dp = param.TempleateHTML;
            var dao = new TempleateHTMLDao();
            dao.Insert(dp);
        }
        public void Update(TempleateHTMLParam param)
        {
            var endep = param.TempleateHTML;
            var dao = new TempleateHTMLDao();
            dao.Update(endep);
        }
        public void Delete(TempleateHTMLParam param)
        {
            var dao = new TempleateHTMLDao();
            using (var tran = new TransactionScope())
            {
                var paramDep = param.TempleateHTMLs;
                foreach (var endep in paramDep)
                {
                    dao.Delete(endep);
                }
                tran.Complete();
            }
        }
        public void SetupDisplayForm(TempleateHTMLParam param)
        {
            var dao = new TempleateHTMLDao();
            param.TempleateHTMLInfo = dao.GetInfo(param.Id);

        }

        public void SetupEditForm(TempleateHTMLParam param)
        {
            var dao = new TempleateHTMLDao();
            param.TempleateHTML = dao.GetbyId(param.Id);
        }
        public void Search(TempleateHTMLParam param)
        {
            var dao = new TempleateHTMLDao();
            dao.Search(param);
        }
        public void GetAll(TempleateHTMLParam param)
        {
            var dao = new TempleateHTMLDao();
            dao.GetAll(param);
        }
        
    }
}
