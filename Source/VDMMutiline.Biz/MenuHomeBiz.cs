using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VDMMutiline.Dao;
using VDMMutiline.SharedComponent.Params;

namespace VDMMutiline.Biz
{
    public class MenuHomeBiz : BaseBiz
    {
        MenuHomeDao dao = new MenuHomeDao();
        public void GetlistMenuHome(MenuHomeParam param)
        {
            dao.GetlistMenuHome(param);
        }
        public void Updata(MenuHomeParam param)
        {
            dao.Updata(param);
        }
    }
}
