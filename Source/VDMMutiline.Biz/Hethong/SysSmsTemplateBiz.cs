using System.Transactions;
using VDMMutiline.Dao;
using VDMMutiline.SharedComponent.Params;

namespace VDMMutiline.Biz
{
    public class SysSmsTemplateBiz
    {
        public void Insert(SysSmsTemplateParam param)
        {
            var dp = param.SysSmsTemplate;
           
            var dao = new SysSmsTemplateDao();
            dao.Insert(dp);
        }
        public void Update(SysSmsTemplateParam param)
        {
            var endep = param.SysSmsTemplate;
            var dao = new SysSmsTemplateDao();
            dao.Update(endep);
        }
        public void Delete(SysSmsTemplateParam param)
        {
            var dao = new SysSmsTemplateDao();
            using (var tran = new TransactionScope())
            {
                var paramDep = param.SysSmsTemplates;
                foreach (var endep in paramDep)
                {
                    dao.Delete(endep);
                }
                tran.Complete();
            }
        }
        public void DeleteUpdate(SysSmsTemplateParam param)
        {
            var dao = new SysSmsTemplateDao();
            using (var tran = new TransactionScope())
            {
                var paramDep = param.SysSmsTemplates;
                foreach (var endep in paramDep)
                {
                    dao.DeleteUpdate(endep);
                }
                tran.Complete();
            }
        }
        public void SetupDisplayForm(SysSmsTemplateParam param)
        {
            var dao = new SysSmsTemplateDao();
            param.SysSmsTemplateInfo = dao.GetInfo(param.Id);

        }

        public void SetupEditForm(SysSmsTemplateParam param)
        {
            var dao = new SysSmsTemplateDao();
            param.SysSmsTemplate = dao.GetbyId(param.Id);
        }
        public void Search(SysSmsTemplateParam param)
        {
            var dao = new SysSmsTemplateDao();
            dao.Search(param);
        }
        public void GetAll(SysSmsTemplateParam param)
        {
            var dao = new SysSmsTemplateDao();
            dao.GetAll(param);
        }
    }
}
