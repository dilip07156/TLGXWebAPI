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
        public ActionResult Index()
        {

            var node = new Uri(ConfigurationManager.AppSettings["ElasticUri"]);
            var settings = new Nest.ConnectionSettings(node);
            settings.ThrowExceptions(alwaysThrow: true); // I like exceptions
            settings.PrettyJson(); // Good for DEBUG
            var client = new Nest.ElasticClient(settings);

            return View();
        }
    }
}
