using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VDMMutiline.SharedComponent.Entities;

namespace VDMMutiline.SharedComponent.EntityInfo.Wallet
{
    public class WalletHistorysInfo: tblWalletHistory
    {
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public int? IdUser { get; set; }
        public string Phone { get; set; }
        public string AmountStr { get; set; }
    }
}