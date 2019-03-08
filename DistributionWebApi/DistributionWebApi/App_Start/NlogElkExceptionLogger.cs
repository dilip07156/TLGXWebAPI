using DistributionWebApi.App_Start;
using DistributionWebApi.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.ExceptionHandling;

namespace DistributionWebApi
{
    public class NlogElkExceptionLogger : ExceptionLogger
    {
        private RepositoryBase<ErrorLog, string> _logRepo;

        public NlogElkExceptionLogger()
        {
            _logRepo = new RepositoryBase<ErrorLog, string>();
        }

        public async override Task LogAsync(ExceptionLoggerContext context, System.Threading.CancellationToken cancellationToken)
        {
            _logRepo.Insert(new ErrorLog
            {
                LogDate = DateTime.Now,
                LogType = "Error",
                Application = "TLGX_WEBAPI",
                Message = context.Exception.Message,
                InnerException = context.Exception.InnerException == null ? string.Empty : context.Exception.InnerException.Message,
                StackTrace = context.Exception.StackTrace,
                Source = context.Exception.Source,
                TargetSite = context.Exception.TargetSite.Name,
                URL = context.Request.RequestUri.OriginalString
            });
            await base.LogAsync(context, cancellationToken);
        }

        public override void Log(ExceptionLoggerContext context)
        {
            //_logRepo.Insert(new ErrorLog
            //{
            //    LogDate = DateTime.Now,
            //    LogType = "Error",
            //    Application = "TLGX_WEBAPI",
            //    Message = context.Exception.Message,
            //    InnerException = context.Exception.InnerException == null ? string.Empty : context.Exception.InnerException.Message,
            //    StackTrace = context.Exception.StackTrace,
            //    Source = context.Exception.Source,
            //    TargetSite = context.Exception.TargetSite.Name,
            //    URL = context.Request.RequestUri.OriginalString,
            //});
            base.Log(context);
        }

        public override bool ShouldLog(ExceptionLoggerContext context)
        {
            return base.ShouldLog(context);
        }
    }
}