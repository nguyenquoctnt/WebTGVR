using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VDMMutiline.Dao;
using VDMMutiline.Dao.Issue;
using VDMMutiline.SharedComponent.EntityInfo.Issue;
using VDMMutiline.SharedComponent.Params;
using VDMMutiline.SharedComponent.Params.Issue;

namespace VDMMutiline.Biz.Issue
{
    public class IssueHistoryBiz : BaseBiz
    {
        public void Insert(IssueParam param)
        {
            var dao = new IssueHistorysDao();
            dao.Insert(param.IssueHistorys, param.isAdmin);
        }
        public void Search(IssueParam param)
        {
            var dao = new IssueHistorysDao();
            dao.Search(param);
        }
        public IssueHistorysInfo GetInfoByCode(string BookingCode)
        {
            var dao = new IssueHistorysDao();
            return dao.GetInfoByCode(BookingCode);
        }
    }
}
