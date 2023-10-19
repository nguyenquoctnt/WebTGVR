using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VDMMutiline.SharedComponent.Constants;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo.VoidTicket;
using VDMMutiline.SharedComponent.Params.CancelTicket;
namespace VDMMutiline.Dao.VoidTicket
{
    public class CancelTicketHistorysDao : BaseDao
    {
        public void Insert(tblCancelTicketHistory item, bool isAdmin)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                dbContext.tblCancelTicketHistories.InsertOnSubmit(item);
                dbContext.SubmitChanges();
            }
        }
        public void Search(CancelTicketHistoryParam param)
        {
            var filter = param.CancelTicketHistoryFilter;
            if (filter.FromDate.HasValue)
                filter.FromDate = filter.FromDate.Value.AddDays(-1);
            if (filter.ToDate.HasValue)
                filter.ToDate = filter.ToDate.Value.AddDays(-1);
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from issue in dbContext.tblCancelTicketHistories
                            join user in dbContext.AspNetUsers on issue.CreateUser equals user.UserName
                            where (filter.FromDate.HasValue == false || issue.CreateDate > filter.FromDate)
                            && (filter.ToDate.HasValue == false || issue.CreateDate < filter.ToDate)
                            && (string.IsNullOrEmpty(filter.BookCode) || issue.BookingCode.Contains(filter.BookCode))
                            && (filter.UserList.Count == 0 || filter.UserList.Contains(user.UserName))
                            && (filter.ParentId.HasValue == false || (user.ParentId == filter.ParentId || user.Id == filter.ParentId))
                            && (filter.UserIdCreate.HasValue == false || (user.Id == filter.UserIdCreate || issue.CreateUser == filter.UserCreate))
                            && user.Isdelete == Constant.IsNotDeleted
                            select new CancelTicketHistorysInfo
                            {
                                Id = issue.Id,
                                BookingCode = issue.BookingCode,
                                Status = issue.Status,
                                Remark = issue.Remark,
                                CreateDate = issue.CreateDate,
                                CreateUser = issue.CreateUser,
                                UserName = user.UserName,
                                DisplayName = user.DisplayName,
                            };

                if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.CancelTicketHistorysInfos = query.OrderByDescending(z => z.CreateDate).Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).ToList();
                }
                else
                {
                    param.CancelTicketHistorysInfos = query.OrderByDescending(z => z.CreateDate).ToList();
                }
            }
        }
    }
}
