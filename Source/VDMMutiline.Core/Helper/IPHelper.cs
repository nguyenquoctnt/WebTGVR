using System.Net;
using System.Web;
namespace VDMMutiline.Core
{
    public class IPHelper
    {
        public static string GetIPAddress(HttpRequestBase request)
        {
            string userHostAddress = request.UserHostAddress;
            string str2 = request.ServerVariables["X_FORWARDED_FOR"];
            string str3 = "";
            if (str2 == null)
            {
                str3 = userHostAddress;
            }
            else
            {
                str3 = str2;
                if (str3.IndexOf(",") > 0)
                {
                    char[] separator = new char[] { ',' };
                    string[] strArray = str3.Split(separator);
                    foreach (string str4 in strArray)
                    {
                        if (!isPrivateIP(str4))
                        {
                            return str4;
                        }
                    }
                }
            }
            return str3;
        }

        public static bool isPrivateIP(string host)
        {
            try
            {
                IPAddress[] hostAddresses = Dns.GetHostAddresses(host);
                IPAddress[] addressArray2 = Dns.GetHostAddresses(Dns.GetHostName());
                foreach (IPAddress address in hostAddresses)
                {
                    if (IPAddress.IsLoopback(address))
                    {
                        return true;
                    }
                    foreach (IPAddress address2 in addressArray2)
                    {
                        if (address.Equals(address2))
                        {
                            return true;
                        }
                    }
                }
            }
            catch
            {
            }
            return false;
        }
    }
}

