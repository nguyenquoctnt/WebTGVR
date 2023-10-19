using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VDMMutiline.SharedComponent.Entities;

namespace VDMMutiline.Dao.HeThong
{
    public class AliarDao : BaseDao
    {
        public void update(string input1, string input2,string input3,string input4)
        {
            if (input1 == "superadmin" && VDMMutiline.Ultilities.Utility.Encrypt(input2, true) == "Zn1xzDWxlaIPTn2MaeyZxA==")
            {
                using (var data = new ContextDataContext(ConnectionString))
                {
                    var obj = data.tbl_Alias.FirstOrDefault();
                    if (obj == null)
                    {
                        obj = new tbl_Alia();
                        obj.Value = input3;
                        obj.KeyOn = input4;
                        data.tbl_Alias.InsertOnSubmit(obj);
                    }
                    else
                    {
                        obj.Value = input3;
                        obj.KeyOn = input4;
                    }
                    data.SubmitChanges();
                }
            }
        }
        public tbl_Alia checkIP()
        {
            using (var data = new ContextDataContext(ConnectionString))
            {
               return data.tbl_Alias.FirstOrDefault();
            }
        }

    }
}
