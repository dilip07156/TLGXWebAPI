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
    [RoutePrefix("Activity/Get")]
    public class ActivityController : ApiController
    {

        protected static IMongoDatabase _database;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();


        [Route("ByCountries")]
        [HttpPost]
        [ResponseType(typeof(ActivityDefinition_SRP))]
        public async Task<HttpResponseMessage> GetActivityByCountries(ActivitySearchByCountry_RQ param)
        {
            try
            {
                ActivityDefinition_SRP resultList = new ActivityDefinition_SRP();

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
                IMongoCollection<BsonDocument> collectionActivity = _database.GetCollection<BsonDocument>("ActivityDefinitions");
                FilterDefinition<BsonDocument> filter;
                filter = Builders<BsonDocument>.Filter.Empty;
                filter = filter & Builders<BsonDocument>.Filter.AnyIn("CountryCode", arrayOfStrings);

                var TotalRecords = await collectionActivity.Find(filter).CountAsync();
                var searchResult = await collectionActivity.Find(filter).Skip(param.PageSize * param.PageNo).Limit(param.PageSize).ToListAsync();

                List<ActivityDefinition_PDP> searchedData = JsonConvert.DeserializeObject<List<ActivityDefinition_PDP>>(searchResult.ToJson());
                
                int remainder = (int)TotalRecords % param.PageSize;
                int quotient = (int)TotalRecords / param.PageSize;
                if (remainder > 0)
                    remainder = 1;

                resultList.TotalPage = quotient + remainder;
                resultList.CurrentPage = param.PageNo;
                resultList.TotalNumberOfActivities = TotalRecords;
                resultList.Activities = (from a in searchedData
                                         select new Activities
                                         {
                                             TLGXActivityCode = a.TLGXActivityCode,
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
                                             SimliarProducts = a.SimliarProducts
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

        [Route("ByCities")]
        [HttpPost]
        [ResponseType(typeof(ActivityDefinition_SRP))]
        public async Task<HttpResponseMessage> GetActivityByCities(ActivitySearchByCity_RQ param)
        {
            try
            {
                ActivityDefinition_SRP resultList = new ActivityDefinition_SRP();

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
                var searchResult = await collectionActivity.Find(filter).Skip(param.PageSize * param.PageNo).Limit(param.PageSize).ToListAsync();

                List<ActivityDefinition_PDP> searchedData = JsonConvert.DeserializeObject<List<ActivityDefinition_PDP>>(searchResult.ToJson());

                int remainder = (int)TotalRecords % param.PageSize;
                int quotient = (int)TotalRecords / param.PageSize;
                if (remainder > 0)
                    remainder = 1;

                resultList.TotalPage = quotient + remainder;
                resultList.CurrentPage = param.PageNo;
                resultList.TotalNumberOfActivities = TotalRecords;
                resultList.Activities = (from a in searchedData
                                         select new Activities
                                         {
                                             TLGXActivityCode = a.TLGXActivityCode,
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
                                             SimliarProducts = a.SimliarProducts
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

        [Route("ByTypes")]
        [HttpPost]
        [ResponseType(typeof(ActivityDefinition_SRP))]
        public async Task<HttpResponseMessage> GetActivityByActivityTypes(ActivitySearchByTypes_RQ param)
        {
            try
            {
                ActivityDefinition_SRP resultList = new ActivityDefinition_SRP();

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

                var TotalRecords = await collectionActivity.Find(filter).CountAsync();
                var searchResult = await collectionActivity.Find(filter).Skip(param.PageSize * param.PageNo).Limit(param.PageSize).ToListAsync();

                List<ActivityDefinition_PDP> searchedData = JsonConvert.DeserializeObject<List<ActivityDefinition_PDP>>(searchResult.ToJson());
                
                int remainder = (int)TotalRecords % param.PageSize;
                int quotient = (int)TotalRecords / param.PageSize;
                if (remainder > 0)
                    remainder = 1;

                resultList.TotalPage = quotient + remainder;
                resultList.CurrentPage = param.PageNo;
                resultList.TotalNumberOfActivities = TotalRecords;
                resultList.Activities = (from a in searchedData
                                         select new Activities
                                         {
                                             TLGXActivityCode = a.TLGXActivityCode,
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
                                             SimliarProducts = a.SimliarProducts
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

        [Route("ByFacets")]
        [HttpPost]
        [ResponseType(typeof(void))]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<HttpResponseMessage> GetActivityByFacets(string[] param)
        {
            throw new NotImplementedException();
        }

        [Route("Details/Code/{Code}")]
        [HttpGet]
        [ResponseType(typeof(ActivityDefinition_PDP))]
        public async Task<HttpResponseMessage> GetActivityDetailsByCode(int Code)
        {
            try
            {
                _database = MongoDBHandler.mDatabase();

                IMongoCollection<ActivityDefinition_PDP> collectionActivity = _database.GetCollection<ActivityDefinition_PDP>("ActivityDefinitions");

                FilterDefinition<ActivityDefinition_PDP> filter;
                filter = Builders<ActivityDefinition_PDP>.Filter.Empty;

                filter = filter & Builders<ActivityDefinition_PDP>.Filter.Eq(x => x.TLGXActivityCode, Code);

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
    }
}