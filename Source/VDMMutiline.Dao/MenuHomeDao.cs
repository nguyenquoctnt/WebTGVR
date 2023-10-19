using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
using VDMMutiline.SharedComponent.Params;

namespace VDMMutiline.Dao
{
    public class MenuHomeDao : BaseDao
    {
        public void GetlistMenuHome(MenuHomeParam param)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.SysMenuHomes
                            join MenuHomeuser in dbContext.SysUserMenuHomes.Where(z => z.Username == param.UserName) on n.Id equals MenuHomeuser.Id_MenuHome into p
                            from MenuHomeuservalue in p.DefaultIfEmpty()
                            select new SysMenuHomeInfo()
                            {
                                Id = n.Id,
                                Menuname = n.Menuname,
                                Status = MenuHomeuservalue.Status ?? true,
                                Username = MenuHomeuservalue.Username,
                            };
                param.SysMenuHomeInfos = query.ToList();
            }
        }
        public void Updata(MenuHomeParam param)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                foreach (var item in param.SysMenuHomeInfos)
                {
                    var objcheck = dbContext.SysUserMenuHomes.FirstOrDefault(z => z.Username == param.UserName && z.Id_MenuHome == item.Id);
                    if (objcheck != null)
                    {
                        objcheck.Status = item.Status;
                        dbContext.SubmitChanges();
                    }
                    else
                    {
                        var ck = new SysUserMenuHome();
                        ck.Status = item.Status;
                        ck.Id_MenuHome = item.Id;
                        ck.Username = param.UserName;
                        dbContext.SysUserMenuHomes.InsertOnSubmit(ck);
                        dbContext.SubmitChanges();
                    }
                }

            }
        }
    }
}
