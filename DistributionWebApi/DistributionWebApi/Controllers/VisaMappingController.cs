using DistributionWebApi.Models;
using DistributionWebApi.Models.VisaMapping;
using DistributionWebApi.Mongo;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using MongoDB.Bson;
using MongoDB.Driver;


namespace DistributionWebApi.Controllers
{
    [RoutePrefix("VisaMapping/Get")]
    public class VisaMappingController : ApiController
    {
        /// <summary>
        /// static object of Mongo DB Class
        /// </summary>
        protected static IMongoDatabase _database;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// This will return all master key value pair related to Visa
        /// </summary>
        /// <param name="CountryCode"></param>
        /// <returns>A key value pair of Visa master attribute type and attribute values</returns>
        //[Route("ByCountryCode")]
        //[HttpGet]
        //[ResponseType(typeof(VisaDefinition))]
        //public async Task<HttpResponseMessage> GetVisaDetailByCountries(string CountryCode)
        //{
        //    try
        //    {
        //        _database = MongoDBHandler.mDatabase();
        //        string[] arrayOfStrings;

        //        IMongoCollection<BsonDocument> collectionVisa = _database.GetCollection<BsonDocument>("VisaCountryDetail");

        //        FilterDefinition<BsonDocument> filterCountry;
        //        filterCountry = Builders<BsonDocument>.Filter.Empty;
        //        filterCountry = filterCountry & Builders<BsonDocument>.Filter.AnyIn("CountryCode", CountryCode);
        //        ProjectionDefinition<BsonDocument> projectCountry = Builders<BsonDocument>.Projection.Include("SupplierCode").Exclude("_id");

        //        var searchCountryResult = await collectionVisa.Find(filterCountry).Project(projectCountry).ToListAsync();
        //        arrayOfStrings = searchCountryResult.Select(s => s["CountryCode"].AsString).ToArray();

        //        VisaDefinition2 objVisa = new VisaDefinition2();

        //        var searchResult = await collectionVisa.Find(s => true).FirstOrDefaultAsync();              

        //        var str = searchResult.ToJson();




        //        //var searchResult = await collectionVisa.Find(s => s.VisaDetail.Where(x => x.CountryCode == CountryCode).FirstOrDefaultAsync();
        //        //var searchResult = await collectionVisa.Find(s => s.VisaDetail.FirstOrDefault(x => x.CountryCode == CountryCode);

        //        HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, searchResult);
        //        return response;

        //    }
        //    catch (Exception ex)
        //    {
        //        NLogHelper.Nlogger_LogError.LogError(ex, this.GetType().FullName, Request.GetActionDescriptor().ActionName, Request.RequestUri.PathAndQuery);
        //        HttpResponseMessage response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Server Error. Contact Admin. Error Date : " + DateTime.Now.ToString());
        //        return response;
        //    }
        //}


        /// <summary>
        /// This will return all Visa details as per country code
        /// </summary>
        /// <param name="CountryCode"></param>
        /// <returns>A key value pair of Visa master attribute type and attribute values</returns>
        [Route("GetVisaDetailsByCountryCode")]
        [HttpGet]
        [ResponseType(typeof(DistributionWebApi.Models.VisaDefinition))]
        public async Task<HttpResponseMessage> GetVisaDetailByCountryCode(string CountryCode)
        {
            try
            {
                _database = MongoDBHandler.mDatabase();
                IMongoCollection<DistributionWebApi.Models.VisaDefinition> collectionVisa = _database.GetCollection<DistributionWebApi.Models.VisaDefinition>("VisaMapping");

                var filter = Builders<DistributionWebApi.Models.VisaDefinition>.Filter.ElemMatch(x => x.VisaDetail, x => x.CountryCode.ToLower() == CountryCode.ToLower());
                var searchResult = await collectionVisa.Find(filter).FirstOrDefaultAsync();

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
        /// This will return single Visa detail  key value pair related to CountryCode
        /// </summary>
        /// <returns>A key value pair of Visa master attribute type and attribute values</returns>
        //[Route("All")]
        //[HttpGet]
        //[ResponseType(typeof(List<VisaDefinition>))]
        //public async Task<HttpResponseMessage> GetVisaMasters()
        //{
        //    try
        //    {

        //        _database = MongoDBHandler.mDatabase();

        //        IMongoCollection<BsonDocument> collectionVisa = _database.GetCollection<BsonDocument>("VisaCountryDetail");

        //        var searchResult = await collectionVisa.Find(s => true).FirstOrDefaultAsync();
        //        VisaDefinition2 objVisa = new VisaDefinition2();

        //        var str = searchResult.ToJson();

        //        ProjectionDefinition<BsonDocument> collectionVisa = Builders<BsonDocument>.Projection.Include("CountryCode").Exclude("_id");

        //        var searchCountryResult =  await collectionVisa.Find(filterCountry).Project(projectCountry).ToListAsync();



        //        JObject json = JObject.Parse(str);
        //        dynamic json2 = JsonConvert.DeserializeObject(str);
        //        //List<VisaDefinition2> list = JsonConvert.DeserializeObject<List<VisaDefinition2>>(str);





        //        HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, searchResult);
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
