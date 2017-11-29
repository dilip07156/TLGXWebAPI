using DistributionWebApi.Models;
using DistributionWebApi.Models.Activity;
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
    /// <summary>
    /// Contains a collection of APIs to allow the search and dislay of a range of Activity Supplier Static Data. Each individual Suppliers' static data has been
    /// standardised into the formats contained within the API. In addition to the Standardisation of the Format of the Data, the Content of the data is also Standardised 
    /// against a range of Classification attributes.
    /// </summary>
    [RoutePrefix("ActivityMapping/Get")]
    public class ActivityMappingController : ApiController
    {

        protected static IMongoDatabase _database;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Retrieves a Search Result List of based on a Collection of Country Ids combined with a System Code. The Country ID should be the calling system's Code 
        /// as the mapping to Activity Destination is handled by the API
        /// </summary>
        /// <param name="param"></param>
        /// <returns>Returns a Collection of Search Results. This is a condensed version of the Activity Product Detail. Full Product Detail can be retrieved for Product Display
        /// by using the GetActivityDefinition Service
        /// </returns>
        [Route("ByCountries")]
        [HttpPost]
        [ResponseType(typeof(ActivitySearchResult))]
        public async Task<HttpResponseMessage> GetActivityByCountries(ActivitySearchByCountry_RQ param)
        {
            try
            {
                ActivitySearchResult resultList = new ActivitySearchResult();

                resultList.PageSize = param.PageSize;
                resultList.CurrentPage = param.PageNo;
                if (param.PageSize == 0)
                {
                    resultList.Message = "Page Size should be greater than Zero.";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, resultList);
                }

                _database = MongoDBHandler.mDatabase();

                //Get System Country Codes from Supplier Codes
                var collection = _database.GetCollection<BsonDocument>("CountryMapping");
                FilterDefinition<BsonDocument> filterCountry;
                filterCountry = Builders<BsonDocument>.Filter.Empty;

                filterCountry = filterCountry & Builders<BsonDocument>.Filter.AnyIn("SupplierCountryCode", param.CountryCodes.Distinct());
                filterCountry = filterCountry & Builders<BsonDocument>.Filter.Regex("SupplierCode", new BsonRegularExpression(new Regex(param.RequestingSupplierCode, RegexOptions.IgnoreCase)));
                ProjectionDefinition<BsonDocument> projectCountry = Builders<BsonDocument>.Projection.Include("CountryCode").Exclude("_id");

                var searchCountryResult = await collection.Find(filterCountry).Project(projectCountry).ToListAsync();
                var arrayOfStrings = searchCountryResult.Select(s => s["CountryCode"].AsString).ToArray();

                //get Activities
                IMongoCollection<BsonDocument> collectionActivity = _database.GetCollection<BsonDocument>("ActivityDefinitionsNew");
                FilterDefinition<BsonDocument> filter;
                filter = Builders<BsonDocument>.Filter.Empty;
                filter = filter & Builders<BsonDocument>.Filter.AnyIn("CountryCode", arrayOfStrings);


                SortDefinition<BsonDocument> sortByPrices;
                sortByPrices = Builders<BsonDocument>.Sort.Ascending("Prices.Price"); //Filter.Eq("Prices.PriceFor", "Product").

                var TotalRecords = await collectionActivity.Find(filter).CountAsync();
                var searchResult = await collectionActivity.Find(filter).Sort(sortByPrices).Skip(param.PageSize * param.PageNo).Limit(param.PageSize).ToListAsync();

                List<ActivityDefinition> searchedData = JsonConvert.DeserializeObject<List<ActivityDefinition>>(searchResult.ToJson());
                
                int remainder = (int)TotalRecords % param.PageSize;
                int quotient = (int)TotalRecords / param.PageSize;
                if (remainder > 0)
                    remainder = 1;

                resultList.TotalPage = quotient + remainder;
                resultList.CurrentPage = param.PageNo;
                resultList.TotalNumberOfActivities = TotalRecords;
                resultList.Activities = (from a in searchedData
                                         select new Activity
                                         {
                                             ActivityCode = a.SystemActivityCode,
                                             SupplierCompanyCode = a.SupplierCompanyCode,
                                             SupplierProductCode = a.SupplierProductCode,
                                             Category = a.Category,
                                             Type = a.Type,
                                             SubType = a.SubType,
                                             Name = a.Name,
                                             Description = a.Description,
                                             Session = a.Session,
                                             StartTime = a.StartTime,
                                             EndTime = a.EndTime,
                                             DaysOfTheWeek = a.DaysOfTheWeek,
                                             PhysicalIntensity = a.PhysicalIntensity,
                                             Overview = a.Overview,
                                             Recommended = a.Recommended,
                                             CountryName = a.CountryName,
                                             CountryCode = a.CountryCode,
                                             CityName = a.CityName,
                                             CityCode = a.CityCode,
                                             StarRating = a.StarRating,
                                             NumberOfReviews = a.NumberOfReviews,
                                             NumberOfLikes = a.NumberOfLikes,
                                             NumberOfViews = a.NumberOfViews,
                                             ActivityMedia = a.ActivityMedia,
                                             Duration = a.Duration,
                                             DeparturePoint = a.DeparturePoint,
                                             ReturnDetails = a.ReturnDetails,
                                             SimliarProducts = a.SimliarProducts,
                                             NumberOfPassengers = a.NumberOfPassengers,
                                             Prices = a.Prices
                                         }).ToList();

                return Request.CreateResponse(HttpStatusCode.OK, resultList);

            }
            catch (Exception ex)
            {
                NLogHelper.Nlogger_LogError.LogError(ex, this.GetType().FullName, Request.GetActionDescriptor().ActionName, Request.RequestUri.PathAndQuery);
                HttpResponseMessage response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Server Error. Contact Admin. Error Date : " + DateTime.Now.ToString());
                return response;
            }
        }

        /// <summary>
        /// Retrieves a Search Result List of based on a Collection of City Ids combined with a System Code.   The City ID should be the calling system's Code 
        /// as the mapping to Activity Destination is handled by the API
        /// </summary>
        /// <param name="param"></param>
        /// <returns>Returns a Collection of Search Results. This is a condensed version of the Activity Product Detail. Full Product Detail can be retrieved for Product Display
        /// by using the GetActivityDefinition Service
        /// </returns>
        [Route("ByCities")]
        [HttpPost]
        [ResponseType(typeof(ActivitySearchResult))]
        public async Task<HttpResponseMessage> GetActivityByCities(ActivitySearchByCity_RQ param)
        {
            try
            {
                ActivitySearchResult resultList = new ActivitySearchResult();

                resultList.PageSize = param.PageSize;
                resultList.CurrentPage = param.PageNo;
                if (param.PageSize == 0)
                {
                    resultList.Message = "Page Size should be greater than Zero.";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, resultList);
                }

                _database = MongoDBHandler.mDatabase();

                //Get System City Codes from Supplier Codes
                var collection = _database.GetCollection<BsonDocument>("CityMapping");
                FilterDefinition<BsonDocument> filterCountry;
                filterCountry = Builders<BsonDocument>.Filter.Empty;

                filterCountry = filterCountry & Builders<BsonDocument>.Filter.AnyIn("SupplierCityCode", param.CityCodes.Distinct());
                filterCountry = filterCountry & Builders<BsonDocument>.Filter.Regex("SupplierCode", new BsonRegularExpression(new Regex(param.RequestingSupplierCode, RegexOptions.IgnoreCase)));
                ProjectionDefinition<BsonDocument> projectCountry = Builders<BsonDocument>.Projection.Include("CityCode").Exclude("_id");

                var searchCountryResult = await collection.Find(filterCountry).Project(projectCountry).ToListAsync();
                var arrayOfStrings = searchCountryResult.Select(s => s["CityCode"].AsString).ToArray();

                //get Activities
                IMongoCollection<BsonDocument> collectionActivity = _database.GetCollection<BsonDocument>("ActivityDefinitions");
                FilterDefinition<BsonDocument> filter;
                filter = Builders<BsonDocument>.Filter.Empty;
                filter = filter & Builders<BsonDocument>.Filter.AnyIn("CityCode", arrayOfStrings);

                var TotalRecords = await collectionActivity.Find(filter).CountAsync();

                SortDefinition<BsonDocument> sortByPrices;
                sortByPrices = Builders<BsonDocument>.Sort.Ascending("Prices.Price");

                var searchResult = await collectionActivity.Find(filter).Sort(sortByPrices).Skip(param.PageSize * param.PageNo).Limit(param.PageSize).ToListAsync();

                List<ActivityDefinition> searchedData = JsonConvert.DeserializeObject<List<ActivityDefinition>>(searchResult.ToJson());

                int remainder = (int)TotalRecords % param.PageSize;
                int quotient = (int)TotalRecords / param.PageSize;
                if (remainder > 0)
                    remainder = 1;

                resultList.TotalPage = quotient + remainder;
                resultList.CurrentPage = param.PageNo;
                resultList.TotalNumberOfActivities = TotalRecords;
                resultList.Activities = (from a in searchedData
                                         select new Activity
                                         {
                                             ActivityCode = a.SystemActivityCode,
                                             SupplierCompanyCode = a.SupplierCompanyCode,
                                             SupplierProductCode = a.SupplierProductCode,
                                             Category = a.Category,
                                             Type = a.Type,
                                             SubType = a.SubType,
                                             Name = a.Name,
                                             Description = a.Description,
                                             Session = a.Session,
                                             StartTime = a.StartTime,
                                             EndTime = a.EndTime,
                                             DaysOfTheWeek = a.DaysOfTheWeek,
                                             PhysicalIntensity = a.PhysicalIntensity,
                                             Overview = a.Overview,
                                             Recommended = a.Recommended,
                                             CountryName = a.CountryName,
                                             CountryCode = a.CountryCode,
                                             CityName = a.CityName,
                                             CityCode = a.CityCode,
                                             StarRating = a.StarRating,
                                             NumberOfReviews = a.NumberOfReviews,
                                             NumberOfLikes = a.NumberOfLikes,
                                             NumberOfViews = a.NumberOfViews,
                                             ActivityMedia = a.ActivityMedia,
                                             Duration = a.Duration,
                                             DeparturePoint = a.DeparturePoint,
                                             ReturnDetails = a.ReturnDetails,
                                             SimliarProducts = a.SimliarProducts,
                                             NumberOfPassengers = a.NumberOfPassengers,
                                             Prices = a.Prices
                                         }).ToList();

               return Request.CreateResponse(HttpStatusCode.OK, resultList);
            }
            catch (Exception ex)
            {
                NLogHelper.Nlogger_LogError.LogError(ex, this.GetType().FullName, Request.GetActionDescriptor().ActionName, Request.RequestUri.PathAndQuery);
                HttpResponseMessage response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Server Error. Contact Admin. Error Date : " + DateTime.Now.ToString());
                return response;
            }
        }

        /// <summary>
        /// Retrieves a Search Result List of based on a Collection of Activity Classifications Ids combined with a System Code.   The classification types can be retrieved using the GetClassificationAttributeStructureService
        /// </summary>
        /// <param name="param"></param>
        /// <returns>Returns a Collection of Search Results. This is a condensed version of the Activity Product Detail. Full Product Detail can be retrieved for Product Display
        /// by using the GetActivityDefinition Service
        /// </returns>
        [Route("ByTypes")]
        [HttpPost]
        [ResponseType(typeof(ActivitySearchResult))]
        public async Task<HttpResponseMessage> GetActivityByActivityTypes(ActivitySearchByTypes_RQ param)
        {
            try
            {
                ActivitySearchResult resultList = new ActivitySearchResult();

                resultList.PageSize = param.PageSize;
                resultList.CurrentPage = param.PageNo;
                if (param.PageSize == 0)
                {
                    resultList.Message = "Page Size should be greater than Zero.";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, resultList);
                }

                _database = MongoDBHandler.mDatabase();

                //get Activities
                IMongoCollection<BsonDocument> collectionActivity = _database.GetCollection<BsonDocument>("ActivityDefinitions");
                FilterDefinition<BsonDocument> filter;
                filter = Builders<BsonDocument>.Filter.Empty;
                filter = filter & Builders<BsonDocument>.Filter.AnyIn("Type", param.ActivityTypes);

                SortDefinition<BsonDocument> sortByPrices;
                sortByPrices = Builders<BsonDocument>.Sort.Ascending("Prices.Price");

                var TotalRecords = await collectionActivity.Find(filter).CountAsync();
                var searchResult = await collectionActivity.Find(filter).Sort(sortByPrices).Skip(param.PageSize * param.PageNo).Limit(param.PageSize).ToListAsync();

                List<ActivityDefinition> searchedData = JsonConvert.DeserializeObject<List<ActivityDefinition>>(searchResult.ToJson());
                
                int remainder = (int)TotalRecords % param.PageSize;
                int quotient = (int)TotalRecords / param.PageSize;
                if (remainder > 0)
                    remainder = 1;

                resultList.TotalPage = quotient + remainder;
                resultList.CurrentPage = param.PageNo;
                resultList.TotalNumberOfActivities = TotalRecords;
                resultList.Activities = (from a in searchedData
                                         select new Activity
                                         {
                                             ActivityCode = a.SystemActivityCode,
                                             SupplierCompanyCode = a.SupplierCompanyCode,
                                             SupplierProductCode = a.SupplierProductCode,
                                             Category = a.Category,
                                             Type = a.Type,
                                             SubType = a.SubType,
                                             Name = a.Name,
                                             Description = a.Description,
                                             Session = a.Session,
                                             StartTime = a.StartTime,
                                             EndTime = a.EndTime,
                                             DaysOfTheWeek = a.DaysOfTheWeek,
                                             PhysicalIntensity = a.PhysicalIntensity,
                                             Overview = a.Overview,
                                             Recommended = a.Recommended,
                                             CountryName = a.CountryName,
                                             CountryCode = a.CountryCode,
                                             CityName = a.CityName,
                                             CityCode = a.CityCode,
                                             StarRating = a.StarRating,
                                             NumberOfReviews = a.NumberOfReviews,
                                             NumberOfLikes = a.NumberOfLikes,
                                             NumberOfViews = a.NumberOfViews,
                                             ActivityMedia = a.ActivityMedia,
                                             Duration = a.Duration,
                                             DeparturePoint = a.DeparturePoint,
                                             ReturnDetails = a.ReturnDetails,
                                             SimliarProducts = a.SimliarProducts,
                                             NumberOfPassengers = a.NumberOfPassengers,
                                             Prices = a.Prices
                                         }).ToList();

                return Request.CreateResponse(HttpStatusCode.OK, resultList);
            }
            catch (Exception ex)
            {
                NLogHelper.Nlogger_LogError.LogError(ex, this.GetType().FullName, Request.GetActionDescriptor().ActionName, Request.RequestUri.PathAndQuery);
                HttpResponseMessage response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Server Error. Contact Admin. Error Date : " + DateTime.Now.ToString());
                return response;
            }
        }

        /// <summary>
        /// This will be included in Second Release
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [Route("ByFacets")]
        [HttpPost]
        [ResponseType(typeof(void))]
        [ApiExplorerSettings(IgnoreApi = false)]
        public async Task<HttpResponseMessage> GetActivityByFacets(string[] param)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Retrieves the Full Activity Product Static Definition based on the MappingSystemCode returned in the GetActivity Search services
        /// </summary>
        /// <param name="Code">Mapping System Activity Code</param>
        /// <returns>Full Activity Static Data Definition for use on a Product Detail Page. 
        /// The data is constructed of Supplier static data and the quality may vary dependent on the information provided by the Supplier. 
        /// Whilst Price information may be contained within the Product Definition, it is recommended that a formal Availability or Pricing Request be made to the Supplier in
        /// Realtime when calling this Service as additional information may be returned then.</returns>
        [Route("Details/Code/{Code}")]
        [HttpGet]
        [ResponseType(typeof(ActivityDefinition))]
        public async Task<HttpResponseMessage> GetActivityDetailsByCode(int Code)
        {
            try
            {
                _database = MongoDBHandler.mDatabase();

                IMongoCollection<ActivityDefinition> collectionActivity = _database.GetCollection<ActivityDefinition>("ActivityDefinitions");

                FilterDefinition<ActivityDefinition> filter;
                filter = Builders<ActivityDefinition>.Filter.Empty;

                filter = filter & Builders<ActivityDefinition>.Filter.Eq(x => x.SystemActivityCode, Code);

                var searchResult = collectionActivity.Find(filter).FirstOrDefaultAsync();

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
        /// This will return all master key value pair related to activity
        /// </summary>
        /// <returns>A key value pair of activity master attribute type and attribute values</returns>
        [Route("Masters")]
        [HttpGet]
        [ResponseType(typeof(List<ActivityMasters>))]
        public async Task<HttpResponseMessage> GetActivityMasters()
        {
            try
            {
                _database = MongoDBHandler.mDatabase();

                IMongoCollection<ActivityMasters> collectionActivity = _database.GetCollection<ActivityMasters>("ActivityMasters");

                var searchResult = await collectionActivity.Find(s => true).SortBy(s => s.Type).ToListAsync();

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