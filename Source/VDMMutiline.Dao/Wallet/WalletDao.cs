using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using VDMMutiline.SharedComponent.Constants;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo.Wallet;
using VDMMutiline.SharedComponent.Params.Wallet;

namespace VDMMutiline.Dao.Wallet
{
    public class WalletDao : BaseDao
    {
        #region Action
        public void Update(tblWalletHistory walletHistory, int IdUser, int UserSource, bool isAdmin)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                using (var tran = new TransactionScope())
                {
                    var objCheckExitsWallet = dbContext.tblWallets.FirstOrDefault(z => z.IdUser == IdUser);
                    var idWallet = 0;
                    if (objCheckExitsWallet == null)
                    {
                        var wallet = new tblWallet()
                        {
                            IdUser = IdUser,
                            Amount = walletHistory.Amount,
                            CreateDate = DateTime.Now,
                            CreateUser = walletHistory.CreateUser,
                            UpdateDate = DateTime.Now,
                            UpdateUser = walletHistory.CreateUser
                        };
                        dbContext.tblWallets.InsertOnSubmit(wallet);
                        dbContext.SubmitChanges();
                        idWallet = wallet.Id;
                    }
                    else
                    {
                        idWallet = objCheckExitsWallet.Id;
                        objCheckExitsWallet.Amount = ((objCheckExitsWallet.Amount ?? 0) + (walletHistory.Amount ?? 0));
                        objCheckExitsWallet.UpdateDate = DateTime.Now;
                        objCheckExitsWallet.UpdateUser = walletHistory.UpdateUser;
                        dbContext.SubmitChanges();
                    }
                    walletHistory.IdWallet = idWallet;
                    dbContext.tblWalletHistories.InsertOnSubmit(walletHistory);
                    dbContext.SubmitChanges();
                    if (!isAdmin)
                    {
                        var objWalletSource = dbContext.tblWallets.FirstOrDefault(z => z.IdUser == UserSource);
                        if(objWalletSource!=null)
                        {
                            if(objWalletSource.Amount>= walletHistory.Amount)
                            {
                                objWalletSource.Amount = (objWalletSource.Amount ?? 0) - (walletHistory.Amount ?? 0);
                                dbContext.SubmitChanges();
                            }
                            else
                            {
                                throw new Exception("Tài khoản của bạn không đủ ký quỹ ");
                            }
                        }
                        else
                        {
                            throw new Exception("Tài khoản của bạn không đủ ký quỹ ");
                        }
                    }
                    tran.Complete();
                }
            }
        }
        #endregion
        #region Query
        public bool CheckKyQuy(int? IdUser, decimal? Amount)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var obj = dbContext.tblWallets.FirstOrDefault(z => z.IdUser == IdUser);
                if (obj != null)
                {
                    if ((obj.Amount ?? 0) > Amount)
                        return true;
                }
                return false;
            }
        }

        public tblWallet GetbyId(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.tblWallets
                            where n.Id == id
                            select n;
                return query.FirstOrDefault();
            }
        }
        public WalletInfo GetInfo(int idUser)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.AspNetUsers
                            join wallet in dbContext.tblWallets on n.Id equals wallet.IdUser into p
                            from wallet in p.DefaultIfEmpty()
                            where n.Id == idUser
                            select new WalletInfo()
                            {
                                Id = wallet==null?0: wallet.Id,
                                IdUser = n.Id,
                                UserName = n.UserName,
                                DisplayName = n.DisplayName,
                                Email = n.Email,
                                Phone = n.PhoneNumber + " - " + n.PhoneNumber2,
                                Amount = wallet.Amount,
                                CreateDate = wallet.CreateDate,
                                CreateUser = wallet.CreateUser,
                                UpdateDate = wallet.UpdateDate,
                                UpdateUser = wallet.UpdateUser
                            };
                return query.FirstOrDefault();
            }
        }
        public void SearchWallet(WalletParam param)
        {
            var filter = param.WalletFilter;
            var listcontennotshow = System.Configuration.ConfigurationSettings.AppSettings.Get("Listcontenotshow").Split(';').ToList();
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.AspNetUsers
                            join wallet in dbContext.tblWallets on n.Id equals wallet.IdUser into p
                            from wallet in p.DefaultIfEmpty()
                            where (string.IsNullOrEmpty(filter.Search) || n.UserName.Contains(filter.Search) || n.DisplayName.Contains(filter.Search))
                             && n.Isdelete == Constant.IsNotDeleted
                             && (filter.UserList.Count == 0 || filter.UserList.Contains(n.UserName))
                             && (filter.ParentId.HasValue == false || (n.ParentId == filter.ParentId || n.Id == filter.ParentId))
                             && (listcontennotshow.Contains(n.UserName) == false)
                            select new WalletInfo
                            {
                                WalletId = wallet.Id,
                                IdUser = n.Id,
                                UserName = n.UserName,
                                DisplayName = n.DisplayName,
                                Phone = n.PhoneNumber + " - " + n.PhoneNumber2,
                                Email = n.Email,
                                Amount = wallet.Amount,
                                CreateDate = wallet.CreateDate,
                                CreateUser = wallet.CreateUser,
                                UpdateDate = wallet.UpdateDate,
                                UpdateUser = wallet.UpdateUser,
                                UrlDomain1 = n.UrlDomain1,
                                UrlDomain2 = n.UrlDomain2,
                                ParentId = n.ParentId,
                                CreateDateUser=n.RegisterDate,
                            };

                if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.WalletInfos = query.OrderBy(z => z.CreateDateUser).Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).ToList();

                }
                else
                {
                    param.WalletInfos = query.OrderBy(z => z.CreateDateUser).ToList();
                }
                foreach (var item in param.WalletInfos.Where(a => a.ParentId != null))
                {
                    var objcha = dbContext.AspNetUsers.FirstOrDefault(z => z.Id == item.ParentId);
                    if (objcha != null)
                    {
                        item.Taikhoancha = objcha.UserName;
                    }
                }
            }
        }
        public void SearchHistorys(WalletParam param)
        {
            var filter = param.WalletFilter;
            if (filter.FromDate.HasValue)
                filter.FromDate = filter.FromDate.Value.AddDays(-1);
            if(filter.ToDate.HasValue)
                filter.ToDate = filter.ToDate.Value.AddDays(-1);
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from wallet in dbContext.tblWallets
                            join user in dbContext.AspNetUsers on wallet.IdUser equals user.Id
                            join walletHistory in dbContext.tblWalletHistories on wallet.Id equals walletHistory.IdWallet
                            where (filter.FromDate.HasValue == false || walletHistory.CreateDate > filter.FromDate)
                            && (filter.ToDate.HasValue == false || walletHistory.CreateDate < filter.ToDate)
                            && (string.IsNullOrEmpty(filter.NumberOfDocuments) || walletHistory.NumberOfDocuments.Contains(filter.NumberOfDocuments))
                            && (filter.UserList.Count == 0 || filter.UserList.Contains(user.UserName))
                            && (filter.ParentId.HasValue == false || (user.ParentId == filter.ParentId || user.Id == filter.ParentId ))
                            && (filter.UserIdCreate.HasValue==false || (wallet.IdUser== filter.UserIdCreate || walletHistory.CreateUser==filter.UserCreate))
                            && user.Isdelete == Constant.IsNotDeleted
                            select new WalletHistorysInfo
                            {
                                Id = walletHistory.Id,
                                IdUser = wallet.IdUser,
                                IdWallet = wallet.Id,
                                Datetime = walletHistory.Datetime,
                                Amount = walletHistory.Amount,
                                Phone = user.PhoneNumber + " - " + user.PhoneNumber2,
                                NumberOfDocuments = walletHistory.NumberOfDocuments,
                                Note = walletHistory.Note,
                                UserName = user.UserName,
                                DisplayName = user.DisplayName,
                                Email = user.Email,
                                CreateDate = walletHistory.CreateDate,
                                CreateUser = walletHistory.CreateUser,
                                UpdateDate = walletHistory.UpdateDate,
                                UpdateUser = walletHistory.UpdateUser
                            };

                if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.WalletHistorysInfos = query.OrderByDescending(z => z.CreateDate).Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).ToList();
                }
                else
                {
                    param.WalletHistorysInfos = query.OrderByDescending(z => z.CreateDate).ToList();
                }
            }
        }
        #endregion
    }
}
