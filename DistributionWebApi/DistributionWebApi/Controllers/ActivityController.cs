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
        /// <summary>
        /// static object of Mongo DB Class
        /// </summary>
        protected static IMongoDatabase _database;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Retrieves a Search Result List of based on a Collection of Country Ids combined with a System Code. The Country ID should be the calling system's Code 
        /// as the mapping to Activity Destination is handled by the API.
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
            ActivitySearchResult resultList = new ActivitySearchResult();

            if (param.PageSize > 100)
            {
                resultList.Message = "Page Size shouldn't be greater than 100.";
                return Request.CreateResponse(HttpStatusCode.BadRequest, resultList);
            }

            _database = MongoDBHandler.mDatabase();

            string[] arrayOfStrings;

            if (!string.IsNullOrWhiteSpace(param.RequestingSupplierCode))
            {
                //Get System Country Codes from Supplier Codes
                var collection = _database.GetCollection<BsonDocument>("CountryMapping");
                FilterDefinition<BsonDocument> filterCountry;
                filterCountry = Builders<BsonDocument>.Filter.Empty;

                filterCountry = filterCountry & Builders<BsonDocument>.Filter.AnyIn("SupplierCountryCode", param.CountryCodes.Select(s => s.Trim().ToUpper()).Distinct());
                filterCountry = filterCountry & Builders<BsonDocument>.Filter.Eq("SupplierCode", param.RequestingSupplierCode.Trim().ToUpper());
                ProjectionDefinition<BsonDocument> projectCountry = Builders<BsonDocument>.Projection.Include("CountryCode").Exclude("_id");

                var searchCountryResult = await collection.Find(filterCountry).Project(projectCountry).ToListAsync();
                arrayOfStrings = searchCountryResult.Select(s => s["CountryCode"].AsString).ToArray();
            }
            else
            {
                arrayOfStrings = param.CountryCodes.Select(s => s.Trim().ToUpper()).Distinct().ToArray();
            }


            //get Activities
            IMongoCollection<BsonDocument> collectionActivity = _database.GetCollection<BsonDocument>("ActivityDefinitions");
            FilterDefinition<BsonDocument> filter;
            filter = Builders<BsonDocument>.Filter.Empty;
            filter = filter & Builders<BsonDocument>.Filter.AnyIn("CountryCode", arrayOfStrings);
            if (param.FilterBySuppliers != null)
            {
                if (param.FilterBySuppliers.Length > 0)
                {
                    filter = filter & Builders<BsonDocument>.Filter.AnyIn("SupplierCompanyCode", param.FilterBySuppliers.Select(s => s.Trim().ToLower()));
                }
            }

            var TotalRecords = await collectionActivity.Find(filter).CountDocumentsAsync();

            List<ActivityDefinition> searchedData = new List<ActivityDefinition>();

            if (TotalRecords != 0 && param.PageSize != 0)
            {
                //SortDefinition<BsonDocument> sortByPrices;
                //sortByPrices = Builders<BsonDocument>.Sort.Ascending("Prices.Price");

                SortDefinition<BsonDocument> sortByid;
                sortByid = Builders<BsonDocument>.Sort.Ascending("_id");

                var searchResult = await collectionActivity.Find(filter)//.ToListAsync();
                    .Skip(param.PageSize * param.PageNo).Limit(param.PageSize).Sort(sortByid).ToListAsync(); //.Sort(sortByPrices)

                searchedData = JsonConvert.DeserializeObject<List<ActivityDefinition>>(searchResult.ToJson());

                resultList.PageSize = param.PageSize;
                resultList.CurrentPage = param.PageNo;

                int remainder = (int)TotalRecords % param.PageSize;
                int quotient = (int)TotalRecords / param.PageSize;
                if (remainder > 0)
                {
                    remainder = 1;
                }
                resultList.TotalPage = quotient + remainder;
            }
            else
            {
                resultList.TotalPage = 0;
            }

            resultList.CurrentPage = param.PageNo;
            resultList.TotalNumberOfActivities = TotalRecords;
            resultList.Activities = (from a in searchedData
                                     select new Activity
                                     {
                                         ActivityCode = a.SystemActivityCode,
                                         SupplierCompanyCode = a.SupplierCompanyCode,
                                         SupplierProductCode = a.SupplierProductCode,
                                         InterestType = a.InterestType,
                                         Category = a.Category,
                                         Type = a.Type,
                                         SubType = a.SubType,
                                         CategoryGroup = a.CategoryGroup,
                                         Name = a.Name,
                                         Description = a.Description,
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
                                         DeparturePoint = a.DeparturePoint,
                                         ReturnDetails = a.ReturnDetails,
                                         ProductOptions = a.ProductOptions,
                                         NumberOfPassengers = a.NumberOfPassengers,
                                         Prices = a.Prices,
                                         SuitableFor = a.SuitableFor,
                                         Specials = a.Specials,
                                         SupplierCityDepartureCodes = a.SupplierCityDepartureCodes
                                     }).ToList();

            foreach (var activity in searchedData)
            {
                var loadedActivity = resultList.Activities.Where(w => w.ActivityCode == activity.SystemActivityCode).First();

                FilterDefinition<BsonDocument> filterForSimilarProducts;
                filterForSimilarProducts = Builders<BsonDocument>.Filter.Empty;

                filterForSimilarProducts = filterForSimilarProducts & Builders<BsonDocument>.Filter.Ne("_id", activity.SystemActivityCode);
                filterForSimilarProducts = filterForSimilarProducts & Builders<BsonDocument>.Filter.Eq("CityCode", activity.CityCode);
                filterForSimilarProducts = filterForSimilarProducts & Builders<BsonDocument>.Filter.AnyIn("ProductSubTypeId", activity.ProductSubTypeId);

                ProjectionDefinition<BsonDocument> project = Builders<BsonDocument>.Projection.Include("_id");
                project = project.Include("Name");
                project = project.Include("Categories");
                project = project.Include("Prices");
                project = project.Include("ProductOptions");

                var SimilarProdSearchResult = await collectionActivity.Find(filterForSimilarProducts).Project(project).ToListAsync();

                List<ActivityDefinition> SimilarProdSearchResultObj = JsonConvert.DeserializeObject<List<ActivityDefinition>>(SimilarProdSearchResult.ToJson());

                if (SimilarProdSearchResultObj != null)
                {
                    loadedActivity.SimliarProducts = SimilarProdSearchResultObj.Select(s => new SimliarProducts
                    {
                        SystemActivityCode = s.SystemActivityCode.ToString(),
                        SystemActivityName = s.Name,
                        CategoryGroup = s.CategoryGroup,
                        Options = s.ProductOptions,
                        Prices = s.Prices
                    }).ToList();
                }

            }

            return Request.CreateResponse(HttpStatusCode.OK, resultList);
        }

        /// <summary>
        /// Retrieves a Search Result List of based on a Collection of City Ids combined with a System Code.   The City ID should be the calling system's Code 
        /// as the mapping to Activity Destination is handled by the API.
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
            ActivitySearchResult resultList = new ActivitySearchResult();

            if (param.PageSize > 100)
            {
                resultList.Message = "Page Size shouldn't be greater than 100.";
                return Request.CreateResponse(HttpStatusCode.BadRequest, resultList);
            }

            _database = MongoDBHandler.mDatabase();

            string[] arrayOfStrings;

            if (!string.IsNullOrWhiteSpace(param.RequestingSupplierCode))
            {
                //Get System City Codes from Supplier Codes
                var collection = _database.GetCollection<BsonDocument>("CityMapping");
                FilterDefinition<BsonDocument> filterCountry;
                filterCountry = Builders<BsonDocument>.Filter.Empty;

                filterCountry = filterCountry & Builders<BsonDocument>.Filter.AnyIn("SupplierCityCode", param.CityCodes.Select(s => s.Trim().ToUpper()).Distinct());
                filterCountry = filterCountry & Builders<BsonDocument>.Filter.Eq("SupplierCode", param.RequestingSupplierCode.Trim().ToUpper());
                ProjectionDefinition<BsonDocument> projectCountry = Builders<BsonDocument>.Projection.Include("CityCode").Exclude("_id");

                var searchCountryResult = await collection.Find(filterCountry).Project(projectCountry).ToListAsync();
                arrayOfStrings = searchCountryResult.Select(s => s["CityCode"].AsString).ToArray();
            }
            else
            {
                arrayOfStrings = param.CityCodes.Select(s => s.Trim().ToUpper()).Distinct().ToArray();
            }


            //get Activities
            IMongoCollection<BsonDocument> collectionActivity = _database.GetCollection<BsonDocument>("ActivityDefinitions");
            FilterDefinition<BsonDocument> filter;
            filter = Builders<BsonDocument>.Filter.Empty;
            filter = filter & Builders<BsonDocument>.Filter.AnyIn("CityCode", arrayOfStrings);

            if (param.FilterBySuppliers != null)
            {
                if (param.FilterBySuppliers.Length > 0)
                {
                    filter = filter & Builders<BsonDocument>.Filter.AnyIn("SupplierCompanyCode", param.FilterBySuppliers.Select(s => s.Trim().ToLower()));
                }
            }

            var TotalRecords = await collectionActivity.Find(filter).CountDocumentsAsync();

            List<ActivityDefinition> searchedData = new List<ActivityDefinition>();

            if (TotalRecords != 0 && param.PageSize != 0)
            {
                //SortDefinition<BsonDocument> sortByPrices;
                //sortByPrices = Builders<BsonDocument>.Sort.Ascending("Prices.Price");
                SortDefinition<BsonDocument> sortByid;
                sortByid = Builders<BsonDocument>.Sort.Ascending("_id");

                var searchResult = await collectionActivity.Find(filter).Skip(param.PageSize * param.PageNo).Limit(param.PageSize).Sort(sortByid).ToListAsync(); //.Sort(sortByPrices)

                searchedData = JsonConvert.DeserializeObject<List<ActivityDefinition>>(searchResult.ToJson());

                resultList.PageSize = param.PageSize;
                resultList.CurrentPage = param.PageNo;

                int remainder = (int)TotalRecords % param.PageSize;
                int quotient = (int)TotalRecords / param.PageSize;
                if (remainder > 0)
                {
                    remainder = 1;
                }
                resultList.TotalPage = quotient + remainder;
            }
            else
            {
                resultList.TotalPage = 0;
            }

            resultList.CurrentPage = param.PageNo;
            resultList.TotalNumberOfActivities = TotalRecords;
            resultList.Activities = (from a in searchedData
                                     select new Activity
                                     {
                                         ActivityCode = a.SystemActivityCode,
                                         SupplierCompanyCode = a.SupplierCompanyCode,
                                         SupplierProductCode = a.SupplierProductCode,
                                         InterestType = a.InterestType,
                                         Category = a.Category,
                                         Type = a.Type,
                                         SubType = a.SubType,
                                         CategoryGroup = a.CategoryGroup,
                                         TLGXDisplaySubType = a.TLGXDisplaySubType,
                                         Name = a.Name,
                                         Description = a.Description,
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
                                         DeparturePoint = a.DeparturePoint,
                                         ReturnDetails = a.ReturnDetails,
                                         ProductOptions = a.ProductOptions,
                                         NumberOfPassengers = a.NumberOfPassengers,
                                         Prices = a.Prices,
                                         SuitableFor = a.SuitableFor,
                                         Specials = a.Specials,
                                         SupplierCityDepartureCodes = a.SupplierCityDepartureCodes
                                     }).ToList();

            foreach (var activity in searchedData)
            {
                var loadedActivity = resultList.Activities.Where(w => w.ActivityCode == activity.SystemActivityCode).First();

                FilterDefinition<BsonDocument> filterForSimilarProducts;
                filterForSimilarProducts = Builders<BsonDocument>.Filter.Empty;

                filterForSimilarProducts = filterForSimilarProducts & Builders<BsonDocument>.Filter.Ne("_id", activity.SystemActivityCode);
                filterForSimilarProducts = filterForSimilarProducts & Builders<BsonDocument>.Filter.Eq("CityCode", activity.CityCode);
                filterForSimilarProducts = filterForSimilarProducts & Builders<BsonDocument>.Filter.AnyIn("ProductSubTypeId", activity.ProductSubTypeId);

                ProjectionDefinition<BsonDocument> project = Builders<BsonDocument>.Projection.Include("_id");
                project = project.Include("Name");
                project = project.Include("Categories");
                project = project.Include("Prices");
                project = project.Include("ProductOptions");

                var SimilarProdSearchResult = await collectionActivity.Find(filterForSimilarProducts).Project(project).ToListAsync();

                List<ActivityDefinition> SimilarProdSearchResultObj = JsonConvert.DeserializeObject<List<ActivityDefinition>>(SimilarProdSearchResult.ToJson());

                if (SimilarProdSearchResultObj != null)
                {
                    loadedActivity.SimliarProducts = SimilarProdSearchResultObj.Select(s => new SimliarProducts
                    {
                        SystemActivityCode = s.SystemActivityCode.ToString(),
                        SystemActivityName = s.Name,
                        //ActivityType = s.SubType,
                        CategoryGroup = s.CategoryGroup,
                        Options = s.ProductOptions,
                        Prices = s.Prices
                    }).ToList();
                }
            }

            return Request.CreateResponse(HttpStatusCode.OK, resultList);
        }

        /// <summary>
        /// Retrieves a Search Result List of based on a Collection of Activity Classifications Ids combined with a System Code. 
        /// The classification types can be retrieved using the GetClassificationAttributeStructureService.
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
            ActivitySearchResult resultList = new ActivitySearchResult();

            if (param.PageSize > 100)
            {
                resultList.Message = "Page Size shouldn't be greater than 100.";
                return Request.CreateResponse(HttpStatusCode.BadRequest, resultList);
            }

            _database = MongoDBHandler.mDatabase();

            //get Activities
            IMongoCollection<BsonDocument> collectionActivity = _database.GetCollection<BsonDocument>("ActivityDefinitions");
            FilterDefinition<BsonDocument> filter;
            filter = Builders<BsonDocument>.Filter.Empty;
            filter = filter & Builders<BsonDocument>.Filter.AnyIn("Categories.Type", param.ActivityTypes);
            if (param.FilterBySuppliers != null)
            {
                if (param.FilterBySuppliers.Length > 0)
                {
                    filter = filter & Builders<BsonDocument>.Filter.AnyIn("SupplierCompanyCode", param.FilterBySuppliers.Select(s => s.Trim().ToLower()));
                }
            }

            var TotalRecords = await collectionActivity.Find(filter).CountDocumentsAsync();

            List<ActivityDefinition> searchedData = new List<ActivityDefinition>();

            if (TotalRecords != 0 && param.PageSize != 0)
            {
                SortDefinition<BsonDocument> sortByPrices;
                sortByPrices = Builders<BsonDocument>.Sort.Ascending("Prices.Price");

                var searchResult = await collectionActivity.Find(filter).Skip(param.PageSize * param.PageNo).Limit(param.PageSize).ToListAsync(); //.Sort(sortByPrices)

                searchedData = JsonConvert.DeserializeObject<List<ActivityDefinition>>(searchResult.ToJson());

                resultList.PageSize = param.PageSize;
                resultList.CurrentPage = param.PageNo;

                int remainder = (int)TotalRecords % param.PageSize;
                int quotient = (int)TotalRecords / param.PageSize;
                if (remainder > 0)
                {
                    remainder = 1;
                }
                resultList.TotalPage = quotient + remainder;
            }
            else
            {
                resultList.TotalPage = 0;
            }

            resultList.CurrentPage = param.PageNo;
            resultList.TotalNumberOfActivities = TotalRecords;
            resultList.Activities = (from a in searchedData
                                     select new Activity
                                     {
                                         ActivityCode = a.SystemActivityCode,
                                         SupplierCompanyCode = a.SupplierCompanyCode,
                                         SupplierProductCode = a.SupplierProductCode,
                                         InterestType = a.InterestType,
                                         Category = a.Category,
                                         Type = a.Type,
                                         SubType = a.SubType,
                                         CategoryGroup = a.CategoryGroup,
                                         TLGXDisplaySubType = a.TLGXDisplaySubType,
                                         Name = a.Name,
                                         Description = a.Description,
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
                                         DeparturePoint = a.DeparturePoint,
                                         ReturnDetails = a.ReturnDetails,
                                         ProductOptions = a.ProductOptions,
                                         NumberOfPassengers = a.NumberOfPassengers,
                                         Prices = a.Prices,
                                         SuitableFor = a.SuitableFor,
                                         Specials = a.Specials,
                                         SupplierCityDepartureCodes = a.SupplierCityDepartureCodes
                                     }).ToList();

            foreach (var activity in searchedData)
            {
                var loadedActivity = resultList.Activities.Where(w => w.ActivityCode == activity.SystemActivityCode).First();

                FilterDefinition<BsonDocument> filterForSimilarProducts;
                filterForSimilarProducts = Builders<BsonDocument>.Filter.Empty;

                filterForSimilarProducts = filterForSimilarProducts & Builders<BsonDocument>.Filter.Ne("_id", activity.SystemActivityCode);
                filterForSimilarProducts = filterForSimilarProducts & Builders<BsonDocument>.Filter.Eq("CityCode", activity.CityCode);
                filterForSimilarProducts = filterForSimilarProducts & Builders<BsonDocument>.Filter.AnyIn("ProductSubTypeId", activity.ProductSubTypeId);

                ProjectionDefinition<BsonDocument> project = Builders<BsonDocument>.Projection.Include("_id");
                project = project.Include("Name");
                project = project.Include("Categories");
                project = project.Include("Prices");
                project = project.Include("ProductOptions");

                var SimilarProdSearchResult = await collectionActivity.Find(filterForSimilarProducts).Project(project).ToListAsync();

                List<ActivityDefinition> SimilarProdSearchResultObj = JsonConvert.DeserializeObject<List<ActivityDefinition>>(SimilarProdSearchResult.ToJson());

                if (SimilarProdSearchResultObj != null)
                {
                    loadedActivity.SimliarProducts = SimilarProdSearchResultObj.Select(s => new SimliarProducts
                    {
                        SystemActivityCode = s.SystemActivityCode.ToString(),
                        SystemActivityName = s.Name,
                        //ActivityType = s.SubType,
                        CategoryGroup = s.CategoryGroup,
                        Options = s.ProductOptions,
                        Prices = s.Prices
                    }).ToList();
                }

            }

            return Request.CreateResponse(HttpStatusCode.OK, resultList);
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
        public HttpResponseMessage GetActivityByFacets(string[] param)
        {
            HttpResponseMessage response = Request.CreateErrorResponse(HttpStatusCode.NoContent, "This functionality is not yet implemented.");
            return response;
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
            _database = MongoDBHandler.mDatabase();

            IMongoCollection<ActivityDefinition> collectionActivity = _database.GetCollection<ActivityDefinition>("ActivityDefinitions");

            FilterDefinition<ActivityDefinition> filter;
            filter = Builders<ActivityDefinition>.Filter.Empty;

            filter = filter & Builders<ActivityDefinition>.Filter.Eq(x => x.SystemActivityCode, Code);

            var searchResult = await collectionActivity.Find(filter).FirstOrDefaultAsync();

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, searchResult);
            return response;
        }

        /// <summary>
        /// Retrieves the Full Activity Product Static Definition based on the Supplier Code and Supplier Product Code
        /// </summary>
        /// <param name="SupplierCode">Supplier Code</param>
        /// <param name="ProductCode">Supplier Product Code</param>
        /// <returns>Full Activity Static Data Definition for use on a Product Detail Page. 
        /// The data is constructed of Supplier static data and the quality may vary dependent on the information provided by the Supplier. 
        /// Whilst Price information may be contained within the Product Definition, it is recommended that a formal Availability or Pricing Request be made to the Supplier in
        /// Realtime when calling this Service as additional information may be returned then.</returns>
        [Route("Details/SupplierCode/{SupplierCode}/SupplierProductCode/{ProductCode}")]
        [HttpGet]
        [ResponseType(typeof(ActivityDefinition))]
        public async Task<HttpResponseMessage> GetActivityDetailsBySupplierAndProductCode(string SupplierCode, string ProductCode)
        {
            _database = MongoDBHandler.mDatabase();

            IMongoCollection<ActivityDefinition> collectionActivity = _database.GetCollection<ActivityDefinition>("ActivityDefinitions");

            FilterDefinition<ActivityDefinition> filter;
            filter = Builders<ActivityDefinition>.Filter.Empty;

            filter = filter & Builders<ActivityDefinition>.Filter.Eq(x => x.SupplierCompanyCode, SupplierCode.Trim().ToLower());
            filter = filter & Builders<ActivityDefinition>.Filter.Eq(x => x.SupplierProductCode, ProductCode.Trim().ToUpper());

            var searchResult = await collectionActivity.Find(filter).FirstOrDefaultAsync();

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, searchResult);
            return response;
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
            _database = MongoDBHandler.mDatabase();

            IMongoCollection<ActivityMasters> collectionActivity = _database.GetCollection<ActivityMasters>("ActivityMasters");

            var searchResult = await collectionActivity.Find(s => true).SortBy(s => s.Type).ToListAsync();

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, searchResult);
            return response;
        }

    }
}