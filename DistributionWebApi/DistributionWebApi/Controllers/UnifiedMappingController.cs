using DistributionWebApi.Models;
using DistributionWebApi.Mongo;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace DistributionWebApi.Controllers
{
    /// <summary>
    /// Used to retrieve unified mapping details
    /// </summary>
    [RoutePrefix("Mapping")]
    public class UnifiedMappingController : ApiController
    {
        /// <summary>
        /// Mongo database handler
        /// </summary>
        protected static IMongoDatabase _database;

        /// <summary>
        /// Retrieves TLGX Acco Id, Common Hotel Id, TLGX Room Type Id for TLGX Common Hotel Ids, accepting multiple suppliers and multiple supplier room types.
        /// API can handle single / multiple TLGX Common Hotel Ids at a time.
        /// API can return hotel mapping along with room type mappings where accommodation room info rows exists in MDM system and it has been processed.
        /// Please note that not all suppliers provide static data for mapping and real time requests into the mapping engine are not permitted.
        /// </summary>
        /// <param name="RQ">Combination of Hotel and Room mapping request.</param>
        /// <returns>Combination of Hotel and Room mapping response.</returns>
        [HttpPost]
        [Route("HotelAndRoomTypeMapping")]
        [ResponseType(typeof(UnifiedHotelAndRoomMapping_RS))]
        public async Task<HttpResponseMessage> GetUnifiedHotelAndRoomTypeMapping(UnifiedHotelAndRoomMapping_RQ RQ)
        {
            try
            {
                var returnResult = new UnifiedHotelAndRoomMapping_RS();

                if (RQ != null)
                {
                    returnResult.SessionId = RQ.SessionId;

                    if (RQ.MappingRequests != null)
                    {

                        #region Variable Declaration and Initialization

                        _database = MongoDBHandler.mDatabase();
                        IMongoCollection<BsonDocument> collectionRoomTypeMapping = _database.GetCollection<BsonDocument>("RoomTypeMapping");
                        IMongoCollection<RoomTypeMappingOnline> collection_rto = _database.GetCollection<RoomTypeMappingOnline>("RoomTypeMappingOnline");
                        IMongoCollection<BsonDocument> collectionProductMapping = _database.GetCollection<BsonDocument>("ProductMappingLite");
                        IMongoCollection<BsonDocument> collectionAccommodationMaster = _database.GetCollection<BsonDocument>("AccommodationMaster");


                        var searchedHotelMappingData = new List<ProductMappingLite>();
                        var searchedRoomMappingData = new List<HotelRoomTypeMappingModel>();
                        var searchedAccomodationSearchData = new List<DC_AccomodationMasterMapping>();

                        var SupplierCodes = RQ.MappingRequests.Where(w => w.SupplierCode != null).Select(x => x.SupplierCode.ToUpper()).Distinct().ToList();
                        var SupplierProductCodes = RQ.MappingRequests.Where(w => w.SupplierProductCode != null).Select(x => x.SupplierProductCode.ToUpper()).Distinct().ToList();
                        var SupplierRoomTypeIds = RQ.MappingRequests.SelectMany(p => p.SupplierRoomTypes.Where(w => w.SupplierRoomId != null).Select(s => s.SupplierRoomId.ToUpper())).Distinct().ToList();
                        var SupplierRoomTypeCodes = RQ.MappingRequests.SelectMany(p => p.SupplierRoomTypes.Where(w => w.SupplierRoomTypeCode != null).Select(s => s.SupplierRoomTypeCode.ToUpper())).Distinct().ToList();

                        #endregion Variable Declaration and Initialization

                        #region HotelMappingSearch

                        FilterDefinition<BsonDocument> filterHotelMapping = Builders<BsonDocument>.Filter.Empty;
                        filterHotelMapping = filterHotelMapping & Builders<BsonDocument>.Filter.AnyIn("SupplierCode", SupplierCodes);
                        filterHotelMapping = filterHotelMapping & Builders<BsonDocument>.Filter.AnyIn("SupplierProductCode", SupplierProductCodes);

                        ProjectionDefinition<BsonDocument> projectHotelMapping = Builders<BsonDocument>.Projection.Include("SupplierCode");
                        projectHotelMapping = projectHotelMapping.Exclude("_id");
                        projectHotelMapping = projectHotelMapping.Include("SupplierProductCode");
                        projectHotelMapping = projectHotelMapping.Include("SystemProductCode");
                        projectHotelMapping = projectHotelMapping.Include("MapId");
                        projectHotelMapping = projectHotelMapping.Include("TlgxMdmHotelId");

                        var searchHotelMappingResult = collectionProductMapping.Find(filterHotelMapping).Project(projectHotelMapping).ToList();
                        searchedHotelMappingData = JsonConvert.DeserializeObject<List<ProductMappingLite>>(searchHotelMappingResult.ToJson());

                        #endregion HotelMappingSearch

                        #region Fetch Accommodation Master
                        FilterDefinition<BsonDocument> filterAccoMaster = Builders<BsonDocument>.Filter.Empty;
                        filterAccoMaster = filterAccoMaster & Builders<BsonDocument>.Filter.AnyIn("_id", searchedHotelMappingData.Select(s => Convert.ToInt32(s.SystemProductCode)));

                        ProjectionDefinition<BsonDocument> projectAccoMaster = Builders<BsonDocument>.Projection.Include("_id");
                        projectAccoMaster = projectAccoMaster.Include("IsRoomMappingCompleted");
                        projectAccoMaster = projectAccoMaster.Include("CountryCode");
                        projectAccoMaster = projectAccoMaster.Include("CountryName");
                        projectAccoMaster = projectAccoMaster.Include("CityCode");
                        projectAccoMaster = projectAccoMaster.Include("CityName");
                        projectAccoMaster = projectAccoMaster.Include("StateCode");
                        projectAccoMaster = projectAccoMaster.Include("StateName");
                        projectAccoMaster = projectAccoMaster.Include("HotelName");
                        projectAccoMaster = projectAccoMaster.Include("ProductCategorySubType");
                        projectAccoMaster = projectAccoMaster.Include("Brand");
                        projectAccoMaster = projectAccoMaster.Include("Chain");
                        projectAccoMaster = projectAccoMaster.Include("AccomodationCompanyVersions");
                        projectAccoMaster = projectAccoMaster.Include("TLGXAccoId");
                        var searchAccoMasterResult = collectionAccommodationMaster.Find(filterAccoMaster).Project(projectAccoMaster).ToList();
                        searchedAccomodationSearchData = JsonConvert.DeserializeObject<List<DC_AccomodationMasterMapping>>(searchAccoMasterResult.ToJson());
                        #endregion Fetch Accommodation Master

                        #region RoomMappingSearch

                        if (SupplierRoomTypeIds.Any() || SupplierRoomTypeCodes.Any())
                        {
                            FilterDefinition<BsonDocument> filterRoomMapping = Builders<BsonDocument>.Filter.Empty;
                            filterRoomMapping = filterRoomMapping & Builders<BsonDocument>.Filter.AnyIn("supplierCode", SupplierCodes);
                            filterRoomMapping = filterRoomMapping & Builders<BsonDocument>.Filter.AnyIn("SupplierProductId", SupplierProductCodes);
                            filterRoomMapping = filterRoomMapping & Builders<BsonDocument>.Filter.Or(Builders<BsonDocument>.Filter.AnyIn("SupplierRoomId", SupplierRoomTypeIds), Builders<BsonDocument>.Filter.AnyIn("SupplierRoomTypeCode", SupplierRoomTypeCodes));

                            ProjectionDefinition<BsonDocument> projectRoomMapping = Builders<BsonDocument>.Projection.Include("supplierCode");
                            projectRoomMapping = projectRoomMapping.Exclude("_id");
                            projectRoomMapping = projectRoomMapping.Include("SupplierProductId");
                            projectRoomMapping = projectRoomMapping.Include("SupplierRoomId");
                            projectRoomMapping = projectRoomMapping.Include("SupplierRoomTypeCode");
                            projectRoomMapping = projectRoomMapping.Include("SystemRoomTypeMapId");
                            projectRoomMapping = projectRoomMapping.Include("SystemProductCode");
                            projectRoomMapping = projectRoomMapping.Include("SystemRoomTypeName");
                            projectRoomMapping = projectRoomMapping.Include("SystemRoomCategory");
                            projectRoomMapping = projectRoomMapping.Include("TLGXAccoRoomId");
                            projectRoomMapping = projectRoomMapping.Include("TLGXAccoId");

                            var searchRoomMappingResult = collectionRoomTypeMapping.Find(filterRoomMapping).Project(projectRoomMapping).ToList();
                            searchedRoomMappingData = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<List<HotelRoomTypeMappingModel>>(searchRoomMappingResult.ToJson());
                        }

                        #endregion RoomMappingSearch

                        #region Build Response

                        returnResult.SessionId = RQ.SessionId;
                        var mappingResponseList = new List<UnifiedHotelAndRoomMapping_Response>();
                        var writeModelDetails = new List<WriteModel<RoomTypeMappingOnline>>();

                        foreach (var mappingRequest in RQ.MappingRequests)
                        {
                            var mappingResponse = new UnifiedHotelAndRoomMapping_Response()
                            {
                                SequenceNumber = mappingRequest.SequenceNumber,
                                ProductType = mappingRequest.ProductType,
                                SupplierCode = mappingRequest.SupplierCode,
                                SupplierProductCode = mappingRequest.SupplierProductCode
                            };

                            var HotelMapping = searchedHotelMappingData.Where(w => w.SupplierCode == mappingRequest.SupplierCode.ToUpper() && w.SupplierProductCode == mappingRequest.SupplierProductCode.ToUpper()).OrderByDescending(o => o.MapId).Select(s => s).FirstOrDefault();

                            if (HotelMapping != null)
                            {
                                var Acco = searchedAccomodationSearchData.Where(w => w.CommonHotelId == Convert.ToInt32(HotelMapping.SystemProductCode)).FirstOrDefault();
                                mappingResponse.CommonHotelId = HotelMapping.SystemProductCode;
                                mappingResponse.ProductMapId = HotelMapping.MapId;
                                mappingResponse.TlgxAccoId = HotelMapping.TlgxMdmHotelId;
                                if (Acco != null)
                                {
                                    mappingResponse.TlgxAccoName = Acco.HotelName;
                                    mappingResponse.ContainsRoomMappings = Acco.IsRoomMappingCompleted;
                                    mappingResponse.SystemCityCode = Acco.CityCode;
                                    mappingResponse.SystemCityName = Acco.CityName;
                                    mappingResponse.SystemCountryCode = Acco.CountryCode;
                                    mappingResponse.SystemCountryName = Acco.CountryName;
                                    mappingResponse.SystemStateCode = Acco.StateCode;
                                    mappingResponse.SystemStateName = Acco.StateName;
                                    mappingResponse.ProductSubType = Acco.ProductCategorySubType;
                                    mappingResponse.Chain = Acco.Chain;
                                    mappingResponse.Brand = Acco.Brand;
                                }
                                else
                                {
                                    mappingResponse.TlgxAccoName = string.Empty;
                                    mappingResponse.ContainsRoomMappings = false;
                                    mappingResponse.SystemCityCode = string.Empty;
                                    mappingResponse.SystemCityName = string.Empty;
                                    mappingResponse.SystemCountryCode = string.Empty;
                                    mappingResponse.SystemCountryName = string.Empty;
                                    mappingResponse.SystemStateCode = string.Empty;
                                    mappingResponse.SystemStateName = string.Empty;
                                    mappingResponse.ProductSubType = string.Empty;
                                    mappingResponse.Chain = string.Empty;
                                    mappingResponse.Brand = string.Empty;
                                }
                            }

                            var RoomMappings = searchedRoomMappingData.Where(w => w.supplierCode == mappingRequest.SupplierCode && w.SupplierProductId == mappingRequest.SupplierProductCode).Select(s => s).ToList();

                            var RoomMappingResponseList = new List<UnifiedHotelAndRoomMapping_RoomTypeResponse>();
                            foreach (var mappingRoomRequest in mappingRequest.SupplierRoomTypes)
                            {
                                var RoomMappingResponse = new UnifiedHotelAndRoomMapping_RoomTypeResponse()
                                {
                                    SupplierRoomCategory = mappingRoomRequest.SupplierRoomCategory,
                                    SupplierRoomCategoryId = mappingRoomRequest.SupplierRoomCategoryId,
                                    SupplierRoomId = mappingRoomRequest.SupplierRoomId,
                                    SupplierRoomName = mappingRoomRequest.SupplierRoomName,
                                    SupplierRoomTypeCode = mappingRoomRequest.SupplierRoomTypeCode,
                                    MappedRooms = new List<UnifiedHotelAndRoomMapping_MappedRoomType>()
                                };

                                if (string.IsNullOrWhiteSpace(mappingRoomRequest.SupplierRoomId) && !string.IsNullOrWhiteSpace(mappingRoomRequest.SupplierRoomTypeCode))
                                {
                                    RoomMappingResponse.MappedRooms = RoomMappings.Where(w => w.SupplierRoomTypeCode == mappingRoomRequest.SupplierRoomTypeCode).Select(s => new UnifiedHotelAndRoomMapping_MappedRoomType { RoomMapId = s.SystemRoomTypeMapId, TlgxAccoRoomId = s.TLGXAccoRoomId, TlgxAccoRoomCategory = s.SystemRoomCategory, TlgxAccoRoomName = s.SystemRoomTypeName }).ToList();
                                }
                                else if (!string.IsNullOrWhiteSpace(mappingRoomRequest.SupplierRoomId) && string.IsNullOrWhiteSpace(mappingRoomRequest.SupplierRoomTypeCode))
                                {
                                    RoomMappingResponse.MappedRooms = RoomMappings.Where(w => w.SupplierRoomId == mappingRoomRequest.SupplierRoomId).Select(s => new UnifiedHotelAndRoomMapping_MappedRoomType { RoomMapId = s.SystemRoomTypeMapId, TlgxAccoRoomId = s.TLGXAccoRoomId, TlgxAccoRoomCategory = s.SystemRoomCategory, TlgxAccoRoomName = s.SystemRoomTypeName }).ToList();
                                }
                                else if (!string.IsNullOrWhiteSpace(mappingRoomRequest.SupplierRoomId) && !string.IsNullOrWhiteSpace(mappingRoomRequest.SupplierRoomTypeCode))
                                {
                                    RoomMappingResponse.MappedRooms = RoomMappings.Where(w => w.SupplierRoomId == mappingRoomRequest.SupplierRoomId && w.SupplierRoomTypeCode == mappingRoomRequest.SupplierRoomTypeCode).Select(s => new UnifiedHotelAndRoomMapping_MappedRoomType { RoomMapId = s.SystemRoomTypeMapId, TlgxAccoRoomId = s.TLGXAccoRoomId, TlgxAccoRoomCategory = s.SystemRoomCategory, TlgxAccoRoomName = s.SystemRoomTypeName }).ToList();
                                }

                                RoomMappingResponseList.Add(RoomMappingResponse);

                                #region Roomtype Mapping Online data insert into Mongo
                                if (RoomMappingResponse.MappedRooms.Count == 0)
                                {
                                    int? SystemProductCode = null;
                                    if (int.TryParse(mappingResponse.CommonHotelId, out int CommonHotelId))
                                    {
                                        SystemProductCode = CommonHotelId;
                                    }

                                    #region Insert into RoomType Mapping Online collection if mappings not found

                                    writeModelDetails.Add(InsertRoomTypeMappingOnline(collection_rto, new RoomTypeMappingOnline
                                    {
                                        Amenities = mappingRoomRequest.Amenities,
                                        BatchId = RQ.SessionId,
                                        BathRoomType = mappingRoomRequest.BathRoomType,
                                        BeddingConfig = mappingRoomRequest.BeddingConfig,
                                        Bedrooms = mappingRoomRequest.Bedrooms,
                                        BedType = mappingRoomRequest.BedType,
                                        ChildAge = mappingRoomRequest.ChildAge,
                                        CreateDateTime = DateTime.Now,
                                        ExtraBed = mappingRoomRequest.ExtraBed,
                                        FloorName = mappingRoomRequest.FloorName,
                                        FloorNumber = mappingRoomRequest.FloorNumber,
                                        MaxAdults = mappingRoomRequest.MaxAdults,
                                        MaxChild = mappingRoomRequest.MaxChild,
                                        MaxGuestOccupancy = mappingRoomRequest.MaxGuestOccupancy,
                                        MaxInfants = mappingRoomRequest.MaxInfants,
                                        MinGuestOccupancy = mappingRoomRequest.MinGuestOccupancy,
                                        Mode = "Online",
                                        PromotionalVendorCode = mappingRoomRequest.PromotionalVendorCode,
                                        Quantity = mappingRoomRequest.Quantity,
                                        RatePlan = mappingRoomRequest.RatePlan,
                                        RatePlanCode = mappingRoomRequest.RatePlanCode,
                                        RoomLocationCode = mappingRoomRequest.RoomLocationCode,
                                        RoomSize = mappingRoomRequest.RoomSize,
                                        RoomView = mappingRoomRequest.RoomView,
                                        Smoking = mappingRoomRequest.Smoking,
                                        SupplierId = mappingRequest.SupplierCode,
                                        SupplierProductId = mappingRequest.SupplierProductCode,
                                        SupplierRoomCategory = mappingRoomRequest.SupplierRoomCategory,
                                        SupplierRoomCategoryId = mappingRoomRequest.SupplierRoomCategoryId,
                                        SupplierRoomId = mappingRoomRequest.SupplierRoomId,
                                        SupplierRoomName = mappingRoomRequest.SupplierRoomName,
                                        SupplierRoomTypeCode = mappingRoomRequest.SupplierRoomTypeCode,
                                        TLGXCommonHotelId = mappingResponse.TlgxAccoId,
                                        SystemProductCode = SystemProductCode
                                    }));

                                    #endregion

                                }
                                #endregion
                            }
                            mappingResponse.SupplierRoomTypes = RoomMappingResponseList;

                            mappingResponseList.Add(mappingResponse);
                        }

                        returnResult.MappingResponses = mappingResponseList;

                        if (writeModelDetails.Any())
                        {
                            Task.Run(() => { collection_rto.BulkWrite(writeModelDetails); });
                        }

                        #endregion Build Response
                    }
                }
                return Request.CreateResponse(HttpStatusCode.OK, returnResult);
            }
            catch (Exception ex)
            {
                NLogHelper.Nlogger_LogError.LogError(ex, this.GetType().FullName, Request.GetActionDescriptor().ActionName, Request.RequestUri.PathAndQuery);
                HttpResponseMessage response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Server Error. Contact Admin. Error Date : " + DateTime.Now.ToString());
                return response;
            }
        }

        /// <summary>
        /// Retrieves the Company specifc Accommodation and Room Codes for the requested Supplier Product and Rooms. 
        /// In addition to this mapping tasks, it also retrieves a number of MDM attributes for the Accommodation Product.
        /// If Mapped Rooms are found, they will be returned  in the mappedRooms mode of the mappingResponses.
        /// If the mappedRooms node is empty then no Rooms have been Mapped for this particular request.
        /// </summary>
        /// <param name="RQ">Combination of Hotel and Room mapping request.</param>
        /// <returns>Combination of Hotel and Room mapping response.</returns>
        [HttpPost]
        [Route("CompanySpecificHotelAndRoomTypeMapping")]
        [ResponseType(typeof(List<CompanySpecificHotelAndRoomType_RS>))]
        public async Task<HttpResponseMessage> CompanySpecificHotelAndRoomTypeMapping(CompanySpecificHotelAndRoomType_Rq RQ)
        {
            try
            {
                HttpResponseMessage response = new HttpResponseMessage();
                if (RQ != null)
                {
                    if (ValidateCompanySpecificHotelAndRoomTypeMappingRQ(RQ))
                    {
                        var returnResult = new CompanySpecificHotelAndRoomType_RS()
                        {
                            SessionId = RQ.SessionId
                        };

                        if (RQ.MappingRequests != null)
                        {
                            #region Variable Declaration and Initialization

                            _database = MongoDBHandler.mDatabase();
                            IMongoCollection<RoomTypeMappingOnline> collection_rto = _database.GetCollection<RoomTypeMappingOnline>("RoomTypeMappingOnline");

                            var SupplierCodes = RQ.MappingRequests.Where(w => w.SupplierCode != null).Select(x => x.SupplierCode.ToUpper()).Distinct().ToList();
                            var SupplierProductCodes = RQ.MappingRequests.Where(w => w.SupplierProductCode != null).Select(x => x.SupplierProductCode.ToUpper()).Distinct().ToList();
                            var SupplierRoomTypeIds = RQ.MappingRequests.SelectMany(p => p.SupplierRoomTypes.Where(w => w.SupplierRoomId != null).Select(s => s.SupplierRoomId.ToUpper())).Distinct().ToList();
                            var SupplierRoomTypeCodes = RQ.MappingRequests.SelectMany(p => p.SupplierRoomTypes.Where(w => w.SupplierRoomTypeCode != null).Select(s => s.SupplierRoomTypeCode.ToUpper())).Distinct().ToList();
                            var TLGXCompanyId = RQ.MappingRequests.Where(w => w.TLGXCompanyCode != null).Select(x => x.TLGXCompanyCode.ToUpper()).Distinct().ToList();

                            #endregion Variable Declaration and Initialization

                            #region Build Response

                            var mappingResponseList = new List<CompanySpecificHotelAndRoomType_Response>();
                            var writeModelDetails = new List<WriteModel<RoomTypeMappingOnline>>();

                            // Retreiving the specific records from filtered results for hotel mapping(AccomodationProductMapping)
                            List<DC_ConpanyAccommodationMapping> searchedConpanyAccommodationMapping = GetCompanyAccommodationProductMapping(SupplierCodes, SupplierProductCodes, TLGXCompanyId);

                            //Looping the requested object to create result
                            foreach (var mappingRequest in RQ.MappingRequests)
                            {
                                var mappingResponse = new CompanySpecificHotelAndRoomType_Response()
                                {
                                    SequenceNumber = mappingRequest.SequenceNumber,
                                    SupplierCode = mappingRequest.SupplierCode,
                                    SupplierProductCode = mappingRequest.SupplierProductCode,
                                    TlgxCompanyCode = mappingRequest.TLGXCompanyCode
                                };


                                var HotelMapping = searchedConpanyAccommodationMapping.Where(w => w.SupplierCode == mappingRequest.SupplierCode && w.SupplierProductCode == mappingRequest.SupplierProductCode && w.TLGXCompanyId == mappingRequest.TLGXCompanyCode).Select(s => s).FirstOrDefault();

                                if (HotelMapping != null)
                                {
                                    mappingResponse.ProductCategory = "Accommodation";
                                    mappingResponse.SupplierProductName = HotelMapping.SupplierProductName;
                                    mappingResponse.TlgxCommonProductId = Convert.ToString(HotelMapping.CommonProductId);
                                    mappingResponse.TlgxCompanyProductId = HotelMapping.CompanyProductId;
                                    mappingResponse.ProductName = HotelMapping.CompanyProductName;
                                    mappingResponse.ProductCategorySubType = HotelMapping.ProductCategorySubType;
                                    mappingResponse.Chain = HotelMapping.Chain;
                                    mappingResponse.Brand = HotelMapping.Brand;
                                    mappingResponse.Interests = (!string.IsNullOrEmpty(HotelMapping.Interest) && HotelMapping.Interest != null) ? HotelMapping.Interest.Split(',').ToList() : new List<string>();
                                    mappingResponse.Rating = HotelMapping.Rating;
                                }

                                var RoomMappingResponseList = new List<CompanySpecificHotelAndRoomTypeMapping_RoomTypeResponse>();

                                //looping the requested object to create result for Room data
                                foreach (var mappingRoomRequest in mappingRequest.SupplierRoomTypes)
                                {
                                    //Check the Room data is available in RT collection of object.
                                    var rooms = HotelMapping != null ? RoomData(mappingRoomRequest, HotelMapping.MappedRooms) : null;
                                    if (rooms != null && rooms.Any())
                                    {
                                        foreach (var room in rooms)
                                        {
                                            var RoomMappingResponse = new CompanySpecificHotelAndRoomTypeMapping_RoomTypeResponse()
                                            {
                                                SupplierRoomCategory = room.SupplierRoomCategory,
                                                SupplierRoomCategoryId = room.SupplierRoomCategoryId,
                                                SupplierRoomId = room.SupplierRoomId,
                                                SupplierRoomName = room.SupplierRoomName,
                                                SupplierRoomTypeCode = room.SupplierRoomTypeCode,
                                                MappedRooms = new List<CompanySpecificHotelAndRoomTypeMapping_MappedRoomType>()
                                            };

                                            RoomMappingResponse.MappedRooms.Add(new CompanySpecificHotelAndRoomTypeMapping_MappedRoomType()
                                            {
                                                TlgxCompanyRoomId = room.CompanyRoomId,
                                                TLGXCommonRoomId = room.TLGXCommonRoomId,
                                                CompanyRoomName = room.CompanyRoomName,
                                                CompanyRoomCategory = room.CompanyRoomCategory,
                                                NakshatraRoomMappingId = Convert.ToInt64(room.NakshatraRoomMappingId),
                                            });
                                            RoomMappingResponseList.Add(RoomMappingResponse);
                                        }
                                    }
                                    else
                                    {
                                        var RoomMappingResponse = GetRoomTypeNotFoundRS(collection_rto, writeModelDetails, mappingRequest, mappingResponse, mappingRoomRequest);
                                        RoomMappingResponseList.Add(RoomMappingResponse);
                                        int? SystemProductCode = null;
                                        if (int.TryParse(mappingResponse.TlgxCommonProductId, out int CommonHotelId))
                                        {
                                            SystemProductCode = CommonHotelId;
                                        }

                                        var rtmo = new RoomTypeMappingOnline
                                        {
                                            CreateDateTime = DateTime.Now,
                                            Mode = "Online",
                                            SupplierId = mappingRequest.SupplierCode,
                                            SupplierProductId = mappingRequest.SupplierProductCode,
                                            TLGXCommonHotelId = mappingResponse.TlgxCompanyProductId,
                                            SystemProductCode = SystemProductCode,
                                            SupplierRoomCategory = mappingRoomRequest.SupplierRoomCategory,
                                            SupplierRoomCategoryId = mappingRoomRequest.SupplierRoomCategoryId,
                                            SupplierRoomId = mappingRoomRequest.SupplierRoomId,
                                            SupplierRoomName = mappingRoomRequest.SupplierRoomName,
                                            SupplierRoomTypeCode = mappingRoomRequest.SupplierRoomTypeCode
                                        };
                                        var filter = InsertRoomTypeMappingOnlineCompanySpecificRequest(collection_rto, rtmo);

                                        var update = Builders<RoomTypeMappingOnline>.Update.Set("CreateDateTime", rtmo.CreateDateTime)
                                                    .Set("Mode", rtmo.Mode)
                                                    .Set("SupplierId", rtmo.SupplierId)
                                                    .Set("SupplierProductId", rtmo.SupplierProductId)
                                                    .Set("TLGXCommonHotelId", rtmo.TLGXCommonHotelId)
                                                    .Set("SystemProductCode", rtmo.SystemProductCode)
                                                    .Set("SupplierRoomCategory", rtmo.SupplierRoomCategory)
                                                    .Set("SupplierRoomCategoryId", rtmo.SupplierRoomCategoryId)
                                                    .Set("SupplierRoomId", rtmo.SupplierRoomId)
                                                    .Set("SupplierRoomName", rtmo.SupplierRoomName)
                                                    .Set("SupplierRoomTypeCode", rtmo.SupplierRoomTypeCode)
                                                    .Set("BatchId", rtmo.BatchId)
                                                    .Set("MaxAdults", rtmo.MaxAdults)
                                                    .Set("MaxChild", rtmo.MaxChild)
                                                    .Set("MaxInfants", rtmo.MaxInfants)
                                                    .Set("MaxGuestOccupancy", rtmo.MaxGuestOccupancy)
                                                    .Set("Quantity", rtmo.Quantity)
                                                    .Set("RatePlan", rtmo.RatePlan)
                                                    .Set("RatePlanCode", rtmo.RatePlanCode)
                                                    .Set("RoomSize", rtmo.RoomSize)
                                                    .Set("BathRoomType", rtmo.BathRoomType)
                                                    .Set("RoomView", rtmo.RoomView)
                                                    .Set("FloorName", rtmo.FloorName)
                                                    .Set("FloorNumber", rtmo.FloorNumber)
                                                    .Set("Amenities", rtmo.Amenities)
                                                    .Set("RoomLocationCode", rtmo.RoomLocationCode)
                                                    .Set("ChildAge", rtmo.ChildAge)
                                                    .Set("ExtraBed", rtmo.ExtraBed)
                                                    .Set("Bedrooms", rtmo.Bedrooms)
                                                    .Set("Smoking", rtmo.Smoking)
                                                    .Set("BedType", rtmo.BedType)
                                                    .Set("MinGuestOccupancy", rtmo.MinGuestOccupancy)
                                                    .Set("PromotionalVendorCode", rtmo.PromotionalVendorCode)
                                                    .Set("BeddingConfig", rtmo.BeddingConfig)
                                                    .Set("CityCode", rtmo.CityCode)
                                                    .Set("CityName", rtmo.CityName)
                                                    .Set("CountryCode", rtmo.CountryCode)
                                                    .Set("CountryName", rtmo.CountryName)
                                                    .Set("StateCode", rtmo.StateCode)
                                                    .Set("StateName", rtmo.StateName)
                                                    .Set("RoomDescription", rtmo.RoomDescription)
                                                    .Set("SupplierProductName", rtmo.SupplierProductName)
                                                    .Set("SupplierProvider", rtmo.SupplierProvider)
                                                    .Set("ProcessDateTime", rtmo.ProcessDateTime)
                                                    .Set("ProcessBatchId", rtmo.ProcessBatchId)
                                                    .Set("ProcessBatchNo", rtmo.ProcessBatchNo)
                                                    .Set("Accommodation_SupplierRoomType_Id", rtmo.Accommodation_SupplierRoomType_Id)
                                                    .Set("Accommodation_Id", rtmo.Accommodation_Id)
                                                    .Set("Accommodation_RoomInfo_Id", rtmo.Accommodation_RoomInfo_Id)
                                                    .Set("Status", rtmo.Status)
                                                    .Set("SystemRoomTypeMapId", rtmo.SystemRoomTypeMapId)
                                                    .Set("MatchingScore", rtmo.MatchingScore)
                                                    .Set("SystemRoomTypeCode", rtmo.SystemRoomTypeCode)
                                                    .Set("TLGXRoomTypeCode", rtmo.TLGXRoomTypeCode);

                                        //var result = collection.UpdateOne(filter, update, new UpdateOptions { IsUpsert = true });
                                        //writeModelDetails.Add(new DeleteManyModel<RoomTypeMappingOnline>(filter));
                                        //writeModelDetails.Add(new ReplaceOneModel<RoomTypeMappingOnline>(filter, rtmo) { IsUpsert = true });
                                        //writeModelDetails.Add(new UpdateOneModel<RoomTypeMappingOnline>(filter, update) { IsUpsert = true });

                                        var exitingData = collection_rto.Find(filter).ToList();
                                        if (!exitingData.Any())
                                        {
                                            collection_rto.ReplaceOne(filter, rtmo, new UpdateOptions { IsUpsert = true });
                                        }
                                    }
                                }
                                mappingResponse.SupplierRoomTypes = RoomMappingResponseList;
                                mappingResponseList.Add(mappingResponse);
                            }
                            returnResult.MappingResponses = mappingResponseList;

                            //if (writeModelDetails.Any())
                            //{
                            //    collection_rto.BulkWriteAsync(writeModelDetails);
                            //}
                            #endregion Build Response
                        }
                        response = Request.CreateResponse(HttpStatusCode.OK, returnResult);
                    }
                    else
                    {
                        response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid request");
                    }
                }
                else
                {
                    response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Malformed request");
                }
                return response;
            }
            catch (Exception ex)
            {
                NLogHelper.Nlogger_LogError.LogError(ex, this.GetType().FullName, Request.GetActionDescriptor().ActionName, Request.RequestUri.PathAndQuery);
                HttpResponseMessage response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Server Error. Contact Admin. Error Date : " + DateTime.Now.ToString());
                return response;
            }
        }

        private ReplaceOneModel<RoomTypeMappingOnline> InsertRoomTypeMappingOnline(IMongoCollection<RoomTypeMappingOnline> collection, RoomTypeMappingOnline rtmo)
        {
            var builder = Builders<RoomTypeMappingOnline>.Filter;
            var filter = builder.Empty;

            if (!string.IsNullOrWhiteSpace(rtmo.TLGXCommonHotelId))
            {
                filter = filter & builder.Eq(c => c.TLGXCommonHotelId, rtmo.TLGXCommonHotelId.ToUpper());
            }

            if (!string.IsNullOrWhiteSpace(rtmo.SupplierId))
            {
                filter = filter & builder.Eq(c => c.SupplierId, rtmo.SupplierId.ToUpper());
            }

            if (!string.IsNullOrWhiteSpace(rtmo.SupplierProductId))
            {
                filter = filter & builder.Eq(c => c.SupplierProductId, rtmo.SupplierProductId.ToUpper());
            }

            if (!string.IsNullOrWhiteSpace(rtmo.SupplierRoomTypeCode))
            {
                filter = filter & builder.Eq(c => c.SupplierRoomTypeCode, rtmo.SupplierRoomTypeCode);
            }

            if (!string.IsNullOrWhiteSpace(rtmo.SupplierRoomId))
            {
                filter = filter & builder.Eq(c => c.SupplierRoomId, rtmo.SupplierRoomId);
            }
            return new ReplaceOneModel<RoomTypeMappingOnline>(filter, rtmo) { IsUpsert = true };
        }


        private FilterDefinition<RoomTypeMappingOnline> InsertRoomTypeMappingOnlineCompanySpecificRequest(IMongoCollection<RoomTypeMappingOnline> collection, RoomTypeMappingOnline rtmo)
        {
            var builder = Builders<RoomTypeMappingOnline>.Filter;
            FilterDefinition<RoomTypeMappingOnline> filter = builder.Empty;

            if (!string.IsNullOrWhiteSpace(rtmo.TLGXCommonHotelId))
            {
                filter = filter & builder.Eq(c => c.TLGXCommonHotelId, rtmo.TLGXCommonHotelId.ToUpper());
            }

            if (!string.IsNullOrWhiteSpace(rtmo.SupplierId))
            {
                filter = filter & builder.Eq(c => c.SupplierId, rtmo.SupplierId.ToUpper());
            }

            if (!string.IsNullOrWhiteSpace(rtmo.SupplierProductId))
            {
                filter = filter & builder.Eq(c => c.SupplierProductId, rtmo.SupplierProductId.ToUpper());
            }

            if (!string.IsNullOrWhiteSpace(rtmo.SupplierRoomTypeCode))
            {
                filter = filter & builder.Eq(c => c.SupplierRoomTypeCode, rtmo.SupplierRoomTypeCode.ToUpper());
            }

            if (!string.IsNullOrWhiteSpace(rtmo.SupplierRoomId))
            {
                filter = filter & builder.Eq(c => c.SupplierRoomId, rtmo.SupplierRoomId.ToUpper());
            }

            if (!string.IsNullOrWhiteSpace(rtmo.SupplierRoomName))
            {
                filter = filter & builder.Eq(c => c.SupplierRoomName, rtmo.SupplierRoomName.ToUpper());
            }

            if (!string.IsNullOrWhiteSpace(rtmo.SupplierRoomCategory))
            {
                filter = filter & builder.Eq(c => c.SupplierRoomCategory, rtmo.SupplierRoomCategory.ToUpper());
            }

            if (!string.IsNullOrWhiteSpace(rtmo.SupplierRoomCategoryId))
            {
                filter = filter & builder.Eq(c => c.SupplierRoomCategoryId, rtmo.SupplierRoomCategoryId.ToUpper());
            }
            return filter;
            //return new ReplaceOneModel<RoomTypeMappingOnline>(filter, rtmo) { IsUpsert = true };
        }

        private List<DC_ConpanyAccommodationMapping> GetCompanyAccommodationProductMapping(List<string> SupplierCodes, List<string> SupplierProductCodes, List<string> TLGXCompanyId)
        {
            FilterDefinition<BsonDocument> filterCompanyAccommodationProductMapping = Builders<BsonDocument>.Filter.Empty;
            filterCompanyAccommodationProductMapping = filterCompanyAccommodationProductMapping & Builders<BsonDocument>.Filter.AnyIn("SupplierCode", SupplierCodes);
            filterCompanyAccommodationProductMapping = filterCompanyAccommodationProductMapping & Builders<BsonDocument>.Filter.AnyIn("SupplierProductCode", SupplierProductCodes);
            filterCompanyAccommodationProductMapping = filterCompanyAccommodationProductMapping & Builders<BsonDocument>.Filter.AnyIn("TLGXCompanyId", TLGXCompanyId);

            ProjectionDefinition<BsonDocument> projectCompanyAccommodationProductMapping = Builders<BsonDocument>.Projection.Include("SupplierCode");
            projectCompanyAccommodationProductMapping = projectCompanyAccommodationProductMapping.Exclude("_id");
            projectCompanyAccommodationProductMapping = projectCompanyAccommodationProductMapping.Include("SupplierCode");
            projectCompanyAccommodationProductMapping = projectCompanyAccommodationProductMapping.Include("SupplierProductCode");
            projectCompanyAccommodationProductMapping = projectCompanyAccommodationProductMapping.Include("SupplierProductName");
            projectCompanyAccommodationProductMapping = projectCompanyAccommodationProductMapping.Include("CompanyProductName");
            projectCompanyAccommodationProductMapping = projectCompanyAccommodationProductMapping.Include("CompanyProductId");
            projectCompanyAccommodationProductMapping = projectCompanyAccommodationProductMapping.Include("CommonProductId");
            projectCompanyAccommodationProductMapping = projectCompanyAccommodationProductMapping.Include("TLGXCompanyId");
            projectCompanyAccommodationProductMapping = projectCompanyAccommodationProductMapping.Include("Rating");
            projectCompanyAccommodationProductMapping = projectCompanyAccommodationProductMapping.Include("TLGXCompanyName");
            projectCompanyAccommodationProductMapping = projectCompanyAccommodationProductMapping.Include("CountryName");
            projectCompanyAccommodationProductMapping = projectCompanyAccommodationProductMapping.Include("CityName");
            projectCompanyAccommodationProductMapping = projectCompanyAccommodationProductMapping.Include("StateName");
            projectCompanyAccommodationProductMapping = projectCompanyAccommodationProductMapping.Include("ProductCategorySubType");
            projectCompanyAccommodationProductMapping = projectCompanyAccommodationProductMapping.Include("Brand");
            projectCompanyAccommodationProductMapping = projectCompanyAccommodationProductMapping.Include("Chain");
            projectCompanyAccommodationProductMapping = projectCompanyAccommodationProductMapping.Include("Interest");
            projectCompanyAccommodationProductMapping = projectCompanyAccommodationProductMapping.Include("MappedRooms");


            IMongoCollection<BsonDocument> collectionCompanyAccommodationProductMapping = _database.GetCollection<BsonDocument>("CompanyAccommodationProductMapping");
            var searchHotelMappingResult1 = collectionCompanyAccommodationProductMapping.Find(filterCompanyAccommodationProductMapping).Project(projectCompanyAccommodationProductMapping).ToList();
            List<DC_ConpanyAccommodationMapping> searchedConpanyAccommodationMapping = JsonConvert.DeserializeObject<List<DC_ConpanyAccommodationMapping>>(searchHotelMappingResult1.ToJson());
            return searchedConpanyAccommodationMapping;
        }

        //If records are not found for RT then it will insert into RoomTypeOnline collection
        private CompanySpecificHotelAndRoomTypeMapping_RoomTypeResponse GetRoomTypeNotFoundRS(IMongoCollection<RoomTypeMappingOnline> collection_rto, List<WriteModel<RoomTypeMappingOnline>> writeModelDetails, CompanySpecificHotelAndRoomType_Request mappingRequest, CompanySpecificHotelAndRoomType_Response mappingResponse, CompanySpecificHotelAndRoomType_RoomTypeRequest mappingRoomRequest)
        {
            var RoomMappingResponse = new CompanySpecificHotelAndRoomTypeMapping_RoomTypeResponse();
            if (!string.IsNullOrEmpty(mappingRoomRequest.SupplierRoomTypeCode)
                || !string.IsNullOrEmpty(mappingRoomRequest.SupplierRoomName)
                || !string.IsNullOrEmpty(mappingRoomRequest.SupplierRoomId)
                || !string.IsNullOrEmpty(mappingRoomRequest.SupplierRoomCategoryId)
                || !string.IsNullOrEmpty(mappingRoomRequest.SupplierRoomCategory))
            {
                RoomMappingResponse.SupplierRoomCategory = mappingRoomRequest.SupplierRoomCategory;
                RoomMappingResponse.SupplierRoomCategoryId = mappingRoomRequest.SupplierRoomCategoryId;
                RoomMappingResponse.SupplierRoomId = mappingRoomRequest.SupplierRoomId;
                RoomMappingResponse.SupplierRoomName = mappingRoomRequest.SupplierRoomName;
                RoomMappingResponse.SupplierRoomTypeCode = mappingRoomRequest.SupplierRoomTypeCode;
                RoomMappingResponse.MappedRooms = new List<CompanySpecificHotelAndRoomTypeMapping_MappedRoomType>();
            }
            return RoomMappingResponse;
        }

        private List<DC_ConpanyAccommodationRoomMapping> RoomData(CompanySpecificHotelAndRoomType_RoomTypeRequest mappingRoomRequest, List<DC_ConpanyAccommodationRoomMapping> RoomMappings)
        {
            var supplierFilter = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(mappingRoomRequest.SupplierRoomTypeCode))
            {
                supplierFilter.Add("SupplierRoomTypeCode", mappingRoomRequest.SupplierRoomTypeCode);
            }

            if (!string.IsNullOrEmpty(mappingRoomRequest.SupplierRoomId))
            {
                supplierFilter.Add("SupplierRoomId", mappingRoomRequest.SupplierRoomId);
            }

            if (!string.IsNullOrEmpty(mappingRoomRequest.SupplierRoomName))
            {
                supplierFilter.Add("SupplierRoomName", mappingRoomRequest.SupplierRoomName);
            }

            if (!string.IsNullOrEmpty(mappingRoomRequest.SupplierRoomCategory))
            {
                supplierFilter.Add("SupplierRoomCategory", mappingRoomRequest.SupplierRoomCategory);
            }

            if (!string.IsNullOrEmpty(mappingRoomRequest.SupplierRoomCategory))
            {
                supplierFilter.Add("SupplierRoomCategoryId", mappingRoomRequest.SupplierRoomCategory);
            }

            var rooms = new List<DC_ConpanyAccommodationRoomMapping>();
            if (supplierFilter.Any() && RoomMappings.Any())
            {
                foreach (var supplier in supplierFilter)
                {
                    rooms = GetRoomMappingsDetails(supplier.Key, supplier.Value, RoomMappings);
                    if (rooms.Any())
                    {
                        break;
                    }
                };
            }
            return rooms;
        }

        private List<DC_ConpanyAccommodationRoomMapping> GetRoomMappingsDetails(string supplierRoomType, string supplierRoomTypeValue, List<DC_ConpanyAccommodationRoomMapping> RoomMappings)
        {
            var rooms = new List<DC_ConpanyAccommodationRoomMapping>();
            if (supplierRoomType == "SupplierRoomTypeCode")
            {
                rooms = RoomMappings.Where(w => w.SupplierRoomTypeCode == supplierRoomTypeValue).ToList();
            }
            else if (supplierRoomType == "SupplierRoomId")
            {
                rooms = RoomMappings.Where(w => w.SupplierRoomId == supplierRoomTypeValue).ToList();
            }
            else if (supplierRoomType == "SupplierRoomName")
            {
                rooms = RoomMappings.Where(w => w.SupplierRoomName == supplierRoomTypeValue).ToList();
            }
            else if (supplierRoomType == "SupplierRoomCategory")
            {
                rooms = RoomMappings.Where(w => w.SupplierRoomCategory == supplierRoomTypeValue).ToList();
            }
            else if (supplierRoomType == "SupplierRoomCategoryId")
            {
                rooms = RoomMappings.Where(w => w.SupplierRoomCategoryId == supplierRoomTypeValue).ToList();
            }
            return rooms;
        }

        private bool ValidateCompanySpecificHotelAndRoomTypeMappingRQ(CompanySpecificHotelAndRoomType_Rq RQ)
        {
            var isValidRQ = false;

            if (RQ != null && !string.IsNullOrEmpty(RQ.SessionId) && RQ.MappingRequests != null)
            {
                foreach (var mappingRequest in RQ.MappingRequests)
                {
                    if (!string.IsNullOrEmpty(mappingRequest.SequenceNumber) &&
                        !string.IsNullOrEmpty(mappingRequest.SupplierCode) &&
                        !string.IsNullOrEmpty(mappingRequest.SupplierProductCode) &&
                        !string.IsNullOrEmpty(mappingRequest.TLGXCompanyCode) &&
                        (mappingRequest.SupplierRoomTypes != null))
                    {
                        if (mappingRequest.SupplierRoomTypes.Any())
                        {
                            foreach (var roomType in mappingRequest.SupplierRoomTypes)
                            {
                                isValidRQ = false;
                                if (!string.IsNullOrEmpty(roomType.SupplierRoomId)
                                || !string.IsNullOrEmpty(roomType.SupplierRoomTypeCode)
                                || !string.IsNullOrEmpty(roomType.SupplierRoomName)
                                || !string.IsNullOrEmpty(roomType.SupplierRoomCategory)
                                || !string.IsNullOrEmpty(roomType.SupplierRoomCategoryId))
                                {
                                    isValidRQ = true;
                                }
                                else
                                {
                                    return isValidRQ;
                                }
                            }
                        }
                        else
                        {
                            isValidRQ = true;
                        }
                    }
                    else
                    {
                        return isValidRQ;
                    }
                    if (!isValidRQ)
                    {
                        return isValidRQ;
                    }

                }
            }
            return isValidRQ;
        }
    }
}