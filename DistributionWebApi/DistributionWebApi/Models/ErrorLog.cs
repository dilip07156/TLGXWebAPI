using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nest;
namespace DistributionWebApi.Models
{
    [ElasticIndexDetails("nakwapiperferror", true)]
    [ElasticsearchType(Name = "log")]
    public class ErrorLog : EntityBase
    {
        public DateTime LogDate { get; set; }
        public string LogType { get; set; }
        public string URL { get; set; }
        public string Application { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public string InnerException { get; set; }
        public string Source { get; set; }
        public string TargetSite { get; set; }
    }
}