using System.Collections.Generic;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
namespace VDMMutiline.SharedComponent.Params
{
    public class TblQuangCaoParam:BaseParam
    {
        #region Action
        public int Id { get; set; }
        public TblQuangCao TblQuangCao { get; set; }

        public List<TblQuangCao> TblQuangCaos { get; set; }

        public TblQuangCaoInfo TblQuangCaoInfo { get; set; }

        public List<TblQuangCaoInfo> TblQuangCaoInfos { get; set; }

        public TblQuangCaoFilter TblQuangCaoFilter { get; set; }


        public List<SysVungHienthiInfo> SysVungHienthiInfos { get; set; }
        #endregion
    }
    public class TblQuangCaoFilter : BaseFilter
    {
        public List<string> ListUserName { get; set; }
        public int? Id { get; set; }
        public int? Vung { get; set; }

    }

}