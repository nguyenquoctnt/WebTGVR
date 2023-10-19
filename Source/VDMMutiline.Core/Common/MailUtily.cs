using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Xml;
using System.Xml.Serialization;
using VDMMutiline.Ultilities;
using VDMMutiline.SharedComponent.EntityInfo;


namespace VDMMutiline.Core
{
    public class MailUtily
    {
        public static void sendmail(List<SettingUserInfo> settinglist, string txtSubject, string txtBody, List<MailAddress> lstMailAddress, Attachment MailAttachment,
        string fromAddress, string fromPassword, int? port, string Host)
        {
            try
            {
               
                var smtp = new System.Net.Mail.SmtpClient();
                {
                    smtp.Host = Host;
                    smtp.Port = port.HasValue ? port.Value : 0;
                    smtp.EnableSsl = true;
                    smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                    smtp.Credentials = new NetworkCredential(fromAddress, fromPassword);
                    smtp.Timeout = 20000;
                }
                MailMessage message = new MailMessage();
                message.From = new MailAddress(fromAddress);

                foreach (MailAddress m in lstMailAddress)
                {
                    message.To.Add(m);
                }
                foreach (MailAddress m in UI.UIUty.GetMailCC(settinglist))
                {
                    message.To.Add(m);
                }
                message.Attachments.Add(MailAttachment);
                message.Subject = txtSubject;
                message.IsBodyHtml = true;
                message.Body = txtBody;
                smtp.Send(message);
            }
            catch (Exception ex)
            {
                try
                {
                    var stringa = "Username:" + fromAddress + "Pass:" + fromPassword + "Lỗi:" + ex.Message;
                    SerializeObject(stringa, HttpContext.Current.Server.MapPath("~/Log/Senmail.xml"));
                }
                catch
                {
                }
            }

        }
        public static void sendmail(List<SettingUserInfo> settinglist, string txtSubject, string txtBody, List<MailAddress> lstMailAddress, 
     string fromAddress, string fromPassword, int? port, string Host)
        {
            try
            {

                var smtp = new System.Net.Mail.SmtpClient();
                {
                    smtp.Host = Host;
                    smtp.Port = port.HasValue ? port.Value : 0;
                    smtp.EnableSsl = true;
                    smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                    smtp.Credentials = new NetworkCredential(fromAddress, fromPassword);
                    smtp.Timeout = 20000;
                }
                MailMessage message = new MailMessage();
                message.From = new MailAddress(fromAddress);

                foreach (MailAddress m in lstMailAddress)
                {
                    message.To.Add(m);
                }
                foreach (MailAddress m in UI.UIUty.GetMailCC(settinglist))
                {
                    message.To.Add(m);
                }
                message.Subject = txtSubject;
                message.IsBodyHtml = true;
                message.Body = txtBody;
                smtp.Send(message);
            }
            catch (Exception ex)
            {
                try
                {
                    var stringa = "Username:" + fromAddress + "Pass:" + fromPassword + "Lỗi:" + ex.Message;
                    SerializeObject(stringa, HttpContext.Current.Server.MapPath("~/Log/Senmail.xml"));
                }
                catch 
                {
                }
            }

        }
        public static void SerializeObject<T>(T serializableObject, string fileName)
        {
            if (serializableObject == null) { return; }

            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                XmlSerializer serializer = new XmlSerializer(serializableObject.GetType());
                using (MemoryStream stream = new MemoryStream())
                {
                    serializer.Serialize(stream, serializableObject);
                    stream.Position = 0;
                    xmlDocument.Load(stream);
                    xmlDocument.Save(fileName);
                    stream.Close();
                }
            }
            catch (Exception ex)
            {
                //Log exception here
            }
        }
        public static void sendmail(string txtSubject, string txtBody, List<MailAddress> lstMailAddress)
        {
            var EmailSend = ConfigurationSettings.AppSettings.Get("Email_System");
            var EmailPassSend = ConfigurationSettings.AppSettings.Get("PASS_Email_System");
            var SmtpPort = ConfigurationSettings.AppSettings.Get("EmailSender_SmtpPort");
            var SmtpClient = ConfigurationSettings.AppSettings.Get("EmailSender_SmtpClient");
            try
            {
                var port = Utility.ConvertToInt(SmtpPort);
                var smtp = new System.Net.Mail.SmtpClient();
                {
                    smtp.Host = SmtpClient;
                    smtp.Port = port.HasValue ? port.Value : 0;
                    smtp.EnableSsl = true;
                    smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                    smtp.Credentials = new NetworkCredential(EmailSend, EmailPassSend);
                    smtp.Timeout = 20000;
                }

                MailMessage message = new MailMessage();
                message.From = new MailAddress(EmailSend);

                foreach (MailAddress m in lstMailAddress)
                {
                    message.To.Add(m);
                }
                message.Subject = txtSubject;
                message.IsBodyHtml = true;
                message.Body = txtBody;
                smtp.Send(message);
            }
            catch (Exception ex)
            {
            }

        }
        //public static void sendmail(string txtSubject, string txtBody, List<MailAddress> lstMailAddress, Attachment MailAttachment)
        //{
        //    var fromAddress = Getsrtting("Email_System");
        //    string fromPassword = Getsrtting("PASS_Email_System");
        //    try
        //    {

