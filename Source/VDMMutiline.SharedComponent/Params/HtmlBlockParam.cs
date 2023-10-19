using System.Collections.Generic;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;

namespace VDMMutiline.SharedComponent.Params
{
    public class HtmlBlockParam : BaseParam
    {
        #region Action
        public int Id { get; set; }
        public  string key { get; set; }
        public tbl_HtmlBlock HtmlBlock { get; set; }
        public List<tbl_HtmlBlock> HtmlBlocks { get; set; }
        public HtmlBlockInfo HtmlBlockInfo { get; set; }
        public List<HtmlBlockInfo> HtmlBlockInfos { get; set; }
        public HtmlBlockFilter HtmlBlockFilter { get; set; }
        #endregion
    }
    public class HtmlBlockFilter : BaseFilter
    {
        public int? Id { get; set; }
        public List<string> ListUserName { get; set; }
    }
}
