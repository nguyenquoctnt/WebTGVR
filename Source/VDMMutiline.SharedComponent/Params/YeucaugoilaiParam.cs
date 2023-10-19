using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;


namespace VDMMutiline.SharedComponent.Params
{
    public class YeucaugoilaiParam : BaseParam
    {
        public int Id { get; set; }
        public YeucaugoilaiInfo YeucaugoilaiInfo { get; set; }
        public List<YeucaugoilaiInfo> YeucaugoilaiInfos { get; set; }
        public tbl_yeucaugoilai yeucaugoilai { get; set; }
        public List<tbl_yeucaugoilai> yeucaugoilais { get; set; }
        public PagedList.IPagedList<YeucaugoilaiInfo> PagedList { get; set; }
        public YeucaugoilaiFilter YeucaugoilaiFilter { get; set; }
        public List<AspNetUserInfo> UserInfos { get; set; }
    }
    public class YeucaugoilaiFilter : BaseFilter
    {
        public int? Id { get; set; }
      
    }
}
