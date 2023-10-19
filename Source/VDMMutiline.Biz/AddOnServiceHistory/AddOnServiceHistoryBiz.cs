using VDMMutiline.Dao.VoidTicket;
using VDMMutiline.SharedComponent.Params.AddOnService;

namespace VDMMutiline.Biz.AddOnServiceHistory
{
    public class AddOnServiceHistoryBiz : BaseBiz
    {
        public void Insert(AddOnServiceParam param)
        {
            var dp = param.AddOnServiceHistory;
            var dao = new AddOnServiceHistorysDao();
            dao.Insert(param);
        }
        public void Search(AddOnServiceParam param)
        {
            var dao = new AddOnServiceHistorysDao();
            dao.Search(param);
        }

    }
}
