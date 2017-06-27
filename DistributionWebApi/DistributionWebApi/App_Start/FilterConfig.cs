using System.Web;
using System.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Filters;
using System.Web.Http.Controllers;
using System.Web.Http.Tracing;
using System.Web.Http;
using NLog;

namespace DistributionWebApi
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class LoggingFilterAttribute : System.Web.Http.Filters.ActionFilterAttribute
    {
        private static Logger _logger = LogManager.GetLogger("Trace");
        //public override void OnActionExecuting(HttpActionContext filterContext)
        //{
        //    LogEventInfo request = new LogEventInfo(LogLevel.Trace, "Trace", string.Empty);
        //    request.Properties["LogType"] = "Trace";
        //    request.Properties["Format"] = "JSON";
        //    request.Properties["Method"] = filterContext.Request.Method.Method;
        //    request.Properties["URL"] = filterContext.Request.RequestUri.OriginalString;
        //    request.Properties["Controller"] = filterContext.ControllerContext.ControllerDescriptor.ControllerType.FullName;
        //    request.Properties["Action"] = filterContext.ActionDescriptor.ActionName;
        //    request.Properties["Parameter"] = Newtonsoft.Json.JsonConvert.SerializeObject(filterContext.ActionArguments);
        //    request.Properties["MessageType"] = "Request";
        //    request.Properties["Username"] = string.Empty;
        //    request.Properties["Token"] = filterContext.Request.Headers.Contains("Token") ? filterContext.Request.Headers.GetValues("Token").FirstOrDefault() : string.Empty;
        //    request.Properties["TraceId"] = filterContext.Request.Properties["MS_RequestContext"];
        //    request.Properties["Application"] = "TLGX_WEBAPI";
        //    request.Properties["HostIp"] = filterContext.Request.RequestUri.Authority;
        //    _logger.Log(request);



        //    //GlobalConfiguration.Configuration.Services.Replace(typeof(ITraceWriter), new NLogHelper.NLogger());
        //    //var trace = GlobalConfiguration.Configuration.Services.GetTraceWriter();
        //    //trace.Info(filterContext.Request, filterContext.ControllerContext.ControllerDescriptor.ControllerType.FullName + "|" + filterContext.ActionDescriptor.ActionName, "JSON", filterContext.ActionArguments);
        //}

        public override void OnActionExecuted(HttpActionExecutedContext filterContext)
        {
            string TraceId = Guid.NewGuid().ToString().Replace("-", "");

            LogEventInfo request = new LogEventInfo(LogLevel.Trace, "Trace", string.Empty);
            //request.Properties["LogType"] = "Trace";
            //request.Properties["Format"] = "JSON";
            //request.Properties["Method"] = filterContext.Request.Method.Method;
            //request.Properties["URL"] = filterContext.Request.RequestUri.OriginalString;
            //request.Properties["Controller"] = filterContext.ActionContext.ControllerContext.ControllerDescriptor.ControllerType.FullName;
            //request.Properties["Action"] = ((System.Web.Http.Controllers.ReflectedHttpActionDescriptor)filterContext.ActionContext.ActionDescriptor).ActionName;
            //request.Properties["Parameter"] = Newtonsoft.Json.JsonConvert.SerializeObject(filterContext.ActionContext.ActionArguments);
            //request.Properties["MessageType"] = "Request";
            //request.Properties["Username"] = string.Empty;
            //request.Properties["Token"] = filterContext.Request.Headers.Contains("Token") ? filterContext.Request.Headers.GetValues("Token").FirstOrDefault() : string.Empty;
            //request.Properties["TraceId"] = TraceId;
            //request.Properties["Application"] = "TLGX_WEBAPI";
            //request.Properties["HostIp"] = filterContext.Request.RequestUri.Authority;
            Models.TraceLog nlog = new Models.TraceLog();

            nlog.LogDate = DateTime.Now;
            //if (filterContext.Request.Properties.ContainsKey["MS_HttpContext"])
            var ctx = filterContext.Request.Properties["MS_HttpContext"] as HttpContextWrapper;
            if (ctx != null)
            {
                nlog.ClientIP = ctx.Request.UserHostAddress;
            }
            nlog.LogType = "Trace";
            nlog.Format = "JSON";
            nlog.Method = filterContext.Request.Method.Method;
            nlog.URL = filterContext.Request.RequestUri.OriginalString;
            nlog.Controller = filterContext.ActionContext.ControllerContext.ControllerDescriptor.ControllerType.FullName;
            nlog.Action = ((System.Web.Http.Controllers.ReflectedHttpActionDescriptor)filterContext.ActionContext.ActionDescriptor).ActionName;
            nlog.Parameter = Newtonsoft.Json.JsonConvert.SerializeObject(filterContext.ActionContext.ActionArguments);
            nlog.MessageType = "Request";
            nlog.Username = string.Empty;
            nlog.Token = filterContext.Request.Headers.Contains("Token") ? filterContext.Request.Headers.GetValues("Token").FirstOrDefault() : string.Empty;
            nlog.TraceId = TraceId;
            nlog.Application = "TLGX_WEBAPI";
            nlog.HostIp = filterContext.Request.RequestUri.Authority;

            request.Message = Newtonsoft.Json.JsonConvert.SerializeObject(nlog);
            _logger.Log(request);

            nlog = new Models.TraceLog();
            request = new LogEventInfo(LogLevel.Trace, "Trace", string.Empty);
            //request.Properties["LogType"] = "Trace";
            //request.Properties["Format"] = "JSON";
            //request.Properties["Method"] = filterContext.Request.Method.Method;
            //request.Properties["URL"] = filterContext.Request.RequestUri.OriginalString;
            //request.Properties["Controller"] = filterContext.ActionContext.ControllerContext.ControllerDescriptor.ControllerType.FullName;
            //request.Properties["Action"] = ((System.Web.Http.Controllers.ReflectedHttpActionDescriptor)filterContext.ActionContext.ActionDescriptor).ActionName;
            //request.Properties["Parameter"] = Newtonsoft.Json.JsonConvert.SerializeObject(((System.Net.Http.ObjectContent)filterContext.Response.Content).Value);
            //request.Properties["MessageType"] = "Response";
            //request.Properties["Username"] = string.Empty;
            //request.Properties["Token"] = filterContext.Request.Headers.Contains("Token") ? filterContext.Request.Headers.GetValues("Token").FirstOrDefault() : string.Empty;
            //request.Properties["TraceId"] = TraceId;
            //request.Properties["Application"] = "TLGX_WEBAPI";
            //request.Properties["HostIp"] = filterContext.Request.RequestUri.Authority;
            //_logger.Log(request);

            nlog.LogDate = DateTime.Now;
            ctx = filterContext.Request.Properties["MS_HttpContext"] as HttpContextWrapper;
            if (ctx != null)
            {
                nlog.ClientIP = ctx.Request.UserHostAddress;
            }
            nlog.LogType = "Trace";
            nlog.Format = "JSON";
            nlog.Method = filterContext.Request.Method.Method;
            nlog.URL = filterContext.Request.RequestUri.OriginalString;
            nlog.Controller = filterContext.ActionContext.ControllerContext.ControllerDescriptor.ControllerType.FullName;
            nlog.Action = ((System.Web.Http.Controllers.ReflectedHttpActionDescriptor)filterContext.ActionContext.ActionDescriptor).ActionName;
            nlog.Parameter = Newtonsoft.Json.JsonConvert.SerializeObject(((System.Net.Http.ObjectContent)filterContext.Response.Content).Value);
            nlog.MessageType = "Response";
            nlog.Username = string.Empty;
            nlog.Token = filterContext.Request.Headers.Contains("Token") ? filterContext.Request.Headers.GetValues("Token").FirstOrDefault() : string.Empty;
            nlog.TraceId = TraceId;
            nlog.Application = "TLGX_WEBAPI";
            nlog.HostIp = filterContext.Request.RequestUri.Authority;

            request.Message = Newtonsoft.Json.JsonConvert.SerializeObject(nlog);
            _logger.Log(request);

            request = null;
            nlog = null;


        }

        //public override void OnActionExecuted(System.Web.Http.Filters.HttpActionExecutedContext context)
        //{
        //    GlobalConfiguration.Configuration.Services.Replace(typeof(ITraceWriter), new NLogHelper.NLogger());
        //    var trace = GlobalConfiguration.Configuration.Services.GetTraceWriter();
        //    trace.Info(context.Request, "Content : " + context.Response.Content, "JSON", null);
        //}
    }

    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
