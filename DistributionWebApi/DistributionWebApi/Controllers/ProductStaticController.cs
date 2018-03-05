using DistributionWebApi.Models.Static;
using DistributionWebApi.Mongo;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using NLog;
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
    /// This Controls to fetch Static Data for Accommodation
    /// </summary>
    [RoutePrefix("StaticData")]
    public class ProductStaticController : ApiController
    {
        /// <summary>
        /// static object of Mongo DB Class
        /// </summary>
        protected static IMongoDatabase _database;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Retrieves a Search Result Accommodation Static Data List of based on a Collection of Supplier Code combined with a Supplier Product Code.
        /// </summary>
        /// <param name="param">Collection of multiple requests</param>
        /// <returns>Returns a Collection of Search Results. Search response will contain requested data and a result field againt each request.
        /// </returns>
        [Route("Accommodation/Get")]
        [HttpPost]
        [ResponseType(typeof(List<StaticData_RS>))]
        public async Task<HttpResponseMessage> GetProductStaticData(List<StaticData_RQ> param)
        {
            try
            {
                if (param == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid request parameter");
                }

                List<StaticData_RS> resultList = new List<StaticData_RS>();

                _database = MongoDBHandler.mDatabase();
                //get AccoStaticData
                var collectionAccoStaticData = _database.GetCollection<Accomodation>("AccoStaticData");

                foreach(var RQ in param)
                {
                    var searchResult = collectionAccoStaticData.Find(x => x.AccomodationInfo.CompanyId == RQ.SupplierCode.Trim().ToUpper() && x.AccomodationInfo.CompanyProductId == RQ.SupplierProductCode.Trim().ToUpper()).FirstOrDefault();
                    resultList.Add(new StaticData_RS
                    {
                        SupplierCode = RQ.SupplierCode,
                        SupplierProductCode = RQ.SupplierProductCode,
                        Result = searchResult
                    });
                }

                //string[] arrayOfSupplierCodes = param.Select(s => s.SupplierCode.Trim().ToUpper()).Distinct().ToArray();
                //string[] arrayOfSupplierProductCodes = param.Select(s => s.SupplierProductCode.Trim().ToUpper()).Distinct().ToArray();

                //FilterDefinition<BsonDocument> filter;
                //filter = Builders<BsonDocument>.Filter.Empty;
                //filter = filter & Builders<BsonDocument>.Filter.AnyIn("AccomodationInfo.CompanyId", arrayOfSupplierCodes);
                //filter = filter & Builders<BsonDocument>.Filter.AnyIn("AccomodationInfo.CompanyProductId", arrayOfSupplierProductCodes);

                //ProjectionDefinition<BsonDocument> project = Builders<BsonDocument>.Projection.Exclude("_id");
                ////project = project.Include("AccomodationInfo");

                //var searchResult = await collectionAccoStaticData.Find(filter).Project(project).ToListAsync();

                //List<Accomodation> searchedData = JsonConvert.DeserializeObject<List<Accomodation>>(searchResult.ToJson());

                ////Found Result
                //resultList = (from a in searchedData
                //              select new StaticData_RS
                //              {
                //                  SupplierCode = a.AccomodationInfo.CompanyId,
                //                  SupplierProductCode = a.AccomodationInfo.CompanyProductId,
                //                  Result = a
                //              }).ToList();

                ////Not Found Result
                //var resultListNotFound = (from RQ in param
                //                          join FOUND in resultList on new { SupplierCode = RQ.SupplierCode.Trim().ToUpper(), ProductCode = RQ.SupplierProductCode.Trim().ToUpper() } equals new { SupplierCode = FOUND.SupplierCode.Trim().ToUpper(), ProductCode = FOUND.SupplierProductCode.Trim().ToUpper() } into foundLJ
                //                          from fdLJ in foundLJ.DefaultIfEmpty()
                //                          where fdLJ == null
                //                          select new StaticData_RS
                //                          {
                //                              SupplierCode = RQ.SupplierCode,
                //                              SupplierProductCode = RQ.SupplierProductCode,
                //                              Result = null
                //                          }).ToList();

                //resultList.AddRange(resultListNotFound);

                return Request.CreateResponse(HttpStatusCode.OK, resultList);
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
