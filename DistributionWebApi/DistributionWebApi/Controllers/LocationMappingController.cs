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
    /// Used to retrieve TLGX Location Mapping Search Data.
    /// </summary>
    [RoutePrefix("Masters/Get")]
    public class LocationMappingController : ApiController
    {
        /// <summary>
        /// Mongo database handler
        /// </summary>
        protected static IMongoDatabase _database;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

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
                    if (!string.IsNullOrWhiteSpace(RQ.Zone_Code))
                    {
                        filterForZone = filterForZone & Builders<ZoneMaster>.Filter.Eq(b => b.Zone_Code, RQ.Zone_Code.Trim().ToUpper());
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
                            Zone_Code = x.Zone_Code
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
                    var details = (from z in searchResult select new { z._id, z.Zone_Name, z.Zone_Type, z.Zone_SubType, z.TLGXCountryCode, z.Latitude, z.Longitude, z.Zone_Radius, z.Zone_Code }).FirstOrDefault();
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
                        resultList.Zone_Code = details.Zone_Code;
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


        /// <summary>
        /// Retrive list of zones based on search parameters like Zone Name Char Basis,Zone Type,Zone Sub Type ,Zone Country Code or Cities Code.
        /// </summary>
        /// <param name="RQ">A minimum of 3 lettered Zone name is required for successful search.</param>        
        /// <returns>
        /// returns List of Zones.
        /// </returns>
        [Route("Zone/ZoneListByCharBasis")]
        [HttpPost]
        [ResponseType(typeof(List<ZoneSearchResponse>))]
        public async Task<HttpResponseMessage> GetZoneListByCharBasis(ZoneSearchRequest RQ)
        {
            try
            {
                List<ZoneSearchResponse> zoneResult = new List<ZoneSearchResponse>();

                if (RQ.PageSize == 0)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Page Size should be greater than Zero.");
                }
                else if (RQ.PageNo == 0)
                {
                    RQ.PageNo = 1;
                }

                _database = MongoDBHandler.mDatabase();
                var collection = _database.GetCollection<ZoneMaster>("ZoneMaster");
                FilterDefinition<ZoneMaster> filterForZone;
                filterForZone = Builders<ZoneMaster>.Filter.Empty;
                if (RQ.Zone_name.Length >= 3)
                {
                    filterForZone = filterForZone & Builders<ZoneMaster>.Filter.Regex(b => b.Zone_Name, new BsonRegularExpression(new Regex(RQ.Zone_name.Trim(), RegexOptions.IgnoreCase)));
                }

                if (RQ.ZoneTypes.Any())
                {
                    filterForZone = filterForZone & Builders<ZoneMaster>.Filter.In(b => b.Zone_Type, RQ.ZoneTypes.Select(x => x.Zone_Type.Trim().ToUpper()));
                }
                if (RQ.Zone_SubType.Any())
                {
                    filterForZone = filterForZone & Builders<ZoneMaster>.Filter.In(b => b.Zone_SubType, RQ.Zone_SubType.Select(x => x.ZoneSub_Type.Trim().ToUpper()));
                }
                if (!string.IsNullOrWhiteSpace(RQ.SystemCountryCode))
                {
                    filterForZone = filterForZone & Builders<ZoneMaster>.Filter.Eq(b => b.TLGXCountryCode, RQ.SystemCountryCode.Trim().ToUpper());
                }

                if (RQ.ZoneCities.Any())
                {
                    List<string> cities = RQ.ZoneCities.Select(x => x.Zone_City.Trim().ToUpper()).ToList();
                    filterForZone = filterForZone & Builders<ZoneMaster>.Filter.ElemMatch(x => x.Zone_CityMapping, x => cities.Contains(x.TLGXCityCode));
                }
                var resultCount = await collection.Find(filterForZone).CountDocumentsAsync();
                //TotalResultReturned
                int TotalSearchedZone = (int)resultCount;
                if (TotalSearchedZone > 0)
                {
                    var result = await collection.Find(filterForZone).Project(x => new ZoneSearchResponse
                    {
                        ZoneCode = x.Zone_Code,
                        ZoneName = x.Zone_Name,
                        ZoneSubType = x.Zone_SubType,
                        ZoneType = x.Zone_Type,
                        ZoneLocationPoint = new ZoneGeoLocationResponse { Zone_Latitude = x.Latitude, Zone_Longitude = x.Longitude },
                        Zone_GeographyMapping = x.Zone_GeographyMapping
                    }).Skip((RQ.PageNo - 1) * RQ.PageSize).Limit(RQ.PageSize).ToListAsync();

                    zoneResult = result;
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, zoneResult);
                    return response;
                }
                else
                {
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.BadRequest, "Data Not Available for request");
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
        /// Retrive list of zones based on search parameters by Geo Point Basis
        /// </summary>
        /// <param name="RQ">A minimum of one correct combination latitude and longitude is  required for successful search.</param>      
        /// <returns>
        /// returns List of Zones.
        /// </returns>
        [Route("Zone/ZoneListByGeoPointBasis")]
        [HttpPost]
        [ResponseType(typeof(List<ZoneSearchResponse>))]
        public async Task<HttpResponseMessage> GetZoneListByGeoPointBasis(ZoneSearchRequestForGeoPoint RQ)
        {

            try
            {
                List<ZoneSearchResponse> zoneResult = new List<ZoneSearchResponse>();

                if (RQ.PageSize == 0)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Page Size should be greater than Zero.");
                }
                else if (RQ.PageNo == 0)
                {
                    RQ.PageNo = 1;
                }
                else if (RQ.ZoneGeoLocations.FirstOrDefault().Zone_Latitude == 0 || RQ.ZoneGeoLocations.FirstOrDefault().Zone_Longitude == 0)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Geo Location Format is not proper.");
                }

                _database = MongoDBHandler.mDatabase();
                var collection = _database.GetCollection<ZoneMaster>("ZoneMaster");
                FilterDefinition<ZoneMaster> filterForZone;
                filterForZone = Builders<ZoneMaster>.Filter.Empty;

                if (RQ.ZoneTypes.Any())
                {
                    filterForZone = filterForZone & Builders<ZoneMaster>.Filter.In(b => b.Zone_Type, RQ.ZoneTypes.Select(x => x.Zone_Type.Trim().ToUpper()));
                }
                if (RQ.Zone_SubType.Any())
                {
                    filterForZone = filterForZone & Builders<ZoneMaster>.Filter.In(b => b.Zone_SubType, RQ.Zone_SubType.Select(x => x.ZoneSub_Type.Trim().ToUpper()));
                }

                if (RQ.ZoneGeoLocations.Any())
                {
                    filterForZone = filterForZone & new BsonDocument()
                {
                    { "loc",new BsonDocument()
                    {
                        { "$near",new BsonDocument()
                        {
                            { "$geometry",new BsonDocument()
                            {
                                {"type","Point" },
                                {"coordinates",new BsonArray(){ RQ.ZoneGeoLocations.FirstOrDefault().Zone_Longitude, RQ.ZoneGeoLocations.FirstOrDefault().Zone_Latitude } }
                            }
                        },
                        {"$minDistance",0 },
                        {"$maxDistance",RQ.GeoDistance>0?RQ.GeoDistance:0 }
                    } }
                    }}
                };
                }
                var result = await collection.Find(filterForZone).Project(x => new ZoneSearchResponse
                {
                    ZoneCode = x.Zone_Code,
                    ZoneName = x.Zone_Name,
                    ZoneSubType = x.Zone_SubType,
                    ZoneType = x.Zone_Type,
                    ZoneLocationPoint = new ZoneGeoLocationResponse { Zone_Latitude = x.Latitude, Zone_Longitude = x.Longitude },
                    Zone_GeographyMapping = x.Zone_GeographyMapping
                }).Skip((RQ.PageNo - 1) * RQ.PageSize).Limit(RQ.PageSize).ToListAsync();

                zoneResult = result;

                return zoneResult.Count > 0 ? Request.CreateResponse(HttpStatusCode.OK, zoneResult) : Request.CreateResponse(HttpStatusCode.BadRequest, "Data Not Available for request");
            }
            catch (Exception ex)
            {
                NLogHelper.Nlogger_LogError.LogError(ex, this.GetType().FullName, Request.GetActionDescriptor().ActionName, Request.RequestUri.PathAndQuery);
                HttpResponseMessage response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Server Error. Contact Admin. Error Date : " + DateTime.Now.ToString());
                return response;
            }
        }

        /// <summary>
        /// Retrive list of zones based on search parameters by Zone Master Codes
        /// </summary>
        /// <param name="RQ">Zone Maaster Codes from Zone master and supplier Codes from Supplier Master Required.</param>      
        /// <returns>
        /// returns List of Zones.
        /// </returns>
        [Route("Zone/SearchbyZoneMasterCode")]
        [HttpPost]
        [ResponseType(typeof(List<ZoneMappingSearchResponse>))]
        public async Task<HttpResponseMessage> SearchbyZoneMasterCode(ZoneMappingSearchRequest RQ)
        {

            try
            {
                List<ZoneMappingSearchResponse> zoneResult = new List<ZoneMappingSearchResponse>();

                if (RQ.PageSize == 0)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Page Size should be greater than Zero.");
                }
                else if (RQ.PageNo == 0)
                {
                    RQ.PageNo = 1;
                }
                
                _database = MongoDBHandler.mDatabase();
                var collection = _database.GetCollection<ZoneMaster>("ZoneMaster");         
                FilterDefinition<ZoneMaster> filterForZone;
                filterForZone = Builders<ZoneMaster>.Filter.Empty;
                if (RQ.ZoneCodes.Any())
                {
                    filterForZone = filterForZone & Builders<ZoneMaster>.Filter.In(b => b.Zone_Code, RQ.ZoneCodes.Select(x => x.Trim().ToUpper()));

                    var zoneMasters = await collection.Find(filterForZone).Skip((RQ.PageNo - 1) * RQ.PageSize).Limit(RQ.PageSize).ToListAsync();

                    if (RQ.SupplierCodes.Any())
                    {
                        zoneMasters.ForEach(a => a.Zone_LocationMapping.RemoveAll(d => !(RQ.SupplierCodes.Contains(d.Supplier_code))));
                    }
                    foreach (ZoneMaster zm in zoneMasters)
                    {
                        ZoneMappingSearchResponse zoneMappingSearchResponse = new ZoneMappingSearchResponse();
                        zoneMappingSearchResponse.ZoneCodes = zm.Zone_Code;
                        List<string> SupplierCodes = zm.Zone_LocationMapping.Select(x => x.Supplier_code).Distinct().ToList();
                        List<Zone_Supplier> zone_Suppliers = new List<Zone_Supplier>();
                        foreach (string SupCode in SupplierCodes)
                        {
                            Zone_Supplier zone_Sup = new Zone_Supplier();
                            zone_Sup.SupplierCode = SupCode;
                            List<Zone_LocationMapping> zone_LocationMappings= zm.Zone_LocationMapping.Where(x=>x.Supplier_code== SupCode).ToList();
                            List<ZoneSupplierLocation> ZoneSupplierLocationList = zone_LocationMappings.ConvertAll(x => new ZoneSupplierLocation
                            {
                                Name = x.Name ?? string.Empty,
                                Code = x.Code ?? string.Empty,
                                Distance = x.Distance,
                                Type = x.ZoneType ?? string.Empty,
                                SubType = x.ZoneSubType ?? string.Empty,
                                Latitude = x.Latitude,
                                Longitude = x.Longitude,
                                FullAddress = x.FullAdress
                            });
                            zone_Sup.MappingLocations = ZoneSupplierLocationList;
                            zone_Suppliers.Add(zone_Sup);
                        }
                        zoneMappingSearchResponse.SupplierCodes = zone_Suppliers;
                        zoneResult.Add(zoneMappingSearchResponse);
                    }                  

                    return zoneResult.Count > 0 ? Request.CreateResponse(HttpStatusCode.OK, zoneResult) : Request.CreateResponse(HttpStatusCode.BadRequest, "Data Not Available for request");                   
                }
                else
                {
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.BadRequest, "Zone Code Length must be greater than Zero");
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
        /// Retrieves System City Mapping for Supplier City Code and Supplier Code
        /// </summary>
        /// <param name="ZoneCode">Supplier City Code</param>
        /// <param name="SupplierCode">Supplier Code</param>
        /// <returns>System City Mapping</returns>
        [HttpGet]
        [Route("Zone/SearchbyZoneMasterCode/ZoneCode/{ZoneCode}/SupplierCode/{SupplierCode}")]
        [ResponseType(typeof(List<ZoneMappingSearchResponse>))]
        public async Task<HttpResponseMessage> GetLocationMappingByCode(string ZoneCode, string SupplierCode)
        {
            try
            {
                List<ZoneMappingSearchResponse> zoneMappingSearchResponses = new List<ZoneMappingSearchResponse>();
                _database = MongoDBHandler.mDatabase();
                var collection = _database.GetCollection<ZoneMaster>("ZoneMaster");
                FilterDefinition<ZoneMaster> filterForZone;
                filterForZone = Builders<ZoneMaster>.Filter.Empty;
                if (!string.IsNullOrWhiteSpace(ZoneCode))
                {
                    filterForZone = filterForZone & Builders<ZoneMaster>.Filter.Eq(b => b.Zone_Code,ZoneCode);                   
                }

                var zoneMasters = await collection.Find(filterForZone).ToListAsync();
                if (!string.IsNullOrWhiteSpace(SupplierCode))
                {
                    zoneMasters.ForEach(a => a.Zone_LocationMapping.RemoveAll(d => !(d.Supplier_code==SupplierCode)));
                }

                foreach (ZoneMaster zm in zoneMasters)
                {
                    ZoneMappingSearchResponse zoneMappingSearchResponse = new ZoneMappingSearchResponse();
                    zoneMappingSearchResponse.ZoneCodes = zm.Zone_Code;
                    List<string> SupplierCodes = zm.Zone_LocationMapping.Select(x => x.Supplier_code).Distinct().ToList();
                    List<Zone_Supplier> zone_Suppliers = new List<Zone_Supplier>();
                    foreach (string SupCode in SupplierCodes)
                    {
                        Zone_Supplier zone_Sup = new Zone_Supplier();
                        zone_Sup.SupplierCode = SupCode;
                        List<Zone_LocationMapping> zone_LocationMappings = zm.Zone_LocationMapping.Where(x => x.Supplier_code == SupCode).ToList();
                        List<ZoneSupplierLocation> ZoneSupplierLocationList = zone_LocationMappings.ConvertAll(x => new ZoneSupplierLocation
                        {
                            Name = x.Name ?? string.Empty,
                            Code = x.Code ?? string.Empty,
                            Distance = x.Distance,
                            Type = x.ZoneType ?? string.Empty,
                            SubType = x.ZoneSubType ?? string.Empty,
                            Latitude = x.Latitude,
                            Longitude = x.Longitude,
                            FullAddress = x.FullAdress
                        });
                        zone_Sup.MappingLocations = ZoneSupplierLocationList;
                        zone_Suppliers.Add(zone_Sup);
                    }
                    zoneMappingSearchResponse.SupplierCodes = zone_Suppliers;
                    zoneMappingSearchResponses.Add(zoneMappingSearchResponse);
                }
                return zoneMappingSearchResponses.Count > 0 ? Request.CreateResponse(HttpStatusCode.OK, zoneMappingSearchResponses) : Request.CreateResponse(HttpStatusCode.BadRequest, "Data Not Available for request");
            }
            catch (Exception ex)
            {
                NLogHelper.Nlogger_LogError.LogError(ex, this.GetType().FullName, Request.GetActionDescriptor().ActionName, Request.RequestUri.PathAndQuery);
                HttpResponseMessage response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Server Error. Contact Admin. Error Date : " + DateTime.Now.ToString());
                return response;
            }
        }


       

        /// <summary>
        /// Retrieves System City Mapping for Supplier City Code and Supplier Code
        /// </summary>        
        /// <param name="SupplierCode">Supplier Code</param>
        /// <returns>System City Mapping</returns>
        [HttpGet]
        [Route("Zone/SearchbyZoneMasterCode/SupplierCode/{SupplierCode}")]
        [ResponseType(typeof(List<SupplierZoneMaster>))]
        
        public async Task<HttpResponseMessage> GetLocationMappingBySupplierCode(string SupplierCode)
        {
            try
            {
                List<SupplierZoneMaster> zoneMappingSearchResponses = new List<SupplierZoneMaster>();
                _database = MongoDBHandler.mDatabase();
                var collection = _database.GetCollection<SupplierZoneMaster>("SupplierZoneMaster");
                FilterDefinition<SupplierZoneMaster> filterForZone;
                filterForZone = Builders<SupplierZoneMaster>.Filter.Empty;               

                if (!string.IsNullOrWhiteSpace(SupplierCode))
                {
                    filterForZone = filterForZone & Builders<SupplierZoneMaster>.Filter.Eq(b => b.Supplier_code, SupplierCode);
                }               
                               
                List<SupplierZoneMaster> ZoneMappingLocationResponseList = await collection.Find(filterForZone).ToListAsync();
                return ZoneMappingLocationResponseList.Count > 0 ? Request.CreateResponse(HttpStatusCode.OK, ZoneMappingLocationResponseList) : Request.CreateResponse(HttpStatusCode.BadRequest, "Data Not Available for request");

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
