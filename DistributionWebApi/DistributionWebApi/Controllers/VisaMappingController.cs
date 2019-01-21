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
        /// This will return all master key value pair related to Visa
        /// </summary>
        /// <param name="CountryCode"></param>
        /// <returns>A key value pair of Visa master attribute type and attribute values</returns>
        [Route("ByCode")]
        [HttpGet]
        [ResponseType(typeof(VisaDefinition2))]
        public async Task<HttpResponseMessage> GetVisaDetailByCounttyCode(string CountryCode)
        {
            try
            {
                _database = MongoDBHandler.mDatabase();
                IMongoCollection<BsonDocument> collectionVisa = _database.GetCollection<BsonDocument>("VisaCountryDetail");


                FilterDefinition<BsonDocument> filter;
                filter = Builders<BsonDocument>.Filter.Empty;

                filter = filter & Builders<BsonDocument>.Filter.Regex("VisaDetail.CountryCode", new BsonRegularExpression(new Regex(CountryCode.Trim(), RegexOptions.IgnoreCase)));

                ProjectionDefinition<BsonDocument> project = Builders<BsonDocument>.Projection.Include("SupplierCode");

                project = project.Exclude("_id");
                project = project.Include("SupplierName");
                project = project.Include("CallType");
                project = project.Include("VisaDetail");

                var searchResult = await collectionVisa.Find(filter).Project(project).FirstOrDefaultAsync();
                var JsonObject = searchResult.ToJson();

                JObject VisaJson = JObject.Parse(JsonObject);

                VisaDefinition2 objVisaDefinition2 = new VisaDefinition2();

                objVisaDefinition2.SupplierCode = searchResult["SupplierCode"].AsString;
                objVisaDefinition2.CallType = searchResult["CallType"].AsString;
                objVisaDefinition2.SupplierName = searchResult["SupplierName"].AsString;
                var strVisaDetail = searchResult["VisaDetail"] as BsonDocument;

                if (!String.IsNullOrEmpty(Convert.ToString(strVisaDetail.ToJson())))
                {
                    objVisaDefinition2.VisaDetail = new List<Models.VisaDetail>();
                    DistributionWebApi.Models.VisaDetail objVisaDetail = new Models.VisaDetail();

                    JObject JobjectVisaDetail = JObject.Parse(Convert.ToString(strVisaDetail.ToJson()));

                    objVisaDetail.CountryCode = (string)VisaJson["VisaDetail"]["CountryCode"];
                    objVisaDetail.CountryName = (string)VisaJson["VisaDetail"]["CountryName"];
                    #region Visa
                    objVisaDetail.Visa = new List<Visa>();

                    Visa objVisa = new Visa();

                    objVisa.AdditionalInfo = (string)VisaJson["VisaDetail"]["Visa"]["AdditionalInfo"];
                    var totalVisaInformationNodes = VisaJson["VisaDetail"]["Visa"]["VisaInformation"].ToList().Count;

                    if (totalVisaInformationNodes > 0)
                    {
                        objVisa.VisaInformation = new List<Models.VisaInformation>();
                        for (int i = 0; i < totalVisaInformationNodes; i++)
                        {
                            DistributionWebApi.Models.VisaInformation objVisaInformationNew = new Models.VisaInformation();
                            objVisaInformationNew.TerritoryCity = (string)VisaJson["VisaDetail"]["Visa"]["VisaInformation"][i]["TerritoryCity"];

                            // VisaInfo Object fill
                            if (VisaJson["VisaDetail"]["Visa"]["VisaInformation"][i]["VisaInfo"].ToList().Count > 0)
                            {
                                objVisaInformationNew.VisaInfo = new List<Models.VisaInfo>();
                                DistributionWebApi.Models.VisaInfo objVisaInfoInew = new Models.VisaInfo();
                                if (VisaJson["VisaDetail"]["Visa"]["VisaInformation"][i]["VisaInfo"].ToArray().Length > 0)
                                {
                                    int TotalChildVisaNodes = VisaJson["VisaDetail"]["Visa"]["VisaInformation"][i]["VisaInfo"].ToArray().Length;
                                    objVisaInfoInew.VisaInformation = new List<Models.VisaInformation2>();
                                    objVisaInfoInew.VisaGeneralInformation = new List<Models.VisaGeneralInformation>();
                                    
                                    DistributionWebApi.Models.VisaGeneralInformation objVisaGeneralInformationNew = new Models.VisaGeneralInformation();
                                    objVisaGeneralInformationNew.GeneralInfo = (string)VisaJson["VisaDetail"]["Visa"]["VisaInformation"][i]["VisaInfo"]["VisaGeneralInformation"]["GeneralInfo"];


                                    objVisaInfoInew.VisaGeneralInformation.Add(objVisaGeneralInformationNew);
                                    
                                    DistributionWebApi.Models.VisaInformation2 objVisaInformation2New = new Models.VisaInformation2();
                                    objVisaInformation2New.Information = (string)VisaJson["VisaDetail"]["Visa"]["VisaInformation"][i]["VisaInfo"]["VisaInformation"]["Information"];


                                    objVisaInfoInew.VisaInformation.Add(objVisaInformation2New);
                                 

                                }

                                #region categories

                                if (VisaJson["VisaDetail"]["Visa"]["VisaInformation"][i]["Categories"]["Category"].ToList().Count > 0)
                                {
                                    int totalCategoriesCount = VisaJson["VisaDetail"]["Visa"]["VisaInformation"][i]["Categories"]["Category"].ToList().Count;
                                    objVisaInformationNew.Categories = new List<VisaCategories>();
                                    objVisaInformationNew.Categories.Add(new VisaCategories());
                                    objVisaInformationNew.Categories[0].Category = new List<Models.VisaCategoryDetail>();
                                    var TypeOfCategory = VisaJson["VisaDetail"]["Visa"]["VisaInformation"][i]["Categories"]["Category"].GetType();
                                    if (TypeOfCategory.Name.ToUpper() == "JARRAY")
                                    {
                                        for (int m = 0; m < totalCategoriesCount; m++)
                                        {
                                            DistributionWebApi.Models.VisaCategoryDetail objVisaCategoryDetailNew = new Models.VisaCategoryDetail();
                                            objVisaCategoryDetailNew.CategoryInfo = new List<Models.VisaCategoryInfo>();
                                            objVisaCategoryDetailNew.CategoryInfo.Add(new Models.VisaCategoryInfo());
                                            objVisaCategoryDetailNew.CategoryInfo[0].Information = new List<VisaInformationChildNode>();

                                            objVisaCategoryDetailNew.CategoryCode = (string)VisaJson["VisaDetail"]["Visa"]["VisaInformation"][i]["Categories"]["Category"][m]["CategoryCode"];
                                            objVisaCategoryDetailNew.CategoryCode = (string)VisaJson["VisaDetail"]["Visa"]["VisaInformation"][i]["Categories"]["Category"][m]["Category"];
                                            objVisaInformationNew.Categories[0].Category.Add(objVisaCategoryDetailNew); 
                                        }
                                    }
                                    else
                                    {
                                        DistributionWebApi.Models.VisaCategoryDetail objVisaCategoryDetailNew = new Models.VisaCategoryDetail();
                                        // it is object

                                        objVisaCategoryDetailNew.CategoryCode = (string)VisaJson["VisaDetail"]["Visa"]["VisaInformation"][i]["Categories"]["Category"]["CategoryCode"];
                                        objVisaCategoryDetailNew.Category = (string)VisaJson["VisaDetail"]["Visa"]["VisaInformation"][i]["Categories"]["Category"]["Category"];

                                        objVisaInformationNew.Categories[0].Category.Add(objVisaCategoryDetailNew);
                                    }


                                   

                                   
                                }

                                #endregion



                                objVisaInformationNew.VisaInfo.Add(objVisaInfoInew);
                            }





                            objVisa.VisaInformation.Add(objVisaInformationNew);
                        }




                    }

                    //objVisa.AdditionalInfo = (string)JobjectVisaDetail["AdditionalInfo"];
                    objVisa.CountryCode = (string)JobjectVisaDetail["CountryCode"];
                    objVisa.CountryDetails = new List<Models.VisaCountryDetails>();
                    objVisa.DiplomaticRepresentation = new List<VisaDiplomaticRepresentation>();
                    objVisa.IndianEmbassy = new List<VisaIndianEmbassy>();
                    objVisa.InternationalAdvisory = new List<Models.VisaInternationalAdvisory>();
                    objVisa.IntlHelpAddress = new Models.VisaIntlHelpAddress();
                    objVisa.IVSAdvisory = new List<Models.VisaIVSAdvisory>();
                    objVisa.ReciprocalVisaInfo = new List<Models.ReciprocalVisaInfo>();
                    objVisa.SAARCInfo = new List<Models.VisaSAARCInfo>();








                    objVisaDetail.Visa.Add(objVisa);                    //}

                    #endregion





                    objVisaDefinition2.VisaDetail.Add(objVisaDetail);
                }








                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, objVisaDefinition2);
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
