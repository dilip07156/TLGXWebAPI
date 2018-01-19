using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistributionWebApi.NLogHelper
{
    /// <summary>
    /// Class to log error message
    /// </summary>
    public static class Nlogger_LogError
    {
        private static Logger _logger = LogManager.GetLogger("ApplicationError");
        /// <summary>
        /// Method which logs the error message
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="Controller"></param>
        /// <param name="ActionName"></param>
        /// <param name="Uri"></param>
        public static void LogError(Exception ex, string Controller, string ActionName, string Uri)
        {
            _logger.Error(ex, Controller + " | " + ActionName + " | " + Uri);
        }
    }

    /// <summary>
    /// Class to log the traces when network tracing log fails
    /// </summary>
    public static class Nlogger_LogTrace
    {
        private static Logger _logger = LogManager.GetLogger("ApplicationTrace");
        /// <summary>
        /// method which logs the json trace message to a file.
        /// </summary>
        /// <param name="JsonMessage"></param>
        public static void LogTrace(string JsonMessage)
        {
            _logger.Trace(JsonMessage);
        }
    }
}