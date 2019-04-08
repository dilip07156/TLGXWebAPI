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
using System.Net.Http;
using System.Diagnostics;
using Newtonsoft.Json.Linq;

namespace DistributionWebApi
{
    /// <summary>
    /// This class will be handle all incoming and outgoing message filter
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class LoggingFilterAttribute : System.Web.Http.Filters.ActionFilterAttribute
    {
        private static Logger _logger = LogManager.GetLogger("Trace");
        /// <summary>
        /// This method executed after a request is done.
        /// </summary>
        /// <param name="filterContext"></param>
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
                    if (filterContext.ActionDescriptor.ActionName == "CompanySpecificHotelAndRoomTypeMapping")
                    {
                        nlog.TotalRecords = token.SelectTokens("$.RQ.MappingRequests..SupplierRoomTypes").Count();
                    }
                    else
                    {
                        nlog.TotalRecords = token.SelectTokens("$.RQ.[*]").Count();
                    }
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

        /// <summary>
        /// This method is executed after the request is processed and ready for the response
        /// </summary>
        /// <param name="filterContext"></param>
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
            if (filterContext.Response != null)
            {
                nlog.Parameter = Newtonsoft.Json.JsonConvert.SerializeObject(((System.Net.Http.ObjectContent)filterContext.Response.Content).Value);
                nlog.Token = filterContext.Request.Headers.Contains("Token") ? filterContext.Request.Headers.GetValues("Token").FirstOrDefault() : string.Empty;
                nlog.TraceId = filterContext.Request.GetCorrelationId().ToString();
                nlog.HostIp = filterContext.Request.RequestUri.Authority;
            }
            nlog.MessageType = "Response";
            nlog.Username = string.Empty;
            nlog.Application = "TLGX_WEBAPI";
            nlog.TotalRecords = 0;
            nlog.ResponseTime = (ResponseDatetime - RequestDatetime).TotalMilliseconds;

            try
            {
                var token = JToken.Parse(nlog.Parameter);
                if(((System.Web.Http.Controllers.ReflectedHttpActionDescriptor)filterContext.ActionContext.ActionDescriptor).ActionName == "CompanySpecificHotelAndRoomTypeMapping")
                {
                    nlog.TotalRecords = token.SelectTokens("$.MappingResponses..SupplierRoomTypes..MappedRooms").Count();
                }
                else
                {
                    nlog.TotalRecords = token.SelectTokens("$.[*]").Count();
                }
                
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

    /// <summary>
    /// 
    /// </summary>
    public class FilterConfig
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filters"></param>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
