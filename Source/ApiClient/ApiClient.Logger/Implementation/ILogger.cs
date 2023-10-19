using System;

namespace ApiClient.Logger
{
    /// <summary>
    /// ILogger interface that All log classes should implement
    /// </summary>
    public interface ILogger
    {
        #region Properties
        /// <summary>
        /// Indicates if the Debug is enabled
        /// </summary>
        bool IsDebugEnabled { get; }

        /// <summary>
        /// Indicates if the Info is enabled
        /// </summary>
        bool IsInfoEnabled  { get; }

        /// <summary>
        /// Indicates if the Warn is enabled
        /// </summary>
        bool IsWarnEnabled  { get; }

        /// <summary>
        /// Indicates if the Error is enabled
        /// </summary>
        bool IsErrorEnabled { get; }

        /// <summary>
        /// Indicates if the Fatal is enabled
        /// </summary>
        bool IsFatalEnabled { get; }
        #endregion

        #region Methods
        /// <summary>
        /// Logs information at an Debug level
        /// </summary>
        /// <param name="logText">Information to log</param>
        /// <param name="arg">Optional: argument to be added to the information to log</param>
        /// <param name="ex">Optional: Exception to log</param>
        /// <param name="args">Optional: Arguments to present on the information to log</param>
        void LogDebug(string logText, object arg = null, Exception ex = null, params object[] args);

        /// <summary>
        /// Logs information at an Debug level
        /// </summary>
        /// <param name="logText">Information to log</param>
        /// <param name="args">Optional: Arguments to present on the information to log</param>
        void LogDebug(string logText, params object[] args);

        /// <summary>
        /// Logs information at an Info level
        /// </summary>
        /// <param name="logText">Information to log</param>
        /// <param name="exception">Optional: Exception to log</param>
        /// <param name="args">Optional: Arguments to present on the information to log </param>
        void LogInfo(string logText, Exception exception = null, params object[] args);

        /// <summary>
        /// Logs information at an Info level as Xml format
        /// </summary>
        /// <param name="logObject">Information to log</param>
        /// <param name="exception">Optional: Exception to log</param>
        /// <param name="args">Optional: Arguments to present on the information to log </param>
        void LogInfoAsXml(Object logObject, Exception exception = null, params object[] args);

        /// <summary>
        /// Logs information at a Warn level
        /// </summary>
        /// <param name="logText">Information to log</param>
        /// <param name="arg">Optional: argument to be added to the information to log</param>
        /// <param name="args">Optional: Arguments to present on the information to log</param>
        void LogWarn(string logText, object arg = null, params object[] args);

        /// <summary>
        /// Logs information at an Error level
        /// </summary>
        /// <param name="logText">Information to log</param>
        /// <param name="arg">Optional: argument to be added to the information to log</param>
        /// <param name="ex">Optional: Exception to log</param>
        /// <param name="args">Optional: Arguments to present on the information to log</param>
        void LogError(string logText, object arg = null, Exception ex = null, params object[] args);
        
        /// <summary>
        /// Logs information at an Fatal level
        /// </summary>
        /// <param name="logText">Information to log</param>
        /// <param name="arg">Optional: argument to be added to the information to log</param>
        /// <param name="ex">Optional: Exception to log</param>
        /// <param name="args">Optional: Arguments to present on the information to log</param>
        void LogFatal(string logText, object arg = null, Exception ex = null, params object[] args);

        #endregion
    }
}
