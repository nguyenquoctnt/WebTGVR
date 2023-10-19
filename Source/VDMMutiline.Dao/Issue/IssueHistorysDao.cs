using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using VDMMutiline.SharedComponent.Constants;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo.Issue;
using VDMMutiline.SharedComponent.Params.Issue;

namespace VDMMutiline.Dao.Issue
{
    public class IssueHistorysDao : BaseDao
    {
        public void Insert(List<tblIssueHistory> lst, bool isAdmin)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                using (var tran = new TransactionScope())
                {
                    foreach (var item in lst)
                    {
                        dbContext.tblIssueHistories.InsertOnSubmit(item);
                        dbContext.SubmitChanges();
                        var idIssueHistory = item.Id;
                        var objUser = dbContext.AspNetUsers.FirstOrDefault(z => z.UserName == item.CreateUser);
                        if (!isAdmin)
                        {
                            if (objUser.IsIssueTicket == true)
                            {
                                var objWalletSource = dbContext.tblWallets.FirstOrDefault(z => z.Id == item.IdWallet);
                                if (objWalletSource != null)
                                {
                                    if (objWalletSource.Amount >= item.Price)
                                    {
                                        objWalletSource.Amount = ((objWalletSource.Amount ?? 0) - (item.Price ?? 0));
                                        dbContext.SubmitChanges();
                                    }
                                    else
                                    {
                                        throw new Exception("Tài khoản của bạn không đủ xuất vé ");
                                    }
                                }
                                else
                                {
                                    throw new Exception("Tài khoản của bạn không đủ xuất vé ");
                                }
                            }
                            else
                            {
                                throw new Exception("Bạn không có quyền xuất vé.");

                            }
                        }
                    }
                    tran.Complete();
                }
            }
        }
        public IssueHistorysInfo GetInfoByCode(string BookingCode)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from issue in dbContext.tblIssueHistories
                            join user in dbContext.AspNetUsers on issue.CreateUser equals user.UserName
                            where issue.BookingCode== BookingCode
                            select new IssueHistorysInfo
                            {
                                Id = issue.Id,
                                BookingCode = issue.BookingCode,
                                Status = issue.Status,
                                Price = issue.Price,
                                Remark = issue.Remark,
                                CreateDate = issue.CreateDate,
                                CreateUser = issue.CreateUser,
                                UserName = user.UserName,
                                DisplayName = user.DisplayName,
                                TicketNumber = issue.TicketNumber,
                            };
                return query.FirstOrDefault();
            }
        }
        public void Search(IssueParam param)
        {
            var filter = param.IssueFilter;
            if (filter.FromDate.HasValue)
                filter.FromDate = filter.FromDate.Value.AddDays(-1);
            if (filter.ToDate.HasValue)
                filter.ToDate = filter.ToDate.Value.AddDays(-1);
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from issue in dbContext.tblIssueHistories
                            join user in dbContext.AspNetUsers on issue.CreateUser equals user.UserName
                            where (filter.FromDate.HasValue == false || issue.CreateDate > filter.FromDate)
                            && (filter.ToDate.HasValue == false || issue.CreateDate < filter.ToDate)
                            && (string.IsNullOrEmpty(filter.BookCode) || issue.BookingCode.Contains(filter.BookCode))
                            && (filter.UserList.Count == 0 || filter.UserList.Contains(user.UserName))
                            && (filter.ParentId.HasValue == false || (user.ParentId == filter.ParentId || user.Id == filter.ParentId))
                            && (filter.UserIdCreate.HasValue == false || (user.Id == filter.UserIdCreate || issue.CreateUser == filter.UserCreate))
                            && user.Isdelete == Constant.IsNotDeleted
                            select new IssueHistorysInfo
                            {
                                Id = issue.Id,
                                BookingCode = issue.BookingCode,
                                Status = issue.Status,
                                Price = issue.Price,
                                Remark = issue.Remark,
                                CreateDate = issue.CreateDate,
                                CreateUser = issue.CreateUser,
                                UserName = user.UserName,
                                DisplayName = user.DisplayName,
                                TicketNumber = issue.TicketNumber,
                            };

                if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.IssueHistorysInfos = query.OrderByDescending(z => z.CreateDate).Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).ToList();
                }
                else
                {
                    param.IssueHistorysInfos = query.OrderByDescending(z => z.CreateDate).ToList();
                }
            }
        }
    }
}
