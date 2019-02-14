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

            RQ = RQ.Where(w => w != null).ToList();

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
                    string[] statusToCheck = { "MAPPED", "AUTOMAPPED" };
                    filter = Builders<BsonDocument>.Filter.Empty;
                    filter = filter & Builders<BsonDocument>.Filter.Regex("SupplierCode", new BsonRegularExpression(new Regex(item.SupplierCode, RegexOptions.IgnoreCase)));
                    filter = filter & Builders<BsonDocument>.Filter.Regex("SupplierProductCode", new BsonRegularExpression(new Regex(item.SupplierProductCode, RegexOptions.IgnoreCase)));
                    filter = filter & Builders<BsonDocument>.Filter.AnyIn("MappingStatus", statusToCheck);

                    BsonDocument searchResult = null;
                    if (item.ProductType.ToLower() == "hotel")
                    {
                        searchResult = await collectionProductMapping.Find(filter)
                       .Project(project)
                       .FirstOrDefaultAsync();

                        if (searchResult != null)
                        {
                            result.SystemProductCode = searchResult["SystemProductCode"].AsString;
                            result.TlgxMdmHotelId = searchResult["TlgxMdmHotelId"].AsString;
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
            _database = MongoDBHandler.mDatabase();

            IMongoCollection<BsonDocument> collectionProductMapping = _database.GetCollection<BsonDocument>("ProductMappingLite");
            //IMongoCollection<BsonDocument> collectionActivityMapping = _database.GetCollection<BsonDocument>("ActivityMappingLite");

            RQ = RQ.Where(w => w != null).ToList();

            var SupplierCodes = RQ.Where(w => w.SupplierCode != null).Select(x => x.SupplierCode.ToUpper()).Distinct().ToArray();
            var SupplierProductCodes = RQ.Where(w => w.SupplierProductCode != null).Select(x => x.SupplierProductCode.ToUpper()).Distinct().ToArray();

            FilterDefinition<BsonDocument> filter;
            filter = Builders<BsonDocument>.Filter.Empty;

            //string[] statusToCheck = { "MAPPED", "AUTOMAPPED" };
            filter = filter & Builders<BsonDocument>.Filter.AnyIn("SupplierCode", SupplierCodes);
            filter = filter & Builders<BsonDocument>.Filter.AnyIn("SupplierProductCode", SupplierProductCodes);
            //filter = filter & Builders<BsonDocument>.Filter.AnyIn("MappingStatus", statusToCheck);

            ProjectionDefinition<BsonDocument> project = Builders<BsonDocument>.Projection.Include("SupplierCode");
            project = project.Exclude("_id");
            project = project.Include("SupplierProductCode");
            project = project.Include("SystemProductCode");
            project = project.Include("MapId");
            project = project.Include("TlgxMdmHotelId");

            var searchResult = await collectionProductMapping.Find(filter).Project(project).ToListAsync();

            List<ProductMappingLite> searchedData = JsonConvert.DeserializeObject<List<ProductMappingLite>>(searchResult.ToJson());

            List<ProductMappingLite_RS> resultList = new List<ProductMappingLite_RS>();

            resultList = (from rq in RQ
                          join sd in searchedData on new { SupplierCode = rq.SupplierCode.ToUpper(), SupplierProductCode = rq.SupplierProductCode.ToUpper() } equals new { SupplierCode = sd.SupplierCode, SupplierProductCode = sd.SupplierProductCode } into sdtemp
                          from sdlj in sdtemp.DefaultIfEmpty()
                          select new ProductMappingLite_RS
                          {
                              MapId = (sdlj == null ? 0 : sdlj.MapId),
                              SupplierProductCode = rq.SupplierProductCode,
                              SupplierCode = rq.SupplierCode,
                              ProductType = rq.ProductType,
                              SequenceNumber = rq.SequenceNumber,
                              SessionId = rq.SessionId,
                              SystemProductCode = (sdlj == null ? string.Empty : sdlj.SystemProductCode),
                              TlgxMdmHotelId = (sdlj == null ? string.Empty : sdlj.TlgxMdmHotelId)
                          }).ToList();

            //int mapCount = resultList.Where(w => w.MapId != 0).Count();
            //foreach (var item in RQ)
            //{
            //    var result = new ProductMappingLite_RS();

            //    result.SessionId = item.SessionId;
            //    result.SequenceNumber = item.SequenceNumber;
            //    result.SupplierCode = item.SupplierCode;
            //    result.SupplierProductCode = item.SupplierProductCode;
            //    result.ProductType = item.ProductType;
            //    result.SystemProductCode = string.Empty;

            //    if (string.IsNullOrWhiteSpace(item.SupplierCode) || string.IsNullOrWhiteSpace(item.SupplierProductCode))
            //    {
            //        result.SystemProductCode = string.Empty;
            //        resultList.Add(result);
            //        result = null;
            //        continue;
            //    }
            //    else
            //    {
            //        var searchMapResult = searchedData.Where(w => w.SupplierCode == item.SupplierCode.ToUpper() && w.SupplierProductCode == item.SupplierProductCode.ToUpper()).Select(s => s).FirstOrDefault();
            //        if (searchMapResult != null)
            //        {
            //            result.SystemProductCode = searchMapResult.SystemProductCode;
            //            result.MapId = searchMapResult.MapId;
            //            resultList.Add(result);
            //            result = null;
            //        }
            //        else
            //        {
            //            result.SystemProductCode = string.Empty;
            //            resultList.Add(result);
            //            result = null;
            //        }
            //    }

            //}

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, resultList);
            return response;
        }

        /// <summary>
        /// Retrieves all Supplier Hotel Code for System product code.
        /// </summary>
        /// <param name="ProductCode"></param>
        /// <returns>A list of Supplier Code and Supplier Product code mapped to System product code which is sent in request</returns>
        [HttpGet]
        [Route("System/Product/ProductCode/{ProductCode}")]
        [ResponseType(typeof(List<CompleteProductMapping_RS>))]
        public async Task<HttpResponseMessage> GetAllSupplierProductMappingByCode(string ProductCode)
        {
            _database = MongoDBHandler.mDatabase();

            IMongoCollection<ProductMapping> collectionProductMapping = _database.GetCollection<ProductMapping>("ProductMapping");

            FilterDefinition<ProductMapping> filter;
            filter = Builders<ProductMapping>.Filter.Empty;
            filter = filter & Builders<ProductMapping>.Filter.Eq(x => x.SystemProductCode, ProductCode.Trim().ToUpper());
            filter = filter & Builders<ProductMapping>.Filter.Or(Builders<ProductMapping>.Filter.Eq(x => x.MappingStatus, "MAPPED"), Builders<ProductMapping>.Filter.Eq(x => x.MappingStatus, "AUTOMAPPED"));

            //var searchResult = await collectionProductMapping.Find(filter)
            //                    .Project(x => new SystemProductMapping_RS
            //                    {
            //                        SupplierCode = x.SupplierCode,
            //                        MapId = x.MapId,
            //                        SupplierProductCode = x.SupplierProductCode,
            //                        SystemProductCode = x.SystemProductCode,
            //                        TlgxMdmHotelId = x.TlgxMdmHotelId
            //                    })
            //                    .ToListAsync();

            var searchResult = await collectionProductMapping.Find(filter)
                                .Project(x => new CompleteProductMapping_RS
                                {
                                    SupplierCode = x.SupplierCode,
                                    MapId = x.MapId,
                                    SupplierProductCode = x.SupplierProductCode,
                                    SystemProductCode = x.SystemProductCode,
                                    TlgxMdmHotelId = x.TlgxMdmHotelId,
                                    SupplierCityCode = x.SupplierCityCode,
                                    SupplierCityName = x.SupplierCityName,
                                    SupplierCountryCode = x.SupplierCountryCode,
                                    SupplierCountryName = x.SupplierCountryName,
                                    SupplierProductName = x.SupplierProductName,
                                    SystemCityCode = x.SystemCityCode,
                                    SystemCityName = x.SystemCityName,
                                    SystemCountryCode = x.SystemCountryCode,
                                    SystemCountryName = x.SystemCountryName,
                                    SystemProductName = x.SystemProductName,
                                    SystemProductType = x.SystemProductType
                                })
                                .ToListAsync();

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, searchResult);
            return response;
        }

        /// <summary>
        /// Retrieves all Supplier Product Mapping for TLGX MDM Hotel Id.
        /// </summary>
        /// <param name="TlgxMdmHotelId"></param>
        /// <returns>A list of Supplier Code and Supplier Product code mapped to TLGX MDM Hotel Id which is sent in request</returns>
        [HttpGet]
        [Route("TLGX/Product/ProductCode/{TlgxMdmHotelId}")]
        [ResponseType(typeof(List<CompleteProductMapping_RS>))]
        public async Task<HttpResponseMessage> GetAllSupplierProductMappingByTlgxMdmCode(string TlgxMdmHotelId)
        {
            _database = MongoDBHandler.mDatabase();

            IMongoCollection<ProductMapping> collectionProductMapping = _database.GetCollection<ProductMapping>("ProductMapping");

            FilterDefinition<ProductMapping> filter;
            filter = Builders<ProductMapping>.Filter.Empty;
            filter = filter & Builders<ProductMapping>.Filter.Eq(x => x.TlgxMdmHotelId, TlgxMdmHotelId.Trim().ToUpper());
            filter = filter & Builders<ProductMapping>.Filter.Or(Builders<ProductMapping>.Filter.Eq(x => x.MappingStatus, "MAPPED"), Builders<ProductMapping>.Filter.Eq(x => x.MappingStatus, "AUTOMAPPED"));

            //var searchResult = await collectionProductMapping.Find(filter)
            //                    .Project(x => new SystemProductMapping_RS
            //                    {
            //                        SupplierCode = x.SupplierCode,
            //                        MapId = x.MapId,
            //                        SupplierProductCode = x.SupplierProductCode,
            //                        SystemProductCode = x.SystemProductCode,
            //                        TlgxMdmHotelId = x.TlgxMdmHotelId
            //                    })
            //                    .ToListAsync();

            var searchResult = await collectionProductMapping.Find(filter)
                                .Project(x => new CompleteProductMapping_RS
                                {
                                    SupplierCode = x.SupplierCode,
                                    MapId = x.MapId,
                                    SupplierProductCode = x.SupplierProductCode,
                                    SystemProductCode = x.SystemProductCode,
                                    TlgxMdmHotelId = x.TlgxMdmHotelId,
                                    SupplierCityCode = x.SupplierCityCode,
                                    SupplierCityName = x.SupplierCityName,
                                    SupplierCountryCode = x.SupplierCountryCode,
                                    SupplierCountryName = x.SupplierCountryName,
                                    SupplierProductName = x.SupplierProductName,
                                    SystemCityCode = x.SystemCityCode,
                                    SystemCityName = x.SystemCityName,
                                    SystemCountryCode = x.SystemCountryCode,
                                    SystemCountryName = x.SystemCountryName,
                                    SystemProductName = x.SystemProductName,
                                    SystemProductType = x.SystemProductType
                                })
                                .ToListAsync();

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, searchResult);
            return response;
        }

        /// <summary>
        /// Retrieves System Hotel Mapping for System Product Code and Supplier Code
        /// </summary>
        /// <param name="ProductCode">System Product Code</param>
        /// <param name="SupplierCode">TLGX Supplier Code</param>
        /// <returns>System Hotel Mapping</returns>
        [HttpGet]
        [Route("System/Product/ProductCode/{ProductCode}/SupplierCode/{SupplierCode}")]
        [ResponseType(typeof(List<CompleteProductMapping_RS>))]
        public async Task<HttpResponseMessage> GetSupplierProductMappingByCode(string ProductCode, string SupplierCode)
        {
            _database = MongoDBHandler.mDatabase();

            IMongoCollection<ProductMapping> collectionProductMapping = _database.GetCollection<ProductMapping>("ProductMapping");

            FilterDefinition<ProductMapping> filter;
            filter = Builders<ProductMapping>.Filter.Empty;
            filter = filter & Builders<ProductMapping>.Filter.Eq(x => x.SystemProductCode, ProductCode.Trim().ToUpper());
            filter = filter & Builders<ProductMapping>.Filter.Eq(x => x.SupplierCode, SupplierCode.Trim().ToUpper());
            filter = filter & Builders<ProductMapping>.Filter.Or(Builders<ProductMapping>.Filter.Eq(x => x.MappingStatus, "MAPPED"), Builders<ProductMapping>.Filter.Eq(x => x.MappingStatus, "AUTOMAPPED"));

            //var searchResult = await collectionProductMapping.Find(filter)
            //                    .Project(x => new SystemProductMapping_RS
            //                    {
            //                        SupplierCode = x.SupplierCode,
            //                        MapId = x.MapId,
            //                        SupplierProductCode = x.SupplierProductCode,
            //                        SystemProductCode = x.SystemProductCode,
            //                        TlgxMdmHotelId = x.TlgxMdmHotelId
            //                    })
            //                    .ToListAsync();

            var searchResult = await collectionProductMapping.Find(filter)
                                .Project(x => new CompleteProductMapping_RS
                                {
                                    SupplierCode = x.SupplierCode,
                                    MapId = x.MapId,
                                    SupplierProductCode = x.SupplierProductCode,
                                    SystemProductCode = x.SystemProductCode,
                                    TlgxMdmHotelId = x.TlgxMdmHotelId,
                                    SupplierCityCode = x.SupplierCityCode,
                                    SupplierCityName = x.SupplierCityName,
                                    SupplierCountryCode = x.SupplierCountryCode,
                                    SupplierCountryName = x.SupplierCountryName,
                                    SupplierProductName = x.SupplierProductName,
                                    SystemCityCode = x.SystemCityCode,
                                    SystemCityName = x.SystemCityName,
                                    SystemCountryCode = x.SystemCountryCode,
                                    SystemCountryName = x.SystemCountryName,
                                    SystemProductName = x.SystemProductName,
                                    SystemProductType = x.SystemProductType
                                })
                                .ToListAsync();

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, searchResult);
            return response;
        }

        /// <summary>
        /// Retrieves System Hotel Mapping for TLGX MDM Hotel Id and Supplier Code
        /// </summary>
        /// <param name="TlgxMdmHotelId">TLGX MDM Hotel Id</param>
        /// <param name="SupplierCode">TLGX Supplier Code</param>
        /// <returns>System Hotel Mapping</returns>
        [HttpGet]
        [Route("TLGX/Product/ProductCode/{TlgxMdmHotelId}/SupplierCode/{SupplierCode}")]
        [ResponseType(typeof(List<CompleteProductMapping_RS>))]
        public async Task<HttpResponseMessage> GetSupplierProductMappingByTlgxMdmHotelId(string TlgxMdmHotelId, string SupplierCode)
        {
            _database = MongoDBHandler.mDatabase();

            IMongoCollection<ProductMapping> collectionProductMapping = _database.GetCollection<ProductMapping>("ProductMapping");

            FilterDefinition<ProductMapping> filter;
            filter = Builders<ProductMapping>.Filter.Empty;
            filter = filter & Builders<ProductMapping>.Filter.Eq(x => x.TlgxMdmHotelId, TlgxMdmHotelId.Trim().ToUpper());
            filter = filter & Builders<ProductMapping>.Filter.Eq(x => x.SupplierCode, SupplierCode.Trim().ToUpper());
            filter = filter & Builders<ProductMapping>.Filter.Or(Builders<ProductMapping>.Filter.Eq(x => x.MappingStatus, "MAPPED"), Builders<ProductMapping>.Filter.Eq(x => x.MappingStatus, "AUTOMAPPED"));

            //var searchResult = await collectionProductMapping.Find(filter)
            //                    .Project(x => new SystemProductMapping_RS
            //                    {
            //                        SupplierCode = x.SupplierCode,
            //                        MapId = x.MapId,
            //                        SupplierProductCode = x.SupplierProductCode,
            //                        SystemProductCode = x.SystemProductCode,
            //                        TlgxMdmHotelId = x.TlgxMdmHotelId
            //                    })
            //                    .ToListAsync();

            var searchResult = await collectionProductMapping.Find(filter)
                                .Project(x => new CompleteProductMapping_RS
                                {
                                    SupplierCode = x.SupplierCode,
                                    MapId = x.MapId,
                                    SupplierProductCode = x.SupplierProductCode,
                                    SystemProductCode = x.SystemProductCode,
                                    TlgxMdmHotelId = x.TlgxMdmHotelId,
                                    SupplierCityCode = x.SupplierCityCode,
                                    SupplierCityName = x.SupplierCityName,
                                    SupplierCountryCode = x.SupplierCountryCode,
                                    SupplierCountryName = x.SupplierCountryName,
                                    SupplierProductName = x.SupplierProductName,
                                    SystemCityCode = x.SystemCityCode,
                                    SystemCityName = x.SystemCityName,
                                    SystemCountryCode = x.SystemCountryCode,
                                    SystemCountryName = x.SystemCountryName,
                                    SystemProductName = x.SystemProductName,
                                    SystemProductType = x.SystemProductType
                                })
                                .ToListAsync();

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, searchResult);
            return response;
        }

        /// <summary>
        /// Retrieves System Hotel Mapping for Supplier ProductCode and Supplier Code
        /// </summary>
        /// <param name="SupplierProductCode">Supplier Product Code</param>
        /// <param name="SupplierCode">TLGX Supplier Code</param>
        /// <returns>System Hotel Mapping</returns>
        [HttpGet]
        [Route("Supplier/Product/SupplierProductCode/{SupplierProductCode}/SupplierCode/{SupplierCode}")]
        [ResponseType(typeof(List<CompleteProductMapping_RS>))]
        public async Task<HttpResponseMessage> GetSystemProductMappingByCode(string SupplierProductCode, string SupplierCode)
        {
            _database = MongoDBHandler.mDatabase();

            IMongoCollection<ProductMapping> collectionProductMapping = _database.GetCollection<ProductMapping>("ProductMapping");

            FilterDefinition<ProductMapping> filter;
            filter = Builders<ProductMapping>.Filter.Empty;
            filter = filter & Builders<ProductMapping>.Filter.Eq(x => x.SupplierProductCode, SupplierProductCode.Trim().ToUpper());
            filter = filter & Builders<ProductMapping>.Filter.Eq(x => x.SupplierCode, SupplierCode.Trim().ToUpper());
            filter = filter & Builders<ProductMapping>.Filter.Or(Builders<ProductMapping>.Filter.Eq(x => x.MappingStatus, "MAPPED"), Builders<ProductMapping>.Filter.Eq(x => x.MappingStatus, "AUTOMAPPED"));

            //var searchResult = await collectionProductMapping.Find(filter)
            //                    .Project(x => new SystemProductMapping_RS
            //                    {
            //                        SupplierCode = x.SupplierCode,
            //                        MapId = x.MapId,
            //                        SupplierProductCode = x.SupplierProductCode,
            //                        SystemProductCode = x.SystemProductCode,
            //                        TlgxMdmHotelId = x.TlgxMdmHotelId
            //                    })
            //                    .ToListAsync();

            var searchResult = await collectionProductMapping.Find(filter)
                                .Project(x => new CompleteProductMapping_RS
                                {
                                    SupplierCode = x.SupplierCode,
                                    MapId = x.MapId,
                                    SupplierProductCode = x.SupplierProductCode,
                                    SystemProductCode = x.SystemProductCode,
                                    TlgxMdmHotelId = x.TlgxMdmHotelId,
                                    SupplierCityCode = x.SupplierCityCode,
                                    SupplierCityName = x.SupplierCityName,
                                    SupplierCountryCode = x.SupplierCountryCode,
                                    SupplierCountryName = x.SupplierCountryName,
                                    SupplierProductName = x.SupplierProductName,
                                    SystemCityCode = x.SystemCityCode,
                                    SystemCityName = x.SystemCityName,
                                    SystemCountryCode = x.SystemCountryCode,
                                    SystemCountryName = x.SystemCountryName,
                                    SystemProductName = x.SystemProductName,
                                    SystemProductType = x.SystemProductType
                                })
                                .ToListAsync();

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, searchResult);
            return response;
        }

        /// <summary>
        /// Retrieves supplier hotel code array by system city code and supplier code
        /// </summary>
        /// <param name="SystemCityCode">System City Code</param>
        /// <param name="SupplierCode">System Supplier Code</param>
        /// <returns>Array of Supplier Product Codes for provided System City Code and Supplier Code</returns>
        [HttpGet]
        [Route("Supplier/Product/SystemCityCode/{SystemCityCode}/SupplierCode/{SupplierCode}")]
        [ResponseType(typeof(string[]))]
        public async Task<HttpResponseMessage> GetListOfSupplierProductCodes(string SystemCityCode, string SupplierCode)
        {
            _database = MongoDBHandler.mDatabase();

            IMongoCollection<ProductMapping> collectionProductMapping = _database.GetCollection<ProductMapping>("ProductMapping");

            FilterDefinition<ProductMapping> filter;
            filter = Builders<ProductMapping>.Filter.Empty;
            filter = filter & Builders<ProductMapping>.Filter.Eq(x => x.SystemCityCode, SystemCityCode.Trim().ToUpper());
            filter = filter & Builders<ProductMapping>.Filter.Eq(x => x.SupplierCode, SupplierCode.Trim().ToUpper());

            var searchResult = await collectionProductMapping.Find(filter)
                                .Project(x => new
                                {
                                    x.SupplierProductCode
                                })
                                .ToListAsync();
            var searchResultArray = searchResult.Select(x => x.SupplierProductCode).Distinct().ToArray();

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, searchResultArray);
            return response;
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