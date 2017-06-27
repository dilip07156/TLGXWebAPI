using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;

namespace DistributionWebApi
{
    public class CacheFilterConfig : ActionFilterAttribute
    {
        public int MaxAge { get; set; }

        public CacheFilterConfig()
        {
            MaxAge = 24 * 7 * 60;
        }
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Response != null)
            {
                actionExecutedContext.Response.Headers.CacheControl = new System.Net.Http.Headers.CacheControlHeaderValue
                {
                    MaxAge = TimeSpan.FromSeconds(MaxAge),
                    //MustRevalidate = true,
                    Public = true

                };

                base.OnActionExecuted(actionExecutedContext);
            }

        }
    }
}