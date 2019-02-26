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
        protected static IMongoDatabase _database;

        /// <summary>
        /// Constructor for HolidayDetailController
        /// </summary>
        public HolidayDetailController()
        {
            _database = MongoDBHandler.mDatabase();
        }

        /// <summary>
        /// Gets supplier details based on supplierName callType tourID
        /// </summary>
        /// <param name="supplierName"> Supplier name </param>
        /// <param name="callType">Call type</param>
        /// <param name="tourID"> Tour ID</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Get/{supplierName}/{callType}/{tourID}")]
        [ResponseType(typeof(HolidayDetail))]
        public async Task<HttpResponseMessage> GetSupplierDeails(string supplierName, string callType, string tourID)
        {
            IMongoCollection<HolidayDetail> collectionHolidayModelMapping = _database.GetCollection<HolidayDetail>("HolidayDetail");
            FilterDefinition<HolidayDetail> filter;
            filter = Builders<HolidayDetail>.Filter.Empty;
            filter = filter & Builders<HolidayDetail>.Filter.Eq(x => x.SupplierName, supplierName.TrimEnd().TrimStart());
            var searchResult = await collectionHolidayModelMapping.Find(filter).FirstOrDefaultAsync();
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, searchResult);
            return response;
        }

    }
}
