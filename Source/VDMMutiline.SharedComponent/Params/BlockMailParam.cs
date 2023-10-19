using System.Collections.Generic;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;

namespace VDMMutiline.SharedComponent.Params
{
    public class BlockMailParam : BaseParam
    {
        #region Action
        public int Id { get; set; }
        public BlockMail BlockMail { get; set; }
        public List<BlockMail> BlockMails { get; set; }

        public BlockMailFilter BlockMailFilter { get; set; }
        #endregion
    }
    public class BlockMailFilter : BaseFilter
    {
        public int? Id { get; set; }
        public string phone { get; set; }
    }
}
