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
using System.Text.RegularExpressions;
using MongoDB.Bson.IO;
using Newtonsoft.Json.Linq;
using System.Web.Http.Description;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace DistributionWebApi.Controllers
{
    /// <summary>
    /// Used to retrieve TLGX Mapping data for Room Type. 
    /// </summary>

    [RoutePrefix("Mapping")]
    public class RoomTypeMappingController : ApiController
    {
        /// <summary>
        /// Mongo database handler
        /// </summary>
        protected static IMongoDatabase _database;

        /// <summary>
        /// Retrieves TLGX Room Type Id for TLGX Common Hotel Ids, accepting multiple suppliers and multiple supplier room types.
        /// API can handle single / multiple TLGX Common Hotel Ids at a time. 
        /// API can only return room type mappings where accommodation room info rows exists in MDM system and it has been processed. 
        /// Please note that not all suppliers provide static data for mapping and real time requests into the mapping engine are not permitted.
        /// </summary>
        /// <param name="RQ"></param>
        /// <returns>Original Mapping request with TLGX Accommodation Room Info Id and TLGXCommonRoomId with Nakshatra MapId in result. 
        /// If there are no mapping record exists, MapId will be returned as 0 and TLGXCommonRoomId will be returned as empty.</returns>
        [HttpPost]
        [Route("RoomTypeMapping")]
        [ResponseType(typeof(RoomTypeMapping_SIRS))]
        public async Task<HttpResponseMessage> GetBulkRoomTypeMapping(RoomTypeMapping_SIRQ RQ)
        {
            var returnResult = new RoomTypeMapping_SIRS();
            if (RQ != null)
            {
                _database = MongoDBHandler.mDatabase();
                IMongoCollection<BsonDocument> collection = _database.GetCollection<BsonDocument>("RoomTypeMapping");
                IMongoCollection<RoomTypeMappingOnline> collection_rto = _database.GetCollection<RoomTypeMappingOnline>("RoomTypeMappingOnline");

                returnResult.Mode = RQ.Mode;
                returnResult.BatchId = RQ.BatchId;

                if (RQ.HotelRoomTypeMappingRequests != null)
                {
                    var hrtmrsl = new List<RoomTypeMapping_SIRS_HotelRoomTypeMappingResponses>();

                    if (RQ.HotelRoomTypeMappingRequests != null)
                    {
                        var writeModelDetails = new List<WriteModel<RoomTypeMappingOnline>>();
                        foreach (var hrtmrq in RQ.HotelRoomTypeMappingRequests)
                        {
                            var hrtmrs = new RoomTypeMapping_SIRS_HotelRoomTypeMappingResponses();
                            hrtmrs.TLGXCommonHotelId = hrtmrq.TLGXCommonHotelId;

                            var hrtmrs_sdl = new List<RoomTypeMapping_SIRS_SupplierData>();

                            if (hrtmrq.SupplierData != null)
                            {
                                foreach (var hrtmrq_sd in hrtmrq.SupplierData)
                                {
                                    var hrtmrs_sd = new RoomTypeMapping_SIRS_SupplierData();

                                    hrtmrs_sd.SupplierId = hrtmrq_sd.SupplierId;
                                    hrtmrs_sd.SupplierProductId = hrtmrq_sd.SupplierProductId;

                                    var hrtmrs_srtl = new List<RoomTypeMapping_SIRS_SupplierRoomType>();

                                    if (hrtmrq_sd.SupplierRoomTypes != null)
                                    {
                                        foreach (var hrtmrq_srt in hrtmrq_sd.SupplierRoomTypes)
                                        {
                                            var hrtmrs_srt = new RoomTypeMapping_SIRS_SupplierRoomType();

                                            hrtmrs_srt.SupplierRoomId = hrtmrq_srt.SupplierRoomId;
                                            hrtmrs_srt.SupplierRoomTypeCode = hrtmrq_srt.SupplierRoomTypeCode;
                                            hrtmrs_srt.SupplierRoomName = hrtmrq_srt.SupplierRoomName;
                                            hrtmrs_srt.SupplierRoomCategory = hrtmrq_srt.SupplierRoomCategory;
                                            hrtmrs_srt.SupplierRoomCategoryId = hrtmrq_srt.SupplierRoomCategoryId;

                                            var iValidCounterCheck = 0;
                                            var builder = Builders<BsonDocument>.Filter;
                                            var filter = builder.Empty;

                                            if (!string.IsNullOrWhiteSpace(hrtmrq.TLGXCommonHotelId))
                                            {
                                                filter = filter & builder.Eq("TLGXAccoId", hrtmrq.TLGXCommonHotelId.ToUpper());
                                                iValidCounterCheck++;
                                            }

                                            if (!string.IsNullOrWhiteSpace(hrtmrq_sd.SupplierId))
                                            {
                                                filter = filter & builder.Eq("supplierCode", hrtmrq_sd.SupplierId.ToUpper());
                                                iValidCounterCheck++;
                                            }

                                            if (!string.IsNullOrWhiteSpace(hrtmrq_sd.SupplierProductId))
                                            {
                                                filter = filter & builder.Eq("SupplierProductId", hrtmrq_sd.SupplierProductId.ToUpper());
                                                iValidCounterCheck++;
                                            }

                                            if (!string.IsNullOrWhiteSpace(hrtmrq_srt.SupplierRoomTypeCode))
                                            {
                                                filter = filter & builder.Eq("SupplierRoomTypeCode", hrtmrq_srt.SupplierRoomTypeCode);
                                                iValidCounterCheck++;
                                            }
                                            else if (!string.IsNullOrWhiteSpace(hrtmrq_srt.SupplierRoomId))
                                            {
                                                filter = filter & builder.Eq("SupplierRoomId", hrtmrq_srt.SupplierRoomId);
                                                iValidCounterCheck++;
                                            }

                                            if (iValidCounterCheck == 4)
                                            {
                                                BsonDocument result = new BsonDocument();
                                                try
                                                {
                                                    result = collection.Find(filter).FirstOrDefault();

                                                    if (result != null)
                                                    {
                                                        hrtmrs_srt.TLGXCommonRoomId = result["TLGXAccoRoomId"].AsString;
                                                        hrtmrs_srt.MapId = Convert.ToString(result["SystemRoomTypeMapId"].AsNullableInt32);
                                                    }
                                                    else
                                                    {
                                                        hrtmrs_srt.TLGXCommonRoomId = string.Empty;
                                                        hrtmrs_srt.MapId = "0";
                                                    }
                                                }
                                                catch
                                                {
                                                    hrtmrs_srt.TLGXCommonRoomId = string.Empty;
                                                    hrtmrs_srt.MapId = "0";
                                                }
                                            }
                                            else
                                            {
                                                hrtmrs_srt.TLGXCommonRoomId = string.Empty;
                                                hrtmrs_srt.MapId = "0";
                                            }

                                            hrtmrs_srtl.Add(hrtmrs_srt);

                                            #region Roomtype Mapping Online data insert into Mongo
                                            if (hrtmrs_srt.MapId == "0")
                                            {
                                                // Fire & Forget 
                                                writeModelDetails.Add(InsertRoomTypeMappingOnline(collection_rto, new RoomTypeMappingOnline
                                                {
                                                    //_id = ObjectId.GenerateNewId(),
                                                    Amenities = hrtmrq_srt.Amenities,
                                                    BatchId = RQ.BatchId,
                                                    BathRoomType = hrtmrq_srt.BathRoomType,
                                                    BeddingConfig = hrtmrq_srt.BeddingConfig,
                                                    Bedrooms = hrtmrq_srt.Bedrooms,
                                                    BedType = hrtmrq_srt.BedType,
                                                    ChildAge = hrtmrq_srt.ChildAge,
                                                    CreateDateTime = DateTime.Now,
                                                    ExtraBed = hrtmrq_srt.ExtraBed,
                                                    FloorName = hrtmrq_srt.FloorName,
                                                    FloorNumber = hrtmrq_srt.FloorNumber,
                                                    MaxAdults = hrtmrq_srt.MaxAdults,
                                                    MaxChild = hrtmrq_srt.MaxChild,
                                                    MaxGuestOccupancy = hrtmrq_srt.MaxGuestOccupancy,
                                                    MaxInfants = hrtmrq_srt.MaxInfants,
                                                    MinGuestOccupancy = hrtmrq_srt.MinGuestOccupancy,
                                                    Mode = RQ.Mode,
                                                    PromotionalVendorCode = hrtmrq_srt.PromotionalVendorCode,
                                                    Quantity = hrtmrq_srt.Quantity,
                                                    RatePlan = hrtmrq_srt.RatePlan,
                                                    RatePlanCode = hrtmrq_srt.RatePlanCode,
                                                    RoomLocationCode = hrtmrq_srt.RoomLocationCode,
                                                    RoomSize = hrtmrq_srt.RoomSize,
                                                    RoomView = hrtmrq_srt.RoomView,
                                                    Smoking = hrtmrq_srt.Smoking,
                                                    SupplierId = hrtmrq_sd.SupplierId,
                                                    SupplierProductId = hrtmrq_sd.SupplierProductId,
                                                    SupplierRoomCategory = hrtmrq_srt.SupplierRoomCategory,
                                                    SupplierRoomCategoryId = hrtmrq_srt.SupplierRoomCategoryId,
                                                    SupplierRoomId = hrtmrq_srt.SupplierRoomId,
                                                    SupplierRoomName = hrtmrq_srt.SupplierRoomName,
                                                    SupplierRoomTypeCode = hrtmrq_srt.SupplierRoomTypeCode,
                                                    TLGXCommonHotelId = hrtmrq.TLGXCommonHotelId
                                                }));
                                            }
                                            #endregion

                                        }
                                    }

                                    hrtmrs_sd.SupplierRoomTypes = hrtmrs_srtl;
                                    hrtmrs_sdl.Add(hrtmrs_sd);
                                }
                            }

                            hrtmrs.SupplierData = hrtmrs_sdl;
                            hrtmrsl.Add(hrtmrs);
                        }

                        if (writeModelDetails.Any())
                        {
                            Task.Run(() => { collection_rto.BulkWrite(writeModelDetails); });
                        }
                    }

                    returnResult.HotelRoomTypeMappingResponses = hrtmrsl;
                }
            }

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, returnResult);
            return response;
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

        //public async Task<HttpResponseMessage> GetBulkRoomTypeMapping(List<Models.RoomTypeMapping_RQ> RQ)
        //{
        //    try
        //    {
        //        _database = MongoDBHandler.mDatabase();

        //        IMongoCollection<BsonDocument> collection = _database.GetCollection<BsonDocument>("RoomTypeMapping");

        //        var SupplierCodes = RQ.Select(x => x.SupplierCode.ToUpper()).Distinct().ToArray();
        //        var SupplierProductCodes = RQ.Select(x => x.SupplierProductCode.ToUpper()).Distinct().ToArray();
        //        var SupplierRoomTypeCodes = RQ.Select(x => x.SupplierRoomTypeCode.ToUpper()).Distinct().ToArray();
        //        var SupplierRoomTypeName = RQ.Select(x => x.SupplierRoomTypeName.ToUpper()).Distinct().ToArray();

        //        FilterDefinition<BsonDocument> filter;
        //        filter = Builders<BsonDocument>.Filter.Empty;

        //        filter = filter & Builders<BsonDocument>.Filter.AnyIn("SupplierCode", SupplierCodes);
        //        filter = filter & Builders<BsonDocument>.Filter.AnyIn("SupplierProductCode", SupplierProductCodes);
        //        filter = filter & Builders<BsonDocument>.Filter.AnyIn("SupplierRoomTypeCode", SupplierRoomTypeCodes);
        //        filter = filter & Builders<BsonDocument>.Filter.AnyIn("SupplierRoomTypeName", SupplierRoomTypeName);

        //        ProjectionDefinition<BsonDocument> project = Builders<BsonDocument>.Projection.Exclude("_id");

        //        var searchBsonResult = await collection.Find(filter).Project(project).ToListAsync();

        //        List<RoomTypeMappingModel> searchResult = Newtonsoft.Json.JsonConvert.DeserializeObject<List<RoomTypeMappingModel>>(searchBsonResult.ToJson());

        //        List<RoomTypeMapping_RS> returnResult = new List<RoomTypeMapping_RS>();

        //        foreach (var item in RQ)
        //        {
        //            var result = new RoomTypeMapping_RS();

        //            result.SequenceNumber = item.SequenceNumber;
        //            result.SupplierCode = item.SupplierCode;
        //            result.SupplierProductCode = item.SupplierProductCode;
        //            result.SupplierRoomTypeName = item.SupplierRoomTypeName;
        //            result.SupplierRoomTypeCode = item.SupplierRoomTypeCode;

        //            if (string.IsNullOrWhiteSpace(item.SupplierCode))
        //            {
        //                result.Status = "ERROR";
        //                result.Remarks = "SupplierCode is mandatory";
        //                returnResult.Add(result);
        //                result = null;
        //                continue;
        //            }
        //            else if (string.IsNullOrWhiteSpace(item.SupplierProductCode))
        //            {
        //                result.Status = "ERROR";
        //                result.Remarks = "SupplierProductCode is mandatory";
        //                returnResult.Add(result);
        //                result = null;
        //                continue;
        //            }
        //            else if (string.IsNullOrWhiteSpace(item.SupplierRoomTypeCode))
        //            {
        //                result.Status = "ERROR";
        //                result.Remarks = "SupplierRoomTypeCode is mandatory";
        //                returnResult.Add(result);
        //                result = null;
        //                continue;
        //            }
        //            else if (string.IsNullOrWhiteSpace(item.SupplierRoomTypeName))
        //            {
        //                result.Status = "ERROR";
        //                result.Remarks = "SupplierRoomTypeName is mandatory";
        //                returnResult.Add(result);
        //                result = null;
        //                continue;
        //            }
        //            else
        //            {
        //                RoomTypeMappingModel searchMapResult = searchResult.Where(w => w.SupplierCode == item.SupplierCode.ToUpper() && w.SupplierProductCode == item.SupplierProductCode.ToUpper() && w.SupplierRoomTypeCode == item.SupplierRoomTypeCode.ToUpper() && w.SupplierRoomTypeName == item.SupplierRoomTypeName.ToUpper()).Select(s => s).FirstOrDefault();

        //                if (searchMapResult == null)
        //                {
        //                    searchMapResult = searchResult.Where(w => w.SupplierCode == item.SupplierCode.ToUpper() && w.SupplierProductCode == item.SupplierProductCode.ToUpper() && w.SupplierRoomTypeCode == item.SupplierRoomTypeCode.ToUpper()).Select(s => s).FirstOrDefault();
        //                }

        //                if (searchMapResult == null)
        //                {
        //                    searchMapResult = searchResult.Where(w => w.SupplierCode == item.SupplierCode.ToUpper() && w.SupplierProductCode == item.SupplierProductCode.ToUpper() && w.SupplierRoomTypeName == item.SupplierRoomTypeName.ToUpper()).Select(s => s).FirstOrDefault();
        //                }

        //                if (searchMapResult != null)
        //                {
        //                    result.SystemProductCode = searchMapResult.SystemProductCode;
        //                    result.SystemRoomTypeMapId = searchMapResult.SystemRoomTypeMapId;
        //                    result.SystemRoomTypeCode = searchMapResult.SystemRoomTypeCode;
        //                    result.SystemRoomTypeName = searchMapResult.SystemRoomTypeName;
        //                    result.SystemStrippedRoomType = searchMapResult.SystemStrippedRoomType;
        //                    result.SystemNormalizedRoomType = searchMapResult.SystemNormalizedRoomType;
        //                    result.Attibutes = searchMapResult.Attibutes;
        //                    result.Status = "MAPPED";
        //                    returnResult.Add(result);
        //                    result = null;
        //                }
        //                else
        //                {
        //                    result.Status = "UNMAPPED";
        //                    result.Remarks = "No Match found.";
        //                    returnResult.Add(result);
        //                    result = null;
        //                }
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
    }
}
