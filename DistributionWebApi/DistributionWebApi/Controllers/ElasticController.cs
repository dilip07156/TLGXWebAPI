using DistributionWebApi.App_Start;
using DistributionWebApi.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace DistributionWebApi.Controllers
{
    public class ElasticController : Controller
    {
        private RepositoryBase<TraceLog, string> _logRepo;

        public ActionResult Index()
        {
            var Environment = System.Configuration.ConfigurationManager.AppSettings["CurrentEnvironment"];
            if (Environment != "PERF")
            {
                Environment = string.Empty;
            }
            _logRepo = new RepositoryBase<TraceLog, string>();
            var data = _logRepo.GetAll();

            return View(data);
        }
    }
}
