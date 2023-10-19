using System.Transactions;
using VDMMutiline.Dao;
using VDMMutiline.SharedComponent.Constants;
using VDMMutiline.SharedComponent.Params;

namespace VDMMutiline.Biz
{
    public class SiteInfoBiz
    {
        public void Insert(SiteInfoParam param)
        {
            var dp = param.tblSiteInfo;
            var dao = new SiteInfoDao();
            dao.Insert(dp);
        }
        public void CopyContenSite(int? siteid, string username)
        {
            var dao = new SiteInfoDao();
            dao.CopyContenSite(siteid, username);
        }
        public void Update(SiteInfoParam param)
        {
            var endep = param.tblSiteInfo;
            var dao = new SiteInfoDao();
            dao.Update(endep);
        }
        public void Delete(SiteInfoParam param)
        {
            var dao = new SiteInfoDao();
            using (var tran = new TransactionScope())
            {
                var paramDep = param.tblSiteInfos;
                foreach (var endep in paramDep)
                {
                    dao.Delete(endep);
                }
                tran.Complete();
            }
        }
        public void SetupDisplayForm(SiteInfoParam param)
        {
            var dao = new SiteInfoDao();
            param.SiteInfo = dao.GetInfo(param.Id);

        }

        public void SetupEditForm(SiteInfoParam param)
        {
            var dao = new SiteInfoDao();
            param.tblSiteInfo = dao.GetbyId(param.Id);
        }
        public void Search(SiteInfoParam param)
        {
            var dao = new SiteInfoDao();
            dao.Search(param);
        }
        public void GetStyleBySiteID(SiteInfoParam param)
        {
            var dao = new SiteInfoDao();
            dao.GetStyleBySiteID(param);
        }
    }
}
