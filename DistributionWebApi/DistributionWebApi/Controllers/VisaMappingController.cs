using DistributionWebApi.Models;
using DistributionWebApi.Models.VisaMapping;
using DistributionWebApi.Mongo;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace DistributionWebApi.Controllers
{
    [RoutePrefix("VisaMapping/Get")]
    public class VisaMappingController : ApiController
    {
        /// <summary>
        /// static object of Mongo DB Class
        /// </summary>
        protected static IMongoDatabase _database;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        
        /// <summary>
        /// This will return all master key value pair related to Visa
        /// </summary>
        /// <param name="CountryCode"></param>
        /// <returns>A key value pair of Visa master attribute type and attribute values</returns>
        [Route("ByCountryCode")]
        [HttpGet]
        [ResponseType(typeof(VisaDefinition))]
        public async Task<HttpResponseMessage> GetVisaDetailByCountries(string CountryCode)
        {
            try
            {
                _database = MongoDBHandler.mDatabase();

                IMongoCollection<VisaDefinition> collectionVisa = _database.GetCollection<VisaDefinition>("VisaCountryDetail");

                var searchResult = await collectionVisa.Find(s => s.VisaDetail.CountryCode == CountryCode).FirstOrDefaultAsync();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, searchResult);
                return response;

            }
            catch (Exception ex)
            {
                NLogHelper.Nlogger_LogError.LogError(ex, this.GetType().FullName, Request.GetActionDescriptor().ActionName, Request.RequestUri.PathAndQuery);
                HttpResponseMessage response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Server Error. Contact Admin. Error Date : " + DateTime.Now.ToString());
                return response;
            }
        }

        /// <summary>
        /// This will return single Visa detail  key value pair related to CountryCode
        /// </summary>
        /// <returns>A key value pair of Visa master attribute type and attribute values</returns>
        [Route("All")]
        [HttpGet]
        [ResponseType(typeof(List<VisaDefinition>))]
        public async Task<HttpResponseMessage> GetVisaMasters()
        {
            try
            {
                _database = MongoDBHandler.mDatabase();

                IMongoCollection<VisaDefinition> collectionVisa = _database.GetCollection<VisaDefinition>("VisaCountryDetail");

                var searchResult = await collectionVisa.Find(s => true).FirstOrDefaultAsync();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, searchResult);
                return response;

            }
            catch (Exception ex)
            {
                NLogHelper.Nlogger_LogError.LogError(ex, this.GetType().FullName, Request.GetActionDescriptor().ActionName, Request.RequestUri.PathAndQuery);
                HttpResponseMessage response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Server Error. Contact Admin. Error Date : " + DateTime.Now.ToString());
                return response;
            }
        }


      
    }
}
