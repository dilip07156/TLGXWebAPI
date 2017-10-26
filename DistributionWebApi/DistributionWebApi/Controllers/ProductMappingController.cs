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

namespace DistributionWebApi.Controllers
{
    /// <summary>
    /// Used to retrieve TLGX Mapping data for specific Product Types. Each service has a FULL and a LITE version. 
    /// The FULL Version includes the ability to send Supplier-specific geographic information. 
    /// This FULL service can be used should the LITE service not retrieve a mapping result.
    /// TLGX Supplier Codes should always be used when making a request.
    /// </summary>

    [RoutePrefix("Mapping")]
    public class ProductMappingController : ApiController
    {
        /// <summary>
        /// Mongo database handler
        /// </summary>
        protected static IMongoDatabase _database;

        /// <summary>
        /// Retrieves TLGX Hotel Property Code for Supplier Code(s), Supplier Hotel Code (s) and Geographical information. 
        /// API can handle single / multiple supplier and single / multiple property requests at a time. 
        /// </summary>
        /// <param name="RQ"></param>
        /// <returns>Original Mapping request with TLGX Hotel Code and Mapped Status result. If there are no mapping record exists, MapId will be returned as Zero.</returns>
        [HttpPost]
        [Route("ProductMapping")]
        [ResponseType(typeof(List<ProductMapping_RS>))]
        public async Task<HttpResponseMessage> GetBulkProductMapping(List<Models.ProductMapping_RQ> RQ)
        {
            try
            {
                //_database = MongoDBHandler.mDatabase();
                //var collection = _database.GetCollection<ProductMapping>("ProductMapping");
                ////FilterDefinition<ProductMapping> filter;

                //var resultList = new List<ProductMapping_RS>();

                //foreach (var item in RQ)
                //{
                //    var result = new ProductMapping_RS();

                //    result.ProductType = item.ProductType;
                //    result.SequenceNumber = item.SequenceNumber;
                //    result.SessionId = item.SessionId;
                //    result.SupplierCityCode = item.SupplierCityCode;
                //    result.SupplierCityName = item.SupplierCityName;
                //    result.SupplierCode = item.SupplierCode;
                //    result.SupplierCountryCode = item.SupplierCountryCode;
                //    result.SupplierCountryName = item.SupplierCountryName;
                //    result.SupplierProductCode = item.SupplierProductCode;
                //    result.SupplierProductName = item.SupplierProductName;
                //    result.Status = string.Empty;
                //    result.SystemProductCode = string.Empty;

                //    if (string.IsNullOrWhiteSpace(item.SupplierCode))
                //    {
                //        result.Status = "Supplier Code can't be blank.";
                //    }

                //    if (string.IsNullOrWhiteSpace(item.SupplierProductCode))
                //    {
                //        result.Status = "Supplier Product Code can't be blank.";
                //    }

                //    if (result.Status != string.Empty)
                //    {
                //        result.SystemProductCode = string.Empty;
                //        resultList.Add(result);
                //        result = null;
                //        continue;
                //    }
                //    else
                //    {
                //        //filter = Builders<ProductMapping>.Filter.Empty;
                //        //filter = filter & Builders<ProductMapping>.Filter.
                //        //filter = filter & Builders<ProductMapping>.Filter.Regex(x => x.SupplierProductCode, new BsonRegularExpression(new Regex(item.SupplierProductCode, RegexOptions.IgnoreCase)));

                //        var searchResult = await collection.Find(x => x.SupplierCode.ToLower() == item.SupplierCode.ToLower() && x.SupplierProductCode.ToLower() == item.SupplierProductCode.ToLower()).FirstOrDefaultAsync();
                //        if (searchResult != null)
                //        {
                //            result.SystemProductCode = searchResult.SystemProductCode;
                //            result.Status = "Mapped";
                //            resultList.Add(result);
                //            result = null;
                //        }
                //        else
                //        {
                //            result.SystemProductCode = string.Empty;
                //            result.Status = "No results found.";
                //            resultList.Add(result);
                //            result = null;
                //        }
                //    }

                //}

                //HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, resultList);
                //return response;


                _database = MongoDBHandler.mDatabase();

                IMongoCollection<BsonDocument> collectionProductMapping;
                collectionProductMapping = _database.GetCollection<BsonDocument>("ProductMapping");

                IMongoCollection<BsonDocument> collectionActivityMapping;
                collectionActivityMapping = _database.GetCollection<BsonDocument>("ActivityMapping");

                FilterDefinition<BsonDocument> filter;
                ProjectionDefinition<BsonDocument> project = Builders<BsonDocument>.Projection.Include("SystemProductCode");
                project = project.Exclude("_id");
                project = project.Include("MapId");

                var resultList = new List<ProductMapping_RS>();

                foreach (var item in RQ)
                {
                    var result = new ProductMapping_RS();

                    result.ProductType = item.ProductType;
                    result.SequenceNumber = item.SequenceNumber;
                    result.SessionId = item.SessionId;
                    result.SupplierCityCode = item.SupplierCityCode;
                    result.SupplierCityName = item.SupplierCityName;
                    result.SupplierCode = item.SupplierCode;
                    result.SupplierCountryCode = item.SupplierCountryCode;
                    result.SupplierCountryName = item.SupplierCountryName;
                    result.SupplierProductCode = item.SupplierProductCode;
                    result.SupplierProductName = item.SupplierProductName;
                    result.Status = string.Empty;
                    result.SystemProductCode = string.Empty;

                    if (string.IsNullOrWhiteSpace(item.SupplierCode))
                    {
                        result.Status = "Supplier Code can't be blank.";
                    }

                    if (string.IsNullOrWhiteSpace(item.SupplierProductCode))
                    {
                        result.Status = "Supplier Product Code can't be blank.";
                    }

                    if (result.Status != string.Empty)
                    {
                        result.SystemProductCode = string.Empty;
                        resultList.Add(result);
                        result = null;
                        continue;
                    }
                    else
                    {
                        filter = Builders<BsonDocument>.Filter.Empty;
                        filter = filter & Builders<BsonDocument>.Filter.Regex("SupplierCode", new BsonRegularExpression(new Regex(item.SupplierCode, RegexOptions.IgnoreCase)));
                        filter = filter & Builders<BsonDocument>.Filter.Regex("SupplierProductCode", new BsonRegularExpression(new Regex(item.SupplierProductCode, RegexOptions.IgnoreCase)));

                        BsonDocument searchResult = null;
                        if (item.ProductType.ToLower() == "hotel")
                        {
                            searchResult = await collectionProductMapping.Find(filter)
                           .Project(project)
                           .FirstOrDefaultAsync();

                            if (searchResult != null)
                            {
                                result.SystemProductCode = searchResult["SystemProductCode"].AsString;
                                result.MapId = searchResult["MapId"].AsInt32;
                                result.Status = "Mapped";
                                resultList.Add(result);
                                result = null;
                            }
                            else
                            {
                                result.SystemProductCode = string.Empty;
                                result.Status = "No results found.";
                                resultList.Add(result);
                                result = null;
                            }

                        }
                        else if (item.ProductType.ToLower() == "activity")
                        {
                            searchResult = await collectionActivityMapping.Find(filter)
                           .Project(project)
                           .FirstOrDefaultAsync();

                            if (searchResult != null)
                            {
                                result.SystemProductCode = searchResult["SystemProductCode"].AsString;
                                result.MapId = searchResult["MapId"].AsInt32;
                                result.Status = "Mapped";
                                resultList.Add(result);
                                result = null;
                            }
                            else
                            {
                                result.SystemProductCode = string.Empty;
                                result.Status = "No results found.";
                                resultList.Add(result);
                                result = null;
                            }

                        }
                        else
                        {
                            result.SystemProductCode = string.Empty;
                            result.Status = "Invalid Product Type.";
                            resultList.Add(result);
                            result = null;
                        }

                    }

                }

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, resultList);
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
        /// Retrieves TLGX Hotel Property Code for Supplier Code(s), Supplier Hotel Code (s) only.
        /// API can handle single / multiple supplier and single / multiple property requests at a time. 
        /// </summary>
        /// <param name="RQ"></param>
        /// <returns>Original Mapping request with TLGX Hotel Code and Mapped Status result. If there are no mapping record exists, MapId will be returned as Zero.</returns>
        [HttpPost]
        [Route("ProductMappingLite")]
        [ResponseType(typeof(List<ProductMappingLite_RS>))]
        public async Task<HttpResponseMessage> GetBulkProductMappingLite(List<Models.ProductMappingLite_RQ> RQ)
        {
            try
            {
                _database = MongoDBHandler.mDatabase();

                IMongoCollection<BsonDocument> collectionProductMapping = _database.GetCollection<BsonDocument>("ProductMappingLite");
                //IMongoCollection<BsonDocument> collectionActivityMapping = _database.GetCollection<BsonDocument>("ActivityMappingLite");

                var SupplierCodes = RQ.Select(x => x.SupplierCode.ToUpper()).Distinct().ToArray();
                var SupplierProductCodes = RQ.Select(x => x.SupplierProductCode.ToUpper()).Distinct().ToArray();

                FilterDefinition<BsonDocument> filter;
                filter = Builders<BsonDocument>.Filter.Empty;

                filter = filter & Builders<BsonDocument>.Filter.AnyIn("SupplierCode", SupplierCodes);
                filter = filter & Builders<BsonDocument>.Filter.AnyIn("SupplierProductCode", SupplierProductCodes);

                ProjectionDefinition<BsonDocument> project = Builders<BsonDocument>.Projection.Include("SupplierCode");
                project = project.Exclude("_id");
                project = project.Include("SupplierProductCode");
                project = project.Include("SystemProductCode");
                project = project.Include("MapId");

                var searchResult = collectionProductMapping.Find(filter).Project(project).ToList();

                List<ProductMappingLite>  searchedData = JsonConvert.DeserializeObject<List<ProductMappingLite>>(searchResult.ToJson());

                List<ProductMappingLite_RS> resultList = new List<ProductMappingLite_RS>();

                //resultList = (from rq in RQ
                //              from sd in searchedData.Where(w => w.SupplierCode == rq.SupplierCode.ToUpper() && w.SupplierProductCode == rq.SupplierProductCode.ToUpper()).DefaultIfEmpty()
                //              select new ProductMappingLite_RS
                //              {
                //                  MapId = sd.MapId,
                //                  SupplierProductCode = rq.SupplierProductCode,
                //                  SupplierCode = rq.SupplierCode,
                //                  ProductType = rq.ProductType,
                //                  SequenceNumber = rq.SequenceNumber,
                //                  SessionId = rq.SessionId,
                //                  SystemProductCode = sd.SystemProductCode
                //              }).ToList();


                foreach (var item in RQ)
                {
                    var result = new ProductMappingLite_RS();

                    result.SessionId = item.SessionId;
                    result.SequenceNumber = item.SequenceNumber;
                    result.SupplierCode = item.SupplierCode;
                    result.SupplierProductCode = item.SupplierProductCode;
                    result.ProductType = item.ProductType;
                    result.SystemProductCode = string.Empty;

                    if (string.IsNullOrWhiteSpace(item.SupplierCode) || string.IsNullOrWhiteSpace(item.SupplierProductCode))
                    {
                        result.SystemProductCode = string.Empty;
                        resultList.Add(result);
                        result = null;
                        continue;
                    }
                    else
                    {
                        var searchMapResult = searchedData.Where(w => w.SupplierCode == item.SupplierCode.ToUpper() && w.SupplierProductCode == item.SupplierProductCode.ToUpper()).Select(s => s).FirstOrDefault();
                        if (searchMapResult != null)
                        {
                            result.SystemProductCode = searchMapResult.SystemProductCode;
                            result.MapId = searchMapResult.MapId;
                            resultList.Add(result);
                            result = null;
                        }
                        else
                        {
                            result.SystemProductCode = string.Empty;
                            resultList.Add(result);
                            result = null;
                        }
                    }

                }

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, resultList);
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
        /// Retrieves all Hotel Property Code for Supplier Code(s), Supplier Hotel Code (s) for System product code.
        /// </summary>
        /// <param name="SystemProductCode"></param>
        /// <returns>A list of Supplier Code and Supplier Product code mapped to System product code sent in request</returns>
        [HttpPost]
        [Route("TlgxProductMapping")]
        [ResponseType(typeof(List<TlgxProductMapping_RS>))]
        public async Task<HttpResponseMessage> GetAllTlgxProductMapping(string SystemProductCode)
        {
            try
            {
                _database = MongoDBHandler.mDatabase();

                IMongoCollection<ProductMappingLite> collectionProductMapping = _database.GetCollection<ProductMappingLite>("ProductMappingLite");

                FilterDefinition<ProductMappingLite> filter;
                filter = Builders<ProductMappingLite>.Filter.Empty;
                filter = filter & Builders<ProductMappingLite>.Filter.Regex(x => x.SystemProductCode, new BsonRegularExpression(new Regex(SystemProductCode, RegexOptions.IgnoreCase)));

                var searchResult = await collectionProductMapping.Find(filter)
                                    .Project(x => new TlgxProductMapping_RS
                                    {
                                        SupplierCode = x.SupplierCode,
                                        MapId = x.MapId,
                                        SupplierProductCode = x.SupplierProductCode,
                                    })
                                    .ToListAsync();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, searchResult);
                return response;

            }
            catch (Exception ex)
            {
                NLogHelper.Nlogger_LogError.LogError(ex, this.GetType().FullName, Request.GetActionDescriptor().ActionName, Request.RequestUri.PathAndQuery);
                HttpResponseMessage response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Server Error. Contact Admin. Error Date : " + DateTime.Now.ToString());
                return response;
            }
        }



        //public async Task<HttpResponseMessage> GetBulkProductMappingLite(List<Models.ProductMappingLite_RQ> RQ)
        //{
        //    try
        //    {
        //        _database = MongoDBHandler.mDatabase();

        //        IMongoCollection<BsonDocument> collectionProductMapping;
        //        collectionProductMapping = _database.GetCollection<BsonDocument>("ProductMappingLite");

        //        IMongoCollection<BsonDocument> collectionActivityMapping;
        //        collectionActivityMapping = _database.GetCollection<BsonDocument>("ActivityMappingLite");

        //        var SupplierCodes = (from r in RQ
        //                             select r.SupplierCode).Distinct().ToArray();
        //        var SupplierProductCodes = (from r in RQ
        //                                    select r.SupplierProductCode).Distinct().ToArray();

        //        FilterDefinition<BsonDocument> filter;
        //        ProjectionDefinition<BsonDocument> project = Builders<BsonDocument>.Projection.Include("SystemProductCode");
        //        project = project.Exclude("_id");
        //        project = project.Include("MapId");
        //        project = project.Include("MapId");

        //        var resultList = new List<ProductMappingLite_RS>();

        //        foreach (var item in RQ)
        //        {
        //            var result = new ProductMappingLite_RS();

        //            result.SessionId = item.SessionId;
        //            result.SequenceNumber = item.SequenceNumber;
        //            result.SupplierCode = item.SupplierCode;
        //            result.SupplierProductCode = item.SupplierProductCode;
        //            result.ProductType = item.ProductType;
        //            result.SystemProductCode = string.Empty;

        //            if (string.IsNullOrWhiteSpace(item.SupplierCode) || string.IsNullOrWhiteSpace(item.SupplierProductCode))
        //            {
        //                result.SystemProductCode = string.Empty;
        //                resultList.Add(result);
        //                result = null;
        //                continue;
        //            }
        //            else
        //            {
        //                filter = Builders<BsonDocument>.Filter.Empty;
        //                filter = filter & Builders<BsonDocument>.Filter.Regex("SupplierCode", new BsonRegularExpression(new Regex(item.SupplierCode, RegexOptions.IgnoreCase)));
        //                filter = filter & Builders<BsonDocument>.Filter.Regex("SupplierProductCode", new BsonRegularExpression(new Regex(item.SupplierProductCode, RegexOptions.IgnoreCase)));

        //                BsonDocument searchResult = null;
        //                if (item.ProductType.ToLower() == "hotel")
        //                {
        //                    searchResult = await collectionProductMapping.Find(filter)
        //                   .Project(project)
        //                   .FirstOrDefaultAsync();

        //                    if (searchResult != null)
        //                    {
        //                        result.SystemProductCode = searchResult["SystemProductCode"].AsString;
        //                        result.MapId = searchResult["MapId"].AsInt32;
        //                        resultList.Add(result);
        //                        result = null;
        //                    }
        //                    else
        //                    {
        //                        result.SystemProductCode = string.Empty;
        //                        resultList.Add(result);
        //                        result = null;
        //                    }

        //                }
        //                else if (item.ProductType.ToLower() == "activity")
        //                {
        //                    searchResult = await collectionActivityMapping.Find(filter)
        //                   .Project(project)
        //                   .FirstOrDefaultAsync();

        //                    if (searchResult != null)
        //                    {
        //                        result.SystemProductCode = searchResult["SystemProductCode"].AsString;
        //                        result.MapId = searchResult["MapId"].AsInt32;
        //                        resultList.Add(result);
        //                        result = null;
        //                    }
        //                    else
        //                    {
        //                        result.SystemProductCode = string.Empty;
        //                        resultList.Add(result);
        //                        result = null;
        //                    }

        //                }
        //                else
        //                {
        //                    result.SystemProductCode = string.Empty;
        //                    resultList.Add(result);
        //                    result = null;
        //                }
        //            }

        //        }

        //        HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, resultList);
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