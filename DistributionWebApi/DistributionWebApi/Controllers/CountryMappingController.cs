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

namespace DistributionWebApi.Controllers
{
    /// <summary>
    /// Used to retrieve TLGX Mapping data for specific Country and Supplier pairs. 
    /// </summary>

    [RoutePrefix("Mapping")]
    public class CountryMappingController : ApiController
    {
        /// <summary>
        /// Mongo database handler
        /// </summary>
        protected static IMongoDatabase _database;

        /// <summary>
        /// Retrieves Supplier Country Mapping for TLGX Country Name and Supplier Name
        /// </summary>
        /// <param name="CountryName">TLGX Country Name</param>
        /// <param name="SupplierName">TLGX Supplier Name</param>
        /// <returns>TLGX Supplier Country Mapping</returns>
        [HttpGet]
        [Route("TLGX/Country/CountryName/{CountryName}/SupplierName/{SupplierName}")]
        [ResponseType(typeof(CountryMapping_RS))]
        public async Task<HttpResponseMessage> GetSupplierCountryMappingByName(string CountryName, string SupplierName)
        {
            try
            {
                _database = MongoDBHandler.mDatabase();
                var collection = _database.GetCollection<CountryMapping>("CountryMapping");
                FilterDefinition<CountryMapping> filter;
                filter = Builders<CountryMapping>.Filter.Empty;

                filter = filter & Builders<CountryMapping>.Filter.Regex(x => x.CountryName, new BsonRegularExpression(new Regex(CountryName, RegexOptions.IgnoreCase)));
                filter = filter & Builders<CountryMapping>.Filter.Regex(x => x.SupplierName, new BsonRegularExpression(new Regex(SupplierName, RegexOptions.IgnoreCase)));

                var searchResult = await collection.Find(filter)
                                    .Project(x => new CountryMapping_RS
                                    {
                                        CountryName = x.CountryName,
                                        CountryCode = x.CountryCode,
                                        SupplierName = x.SupplierName,
                                        SupplierCode = x.SupplierCode,
                                        SupplierCountryCode = x.SupplierCountryCode,
                                        SupplierCountryName = x.SupplierCountryName,
                                        MapId = x.MapId
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

        /// <summary>
        /// Retrieves Supplier Country Master for TLGX Country Code
        /// </summary>
        /// <param name="CountryCode">TLGX Country Code</param>
        /// <param name="SupplierCode">TLGX Supplier Code</param>
        /// <returns>TLGX Supplier Country Mapping</returns>
        [HttpGet]
        [Route("TLGX/Country/CountryCode/{CountryCode}/SupplierCode/{SupplierCode}")]
        [ResponseType(typeof(CountryMapping_RS))]
        public async Task<HttpResponseMessage> GetSupplierCountryMappingByCode(string CountryCode, string SupplierCode)
        {
            try
            {
                _database = MongoDBHandler.mDatabase();
                var collection = _database.GetCollection<CountryMapping>("CountryMapping");
                FilterDefinition<CountryMapping> filter;
                filter = Builders<CountryMapping>.Filter.Empty;

                filter = filter & Builders<CountryMapping>.Filter.Regex(x => x.CountryCode, new BsonRegularExpression(new Regex(CountryCode, RegexOptions.IgnoreCase)));
                filter = filter & Builders<CountryMapping>.Filter.Regex(x => x.SupplierCode, new BsonRegularExpression(new Regex(SupplierCode, RegexOptions.IgnoreCase)));

                var searchResult = await collection.Find(filter)
                                    .Project(x => new CountryMapping_RS
                                    {
                                        CountryName = x.CountryName,
                                        CountryCode = x.CountryCode,
                                        SupplierName = x.SupplierName,
                                        SupplierCode = x.SupplierCode,
                                        SupplierCountryCode = x.SupplierCountryCode,
                                        SupplierCountryName = x.SupplierCountryName,
                                        MapId = x.MapId
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


        /// <summary>
        /// Retrieves All Supplier code and Country code for TLGX Country Code
        /// </summary>
        /// <param name="CountryCode">TLGX Country Code</param>
        /// <returns>All TLGX Supplier Country Mapping</returns>
        [HttpGet]
        [Route("TLGX/Country/CountryCode/{CountryCode}")]
        [ResponseType(typeof(TlgxCountryMapping_RS))]
        public async Task<HttpResponseMessage> GetAllSupplierCountryMappingByCode(string CountryCode)
        {
            try
            {
                _database = MongoDBHandler.mDatabase();
                var collection = _database.GetCollection<CountryMapping>("CountryMapping");
                FilterDefinition<CountryMapping> filter;
                filter = Builders<CountryMapping>.Filter.Empty;

                filter = filter & Builders<CountryMapping>.Filter.Regex(x => x.CountryCode, new BsonRegularExpression(new Regex(CountryCode, RegexOptions.IgnoreCase)));

                var searchResult = await collection.Find(filter)
                                    .Project(x => new TlgxCountryMapping_RS
                                    {
                                        SupplierCode = x.SupplierCode,
                                        SupplierCountryCode = x.SupplierCountryCode,
                                        MapId = x.MapId
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


        /// <summary>
        /// Retrieves TLGX System Country Code and Name against Supplier country code
        /// </summary>
        /// <param name="CountryCode">Supplier-specific Country Code</param>
        /// <param name="SupplierCode">TLGX Supplier Code</param>
        /// <returns>TLGX Country Master</returns>
        [HttpGet]
        [Route("Supplier/Country/SupplierCountryCode/{CountryCode}/SupplierCode/{SupplierCode}")]
        [ResponseType(typeof(CountryMapping_RS))]
        public async Task<HttpResponseMessage> GetSystemCountryMappingByCode(string CountryCode, string SupplierCode)
        {
            try
            {
                _database = MongoDBHandler.mDatabase();
                var collection = _database.GetCollection<CountryMapping>("CountryMapping");
                FilterDefinition<CountryMapping> filter;
                filter = Builders<CountryMapping>.Filter.Empty;

                filter = filter & Builders<CountryMapping>.Filter.Regex(x => x.SupplierCountryCode, new BsonRegularExpression(new Regex(CountryCode, RegexOptions.IgnoreCase)));
                filter = filter & Builders<CountryMapping>.Filter.Regex(x => x.SupplierCode, new BsonRegularExpression(new Regex(SupplierCode, RegexOptions.IgnoreCase)));

                var searchResult = await collection.Find(filter)
                                    .Project(x => new CountryMapping_RS
                                    {
                                        CountryName = x.CountryName,
                                        CountryCode = x.CountryCode,
                                        SupplierName = x.SupplierName,
                                        SupplierCode = x.SupplierCode,
                                        SupplierCountryCode = x.SupplierCountryCode,
                                        SupplierCountryName = x.SupplierCountryName,
                                        MapId = x.MapId
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

        /// <summary>
        /// Retrieves TLGX System Country Code and Name against Supplier Country Name and Supplier Name
        /// </summary>
        /// <param name="CountryName">Supplier-specific Country Name</param>
        /// <param name="SupplierName">TLGX Supplier Name</param>
        /// <returns>TLGX Country Master</returns>
        [HttpGet]
        [Route("Supplier/Country/SupplierCountryName/{CountryName}/SupplierName/{SupplierName}")]
        [ResponseType(typeof(CountryMapping_RS))]
        public async Task<HttpResponseMessage> GetSystemCountryMappingByName(string CountryName, string SupplierName)
        {
            try
            {
                _database = MongoDBHandler.mDatabase();
                var collection = _database.GetCollection<CountryMapping>("CountryMapping");
                FilterDefinition<CountryMapping> filter;
                filter = Builders<CountryMapping>.Filter.Empty;

                filter = filter & Builders<CountryMapping>.Filter.Regex(x => x.SupplierCountryName, new BsonRegularExpression(new Regex(CountryName, RegexOptions.IgnoreCase)));
                filter = filter & Builders<CountryMapping>.Filter.Regex(x => x.SupplierName, new BsonRegularExpression(new Regex(SupplierName, RegexOptions.IgnoreCase)));

                var searchResult = await collection.Find(filter)
                                    .Project(x => new CountryMapping_RS
                                    {
                                        CountryName = x.CountryName,
                                        CountryCode = x.CountryCode,
                                        SupplierName = x.SupplierName,
                                        SupplierCode = x.SupplierCode,
                                        SupplierCountryCode = x.SupplierCountryCode,
                                        SupplierCountryName = x.SupplierCountryName,
                                        MapId = x.MapId
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

        /// <summary>
        /// Retrieves Country Mapping for a Supplier against a specific supplier
        /// </summary>
        /// <param name="RQ"></param>
        /// <returns>Target supplier Country Name and Country Code</returns>
        [HttpPost]
        [Route("Country/CrossSupplierMapping")]
        [ResponseType(typeof(List<CrossSupplierCountryMapping_RS>))]
        public async Task<HttpResponseMessage> GetCrossSupplierCountryMapping(List<CrossSupplierCountryMapping_RQ> RQ)
        {
            try
            {
                List<CrossSupplierCountryMapping_RS> RS = new List<CrossSupplierCountryMapping_RS>();

                _database = MongoDBHandler.mDatabase();
                var collection = _database.GetCollection<CountryMapping>("CountryMapping");

                foreach (var iRQ in RQ)
                {
                    CrossSupplierCountryMapping_RS iRS = new CrossSupplierCountryMapping_RS();
                    iRS.SourceSupplierCode = iRQ.SourceSupplierCode;
                    iRS.SourceSupplierCountryCode = iRQ.SourceSupplierCountryCode;
                    iRS.TargetSupplierCode = iRQ.TargetSupplierCode;

                    if (iRS.SourceSupplierCode == null || iRS.SourceSupplierCountryCode == null || iRS.TargetSupplierCode == null)
                    {
                        iRS.Status = "Bad Request";
                    }
                    else
                    {
                        if (iRQ.SourceSupplierCode.ToUpper() == iRQ.TargetSupplierCode.ToUpper())
                        {
                            if (iRQ.SourceSupplierCode.ToUpper() == "TLGX")
                            {
                                var res = await collection.Find(x => (x.CountryCode.ToLower() == iRQ.SourceSupplierCountryCode.ToLower())).FirstOrDefaultAsync();
                                if (res != null)
                                {
                                    iRS.TargetSupplierCountryCode = res.CountryCode;
                                    iRS.TargetSupplierCountryName = res.CountryName;
                                    iRS.Status = "Mapped";
                                }
                                else
                                {
                                    iRS.TargetSupplierCountryCode = string.Empty;
                                    iRS.TargetSupplierCountryName = string.Empty;
                                    iRS.Status = "No Results Found";
                                }
                            }
                            else
                            {
                                var res = await collection.Find(x => (x.SupplierCode.ToLower() == iRQ.SourceSupplierCode.ToLower() && x.SupplierCountryCode.ToLower() == iRQ.SourceSupplierCountryCode.ToLower())).FirstOrDefaultAsync();
                                if (res != null)
                                {
                                    iRS.TargetSupplierCountryCode = res.SupplierCountryCode;
                                    iRS.TargetSupplierCountryName = res.SupplierCountryName;
                                    iRS.Status = "Mapped";
                                }
                                else
                                {
                                    iRS.TargetSupplierCountryCode = string.Empty;
                                    iRS.TargetSupplierCountryName = string.Empty;
                                    iRS.Status = "No Results Found";
                                }
                            }
                        }
                        else if (iRQ.SourceSupplierCode.ToUpper() != iRQ.TargetSupplierCode.ToUpper() && iRQ.SourceSupplierCode.ToUpper() == "TLGX")
                        {
                            var res = await collection.Find(x => (x.SupplierCode.ToLower() == iRQ.TargetSupplierCode && x.CountryCode.ToLower() == iRQ.SourceSupplierCountryCode.ToLower())).FirstOrDefaultAsync();
                            if (res != null)
                            {
                                iRS.TargetSupplierCountryCode = res.SupplierCountryCode;
                                iRS.TargetSupplierCountryName = res.SupplierCountryName;
                                iRS.Status = "Mapped";
                            }
                            else
                            {
                                iRS.TargetSupplierCountryCode = string.Empty;
                                iRS.TargetSupplierCountryName = string.Empty;
                                iRS.Status = "No Results Found";
                            }
                        }
                        else if (iRQ.SourceSupplierCode.ToUpper() != iRQ.TargetSupplierCode.ToUpper() && iRQ.TargetSupplierCode.ToUpper() == "TLGX")
                        {
                            var res = await collection.Find(x => (x.SupplierCode.ToLower() == iRQ.SourceSupplierCode && x.SupplierCountryCode.ToLower() == iRQ.SourceSupplierCountryCode.ToLower())).FirstOrDefaultAsync();
                            if (res != null)
                            {
                                iRS.TargetSupplierCountryCode = res.CountryCode;
                                iRS.TargetSupplierCountryName = res.CountryName;
                                iRS.Status = "Mapped";
                            }
                            else
                            {
                                iRS.TargetSupplierCountryCode = string.Empty;
                                iRS.TargetSupplierCountryName = string.Empty;
                                iRS.Status = "No Results Found";
                            }
                        }
                        else if (iRQ.SourceSupplierCode.ToUpper() != iRQ.TargetSupplierCode.ToUpper() && iRQ.SourceSupplierCode.ToUpper() != "TLGX" && iRQ.TargetSupplierCode.ToUpper() != "TLGX")
                        {
                            var resultForSource = await collection.Find(x => (x.SupplierCode.ToLower() == iRQ.SourceSupplierCode.ToLower() && x.SupplierCountryCode.ToLower() == iRQ.SourceSupplierCountryCode.ToLower())).FirstOrDefaultAsync();

                            if (resultForSource == null)
                            {
                                iRS.TargetSupplierCountryCode = string.Empty;
                                iRS.TargetSupplierCountryName = string.Empty;
                                iRS.Status = "No Results Found";
                            }
                            else
                            {
                                var resultForTarget = await collection.Find(x => (x.SupplierCode.ToLower() == iRQ.TargetSupplierCode.ToLower() && x.CountryCode.ToLower() == resultForSource.CountryCode.ToLower())).FirstOrDefaultAsync();
                                if (resultForTarget == null)
                                {
                                    iRS.TargetSupplierCountryCode = string.Empty;
                                    iRS.TargetSupplierCountryName = string.Empty;
                                    iRS.Status = "No Results Found";
                                }
                                else
                                {

                                    iRS.TargetSupplierCountryCode = resultForTarget.SupplierCountryCode;
                                    iRS.TargetSupplierCountryName = resultForTarget.SupplierCountryName;
                                    iRS.Status = "Mapped";
                                }
                            }
                        }
                    }

                    RS.Add(iRS);
                }

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, RS);
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