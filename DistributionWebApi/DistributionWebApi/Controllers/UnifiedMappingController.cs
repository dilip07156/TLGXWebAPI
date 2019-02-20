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
    /// Used to retrieve Unified mappings 
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


                        List<ProductMappingLite> searchedHotelMappingData = new List<ProductMappingLite>();
                        List<HotelRoomTypeMappingModel> searchedRoomMappingData = new List<HotelRoomTypeMappingModel>();
                        List<DC_AccomodationMasterMapping> searchedAccomodationSearchData = new List<DC_AccomodationMasterMapping>();

                        var SupplierCodes = RQ.MappingRequests.Where(w => w.SupplierCode != null).Select(x => x.SupplierCode.ToUpper()).Distinct().ToArray();
                        var SupplierProductCodes = RQ.MappingRequests.Where(w => w.SupplierProductCode != null).Select(x => x.SupplierProductCode.ToUpper()).Distinct().ToArray();
                        var SupplierRoomTypeIds = RQ.MappingRequests.SelectMany(p => p.SupplierRoomTypes.Where(w => w.SupplierRoomId != null).Select(s => s.SupplierRoomId.ToUpper())).Distinct().ToArray();
                        var SupplierRoomTypeCodes = RQ.MappingRequests.SelectMany(p => p.SupplierRoomTypes.Where(w => w.SupplierRoomTypeCode != null).Select(s => s.SupplierRoomTypeCode.ToUpper())).Distinct().ToArray();
                        //var TLGXCompanyId = RQ.MappingRequests.Where(w => w.TLGXCompanyId != null).Select(x => x.TLGXCompanyId.ToUpper()).Distinct().ToArray();

                        #endregion Variable Declaration and Initialization

                        #region HotelMappingSearch

                        FilterDefinition<BsonDocument> filterHotelMapping;
                        filterHotelMapping = Builders<BsonDocument>.Filter.Empty;

                        //string[] statusToCheck = { "MAPPED", "AUTOMAPPED" };
                        filterHotelMapping = filterHotelMapping & Builders<BsonDocument>.Filter.AnyIn("SupplierCode", SupplierCodes);
                        filterHotelMapping = filterHotelMapping & Builders<BsonDocument>.Filter.AnyIn("SupplierProductCode", SupplierProductCodes);
                        //filterHotelMapping = filterHotelMapping & Builders<BsonDocument>.Filter.AnyIn("MappingStatus", statusToCheck);

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
                        FilterDefinition<BsonDocument> filterAccoMaster;
                        filterAccoMaster = Builders<BsonDocument>.Filter.Empty;
                        filterAccoMaster = filterAccoMaster & Builders<BsonDocument>.Filter.AnyIn("_id", searchedHotelMappingData.Select(s => Convert.ToInt32(s.SystemProductCode)));
                        //filterAccoMaster = filterAccoMaster & Builders<BsonDocument>.Filter.AnyIn("AccomodationCompanyVersions.CompanyId", TLGXCompanyId);
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
                        var searchAccoMasterResult = collectionAccommodationMaster.Find(filterAccoMaster).Project(projectAccoMaster).ToList();
                        searchedAccomodationSearchData = JsonConvert.DeserializeObject<List<DC_AccomodationMasterMapping>>(searchAccoMasterResult.ToJson());
                        #endregion Fetch Accommodation Master

                        #region RoomMappingSearch

                        if (SupplierRoomTypeIds.Length > 0 || SupplierRoomTypeCodes.Length > 0)
                        {
                            FilterDefinition<BsonDocument> filterRoomMapping;
                            filterRoomMapping = Builders<BsonDocument>.Filter.Empty;

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

                        foreach (var mappingRequest in RQ.MappingRequests)
                        {
                            var mappingResponse = new UnifiedHotelAndRoomMapping_Response();

                            mappingResponse.SequenceNumber = mappingRequest.SequenceNumber;
                            mappingResponse.ProductType = mappingRequest.ProductType;
                            mappingResponse.SupplierCode = mappingRequest.SupplierCode;
                            mappingResponse.SupplierProductCode = mappingRequest.SupplierProductCode;

                            var HotelMapping = searchedHotelMappingData.Where(w => w.SupplierCode == mappingRequest.SupplierCode && w.SupplierProductCode == mappingRequest.SupplierProductCode).OrderByDescending(o => o.MapId).Select(s => s).FirstOrDefault();

                            if (HotelMapping != null)
                            {
                                var Acco = searchedAccomodationSearchData.Where(w => w.CommonHotelId == Convert.ToInt32(HotelMapping.SystemProductCode)).FirstOrDefault();

                                //Need to remove once it is Confirm
                                //if (Acco != null && Acco.AccomodationCompanyVersions != null)
                                //{
                                //    var companyVersion = Acco.AccomodationCompanyVersions.Where(x => x.CompanyId == mappingRequest.TLGXCompanyId).SingleOrDefault();

                                //    mappingResponse.CommonHotelId = HotelMapping.SystemProductCode;
                                //    mappingResponse.ProductMapId = HotelMapping.MapId;
                                //    mappingResponse.TlgxAccoId = companyVersion.TLGXAccoId;
                                //    mappingResponse.TlgxAccoName = companyVersion.ProductName;
                                //    mappingResponse.ContainsRoomMappings = (Acco == null ? false : Acco.IsRoomMappingCompleted);
                                //    mappingResponse.SystemCityCode = (Acco == null ? string.Empty : Acco.CityCode);
                                //    mappingResponse.SystemCityName = (Acco == null ? string.Empty : Acco.CityName);
                                //    mappingResponse.SystemCountryCode = (Acco == null ? string.Empty : Acco.CountryCode);
                                //    mappingResponse.SystemCountryName = (Acco == null ? string.Empty : Acco.CountryName);
                                //    mappingResponse.SystemStateCode = (Acco == null ? string.Empty : Acco.StateCode);
                                //    mappingResponse.SystemStateName = (Acco == null ? string.Empty : Acco.StateName);
                                //    mappingResponse.ProductSubType = companyVersion.ProductCatSubType;
                                //    mappingResponse.Chain = companyVersion.Chain;
                                //    mappingResponse.Brand = companyVersion.Brand;
                                //    mappingResponse.TlgxCompanyId = companyVersion.CompanyId;
                                //    mappingResponse.TlgxCompanyHotelId = companyVersion.CommonProductId;
                                //}
                                //else
                                //{
                                //Need to remove once it is Confirm
                                mappingResponse.CommonHotelId = HotelMapping.SystemProductCode;
                                mappingResponse.ProductMapId = HotelMapping.MapId;
                                mappingResponse.TlgxAccoId = HotelMapping.TlgxMdmHotelId;
                                mappingResponse.TlgxAccoName = (Acco == null ? string.Empty : Acco.HotelName);
                                mappingResponse.ContainsRoomMappings = (Acco == null ? false : Acco.IsRoomMappingCompleted);
                                mappingResponse.SystemCityCode = (Acco == null ? string.Empty : Acco.CityCode);
                                mappingResponse.SystemCityName = (Acco == null ? string.Empty : Acco.CityName);
                                mappingResponse.SystemCountryCode = (Acco == null ? string.Empty : Acco.CountryCode);
                                mappingResponse.SystemCountryName = (Acco == null ? string.Empty : Acco.CountryName);
                                mappingResponse.SystemStateCode = (Acco == null ? string.Empty : Acco.StateCode);
                                mappingResponse.SystemStateName = (Acco == null ? string.Empty : Acco.StateName);
                                mappingResponse.ProductSubType = (Acco == null ? string.Empty : Acco.ProductCategorySubType);
                                mappingResponse.Chain = (Acco == null ? string.Empty : Acco.Chain);
                                mappingResponse.Brand = (Acco == null ? string.Empty : Acco.Brand);

                                //Need to remove once it is Confirm
                                //mappingResponse.TlgxCompanyId = (Acco == null ? string.Empty : mappingRequest.TLGXCompanyId);
                                //mappingResponse.TlgxCompanyHotelId = (Acco == null ? string.Empty :  mappingRequest.TLGXCompanyId);
                                //}
                                //Need to remove once it is Confirm
                            }

                            var RoomMappings = searchedRoomMappingData.Where(w => w.supplierCode == mappingRequest.SupplierCode && w.SupplierProductId == mappingRequest.SupplierProductCode).Select(s => s).ToList();

                            var RoomMappingResponseList = new List<UnifiedHotelAndRoomMapping_RoomTypeResponse>();
                            foreach (var mappingRoomRequest in mappingRequest.SupplierRoomTypes)
                            {
                                var RoomMappingResponse = new UnifiedHotelAndRoomMapping_RoomTypeResponse();
                                RoomMappingResponse.SupplierRoomCategory = mappingRoomRequest.SupplierRoomCategory;
                                RoomMappingResponse.SupplierRoomCategoryId = mappingRoomRequest.SupplierRoomCategoryId;
                                RoomMappingResponse.SupplierRoomId = mappingRoomRequest.SupplierRoomId;
                                RoomMappingResponse.SupplierRoomName = mappingRoomRequest.SupplierRoomName;
                                RoomMappingResponse.SupplierRoomTypeCode = mappingRoomRequest.SupplierRoomTypeCode;

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
                                else
                                {
                                    RoomMappingResponse.MappedRooms = new List<UnifiedHotelAndRoomMapping_MappedRoomType>();
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
                                    RoomTypeMappingController _obj = new RoomTypeMappingController();
                                    // Fire & Forget 
                                    await Task.Run(() => _obj.InsertRoomTypeMappingOnline(collection_rto, new RoomTypeMappingOnline
                                    {
                                        //_id = ObjectId.GenerateNewId(),
                                        Amenities = mappingRoomRequest.Amenities,
                                        BatchId = RQ.SessionId, //RQ.BatchId,
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
                                        Mode = "Online", //RQ.Mode,
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

                        #endregion Build Response

                    }
                }

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, returnResult);
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
        /// Retrieves TLGX Acco Id, Common Hotel Id, TLGX Room Type Id for TLGX Common Hotel Ids, accepting multiple suppliers and multiple supplier room types.
        /// API can handle single / multiple TLGX Common Hotel Ids at a time. 
        /// API can return hotel mapping along with room type mappings where accommodation room info rows exists in MDM system and it has been processed.
        /// Please note that not all suppliers provide static data for mapping and real time requests into the mapping engine are not permitted.
        /// </summary>
        /// <param name="RQ">Combination of Hotel and Room mapping request.</param>
        /// <returns>Combination of Hotel and Room mapping response.</returns>
        [HttpPost]
        [Route("HotelAndRoomTypeMapping1")]
        [ResponseType(typeof(UnifiedHotelAndRoomMapping_RS1))]
        public async Task<HttpResponseMessage> GetUnifiedHotelAndRoomTypeMapping1(UnifiedHotelAndRoomMapping_RQ1 RQ)
        {
            try
            {
                var returnResult = new UnifiedHotelAndRoomMapping_RS1();

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
                        IMongoCollection<BsonDocument> collectionAccommodationRoomInfoMaster = _database.GetCollection<BsonDocument>("AccommodationRoomInfoMaster");


                        List<ProductMappingLite> searchedHotelMappingData = new List<ProductMappingLite>();
                        List<HotelRoomTypeMappingModel> searchedRoomMappingData = new List<HotelRoomTypeMappingModel>();
                        List<DC_AccomodationMasterMapping1> searchedAccomodationSearchData = new List<DC_AccomodationMasterMapping1>();
                        List<DC_AccomodationRoomInfo> searchedAccomodationRoomInfoSearchData = new List<DC_AccomodationRoomInfo>();

                        var SupplierCodes = RQ.MappingRequests.Where(w => w.SupplierCode != null).Select(x => x.SupplierCode.ToUpper()).Distinct().ToArray();
                        var SupplierProductCodes = RQ.MappingRequests.Where(w => w.SupplierProductCode != null).Select(x => x.SupplierProductCode.ToUpper()).Distinct().ToArray();
                        var SupplierRoomTypeIds = RQ.MappingRequests.SelectMany(p => p.SupplierRoomTypes.Where(w => w.SupplierRoomId != null).Select(s => s.SupplierRoomId.ToUpper())).Distinct().ToArray();
                        var SupplierRoomTypeCodes = RQ.MappingRequests.SelectMany(p => p.SupplierRoomTypes.Where(w => w.SupplierRoomTypeCode != null).Select(s => s.SupplierRoomTypeCode.ToUpper())).Distinct().ToArray();
                        var TLGXCompanyId = RQ.MappingRequests.Where(w => w.TLGXCompanyCode != null).Select(x => x.TLGXCompanyCode.ToUpper()).Distinct().ToArray();

                        #endregion Variable Declaration and Initialization

                        #region HotelMappingSearch

                        FilterDefinition<BsonDocument> filterHotelMapping;
                        filterHotelMapping = Builders<BsonDocument>.Filter.Empty;

                        //string[] statusToCheck = { "MAPPED", "AUTOMAPPED" };
                        filterHotelMapping = filterHotelMapping & Builders<BsonDocument>.Filter.AnyIn("SupplierCode", SupplierCodes);
                        filterHotelMapping = filterHotelMapping & Builders<BsonDocument>.Filter.AnyIn("SupplierProductCode", SupplierProductCodes);
                        //filterHotelMapping = filterHotelMapping & Builders<BsonDocument>.Filter.AnyIn("MappingStatus", statusToCheck);

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
                        FilterDefinition<BsonDocument> filterAccoMaster;
                        filterAccoMaster = Builders<BsonDocument>.Filter.Empty;
                        filterAccoMaster = filterAccoMaster & Builders<BsonDocument>.Filter.AnyIn("_id", searchedHotelMappingData.Select(s => Convert.ToInt32(s.SystemProductCode)));
                        filterAccoMaster = filterAccoMaster & Builders<BsonDocument>.Filter.AnyIn("AccomodationCompanyVersions.CompanyId", TLGXCompanyId);
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
                        projectAccoMaster = projectAccoMaster.Include("HotelStarRating");
                        projectAccoMaster = projectAccoMaster.Include("Interest");
                        var searchAccoMasterResult = collectionAccommodationMaster.Find(filterAccoMaster).Project(projectAccoMaster).ToList();
                        searchedAccomodationSearchData = JsonConvert.DeserializeObject<List<DC_AccomodationMasterMapping1>>(searchAccoMasterResult.ToJson());
                        #endregion Fetch Accommodation Master

                        #region RoomMappingSearch

                        if (SupplierRoomTypeIds.Length > 0 || SupplierRoomTypeCodes.Length > 0)
                        {
                            FilterDefinition<BsonDocument> filterRoomMapping;
                            filterRoomMapping = Builders<BsonDocument>.Filter.Empty;

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



                            #region Fetch AccommodationRoomInfoMaster data
                            FilterDefinition<BsonDocument> filterAccommodationRoomInfoMaster;
                            filterAccommodationRoomInfoMaster = Builders<BsonDocument>.Filter.Empty;
                            filterAccommodationRoomInfoMaster = filterAccommodationRoomInfoMaster & Builders<BsonDocument>.Filter.AnyIn("AccomodationRoomInfoCompanyVersions.TLGXAccoId", searchedRoomMappingData.Select(s => s.TLGXAccoId));
                            filterAccommodationRoomInfoMaster = filterAccommodationRoomInfoMaster & Builders<BsonDocument>.Filter.AnyIn("AccomodationRoomInfoCompanyVersions.TLGXAccoRoomID", searchedRoomMappingData.Select(s => s.TLGXAccoRoomId));
                            filterAccommodationRoomInfoMaster = filterAccommodationRoomInfoMaster & Builders<BsonDocument>.Filter.AnyIn("AccomodationRoomInfoCompanyVersions.CompanyId", TLGXCompanyId);

                            ProjectionDefinition<BsonDocument> projectAccommodationRoomInfoMaster = Builders<BsonDocument>.Projection.Include("CompanyId");
                            projectAccommodationRoomInfoMaster = projectAccommodationRoomInfoMaster.Include("_id");
                            projectAccommodationRoomInfoMaster = projectAccommodationRoomInfoMaster.Include("CommonHotelId");
                            projectAccommodationRoomInfoMaster = projectAccommodationRoomInfoMaster.Include("RoomView");
                            projectAccommodationRoomInfoMaster = projectAccommodationRoomInfoMaster.Include("RoomName");
                            projectAccommodationRoomInfoMaster = projectAccommodationRoomInfoMaster.Include("Smoking");
                            projectAccommodationRoomInfoMaster = projectAccommodationRoomInfoMaster.Include("BathRoomType");
                            projectAccommodationRoomInfoMaster = projectAccommodationRoomInfoMaster.Include("BedType");
                            projectAccommodationRoomInfoMaster = projectAccommodationRoomInfoMaster.Include("CompanyRoomCategory");
                            projectAccommodationRoomInfoMaster = projectAccommodationRoomInfoMaster.Include("RoomCategory");
                            projectAccommodationRoomInfoMaster = projectAccommodationRoomInfoMaster.Include("AccomodationRoomInfoCompanyVersions");

                            var searchAccommodationRoomInfoMasterResult = collectionAccommodationRoomInfoMaster.Find(filterAccommodationRoomInfoMaster).Project(projectAccommodationRoomInfoMaster).ToList();
                            searchedAccomodationRoomInfoSearchData = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<List<DC_AccomodationRoomInfo>>(searchAccommodationRoomInfoMasterResult.ToJson());

                            #endregion
                        }

                        #endregion RoomMappingSearch

                        #region Build Response

                        returnResult.SessionId = RQ.SessionId;
                        var mappingResponseList = new List<UnifiedHotelAndRoomMapping_Response1>();

                        foreach (var mappingRequest in RQ.MappingRequests)
                        {
                            var mappingResponse = new UnifiedHotelAndRoomMapping_Response1();

                            mappingResponse.SequenceNumber = mappingRequest.SequenceNumber;

                            mappingResponse.SupplierCode = mappingRequest.SupplierCode;
                            mappingResponse.SupplierProductCode = mappingRequest.SupplierProductCode;

                            var HotelMapping = searchedHotelMappingData.Where(w => w.SupplierCode == mappingRequest.SupplierCode && w.SupplierProductCode == mappingRequest.SupplierProductCode).OrderByDescending(o => o.MapId).Select(s => s).FirstOrDefault();

                            if (HotelMapping != null)
                            {
                                var Acco = searchedAccomodationSearchData.Where(w => w.CommonHotelId == Convert.ToInt32(HotelMapping.SystemProductCode)).FirstOrDefault();


                                if (Acco != null && Acco.AccomodationCompanyVersions != null)
                                {
                                    var companyVersion = Acco.AccomodationCompanyVersions.Where(x => x.CompanyId == mappingRequest.TLGXCompanyCode).SingleOrDefault();

                                    mappingResponse.TlgxCompanyCode = mappingRequest.TLGXCompanyCode;
                                    mappingResponse.NakshatraMappingId = HotelMapping.MapId;
                                    mappingResponse.CommonProductId = Convert.ToString(Acco.CommonHotelId);
                                    mappingResponse.CompanyProductId = companyVersion.TLGXAccoId;
                                    mappingResponse.ProductName = companyVersion.ProductName;
                                    mappingResponse.ProductCategory = "Accommodation";
                                    mappingResponse.ProductCategorySubType = companyVersion.ProductCatSubType;
                                    mappingResponse.Chain = companyVersion.Chain;
                                    mappingResponse.Brand = companyVersion.Brand;
                                    mappingResponse.SystemCityCode = (Acco == null ? string.Empty : Acco.CityCode);
                                    mappingResponse.SystemCityName = (Acco == null ? string.Empty : Acco.CityName);
                                    mappingResponse.SystemCountryCode = (Acco == null ? string.Empty : Acco.CountryCode);
                                    mappingResponse.SystemCountryName = (Acco == null ? string.Empty : Acco.CountryName);
                                    mappingResponse.SystemStateCode = (Acco == null ? string.Empty : Acco.StateCode);
                                    mappingResponse.SystemStateName = (Acco == null ? string.Empty : Acco.StateName);
                                    mappingResponse.Interests = (companyVersion == null ? Acco.Interests : companyVersion.Interests);
                                    mappingResponse.Rating = (companyVersion == null ? Acco.HotelStarRating : companyVersion.StarRating);
                                    mappingResponse.ContainsRoomMappings = (Acco == null ? false : Acco.IsRoomMappingCompleted);
                                    mappingResponse.DirectContract = (Acco == null ? false : Acco.IsDirectContract);
                                    //mappingResponse.ProductSubType = companyVersion.ProductCatSubType;

                                    //mappingResponse.TlgxCompanyId = companyVersion.CompanyId;
                                    mappingResponse.TlgxCompanyHotelId = companyVersion.CommonProductId;
                                }
                                else
                                {
                                    //mappingResponse.CommonHotelId = HotelMapping.SystemProductCode;
                                    //mappingResponse.ProductMapId = HotelMapping.MapId;
                                    //mappingResponse.TlgxAccoId = HotelMapping.TlgxMdmHotelId;
                                    //mappingResponse.TlgxAccoName = (Acco == null ? string.Empty : Acco.HotelName);
                                    //mappingResponse.ContainsRoomMappings = (Acco == null ? false : Acco.IsRoomMappingCompleted);
                                    mappingResponse.SystemCityCode = (Acco == null ? string.Empty : Acco.CityCode);
                                    mappingResponse.SystemCityName = (Acco == null ? string.Empty : Acco.CityName);
                                    mappingResponse.SystemCountryCode = (Acco == null ? string.Empty : Acco.CountryCode);
                                    mappingResponse.SystemCountryName = (Acco == null ? string.Empty : Acco.CountryName);
                                    mappingResponse.SystemStateCode = (Acco == null ? string.Empty : Acco.StateCode);
                                    mappingResponse.SystemStateName = (Acco == null ? string.Empty : Acco.StateName);
                                    //mappingResponse.ProductSubType = (Acco == null ? string.Empty : Acco.ProductCategorySubType);
                                    mappingResponse.Chain = (Acco == null ? string.Empty : Acco.Chain);
                                    mappingResponse.Brand = (Acco == null ? string.Empty : Acco.Brand);
                                    //mappingResponse.TlgxCompanyId = (Acco == null ? string.Empty : (Acco.AccomodationCompanyVersions.Where(x => x.CompanyId == mappingRequest.TLGXCompanyCode).SingleOrDefault().CompanyId) ?? null);
                                    mappingResponse.TlgxCompanyHotelId = (Acco == null ? string.Empty : (Acco.AccomodationCompanyVersions.Where(x => x.CompanyId == mappingRequest.TLGXCompanyCode).SingleOrDefault().CompanyProductId) ?? null);
                                }
                            }

                            var RoomMappings = searchedRoomMappingData.Where(w => w.supplierCode == mappingRequest.SupplierCode && w.SupplierProductId == mappingRequest.SupplierProductCode).Select(s => s).ToList();

                            var RoomMappingResponseList = new List<UnifiedHotelAndRoomMapping_RoomTypeResponse1>();

                            if (RoomMappings != null && RoomMappings.Count > 0)
                            {

                                var adminRoomInfo = searchedAccomodationRoomInfoSearchData.Where(w => w.CommonHotelId == Convert.ToInt32(HotelMapping.SystemProductCode)).FirstOrDefault(); ;

                                //var adminRoom = adminRoomInfo.AccomodationRoomInfoCompanyVersions.Where(x => x.TLGXAccoId == room.TLGXAccoId && x.TLGXAccoRoomID == room.TLGXAccoRoomId && x.CompanyId == mappingRequest.TLGXCompanyCode).FirstOrDefault();

                                foreach (var mappingRoomRequest in mappingRequest.SupplierRoomTypes)
                                {

                                    var RoomMappingResponse = new UnifiedHotelAndRoomMapping_RoomTypeResponse1();
                                    RoomMappingResponse.SupplierRoomCategory = mappingRoomRequest.SupplierRoomCategory;
                                    RoomMappingResponse.SupplierRoomCategoryId = mappingRoomRequest.SupplierRoomCategoryId;
                                    RoomMappingResponse.SupplierRoomId = mappingRoomRequest.SupplierRoomId;
                                    RoomMappingResponse.SupplierRoomName = mappingRoomRequest.SupplierRoomName;
                                    RoomMappingResponse.SupplierRoomTypeCode = mappingRoomRequest.SupplierRoomTypeCode;


                                    var d = from rm in  RoomMappings
                                            join ri in searchedAccomodationRoomInfoSearchData 
                                             on rm.SystemProductCode equals ri.CommonHotelId
                                            where 


                                    //if (string.IsNullOrWhiteSpace(mappingRoomRequest.SupplierRoomId) && !string.IsNullOrWhiteSpace(mappingRoomRequest.SupplierRoomTypeCode))
                                    //{
                                    //    RoomMappingResponse.MappedRooms = RoomMappings.Where(w => w.SupplierRoomTypeCode == mappingRoomRequest.SupplierRoomTypeCode).Select(s => new UnifiedHotelAndRoomMapping_MappedRoomType { RoomMapId = s.SystemRoomTypeMapId, TlgxAccoRoomId = s.TLGXAccoRoomId, TlgxAccoRoomCategory = s.SystemRoomCategory, TlgxAccoRoomName = s.SystemRoomTypeName }).ToList();
                                    //}
                                    //else if (!string.IsNullOrWhiteSpace(mappingRoomRequest.SupplierRoomId) && string.IsNullOrWhiteSpace(mappingRoomRequest.SupplierRoomTypeCode))
                                    //{
                                    //    RoomMappingResponse.MappedRooms = RoomMappings.Where(w => w.SupplierRoomId == mappingRoomRequest.SupplierRoomId).Select(s => new UnifiedHotelAndRoomMapping_MappedRoomType { RoomMapId = s.SystemRoomTypeMapId, TlgxAccoRoomId = s.TLGXAccoRoomId, TlgxAccoRoomCategory = s.SystemRoomCategory, TlgxAccoRoomName = s.SystemRoomTypeName }).ToList();
                                    //}
                                    //else if (!string.IsNullOrWhiteSpace(mappingRoomRequest.SupplierRoomId) && !string.IsNullOrWhiteSpace(mappingRoomRequest.SupplierRoomTypeCode))
                                    //{
                                    //    RoomMappingResponse.MappedRooms = RoomMappings.Where(w => w.SupplierRoomId == mappingRoomRequest.SupplierRoomId && w.SupplierRoomTypeCode == mappingRoomRequest.SupplierRoomTypeCode).Select(s => new UnifiedHotelAndRoomMapping_MappedRoomType { RoomMapId = s.SystemRoomTypeMapId, TlgxAccoRoomId = s.TLGXAccoRoomId, TlgxAccoRoomCategory = s.SystemRoomCategory, TlgxAccoRoomName = s.SystemRoomTypeName }).ToList();
                                    //}
                                    //else
                                    //{
                                    //    RoomMappingResponse.MappedRooms = new List<UnifiedHotelAndRoomMapping_MappedRoomType1>();
                                    //}

                                    RoomMappingResponseList.Add(RoomMappingResponse);

                                    #region Roomtype Mapping Online data insert into Mongo
                                    if (RoomMappingResponse.MappedRooms != null && RoomMappingResponse.MappedRooms.Count == 0)
                                    {
                                        int? SystemProductCode = null;
                                        //if (int.TryParse(mappingResponse.CommonHotelId, out int CommonHotelId))
                                        //{
                                        //    SystemProductCode = CommonHotelId;
                                        //}

                                        #region Insert into RoomType Mapping Online collection if mappings not found
                                        RoomTypeMappingController _obj = new RoomTypeMappingController();
                                        // Fire & Forget 
                                        await Task.Run(() => _obj.InsertRoomTypeMappingOnline(collection_rto, new RoomTypeMappingOnline
                                        {
                                            //_id = ObjectId.GenerateNewId(),

                                            CreateDateTime = DateTime.Now,

                                            Mode = "Online", //RQ.Mode,

                                            SupplierId = mappingRequest.SupplierCode,
                                            SupplierProductId = mappingRequest.SupplierProductCode,
                                            SupplierRoomCategory = mappingRoomRequest.SupplierRoomCategory,
                                            SupplierRoomCategoryId = mappingRoomRequest.SupplierRoomCategoryId,
                                            SupplierRoomId = mappingRoomRequest.SupplierRoomId,
                                            SupplierRoomName = mappingRoomRequest.SupplierRoomName,
                                            SupplierRoomTypeCode = mappingRoomRequest.SupplierRoomTypeCode,
                                            // TLGXCommonHotelId = mappingResponse.TlgxAccoId,
                                            SystemProductCode = SystemProductCode
                                        }));
                                        #endregion

                                    }
                                    #endregion
                                }

                                mappingResponse.SupplierRoomTypes = RoomMappingResponseList;

                                mappingResponseList.Add(mappingResponse);

                            }

                        }
                        returnResult.MappingResponses = mappingResponseList;

                        #endregion Build Response

                    }
                }

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, returnResult);
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