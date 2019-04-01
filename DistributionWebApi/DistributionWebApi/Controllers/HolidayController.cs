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
        [ApiExplorerSettings(IgnoreApi = true)]
        [Obsolete]
        [HttpGet]
        [Route("{TourCode}")]
        [ResponseType(typeof(List<HolidayModel>))]
        public async Task<HttpResponseMessage> GetHolidayMappingByCode(string TourCode)
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

        /// <summary>
        /// Retrieves a Search Result List of based on a Collection of All Holiday Model filter like Country,City Code, Holiday Name,Holiday Flavour, 
        /// Suppliers and various Key facet mapping parameters.  
        /// </summary>
        /// <param name="HolidaySearchRequestParams"></param>
        /// <returns>Returns a Collection of Search Results. This is a actual Holiday mapping Data from supplier response.</returns>
        [HttpPost]
        [Route("Search")]
        [ResponseType(typeof(HolidayMappingSearchResult))]
        public async Task<HttpResponseMessage> HolidaySearch(HolidaySearchRequestParams HolidaySearchRequestParams)
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

        /// <summary>
        /// Returns a single Holiday Mapping Object for Given nakshatra Holiday Id
        /// </summary>
        /// <param name="NakshtraHolidayid">Nakshatra Unique Id for Holiday mapping</param>
        /// <returns>Returns single Holiday Model from Mongo DB for Nakshtra Holiday Id</returns>
        [HttpGet]
        [Route("Get/{NakshtraHolidayid}")]
        [ResponseType(typeof(HolidayModel))]
        public async Task<HttpResponseMessage> GetHolidayByNakshatraHolidayId(string NakshtraHolidayid)
        {
            _database = MongoDBHandler.mDatabase();

            IMongoCollection<HolidayModel> collectionHolidayModelMapping = _database.GetCollection<HolidayModel>("HolidayMapping");

            FilterDefinition<HolidayModel> filter;
            filter = Builders<HolidayModel>.Filter.Empty;
            //NakshtraHolidayid = NakshtraHolidayid.ToUpper();
            filter = filter & Builders<HolidayModel>.Filter.Eq(x => x.NakshatraHolidayId, NakshtraHolidayid.TrimEnd().TrimStart());

            var searchResult = await collectionHolidayModelMapping.Find(filter).FirstOrDefaultAsync();

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, searchResult);
            return response;
        }


        /// <summary>
        /// This API is used for updating mandatory attributes after getting reviewed by user for the supplier given responses about if anything is missing from the supplier end.
        /// </summary>
        /// <param name="HolidayModel">Holiday mapping object</param>
        /// <returns>This returns the updated Holiday Detail Model for given nakshtra holiday ID.</returns>
        [HttpPost]
        [Route("edit")]
        public async Task<HttpResponseMessage> updateHolidayMapping(HolidayModel HolidayModel)
        {
            _database = MongoDBHandler.mDatabase();

            IMongoCollection<HolidayModel> collectionHolidayModelMapping = _database.GetCollection<HolidayModel>("HolidayMapping");

            if (HolidayModel != null)
            {
                FilterDefinition<HolidayModel> filter;
                filter = Builders<HolidayModel>.Filter.Empty;
                var nakshatraHolidayId = HolidayModel.NakshatraHolidayId.ToString().ToUpper();
                filter = filter & Builders<HolidayModel>.Filter.Eq(x => x.NakshatraHolidayId, nakshatraHolidayId);
                var searchResult = await collectionHolidayModelMapping.Find(filter).FirstOrDefaultAsync();
                // Update this record.
                if (searchResult != null)
                {
                    searchResult = HolidayModel;
                    var filter1 = Builders<HolidayModel>.Filter.Eq(c => c.NakshatraHolidayId, HolidayModel.NakshatraHolidayId);
                    collectionHolidayModelMapping.ReplaceOne(filter, HolidayModel, new UpdateOptions { IsUpsert = true });
                }
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, searchResult);
                return response;
            }
            return null;
        }
    }
}
