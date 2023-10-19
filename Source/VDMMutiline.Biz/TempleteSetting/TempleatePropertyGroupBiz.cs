using System.Transactions;
using VDMMutiline.Dao.TempleteSetting;
using VDMMutiline.SharedComponent.Constants;
using VDMMutiline.SharedComponent.Params.TempleteSetting;
namespace VDMMutiline.Biz.TempleteSetting
{
    public class TempleatePropertyGroupBiz : BaseBiz
    {
        public void Insert(TempleatePropertyGroupParam param)
        {
            var dp = param.TempleatePropertyGroup;
            var dao = new TempleatePropertyGroupDao();
            dao.Insert(dp);
        }
        public void Update(TempleatePropertyGroupParam param)
        {
            var endep = param.TempleatePropertyGroup;
            var dao = new TempleatePropertyGroupDao();
            dao.Update(endep);
        }
        public void Delete(TempleatePropertyGroupParam param)
        {
            var dao = new TempleatePropertyGroupDao();
            using (var tran = new TransactionScope())
            {
                var paramDep = param.TempleatePropertyGroups;
                foreach (var endep in paramDep)
                {
                    dao.Delete(endep);
                }
                tran.Complete();
            }
        }
        public void SetupDisplayForm(TempleatePropertyGroupParam param)
        {
            var dao = new TempleatePropertyGroupDao();
            param.TempleatePropertyGroupInfo = dao.GetInfo(param.Id);

        }

        public void SetupEditForm(TempleatePropertyGroupParam param)
        {
            var dao = new TempleatePropertyGroupDao();
            param.TempleatePropertyGroup = dao.GetbyId(param.Id);
        }
        public void Search(TempleatePropertyGroupParam param)
        {
            var dao = new TempleatePropertyGroupDao();
            dao.Search(param);
        }
        public void SearchAllMenu(TempleatePropertyGroupParam param)
        {
            var dao = new TempleatePropertyGroupDao();
            dao.SearchAllMenu(param);
        }

        public void SearchListParam(TempleatePropertyGroupParam param)
        {
            var dao = new TempleatePropertyGroupDao();
            dao.SearchListParam(param);
        }

        public void GetAll(TempleatePropertyGroupParam param)
        {
            var dao = new TempleatePropertyGroupDao();
            dao.GetAll(param);
        }
        public void Prepare(TempleatePropertyGroupParam param)
        {
            var dao = new TempleatePropertyGroupDao();
            dao.PrepareTree(param);
        }
    }
}
