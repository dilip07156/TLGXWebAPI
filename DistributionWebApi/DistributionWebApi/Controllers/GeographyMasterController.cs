using DistributionWebApi.Models;
using DistributionWebApi.Mongo;
using DistributionWebApi.ZoneModels;
using MongoDB.Bson;
using MongoDB.Driver;
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
            _database = MongoDBHandler.mDatabase();
            var collection = _database.GetCollection<Country>("CountryMaster");
            var result = await collection.Find(bson => true).SortBy(s => s.CountryName).ToListAsync();
            return Request.CreateResponse(HttpStatusCode.OK, result);
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
            _database = MongoDBHandler.mDatabase();
            var collection = _database.GetCollection<Country>("CountryMaster");
            var result = await collection.Find(c => c.CountryCode == CountryCode.Trim().ToUpper()).SortBy(s => s.CountryName).ToListAsync();
            return Request.CreateResponse(HttpStatusCode.OK, result);
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
            _database = MongoDBHandler.mDatabase();
            var collection = _database.GetCollection<Country>("CountryMaster");
            var result = await collection.Find(c => c.CountryName == CountryName.Trim().ToUpper()).SortBy(s => s.CountryName).ToListAsync();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Retrieve all TLGX Master continent with StartsWith Filter on TLGX continent Code
        /// </summary>
        /// <param name="ContinentCode">Passing continent code as parameter</param>
        /// <returns>List of TLGX Country Masters. Currently restricted to internal Name and Code data.</returns>
        [Route("Countries/ContinentCode/{ContinentCode}")]
        [HttpGet]
        [ResponseType(typeof(List<Country>))]
        public async Task<HttpResponseMessage> GetContinentByContinentCode(string ContinentCode)
        {
            _database = MongoDBHandler.mDatabase();
            var collection = _database.GetCollection<Country>("CountryMaster");
            var result = await collection.Find(c => c.ContinentCode == ContinentCode.Trim().ToUpper()).SortBy(s => s.CountryName).ToListAsync();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Retrieve all TLGX Master continent with StartsWith Filter on TLGX continent Name. 
        /// </summary>
        /// <param name="ContinentName">Passing continent name as parameter</param>
        /// <returns>List of TLGX Country Masters. Currently restricted to internal Name and Code data.</returns>
        [Route("Countries/ContinentName/{ContinentName}")]
        [HttpGet]
        [ResponseType(typeof(List<Country>))]
        public async Task<HttpResponseMessage> GetContinentByContinentName(string ContinentName)
        {
            _database = MongoDBHandler.mDatabase();
            var collection = _database.GetCollection<Country>("CountryMaster");
            var result = await collection.Find(c => c.ContinentName == ContinentName.Trim().ToUpper()).SortBy(s => s.CountryName).ToListAsync();
            return Request.CreateResponse(HttpStatusCode.OK, result);
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
            _database = MongoDBHandler.mDatabase();
            var collection = _database.GetCollection<City>("CityMaster");
            var result = await collection.Find(c => true).SortBy(s => s.CityName).ToListAsync();
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
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
            _database = MongoDBHandler.mDatabase();
            var collection = _database.GetCollection<City>("CityMaster");
            var result = await collection.Find(c => c.CountryCode == CountryCode.Trim().ToUpper()).SortBy(s => s.CityName).ToListAsync();
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
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
            _database = MongoDBHandler.mDatabase();
            var collection = _database.GetCollection<City>("CityMaster");
            var result = await collection.Find(c => c.CountryName.ToUpper() == CountryName.Trim().ToUpper()).SortBy(s => s.CityName).ToListAsync();
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }

        /// <summary>
        /// Retrieve all TLGX System Cities with StartsWith Filter on TLGX continent Code
        /// </summary>
        /// <param name="ContinentCode">Passing continent code as parameter</param>
        /// <returns>List of TLGX City Masters. Currently restricted to internal Name and Code data.</returns>
        [Route("Cities/ContinentCode/{ContinentCode}")]
        [HttpGet]
        [ResponseType(typeof(List<City>))]
        public async Task<HttpResponseMessage> GetCityByContinentCode(string ContinentCode)
        {
            _database = MongoDBHandler.mDatabase();
            var collection = _database.GetCollection<City>("CityMaster");
            var result = await collection.Find(c => c.ContinentCode == ContinentCode.Trim().ToUpper()).SortBy(s => s.CityName).ToListAsync();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Retrieve all TLGX System Cities with StartsWith Filter on TLGX continent Name
        /// </summary>
        /// <param name="ContinentName">Passing continent name as parameter</param>
        /// <returns>List of TLGX City Masters. Currently restricted to internal Name and Code data.</returns>
        [Route("Cities/ContinentName/{ContinentName}")]
        [HttpGet]
        [ResponseType(typeof(List<City>))]
        public async Task<HttpResponseMessage> GetCityByContinentName(string ContinentName)
        {
            _database = MongoDBHandler.mDatabase();
            var collection = _database.GetCollection<City>("CityMaster");
            var result = await collection.Find(c => c.ContinentName == ContinentName.Trim().ToUpper()).SortBy(s => s.CityName).ToListAsync();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Retrieve all TLGX System States
        /// </summary>
        /// <returns>List of TLGX State Masters. Currently restricted to internal Name and Code data.</returns>
        [Route("States")]
        [HttpGet]
        [ResponseType(typeof(List<State>))]
        public async Task<HttpResponseMessage> GetAllStates()
        {
            _database = MongoDBHandler.mDatabase();
            var collection = _database.GetCollection<State>("StateMaster");
            var result = await collection.Find(c => true).SortBy(s => s.StateName).ToListAsync();
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }

        /// <summary>
        /// Retrieve all TLGX System States with StartsWith Filter on TLGX Country Code
        /// </summary>
        /// <param name="CountryCode"></param>
        /// <returns>List of TLGX State Masters. Currently restricted to internal Name and Code data.</returns>
        [Route("States/CountryCode/{CountryCode}")]
        [HttpGet]
        [ResponseType(typeof(List<State>))]
        public async Task<HttpResponseMessage> GetStateByCountryCode(string CountryCode)
        {
            _database = MongoDBHandler.mDatabase();
            var collection = _database.GetCollection<State>("StateMaster");
            var result = await collection.Find(c => c.CountryCode.ToUpper() == CountryCode.Trim().ToUpper()).SortBy(s => s.StateName).ToListAsync();
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }


        /// <summary>
        /// Retrieve all active OAG Ports
        /// </summary>
        /// <returns>List of active OAG Port Masters.</returns>
        [Route("Ports")]
        [HttpGet]
        [ResponseType(typeof(List<Port>))]
        public async Task<HttpResponseMessage> GetAllPorts()
        {
            _database = MongoDBHandler.mDatabase();
            var collection = _database.GetCollection<Port>("PortMaster");
            var result = await collection.Find(c => c.oag_inactive_indicator != "I").SortBy(s => s.oag_port_name).ToListAsync();
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }

        /// <summary>
        /// Retrieve all active OAG Ports with filter on Country Code
        /// </summary>
        /// <param name="CountryCode"></param>
        /// <returns>List of active OAG Port Masters.</returns>
        [Route("Ports/CountryCode/{CountryCode}")]
        [HttpGet]
        [ResponseType(typeof(List<Port>))]
        public async Task<HttpResponseMessage> GetPortByCountryCode(string CountryCode)
        {
            _database = MongoDBHandler.mDatabase();
            var collection = _database.GetCollection<Port>("PortMaster");
            var result = await collection.Find(c => c.oag_inactive_indicator != "I" && c.oag_country_code.ToUpper() == CountryCode.Trim().ToUpper()).SortBy(s => s.oag_port_name).ToListAsync();
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }

       
    }

}