using System.Collections.Generic;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;

namespace VDMMutiline.SharedComponent.Params
{
    public class BlockSMParam : BaseParam
    {
        #region Action
        public int Id { get; set; }
        public BlockSM BlockSM { get; set; }
        public List<BlockSM> BlockSMss { get; set; }

        public BlockSMFilter BlockSMFilter { get; set; }
        #endregion
    }
    public class BlockSMFilter : BaseFilter
    {
        public int? Id { get; set; }
        public string phone { get; set; }
    }
}
