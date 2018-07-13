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
using System.Text;
using System.Text.RegularExpressions;

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
                var result = await collection.Find(c => c.CountryCode == CountryCode.Trim().ToUpper()).SortBy(s => s.CountryName).ToListAsync();
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
                var result = await collection.Find(c => c.CountryName == CountryName.Trim().ToUpper()).SortBy(s => s.CountryName).ToListAsync();
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
                var result = await collection.Find(c => c.CountryCode == CountryCode.Trim().ToUpper()).SortBy(s => s.CityName).ToListAsync();
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
                var result = await collection.Find(c => c.CountryName.ToUpper() == CountryName.Trim().ToUpper()).SortBy(s => s.CityName).ToListAsync();
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
        /// Retrieve all TLGX System States
        /// </summary>
        /// <returns>List of TLGX State Masters. Currently restricted to internal Name and Code data.</returns>
        [Route("States")]
        [HttpGet]
        [ResponseType(typeof(List<State>))]
        public async Task<HttpResponseMessage> GetAllStates()
        {
            try
            {
                _database = MongoDBHandler.mDatabase();
                var collection = _database.GetCollection<State>("StateMaster");
                var result = await collection.Find(c => true).SortBy(s => s.StateName).ToListAsync();
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
        /// Retrieve all TLGX System States with StartsWith Filter on TLGX Country Code
        /// </summary>
        /// <param name="CountryCode"></param>
        /// <returns>List of TLGX State Masters. Currently restricted to internal Name and Code data.</returns>
        [Route("States/CountryCode/{CountryCode}")]
        [HttpGet]
        [ResponseType(typeof(List<State>))]
        public async Task<HttpResponseMessage> GetStateByCountryCode(string CountryCode)
        {
            try
            {
                _database = MongoDBHandler.mDatabase();
                var collection = _database.GetCollection<State>("StateMaster");
                var result = await collection.Find(c => c.CountryCode.ToUpper() == CountryCode.Trim().ToUpper()).SortBy(s => s.StateName).ToListAsync();
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
        /// Retrieve all active OAG Ports
        /// </summary>
        /// <returns>List of active OAG Port Masters.</returns>
        [Route("Ports")]
        [HttpGet]
        [ResponseType(typeof(List<Port>))]
        public async Task<HttpResponseMessage> GetAllPorts()
        {
            try
            {
                _database = MongoDBHandler.mDatabase();
                var collection = _database.GetCollection<Port>("PortMaster");
                var result = await collection.Find(c => c.oag_inactive_indicator != "I").SortBy(s => s.oag_port_name).ToListAsync();

                //var returnResult = (from a in result
                //                    select new Active_Port
                //                    {
                //                        oag_location_code = a.oag_location_code,
                //                        oag_multi_airport_citycode = a.oag_multi_airport_citycode,
                //                        oag_location_type_code = a.oag_location_type_code,
                //                        oag_location_type = a.oag_location_type,
                //                        oag_location_subtype_code = a.oag_location_subtype_code,
                //                        oag_location_subtype = a.oag_location_subtype,
                //                        oag_location_name = a.oag_location_name,
                //                        oag_port_name = a.oag_port_name,
                //                        oag_country_code = a.oag_country_code,
                //                        oag_country_subcode = a.oag_country_subcode,
                //                        oag_country_name = a.oag_country_name,
                //                        oag_state_code = a.oag_state_code,
                //                        oag_state_subcode = a.oag_state_subcode,
                //                        oag_time_division = a.oag_time_division,
                //                        oag_latitiude = a.oag_latitiude,
                //                        oag_longitude = a.oag_longitude,
                //                        tlgx_country_code = a.tlgx_country_code,
                //                        tlgx_country_name = a.tlgx_country_name,
                //                        tlgx_state_code = a.tlgx_state_code,
                //                        tlgx_state_name = a.tlgx_state_name,
                //                        tlgx_city_code = a.tlgx_city_code,
                //                        tlgx_city_name = a.tlgx_city_name
                //                    }).ToList();

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
        /// Retrieve all active OAG Ports with filter on Country Code
        /// </summary>
        /// <param name="CountryCode"></param>
        /// <returns>List of active OAG Port Masters.</returns>
        [Route("Ports/CountryCode/{CountryCode}")]
        [HttpGet]
        [ResponseType(typeof(List<Port>))]
        public async Task<HttpResponseMessage> GetPortByCountryCode(string CountryCode)
        {
            try
            {
                _database = MongoDBHandler.mDatabase();
                var collection = _database.GetCollection<Port>("PortMaster");
                var result = await collection.Find(c => c.oag_inactive_indicator != "I" && c.oag_country_code.ToUpper() == CountryCode.Trim().ToUpper()).SortBy(s => s.oag_port_name).ToListAsync();

                //var returnResult = (from a in result
                //                    select new Active_Port
                //                    {
                //                        oag_location_code = a.oag_location_code,
                //                        oag_multi_airport_citycode = a.oag_multi_airport_citycode,
                //                        oag_location_type_code = a.oag_location_type_code,
                //                        oag_location_type = a.oag_location_type,
                //                        oag_location_subtype_code = a.oag_location_subtype_code,
                //                        oag_location_subtype = a.oag_location_subtype,
                //                        oag_location_name = a.oag_location_name,
                //                        oag_port_name = a.oag_port_name,
                //                        oag_country_code = a.oag_country_code,
                //                        oag_country_subcode = a.oag_country_subcode,
                //                        oag_country_name = a.oag_country_name,
                //                        oag_state_code = a.oag_state_code,
                //                        oag_state_subcode = a.oag_state_subcode,
                //                        oag_time_division = a.oag_time_division,
                //                        oag_latitiude = a.oag_latitiude,
                //                        oag_longitude = a.oag_longitude,
                //                        tlgx_country_code = a.tlgx_country_code,
                //                        tlgx_country_name = a.tlgx_country_name,
                //                        tlgx_state_code = a.tlgx_state_code,
                //                        tlgx_state_name = a.tlgx_state_name,
                //                        tlgx_city_code = a.tlgx_city_code,
                //                        tlgx_city_name = a.tlgx_city_name
                //                    }).ToList();

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
        /// Retrive zones based on  search parameters
        /// </summary>
        /// <param name="RQ"></param>
        /// <returns>
        /// returns List of Zones.
        /// </returns>
        [Route("Zone/Search")]
        [HttpPost]
        [ResponseType(typeof(List<Zone>))]
        public async Task<HttpResponseMessage> GetZones(ZoneSearchRQ RQ)
        {
            try
            {
                _database = MongoDBHandler.mDatabase();
                var collection = _database.GetCollection<Zone>("ZoneMaster");
                FilterDefinition<Zone> filterForZone;
                filterForZone = Builders<Zone>.Filter.Empty;
               
                if (!string.IsNullOrWhiteSpace(RQ.Zone_name))
                {
                    if(RQ.Zone_name.Length >= 3)
                    {
                        filterForZone = filterForZone & Builders<Zone>.Filter.Regex(b=>b.Zone_Name,new Regex("^"+RQ.Zone_name.Trim().ToUpper()));
                        //like search
                        //filterForZone = filterForZone & Builders<Zone>.Filter.Regex(b => b.Zone_Name, new BsonRegularExpression(new Regex(RQ.Zone_name.Trim(), RegexOptions.IgnoreCase)));
                        if (!string.IsNullOrWhiteSpace(RQ.Zone_Type))
                        {
                            filterForZone = filterForZone & Builders<Zone>.Filter.Eq(b=>b.Zone_Type, RQ.Zone_Type.Trim().ToUpper());
                        }
                        if (!string.IsNullOrWhiteSpace(RQ.Zone_SubType))
                        {
                            filterForZone = filterForZone & Builders<Zone>.Filter.Eq(b => b.Zone_SubType, RQ.Zone_SubType.Trim().ToUpper());
                        }
                        if (!string.IsNullOrWhiteSpace(RQ.SystemCountryCode))
                        {
                            filterForZone = filterForZone & Builders<Zone>.Filter.Eq(b=>b.TLGXCountryCode, RQ.SystemCountryCode.Trim().ToUpper());
                        }
                        var result = await collection.Find(filterForZone).ToListAsync();

                        HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, result);
                        return response;
                    }
                    else
                    {
                        HttpResponseMessage res = Request.CreateResponse(HttpStatusCode.BadRequest, "ZoneName Should be atleast 3 chars");
                        return res;
                    }   
                }
                else
                {
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.BadRequest,"Invalid ZoneName");
                    return response;
                }

            }
            catch (Exception ex)
            {
                NLogHelper.Nlogger_LogError.LogError(ex, this.GetType().FullName, Request.GetActionDescriptor().ActionName, Request.RequestUri.PathAndQuery);
                HttpResponseMessage response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Server Error. Contact Admin. Error Date : " + DateTime.Now.ToString());
                return response;
            }
        }
        /// <summary>
        /// Retrive all ZoneTypes and SubTypes
        /// </summary>
        /// <returns></returns>
        [Route("ZoneTypeMaster")]
        [HttpGet]
        [ResponseType(typeof(List<ZoneTypeMaster>))]
        public async Task <HttpResponseMessage> GetZoneTypeMaster()
        {
            try
            {
                _database = MongoDBHandler.mDatabase();
                var collection = _database.GetCollection<ZoneTypeMaster>("ZoneTypeMaster");
                var result = await collection.Find(bson=>true).SortBy(s => s.Zone_Type).ToListAsync();
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, result);
                return response;
            }
            catch(Exception ex)
            {
                NLogHelper.Nlogger_LogError.LogError(ex, this.GetType().FullName, Request.GetActionDescriptor().ActionName, Request.RequestUri.PathAndQuery);
                HttpResponseMessage response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Server Error. Contact Admin. Error Date : " + DateTime.Now.ToString());
                return response;
            }

        }
    }
}