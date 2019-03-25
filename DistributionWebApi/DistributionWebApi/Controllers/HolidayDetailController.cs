using DistributionWebApi.Models;
using DistributionWebApi.Mongo;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace DistributionWebApi.Controllers
{
    /// <summary>
    /// Get data from HolidayDetail collection
    /// </summary>
    [RoutePrefix("HolidayDetail")]
    public class HolidayDetailController : ApiController
    {
        /// <summary>
        /// Mongo database handler
        /// </summary>
        private static IMongoDatabase _database;

        /// <summary>
        /// Constructor for HolidayDetailController
        /// </summary>
        public HolidayDetailController()
        {
            _database = MongoDBHandler.mDatabase();
        }

        /// <summary>
        /// This API is used for returning colletions all supplier responses whether it will be XML or JSON for given filters as 
        /// Supplier Name and tourID.
        /// </summary>
        /// <param name="supplierName">Supplier name is first filter mandatory Criteria</param>
        /// <param name="tourID">Supplier product code is second filter mandatory criteria.</param>
        /// <returns>list of supplier response</returns>
        [HttpGet]
        [Route("Get/{supplierName}/{tourID}")]
        [ResponseType(typeof(HolidayDetail))]
        public async Task<HttpResponseMessage> GetSupplierDeails(string supplierName, string tourID)
        {
            IMongoCollection<HolidayDetail> holidayDetailCollection = _database.GetCollection<HolidayDetail>("HolidayDetail");
            FilterDefinition<HolidayDetail> filter;
            filter = Builders<HolidayDetail>.Filter.Empty;
            
            if (!string.IsNullOrEmpty(supplierName))
            {
                var escapedSupplierName = Regex.Escape(supplierName);
                filter = filter & Builders<HolidayDetail>.Filter.Regex(x => x.SupplierName, new BsonRegularExpression(new Regex(escapedSupplierName, RegexOptions.IgnoreCase)));
                filter = filter & Builders<HolidayDetail>.Filter.Where(x => x.CallType != "TourList");
            }

            if (!string.IsNullOrEmpty(tourID))
            {
                filter = filter & Builders<HolidayDetail>.Filter.ElemMatch(x => x.CallDetails.TourIDs, x => x.TourID.ToUpper() == tourID.ToUpper());
                
            }
            
            var searchResult = await holidayDetailCollection.Find(filter).ToListAsync();
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, searchResult);
            return response;
        }
    }
}
