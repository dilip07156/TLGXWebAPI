﻿using System;
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
    /// Used to retrieve System City Mapping data for specific City and Supplier pairs. 
    /// </summary>

    [RoutePrefix("Mapping")]
    public class CityMappingController : ApiController
    {
        /// <summary>
        /// Mongo database handler
        /// </summary>
        protected static IMongoDatabase _database;

        /// <summary>
        /// Retrieves Supplier City Mapping for System Country Name, City Name and Supplier Name
        /// </summary>
        /// <param name="CountryName">System Country Name</param>
        /// <param name="CityName">System City Name</param>
        /// <param name="SupplierName">System Supplier Name</param>
        /// <returns>Supplier City Mapping</returns>
        [HttpGet]
        [Route("System/City/CountryName/{CountryName}/CityName/{CityName}/SupplierName/{SupplierName}")]
        [ResponseType(typeof(CityMapping_RS))]
        public async Task<HttpResponseMessage> GetSupplierCityMappingByName(string CountryName, string CityName, string SupplierName)
        {
            return await GetCityMapping(CountryName, string.Empty, CityName, string.Empty, SupplierName, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
        }

        /// <summary>
        /// Retrieves System City Mapping for System City Code and Supplier Code
        /// </summary>
        /// <param name="CityCode">System City Code</param>
        /// <param name="SupplierCode">Supplier Code</param>
        /// <returns>System City Mapping</returns>
        [HttpGet]
        [Route("System/City/CityCode/{CityCode}/SupplierCode/{SupplierCode}")]
        [ResponseType(typeof(CityMapping_RS))]
        public async Task<HttpResponseMessage> GetSupplierCityMappingByCode(string CityCode, string SupplierCode)
        {
            return await GetCityMapping(string.Empty, string.Empty, string.Empty, CityCode, string.Empty, SupplierCode, string.Empty, string.Empty, string.Empty, string.Empty);
        }


        /// <summary>
        /// Retrieves All Supplier City Mapping for System City Code
        /// </summary>
        /// <param name="CityCode">System City Code</param>
        /// <returns>All Supplier City Mapping</returns>
        [HttpGet]
        [Route("System/City/CityCode/{CityCode}")]
        [ResponseType(typeof(SystemCityMapping_RS))]
        public async Task<HttpResponseMessage> GetAllSupplierCityMappingByCode(string CityCode)
        {
            _database = MongoDBHandler.mDatabase();
            var collection = _database.GetCollection<CityMapping>("CityMapping");
            FilterDefinition<CityMapping> filter;
            filter = Builders<CityMapping>.Filter.Empty;

            filter = filter & Builders<CityMapping>.Filter.Regex(x => x.CityCode, new BsonRegularExpression(new Regex(CityCode, RegexOptions.IgnoreCase)));

            var searchResult = await collection.Find(filter)
                                .Project(x => new SystemCityMapping_RS
                                {
                                    SupplierCode = x.SupplierCode,
                                    MapId = x.MapId,
                                    SupplierCityCode = x.SupplierCityCode,
                                })
                                .ToListAsync();

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, searchResult);
            return response;
        }

        /// <summary>
        /// Retrieves System City Mapping for Supplier City Code and Supplier Code
        /// </summary>
        /// <param name="CityCode">Supplier City Code</param>
        /// <param name="SupplierCode">Supplier Code</param>
        /// <returns>System City Mapping</returns>
        [HttpGet]
        [Route("Supplier/City/SupplierCityCode/{CityCode}/SupplierCode/{SupplierCode}")]
        [ResponseType(typeof(CityMapping_RS))]
        public async Task<HttpResponseMessage> GetSystemCityMappingByCode(string CityCode, string SupplierCode)
        {
            return await GetCityMapping(string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                SupplierCode,
                string.Empty,
                string.Empty,
                string.Empty,
                CityCode);
        }

        /// <summary>
        /// Retrieves System City Mapping for Supplier Country Code, City Code and Supplier Name
        /// </summary>
        /// <param name="CountryName">Supplier Country Name</param>
        /// <param name="CityName">Supplier City Name</param>
        /// <param name="SupplierName">Supplier Name</param>
        /// <returns>System City Mapping</returns>
        [HttpGet]
        [Route("Supplier/City/SupplierCountryName/{CountryName}/SupplierCityName/{CityName}/SupplierCode/{SupplierName}")]
        [ResponseType(typeof(CityMapping_RS))]
        public async Task<HttpResponseMessage> GetSystemCityMappingByName(string CountryName, string CityName, string SupplierName)
        {
            return await GetCityMapping(string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                SupplierName,
                string.Empty,
                CountryName,
                string.Empty,
                CityName,
                string.Empty);
        }

        /// <summary>
        /// Generic City Mapping result
        /// </summary>
        /// <param name="CountryName"></param>
        /// <param name="CountryCode"></param>
        /// <param name="CityName"></param>
        /// <param name="CityCode"></param>
        /// <param name="SupplierName"></param>
        /// <param name="SupplierCode"></param>
        /// <param name="SupplierCountryName"></param>
        /// <param name="SupplierCountryCode"></param>
        /// <param name="SupplierCityName"></param>
        /// <param name="SupplierCityCode"></param>
        /// <returns>List of CityMapping</returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [ResponseType(typeof(List<CityMapping_RS>))]
        public async Task<HttpResponseMessage> GetCityMapping(string CountryName, string CountryCode, string CityName, string CityCode, string SupplierName, string SupplierCode, string SupplierCountryName, string SupplierCountryCode, string SupplierCityName, string SupplierCityCode)
        {
            _database = MongoDBHandler.mDatabase();
            var collection = _database.GetCollection<CityMapping>("CityMapping");
            FilterDefinition<CityMapping> filter;
            filter = Builders<CityMapping>.Filter.Empty;

            if (!string.IsNullOrWhiteSpace(CountryName))
            {
                //filter = filter & Builders<CityMapping>.Filter.Regex(x => x.CountryName, new BsonRegularExpression(new Regex(CountryName.Trim(), RegexOptions.IgnoreCase)));
                filter = filter & Builders<CityMapping>.Filter.Eq(x => x.CountryName, CountryName.Trim().ToUpper());
            }

            if (!string.IsNullOrWhiteSpace(CountryCode))
            {
                //filter = filter & Builders<CityMapping>.Filter.Regex(x => x.CountryCode, new BsonRegularExpression(new Regex(CountryCode.Trim(), RegexOptions.IgnoreCase)));
                filter = filter & Builders<CityMapping>.Filter.Eq(x => x.CountryCode, CountryCode.Trim().ToUpper());
            }

            if (!string.IsNullOrWhiteSpace(CityName))
            {
                //filter = filter & Builders<CityMapping>.Filter.Regex(x => x.CityName, new BsonRegularExpression(new Regex(CityName.Trim(), RegexOptions.IgnoreCase)));
                filter = filter & Builders<CityMapping>.Filter.Eq(x => x.CityName, CityName.Trim().ToUpper());
            }

            if (!string.IsNullOrWhiteSpace(CityCode))
            {
                //filter = filter & Builders<CityMapping>.Filter.Regex(x => x.CityCode, new BsonRegularExpression(new Regex(CityCode.Trim(), RegexOptions.IgnoreCase)));
                filter = filter & Builders<CityMapping>.Filter.Eq(x => x.CityCode, CityCode.Trim().ToUpper());
            }

            if (!string.IsNullOrWhiteSpace(SupplierName))
            {
                //filter = filter & Builders<CityMapping>.Filter.Regex(x => x.SupplierName, new BsonRegularExpression(new Regex(SupplierName.Trim(), RegexOptions.IgnoreCase)));
                filter = filter & Builders<CityMapping>.Filter.Eq(x => x.SupplierName, SupplierName.Trim().ToUpper());
            }

            if (!string.IsNullOrWhiteSpace(SupplierCode))
            {
                //filter = filter & Builders<CityMapping>.Filter.Regex(x => x.SupplierCode, new BsonRegularExpression(new Regex(SupplierCode.Trim(), RegexOptions.IgnoreCase)));
                filter = filter & Builders<CityMapping>.Filter.Eq(x => x.SupplierCode, SupplierCode.Trim().ToUpper());
            }

            if (!string.IsNullOrWhiteSpace(SupplierCountryName))
            {
                //filter = filter & Builders<CityMapping>.Filter.Regex(x => x.SupplierCountryName, new BsonRegularExpression(new Regex(SupplierCountryName.Trim(), RegexOptions.IgnoreCase)));
                filter = filter & Builders<CityMapping>.Filter.Eq(x => x.SupplierCountryName, SupplierCountryName.Trim().ToUpper());
            }

            if (!string.IsNullOrWhiteSpace(SupplierCountryCode))
            {
                //filter = filter & Builders<CityMapping>.Filter.Regex(x => x.SupplierCountryCode, new BsonRegularExpression(new Regex(SupplierCountryCode.Trim(), RegexOptions.IgnoreCase)));
                filter = filter & Builders<CityMapping>.Filter.Eq(x => x.SupplierCountryCode, SupplierCountryCode.Trim().ToUpper());
            }

            if (!string.IsNullOrWhiteSpace(SupplierCityName))
            {
                //filter = filter & Builders<CityMapping>.Filter.Regex(x => x.SupplierCityName, new BsonRegularExpression(new Regex(SupplierCityName.Trim(), RegexOptions.IgnoreCase)));
                filter = filter & Builders<CityMapping>.Filter.Eq(x => x.SupplierCityName, SupplierCityName.Trim().ToUpper());
            }

            if (!string.IsNullOrWhiteSpace(SupplierCityCode))
            {
                //filter = filter & Builders<CityMapping>.Filter.Regex(x => x.SupplierCityCode, new BsonRegularExpression(new Regex(SupplierCityCode.Trim(), RegexOptions.IgnoreCase)));
                filter = filter & Builders<CityMapping>.Filter.Eq(x => x.SupplierCityCode, SupplierCityCode.Trim().ToUpper());
            }

            var searchResult = await collection.Find(filter)
                                .Project(x => new CityMapping_RS
                                {
                                    CountryName = x.CountryName,
                                    CountryCode = x.CountryCode,
                                    SupplierName = x.SupplierName,
                                    SupplierCode = x.SupplierCode,
                                    SupplierCountryCode = x.SupplierCountryCode,
                                    SupplierCountryName = x.SupplierCountryName,
                                    MapId = x.MapId,
                                    CityCode = x.CityCode,
                                    CityName = x.CityName,
                                    SupplierCityCode = x.SupplierCityCode,
                                    SupplierCityName = x.SupplierCityName
                                })
                                .ToListAsync();

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, searchResult);
            return response;
        }

        /// <summary>
        /// Retrieves City Mapping for a Supplier against a specific supplier
        /// </summary>
        /// <param name="RQ"></param>
        /// <returns>Target supplier City Name and City Code</returns>
        [HttpPost]
        [Route("City/CrossSupplierMapping")]
        [ResponseType(typeof(List<CrossSupplierCityMapping_RS>))]
        public async Task<HttpResponseMessage> GetCrossSupplierCityMapping(List<CrossSupplierCityMapping_RQ> RQ)
        {
            List<CrossSupplierCityMapping_RS> RS = new List<CrossSupplierCityMapping_RS>();

            _database = MongoDBHandler.mDatabase();
            var collection = _database.GetCollection<CityMapping>("CityMapping");

            foreach (var iRQ in RQ)
            {
                CrossSupplierCityMapping_RS iRS = new CrossSupplierCityMapping_RS();
                iRS.SourceSupplierCode = iRQ.SourceSupplierCode;
                iRS.SourceSupplierCountryCode = iRQ.SourceSupplierCountryCode;
                iRS.SourceSupplierCityCode = iRQ.SourceSupplierCityCode;
                iRS.TargetSupplierCode = iRQ.TargetSupplierCode;

                if (iRS.SourceSupplierCode == null || iRS.SourceSupplierCountryCode == null || iRS.SourceSupplierCityCode == null || iRS.TargetSupplierCode == null)
                {
                    iRS.Status = "Bad Request";
                }
                else
                {
                    if (iRQ.SourceSupplierCode.ToUpper() == iRQ.TargetSupplierCode.ToUpper())
                    {
                        if (iRQ.SourceSupplierCode.ToUpper() == "TLGX")
                        {
                            var res = await collection.Find(x => (x.CountryCode.ToLower() == iRQ.SourceSupplierCountryCode.ToLower() && x.CityCode.ToLower() == iRQ.SourceSupplierCityCode.ToLower())).FirstOrDefaultAsync();
                            if (res != null)
                            {
                                iRS.TargetSupplierCityCode = res.CityCode;
                                iRS.TargetSupplierCityName = res.CityName;
                                iRS.Status = "Mapped";
                            }
                            else
                            {
                                iRS.TargetSupplierCityCode = string.Empty;
                                iRS.TargetSupplierCityName = string.Empty;
                                iRS.Status = "No Results Found";
                            }
                        }
                        else
                        {
                            var res = await collection.Find(x => (x.SupplierCode.ToLower() == iRQ.SourceSupplierCode.ToLower() && x.SupplierCountryCode.ToLower() == iRQ.SourceSupplierCountryCode.ToLower() && x.SupplierCityCode.ToLower() == iRQ.SourceSupplierCityCode.ToLower())).FirstOrDefaultAsync();
                            if (res != null)
                            {
                                iRS.TargetSupplierCityCode = res.SupplierCityCode;
                                iRS.TargetSupplierCityName = res.SupplierCityName;
                                iRS.Status = "Mapped";
                            }
                            else
                            {
                                iRS.TargetSupplierCityCode = string.Empty;
                                iRS.TargetSupplierCityName = string.Empty;
                                iRS.Status = "No Results Found";
                            }
                        }
                    }
                    else if (iRQ.SourceSupplierCode.ToUpper() != iRQ.TargetSupplierCode.ToUpper() && iRQ.SourceSupplierCode.ToUpper() == "TLGX")
                    {
                        var res = await collection.Find(x => (x.SupplierCode.ToLower() == iRQ.TargetSupplierCode && x.CountryCode.ToLower() == iRQ.SourceSupplierCountryCode.ToLower() && x.CityCode.ToLower() == iRQ.SourceSupplierCityCode.ToLower())).FirstOrDefaultAsync();
                        if (res != null)
                        {
                            iRS.TargetSupplierCityCode = res.SupplierCityCode;
                            iRS.TargetSupplierCityName = res.SupplierCityName;
                            iRS.Status = "Mapped";
                        }
                        else
                        {
                            iRS.TargetSupplierCityCode = string.Empty;
                            iRS.TargetSupplierCityName = string.Empty;
                            iRS.Status = "No Results Found";
                        }
                    }
                    else if (iRQ.SourceSupplierCode.ToUpper() != iRQ.TargetSupplierCode.ToUpper() && iRQ.TargetSupplierCode.ToUpper() == "TLGX")
                    {
                        var res = await collection.Find(x => (x.SupplierCode.ToLower() == iRQ.SourceSupplierCode && x.SupplierCountryCode.ToLower() == iRQ.SourceSupplierCountryCode.ToLower() && x.SupplierCityCode.ToLower() == iRQ.SourceSupplierCityCode.ToLower())).FirstOrDefaultAsync();
                        if (res != null)
                        {
                            iRS.TargetSupplierCityCode = res.CityCode;
                            iRS.TargetSupplierCityName = res.CityName;
                            iRS.Status = "Mapped";
                        }
                        else
                        {
                            iRS.TargetSupplierCityCode = string.Empty;
                            iRS.TargetSupplierCityName = string.Empty;
                            iRS.Status = "No Results Found";
                        }
                    }
                    else if (iRQ.SourceSupplierCode.ToUpper() != iRQ.TargetSupplierCode.ToUpper() && iRQ.SourceSupplierCode.ToUpper() != "TLGX" && iRQ.TargetSupplierCode.ToUpper() != "TLGX")
                    {
                        var resultForSource = await collection.Find(x => (x.SupplierCode.ToLower() == iRQ.SourceSupplierCode.ToLower() && x.SupplierCountryCode.ToLower() == iRQ.SourceSupplierCountryCode.ToLower() && x.SupplierCityCode.ToLower() == iRQ.SourceSupplierCityCode.ToLower())).FirstOrDefaultAsync();

                        if (resultForSource == null)
                        {
                            iRS.TargetSupplierCityCode = string.Empty;
                            iRS.TargetSupplierCityName = string.Empty;
                            iRS.Status = "No Results Found";
                        }
                        else
                        {
                            var resultForTarget = await collection.Find(x => (x.SupplierCode.ToLower() == iRQ.TargetSupplierCode.ToLower() && x.CountryCode.ToLower() == resultForSource.CountryCode.ToLower() && x.CityCode.ToLower() == resultForSource.CityCode.ToLower() && x.CityName.ToLower() == resultForSource.CityName.ToLower())).FirstOrDefaultAsync();
                            if (resultForTarget == null)
                            {
                                iRS.TargetSupplierCityCode = string.Empty;
                                iRS.TargetSupplierCityName = string.Empty;
                                iRS.Status = "No Results Found";
                            }
                            else
                            {

                                iRS.TargetSupplierCityCode = resultForTarget.SupplierCityCode;
                                iRS.TargetSupplierCityName = resultForTarget.SupplierCityName;
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

        /// <summary>
        /// Retrieves all city mappings filtered by supplier code
        /// </summary>
        /// <param name="SupplierCode">System specified supplier code</param>
        /// <returns>List of city mapping response that contains supplier city details and mapped to system city details with map id</returns>
        [HttpPost]
        [Route("City/GetAllBySupplierCode/{SupplierCode}")]
        [ResponseType(typeof(List<CityMapping_RS>))]
        public async Task<HttpResponseMessage> GetAllCityMappingsBySupplierCode(string SupplierCode)
        {
            return await GetCityMapping(string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                SupplierCode,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty);
        }
    }
}
