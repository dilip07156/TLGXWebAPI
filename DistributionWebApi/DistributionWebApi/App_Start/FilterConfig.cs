﻿using System.Web;
using System.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Filters;
using System.Web.Http.Controllers;
using System.Web.Http.Tracing;
using System.Web.Http;
using NLog;
using System.Net.Http;
using System.Diagnostics;
using Newtonsoft.Json.Linq;

namespace DistributionWebApi
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class LoggingFilterAttribute : System.Web.Http.Filters.ActionFilterAttribute
    {
        private static Logger _logger = LogManager.GetLogger("Trace");
        public override void OnActionExecuting(HttpActionContext filterContext)
        {
            DateTime RequestDatetime = DateTime.Now;

            LogEventInfo request = new LogEventInfo(LogLevel.Trace, "Trace", string.Empty);
            Models.TraceLog nlog = new Models.TraceLog();

            filterContext.Request.Properties[filterContext.ActionDescriptor.ActionName] = RequestDatetime;

            nlog.LogDate = RequestDatetime;

            var ctx = filterContext.Request.Properties["MS_HttpContext"] as HttpContextWrapper;
            if (ctx != null)
            {
                nlog.ClientIP = ctx.Request.UserHostAddress;
            }
            nlog.LogType = "Trace";
            nlog.Format = "JSON";
            nlog.Method = filterContext.Request.Method.Method;
            nlog.URL = filterContext.Request.RequestUri.OriginalString;
            nlog.Controller = filterContext.ControllerContext.ControllerDescriptor.ControllerType.FullName;
            nlog.Action = filterContext.ActionDescriptor.ActionName;
            nlog.Parameter = Newtonsoft.Json.JsonConvert.SerializeObject(filterContext.ActionArguments);
            nlog.MessageType = "Request";
            nlog.Username = string.Empty;
            nlog.Token = filterContext.Request.Headers.Contains("Token") ? filterContext.Request.Headers.GetValues("Token").FirstOrDefault() : string.Empty;
            nlog.TraceId = filterContext.Request.GetCorrelationId().ToString();
            nlog.Application = "TLGX_WEBAPI";
            nlog.HostIp = filterContext.Request.RequestUri.Authority;


            if (nlog.Method.ToUpper() == "POST")
            {
                try
                {
                    var token = JToken.Parse(nlog.Parameter);
                    nlog.TotalRecords = token.SelectTokens("$.RQ.[*]").Count();
                }
                catch
                {
                    nlog.TotalRecords = 0;
                }
            }
            else
            {
                nlog.TotalRecords = 0;
            }


            request.Message = Newtonsoft.Json.JsonConvert.SerializeObject(nlog);
            _logger.Log(request);

        }

        public override void OnActionExecuted(HttpActionExecutedContext filterContext)
        {
            DateTime ResponseDatetime = DateTime.Now;
            DateTime RequestDatetime = (DateTime)filterContext.Request.Properties[filterContext.ActionContext.ActionDescriptor.ActionName];

            LogEventInfo request = new LogEventInfo(LogLevel.Trace, "Trace", string.Empty);
            Models.TraceLog nlog = new Models.TraceLog();

            nlog.LogDate = ResponseDatetime;
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
            nlog.Parameter = Newtonsoft.Json.JsonConvert.SerializeObject(((System.Net.Http.ObjectContent)filterContext.Response.Content).Value);
            nlog.MessageType = "Response";
            nlog.Username = string.Empty;
            nlog.Token = filterContext.Request.Headers.Contains("Token") ? filterContext.Request.Headers.GetValues("Token").FirstOrDefault() : string.Empty;
            nlog.TraceId = filterContext.Request.GetCorrelationId().ToString();
            nlog.Application = "TLGX_WEBAPI";
            nlog.HostIp = filterContext.Request.RequestUri.Authority;
            nlog.TotalRecords = 0;
            nlog.ResponseTime = (ResponseDatetime - RequestDatetime).Milliseconds;

            try
            {
                var token = JToken.Parse(nlog.Parameter);
                nlog.TotalRecords = token.SelectTokens("$.[*]").Count();
            }
            catch
            {
                nlog.TotalRecords = 0;
            }

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
