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
using System.Xml;
using System.IO;
using System.Text;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using System.Web.Http.Description;
using Newtonsoft.Json;
using System.Diagnostics;

namespace DistributionWebApi.Controllers
{
    /// <summary>
    /// Use To Retrive Holiday Data using productCode.
    /// </summary>
    [RoutePrefix("HolidayMapping")]
    public class HolidayController : ApiController
    {
        /// <summary>
        /// Mongo database handler
        /// </summary>
        protected static IMongoDatabase _database;
        /// <summary>
        /// Retrieves all Holiday Information for System product code.
        /// </summary>
        /// <param name="TourCode"></param>
        /// <returns>Detailed information about Holiday</returns>
        [HttpGet]
        [Route("System/{TourCode}")]
        [ResponseType(typeof(List<HolidayModel>))]
        public async Task<HttpResponseMessage> GetHolidayMappingByCode(string TourCode)
        {
            try
            {
                _database = MongoDBHandler.mDatabase();
                if (!string.IsNullOrWhiteSpace(TourCode))
                {

                    IMongoCollection<BsonDocument> collectionHolidayMapping = _database.GetCollection<BsonDocument>("CKISHolidayData");

                    FilterDefinition<BsonDocument> filter;
                    filter = Builders<BsonDocument>.Filter.Empty;
                    // filter = filter & Builders<BsonDocument>.Filter.Eq("SupplierName",SupplierName);
                    filter = filter & Builders<BsonDocument>.Filter.Eq("Holiday.Id", TourCode);

                    var searchResult = await collectionHolidayMapping.Find(filter).ToListAsync();

                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, searchResult);
                    return response;
                }
                else
                {
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid Reuest Parameters");
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


        [HttpPost]
        [Route("System/GetHolidaySearch")]
        [ResponseType(typeof(HolidayMappingSearchResult))]
        public async Task<HttpResponseMessage> GetHolidaySearch(HolidaySearchRequestParams HolidaySearchRequestParams)
        {

            try
            {
                _database = MongoDBHandler.mDatabase();
                HolidayMappingSearchResult resultList = new HolidayMappingSearchResult();
                if (HolidaySearchRequestParams != null)
                {
                    IMongoCollection<DistributionWebApi.Models.HolidayModel> collectionHolidayMapping = _database.GetCollection<DistributionWebApi.Models.HolidayModel>("HolidayMapping");

                    FilterDefinition<HolidayModel> filter;
                    filter = Builders<HolidayModel>.Filter.Empty;

                    if (!string.IsNullOrEmpty(HolidaySearchRequestParams.Country))
                    {
                        filter = filter & Builders<DistributionWebApi.Models.HolidayModel>.Filter.ElemMatch(x => x.Destinations, x => x.TLGXCountryCode.ToUpper() == HolidaySearchRequestParams.Country.ToUpper());
                    }
                    if (!string.IsNullOrEmpty(HolidaySearchRequestParams.CityCode))
                    {
                        filter = filter & Builders<DistributionWebApi.Models.HolidayModel>.Filter.ElemMatch(x => x.Destinations, x => x.TLGXCityCode.ToUpper() == HolidaySearchRequestParams.CityCode.ToUpper());
                    }
                    if (!string.IsNullOrEmpty(HolidaySearchRequestParams.HolidayName))
                    {
                        var escapedHolidayName = Regex.Escape(HolidaySearchRequestParams.HolidayName);
                        filter = filter & Builders<DistributionWebApi.Models.HolidayModel>.Filter.Regex(x => x.ProductName, new BsonRegularExpression(new Regex(escapedHolidayName, RegexOptions.IgnoreCase)));
                    }
                    if (!string.IsNullOrEmpty(HolidaySearchRequestParams.HolidayFlavour))
                    {
                        var escapedHolidayFlavour = Regex.Escape(HolidaySearchRequestParams.HolidayFlavour);
                        filter = filter & Builders<DistributionWebApi.Models.HolidayModel>.Filter.Regex(x => x.ProductFlavorName, new BsonRegularExpression(new Regex(escapedHolidayFlavour, RegexOptions.IgnoreCase)));
                    }
                    if (!string.IsNullOrEmpty(HolidaySearchRequestParams.Supplier))
                    {
                        var escapedSupplierName = Regex.Escape(HolidaySearchRequestParams.Supplier);
                        filter = filter & Builders<DistributionWebApi.Models.HolidayModel>.Filter.Regex(x => x.SupplierName, new BsonRegularExpression(new Regex(escapedSupplierName, RegexOptions.IgnoreCase)));
                        //filter = filter & Builders<DistributionWebApi.Models.HolidayModel>.Filter.Eq(x => x.SupplierName.ToLower(), HolidaySearchRequestParams.Supplier.ToLower());
                    }
                    if (!string.IsNullOrEmpty(HolidaySearchRequestParams.MappingStatus))
                    {
                        var escapedMappingStatus = Regex.Escape(HolidaySearchRequestParams.MappingStatus);
                        filter = filter & Builders<DistributionWebApi.Models.HolidayModel>.Filter.Regex(x => x.UserReviewStatus, new BsonRegularExpression(new Regex(escapedMappingStatus, RegexOptions.IgnoreCase)));
                    }

                    if (HolidaySearchRequestParams.IsInterestsMissing)
                    {
                        filter = filter & Builders<DistributionWebApi.Models.HolidayModel>.Filter.Size(x => x.Interests, 0);
                    }

                    if (HolidaySearchRequestParams.IsTravellerTypeMissing)
                    {
                        filter = filter & Builders<DistributionWebApi.Models.HolidayModel>.Filter.Size(x => x.TravellerType, 0);
                    }

                    if (HolidaySearchRequestParams.IsComfortLevelMissing)
                    {
                        filter = filter & Builders<DistributionWebApi.Models.HolidayModel>.Filter.Size(x => x.ComfortLevel, 0);
                    }

                    if (HolidaySearchRequestParams.IsTravelFrequencyMissing)
                    {
                        filter = filter & Builders<DistributionWebApi.Models.HolidayModel>.Filter.Size(x => x.TravelFrequency, 0);                       
                    }

                    if (HolidaySearchRequestParams.IsStayTypeMissing)
                    {
                        filter = filter & Builders<DistributionWebApi.Models.HolidayModel>.Filter.Eq(x => x.StayType, string.Empty);
                    }

                    if (HolidaySearchRequestParams.IsTravelFrequencyMissing)
                    {
                        filter = filter & Builders<DistributionWebApi.Models.HolidayModel>.Filter.Size(x => x.TravelFrequency, 0);                    
                    }
                    if (HolidaySearchRequestParams.IsPaceOfHolidayMissing)
                    {
                        filter = filter & Builders<DistributionWebApi.Models.HolidayModel>.Filter.Eq(x => x.PaceOfHoliday, null);                    
                    }

                    if (HolidaySearchRequestParams.IsUSPMissing)
                    {
                        filter = filter & Builders<DistributionWebApi.Models.HolidayModel>.Filter.Eq(x => x.UniqueSellingPoints, null);
                    }

                    var TotalRecords = await collectionHolidayMapping.Find(filter).CountDocumentsAsync();

                    List<HolidayModel> searchedData = new List<HolidayModel>();
              
                    if (TotalRecords != 0 && HolidaySearchRequestParams.PageSize != 0)
                    {                     
                        searchedData = await collectionHolidayMapping.Find(filter).Skip(HolidaySearchRequestParams.PageSize * HolidaySearchRequestParams.PageNo).Limit(HolidaySearchRequestParams.PageSize).ToListAsync();


                        resultList.PageSize = HolidaySearchRequestParams.PageSize;
                        resultList.CurrentPage = HolidaySearchRequestParams.PageNo;

                        int remainder = (int)TotalRecords % HolidaySearchRequestParams.PageSize;
                        int quotient = (int)TotalRecords / HolidaySearchRequestParams.PageSize;
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

                    resultList.CurrentPage = HolidaySearchRequestParams.PageNo;
                    resultList.TotalNumberOfHolidays = TotalRecords;
                    resultList.Holidays = (from a in searchedData
                                           select new HolidaySearchResponse
                                           {
                                               NAKID = a.NakshatraHolidayId,
                                               HolidayName = a.ProductName,
                                               HolidayId = a.SupplierHolidayId,
                                               Country = (a.Destinations.Count > 0 ? a.Destinations.First().TLGXCountryName : ""),
                                               City = (a.Destinations.Count > 0 ? a.Destinations.First().TLGXCityName : ""),
                                               Source = a.SupplierName,
                                               FlavourId = a.ProductFlavorID,
                                               HolidayFlavour = a.ProductFlavorName

                                           }).ToList();


                    return Request.CreateResponse(HttpStatusCode.OK, resultList);

                }
                else
                    return null;

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
