using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;

namespace DistributionWebApi.App_Start
{
    public static class SetupFiltersExtensions
    {
        public static IAppBuilder SetupFilters(this IAppBuilder builder, HttpConfiguration config)
        {
            config.Services.Replace(typeof(IExceptionHandler), new GlobalWapiExceptionHandler());
            return builder;
        }
    }
}