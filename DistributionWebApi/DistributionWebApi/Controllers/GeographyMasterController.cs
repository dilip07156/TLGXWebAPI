using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using MongoDB.Driver;
using DistributionWebApi.Mongo;
using DistributionWebApi.Models;
using MongoDB.Bson;
using System.Threading.Tasks;
using System.Web.Http.Description;
using NLog;

namespace DistributionWebApi.Controllers
{
    /// <summary>
    /// Used to retrieve TLGX Master Geographical information.
    /// </summary>
    [RoutePrefix("Masters/Get")]
    public class GeographyMasterController : ApiController
    {
        /// <summary>
        /// Mongo database handler
        /// </summary>
        protected static IMongoDatabase _database;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Retrieve all TLGX Master Countries
        /// </summary>
        /// <returns>List of TLGX Country Masters. Currently restricted to internal Name and Code data.</returns>
        [Route("Countries")]
        [HttpGet]
        [ResponseType(typeof(List<Country>))]
        public async Task<HttpResponseMessage> GetAllContries()
        {
            try
            {
                _database = MongoDBHandler.mDatabase();
                var collection = _database.GetCollection<Country>("CountryMaster");
                var result = await collection.Find(bson => true).SortBy(s => s.CountryName).ToListAsync();
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, result);
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
        /// Retrieve all TLGX Master countries with StartsWith Filter on TLGX Country Code
        /// </summary>
        /// <param name="CountryCode"></param>
        /// <returns>List of TLGX Country Masters. Currently restricted to internal Name and Code data.</returns>
        [Route("Countries/CountryCode/{CountryCode}")]
        [HttpGet]
        [ResponseType(typeof(List<Country>))]
        public async Task<HttpResponseMessage> GetCountriesByCode(string CountryCode)
        {
            try
            {
                _database = MongoDBHandler.mDatabase();
                var collection = _database.GetCollection<Country>("CountryMaster");
                var result = await collection.Find(c => c.CountryCode.ToLower().StartsWith(CountryCode.ToLower())).SortBy(s => s.CountryName).ToListAsync();
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, result);
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
        /// Retrieve all TLGX Master Countries with StartsWith Filter on TLGX Country Name. 
        /// </summary>
        /// <param name="CountryName"></param>
        /// <returns>List of TLGX Country Masters. Currently restricted to internal Name and Code data.</returns>
        [Route("Countries/CountryName/{CountryName}")]
        [HttpGet]
        [ResponseType(typeof(List<Country>))]
        public async Task<HttpResponseMessage> GetCountriesByName(string CountryName)
        {
            try
            {
                _database = MongoDBHandler.mDatabase();
                var collection = _database.GetCollection<Country>("CountryMaster");
                var result = await collection.Find(c => c.CountryName.ToLower().StartsWith(CountryName.ToLower())).SortBy(s => s.CountryName).ToListAsync();
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, result);
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
        /// Retrieve all TLGX System Cities
        /// </summary>
        /// <returns>List of TLGX City Masters. Currently restricted to internal Name and Code data.</returns>
        [Route("Cities")]
        [HttpGet]
        [ResponseType(typeof(List<City>))]
        public async Task<HttpResponseMessage> GetAllCities()
        {
            try
            {
                _database = MongoDBHandler.mDatabase();
                var collection = _database.GetCollection<City>("CityMaster");
                var result = await collection.Find(c => true).SortBy(s => s.CityName).ToListAsync();
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, result);
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
        /// Retrieve all TLGX System Cities with StartsWith Filter on TLGX Country Code
        /// </summary>
        /// <param name="CountryCode"></param>
        /// <returns>List of TLGX City Masters. Currently restricted to internal Name and Code data.</returns>
        [Route("Cities/CountryCode/{CountryCode}")]
        [HttpGet]
        [ResponseType(typeof(List<City>))]
        public async Task<HttpResponseMessage> GetCityByCountryCode(string CountryCode)
        {
            try
            {
                _database = MongoDBHandler.mDatabase();
                var collection = _database.GetCollection<City>("CityMaster");
                var result = await collection.Find(c => c.CountryCode.ToLower().StartsWith(CountryCode.ToLower())).SortBy(s => s.CityName).ToListAsync();
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, result);
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
        /// Retrieve all TLGX System Cities with StartsWith Filter on TLGX Country Name
        /// </summary>
        /// <param name="CountryName"></param>
        /// <returns>List of TLGX City Masters. Currently restricted to internal Name and Code data.</returns>
        [Route("Cities/CountryName/{CountryName}")]
        [HttpGet]
        [ResponseType(typeof(List<City>))]
        public async Task<HttpResponseMessage> GetCityByCountryName(string CountryName)
        {
            try
            {
                _database = MongoDBHandler.mDatabase();
                var collection = _database.GetCollection<City>("CityMaster");
                var result = await collection.Find(c => c.CountryName.ToLower().StartsWith(CountryName.ToLower())).SortBy(s => s.CityName).ToListAsync();
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, result);
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