using System.Collections.Generic;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
namespace VDMMutiline.SharedComponent.Params
{
    public class SysSmsTemplateParam:BaseParam
    {
        #region Action
        public int Id { get; set; }
        public SysSmsTemplate SysSmsTemplate { get; set; }

        public List<SysSmsTemplate> SysSmsTemplates { get; set; }

        public SysSmsTemplateInfo SysSmsTemplateInfo { get; set; }

        public List<SysSmsTemplateInfo> SysSmsTemplateInfos { get; set; }

        public SysSmsTemplateFilter SysSmsTemplateFilter { get; set; }
        #endregion
    }
    public class SysSmsTemplateFilter : BaseFilter
    {
        public int? Id { get; set; }
    }

}