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
    /// Used to retrieve Supplier Country Mapping Data. 
    /// </summary>

    [RoutePrefix("Mapping")]
    public class CountryMappingController : ApiController
    {
        /// <summary>
        /// Mongo database handler
        /// </summary>
        protected static IMongoDatabase _database;

        /// <summary>
        /// Retrieves Supplier Country Mapping for Mapping System Country Name and Supplier System Name.
        /// </summary>
        /// <param name="CountryName">Mapping System Country Name</param>
        /// <param name="SupplierName">Supplier System Name</param>
        /// <returns>Supplier Country Mapping for System Country Name</returns>
        [HttpGet]
        [Route("System/Country/CountryName/{CountryName}/SupplierName/{SupplierName}")]
        [ResponseType(typeof(CountryMapping_RS))]
        public async Task<HttpResponseMessage> GetSupplierCountryMappingByName(string CountryName, string SupplierName)
        {
            _database = MongoDBHandler.mDatabase();
            var collection = _database.GetCollection<CountryMapping>("CountryMapping");
            FilterDefinition<CountryMapping> filter;
            filter = Builders<CountryMapping>.Filter.Empty;

            filter = filter & Builders<CountryMapping>.Filter.Eq(x => x.CountryName, CountryName.Trim().ToUpper());
            filter = filter & Builders<CountryMapping>.Filter.Eq(x => x.SupplierName, SupplierName.Trim().ToUpper());

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

        /// <summary>
        /// Retrieves Supplier Country Mapping for using Mapping System Country Code
        /// </summary>
        /// <param name="CountryCode">Mapping System Country Code</param>
        /// <param name="SupplierCode">Mapping System End Supplier Code</param>
        /// <returns>Mapping System Supplier Country Mapping for Supplier Country Code</returns>
        [HttpGet]
        [Route("System/Country/CountryCode/{CountryCode}/SupplierCode/{SupplierCode}")]
        [ResponseType(typeof(CountryMapping_RS))]
        public async Task<HttpResponseMessage> GetSupplierCountryMappingByCode(string CountryCode, string SupplierCode)
        {
            _database = MongoDBHandler.mDatabase();
            var collection = _database.GetCollection<CountryMapping>("CountryMapping");
            FilterDefinition<CountryMapping> filter;
            filter = Builders<CountryMapping>.Filter.Empty;

            filter = filter & Builders<CountryMapping>.Filter.Eq(x => x.CountryCode, CountryCode.Trim().ToUpper());
            filter = filter & Builders<CountryMapping>.Filter.Eq(x => x.SupplierCode, SupplierCode.Trim().ToUpper());

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


        /// <summary>
        /// Retrieves All Supplier Countries mapped in the Mapping System for a specific Mapping System Country
        /// </summary>
        /// <param name="CountryCode">Mapping System Country Code</param>
        /// <returns>All Supplier Country Mapping</returns>
        [HttpGet]
        [Route("System/Country/CountryCode/{CountryCode}")]
        [ResponseType(typeof(SystemCountryMapping_RS))]
        public async Task<HttpResponseMessage> GetAllSupplierCountryMappingByCode(string CountryCode)
        {
            _database = MongoDBHandler.mDatabase();
            var collection = _database.GetCollection<CountryMapping>("CountryMapping");
            FilterDefinition<CountryMapping> filter;
            filter = Builders<CountryMapping>.Filter.Empty;

            filter = filter & Builders<CountryMapping>.Filter.Eq(x => x.CountryCode, CountryCode.Trim().ToUpper());

            var searchResult = await collection.Find(filter)
                                .Project(x => new SystemCountryMapping_RS
                                {
                                    SupplierCode = x.SupplierCode,
                                    SupplierCountryCode = x.SupplierCountryCode,
                                    MapId = x.MapId
                                })
                                .ToListAsync();

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, searchResult);
            return response;
        }


        /// <summary>
        /// Retrieves Mapping System Country Code and Name for Supplier using Country Code
        /// </summary>
        /// <param name="CountryCode">Supplier-specific Country Code</param>
        /// <param name="SupplierCode">Mapping System Supplier System Code</param>
        /// <returns>TLGX Country Master</returns>
        [HttpGet]
        [Route("Supplier/Country/SupplierCountryCode/{CountryCode}/SupplierCode/{SupplierCode}")]
        [ResponseType(typeof(CountryMapping_RS))]
        public async Task<HttpResponseMessage> GetSystemCountryMappingByCode(string CountryCode, string SupplierCode)
        {
            _database = MongoDBHandler.mDatabase();
            var collection = _database.GetCollection<CountryMapping>("CountryMapping");
            FilterDefinition<CountryMapping> filter;
            filter = Builders<CountryMapping>.Filter.Empty;

            filter = filter & Builders<CountryMapping>.Filter.Eq(x => x.SupplierCountryCode, CountryCode.Trim().ToUpper());
            filter = filter & Builders<CountryMapping>.Filter.Eq(x => x.SupplierCode, SupplierCode.Trim().ToUpper());

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

        /// <summary>
        /// Retrieves Mapping System Country Code and Name using Supplier Country Name
        /// </summary>
        /// <param name="CountryName">Supplier-specific Country Name</param>
        /// <param name="SupplierName">Mapping System Supplier Name</param>
        /// <returns>TLGX Country Master</returns>
        [HttpGet]
        [Route("Supplier/Country/SupplierCountryName/{CountryName}/SupplierName/{SupplierName}")]
        [ResponseType(typeof(CountryMapping_RS))]
        public async Task<HttpResponseMessage> GetSystemCountryMappingByName(string CountryName, string SupplierName)
        {
            _database = MongoDBHandler.mDatabase();
            var collection = _database.GetCollection<CountryMapping>("CountryMapping");
            FilterDefinition<CountryMapping> filter;
            filter = Builders<CountryMapping>.Filter.Empty;

            filter = filter & Builders<CountryMapping>.Filter.Eq(x => x.SupplierCountryName, CountryName.Trim().ToUpper());
            filter = filter & Builders<CountryMapping>.Filter.Eq(x => x.SupplierName, SupplierName.Trim().ToUpper());

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

        /// <summary>
        /// Retrieves cross-supplier and system mapping for Country. 
        /// This would be used where you would want to convert a GTA Country into a Hotel Beds Country
        /// </summary>
        /// <param name="RQ"></param>
        /// <returns>Target supplier Country Name and Country Code</returns>
        [HttpPost]
        [Route("CrossSupplier/Country")]
        [ResponseType(typeof(List<CrossSupplierCountryMapping_RS>))]
        public async Task<HttpResponseMessage> GetCrossSupplierCountryMapping(List<CrossSupplierCountryMapping_RQ> RQ)
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
                        if (iRQ.SourceSupplierCode.ToUpper() == "TLGX")// change me to SYSTEM
                        {
                            var res = await collection.Find(x => (x.CountryCode == iRQ.SourceSupplierCountryCode.Trim().ToUpper())).FirstOrDefaultAsync();
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
                            var res = await collection.Find(x => (x.SupplierCode == iRQ.SourceSupplierCode.Trim().ToUpper() && x.SupplierCountryCode == iRQ.SourceSupplierCountryCode.Trim().ToUpper())).FirstOrDefaultAsync();
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
                        var res = await collection.Find(x => (x.SupplierCode == iRQ.TargetSupplierCode.Trim().ToUpper() && x.CountryCode == iRQ.SourceSupplierCountryCode.Trim().ToUpper())).FirstOrDefaultAsync();
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
                        var res = await collection.Find(x => (x.SupplierCode == iRQ.SourceSupplierCode.Trim().ToUpper() && x.SupplierCountryCode == iRQ.SourceSupplierCountryCode.Trim().ToUpper())).FirstOrDefaultAsync();
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
                        var resultForSource = await collection.Find(x => (x.SupplierCode == iRQ.SourceSupplierCode.Trim().ToUpper() && x.SupplierCountryCode == iRQ.SourceSupplierCountryCode.Trim().ToUpper())).FirstOrDefaultAsync();

                        if (resultForSource == null)
                        {
                            iRS.TargetSupplierCountryCode = string.Empty;
                            iRS.TargetSupplierCountryName = string.Empty;
                            iRS.Status = "No Results Found";
                        }
                        else
                        {
                            var resultForTarget = await collection.Find(x => (x.SupplierCode == iRQ.TargetSupplierCode.Trim().ToUpper() && x.CountryCode == resultForSource.CountryCode.Trim().ToUpper())).FirstOrDefaultAsync();
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

    }
}