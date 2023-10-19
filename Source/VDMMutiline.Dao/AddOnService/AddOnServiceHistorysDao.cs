using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using VDMMutiline.SharedComponent.Constants;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo.AddOnService;
using VDMMutiline.SharedComponent.Params.AddOnService;
namespace VDMMutiline.Dao.VoidTicket
{
    public class AddOnServiceHistorysDao : BaseDao
    {
        public void Insert(AddOnServiceParam param)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                using (var tran = new TransactionScope())
                {
                    if (!param.isAdmin)
                    {
                        var objFirst = param.AddOnServiceHistorys.FirstOrDefault();
                        var TotalPrice = param.AddOnServiceHistorys.Sum(z=>z.Price);
                        var objUser = dbContext.AspNetUsers.FirstOrDefault(z => z.UserName == objFirst.CreateUser);
                        //if (objUser.IsIssueTicket == true)
                        //{
                        var objWalletSource = dbContext.tblWallets.FirstOrDefault(z => z.Id == objFirst.IdWallet);
                        if (objWalletSource != null)
                        {
                            if (objWalletSource.Amount >= TotalPrice)
                            {
                                objWalletSource.Amount = ((objWalletSource.Amount ?? 0) - (TotalPrice ?? 0));
                                dbContext.SubmitChanges();
                            }
                            else
                            {
                                throw new Exception("Tài khoản của bạn không đủ ");
                            }
                        }
                        else
                        {
                            throw new Exception("Tài khoản của bạn không đủ");
                        }
                        //}
                        //else
                        //{
                        //    throw new Exception("Bạn không có quyền xuất vé.");

                        //}
                    }
                    foreach (var item in param.AddOnServiceHistorys)
                    {
                        dbContext.tblAddOnServiceHistories.InsertOnSubmit(item);
                        dbContext.SubmitChanges();
                        var idIssueHistory = item.Id;
                    }
                    tran.Complete();
                }
            }
        }
        public void Search(AddOnServiceParam param)
        {
            var filter = param.AddOnServiceHistoryFilter;
            if (filter.FromDate.HasValue)
                filter.FromDate = filter.FromDate.Value.AddDays(-1);
            if (filter.ToDate.HasValue)
                filter.ToDate = filter.ToDate.Value.AddDays(-1);
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from issue in dbContext.tblAddOnServiceHistories
                            join user in dbContext.AspNetUsers on issue.CreateUser equals user.UserName
                            where (filter.FromDate.HasValue == false || issue.CreateDate > filter.FromDate)
                            && (filter.ToDate.HasValue == false || issue.CreateDate < filter.ToDate)
                            && (string.IsNullOrEmpty(filter.BookCode) || issue.BookingCode.Contains(filter.BookCode))
                            && (filter.UserList.Count == 0 || filter.UserList.Contains(user.UserName))
                            && (filter.ParentId.HasValue == false || (user.ParentId == filter.ParentId || user.Id == filter.ParentId))
                            && (filter.UserIdCreate.HasValue == false || (user.Id == filter.UserIdCreate || issue.CreateUser == filter.UserCreate))
                            && user.Isdelete == Constant.IsNotDeleted
                            select new AddOnServiceHistorysInfo
                            {
                                Id = issue.Id,
                                BookingCode = issue.BookingCode,
                                FullName = issue.FullName,
                                Status = issue.Status,
                                CodeBaggage = issue.CodeBaggage,
                                ValueBaggage = issue.ValueBaggage,
                                Price = issue.Price,
                                Type = issue.Type,
                                Remark = issue.Remark,
                                CreateDate = issue.CreateDate,
                                CreateUser = issue.CreateUser,
                                UserName = user.UserName,
                                DisplayName = user.DisplayName,
                                GoiHanhLy = issue.ValueBaggage + "Kg",
                            };

                if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.AddOnServiceHistorysInfos = query.OrderByDescending(z => z.CreateDate).Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).ToList();
                }
                else
                {
                    param.AddOnServiceHistorysInfos = query.OrderByDescending(z => z.CreateDate).ToList();
                }
            }
        }
    }
}
