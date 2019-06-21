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
using MongoDB.Driver.Linq;


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
                        projectAccoMaster = projectAccoMaster.Include("TLGXAccoId");
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
                        var writeModelDetails = new List<WriteModel<RoomTypeMappingOnline>>();

                        foreach (var mappingRequest in RQ.MappingRequests)
                        {
                            var mappingResponse = new UnifiedHotelAndRoomMapping_Response();

                            mappingResponse.SequenceNumber = mappingRequest.SequenceNumber;
                            mappingResponse.ProductType = mappingRequest.ProductType;
                            mappingResponse.SupplierCode = mappingRequest.SupplierCode;
                            mappingResponse.SupplierProductCode = mappingRequest.SupplierProductCode;

                            var HotelMapping = searchedHotelMappingData.Where(w => w.SupplierCode == mappingRequest.SupplierCode.ToUpper() && w.SupplierProductCode == mappingRequest.SupplierProductCode.ToUpper()).OrderByDescending(o => o.MapId).Select(s => s).FirstOrDefault();

                            if (HotelMapping != null)
                            {
                                var Acco = searchedAccomodationSearchData.Where(w => w.CommonHotelId == Convert.ToInt32(HotelMapping.SystemProductCode)).FirstOrDefault();


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

                                    writeModelDetails.Add(InsertRoomTypeMappingOnline(collection_rto, new RoomTypeMappingOnline
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

                        // getting code of supplier in string variable.
                        string strSupplierCode = Convert.ToString(SupplierCodes[0]);

                        // Making object of Supplier MCON data values from Supplier Document
                        IMongoCollection<BsonDocument> collectionSupplierMaster = _database.GetCollection<BsonDocument>("Supplier");
                        ProjectionDefinition<BsonDocument> SupplierField = Builders<BsonDocument>.Projection.Include("SupplierCode");
                        SupplierField = SupplierField.Exclude("_id");
                        var builder = Builders<BsonDocument>.Filter;

                        //query to only bring data which is required and checking if related Supplier have MCON flag as YES or not and storing result in a variable.
                        // This query only checks the attribute "MCON.HoldInsertOnlineRoomTypeMappingData" from Supplier document in MCON node.
                        var query = builder.Eq("MCON.HoldInsertOnlineRoomTypeMappingData", "YES") & builder.Eq("SupplierCode", strSupplierCode);
                        var filteredSupplierList = await collectionSupplierMaster.Find(query).Project(SupplierField).ToListAsync();

                        // checking if variable "filteredSupplierList" is having some supplier and SupplierCode is not empty.
                        if (filteredSupplierList.Count > 0 || strSupplierCode == "")
                        {
                            // if count is greater than Zero, means this it should not be allowed to get written in database.
                            // If SupplierCode is coming empty due to some data issues, it should not be allowed to get written in database.
                        }
                        else
                        {
                            if (writeModelDetails.Any())
                            {
                                Task.Run(() => { collection_rto.BulkWrite(writeModelDetails); });
                            }
                        }
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

        #region GetUnifiedHotelAndRoomTypeMappingV2 Not in Use for now        


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


        private ReplaceOneModel<RoomTypeMappingOnline> InsertRoomTypeMappingOnlineCompanySpecificRequest(IMongoCollection<RoomTypeMappingOnline> collection, RoomTypeMappingOnline rtmo)
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

            if (!string.IsNullOrWhiteSpace(rtmo.SupplierRoomName))
            {
                filter = filter & builder.Eq(c => c.SupplierRoomName, rtmo.SupplierRoomName);
            }


            if (!string.IsNullOrWhiteSpace(rtmo.SupplierRoomCategory))
            {
                filter = filter & builder.Eq(c => c.SupplierRoomCategory, rtmo.SupplierRoomCategory);
            }

            if (!string.IsNullOrWhiteSpace(rtmo.SupplierRoomCategoryId))
            {
                filter = filter & builder.Eq(c => c.SupplierRoomCategoryId, rtmo.SupplierRoomCategoryId);
            }

            return new ReplaceOneModel<RoomTypeMappingOnline>(filter, rtmo) { IsUpsert = true };
        }

        /// <summary>
        /// Retrieves TLGX Acco Id, Common Hotel Id, TLGX Room Type Id for TLGX Common Hotel Ids, accepting multiple suppliers and multiple supplier room types.
        /// API can handle single / multiple TLGX Common Hotel Ids at a time. 
        /// API can return hotel mapping along with room type mappings where accommodation room info rows exists in MDM system and it has been processed.
        /// Please note that not all suppliers provide static data for mapping and real time requests into the mapping engine are not permitted.
        /// </summary>
        /// <param name="RQ">Combination of Hotel and Room mapping request.</param>
        /// <returns>Combination of Hotel and Room mapping response.</returns>
        //[HttpPost]
        //[Route("HotelAndRoomTypeMappingV2")]
        //[ResponseType(typeof(CompanySpecificHotelAndRoomType_RS))]
        //public async Task<HttpResponseMessage> GetUnifiedHotelAndRoomTypeMappingV2(CompanySpecificHotelAndRoomType_Rq RQ)
        //{
        //    try
        //    {
        //        var returnResult = new CompanySpecificHotelAndRoomType_RS();

        //        if (RQ != null)
        //        {
        //            returnResult.SessionId = RQ.SessionId;

        //            if (RQ.MappingRequests != null)
        //            {

        //                #region Variable Declaration and Initialization

        //                // declaring instance of Mongo db                        
        //                _database = MongoDBHandler.mDatabase();
        //                // declaring objects of Mongo collection
        //                IMongoCollection<BsonDocument> collectionRoomTypeMapping = _database.GetCollection<BsonDocument>("RoomTypeMapping");
        //                IMongoCollection<RoomTypeMappingOnline> collection_rto = _database.GetCollection<RoomTypeMappingOnline>("RoomTypeMappingOnline");
        //                IMongoCollection<BsonDocument> collectionProductMapping = _database.GetCollection<BsonDocument>("ProductMappingLite");
        //                IMongoCollection<BsonDocument> collectionAccommodationMaster = _database.GetCollection<BsonDocument>("AccommodationMaster");
        //                IMongoCollection<BsonDocument> collectionAccommodationRoomInfoMaster = _database.GetCollection<BsonDocument>("AccommodationRoomInfoMaster");

        //                // declaring list for mongo entities
        //                List<ProductMappingLite> searchedHotelMappingData = new List<ProductMappingLite>();
        //                List<HotelRoomTypeMappingModel> searchedRoomMappingData = new List<HotelRoomTypeMappingModel>();
        //                List<DC_AccomodationMasterMapping1> searchedAccomodationSearchData = new List<DC_AccomodationMasterMapping1>();
        //                List<DC_AccomodationRoomInfo> searchedAccomodationRoomInfoSearchData = new List<DC_AccomodationRoomInfo>();

        //                // setting request fields to variables
        //                var SupplierCodes = RQ.MappingRequests.Where(w => w.SupplierCode != null).Select(x => x.SupplierCode.ToUpper()).Distinct().ToArray();
        //                var SupplierProductCodes = RQ.MappingRequests.Where(w => w.SupplierProductCode != null).Select(x => x.SupplierProductCode.ToUpper()).Distinct().ToArray();



        //                var SupplierRoomTypeIds = RQ.MappingRequests.SelectMany(p => p.SupplierRoomTypes.Where(w => w.SupplierRoomId != null).Select(s => s.SupplierRoomId.ToUpper())).Distinct().ToArray();
        //                var SupplierRoomTypeCodes = RQ.MappingRequests.SelectMany(p => p.SupplierRoomTypes.Where(w => w.SupplierRoomTypeCode != null).Select(s => s.SupplierRoomTypeCode.ToUpper())).Distinct().ToArray();
        //                var TLGXCompanyId = RQ.MappingRequests.Where(w => w.TLGXCompanyCode != null).Select(x => x.TLGXCompanyCode.ToUpper()).Distinct().ToArray();

        //                #endregion Variable Declaration and Initialization

        //                #region HotelMappingSearch
        //                // Generating the filtered definition for collectionProductMapping
        //                FilterDefinition<BsonDocument> filterHotelMapping;
        //                filterHotelMapping = Builders<BsonDocument>.Filter.Empty;
        //                filterHotelMapping = filterHotelMapping & Builders<BsonDocument>.Filter.AnyIn("SupplierCode", SupplierCodes);
        //                filterHotelMapping = filterHotelMapping & Builders<BsonDocument>.Filter.AnyIn("SupplierProductCode", SupplierProductCodes);

        //                // including & excluding the fileds from definition result
        //                ProjectionDefinition<BsonDocument> projectHotelMapping = Builders<BsonDocument>.Projection.Include("SupplierCode");
        //                projectHotelMapping = projectHotelMapping.Exclude("_id");
        //                projectHotelMapping = projectHotelMapping.Include("SupplierProductCode");
        //                projectHotelMapping = projectHotelMapping.Include("SystemProductCode");
        //                projectHotelMapping = projectHotelMapping.Include("MapId");
        //                projectHotelMapping = projectHotelMapping.Include("TlgxMdmHotelId");

        //                // filtering the product mapping collection and retreiving filtred result from mongo collection "collectionProductMapping"
        //                var searchHotelMappingResult = collectionProductMapping.Find(filterHotelMapping).Project(projectHotelMapping).ToList();
        //                searchedHotelMappingData = JsonConvert.DeserializeObject<List<ProductMappingLite>>(searchHotelMappingResult.ToJson());

        //                #endregion HotelMappingSearch

        //                #region Fetch Accommodation Master

        //                // Generating the filtered definition for collectionAccommodationMaster
        //                FilterDefinition<BsonDocument> filterAccoMaster;
        //                filterAccoMaster = Builders<BsonDocument>.Filter.Empty;
        //                filterAccoMaster = filterAccoMaster & Builders<BsonDocument>.Filter.AnyIn("_id", searchedHotelMappingData.Select(s => Convert.ToInt32(s.SystemProductCode)));
        //                filterAccoMaster = filterAccoMaster & Builders<BsonDocument>.Filter.AnyIn("AccomodationCompanyVersions.CompanyId", TLGXCompanyId);

        //                // including & excluding the fileds from definition result
        //                ProjectionDefinition<BsonDocument> projectAccoMaster = Builders<BsonDocument>.Projection.Include("_id");
        //                projectAccoMaster = projectAccoMaster.Include("IsRoomMappingCompleted");
        //                projectAccoMaster = projectAccoMaster.Include("CountryCode");
        //                projectAccoMaster = projectAccoMaster.Include("CountryName");
        //                projectAccoMaster = projectAccoMaster.Include("CityCode");
        //                projectAccoMaster = projectAccoMaster.Include("CityName");
        //                projectAccoMaster = projectAccoMaster.Include("StateCode");
        //                projectAccoMaster = projectAccoMaster.Include("StateName");
        //                projectAccoMaster = projectAccoMaster.Include("HotelName");
        //                projectAccoMaster = projectAccoMaster.Include("ProductCategorySubType");
        //                projectAccoMaster = projectAccoMaster.Include("Brand");
        //                projectAccoMaster = projectAccoMaster.Include("Chain");
        //                projectAccoMaster = projectAccoMaster.Include("AccomodationCompanyVersions");
        //                projectAccoMaster = projectAccoMaster.Include("HotelStarRating");
        //                projectAccoMaster = projectAccoMaster.Include("Interest");

        //                // filtering the product mapping collection and retreiving filtred result from mongo collection "collectionAccommodationMaster"
        //                var searchAccoMasterResult = collectionAccommodationMaster.Find(filterAccoMaster).Project(projectAccoMaster).ToList();
        //                searchedAccomodationSearchData = JsonConvert.DeserializeObject<List<DC_AccomodationMasterMapping1>>(searchAccoMasterResult.ToJson());
        //                #endregion Fetch Accommodation Master

        //                #region RoomMappingSearch
        //                // Checking if Room type Id provided in request body or not
        //                if (SupplierRoomTypeIds.Length > 0 || SupplierRoomTypeCodes.Length > 0)
        //                {
        //                    // Generating the filtered definition for collectionRoomTypeMapping
        //                    FilterDefinition<BsonDocument> filterRoomMapping;
        //                    filterRoomMapping = Builders<BsonDocument>.Filter.Empty;

        //                    filterRoomMapping = filterRoomMapping & Builders<BsonDocument>.Filter.AnyIn("supplierCode", SupplierCodes);
        //                    filterRoomMapping = filterRoomMapping & Builders<BsonDocument>.Filter.AnyIn("SupplierProductId", SupplierProductCodes);
        //                    filterRoomMapping = filterRoomMapping & Builders<BsonDocument>.Filter.Or(Builders<BsonDocument>.Filter.AnyIn("SupplierRoomId", SupplierRoomTypeIds), Builders<BsonDocument>.Filter.AnyIn("SupplierRoomTypeCode", SupplierRoomTypeCodes));

        //                    // including & excluding the fileds from definition result
        //                    ProjectionDefinition<BsonDocument> projectRoomMapping = Builders<BsonDocument>.Projection.Include("supplierCode");
        //                    projectRoomMapping = projectRoomMapping.Exclude("_id");
        //                    projectRoomMapping = projectRoomMapping.Include("SupplierProductId");
        //                    projectRoomMapping = projectRoomMapping.Include("SupplierRoomId");
        //                    projectRoomMapping = projectRoomMapping.Include("SupplierRoomName");
        //                    projectRoomMapping = projectRoomMapping.Include("SupplierRoomTypeCode");
        //                    projectRoomMapping = projectRoomMapping.Include("SystemRoomTypeMapId");
        //                    projectRoomMapping = projectRoomMapping.Include("SystemProductCode");
        //                    projectRoomMapping = projectRoomMapping.Include("SystemRoomTypeName");
        //                    projectRoomMapping = projectRoomMapping.Include("SystemRoomCategory");
        //                    projectRoomMapping = projectRoomMapping.Include("TLGXAccoRoomId");
        //                    projectRoomMapping = projectRoomMapping.Include("TLGXAccoId");

        //                    // filtering the product mapping collection and retreiving filtred result from mongo collection "collectionRoomTypeMapping"
        //                    var searchRoomMappingResult = collectionRoomTypeMapping.Find(filterRoomMapping).Project(projectRoomMapping).ToList();
        //                    searchedRoomMappingData = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<List<HotelRoomTypeMappingModel>>(searchRoomMappingResult.ToJson());

        //                    #region Fetch AccommodationRoomInfoMaster data

        //                    // Generating the filtered definition for collectionAccommodationRoomInfoMaster
        //                    FilterDefinition<BsonDocument> filterAccommodationRoomInfoMaster;
        //                    filterAccommodationRoomInfoMaster = Builders<BsonDocument>.Filter.Empty;
        //                    filterAccommodationRoomInfoMaster = filterAccommodationRoomInfoMaster & Builders<BsonDocument>.Filter.AnyIn("AccomodationRoomInfoCompanyVersions.TLGXAccoId", searchedRoomMappingData.Select(s => s.TLGXAccoId));
        //                    filterAccommodationRoomInfoMaster = filterAccommodationRoomInfoMaster & Builders<BsonDocument>.Filter.AnyIn("AccomodationRoomInfoCompanyVersions.TLGXAccoRoomID", searchedRoomMappingData.Select(s => s.TLGXAccoRoomId));
        //                    filterAccommodationRoomInfoMaster = filterAccommodationRoomInfoMaster & Builders<BsonDocument>.Filter.AnyIn("AccomodationRoomInfoCompanyVersions.CompanyId", TLGXCompanyId);

        //                    // including & excluding the fileds from definition result
        //                    ProjectionDefinition<BsonDocument> projectAccommodationRoomInfoMaster = Builders<BsonDocument>.Projection.Include("CompanyId");
        //                    projectAccommodationRoomInfoMaster = projectAccommodationRoomInfoMaster.Include("_id");
        //                    projectAccommodationRoomInfoMaster = projectAccommodationRoomInfoMaster.Include("CommonHotelId");
        //                    projectAccommodationRoomInfoMaster = projectAccommodationRoomInfoMaster.Include("RoomView");
        //                    projectAccommodationRoomInfoMaster = projectAccommodationRoomInfoMaster.Include("RoomName");
        //                    projectAccommodationRoomInfoMaster = projectAccommodationRoomInfoMaster.Include("Smoking");
        //                    projectAccommodationRoomInfoMaster = projectAccommodationRoomInfoMaster.Include("BathRoomType");
        //                    projectAccommodationRoomInfoMaster = projectAccommodationRoomInfoMaster.Include("BedType");
        //                    projectAccommodationRoomInfoMaster = projectAccommodationRoomInfoMaster.Include("CompanyRoomCategory");
        //                    projectAccommodationRoomInfoMaster = projectAccommodationRoomInfoMaster.Include("RoomCategory");
        //                    projectAccommodationRoomInfoMaster = projectAccommodationRoomInfoMaster.Include("AccomodationRoomInfoCompanyVersions");

        //                    // filtering the product mapping collection and retreiving filtred result from mongo collection "collectionAccommodationRoomInfoMaster"
        //                    var searchAccommodationRoomInfoMasterResult = collectionAccommodationRoomInfoMaster.Find(filterAccommodationRoomInfoMaster).Project(projectAccommodationRoomInfoMaster).ToList();
        //                    searchedAccomodationRoomInfoSearchData = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<List<DC_AccomodationRoomInfo>>(searchAccommodationRoomInfoMasterResult.ToJson());

        //                    #endregion
        //                }

        //                #endregion RoomMappingSearch

        //                #region Build Response
        //                // building the response object
        //                returnResult.SessionId = RQ.SessionId;
        //                var mappingResponseList = new List<CompanySpecificHotelAndRoomType_Response>();

        //                //looping the requested object to create result
        //                foreach (var mappingRequest in RQ.MappingRequests)
        //                {
        //                    var mappingResponse = new CompanySpecificHotelAndRoomType_Response();

        //                    mappingResponse.SequenceNumber = mappingRequest.SequenceNumber;

        //                    mappingResponse.SupplierCode = mappingRequest.SupplierCode;
        //                    mappingResponse.SupplierProductCode = mappingRequest.SupplierProductCode;

        //                    // retreiving the specific records from filtered results for hotel mapping(AccomodationProductMapping)
        //                    var HotelMapping = searchedHotelMappingData.Where(w => w.SupplierCode == mappingRequest.SupplierCode && w.SupplierProductCode == mappingRequest.SupplierProductCode).OrderByDescending(o => o.MapId).Select(s => s).FirstOrDefault();

        //                    if (HotelMapping != null)
        //                    {
        //                        //if required record exists then fetch its accomodationMasterData
        //                        var Acco = searchedAccomodationSearchData.Where(w => w.CommonHotelId == Convert.ToInt32(HotelMapping.SystemProductCode)).FirstOrDefault();

        //                        mappingResponse.ProductCategory = "Accommodation";


        //                        //checking if data exists for field AccomodationCompanyVersions in given accomodationMasterData
        //                        if (Acco != null && Acco.AccomodationCompanyVersions != null)
        //                        {
        //                            // retreiving the company version data based on company id for accomodationMasterData
        //                            var companyVersion = Acco.AccomodationCompanyVersions.Where(x => x.CompanyId == mappingRequest.TLGXCompanyCode).SingleOrDefault();

        //                            // setting fields for response object
        //                            mappingResponse.TlgxCompanyCode = mappingRequest.TLGXCompanyCode;
        //                            mappingResponse.TlgxCommonProductId = Convert.ToString(Acco.CommonHotelId);
        //                            mappingResponse.TlgxCompanyProductId = (companyVersion == null ? string.Empty : companyVersion.TLGXAccoId);
        //                            mappingResponse.ProductName = (companyVersion == null ? string.Empty : companyVersion.ProductName);
        //                            mappingResponse.ProductCategorySubType = (companyVersion == null ? string.Empty : companyVersion.ProductCatSubType);
        //                            mappingResponse.Chain = (companyVersion == null ? string.Empty : companyVersion.Chain);
        //                            mappingResponse.Brand = (companyVersion == null ? string.Empty : companyVersion.Brand);
        //                            mappingResponse.Interests = (companyVersion == null ? Acco.Interests : companyVersion.Interests);
        //                            mappingResponse.Rating = (companyVersion == null ? Acco.HotelStarRating : companyVersion.StarRating);
        //                            mappingResponse.ContainsRoomMappings = (Acco == null ? false : Acco.IsRoomMappingCompleted);
        //                            mappingResponse.DirectContract = (Acco == null ? false : Acco.IsDirectContract);

        //                        }
        //                        else
        //                        {
        //                            // retreiving data for accomodationMasterData
        //                            // setting fields for response object
        //                            mappingResponse.TlgxCompanyCode = mappingRequest.TLGXCompanyCode;
        //                            mappingResponse.TlgxCommonProductId = (Acco == null ? string.Empty : Convert.ToString(Acco.CommonHotelId));
        //                            mappingResponse.TlgxCompanyProductId = (Acco == null ? string.Empty : Acco.TLGXAccoId);
        //                            mappingResponse.ProductName = (Acco == null ? string.Empty : Acco.HotelName);
        //                            mappingResponse.ProductCategorySubType = (Acco == null ? string.Empty : Acco.ProductCategorySubType);
        //                            mappingResponse.Chain = (Acco == null ? string.Empty : Acco.Chain);
        //                            mappingResponse.Brand = (Acco == null ? string.Empty : Acco.Brand);
        //                            mappingResponse.Interests = (Acco == null ? new List<string>() : Acco.Interests);
        //                            mappingResponse.ContainsRoomMappings = (Acco == null ? false : Acco.IsRoomMappingCompleted);
        //                            mappingResponse.Rating = (Acco == null ? string.Empty : Acco.HotelStarRating);
        //                            mappingResponse.ContainsRoomMappings = (Acco == null ? false : Acco.IsRoomMappingCompleted);
        //                            mappingResponse.DirectContract = (Acco == null ? false : Acco.IsDirectContract);


        //                        }
        //                    }

        //                    //if required record exists then fetch its searchedRoomMappingData (SupplierRoomTypeMapping Data)
        //                    var RoomMappings = searchedRoomMappingData.Where(w => w.supplierCode == mappingRequest.SupplierCode && w.SupplierProductId == mappingRequest.SupplierProductCode).Select(s => s).ToList();

        //                    var RoomMappingResponseList = new List<CompanySpecificHotelAndRoomTypeMapping_RoomTypeResponse>();


        //                    if (RoomMappings != null && RoomMappings.Count > 0)
        //                    {
        //                        //if required record exists then fetch its searchedAccomodationRoomInfoSearchData (AccomodationAdminRoomInfo)
        //                        var adminRoomInfo = searchedAccomodationRoomInfoSearchData.Where(w => w.CommonHotelId == Convert.ToInt32(HotelMapping.SystemProductCode)).FirstOrDefault(); ;

        //                        //looping the requested object to create result for Room data
        //                        foreach (var mappingRoomRequest in mappingRequest.SupplierRoomTypes)
        //                        {

        //                            var RoomMappingResponse = new CompanySpecificHotelAndRoomTypeMapping_RoomTypeResponse();
        //                            //RoomMappingResponse.SupplierRoomCategory = mappingRoomRequest.SupplierRoomCategory;
        //                            //RoomMappingResponse.SupplierRoomCategoryId = mappingRoomRequest.SupplierRoomCategoryId;
        //                            //RoomMappingResponse.SupplierRoomId = mappingRoomRequest.SupplierRoomId;
        //                            //RoomMappingResponse.SupplierRoomName = mappingRoomRequest.SupplierRoomName;
        //                            //RoomMappingResponse.SupplierRoomTypeCode = mappingRoomRequest.SupplierRoomTypeCode;



        //                            var room = RoomMappings.Where(w => w.SupplierRoomTypeCode == mappingRoomRequest.SupplierRoomTypeCode).FirstOrDefault();
        //                            if (room != null)
        //                            {
        //                                RoomMappingResponse.SupplierRoomCategory = room.SupplierRoomCategory;
        //                                RoomMappingResponse.SupplierRoomCategoryId = room.SupplierRoomCategoryId;
        //                                RoomMappingResponse.SupplierRoomId = room.SupplierRoomId;
        //                                RoomMappingResponse.SupplierRoomName = room.SupplierRoomName;
        //                                RoomMappingResponse.SupplierRoomTypeCode = room.SupplierRoomTypeCode;
        //                            }


        //                            //checking if data exists for field AccomodationRoomInfoCompanyVersions in given searchedAccomodationRoomInfoSearchData
        //                            if (adminRoomInfo != null && adminRoomInfo.AccomodationRoomInfoCompanyVersions != null)
        //                            {

        //                                //if SupplierRoomTypeCode data is exists in request
        //                                if (string.IsNullOrWhiteSpace(mappingRoomRequest.SupplierRoomId) && !string.IsNullOrWhiteSpace(mappingRoomRequest.SupplierRoomTypeCode))
        //                                {



        //                                    RoomMappingResponse.MappedRooms = RoomMappings.Where(w => w.SupplierRoomTypeCode == mappingRoomRequest.SupplierRoomTypeCode)
        //                                      .Select(s => new CompanySpecificHotelAndRoomTypeMapping_MappedRoomType
        //                                      {
        //                                          TlgxCompanyRoomId = s.TLGXAccoRoomId,
        //                                          CompanyRoomName = (adminRoomInfo.AccomodationRoomInfoCompanyVersions.Where(x => x.TLGXAccoId == s.TLGXAccoId && x.TLGXAccoRoomID == s.TLGXAccoRoomId && x.CompanyId == mappingRequest.TLGXCompanyCode).FirstOrDefault().RoomName),
        //                                          CompanyRoomCategory = (adminRoomInfo.AccomodationRoomInfoCompanyVersions.Where(x => x.TLGXAccoId == s.TLGXAccoId && x.TLGXAccoRoomID == s.TLGXAccoRoomId && x.CompanyId == mappingRequest.TLGXCompanyCode).FirstOrDefault().CompanyRoomCategory),
        //                                          NakshatraRoomMappingId = s.SystemRoomTypeMapId
        //                                      }).ToList();

        //                                    RoomMappingResponse.MappedRooms = RoomMappings.Where(w => w.SupplierRoomTypeCode == mappingRoomRequest.SupplierRoomTypeCode)
        //                                        .Select(s => new CompanySpecificHotelAndRoomTypeMapping_MappedRoomType
        //                                        {
        //                                            TlgxCompanyRoomId = s.TLGXAccoRoomId,
        //                                            CompanyRoomName = (adminRoomInfo.AccomodationRoomInfoCompanyVersions.Where(x => x.TLGXAccoId == s.TLGXAccoId && x.TLGXAccoRoomID == s.TLGXAccoRoomId && x.CompanyId == mappingRequest.TLGXCompanyCode).FirstOrDefault().RoomName),
        //                                            CompanyRoomCategory = (adminRoomInfo.AccomodationRoomInfoCompanyVersions.Where(x => x.TLGXAccoId == s.TLGXAccoId && x.TLGXAccoRoomID == s.TLGXAccoRoomId && x.CompanyId == mappingRequest.TLGXCompanyCode).FirstOrDefault().CompanyRoomCategory),
        //                                            NakshatraRoomMappingId = s.SystemRoomTypeMapId
        //                                        }).ToList();
        //                                }
        //                                //if SupplierRoomId data is exists in request
        //                                else if (!string.IsNullOrWhiteSpace(mappingRoomRequest.SupplierRoomId) && string.IsNullOrWhiteSpace(mappingRoomRequest.SupplierRoomTypeCode))
        //                                {
        //                                    RoomMappingResponse.MappedRooms = RoomMappings.Where(w => w.SupplierRoomId == mappingRoomRequest.SupplierRoomId).Select(s => new CompanySpecificHotelAndRoomTypeMapping_MappedRoomType
        //                                    {
        //                                        TlgxCompanyRoomId = s.TLGXAccoRoomId,
        //                                        CompanyRoomName = (adminRoomInfo.AccomodationRoomInfoCompanyVersions.Where(x => x.TLGXAccoId == s.TLGXAccoId && x.TLGXAccoRoomID == s.TLGXAccoRoomId && x.CompanyId == mappingRequest.TLGXCompanyCode).FirstOrDefault().RoomName),
        //                                        CompanyRoomCategory = (adminRoomInfo.AccomodationRoomInfoCompanyVersions.Where(x => x.TLGXAccoId == s.TLGXAccoId && x.TLGXAccoRoomID == s.TLGXAccoRoomId && x.CompanyId == mappingRequest.TLGXCompanyCode).FirstOrDefault().CompanyRoomCategory),
        //                                        NakshatraRoomMappingId = s.SystemRoomTypeMapId
        //                                    }).ToList();
        //                                }
        //                                //if SupplierRoomId and SupplierRoomTypeCode data is exists in request
        //                                else if (!string.IsNullOrWhiteSpace(mappingRoomRequest.SupplierRoomId) && !string.IsNullOrWhiteSpace(mappingRoomRequest.SupplierRoomTypeCode))
        //                                {
        //                                    RoomMappingResponse.MappedRooms = RoomMappings.Where(w => w.SupplierRoomId == mappingRoomRequest.SupplierRoomId && w.SupplierRoomTypeCode == mappingRoomRequest.SupplierRoomTypeCode).Select(s => new CompanySpecificHotelAndRoomTypeMapping_MappedRoomType
        //                                    {
        //                                        TlgxCompanyRoomId = s.TLGXAccoRoomId,
        //                                        CompanyRoomName = (adminRoomInfo.AccomodationRoomInfoCompanyVersions.Where(x => x.TLGXAccoId == s.TLGXAccoId && x.TLGXAccoRoomID == s.TLGXAccoRoomId && x.CompanyId == mappingRequest.TLGXCompanyCode).FirstOrDefault().RoomName),
        //                                        CompanyRoomCategory = (adminRoomInfo.AccomodationRoomInfoCompanyVersions.Where(x => x.TLGXAccoId == s.TLGXAccoId && x.TLGXAccoRoomID == s.TLGXAccoRoomId && x.CompanyId == mappingRequest.TLGXCompanyCode).FirstOrDefault().CompanyRoomCategory),
        //                                        NakshatraRoomMappingId = s.SystemRoomTypeMapId
        //                                    }).ToList();
        //                                }
        //                                else
        //                                {
        //                                    RoomMappingResponse.MappedRooms = new List<CompanySpecificHotelAndRoomTypeMapping_MappedRoomType>();
        //                                }
        //                            }
        //                            else
        //                            {
        //                                //checking if data bot exists for field AccomodationRoomInfoCompanyVersions then it will retrive data from adminRoomInfo
        //                                //var room = RoomMappings.Where(w => w.SupplierRoomTypeCode == mappingRoomRequest.SupplierRoomTypeCode).FirstOrDefault();

        //                                //if SupplierRoomTypeCode data is exists in request
        //                                if (string.IsNullOrWhiteSpace(mappingRoomRequest.SupplierRoomId) && !string.IsNullOrWhiteSpace(mappingRoomRequest.SupplierRoomTypeCode))
        //                                {

        //                                    if (room != null)
        //                                    {
        //                                        RoomMappingResponse.SupplierRoomCategory = room.SupplierRoomCategory;
        //                                        RoomMappingResponse.SupplierRoomCategoryId = room.SupplierRoomCategoryId;
        //                                        RoomMappingResponse.SupplierRoomId = room.SupplierRoomId;
        //                                        RoomMappingResponse.SupplierRoomName = room.SupplierRoomName;
        //                                        RoomMappingResponse.SupplierRoomTypeCode = room.SupplierRoomTypeCode;
        //                                    }


        //                                    RoomMappingResponse.MappedRooms = RoomMappings.Where(w => w.SupplierRoomTypeCode == mappingRoomRequest.SupplierRoomTypeCode)
        //                                      .Select(s => new CompanySpecificHotelAndRoomTypeMapping_MappedRoomType
        //                                      {
        //                                          TlgxCompanyRoomId = s.TLGXAccoRoomId,
        //                                          CompanyRoomName = adminRoomInfo != null ? adminRoomInfo.RoomName : null,
        //                                          CompanyRoomCategory = adminRoomInfo != null ? adminRoomInfo.CompanyRoomCategory : null,
        //                                          NakshatraRoomMappingId = s.SystemRoomTypeMapId
        //                                      }).ToList();

        //                                    RoomMappingResponse.MappedRooms = RoomMappings.Where(w => w.SupplierRoomTypeCode == mappingRoomRequest.SupplierRoomTypeCode)
        //                                        .Select(s => new CompanySpecificHotelAndRoomTypeMapping_MappedRoomType
        //                                        {
        //                                            TlgxCompanyRoomId = s.TLGXAccoRoomId,
        //                                            CompanyRoomName = adminRoomInfo != null ? adminRoomInfo.RoomName : null,
        //                                            CompanyRoomCategory = adminRoomInfo != null ? adminRoomInfo.CompanyRoomCategory : null,
        //                                            NakshatraRoomMappingId = s.SystemRoomTypeMapId
        //                                        }).ToList();
        //                                }
        //                                //if SupplierRoomId data is exists in request
        //                                else if (!string.IsNullOrWhiteSpace(mappingRoomRequest.SupplierRoomId) && string.IsNullOrWhiteSpace(mappingRoomRequest.SupplierRoomTypeCode))
        //                                {
        //                                    RoomMappingResponse.MappedRooms = RoomMappings.Where(w => w.SupplierRoomId == mappingRoomRequest.SupplierRoomId).Select(s => new CompanySpecificHotelAndRoomTypeMapping_MappedRoomType
        //                                    {
        //                                        TlgxCompanyRoomId = s.TLGXAccoRoomId,
        //                                        CompanyRoomName = adminRoomInfo != null ? adminRoomInfo.RoomName : null,
        //                                        CompanyRoomCategory = adminRoomInfo != null ? adminRoomInfo.CompanyRoomCategory : null,
        //                                        NakshatraRoomMappingId = s.SystemRoomTypeMapId
        //                                    }).ToList();
        //                                }
        //                                //if SupplierRoomId and SupplierRoomTypeCode data is exists in request
        //                                else if (!string.IsNullOrWhiteSpace(mappingRoomRequest.SupplierRoomId) && !string.IsNullOrWhiteSpace(mappingRoomRequest.SupplierRoomTypeCode))
        //                                {
        //                                    RoomMappingResponse.MappedRooms = RoomMappings.Where(w => w.SupplierRoomId == mappingRoomRequest.SupplierRoomId && w.SupplierRoomTypeCode == mappingRoomRequest.SupplierRoomTypeCode).Select(s => new CompanySpecificHotelAndRoomTypeMapping_MappedRoomType
        //                                    {
        //                                        TlgxCompanyRoomId = s.TLGXAccoRoomId,
        //                                        CompanyRoomName = (adminRoomInfo.AccomodationRoomInfoCompanyVersions.Where(x => x.TLGXAccoId == s.TLGXAccoId && x.TLGXAccoRoomID == s.TLGXAccoRoomId && x.CompanyId == mappingRequest.TLGXCompanyCode).FirstOrDefault().RoomName),
        //                                        CompanyRoomCategory = (adminRoomInfo.AccomodationRoomInfoCompanyVersions.Where(x => x.TLGXAccoId == s.TLGXAccoId && x.TLGXAccoRoomID == s.TLGXAccoRoomId && x.CompanyId == mappingRequest.TLGXCompanyCode).FirstOrDefault().CompanyRoomCategory),
        //                                        NakshatraRoomMappingId = s.SystemRoomTypeMapId
        //                                    }).ToList();
        //                                }
        //                                else
        //                                {
        //                                    RoomMappingResponse.MappedRooms = new List<CompanySpecificHotelAndRoomTypeMapping_MappedRoomType>();
        //                                }
        //                            }

        //                            //Adding created response into RoomMappingResponseList
        //                            RoomMappingResponseList.Add(RoomMappingResponse);

        //                            #region Roomtype Mapping Online data insert into Mongo
        //                            if (RoomMappingResponse.MappedRooms != null && RoomMappingResponse.MappedRooms.Count == 0)
        //                            {
        //                                int? SystemProductCode = null;
        //                                //if (int.TryParse(mappingResponse.CommonHotelId, out int CommonHotelId))
        //                                //{
        //                                //    SystemProductCode = CommonHotelId;
        //                                //}

        //                                #region Insert into RoomType Mapping Online collection if mappings not found
        //                                RoomTypeMappingController _obj = new RoomTypeMappingController();
        //                                // Fire & Forget 
        //                                await Task.Run(() => _obj.InsertRoomTypeMappingOnline(collection_rto, new RoomTypeMappingOnline
        //                                {
        //                                    //_id = ObjectId.GenerateNewId(),

        //                                    CreateDateTime = DateTime.Now,

        //                                    Mode = "Online", //RQ.Mode,

        //                                    SupplierId = mappingRequest.SupplierCode,
        //                                    SupplierProductId = mappingRequest.SupplierProductCode,
        //                                    SupplierRoomCategory = mappingRoomRequest.SupplierRoomCategory,
        //                                    SupplierRoomCategoryId = mappingRoomRequest.SupplierRoomCategoryId,
        //                                    SupplierRoomId = mappingRoomRequest.SupplierRoomId,
        //                                    SupplierRoomName = mappingRoomRequest.SupplierRoomName,
        //                                    SupplierRoomTypeCode = mappingRoomRequest.SupplierRoomTypeCode,
        //                                    // TLGXCommonHotelId = mappingResponse.TlgxAccoId,
        //                                    SystemProductCode = SystemProductCode
        //                                }));
        //                                #endregion

        //                            }
        //                            #endregion
        //                        }

        //                        mappingResponse.SupplierRoomTypes = RoomMappingResponseList;

        //                    }
        //                    else
        //                    {
        //                        foreach (var mappingRoomRequest in mappingRequest.SupplierRoomTypes)
        //                        {

        //                            var RoomMappingResponse = new CompanySpecificHotelAndRoomTypeMapping_RoomTypeResponse();
        //                            RoomMappingResponse.SupplierRoomCategory = mappingRoomRequest.SupplierRoomCategory;
        //                            RoomMappingResponse.SupplierRoomCategoryId = mappingRoomRequest.SupplierRoomCategoryId;
        //                            RoomMappingResponse.SupplierRoomId = mappingRoomRequest.SupplierRoomId;
        //                            RoomMappingResponse.SupplierRoomName = mappingRoomRequest.SupplierRoomName;
        //                            RoomMappingResponse.SupplierRoomTypeCode = mappingRoomRequest.SupplierRoomTypeCode;
        //                            RoomMappingResponse.MappedRooms = new List<CompanySpecificHotelAndRoomTypeMapping_MappedRoomType>();
        //                            //Adding created response into RoomMappingResponseList
        //                            RoomMappingResponseList.Add(RoomMappingResponse);

        //                        }

        //                        mappingResponse.SupplierRoomTypes = RoomMappingResponseList;
        //                    }

        //                    mappingResponseList.Add(mappingResponse);

        //                }
        //                returnResult.MappingResponses = mappingResponseList;

        //                #endregion Build Response

        //            }
        //        }

        //        HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, returnResult);
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        NLogHelper.Nlogger_LogError.LogError(ex, this.GetType().FullName, Request.GetActionDescriptor().ActionName, Request.RequestUri.PathAndQuery);
        //        HttpResponseMessage response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Server Error. Contact Admin. Error Date : " + DateTime.Now.ToString());
        //        return response;
        //    }
        //}
        #endregion


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
                var returnResult = new CompanySpecificHotelAndRoomType_RS();

                string message = "";
                if (!ValidateCompanySpecificHotelAndRoomTypeMappingRQ(RQ, ref message))
                {
                    HttpResponseMessage response1 = Request.CreateErrorResponse(HttpStatusCode.BadRequest, message);
                    return response1;

                }

                if (RQ != null)
                {

                    returnResult.SessionId = RQ.SessionId;

                    if (RQ.MappingRequests != null)
                    {

                        #region Variable Declaration and Initialization

                        // declaring instance of Mongo db                        
                        _database = MongoDBHandler.mDatabase();
                        // declaring objects of Mongo collection
                        IMongoCollection<BsonDocument> collectionCompanyAccommodationProductMapping = _database.GetCollection<BsonDocument>("CompanyAccommodationProductMapping");

                        IMongoCollection<BsonDocument> collectionRoomTypeMapping = _database.GetCollection<BsonDocument>("RoomTypeMapping");
                        IMongoCollection<RoomTypeMappingOnline> collection_rto = _database.GetCollection<RoomTypeMappingOnline>("RoomTypeMappingOnline");
                        IMongoCollection<BsonDocument> collectionProductMapping = _database.GetCollection<BsonDocument>("ProductMappingLite");
                        IMongoCollection<BsonDocument> collectionAccommodationMaster = _database.GetCollection<BsonDocument>("AccommodationMaster");
                        IMongoCollection<BsonDocument> collectionAccommodationRoomInfoMaster = _database.GetCollection<BsonDocument>("AccommodationRoomInfoMaster");

                        // declaring list for mongo entities
                        List<DC_ConpanyAccommodationMapping> searchedConpanyAccommodationMapping = new List<DC_ConpanyAccommodationMapping>();



                        // setting request fields to variables
                        var SupplierCodes = RQ.MappingRequests.Where(w => w.SupplierCode != null).Select(x => x.SupplierCode.ToUpper()).Distinct().ToArray();
                        var SupplierProductCodes = RQ.MappingRequests.Where(w => w.SupplierProductCode != null).Select(x => x.SupplierProductCode.ToUpper()).Distinct().ToArray();



                        var SupplierRoomTypeIds = RQ.MappingRequests.SelectMany(p => p.SupplierRoomTypes.Where(w => w.SupplierRoomId != null).Select(s => s.SupplierRoomId.ToUpper())).Distinct().ToArray();
                        var SupplierRoomTypeCodes = RQ.MappingRequests.SelectMany(p => p.SupplierRoomTypes.Where(w => w.SupplierRoomTypeCode != null).Select(s => s.SupplierRoomTypeCode.ToUpper())).Distinct().ToArray();
                        var TLGXCompanyId = RQ.MappingRequests.Where(w => w.TLGXCompanyCode != null).Select(x => x.TLGXCompanyCode.ToUpper()).Distinct().ToArray();

                        #endregion Variable Declaration and Initialization

                        #region Filtering CompanyAccommodationProductMapping
                        FilterDefinition<BsonDocument> filterCompanyAccommodationProductMapping;
                        filterCompanyAccommodationProductMapping = Builders<BsonDocument>.Filter.Empty;
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
                        projectCompanyAccommodationProductMapping = projectCompanyAccommodationProductMapping.Include("TLGXProduct_Id");
                        projectCompanyAccommodationProductMapping = projectCompanyAccommodationProductMapping.Include("MappedRooms");



                        var searchHotelMappingResult1 = collectionCompanyAccommodationProductMapping.Find(filterCompanyAccommodationProductMapping).Project(projectCompanyAccommodationProductMapping).ToList();
                        searchedConpanyAccommodationMapping = JsonConvert.DeserializeObject<List<DC_ConpanyAccommodationMapping>>(searchHotelMappingResult1.ToJson());

                        #endregion


                        #region Build Response
                        // building the response object
                        returnResult.SessionId = RQ.SessionId;
                        var mappingResponseList = new List<CompanySpecificHotelAndRoomType_Response>();
                        var writeModelDetails = new List<WriteModel<RoomTypeMappingOnline>>();
                        //looping the requested object to create result
                        foreach (var mappingRequest in RQ.MappingRequests)
                        {
                            var mappingResponse = new CompanySpecificHotelAndRoomType_Response();

                            mappingResponse.SequenceNumber = mappingRequest.SequenceNumber;

                            mappingResponse.SupplierCode = mappingRequest.SupplierCode;
                            mappingResponse.SupplierProductCode = mappingRequest.SupplierProductCode;

                            // retreiving the specific records from filtered results for hotel mapping(AccomodationProductMapping)
                            var HotelMapping = searchedConpanyAccommodationMapping.Where(w => w.SupplierCode == mappingRequest.SupplierCode && w.SupplierProductCode == mappingRequest.SupplierProductCode && w.TLGXCompanyId == mappingRequest.TLGXCompanyCode).Select(s => s).FirstOrDefault();

                            if (HotelMapping != null)
                            {
                                //if required record exists then fetch its accomodationMasterData
                                //var Acco = searchedAccomodationSearchData.Where(w => w.CommonHotelId == Convert.ToInt32(HotelMapping.SystemProductCode)).FirstOrDefault();

                                mappingResponse.ProductCategory = "Accommodation";
                                //mappingResponse.NakshatraMappingId = HotelMapping.MapId;

                                // setting fields for response object
                                mappingResponse.TlgxCompanyCode = mappingRequest.TLGXCompanyCode;
                                mappingResponse.SupplierProductName = HotelMapping.SupplierProductName;
                                mappingResponse.TlgxCommonProductId = Convert.ToString(HotelMapping.CommonProductId);
                                mappingResponse.TlgxCompanyProductId = HotelMapping.CompanyProductId;
                                mappingResponse.ProductName = HotelMapping.CompanyProductName;
                                mappingResponse.ProductCategorySubType = HotelMapping.ProductCategorySubType;
                                mappingResponse.Chain = HotelMapping.Chain;
                                mappingResponse.Brand = HotelMapping.Brand;
                                mappingResponse.TlgxProduct_Id = HotelMapping.TLGXProduct_Id;

                                List<string> lstInterest;
                                if (!string.IsNullOrEmpty(HotelMapping.Interest))
                                {
                                    lstInterest = HotelMapping.Interest.Split(',').ToList();
                                }
                                else
                                {
                                    lstInterest = new List<string>();
                                }

                                mappingResponse.Interests = lstInterest;
                                mappingResponse.Rating = HotelMapping.Rating;

                                //if required record exists then fetch its searchedRoomMappingData (SupplierRoomTypeMapping Data)
                                var RoomMappings = HotelMapping.MappedRooms;

                                var RoomMappingResponseList = new List<CompanySpecificHotelAndRoomTypeMapping_RoomTypeResponse>();

                                //Check if Hotel contain Room Data
                                if (RoomMappings != null && RoomMappings.Count > 0)
                                {

                                    //looping the requested object to create result for Room data
                                    foreach (var mappingRoomRequest in mappingRequest.SupplierRoomTypes)
                                    {

                                        DC_ConpanyAccommodationRoomMapping room = null;

                                        var rooms = RoomMappings;

                                        //Check the Room data is available in RT collection of object.
                                        rooms = RoomData(mappingRoomRequest, rooms, "SupplierRoomTypeCode");

                                        //Group the Supplier Room data to consolidate Mapped Room data.
                                        var dt = rooms.GroupBy(x =>
                                        new
                                        {
                                            x.Accommodation_CompanyVersion_Id,
                                            x.SupplierProductId,
                                            x.SupplierRoomCategory,
                                            x.SupplierRoomCategoryId,
                                            x.SupplierRoomId,
                                            x.SupplierRoomName,
                                            x.SupplierRoomTypeCode
                                        }).Select(gc => new CompanySpecificHotelAndRoomTypeMapping_RoomTypeResponse
                                        {
                                            SupplierRoomTypeCode = gc.Key.SupplierRoomTypeCode,
                                            SupplierRoomId = gc.Key.SupplierRoomId,
                                            SupplierRoomName = gc.Key.SupplierRoomName,
                                            SupplierRoomCategoryId = gc.Key.SupplierRoomCategoryId,
                                            SupplierRoomCategory = gc.Key.SupplierRoomCategory,
                                            MappedRooms = gc.Select(x => new CompanySpecificHotelAndRoomTypeMapping_MappedRoomType
                                            {
                                                TlgxCompanyRoomId = x.CompanyRoomId,
                                                TLGXCommonRoomId = x.TLGXCommonRoomId,
                                                CompanyRoomName = x.CompanyRoomName,
                                                CompanyRoomCategory = x.CompanyRoomCategory,
                                                NakshatraRoomMappingId = Convert.ToInt64(x.NakshatraRoomMappingId),

                                            }).ToList()
                                        });
                                        //Group the Supplier Room data to consolidate Mapped Room data.

                                        //Looping the to generate RS
                                        if (dt != null && dt.Any())
                                        {

                                            foreach (var room1 in dt)
                                            {
                                                var RoomMappingResponse = room1;
                                                RoomMappingResponse.MappedRooms = room1.MappedRooms;
                                                RoomMappingResponseList.Add(RoomMappingResponse);
                                            }
                                        }
                                        else
                                        {
                                            mappingResponse.TlgxCompanyCode = mappingRequest.TLGXCompanyCode;
                                            GetRoomTypeNotFoundRS(collection_rto, writeModelDetails, mappingRequest, mappingResponse, ref RoomMappingResponseList, mappingRoomRequest);
                                            #region Included into new Method
                                            //if (!string.IsNullOrEmpty(mappingRoomRequest.SupplierRoomTypeCode)
                                            //    || !string.IsNullOrEmpty(mappingRoomRequest.SupplierRoomName)
                                            //    || !string.IsNullOrEmpty(mappingRoomRequest.SupplierRoomId)
                                            //    || !string.IsNullOrEmpty(mappingRoomRequest.SupplierRoomCategoryId)
                                            //    || !string.IsNullOrEmpty(mappingRoomRequest.SupplierRoomCategory))
                                            //{
                                            //    var RoomMappingResponse = new CompanySpecificHotelAndRoomTypeMapping_RoomTypeResponse();
                                            //    RoomMappingResponse.SupplierRoomCategory = mappingRoomRequest.SupplierRoomCategory;
                                            //    RoomMappingResponse.SupplierRoomCategoryId = mappingRoomRequest.SupplierRoomCategoryId;
                                            //    RoomMappingResponse.SupplierRoomId = mappingRoomRequest.SupplierRoomId;
                                            //    RoomMappingResponse.SupplierRoomName = mappingRoomRequest.SupplierRoomName;
                                            //    RoomMappingResponse.SupplierRoomTypeCode = mappingRoomRequest.SupplierRoomTypeCode;
                                            //    RoomMappingResponse.MappedRooms =  new List<CompanySpecificHotelAndRoomTypeMapping_MappedRoomType>(); 
                                            //    #region Roomtype Mapping Online data insert into Mongo
                                            //    if (RoomMappingResponse.MappedRooms.Count == 0)
                                            //    {
                                            //        int? SystemProductCode = null;
                                            //        if (int.TryParse(mappingResponse.TlgxCommonProductId, out int CommonHotelId))
                                            //        {
                                            //            SystemProductCode = CommonHotelId;
                                            //        }

                                            //        #region Insert into RoomType Mapping Online collection if mappings not found

                                            //        writeModelDetails.Add(InsertRoomTypeMappingOnline(collection_rto, new RoomTypeMappingOnline
                                            //        {
                                            //            //_id = ObjectId.GenerateNewId(),
                                            //            CreateDateTime = DateTime.Now,

                                            //            Mode = "Online", //RQ.Mode,

                                            //            SupplierId = mappingRequest.SupplierCode,
                                            //            SupplierProductId = mappingRequest.SupplierProductCode,
                                            //            SupplierRoomCategory = mappingRoomRequest.SupplierRoomCategory,
                                            //            SupplierRoomCategoryId = mappingRoomRequest.SupplierRoomCategoryId,
                                            //            SupplierRoomId = mappingRoomRequest.SupplierRoomId,
                                            //            SupplierRoomName = mappingRoomRequest.SupplierRoomName,
                                            //            SupplierRoomTypeCode = mappingRoomRequest.SupplierRoomTypeCode,
                                            //            TLGXCommonHotelId = mappingResponse.TlgxCompanyProductId,
                                            //            SystemProductCode = SystemProductCode
                                            //        }));

                                            //        #endregion

                                            //    }
                                            //    #endregion


                                            //    RoomMappingResponseList.Add(RoomMappingResponse);
                                            //}
                                            #endregion

                                        }

                                    }
                                }
                                else
                                {
                                    foreach (var mappingRoomRequest in mappingRequest.SupplierRoomTypes)
                                    {
                                        GetRoomTypeNotFoundRS(collection_rto, writeModelDetails, mappingRequest, mappingResponse, ref RoomMappingResponseList, mappingRoomRequest);
                                    }
                                }

                                mappingResponse.SupplierRoomTypes = RoomMappingResponseList;
                                mappingResponseList.Add(mappingResponse);


                            }
                            else
                            {
                                mappingResponse.TlgxCompanyCode = mappingRequest.TLGXCompanyCode;
                                var RoomMappingResponseList = new List<CompanySpecificHotelAndRoomTypeMapping_RoomTypeResponse>();
                                foreach (var mappingRoomRequest in mappingRequest.SupplierRoomTypes)
                                {
                                    GetRoomTypeNotFoundRS(collection_rto, writeModelDetails, mappingRequest, mappingResponse, ref RoomMappingResponseList, mappingRoomRequest);

                                }

                                mappingResponseList.Add(mappingResponse);
                                mappingResponse.SupplierRoomTypes = RoomMappingResponseList;
                            }


                        }
                        returnResult.MappingResponses = mappingResponseList;

                        // getting code of supplier in string variable.
                        string strSupplierCode = Convert.ToString(SupplierCodes[0]);

                        // Making object of Supplier MCON data values from Supplier Document
                        IMongoCollection<BsonDocument> collectionSupplierMaster = _database.GetCollection<BsonDocument>("Supplier");
                        ProjectionDefinition<BsonDocument> SupplierField = Builders<BsonDocument>.Projection.Include("SupplierCode");
                        SupplierField = SupplierField.Exclude("_id");
                        var builder = Builders<BsonDocument>.Filter;

                        //query to only bring data which is required and checking if related Supplier have MCON flag as YES or not and storing result in a variable.
                        // This query only checks the attribute "MCON.HoldInsertOnlineRoomTypeMappingData" from Supplier document in MCON node.
                        var query = builder.Eq("MCON.HoldInsertOnlineRoomTypeMappingData", "YES") & builder.Eq("SupplierCode", strSupplierCode);
                        var filteredSupplierList = await collectionSupplierMaster.Find(query).Project(SupplierField).ToListAsync();

                        // checking if variable "filteredSupplierList" is having some supplier and SupplierCode is not empty.
                        if (filteredSupplierList.Count > 0 || strSupplierCode == "")
                        {
                            // if count is greater than Zero, means this it should not be allowed to get written in database.
                            // If SupplierCode is coming empty due to some data issues, it should not be allowed to get written in database.
                        }
                        else
                        {
                            if (writeModelDetails.Any())
                            {
                                Task.Run(() => { collection_rto.BulkWrite(writeModelDetails); });
                            }
                        }

                        #endregion Build Response

                    }
                }
                else
                {

                    HttpResponseMessage response1 = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid Request");
                    return response1;
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

        //if records are not found for RT then it will insert into RoomTypeOnline collection
        private void GetRoomTypeNotFoundRS(IMongoCollection<RoomTypeMappingOnline> collection_rto, List<WriteModel<RoomTypeMappingOnline>> writeModelDetails, CompanySpecificHotelAndRoomType_Request mappingRequest, CompanySpecificHotelAndRoomType_Response mappingResponse, ref List<CompanySpecificHotelAndRoomTypeMapping_RoomTypeResponse> RoomMappingResponseList, CompanySpecificHotelAndRoomType_RoomTypeRequest mappingRoomRequest)
        {
            if (!string.IsNullOrEmpty(mappingRoomRequest.SupplierRoomTypeCode)
                || !string.IsNullOrEmpty(mappingRoomRequest.SupplierRoomName)
                || !string.IsNullOrEmpty(mappingRoomRequest.SupplierRoomId)
                || !string.IsNullOrEmpty(mappingRoomRequest.SupplierRoomCategoryId)
                || !string.IsNullOrEmpty(mappingRoomRequest.SupplierRoomCategory))
            {
                var RoomMappingResponse = new CompanySpecificHotelAndRoomTypeMapping_RoomTypeResponse();
                RoomMappingResponse.SupplierRoomCategory = mappingRoomRequest.SupplierRoomCategory;
                RoomMappingResponse.SupplierRoomCategoryId = mappingRoomRequest.SupplierRoomCategoryId;
                RoomMappingResponse.SupplierRoomId = mappingRoomRequest.SupplierRoomId;
                RoomMappingResponse.SupplierRoomName = mappingRoomRequest.SupplierRoomName;
                RoomMappingResponse.SupplierRoomTypeCode = mappingRoomRequest.SupplierRoomTypeCode;
                RoomMappingResponse.MappedRooms = new List<CompanySpecificHotelAndRoomTypeMapping_MappedRoomType>();
                #region Roomtype Mapping Online data insert into Mongo
                if (RoomMappingResponse.MappedRooms.Count == 0)
                {
                    int? SystemProductCode = null;
                    if (int.TryParse(mappingResponse.TlgxCommonProductId, out int CommonHotelId))
                    {
                        SystemProductCode = CommonHotelId;
                    }

                    #region Insert into RoomType Mapping Online collection if mappings not found

                    writeModelDetails.Add(InsertRoomTypeMappingOnlineCompanySpecificRequest(collection_rto, new RoomTypeMappingOnline
                    {
                        //_id = ObjectId.GenerateNewId(),
                        CreateDateTime = DateTime.Now,

                        Mode = "Online", //RQ.Mode,

                        SupplierId = mappingRequest.SupplierCode,
                        SupplierProductId = mappingRequest.SupplierProductCode,
                        TLGXCommonHotelId = mappingResponse.TlgxCompanyProductId,
                        SystemProductCode = SystemProductCode,
                        SupplierRoomCategory = mappingRoomRequest.SupplierRoomCategory,
                        SupplierRoomCategoryId = mappingRoomRequest.SupplierRoomCategoryId,
                        SupplierRoomId = mappingRoomRequest.SupplierRoomId,
                        SupplierRoomName = mappingRoomRequest.SupplierRoomName,
                        SupplierRoomTypeCode = mappingRoomRequest.SupplierRoomTypeCode
                    }));

                    #endregion

                }
                #endregion


                RoomMappingResponseList.Add(RoomMappingResponse);
            }
        }

        //Generated recorsive function to check field sequentially
        private List<DC_ConpanyAccommodationRoomMapping> RoomData(CompanySpecificHotelAndRoomType_RoomTypeRequest mappingRoomRequest, List<DC_ConpanyAccommodationRoomMapping> RoomMappings, string FieldName)
        {

            var rooms = RoomMappings;
            switch (FieldName)
            {
                case "SupplierRoomTypeCode":
                    if (!string.IsNullOrEmpty(mappingRoomRequest.SupplierRoomTypeCode))
                    {
                        rooms = RoomMappings.Where(w => w.SupplierRoomTypeCode == mappingRoomRequest.SupplierRoomTypeCode).ToList();

                        //if record not found with 1st condition then go for another check.
                        if (rooms.Count == 0)
                        {
                            rooms = RoomData(mappingRoomRequest, RoomMappings, "SupplierRoomId");
                        }

                    }
                    else
                    {
                        goto case "SupplierRoomId";
                    }

                    break;
                case "SupplierRoomId":
                    if (!string.IsNullOrEmpty(mappingRoomRequest.SupplierRoomId))
                    {
                        rooms = RoomMappings.Where(w => w.SupplierRoomId == mappingRoomRequest.SupplierRoomId).ToList();
                        if (rooms.Count == 0)
                        {
                            rooms = RoomData(mappingRoomRequest, RoomMappings, "SupplierRoomName");
                        }
                    }
                    else
                    {
                        goto case "SupplierRoomName";
                    }
                    break;
                case "SupplierRoomName":
                    if (!string.IsNullOrEmpty(mappingRoomRequest.SupplierRoomName))
                    {
                        rooms = RoomMappings.Where(w => w.SupplierRoomName == mappingRoomRequest.SupplierRoomName).ToList();
                        if (rooms.Count == 0)
                        {
                            RoomData(mappingRoomRequest, RoomMappings, "SupplierRoomCategory");
                        }
                    }
                    else
                    {
                        goto case "SupplierRoomCategory";
                    }
                    break;
                case "SupplierRoomCategory":
                    if (!string.IsNullOrEmpty(mappingRoomRequest.SupplierRoomCategory))
                    {
                        rooms = RoomMappings.Where(w => w.SupplierRoomCategory == mappingRoomRequest.SupplierRoomCategory).ToList();
                        if (rooms.Count == 0)
                        {
                            rooms = RoomData(mappingRoomRequest, RoomMappings, "SupplierRoomCategoryId");
                        }
                    }
                    else
                    {
                        goto case "SupplierRoomCategoryId";
                    }
                    break;
                case "SupplierRoomCategoryId":
                    if (!string.IsNullOrEmpty(mappingRoomRequest.SupplierRoomCategory))
                    {
                        rooms = RoomMappings.Where(w => w.SupplierRoomCategoryId == mappingRoomRequest.SupplierRoomCategoryId).ToList();
                    }
                    else
                    {
                        goto default;
                    }
                    break;

                default:
                    rooms = new List<DC_ConpanyAccommodationRoomMapping>();
                    break;



            }

            return rooms;
        }

        private bool ValidateCompanySpecificHotelAndRoomTypeMappingRQ(CompanySpecificHotelAndRoomType_Rq RQ, ref string message)
        {

            bool isValid = false;
            message = "invalid request";

            if (RQ == null)
            {
                message = "malformed request";
                return false;
            }

            if (!string.IsNullOrEmpty(RQ.SessionId) && RQ.MappingRequests != null)
            {

                foreach (var mappingRequest in RQ.MappingRequests)
                {


                    if (!string.IsNullOrEmpty(mappingRequest.SequenceNumber) &&
                        !string.IsNullOrEmpty(mappingRequest.SupplierCode)
                        && !string.IsNullOrEmpty(mappingRequest.SupplierProductCode)
                        && !string.IsNullOrEmpty(mappingRequest.TLGXCompanyCode)
                        && (mappingRequest.SupplierRoomTypes != null)
                        )
                    {
                        if (mappingRequest.SupplierRoomTypes.Any())
                        {
                            foreach (var roomType in mappingRequest.SupplierRoomTypes)
                            {
                                isValid = false;

                                if (!string.IsNullOrEmpty(roomType.SupplierRoomId)
                                || !string.IsNullOrEmpty(roomType.SupplierRoomTypeCode)
                                || !string.IsNullOrEmpty(roomType.SupplierRoomName)
                                || !string.IsNullOrEmpty(roomType.SupplierRoomCategory)
                                || !string.IsNullOrEmpty(roomType.SupplierRoomCategoryId))
                                {
                                    isValid = true;
                                }
                                else
                                {
                                    return isValid;
                                }
                            }

                        }
                        else
                        {
                            isValid = true;
                        }

                    }
                    else
                    {

                        return isValid;

                    }

                    if (!isValid)
                    {
                        return isValid;
                    }

                }
            }

            return isValid;
        }

    }
}