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
        /// Retrieves TLGX Room Type Info for Supplier Code(s), Supplier Hotel Code (s) and Supplier Room Code/Name.
        /// API can handle single / multiple supplier and single / multiple property requests at a time. 
        /// </summary>
        /// <param name="RQ"></param>
        /// <returns>Original Mapping request with TLGX Room Info details and Mapped Status result. If there are no mapping record exists, MapId will be returned as Zero.</returns>
        [HttpPost]
        [Route("RoomTypeMapping")]
        [ResponseType(typeof(List<RoomTypeMapping_SIRS>))]
        public async Task<HttpResponseMessage> GetBulkRoomTypeMapping(List<RoomTypeMapping_SIRQ> RQ)
        {
            try
            {
                _database = MongoDBHandler.mDatabase();
                IMongoCollection<BsonDocument> collection = _database.GetCollection<BsonDocument>("RoomTypeMapping");

                var returnResult = new List<RoomTypeMapping_SIRS>();

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
