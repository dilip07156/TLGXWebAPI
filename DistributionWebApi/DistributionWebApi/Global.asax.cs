﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using static DistributionWebApi.WebApiConfig;

namespace DistributionWebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            NLog.Targets.Target.Register("ELK", typeof(DistributionWebApi.App_Start.NLogELKTargetWithProxy)); //dynamic

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Serialize;
            //GlobalConfiguration.Configuration.Formatters.Remove(GlobalConfiguration.Configuration.Formatters.XmlFormatter);

            GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();

            GlobalConfiguration.Configuration.Formatters.Add(new CsvMediaTypeFormatter(new QueryStringMapping("format", "csv", "text/csv")));
            GlobalConfiguration.Configuration.Formatters.Add(new TextPipeMediaTypeFormatter(new QueryStringMapping("format", "pipe", "text/csv")));
            GlobalConfiguration.Configuration.Formatters.Add(new JsonpFormatter());
            GlobalConfiguration.Configuration.Formatters.Add(new JsonDownLoadFormatter(new QueryStringMapping("format", "json", "application/json")));
            GlobalConfiguration.Configuration.Formatters.Add(new XmlDownLoadFormatter(new QueryStringMapping("format", "xml", "application/xml")));

            //GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();
            //GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();
            //GlobalConfiguration.Configuration.Formatters.JsonFormatter.UseDataContractJsonSerializer = false;

            //GlobalConfiguration.Configuration.Formatters.JsonFormatter.MediaTypeMappings.Add(new UriPathExtensionMapping("json", "application/json"));

            //GlobalConfiguration.Configuration.Formatters.XmlFormatter.MediaTypeMappings.Add(new UriPathExtensionMapping("xml", "application/xml"));

            //log4net.Config.XmlConfigurator.Configure();

            GlobalConfiguration.Configuration.Formatters.XmlFormatter.UseXmlSerializer = true;

            GlobalConfiguration.Configuration.Filters.Add(new LoggingFilterAttribute());

            //GlobalConfiguration.Configuration.Filters.Add(new CacheFilterConfig());

            GlobalConfiguration.Configuration.Filters.Add(new ExceptionFilterConfig());

            GlobalConfiguration.Configuration.Services.Replace(typeof(IExceptionHandler), new GlobalWapiExceptionHandler());
        }
    }
}
