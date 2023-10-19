using System.Collections.Generic;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
namespace VDMMutiline.SharedComponent.Params
{
    public class AspNetUserClaimsParam:BaseParam
    {
        #region Action
        public int Id { get; set; }
        public AspNetUserClaim AspNetUserClaims { get; set; }

        public List<AspNetUserClaim> AspNetUserClaimss { get; set; }

        public AspNetUserClaimsInfo AspNetUserClaimsInfo { get; set; }

        public List<AspNetUserClaimsInfo> AspNetUserClaimsInfos { get; set; }

        public AspNetUserClaimsFilter AspNetUserClaimsFilter { get; set; }
        #endregion
    }
    public class AspNetUserClaimsFilter : BaseFilter
    {
        public int? Id { get; set; }
    }

}