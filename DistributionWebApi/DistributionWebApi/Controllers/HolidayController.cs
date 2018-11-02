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
    }
}
