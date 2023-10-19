using VDMMutiline.Dao.VoidTicket;
using VDMMutiline.SharedComponent.Params.VoidTicket;
namespace VDMMutiline.Biz.VoidTicketHistory
{
    public class VoidTicketHistoryBiz : BaseBiz
    {
        public void Insert(VoidTicketHistoryParam param)
        {
            var dp = param.VoidTicketHistory;
            var dao = new VoidTicketHistorysDao();
            dao.Insert(dp, param.isAdmin);
        }
        public void Search(VoidTicketHistoryParam param)
        {
            var dao = new VoidTicketHistorysDao();
            dao.Search(param);
        }

    }
}
