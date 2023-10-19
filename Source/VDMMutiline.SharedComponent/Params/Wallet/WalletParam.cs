using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
using VDMMutiline.SharedComponent.EntityInfo.Wallet;

namespace VDMMutiline.SharedComponent.Params.Wallet
{
    public class WalletParam : BaseParam
    {
        public int IdWallet { get; set; }
        public int IdWalletHistory { get; set; }
        public int UserID { get; set; }
        public int UserIDSource { get; set; }
        public bool IsAdmin { get; set; }
        public tblWallet Wallet { get; set; }
        public List<tblWallet> Wallets { get; set; }
        public WalletInfo WalletInfo { get; set; }
        public List<WalletInfo> WalletInfos { get; set; }
        public tblWalletHistory WalletHistory { get; set; }
        public List<tblWalletHistory> WalletHistorys { get; set; }
        public WalletHistorysInfo WalletHistorysInfo { get; set; }
        public List<WalletHistorysInfo> WalletHistorysInfos { get; set; }
        public List<AspNetUserInfo> AspNetUserInfos { get; set; }
        public WalletFilter WalletFilter { get; set; }
    }
    public class WalletFilter : BaseFilter
    {
        public List<string> UserList { get; set; }
        public int? ParentId { get; set; }
        public int? UserIdCreate { get; set; }
        public string UserCreate { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string NumberOfDocuments { get; set; }
    }
}