using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VDMMutiline.SharedComponent.Entities;

namespace VDMMutiline.SharedComponent.EntityInfo.Wallet
{
    public class WalletInfo:tblWallet
    {
        public int? WalletId { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string UrlDomain2 { get; set; }
        public string UrlDomain1 { get; set; }
        public string Taikhoancha { get; set; }
        public string AmountString { get; set; }
        public int? ParentId { get; set; }
        public DateTime? CreateDateUser { get; set; }
    }
}