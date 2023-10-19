using System;
using NLog;
using ApiClient.Serialization;

namespace ApiClient.Logger
{
    /// <summary>
    /// This class represents the Logger implementation to be used by the application
    /// and currently encapsulates NLog
    /// </summary>
    public class Logger : ILogger
    {
        #region Fields
        private readonly NLog.Logger logInstance;
        #endregion

        #region Public Properties
        /// <summary>
        /// Indicates if the Debug is enabled
        /// </summary>
        public bool IsDebugEnabled
        {
            get { return logInstance.IsDebugEnabled; }
        }

        /// <summary>
        /// Indicates if the Info is enabled
        /// </summary>
        public bool IsInfoEnabled
        {
            get { return logInstance.IsInfoEnabled; }
        }

        /// <summary>
        /// Indicates if the Warn is enabled
        /// </summary>
        public bool IsWarnEnabled
        {
            get { return logInstance.IsWarnEnabled; }
        }

        /// <summary>
        /// Indicates if the Error is enabled
        /// </summary>
        public bool IsErrorEnabled
        {
            get { return logInstance.IsErrorEnabled; }
        }

        /// <summary>
        /// Indicates if the Fatal is enabled
        /// </summary>
        public bool IsFatalEnabled
        {
            get { return logInstance.IsFatalEnabled; }
        }
        #endregion

        #region Constructors
        
        /// <summary>
        /// Constructor responsible for obtaing the logger according to the Type passed.
        /// </summary>
        /// <param name="type">Object Type to Log</param>
        public Logger(string name)
        {
            logInstance = LogManager.GetLogger(name);
        }
        
        #endregion

        #region Public Methods
        
        /// <summary>
        /// Logs information at an Debug level
        /// </summary>
        /// <param name="logText">Information to log</param>
        /// <param name="arg">Optional: argument to be added to the information to log</param>
        /// <param name="ex">Optional: Exception to log</param>
        /// <param name="args">Optional: Arguments to present on the information to log</param>
        public void LogDebug(string logText, object arg = null, Exception ex = null, params object[] args)
        {
            if (args != null && args.Length > 0)
            {
                logInstance.Debug(logText, args);
            }
            else if (ex != null)
            {
                logInstance.Debug(ex, logText);
            }
            else if (arg != null)
            {
                logInstance.Debug(logText, arg);
            }
            else
            {
                logInstance.Debug(logText);
            }
        }

        /// <summary>
        /// Logs information at an Debug level
        /// </summary>
        /// <param name="logText">Information to log</param>
        /// <param name="args">Optional: Arguments to present on the information to log</param>
        public void LogDebug(string logText, params object[] args)
        {
            LogDebug(logText, arg: null, ex: null, args: args);
        }

        /// <summary>
        /// Logs information at an Info level
        /// </summary>
        /// <param name="logText">Information to log</param>
        /// <param name="exception">Optional: Exception to log</param>
        /// <param name="args">Optional: Arguments to present on the information to log </param>
        public void LogInfo(string logText, Exception exception = null, params object[] args )
        {
            if (args != null && args.Length > 0)
            {
                logInstance.Info(logText, args);
            }
            else if (exception != null)
            {
                logInstance.Info(exception, logText);
            }
            else
            {
                logInstance.Info(logText);
            }
        }

        /// <summary>
        /// Logs information at an Info level as Xml format
        /// </summary>
        /// <param name="logObject">Information to log</param>
        /// <param name="exception">Optional: Exception to log</param>
        /// <param name="args">Optional: Arguments to present on the information to log </param>
        public void LogInfoAsXml(object logObject, Exception exception = null, params object[] args)
        {
            LogInfo(XmlSerialization.XmlSerializeToString(logObject), exception, args);
        }

        /// <summary>
        /// Logs information at a Warn level
        /// </summary>
        /// <param name="logText">Information to log</param>
        /// <param name="arg">Optional: argument to be added to the information to log</param>
        /// <param name="args">Optional: Arguments to present on the information to log</param>
        public void LogWarn(string logText, object arg = null, params object[] args)
        {
            if (arg == null)
            {
                logInstance.Warn(logText, arg);
            }
            else if (args != null && args.Length > 0)
            {
                logInstance.Warn(logText, args);
            }
            else
            {
                logInstance.Warn(logText);
            }
        }

        /// <summary>
        /// Logs information at an Error level
        /// </summary>
        /// <param name="logText">Information to log</param>
        /// <param name="arg">Optional: argument to be added to the information to log</param>
        /// <param name="ex">Optional: Exception to log</param>
        /// <param name="args">Optional: Arguments to present on the information to log</param>
        public void LogError(string logText, object arg = null, Exception ex = null, params object[] args)
        {
            if (args != null && args.Length > 0)
            {
                logInstance.Error(logText, args);
            }
            else if (ex != null)
            {
                logInstance.Error(ex, logText);
            }
            else if (arg != null)
            {
                logInstance.Error(logText, arg);
            }
            else
            {
                logInstance.Error(logText);
            }
        }
        
        /// <summary>
        /// Logs information at an Fatal level
        /// </summary>
        /// <param name="logText">Information to log</param>
        /// <param name="arg">Optional: argument to be added to the information to log</param>
        /// <param name="ex">Optional: Exception to log</param>
        /// <param name="args">Optional: Arguments to present on the information to log</param>
        public void LogFatal(string logText, object arg = null, Exception ex = null, params object[] args)
        {
            if (args != null && args.Length > 0)
            {
                logInstance.Fatal(logText, args);
            }
            else if (ex != null)
            {
                logInstance.Fatal(ex, logText);
            }
            else if (arg != null)
            {
                logInstance.Fatal(logText, arg);
            }
            else
            {
                logInstance.Fatal(logText);
            }
        }

        #endregion
    }
}
