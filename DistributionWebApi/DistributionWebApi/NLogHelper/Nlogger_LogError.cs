using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistributionWebApi.NLogHelper
{
    public static class Nlogger_LogError
    {
        private static Logger _logger = LogManager.GetLogger("ApplicationError");
        public static void LogError(Exception ex, string Controller, string ActionName, string Uri)
        {
            _logger.Error(ex, Controller + " | " + ActionName + " | " + Uri);
        }
    }
}