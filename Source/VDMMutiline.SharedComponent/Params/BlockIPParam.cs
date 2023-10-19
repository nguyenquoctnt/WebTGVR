using System.Collections.Generic;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;

namespace VDMMutiline.SharedComponent.Params
{
    public class BlockIPParam : BaseParam
    {
        #region Action
        public int Id { get; set; }
        public BlockIP BlockIP { get; set; }
        public List<BlockIP> BlockIPs { get; set; }

        public BlockIPFilter BlockIPFilter { get; set; }
        #endregion
    }
    public class BlockIPFilter : BaseFilter
    {
        public int? Id { get; set; }
        public string IP { get; set; }
    }
}
