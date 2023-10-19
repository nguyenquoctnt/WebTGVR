using System;
using System.Collections.Generic;

namespace ApiClient.Logger
{
    /// <summary>
    /// This class is responsible for creating and returning loggers
    /// </summary>
    public static class LogFactory
    {
        #region Fields
        /// <summary>
        /// LoggerRepository
        /// </summary>
        private static Dictionary<string, Logger> LoggerRepository = new Dictionary<string, Logger>();
        
        /// <summary>
        /// Object to lock
        /// </summary>
        private static object syncRoot = new Object();

        #endregion

        #region Public Methods
        /// <summary>
        /// Returns a Logger given a specific Type
        /// </summary>
        /// <param name="name">Type</param>
        /// <returns>Logger for the type specified</returns>
        public static ILogger GetLogger(string name)
        {
            Logger log = null;
            LoggerRepository.TryGetValue(name, out log);
            if(log == null)
            {/*
                log = new Logger(type);
                LoggerRepository[type] = log; //if this "type" already exists will just overwrite it as opposed to Add method which
                 * would throw an exception.
                */
                lock (syncRoot)
                {
                    LoggerRepository.TryGetValue(name, out log);
                    if(log == null)
                    {
                        log = new Logger(name);
                        LoggerRepository.Add(name, log);
                    }
                    
                }

            }
            return log;
        }
        #endregion
  
    }
}
