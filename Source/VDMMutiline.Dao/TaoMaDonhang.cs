using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VDMMutiline.Dao
{
    public class TaoMaDonhang
    {
        public static string CreateMaThongTin(int ID)
        {
            string Mau = "ORD";
            DateTime GetDate = DateTime.Now;
            string Nam = GetDate.ToString("yy");
            string stringid = ID.ToString();
            string MTT = string.Empty;
            Nam = "";
            if (stringid.Length >= 6)
            {
                MTT = Mau  + Nam + stringid;
            }
            else if (stringid.Length == 5)
            {
                MTT = Mau  + Nam + "0" + stringid;
            }
            else if (stringid.Length == 4)
            {
                MTT = Mau  + Nam + "00" + stringid;
            }
            else if (stringid.Length == 3)
            {
                MTT = Mau  + Nam + "000" + stringid;
            }
            else if (stringid.Length == 2)
            {
                MTT = Mau  + Nam + "0000" + stringid;
            }
            else if (stringid.Length == 1)
            {
                MTT = Mau  + Nam + "00000" + stringid;
            }
            return MTT;
        }

    }
}
