using VDMMutiline.SharedComponent.EntityInfo;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace VDMMutiline.Core
{
    public class NotifyHelper
    {
        public static void NotifyUsers(bool isAllUsers, NotificationInfo notification)
        {
            string s = "NotifyID=" + notification.NotificationID;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(System.Configuration.ConfigurationManager.AppSettings["UrlNotify"]);
                request.Method = "POST";
                request.KeepAlive = true;
                byte[] bytes = Encoding.UTF8.GetBytes(s);
                request.ContentLength = s.Length;
                request.ContentType = "application/x-www-form-urlencoded";
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(bytes, 0, bytes.Length);
                }
                string result = "" + request.RequestUri.ToString();
                using (HttpWebResponse webResponse = (HttpWebResponse)request.GetResponse())   //Error occurs here
                {
                    using (StreamReader sr = new StreamReader(webResponse.GetResponseStream()))
                    {
                        result += sr.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}

