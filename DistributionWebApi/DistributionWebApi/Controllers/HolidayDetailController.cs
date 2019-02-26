using DistributionWebApi.Models;
using DistributionWebApi.Mongo;
using MongoDB.Driver;
using System.Net;
using System.Net.Http;
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
        /// Fetch supplier details
        /// </summary>
        /// <param name="supplierName">Supplier name</param>
        /// <param name="tourID">Supplier product code</param>
        /// <returns>list of supplier response</returns>
        [HttpGet]
        [Route("Get/{supplierName}/{tourID}")]
        [ResponseType(typeof(HolidayDetail))]
        public async Task<HttpResponseMessage> GetSupplierDeails(string supplierName, string tourID)
        {
            IMongoCollection<HolidayDetail> holidayDetailCollection = _database.GetCollection<HolidayDetail>("HolidayDetail");
            FilterDefinition<HolidayDetail> filter;
            filter = Builders<HolidayDetail>.Filter.Empty;
            filter = filter & Builders<HolidayDetail>.Filter.Eq(x => x.SupplierName, supplierName.TrimEnd().TrimStart());
            filter = filter & Builders<HolidayDetail>.Filter.ElemMatch(x => x.CallDetails.TourIDs, x => x.TourID == tourID);
            var searchResult = await holidayDetailCollection.Find(filter).ToListAsync();
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, searchResult);
            return response;
        }
    }
}
