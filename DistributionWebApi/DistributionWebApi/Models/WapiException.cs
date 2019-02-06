using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace DistributionWebApi.Models
{
    public class WapiException : Exception
    {
        public WapiException()
        {
        }

        public WapiException(HttpStatusCode statusCode, string errorCode, string errorDescription) : base($"{errorCode}::{errorDescription}")
        {
            StatusCode = statusCode;
        }

        public WapiException(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
        }

        public HttpStatusCode StatusCode { get; }
    }
}