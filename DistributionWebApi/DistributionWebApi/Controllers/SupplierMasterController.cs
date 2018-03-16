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
using System.Web.Http.Description;

namespace DistributionWebApi.Controllers
{
    /// <summary>
    /// Used to retrieve Master Supplier Data for use in conjunction with all mapping services. TLGX Supplier Codes should always be used when making a mapping request.
    /// </summary>
    [RoutePrefix("Masters/Get")]
    public class SupplierMasterController : ApiController
    {
        /// <summary>
        /// Mongo database handler
        /// </summary>
        protected static IMongoDatabase _database;

        /// <summary>
        /// Retrieve Full TLGX Master Supplier List for TLGX Mapping
        /// </summary>
        /// <returns>TLGX Supplier List</returns>
        [Route("Supplier/All")]
        [HttpGet]
        [ResponseType(typeof(Supplier))]
        public async Task<HttpResponseMessage> GetAllSupplier()
        {
            try
            {
                _database = MongoDBHandler.mDatabase();
                var collection = _database.GetCollection<Supplier>("Supplier");
                var result = await collection.Find(s => true).SortBy(s => s.SupplierCode).ToListAsync();
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, result);
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
        /// Get Full TLGX Supplier Master Detail by TLGX Supplier Code 
        /// </summary>
        /// <param name="Code">TLGX Supplier Code. This can be retrieved using Supplier Master Service.</param>
        /// <returns>List of Supplier</returns>
        [Route("Supplier/Code/{Code}")]
        [HttpGet]
        [ResponseType(typeof(Supplier))]
        public async Task<HttpResponseMessage> GetSupplierByCode(string Code)
        {
            try
            {
                _database = MongoDBHandler.mDatabase();
                var collection = _database.GetCollection<Supplier>("Supplier");
                var result = await collection.Find(s => s.SupplierCode == Code.Trim().ToUpper()).SortBy(s => s.SupplierCode).ToListAsync();
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, result);
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
        /// Get all system suppliers by supplier name (Filtered using StartsWith)
        /// </summary>
        /// <param name="Name">TLGX Supplier Name. This can be retrieved using Supplier Master Service.</param>
        /// <returns>List of Supplier</returns>
        [Route("Supplier/Name/{Name}")]
        [HttpGet]
        [ResponseType(typeof(Supplier))]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<HttpResponseMessage> GetSupplierByName(string Name)
        {
            try
            {
                _database = MongoDBHandler.mDatabase();
                var collection = _database.GetCollection<Supplier>("Supplier");
                var result = await collection.Find(s => s.SupplierName == Name.Trim().ToUpper()).SortBy(s => s.SupplierCode).ToListAsync();
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, result);
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