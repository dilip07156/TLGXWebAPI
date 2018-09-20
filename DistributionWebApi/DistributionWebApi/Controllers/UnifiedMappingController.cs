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
                    _database = MongoDBHandler.mDatabase();
                    IMongoCollection<BsonDocument> collectionRoomTypeMapping = _database.GetCollection<BsonDocument>("RoomTypeMapping");
                    IMongoCollection<RoomTypeMappingOnline> collection_rto = _database.GetCollection<RoomTypeMappingOnline>("RoomTypeMappingOnline");
                    IMongoCollection<BsonDocument> collectionProductMapping = _database.GetCollection<BsonDocument>("ProductMappingLite");

                    List<ProductMappingLite> searchedHotelMappingData = new List<ProductMappingLite>();
                    List<HotelRoomTypeMappingModel> searchedRoomMappingData = new List<HotelRoomTypeMappingModel>();

                    var SupplierCodes = RQ.MappingRequests.Where(w => w.SupplierCode != null).Select(x => x.SupplierCode.ToUpper()).Distinct().ToArray();
                    var SupplierProductCodes = RQ.MappingRequests.Where(w => w.SupplierProductCode != null).Select(x => x.SupplierProductCode.ToUpper()).Distinct().ToArray();
                    var SupplierRoomTypeIds = RQ.MappingRequests.SelectMany(p => p.SupplierRoomTypes.Where(w => w.SupplierRoomId != null).Select(s => s.SupplierRoomId.ToUpper())).Distinct().ToArray();
                    var SupplierRoomTypeCodes = RQ.MappingRequests.SelectMany(p => p.SupplierRoomTypes.Where(w => w.SupplierRoomTypeCode != null).Select(s => s.SupplierRoomTypeCode.ToUpper())).Distinct().ToArray();

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

                    #region RoomMappingSearch

                    if(SupplierRoomTypeIds.Length > 0 || SupplierRoomTypeCodes.Length > 0)
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
                        projectRoomMapping = projectRoomMapping.Include("SystemRoomTypeMapId");
                        projectRoomMapping = projectRoomMapping.Include("SystemProductCode");
                        projectRoomMapping = projectRoomMapping.Include("TLGXAccoRoomId");
                        projectRoomMapping = projectRoomMapping.Include("TLGXAccoId");

                        var searchRoomMappingResult = collectionRoomTypeMapping.Find(filterRoomMapping).Project(projectRoomMapping).ToList();
                        searchedRoomMappingData = JsonConvert.DeserializeObject<List<HotelRoomTypeMappingModel>>(searchRoomMappingResult.ToJson());
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
                            mappingResponse.CommonHotelId = HotelMapping.SystemProductCode;
                            mappingResponse.ProductMapId = HotelMapping.MapId;
                            mappingResponse.TlgxAccoId = HotelMapping.TlgxMdmHotelId;
                        }

                        var RoomMappings = searchedRoomMappingData.Where(w => w.supplierCode == mappingRequest.SupplierCode && w.SupplierProductId == mappingRequest.SupplierProductCode).Select(s => s).ToList();
                        if (RoomMappings.Count > 0)
                        {
                            mappingResponse.ContainsRoomMappings = true;
                        }

                        var RoomMappingResponseList = new List<UnifiedHotelAndRoomMapping_RoomTypeResponse>();
                        foreach (var mappingRoomRequest in mappingRequest.SupplierRoomTypes)
                        {
                            var RoomMappingResponse = new UnifiedHotelAndRoomMapping_RoomTypeResponse();
                            RoomMappingResponse.SupplierRoomCategory = mappingRoomRequest.SupplierRoomCategory;
                            RoomMappingResponse.SupplierRoomCategoryId = mappingRoomRequest.SupplierRoomCategoryId;
                            RoomMappingResponse.SupplierRoomId = mappingRoomRequest.SupplierRoomId;
                            RoomMappingResponse.SupplierRoomName = mappingRoomRequest.SupplierRoomName;
                            RoomMappingResponse.SupplierRoomTypeCode = mappingRoomRequest.SupplierRoomTypeCode;

                            if(mappingResponse.ContainsRoomMappings)
                            {
                                RoomMappingResponse.MappedRooms = RoomMappings.Where(w => w.SupplierRoomId == mappingRoomRequest.SupplierRoomId || w.SupplierRoomTypeCode == mappingRoomRequest.SupplierRoomTypeCode).Select(s => new UnifiedHotelAndRoomMapping_MappedRoomType { RoomMapId = s.SystemRoomTypeMapId, TlgxAccoRoomId = s.TLGXAccoRoomId }).ToList();
                            }
                            else
                            {
                                RoomMappingResponse.MappedRooms = new List<UnifiedHotelAndRoomMapping_MappedRoomType>();
                            }
                            
                            RoomMappingResponseList.Add(RoomMappingResponse);
                        }
                        mappingResponse.SupplierRoomTypes = RoomMappingResponseList;

                        mappingResponseList.Add(mappingResponse);
                    }

                    returnResult.MappingResponses = mappingResponseList;

                    #endregion Build Response

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