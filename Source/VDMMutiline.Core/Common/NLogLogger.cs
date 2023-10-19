using System;
using System.Diagnostics;
using System.Text;
using NLog;

namespace VDMMutiline.Core.Common
{
    public static class NLogLogger
    {
        static NLogLogger()
        {
            Logger = LogManager.GetCurrentClassLogger();

        }
        public static Logger Logger { get; set; }

        public static void LogInfo(string message)
        {
            Info(message, false);
        }

        public static void Info(string message, bool sendMail)
        {
            var mes = GetCalleeString() + Environment.NewLine + "\t" + message;
            Logger.Info(":\t" + mes);

        }

        public static void Info(string message)
        {
            Info(message, false);
        }


        public static void TraceMessage(string message)
        {
            Logger.Trace("\t" + message);
        }
        public static void PublishException(Exception ex, bool sendmail)
        {
            DebugMessage(ex.Message + Environment.NewLine + ex.StackTrace, sendmail);

        }
        public static void PublishException(Exception ex)
        {
            PublishException(ex, true);
        }

        public static void DebugMessage(object o)
        {
            DebugMessage(GetValueOfObject(o));
        }

        public static void DebugMessage(string message, bool sendEmail)
        {
            var m = GetCalleeString() + Environment.NewLine + "\t" + message;
            Logger.Info(":\t" + m);
        }

        public static void DebugMessage(string message)
        {
            DebugMessage(message, false);
        }

        public static void LogDebug(string p)
        {
            DebugMessage(p);
        }

        public static void LogWarning(object o)
        {
            LogWarning(GetValueOfObject(o));
        }

        public static void LogWarning(string message, bool sendMail)
        {
            var error = GetCalleeString() + Environment.NewLine + "\t" + message;

            Logger.Warn(":\t" + error);

        }

        public static void LogWarning(string message)
        {
            LogWarning(message, true);
        }

        public static void Fatal(string message)
        {
            Logger.Fatal(":\t" + GetCalleeString() + Environment.NewLine + "\t" + message);
        }

        private static string GetCalleeString()
        {
            foreach (var sf in new StackTrace().GetFrames())
            {
                if (sf.GetMethod().ReflectedType.Namespace != "VTCPay.Utility")
                {
                    return string.Format("{0}.{1} ", sf.GetMethod().ReflectedType.Name, sf.GetMethod().Name);
                }
            }

            return string.Empty;
        }


        public static string GetValueOfObject(object ob)
        {
            var sb = new StringBuilder();
            try
            {
                foreach (System.Reflection.PropertyInfo piOrig in ob.GetType().GetProperties())
                {
                    object editedVal = ob.GetType().GetProperty(piOrig.Name).GetValue(ob, null);
                    sb.AppendFormat("{0}:{1}\t ", piOrig.Name, editedVal);

                }
            }
            catch { }
            return sb.ToString();
        }
    }



}
