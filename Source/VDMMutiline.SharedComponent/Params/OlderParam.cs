using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;

namespace VDMMutiline.SharedComponent.Params
{
    public class OlderParam : BaseParam
    {
        public int Id { get; set; }
        public BK_Order BK_Order { get; set; }
        public List<BK_Order> BK_Orders { get; set; }
        public PagedList.IPagedList<BK_Order> PagedList { get; set; }
        public List<string> ListUserSite { get; set; }
        public string Seach { get; set; }
    }
}
