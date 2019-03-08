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
            _database = MongoDBHandler.mDatabase();
            var collection = _database.GetCollection<Country>("CountryMaster");
            var result = await collection.Find(bson => true).SortBy(s => s.CountryName).ToListAsync();
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
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
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
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
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
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

        /// <summary>
        /// Retrive list of zones based on search parameters
        /// </summary>
        /// <param name="RQ">A minimum of 3 lettered Zone name is required for successful search.</param>
        /// <returns>
        /// returns List of Zones.
        /// </returns>
        [Route("Zone/Search")]
        [HttpPost]
        [ResponseType(typeof(List<ZoneInfo>))]
        public async Task<HttpResponseMessage> GetZones(ZoneSearchRQ RQ)
        {
            ZoneSearchRS zoneResult = new ZoneSearchRS();

            if (RQ.PageSize == 0)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Page Size should be greater than Zero.");
            }

            _database = MongoDBHandler.mDatabase();
            var collection = _database.GetCollection<ZoneMaster>("ZoneMaster");
            FilterDefinition<ZoneMaster> filterForZone;
            filterForZone = Builders<ZoneMaster>.Filter.Empty;

            if (!string.IsNullOrWhiteSpace(RQ.Zone_name))
            {
                if (RQ.Zone_name.Length >= 3)
                {
                    filterForZone = filterForZone & Builders<ZoneMaster>.Filter.Regex(b => b.Zone_Name, new Regex("^" + RQ.Zone_name.Trim().ToUpper()));
                    //like search
                    //filterForZone = filterForZone & Builders<ZoneMaster>.Filter.Regex(b => b.Zone_Name, new BsonRegularExpression(new Regex(RQ.Zone_name.Trim(), RegexOptions.IgnoreCase)));
                    if (!string.IsNullOrWhiteSpace(RQ.Zone_Type))
                    {
                        filterForZone = filterForZone & Builders<ZoneMaster>.Filter.Eq(b => b.Zone_Type, RQ.Zone_Type.Trim().ToUpper());
                    }
                    if (!string.IsNullOrWhiteSpace(RQ.Zone_SubType))
                    {
                        filterForZone = filterForZone & Builders<ZoneMaster>.Filter.Eq(b => b.Zone_SubType, RQ.Zone_SubType.Trim().ToUpper());
                    }
                    if (!string.IsNullOrWhiteSpace(RQ.SystemCountryCode))
                    {
                        filterForZone = filterForZone & Builders<ZoneMaster>.Filter.Eq(b => b.TLGXCountryCode, RQ.SystemCountryCode.Trim().ToUpper());
                    }
                    var resultCount = await collection.Find(filterForZone).CountDocumentsAsync();
                    //TotalResultReturned
                    int TotalSearchedZone = (int)resultCount;
                    if (TotalSearchedZone > 0)
                    {
                        //for TotalPages in search result
                        int remainder = TotalSearchedZone % RQ.PageSize;
                        int quotient = TotalSearchedZone / RQ.PageSize;
                        if (remainder > 0)
                            remainder = 1;
                        //end
                        zoneResult.PageSize = RQ.PageSize;
                        zoneResult.CurrentPage = RQ.PageNo;
                        zoneResult.TotalNumberOfZones = TotalSearchedZone;
                        zoneResult.TotalPage = quotient + remainder;
                        var result = await collection.Find(filterForZone).Project(x => new ZoneInfo
                        {
                            ZoneId = x._id,
                            TLGXCountryCode = x.TLGXCountryCode,
                            Zone_Name = x.Zone_Name,
                            Zone_SubType = x.Zone_SubType,
                            Zone_Type = x.Zone_Type,
                        }).Skip(RQ.PageNo * RQ.PageSize).Limit(RQ.PageSize).ToListAsync();
                        zoneResult.Zones = result;
                        HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, zoneResult);
                        return response;
                    }
                    else
                    {
                        HttpResponseMessage res = Request.CreateResponse(HttpStatusCode.BadRequest, "No zone found for this filter criteria.");
                        return res;
                    }
                }
                else
                {
                    HttpResponseMessage res = Request.CreateResponse(HttpStatusCode.BadRequest, "ZoneName Should be atleast 3 chars");
                    return res;
                }
            }

            else
            {
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid ZoneName");
                return response;
            }
        }

        /// <summary>
        /// Retrieve Zone Details with filter on System specific ZoneId which is returned from ZoneSearch API.
        /// </summary>
        /// <param name="RQ"></param>
        /// <returns>Details of Zone and Hotels within specified ZoneRange.
        /// As of now we have included hotels within 10Km range. However, this may change in future.
        /// </returns>
        [Route("Zone/ZoneDetail")]
        [HttpPost]
        [ResponseType(typeof(List<ZoneDetails>))]
        public async Task<HttpResponseMessage> GetZoneDetails(ZoneDetailRQ RQ)
        {
            ZoneDetails resultList = new ZoneDetails();
            if (RQ.PageSize == 0)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Page Size should be greater than Zero.");
            }
            _database = MongoDBHandler.mDatabase();
            if (!string.IsNullOrWhiteSpace(RQ.ZoneId))
            {
                var collection = _database.GetCollection<ZoneMaster>("ZoneMaster");
                var searchResult = await collection.Find(x => x._id == RQ.ZoneId.ToUpper()).ToListAsync();
                if (searchResult != null && searchResult.Count > 0)
                {
                    var details = (from z in searchResult select new { z._id, z.Zone_Name, z.Zone_Type, z.Zone_SubType, z.TLGXCountryCode, z.Latitude, z.Longitude, z.Zone_Radius }).FirstOrDefault();
                    if (details != null)
                    {

                        resultList.ZoneId = details._id;
                        resultList.Zone_Name = details.Zone_Name;
                        resultList.Zone_Type = details.Zone_Type;
                        resultList.Zone_SubType = details.Zone_SubType;
                        resultList.SystemCountryCode = details.TLGXCountryCode;
                        resultList.Latitude = details.Latitude;
                        resultList.Longitude = details.Longitude;
                        resultList.Zone_Radius = details.Zone_Radius;
                    }
                    // For Zone-Hotels 
                    var SearchZoneProducts = (from m in searchResult select m.Zone_ProductMapping).ToList();
                    int TotalHotels = SearchZoneProducts[0].Count();
                    resultList.PageSize = RQ.PageSize;
                    resultList.CurrentPage = RQ.PageNo;
                    resultList.TotalNumberOfHotels = TotalHotels;

                    if (TotalHotels > 0)
                    {
                        int remainder = TotalHotels % RQ.PageSize;
                        int quotient = TotalHotels / RQ.PageSize;
                        if (remainder > 0)
                        {
                            remainder = 1;
                        }
                        resultList.TotalPage = quotient + remainder;

                        resultList.ZoneHotels = (from ap in SearchZoneProducts[0]
                                                 select new Zone_ProductMapping
                                                 {
                                                     Distance = ap.Distance,
                                                     IsIncluded = ap.IsIncluded,
                                                     TLGXCompanyHotelID = ap.TLGXCompanyHotelID,
                                                     TLGXHotelName = ap.TLGXHotelName,
                                                     TLGXProductType = ap.TLGXProductType,
                                                     Unit = ap.Unit
                                                 }).Skip(RQ.PageSize * RQ.PageNo).Take(RQ.PageSize).ToList();
                    }
                    else
                    {
                        resultList.TotalPage = 0;
                        resultList.ZoneHotels = new List<Zone_ProductMapping>();
                    }
                    //End
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, resultList);
                    return response;
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "No zone found");
                }

            }
            else
            {
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid Reuest Parameters");
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
        public async Task<HttpResponseMessage> GetZoneTypeMaster()
        {
            _database = MongoDBHandler.mDatabase();
            var collection = _database.GetCollection<ZoneTypeMaster>("ZoneTypeMaster");
            var result = await collection.Find(bson => true).SortBy(s => s.Zone_Type).ToListAsync();
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }
    }
}