        //        var port = Utility.ConvertToInt(Getsrtting("EmailSender_SmtpPort"));
        //        string Host = Getsrtting("EmailSender_SmtpClient");

        //        var smtp = new System.Net.Mail.SmtpClient();
        //        {
        //            smtp.Host = Host;
        //            smtp.Port = port.HasValue ? port.Value : 0;
        //            smtp.EnableSsl = true;
        //            smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
        //            smtp.Credentials = new NetworkCredential(fromAddress, fromPassword);
        //            smtp.Timeout = 20000;
        //        }

        //        MailMessage message = new MailMessage();
        //        message.From = new MailAddress(fromAddress);

        //        foreach (MailAddress m in lstMailAddress)
        //        {
        //            message.To.Add(m);
        //        }
        //        foreach (MailAddress m in GetMailCC())
        //        {
        //            message.To.Add(m);
        //        }
        //        message.Attachments.Add(MailAttachment);
        //        message.Subject = txtSubject;
        //        message.IsBodyHtml = true;
        //        message.Body = txtBody;
        //        smtp.Send(message);
        //    }
        //    catch (Exception ex)
        //    {
        //        var stringa = "Username:" + fromAddress + "Pass:" + fromPassword + "Lỗi:" + ex.Message;
        //        SerializeObject(stringa, HttpContext.Current.Server.MapPath("~/Log/Senmail.xml"));
        //    }

        //}
        //public static void sendmail(string txtSubject, string txtBody, List<MailAddress> lstMailAddress, Attachment MailAttachment,
        //    string fromAddress, string fromPassword, int? port, string Host)
        //{
        //    try
        //    {
        //        fromAddress = !string.IsNullOrEmpty(fromAddress) ? fromAddress : Getsrtting("Email_System");
        //        fromPassword = !string.IsNullOrEmpty(fromPassword) ? fromPassword : Getsrtting("PASS_Email_System");
        //        port = port > 0 ? port : Utility.ConvertToInt(Getsrtting("EmailSender_SmtpPort"));
        //        Host = !string.IsNullOrEmpty(Host) ? Host : Getsrtting("EmailSender_SmtpClient");
        //        var smtp = new System.Net.Mail.SmtpClient();
        //        {
        //            smtp.Host = Host;
        //            smtp.Port = port.HasValue ? port.Value : 0;
        //            smtp.EnableSsl = true;
        //            smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
        //            smtp.Credentials = new NetworkCredential(fromAddress, fromPassword);
        //            smtp.Timeout = 20000;
        //        }
        //        MailMessage message = new MailMessage();
        //        message.From = new MailAddress(fromAddress);

        //        foreach (MailAddress m in lstMailAddress)
        //        {
        //            message.To.Add(m);
        //        }
        //        foreach (MailAddress m in GetMailCC())
        //        {
        //            message.To.Add(m);
        //        }
        //        message.Attachments.Add(MailAttachment);
        //        message.Subject = txtSubject;
        //        message.IsBodyHtml = true;
        //        message.Body = txtBody;
        //        smtp.Send(message);
        //    }
        //    catch (Exception ex)
        //    {
        //        var stringa = "Username:" + fromAddress + "Pass:" + fromPassword + "Lỗi:" + ex.Message;
        //        SerializeObject(stringa, HttpContext.Current.Server.MapPath("~/Log/Senmail.xml"));
        //    }

        //}



    }
}