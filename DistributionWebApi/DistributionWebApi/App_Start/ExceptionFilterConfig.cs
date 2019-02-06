using DistributionWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;

namespace DistributionWebApi
{
    public class ExceptionFilterConfig : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            var exception = context.Exception as WapiException;
            if (exception != null)
            {
                context.Response = context.Request.CreateErrorResponse(exception.StatusCode, exception.Message);
            }
        }
    }
}