using System.Collections.Generic;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
namespace VDMMutiline.SharedComponent.Params
{
    public class SiteInfoParam : BaseParam
    {
        #region Action
        public int Id { get; set; }
        public tbl_SiteInfo tblSiteInfo { get; set; }

        public List<tbl_SiteInfo> tblSiteInfos { get; set; }

        public SiteInfo SiteInfo { get; set; }

        public List<SiteInfo> SiteInfos { get; set; }

        public SiteInfoFilter SiteInfoFilter { get; set; }
        public List<tbl_StyleSite> StyleSites { get; set; }
        #endregion
    }
    public class SiteInfoFilter : BaseFilter
    {
        public int? Id { get; set; }
    }

}