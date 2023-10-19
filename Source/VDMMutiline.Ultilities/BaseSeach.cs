using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VDMMutiline.Ultilities;

namespace VDMMutiline.Ultilities
{
    public class BaseSeach
    {
        public static string ThanhtoanCodeWesite = System.Configuration.ConfigurationSettings.AppSettings.Get("ThanhtoanCodeWesite");
        public static string DonviThanhToan = System.Configuration.ConfigurationSettings.AppSettings.Get("DonviThanhToan");
        public static string TaiKhoanNhanTien = System.Configuration.ConfigurationSettings.AppSettings.Get("TaiKhoanNhanTien");
        public static string Checkip = System.Configuration.ConfigurationSettings.AppSettings.Get("Checkip");

        public string Username
        {
            set
            {
                string values = System.Configuration.ConfigurationSettings.AppSettings.Get("Username");
                if (!string.IsNullOrEmpty(values))
                    value = Utility.Decrypt(values, true);
            }
            get
            {
                string values = System.Configuration.ConfigurationSettings.AppSettings.Get("Username");
                if (!string.IsNullOrEmpty(values))
                    values = Utility.Decrypt(values, true);
                return values;
            }

        }
        public string Password
        {
            set
            {
                string values = System.Configuration.ConfigurationSettings.AppSettings.Get("Password");
                if (!string.IsNullOrEmpty(values))
                    value = Utility.Decrypt(values, true);
            }
            get
            {
                string values = System.Configuration.ConfigurationSettings.AppSettings.Get("Password");
                if (!string.IsNullOrEmpty(values))
                    values = Utility.Decrypt(values, true);
                return values;
            }

        }
        public string HeaderUser
        {
            set
            {
                string values = System.Configuration.ConfigurationSettings.AppSettings.Get("HeaderUser");
                if (!string.IsNullOrEmpty(values))
                    value = Utility.Decrypt(values, true);
            }
            get
            {
                string values = System.Configuration.ConfigurationSettings.AppSettings.Get("HeaderUser");
                if (!string.IsNullOrEmpty(values))
                    values = Utility.Decrypt(values, true);
                return values;
            }

        }
        public string HeaderPassword
        {
            set
            {
                string values = System.Configuration.ConfigurationSettings.AppSettings.Get("HeaderPassword");
                if (!string.IsNullOrEmpty(values))
                    value = Utility.Decrypt(values, true);
            }
            get
            {
                string values = System.Configuration.ConfigurationSettings.AppSettings.Get("HeaderPassword");
                if (!string.IsNullOrEmpty(values))
                    values = Utility.Decrypt(values, true);
                return values;
            }

        }

        public string HeaderUserJQ
        {
            set
            {
                string values = System.Configuration.ConfigurationSettings.AppSettings.Get("HeaderUserJQ");
                if (!string.IsNullOrEmpty(values))
                    value = Utility.Decrypt(values, true);
            }
            get
            {
                string values = System.Configuration.ConfigurationSettings.AppSettings.Get("HeaderUserJQ");
                if (!string.IsNullOrEmpty(values))
                    values = Utility.Decrypt(values, true);
                return values;
            }

        }
        public string HeaderPasswordJQ
        {
            set
            {
                string values = System.Configuration.ConfigurationSettings.AppSettings.Get("HeaderPasswordJQ");
                if (!string.IsNullOrEmpty(values))
                    value = Utility.Decrypt(values, true);
            }
            get
            {
                string values = System.Configuration.ConfigurationSettings.AppSettings.Get("HeaderPasswordJQ");
                if (!string.IsNullOrEmpty(values))
                    values = Utility.Decrypt(values, true);
                return values;
            }

        }
        public string Uservethang
        {
            set
            {
                string values = System.Configuration.ConfigurationSettings.AppSettings.Get("Uservethang");
                if (!string.IsNullOrEmpty(values))
                    value = Utility.Decrypt(values, true);
            }
            get
            {
                string values = System.Configuration.ConfigurationSettings.AppSettings.Get("Uservethang");
                if (!string.IsNullOrEmpty(values))
                    values = Utility.Decrypt(values, true);
                return values;
            }

        }
        public string Pasvethang
        {
            set
            {
                string values = System.Configuration.ConfigurationSettings.AppSettings.Get("Pasvethang");
                if (!string.IsNullOrEmpty(values))
                    value = Utility.Decrypt(values, true);
            }
            get
            {
                string values = System.Configuration.ConfigurationSettings.AppSettings.Get("Pasvethang");
                if (!string.IsNullOrEmpty(values))
                    values = Utility.Decrypt(values, true);
                return values;
            }

        }
    }
}
