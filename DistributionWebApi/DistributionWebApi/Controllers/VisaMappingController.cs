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
        [ResponseType(typeof(VisaDefinition2))]
        public async Task<HttpResponseMessage> GetVisaDetailByCountryCode(string CountryCode)
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

                            // fill CategoryForms Object
                            objVisaInformationNew.CategoryForms = new CategoryForms();
                            objVisaInformationNew.CategoryForms.CategoryForm = new List<CategoryForm>();
                            if (VisaJson["VisaDetail"]["Visa"]["VisaInformation"][i]["CategoryForms"].ToList().Count > 0)
                            {
                                var TypeOfCategoryForm = VisaJson["VisaDetail"]["Visa"]["VisaInformation"][i]["CategoryForms"]["CategoryForm"].GetType();
                                int TotalCategoryForms = VisaJson["VisaDetail"]["Visa"]["VisaInformation"][i]["CategoryForms"]["CategoryForm"].ToList().Count;
                                for (int c = 0; c < TotalCategoryForms; c++)
                                {
                                    CategoryForm objCategoryFormNew = new CategoryForm();
                                    objCategoryFormNew.CategoryCode = (string)VisaJson["VisaDetail"]["Visa"]["VisaInformation"][i]["CategoryForms"]["CategoryForm"][c]["CategoryCode"];
                                    objCategoryFormNew.Form = (string)VisaJson["VisaDetail"]["Visa"]["VisaInformation"][i]["CategoryForms"]["CategoryForm"][c]["Form"];
                                    objCategoryFormNew.FormPath = (string)VisaJson["VisaDetail"]["Visa"]["VisaInformation"][i]["CategoryForms"]["CategoryForm"][c]["FormPath"];

                                    objVisaInformationNew.CategoryForms.CategoryForm.Add(objCategoryFormNew);
                                }
                            }


                            #region CategoryFees



                            if (VisaJson["VisaDetail"]["Visa"]["VisaInformation"][i]["CategoryFees"].ToList().Count > 0)
                            {
                                objVisaInformationNew.CategoryFees = new List<VisaCategoryFees>();
                                objVisaInformationNew.CategoryFees.Add(new VisaCategoryFees());
                                objVisaInformationNew.CategoryFees[0].Category = new List<List<VisaCategoryFee>>();



                                var TypeOfCategoryFees = VisaJson["VisaDetail"]["Visa"]["VisaInformation"][i]["CategoryFees"].GetType();

                                if (TypeOfCategoryFees.Name.ToUpper() == "JARRAY")
                                {
                                    int TotalCategoryFees = VisaJson["VisaDetail"]["Visa"]["VisaInformation"][i]["CategoryFees"].ToList().Count;

                                    for (int p = 0; p < TotalCategoryFees; p++)
                                    {
                                        var TypeOfCategoryFee = VisaJson["VisaDetail"]["Visa"]["VisaInformation"][i]["CategoryFees"][p]["CategoryFee"].GetType();
                                        if (TypeOfCategoryFee.Name.ToUpper() == "JARRAY")
                                        {
                                            List<VisaCategoryFee> VisaCategoryfeeList = new List<VisaCategoryFee>();

                                            int TotalCategoryFeesNode = VisaJson["VisaDetail"]["Visa"]["VisaInformation"][i]["CategoryFees"][p]["CategoryFee"].ToList().Count;
                                            for (int g = 0; g < TotalCategoryFeesNode; g++)
                                            {
                                                VisaCategoryfeeList.Add(new VisaCategoryFee
                                                {
                                                    Category = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["VisaInformation"][i]["CategoryFees"][p]["CategoryFee"][g]["Category"]),
                                                    CategoryCode = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["VisaInformation"][i]["CategoryFees"][p]["CategoryFee"][g]["CategoryCode"]),
                                                    CategoryFeeAmountINR = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["VisaInformation"][i]["CategoryFees"][p]["CategoryFee"][g]["CategoryFeeAmountINR"]),
                                                    CategoryFeeAmountOther = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["VisaInformation"][i]["CategoryFees"][p]["CategoryFee"][g]["CategoryFeeAmountOther"])
                                                });
                                            }
                                            objVisaInformationNew.CategoryFees[0].Category.Add(VisaCategoryfeeList);

                                        }
                                        else
                                        {

                                            List<VisaCategoryFee> VisaCategoryfeeList = new List<VisaCategoryFee>();
                                            VisaCategoryfeeList.Add(new VisaCategoryFee
                                            {
                                                Category = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["VisaInformation"][i]["CategoryFees"][p]["CategoryFee"]["Category"]),
                                                CategoryCode = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["VisaInformation"][i]["CategoryFees"][p]["CategoryFee"]["CategoryCode"]),
                                                CategoryFeeAmountINR = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["VisaInformation"][i]["CategoryFees"][p]["CategoryFee"]["CategoryFeeAmountINR"]),
                                                CategoryFeeAmountOther = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["VisaInformation"][i]["CategoryFees"][p]["CategoryFee"]["CategoryFeeAmountOther"])
                                            });

                                            objVisaInformationNew.CategoryFees[0].Category.Add(VisaCategoryfeeList);

                                        }
                                    }
                                }
                                else
                                {
                                    List<VisaCategoryFee> VisaCategoryfeeList = new List<VisaCategoryFee>();
                                    VisaCategoryfeeList.Add(new VisaCategoryFee
                                    {
                                        Category = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["VisaInformation"][i]["CategoryFees"]["CategoryFee"]["Category"]),
                                        CategoryCode = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["VisaInformation"][i]["CategoryFees"]["CategoryFee"]["CategoryCode"]),
                                        CategoryFeeAmountINR = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["VisaInformation"][i]["CategoryFees"]["CategoryFee"]["CategoryFeeAmountINR"]),
                                        CategoryFeeAmountOther = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["VisaInformation"][i]["CategoryFees"]["CategoryFee"]["CategoryFeeAmountOther"])
                                    });

                                    objVisaInformationNew.CategoryFees[0].Category.Add(VisaCategoryfeeList);
                                }
                            }

                            #endregion




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
                                    if (VisaJson["VisaDetail"]["Visa"]["VisaInformation"][i]["VisaInfo"]["VisaGeneralInformation"]["GeneralInfo"].GetType().Name.ToUpper() == "JOBJECT")
                                    {

                                    }
                                    else
                                    {
                                        objVisaGeneralInformationNew.GeneralInfo = (string)VisaJson["VisaDetail"]["Visa"]["VisaInformation"][i]["VisaInfo"]["VisaGeneralInformation"]["GeneralInfo"];
                                    }

                                    objVisaInfoInew.VisaGeneralInformation.Add(objVisaGeneralInformationNew);

                                    DistributionWebApi.Models.VisaInformation2 objVisaInformation2New = new Models.VisaInformation2();


                                    var TypeOfInformation = VisaJson["VisaDetail"]["Visa"]["VisaInformation"][i]["VisaInfo"]["VisaInformation"]["Information"].GetType();
                                    if (TypeOfInformation.Name.ToUpper() == "JOBJECT")
                                    {
                                        if (VisaJson["VisaDetail"]["Visa"]["VisaInformation"][i]["VisaInfo"]["VisaInformation"]["Information"] != null &&
                                            VisaJson["VisaDetail"]["Visa"]["VisaInformation"][i]["VisaInfo"]["VisaInformation"]["Information"]["InformationLink"].ToList().Count > 0)
                                        {
                                            objVisaInformation2New.Information = new Information();
                                            int totalInformationLinks = VisaJson["VisaDetail"]["Visa"]["VisaInformation"][i]["VisaInfo"]["VisaInformation"]["Information"]["InformationLink"].ToList().Count;
                                            objVisaInformation2New.Information.InformationLink = new List<InformationLink>();
                                            for (int l = 0; l < totalInformationLinks; l++)
                                            {
                                                InformationLink objInformationLinkNew = new InformationLink();
                                                objInformationLinkNew.href = (string)VisaJson["VisaDetail"]["Visa"]["VisaInformation"][i]["VisaInfo"]["VisaInformation"]["Information"]["InformationLink"][l]["href"];
                                                objInformationLinkNew.content = (string)VisaJson["VisaDetail"]["Visa"]["VisaInformation"][i]["VisaInfo"]["VisaInformation"]["Information"]["InformationLink"][l]["content"];
                                                objInformationLinkNew.target = (string)VisaJson["VisaDetail"]["Visa"]["VisaInformation"][i]["VisaInfo"]["VisaInformation"]["Information"]["InformationLink"][l]["target"];

                                                objVisaInformation2New.Information.InformationLink.Add(objInformationLinkNew);

                                            }
                                        }
                                    }
                                    else
                                    {
                                        //its a jvalue.
                                        objVisaInformation2New.Information = new Information();
                                        //int totalInformationLinks = VisaJson["VisaDetail"]["Visa"]["VisaInformation"][i]["VisaInfo"]["VisaInformation"]["Information"]["InformationLink"].ToList().Count;
                                        objVisaInformation2New.Information.InformationLink = new List<InformationLink>();
                                        InformationLink objInformationLinkNew = new InformationLink();
                                        objInformationLinkNew.content = (string)VisaJson["VisaDetail"]["Visa"]["VisaInformation"][i]["VisaInfo"]["VisaInformation"]["Information"];
                                        objVisaInformation2New.Information.InformationLink.Add(objInformationLinkNew);

                                    }

                                    //else
                                    //objVisaInformation2New.Information = (string)VisaJson["VisaDetail"]["Visa"]["VisaInformation"][i]["VisaInfo"]["VisaInformation"]["Information"];


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
                                            objVisaCategoryDetailNew.CategoryInfo[0].Information.Add(new VisaInformationChildNode());

                                            objVisaCategoryDetailNew.CategoryCode = (string)VisaJson["VisaDetail"]["Visa"]["VisaInformation"][i]["Categories"]["Category"][m]["CategoryCode"];
                                            objVisaCategoryDetailNew.Category = (string)VisaJson["VisaDetail"]["Visa"]["VisaInformation"][i]["Categories"]["Category"][m]["Category"];
                                            // objVisaCategoryDetailNew.CategoryNotes = (string)VisaJson["VisaDetail"]["Visa"]["VisaInformation"][i]["Categories"]["Category"][m]["CategoryNotes"];



                                            var TypeOfCategoryNotes = VisaJson["VisaDetail"]["Visa"]["VisaInformation"][i]["Categories"]["Category"][m]["CategoryNotes"].GetType();
                                            if (TypeOfCategoryNotes.Name.ToUpper() == "JOBJECT")
                                            {
                                                objVisaCategoryDetailNew.CategoryNotes = new CategoryNotes();
                                                objVisaCategoryDetailNew.CategoryNotes.Notes = new List<string>();

                                                if (VisaJson["VisaDetail"]["Visa"]["VisaInformation"][i]["Categories"]["Category"][m]["CategoryNotes"]["Note"].ToList().Count > 0)
                                                {
                                                    int TotalNotesTag = VisaJson["VisaDetail"]["Visa"]["VisaInformation"][i]["Categories"]["Category"][m]["CategoryNotes"]["Note"].ToList().Count;
                                                    for (int y = 0; y < TotalNotesTag; y++)
                                                    {
                                                        objVisaCategoryDetailNew.CategoryNotes.Notes.Add(Convert.ToString(VisaJson["VisaDetail"]["Visa"]["VisaInformation"][i]["Categories"]["Category"][m]
                                                                                    ["CategoryNotes"][y]["Note"]));
                                                    }

                                                }
                                            }
                                            else
                                            {     // Its Object
                                                objVisaCategoryDetailNew.CategoryNotes = new CategoryNotes();
                                                objVisaCategoryDetailNew.CategoryNotes.Notes = new List<string>();


                                                if (Convert.ToString(VisaJson["VisaDetail"]["Visa"]["VisaInformation"][i]["Categories"]["Category"][m]["CategoryNotes"]) != "")
                                                {

                                                    if (VisaJson["VisaDetail"]["Visa"]["VisaInformation"][i]["Categories"]["Category"][m]["CategoryNotes"].ToList().Count == 0)
                                                    {
                                                        // read it from CategoryNotes Directly
                                                        objVisaCategoryDetailNew.CategoryNotes.Notes.Add(Convert.ToString(VisaJson["VisaDetail"]["Visa"]["VisaInformation"][i]["Categories"]["Category"][m]
                                                                               ["CategoryNotes"]));
                                                    }
                                                    else
                                                    {
                                                        if (VisaJson["VisaDetail"]["Visa"]["VisaInformation"][i]["Categories"]["Category"][m]["CategoryNotes"]["Note"].ToList().Count > 0)
                                                        {
                                                            int TotalNotesTag = VisaJson["VisaDetail"]["Visa"]["VisaInformation"][i]["Categories"]["Category"][m]["CategoryNotes"]["Note"].ToList().Count;
                                                            for (int b = 0; b < TotalNotesTag; b++)
                                                            {
                                                                objVisaCategoryDetailNew.CategoryNotes.Notes.Add(Convert.ToString(VisaJson["VisaDetail"]["Visa"]["VisaInformation"][i]["Categories"]["Category"][m]
                                                                              ["CategoryNotes"]["Note"][b]));
                                                            }

                                                        }
                                                    }
                                                }
                                            }

                                            var TypeOfCategoryRequirements = VisaJson["VisaDetail"]["Visa"]["VisaInformation"][i]["Categories"]["Category"][m]["CategoryRequirements"].GetType();
                                            if (TypeOfCategoryRequirements.Name.ToUpper() == "JARRAY")
                                            {
                                                objVisaCategoryDetailNew.CategoryRequirements = new List<Models.VisaCategoryRequirements>();
                                                objVisaCategoryDetailNew.CategoryRequirements.Add(new Models.VisaCategoryRequirements());
                                                objVisaCategoryDetailNew.CategoryRequirements[0].Requirements = new Requirements();

                                                int totalRequirements = VisaJson["VisaDetail"]["Visa"]["VisaInformation"][i]["Categories"]["Category"][m]["CategoryRequirements"].ToList().Count;

                                                for (int q = 0; q < totalRequirements; q++)
                                                {
                                                    objVisaCategoryDetailNew.CategoryRequirements[0].Requirements.Line = Convert.ToString(
                                                        VisaJson["VisaDetail"]["Visa"]["VisaInformation"][i]["Categories"]["Category"][m]["CategoryRequirements"][q]["Requirements"]["Line"]);
                                                }
                                            }
                                            else
                                            {
                                                objVisaCategoryDetailNew.CategoryRequirements = new List<Models.VisaCategoryRequirements>();
                                                objVisaCategoryDetailNew.CategoryRequirements.Add(new Models.VisaCategoryRequirements());
                                                objVisaCategoryDetailNew.CategoryRequirements[0].Requirements = new Requirements();

                                                if (VisaJson["VisaDetail"]["Visa"]["VisaInformation"][i]["Categories"]["Category"][m]["CategoryRequirements"].ToList().Count > 0)
                                                {
                                                    if (VisaJson["VisaDetail"]["Visa"]["VisaInformation"][i]["Categories"]["Category"][m]["CategoryRequirements"]["Requirements"].ToList().Count > 0)
                                                    {
                                                        objVisaCategoryDetailNew.CategoryRequirements[0].Requirements.Line = Convert.ToString(
                                                            VisaJson["VisaDetail"]["Visa"]["VisaInformation"][i]["Categories"]["Category"][m]["CategoryRequirements"]["Requirements"]["Line"]);
                                                    }

                                                }

                                            }


                                            objVisaCategoryDetailNew.CategoryInfo[0].Information[0].ProcessingTime = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["VisaInformation"][i]["Categories"]["Category"][m]["CategoryInfo"]
                                                 ["Information"]["ProcessingTime"]);
                                            objVisaCategoryDetailNew.CategoryInfo[0].Information[0].VisaProcedure = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["VisaInformation"][i]["Categories"]["Category"][m]["CategoryInfo"]
                                                                                           ["Information"]["VisaProcedure"]);
                                            objVisaCategoryDetailNew.CategoryInfo[0].Information[0].content = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["VisaInformation"][i]["Categories"]["Category"][m]["CategoryInfo"]
                                                                                      ["Information"]["content"]);
                                            var TypeOfDocumentRequiredNode = VisaJson["VisaDetail"]["Visa"]["VisaInformation"][i]["Categories"]["Category"][m]["CategoryInfo"]["Information"]["DocumentsRequired"].GetType();
                                            if (TypeOfDocumentRequiredNode.Name.ToUpper() == "JOBJECT")
                                            {

                                            }
                                            else
                                            {
                                                objVisaCategoryDetailNew.CategoryInfo[0].Information[0].DocumentsRequired = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["VisaInformation"][i]["Categories"]["Category"][m]["CategoryInfo"]
                                                                                         ["Information"]["DocumentsRequired"]);
                                            }
                                            objVisaInformationNew.Categories[0].Category.Add(objVisaCategoryDetailNew);
                                        }
                                    }
                                    else
                                    {
                                        DistributionWebApi.Models.VisaCategoryDetail objVisaCategoryDetailNew = new Models.VisaCategoryDetail();
                                        // it is object

                                        objVisaCategoryDetailNew.CategoryCode = (string)VisaJson["VisaDetail"]["Visa"]["VisaInformation"][i]["Categories"]["Category"]["CategoryCode"];
                                        // Category is object here
                                        var TypeOfCategoryNode = VisaJson["VisaDetail"]["Visa"]["VisaInformation"][i]["Categories"]["Category"]["Category"].GetType();
                                        if (TypeOfCategoryNode.Name.ToUpper() == "JOBJECT")
                                        {

                                        }
                                        else
                                        {
                                            objVisaCategoryDetailNew.Category = (string)VisaJson["VisaDetail"]["Visa"]["VisaInformation"][i]["Categories"]["Category"]["Category"];
                                            //objVisaCategoryDetailNew.CategoryNotes = (string)VisaJson["VisaDetail"]["Visa"]["VisaInformation"][i]["Categories"]["Category"]["CategoryNotes"];
                                        }

                                        if (VisaJson["VisaDetail"]["Visa"]["VisaInformation"][i]["Categories"]["Category"]["CategoryNotes"] != null &&
                                            VisaJson["VisaDetail"]["Visa"]["VisaInformation"][i]["Categories"]["Category"]["CategoryNotes"].ToList().Count > 0)
                                        {
                                            var TypeOfCategoryNotesNode = VisaJson["VisaDetail"]["Visa"]["VisaInformation"][i]["Categories"]["Category"]["CategoryNotes"].GetType();
                                            if (TypeOfCategoryNotesNode.Name.ToUpper() == "JOBJECT")
                                            {

                                            }
                                            else
                                            {
                                                //objVisaCategoryDetailNew.CategoryNotes = new List<CategoryNotes>();
                                                //objVisaCategoryDetailNew.CategoryNotes.Add(new CategoryNotes());
                                                //objVisaCategoryDetailNew.CategoryNotes[0].Notes = new List<string>();
                                                //objVisaCategoryDetailNew.CategoryNotes[0].Notes.Add((string)VisaJson["VisaDetail"]["Visa"]["VisaInformation"][i]["Categories"]["Category"]["CategoryNotes"]);
                                            }

                                        }



                                        #region CategoryInfo

                                        objVisaCategoryDetailNew.CategoryInfo = new List<Models.VisaCategoryInfo>();
                                        objVisaCategoryDetailNew.CategoryInfo.Add(new Models.VisaCategoryInfo());

                                        objVisaCategoryDetailNew.CategoryInfo[0].Information = new List<VisaInformationChildNode>();
                                        objVisaCategoryDetailNew.CategoryInfo[0].Information.Add(new VisaInformationChildNode());
                                        objVisaCategoryDetailNew.CategoryInfo[0].Information[0].ProcessingTime = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["VisaInformation"][i]["Categories"]["Category"]["CategoryInfo"]
                                                                                        ["Information"]["ProcessingTime"]);
                                        objVisaCategoryDetailNew.CategoryInfo[0].Information[0].VisaProcedure = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["VisaInformation"][i]["Categories"]["Category"]["CategoryInfo"]
                                                                                       ["Information"]["VisaProcedure"]);
                                        objVisaCategoryDetailNew.CategoryInfo[0].Information[0].content = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["VisaInformation"][i]["Categories"]["Category"]["CategoryInfo"]
                                                                                     ["Information"]["content"]);
                                        var TypeOfDocumentRequiredNode = VisaJson["VisaDetail"]["Visa"]["VisaInformation"][i]["Categories"]["Category"]["CategoryInfo"]["Information"]["DocumentsRequired"].GetType();
                                        if (TypeOfDocumentRequiredNode.Name.ToUpper() == "JOBJECT")
                                        {

                                        }
                                        else
                                        {
                                            objVisaCategoryDetailNew.CategoryInfo[0].Information[0].DocumentsRequired = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["VisaInformation"][i]["Categories"]["Category"]["CategoryInfo"]
                                                                                     ["Information"]["DocumentsRequired"]);
                                        }



                                        #endregion



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


                    #region ReciprocalInfo
                    if (VisaJson["VisaDetail"]["Visa"]["ReciprocalVisaInfo"].ToList().Count > 0)
                    {
                        objVisa.ReciprocalVisaInfo.Add(new Models.ReciprocalVisaInfo());
                        objVisa.ReciprocalVisaInfo[0].Description = new List<Models.VisaDescription>();
                        objVisa.ReciprocalVisaInfo[0].Description.Add(new Models.VisaDescription());
                        objVisa.ReciprocalVisaInfo[0].Description[0].ReciprocalVisaInfo = new List<ReciprocalVisaInfo2>();
                        objVisa.ReciprocalVisaInfo[0].Description[0].ReciprocalVisaInfo.Add(new ReciprocalVisaInfo2());
                        objVisa.ReciprocalVisaInfo[0].Description[0].ReciprocalVisaInfo[0].InformationLink = new List<VisaInformationLink>();
                        objVisa.ReciprocalVisaInfo[0].Description[0].ReciprocalVisaInfo[0].content = new List<string>();
                        var ContentType = VisaJson["VisaDetail"]["Visa"]["ReciprocalVisaInfo"]["Description"]["ReciprocalVisaInfo"]["content"].GetType();
                        if (ContentType.Name.ToUpper() == "JARRAY")
                        {
                            int totalContentRecords = VisaJson["VisaDetail"]["Visa"]["ReciprocalVisaInfo"]["Description"]["ReciprocalVisaInfo"]["content"].ToList().Count;
                            for (int p = 0; p < totalContentRecords; p++)
                            {
                                objVisa.ReciprocalVisaInfo[0].Description[0].ReciprocalVisaInfo[0].content.Add((string)VisaJson["VisaDetail"]["Visa"]["ReciprocalVisaInfo"]["Description"]["ReciprocalVisaInfo"]
                                                                         ["content"][p]);
                            }
                        }
                        else
                        {
                            objVisa.ReciprocalVisaInfo[0].Description[0].ReciprocalVisaInfo[0].content.Add((string)VisaJson["VisaDetail"]["Visa"]["ReciprocalVisaInfo"]["Description"]["ReciprocalVisaInfo"]["content"]);
                        }

                        if (VisaJson["VisaDetail"]["Visa"]["ReciprocalVisaInfo"]["Description"]["ReciprocalVisaInfo"]["InformationLink"].ToList().Count > 0)
                        {
                            objVisa.ReciprocalVisaInfo[0].Description[0].ReciprocalVisaInfo[0].InformationLink = new List<VisaInformationLink>();
                            objVisa.ReciprocalVisaInfo[0].Description[0].ReciprocalVisaInfo[0].InformationLink.Add(new VisaInformationLink());
                            objVisa.ReciprocalVisaInfo[0].Description[0].ReciprocalVisaInfo[0].InformationLink[0].content = new List<string>();

                            var InformationLinkType = VisaJson["VisaDetail"]["Visa"]["ReciprocalVisaInfo"]["Description"]["ReciprocalVisaInfo"]["InformationLink"].GetType();
                            if (InformationLinkType.Name.ToUpper() == "JARRAY")
                            {
                                int TotalInformationLinks = VisaJson["VisaDetail"]["Visa"]["ReciprocalVisaInfo"]["Description"]["ReciprocalVisaInfo"]["InformationLink"].ToList().Count;
                                for (int t = 0; t < TotalInformationLinks; t++)
                                {
                                    VisaInformationLink objVisaInformationLinkNew = new VisaInformationLink();
                                    objVisa.ReciprocalVisaInfo[0].Description[0].ReciprocalVisaInfo[0].InformationLink[t].content = new List<string>();
                                    objVisa.ReciprocalVisaInfo[0].Description[0].ReciprocalVisaInfo[0].InformationLink[t].content.Add((string)VisaJson["VisaDetail"]["Visa"]["ReciprocalVisaInfo"]["Description"]["ReciprocalVisaInfo"]["InformationLink"][t]["content"]);
                                    objVisa.ReciprocalVisaInfo[0].Description[0].ReciprocalVisaInfo[0].InformationLink[t].href = (string)VisaJson["VisaDetail"]["Visa"]["ReciprocalVisaInfo"]["Description"]["ReciprocalVisaInfo"]["InformationLink"][t]["href"];
                                    objVisa.ReciprocalVisaInfo[0].Description[0].ReciprocalVisaInfo[0].InformationLink[t].target = (string)VisaJson["VisaDetail"]["Visa"]["ReciprocalVisaInfo"]["Description"]["ReciprocalVisaInfo"]["InformationLink"][t]["target"];

                                    objVisa.ReciprocalVisaInfo[0].Description[0].ReciprocalVisaInfo[0].InformationLink.Add(objVisaInformationLinkNew);


                                }
                                if (objVisa.ReciprocalVisaInfo[0].Description[0].ReciprocalVisaInfo[0].InformationLink.Last().content == null)
                                {
                                    objVisa.ReciprocalVisaInfo[0].Description[0].ReciprocalVisaInfo[0].InformationLink.Remove(objVisa.ReciprocalVisaInfo[0].Description[0].ReciprocalVisaInfo[0].InformationLink.Last());
                                }
                            }
                            else
                            {
                                objVisa.ReciprocalVisaInfo[0].Description[0].ReciprocalVisaInfo[0].InformationLink.Add(new VisaInformationLink());
                                objVisa.ReciprocalVisaInfo[0].Description[0].ReciprocalVisaInfo[0].InformationLink[0].content.Add((string)VisaJson["VisaDetail"]["Visa"]["ReciprocalVisaInfo"]["Description"]["ReciprocalVisaInfo"]["InformationLink"]["content"]);
                                objVisa.ReciprocalVisaInfo[0].Description[0].ReciprocalVisaInfo[0].InformationLink[0].href = (string)VisaJson["VisaDetail"]["Visa"]["ReciprocalVisaInfo"]["Description"]["ReciprocalVisaInfo"]["InformationLink"]["href"];
                                objVisa.ReciprocalVisaInfo[0].Description[0].ReciprocalVisaInfo[0].InformationLink[0].target = (string)VisaJson["VisaDetail"]["Visa"]["ReciprocalVisaInfo"]["Description"]["ReciprocalVisaInfo"]["InformationLink"]["target"];

                                if (objVisa.ReciprocalVisaInfo[0].Description[0].ReciprocalVisaInfo[0].InformationLink.Last().content == null)
                                {
                                    objVisa.ReciprocalVisaInfo[0].Description[0].ReciprocalVisaInfo[0].InformationLink.Remove(objVisa.ReciprocalVisaInfo[0].Description[0].ReciprocalVisaInfo[0].InformationLink.Last());
                                }

                            }


                        }

                    }
                    #endregion

                    #region InternationalAdvisory

                    if (VisaJson["VisaDetail"]["Visa"]["InternationalAdvisory"].ToList().Count > 0)
                    {
                        objVisa.InternationalAdvisory.Add(new Models.VisaInternationalAdvisory());
                        objVisa.InternationalAdvisory[0].Description = new List<Models.VisaDescription2>();
                        objVisa.InternationalAdvisory[0].Description.Add(new Models.VisaDescription2());
                        objVisa.InternationalAdvisory[0].Description[0].VisaInternationalAdvisory = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["InternationalAdvisory"]["Description"]["InternationalAdvisory"]);


                    }


                    #endregion

                    #region IVSAdvisory

                    if (VisaJson["VisaDetail"]["Visa"]["IVSAdvisory"].ToList().Count > 0)
                    {
                        DistributionWebApi.Models.VisaIVSAdvisory objVisaIVSAdvisoryNew = new Models.VisaIVSAdvisory();
                        objVisaIVSAdvisoryNew.Description = new List<Models.VisaDescription3>();
                        objVisaIVSAdvisoryNew.Description.Add(new Models.VisaDescription3());
                        objVisaIVSAdvisoryNew.Description[0].Heading = new List<Models.VisaHeading>();
                        objVisaIVSAdvisoryNew.Description[0].Heading.Add(new Models.VisaHeading
                        {
                            content = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["IVSAdvisory"]["Description"]["Heading"]["content"])
                        });

                        objVisa.IVSAdvisory.Add(objVisaIVSAdvisoryNew);

                    }

                    #endregion


                    #region IndianEmbassy

                    if (VisaJson["VisaDetail"]["Visa"]["IndianEmbassy"].ToList().Count > 0)
                    {
                        if (VisaJson["VisaDetail"]["Visa"]["IndianEmbassy"]["Office"].ToList().Count > 0)
                        {
                            var ContentType = VisaJson["VisaDetail"]["Visa"]["IndianEmbassy"]["Office"].GetType();
                            if (ContentType.Name.ToUpper() == "JARRAY")
                            {
                                int TotalOffices = VisaJson["VisaDetail"]["Visa"]["IndianEmbassy"]["Office"].ToList().Count;
                                objVisa.IndianEmbassy.Add(new VisaIndianEmbassy
                                {
                                    Office = new List<Models.VisaOffice>()
                                });
                                for (int r = 0; r < TotalOffices; r++)
                                {
                                    objVisa.IndianEmbassy[0].Office.Add(new Models.VisaOffice
                                    {
                                        Address = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["IndianEmbassy"]["Office"][r]["Address"]),
                                        City = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["IndianEmbassy"]["Office"][r]["City"]),
                                        Country = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["IndianEmbassy"]["Office"][r]["Country"]),
                                        Email = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["IndianEmbassy"]["Office"][r]["Email"]),
                                        Fax = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["IndianEmbassy"]["Office"][r]["Fax"]),
                                        Name = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["IndianEmbassy"]["Office"][r]["Name"]),
                                        Phone = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["IndianEmbassy"]["Office"][r]["Phone"]),
                                        PinCode = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["IndianEmbassy"]["Office"][r]["PinCode"]),
                                        SystemCityCode = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["IndianEmbassy"]["Office"][r]["SystemCityCode"]),
                                        SystemCityName = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["IndianEmbassy"]["Office"][r]["SystemCityName"]),
                                        SystemCountryCode = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["IndianEmbassy"]["Office"][r]["SystemCountryCode"]),
                                        SystemCountryName = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["IndianEmbassy"]["Office"][r]["SystemCountryName"]),
                                        URL = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["IndianEmbassy"]["Office"][r]["URL"]),
                                        Website = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["IndianEmbassy"]["Office"][r]["Website"])
                                    });
                                }
                            }
                            else
                            {
                                objVisa.IndianEmbassy.Add(new VisaIndianEmbassy
                                {
                                    Office = new List<Models.VisaOffice>()
                                });

                                objVisa.IndianEmbassy[0].Office.Add(new Models.VisaOffice
                                {
                                    Address = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["IndianEmbassy"]["Office"]["Address"]),
                                    City = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["IndianEmbassy"]["Office"]["City"]),
                                    Country = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["IndianEmbassy"]["Office"]["Country"]),
                                    Email = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["IndianEmbassy"]["Office"]["Email"]),
                                    Fax = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["IndianEmbassy"]["Office"]["Fax"]),
                                    Name = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["IndianEmbassy"]["Office"]["Name"]),
                                    Phone = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["IndianEmbassy"]["Office"]["Phone"]),
                                    PinCode = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["IndianEmbassy"]["Office"]["PinCode"]),
                                    SystemCityCode = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["IndianEmbassy"]["Office"]["SystemCityCode"]),
                                    SystemCityName = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["IndianEmbassy"]["Office"]["SystemCityName"]),
                                    SystemCountryCode = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["IndianEmbassy"]["Office"]["SystemCountryCode"]),
                                    SystemCountryName = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["IndianEmbassy"]["Office"]["SystemCountryName"]),
                                    URL = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["IndianEmbassy"]["Office"]["URL"]),
                                    Website = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["IndianEmbassy"]["Office"]["Website"])
                                });
                            }

                        }

                    }

                    #endregion

                    #region DiplomaticRepresentation

                    if (VisaJson["VisaDetail"]["Visa"]["DiplomaticRepresentation"].ToList().Count > 0)
                    {
                        if (VisaJson["VisaDetail"]["Visa"]["DiplomaticRepresentation"]["Offices"].ToList().Count > 0)
                        {
                            objVisa.DiplomaticRepresentation.Add(new VisaDiplomaticRepresentation());
                            objVisa.DiplomaticRepresentation[0].Offices = new List<Models.VisaOffices>();
                            objVisa.DiplomaticRepresentation[0].Offices.Add(new Models.VisaOffices());
                            objVisa.DiplomaticRepresentation[0].Offices[0].Office = new List<Models.VisaOffice2>();


                            var TypeOfDiplomaticOffice = VisaJson["VisaDetail"]["Visa"]["DiplomaticRepresentation"]["Offices"]["Office"].GetType();
                            if (TypeOfDiplomaticOffice.Name.ToUpper() == "JARRAY")
                            {
                                int TotalDiplomaticOffices = VisaJson["VisaDetail"]["Visa"]["DiplomaticRepresentation"]["Offices"]["Office"].ToList().Count;

                                for (int cnt = 0; cnt < TotalDiplomaticOffices; cnt++)
                                {
                                    DistributionWebApi.Models.VisaOffice2 objVisaOffice2New = new Models.VisaOffice2()
                                    {
                                        Address = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["DiplomaticRepresentation"]["Offices"]["Office"][cnt]["Address"]),
                                        City = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["DiplomaticRepresentation"]["Offices"]["Office"][cnt]["City"]),
                                        CollectionTimings = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["DiplomaticRepresentation"]["Offices"]["Office"][cnt]["CollectionTimings"]),
                                        Country = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["DiplomaticRepresentation"]["Offices"]["Office"][cnt]["Country"]),
                                        Email = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["DiplomaticRepresentation"]["Offices"]["Office"][cnt]["Email"]),
                                        Fax = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["DiplomaticRepresentation"]["Offices"]["Office"][cnt]["Fax"]),
                                        Name = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["DiplomaticRepresentation"]["Offices"]["Office"][cnt]["Name"]),
                                        Notes = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["DiplomaticRepresentation"]["Offices"]["Office"][cnt]["Notes"]),
                                        Phone = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["DiplomaticRepresentation"]["Offices"]["Office"][cnt]["Phone"]),
                                        PinCode = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["DiplomaticRepresentation"]["Offices"]["Office"][cnt]["PinCode"]),
                                        PublicTimings = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["DiplomaticRepresentation"]["Offices"]["Office"][cnt]["PublicTimings"]),
                                        Telephone = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["DiplomaticRepresentation"]["Offices"]["Office"][cnt]["Telephone"]),
                                        Timings = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["DiplomaticRepresentation"]["Offices"]["Office"][cnt]["Timings"]),
                                        VisaTimings = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["DiplomaticRepresentation"]["Offices"]["Office"][cnt]["Timings"]),
                                        Website = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["DiplomaticRepresentation"]["Offices"]["Office"][cnt]["Website"])
                                    };

                                    objVisa.DiplomaticRepresentation[0].Offices[0].Office.Add(objVisaOffice2New);
                                }

                            }
                            else
                            {
                                // It is Object 
                                objVisa.DiplomaticRepresentation[0].Offices[0].Office.Add(new Models.VisaOffice2
                                {
                                    Address = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["DiplomaticRepresentation"]["Offices"]["Office"]["Address"]),
                                    City = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["DiplomaticRepresentation"]["Offices"]["Office"]["City"]),
                                    CollectionTimings = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["DiplomaticRepresentation"]["Offices"]["Office"]["CollectionTimings"]),
                                    Country = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["DiplomaticRepresentation"]["Offices"]["Office"]["Country"]),
                                    Email = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["DiplomaticRepresentation"]["Offices"]["Office"]["Email"]),
                                    Fax = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["DiplomaticRepresentation"]["Offices"]["Office"]["Fax"]),
                                    Name = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["DiplomaticRepresentation"]["Offices"]["Office"]["Name"]),
                                    Notes = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["DiplomaticRepresentation"]["Offices"]["Office"]["Notes"]),
                                    Phone = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["DiplomaticRepresentation"]["Offices"]["Office"]["Phone"]),
                                    PinCode = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["DiplomaticRepresentation"]["Offices"]["Office"]["PinCode"]),
                                    PublicTimings = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["DiplomaticRepresentation"]["Offices"]["Office"]["PublicTimings"]),
                                    Telephone = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["DiplomaticRepresentation"]["Offices"]["Office"]["Telephone"]),
                                    Timings = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["DiplomaticRepresentation"]["Offices"]["Office"]["Timings"]),
                                    VisaTimings = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["DiplomaticRepresentation"]["Offices"]["Office"]["Timings"]),
                                    Website = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["DiplomaticRepresentation"]["Offices"]["Office"]["Website"])
                                });
                            }

                        }

                    }
                    #endregion

                    #region SAARCInfo

                    if (VisaJson["VisaDetail"]["Visa"]["SAARCInfo"].ToList().Count > 0)
                    {
                        if (VisaJson["VisaDetail"]["Visa"]["SAARCInfo"]["CountryOffices"].ToList().Count > 0)
                        {
                            objVisa.SAARCInfo.Add(new Models.VisaSAARCInfo());
                            objVisa.SAARCInfo[0].CountryOffices = new List<Models.VisaCountryOffices>();
                            objVisa.SAARCInfo[0].CountryOffices.Add(new Models.VisaCountryOffices());
                            objVisa.SAARCInfo[0].CountryOffices[0].CountryOffice = new List<Models.VisaCountryOffice>();




                            var TypeCountryOffice = VisaJson["VisaDetail"]["Visa"]["SAARCInfo"]["CountryOffices"]["CountryOffic"].GetType();
                            if (TypeCountryOffice.Name.ToUpper() == "JARRAY")
                            {

                                int TotalCountryOffices = VisaJson["VisaDetail"]["Visa"]["SAARCInfo"]["CountryOffices"]["CountryOffic"].ToList().Count;

                                for (int Cnt = 0; Cnt < TotalCountryOffices; Cnt++)
                                {
                                    objVisa.SAARCInfo[0].CountryOffices[0].CountryOffice.Add(new Models.VisaCountryOffice
                                    {
                                        Address = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["SAARCInfo"]["CountryOffices"]["CountryOffic"][Cnt]["Address"]),
                                        City = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["SAARCInfo"]["CountryOffices"]["CountryOffic"][Cnt]["City"]),
                                        CountryID = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["SAARCInfo"]["CountryOffices"]["CountryOffic"][Cnt]["CountryID"]),
                                        County = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["SAARCInfo"]["CountryOffices"]["CountryOffic"][Cnt]["County"]),
                                        Fax = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["SAARCInfo"]["CountryOffices"]["CountryOffic"][Cnt]["Fax"]),
                                        Name = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["SAARCInfo"]["CountryOffices"]["CountryOffic"][Cnt]["Name"]),
                                        PinCode = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["SAARCInfo"]["CountryOffices"]["CountryOffic"][Cnt]["PinCode"]),
                                        Telephone = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["SAARCInfo"]["CountryOffices"]["CountryOffic"][Cnt]["Telephone"]),
                                        VisaRequired = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["SAARCInfo"]["CountryOffices"]["CountryOffic"][Cnt]["VisaRequired"]),
                                        Website = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["SAARCInfo"]["CountryOffices"]["CountryOffic"][Cnt]["Website"]),
                                        WhereToApply = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["SAARCInfo"]["CountryOffices"]["CountryOffic"][Cnt]["WhereToApply"])
                                    });
                                }
                            }
                            else
                            {
                                objVisa.SAARCInfo[0].CountryOffices[0].CountryOffice.Add(new Models.VisaCountryOffice
                                {
                                    Address = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["SAARCInfo"]["CountryOffices"]["CountryOffic"]["Address"]),
                                    City = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["SAARCInfo"]["CountryOffices"]["CountryOffic"]["City"]),
                                    CountryID = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["SAARCInfo"]["CountryOffices"]["CountryOffic"]["CountryID"]),
                                    County = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["SAARCInfo"]["CountryOffices"]["CountryOffic"]["County"]),
                                    Fax = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["SAARCInfo"]["CountryOffices"]["CountryOffic"]["Fax"]),
                                    Name = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["SAARCInfo"]["CountryOffices"]["CountryOffic"]["Name"]),
                                    PinCode = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["SAARCInfo"]["CountryOffices"]["CountryOffic"]["PinCode"]),
                                    Telephone = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["SAARCInfo"]["CountryOffices"]["CountryOffic"]["Telephone"]),
                                    VisaRequired = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["SAARCInfo"]["CountryOffices"]["CountryOffic"]["VisaRequired"]),
                                    Website = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["SAARCInfo"]["CountryOffices"]["CountryOffic"]["Website"]),
                                    WhereToApply = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["SAARCInfo"]["CountryOffices"]["CountryOffic"]["WhereToApply"])
                                });

                            }

                        }
                    }
                    #endregion

                    #region IntlHelpAddress

                    if (VisaJson["VisaDetail"]["Visa"]["IntlHelpAddress"].ToList().Count > 0)
                    {
                        if (VisaJson["VisaDetail"]["Visa"]["IntlHelpAddress"]["HelpAddress"].ToList().Count > 0)
                        {
                            objVisa.IntlHelpAddress.HelpAddress = new List<Models.VisaHelpAddress>();


                            var TypeOfHelpAddress = VisaJson["VisaDetail"]["Visa"]["IntlHelpAddress"]["HelpAddress"].GetType();
                            if (TypeOfHelpAddress.Name.ToUpper() == "JARRAY")
                            {
                                int TotalHelpAddress = VisaJson["VisaDetail"]["Visa"]["IntlHelpAddress"]["HelpAddress"].ToList().Count;
                                for (int f = 0; f < TotalHelpAddress; f++)
                                {
                                    objVisa.IntlHelpAddress.HelpAddress.Add(new Models.VisaHelpAddress
                                    {
                                        Address = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["IntlHelpAddress"]["HelpAddress"][f]["Address"]),
                                        City = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["IntlHelpAddress"]["HelpAddress"][f]["City"]),
                                        Country = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["IntlHelpAddress"]["HelpAddress"][f]["Country"]),
                                        Fax = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["IntlHelpAddress"]["HelpAddress"][f]["Fax"]),
                                        Name = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["IntlHelpAddress"]["HelpAddress"][f]["Name"]),
                                        Phone = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["IntlHelpAddress"]["HelpAddress"][f]["Phone"]),
                                        PinCode = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["IntlHelpAddress"]["HelpAddress"][f]["PinCode"]),
                                        URL = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["IntlHelpAddress"]["HelpAddress"][f]["URL"]),
                                        Website = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["IntlHelpAddress"]["HelpAddress"][f]["Website"])
                                    });
                                }

                            }
                            else
                            {// it is object

                                objVisa.IntlHelpAddress.HelpAddress.Add(new Models.VisaHelpAddress
                                {
                                    Address = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["IntlHelpAddress"]["HelpAddress"]["Address"]),
                                    City = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["IntlHelpAddress"]["HelpAddress"]["City"]),
                                    Country = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["IntlHelpAddress"]["HelpAddress"]["Country"]),
                                    Fax = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["IntlHelpAddress"]["HelpAddress"]["Fax"]),
                                    Name = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["IntlHelpAddress"]["HelpAddress"]["Name"]),
                                    Phone = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["IntlHelpAddress"]["HelpAddress"]["Phone"]),
                                    PinCode = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["IntlHelpAddress"]["HelpAddress"]["PinCode"]),
                                    URL = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["IntlHelpAddress"]["HelpAddress"]["URL"]),
                                    Website = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["IntlHelpAddress"]["HelpAddress"]["Website"])
                                });
                            }


                        }
                    }
                    #endregion

                    #region CountryDetails

                    if (VisaJson["VisaDetail"]["Visa"]["CountryDetails"].ToList().Count > 0)
                    {
                        if (VisaJson["VisaDetail"]["Visa"]["CountryDetails"]["GeneralInfo"].ToList().Count > 0)
                        {
                            var TypeOfGeneralInfo = VisaJson["VisaDetail"]["Visa"]["CountryDetails"]["GeneralInfo"].GetType();
                            if (TypeOfGeneralInfo.Name.ToUpper() == "JARRAY")
                            {
                                objVisa.CountryDetails.Add(new Models.VisaCountryDetails());
                                objVisa.CountryDetails[0].GeneralInfo = new List<VisaGeneralInfo>();

                                int TotalGeneralInfos = VisaJson["VisaDetail"]["Visa"]["CountryDetails"]["GeneralInfo"].ToList().Count;
                                for (int e = 0; e < TotalGeneralInfos; e++)
                                {
                                    objVisa.CountryDetails[0].GeneralInfo.Add(new VisaGeneralInfo()
                                    {
                                        Area = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["CountryDetails"]["GeneralInfo"][e]["Area"]),
                                        Capital = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["CountryDetails"]["GeneralInfo"][e]["Capital"]),
                                        Code = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["CountryDetails"]["GeneralInfo"][e]["Code"]),
                                        Currency = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["CountryDetails"]["GeneralInfo"][e]["Currency"]),
                                        Flag = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["CountryDetails"]["GeneralInfo"][e]["Flag"]),
                                        Languages = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["CountryDetails"]["GeneralInfo"][e]["Languages"]),
                                        LargeMap = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["CountryDetails"]["GeneralInfo"][e]["LargeMap"]),
                                        Location = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["CountryDetails"]["GeneralInfo"][e]["Location"]),
                                        NationalDay = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["CountryDetails"]["GeneralInfo"][e]["NationalDay"]),
                                        Population = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["CountryDetails"]["GeneralInfo"][e]["Population"]),
                                        SmallMap = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["CountryDetails"]["GeneralInfo"][e]["SmallMap"]),
                                        Time = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["CountryDetails"]["GeneralInfo"][e]["Time"]),
                                        WorldFactBook = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["CountryDetails"]["GeneralInfo"][e]["WorldFactBook"])
                                    });
                                }
                            }
                            else
                            {
                                objVisa.CountryDetails.Add(new Models.VisaCountryDetails());
                                objVisa.CountryDetails[0].GeneralInfo = new List<VisaGeneralInfo>();
                                objVisa.CountryDetails[0].GeneralInfo.Add(new VisaGeneralInfo()
                                {
                                    Area = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["CountryDetails"]["GeneralInfo"]["Area"]),
                                    Capital = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["CountryDetails"]["GeneralInfo"]["Capital"]),
                                    Code = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["CountryDetails"]["GeneralInfo"]["Code"]),
                                    Currency = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["CountryDetails"]["GeneralInfo"]["Currency"]),
                                    Flag = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["CountryDetails"]["GeneralInfo"]["Flag"]),
                                    Languages = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["CountryDetails"]["GeneralInfo"]["Languages"]),
                                    LargeMap = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["CountryDetails"]["GeneralInfo"]["LargeMap"]),
                                    Location = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["CountryDetails"]["GeneralInfo"]["Location"]),
                                    NationalDay = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["CountryDetails"]["GeneralInfo"]["NationalDay"]),
                                    Population = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["CountryDetails"]["GeneralInfo"]["Population"]),
                                    SmallMap = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["CountryDetails"]["GeneralInfo"]["SmallMap"]),
                                    Time = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["CountryDetails"]["GeneralInfo"]["Time"]),
                                    WorldFactBook = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["CountryDetails"]["GeneralInfo"]["WorldFactBook"])
                                });
                            }

                        }



                        if (VisaJson["VisaDetail"]["Visa"]["CountryDetails"]["Airports"].ToList().Count > 0)
                        {
                            if (VisaJson["VisaDetail"]["Visa"]["CountryDetails"]["Airports"]["Airport"].ToList().Count > 0)
                            {
                                var TypeOfAirport = VisaJson["VisaDetail"]["Visa"]["CountryDetails"]["Airports"]["Airport"].GetType();
                                if (TypeOfAirport.Name.ToUpper() == "JARRAY")
                                {
                                    if (objVisa.CountryDetails.Count == 0)
                                    {
                                        objVisa.CountryDetails.Add(new Models.VisaCountryDetails());
                                    }
                                    if (objVisa.CountryDetails[0].Airports == null)
                                    {
                                        objVisa.CountryDetails[0].Airports = new List<VisaAirports>();
                                    }

                                    int TotalAirports = VisaJson["VisaDetail"]["Visa"]["CountryDetails"]["Airports"]["Airport"].ToList().Count;

                                    objVisa.CountryDetails[0].Airports = new List<VisaAirports>();
                                    objVisa.CountryDetails[0].Airports.Add(new VisaAirports());
                                    objVisa.CountryDetails[0].Airports[0].Airport = new List<Models.VisaAirport>();

                                    for (int u = 0; u < TotalAirports; u++)
                                    {
                                        objVisa.CountryDetails[0].Airports[0].Airport.Add(new Models.VisaAirport()
                                        {
                                            Code = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["CountryDetails"]["Airports"]["Airport"][u]["Type"]),
                                            Name = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["CountryDetails"]["Airports"]["Airport"][u]["Code"]),
                                            Type = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["CountryDetails"]["Airports"]["Airport"][u]["Name"])
                                        });
                                    }

                                }
                                else
                                {  // It is object
                                    if (objVisa.CountryDetails.Count == 0)
                                    {
                                        objVisa.CountryDetails.Add(new Models.VisaCountryDetails());
                                    }
                                    if (objVisa.CountryDetails[0].Airports == null)
                                    {
                                        objVisa.CountryDetails[0].Airports = new List<VisaAirports>();
                                    }
                                    objVisa.CountryDetails[0].Airports = new List<VisaAirports>();
                                    objVisa.CountryDetails[0].Airports.Add(new VisaAirports());
                                    objVisa.CountryDetails[0].Airports[0].Airport = new List<Models.VisaAirport>();
                                    objVisa.CountryDetails[0].Airports[0].Airport.Add(new Models.VisaAirport()
                                    {
                                        Code = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["CountryDetails"]["Airports"]["Airport"]["Type"]),
                                        Name = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["CountryDetails"]["Airports"]["Airport"]["Code"]),
                                        Type = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["CountryDetails"]["Airports"]["Airport"]["Name"])
                                    });
                                }

                            }
                        }


                        if (VisaJson["VisaDetail"]["Visa"]["CountryDetails"]["CountryName"].ToList().Count > 0)
                        {
                            if (objVisa.CountryDetails.Count == 0)
                            {
                                objVisa.CountryDetails.Add(new Models.VisaCountryDetails());
                            }
                            objVisa.CountryDetails[0].CountryName = new List<Models.VisaCountryName>();


                            var TypeOfCounytryName = VisaJson["VisaDetail"]["Visa"]["CountryDetails"]["CountryName"].GetType();
                            if (TypeOfCounytryName.Name.ToUpper() == "JARRAY")
                            {
                                int TotalCountries = VisaJson["VisaDetail"]["Visa"]["CountryDetails"]["CountryName"].ToList().Count;
                                for (int y = 0; y < TotalCountries; y++)
                                {
                                    objVisa.CountryDetails[0].CountryName.Add(new Models.VisaCountryName()
                                    {
                                        Name = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["CountryDetails"]["CountryName"][y]["Name"])
                                    });
                                }
                            }
                            else
                            { // It is object

                                objVisa.CountryDetails[0].CountryName.Add(new Models.VisaCountryName()
                                {
                                    Name = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["CountryDetails"]["CountryName"]["Name"])
                                });
                            }
                        }


                        if (VisaJson["VisaDetail"]["Visa"]["CountryDetails"]["Airlines"].ToList().Count > 0)
                        {
                            if (VisaJson["VisaDetail"]["Visa"]["CountryDetails"]["Airlines"]["Airline"].ToList().Count > 0)
                            {
                                if (objVisa.CountryDetails.Count == 0)
                                {
                                    objVisa.CountryDetails.Add(new Models.VisaCountryDetails());
                                }

                                var TypeOfAirlines = VisaJson["VisaDetail"]["Visa"]["CountryDetails"]["Airlines"]["Airline"].GetType();
                                if (TypeOfAirlines.Name.ToUpper() == "JARRAY")
                                {
                                    objVisa.CountryDetails[0].Airlines = new List<VisaAirlines>();
                                    objVisa.CountryDetails[0].Airlines.Add(new VisaAirlines());
                                    objVisa.CountryDetails[0].Airlines[0].Airline = new List<VisaAirline>();

                                    int TotalAirlines = VisaJson["VisaDetail"]["Visa"]["CountryDetails"]["Airlines"]["Airline"].ToList().Count;

                                    for (int f = 0; f < TotalAirlines; f++)
                                    {
                                        objVisa.CountryDetails[0].Airlines[0].Airline.Add(new VisaAirline()
                                        {
                                            Code = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["CountryDetails"]["Airlines"]["Airline"][f]["Code"]),
                                            Name = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["CountryDetails"]["Airlines"]["Airline"][f]["Name"])
                                        });
                                    }
                                }
                                else
                                { // It is Object
                                    objVisa.CountryDetails[0].Airlines = new List<VisaAirlines>();
                                    objVisa.CountryDetails[0].Airlines.Add(new VisaAirlines());
                                    objVisa.CountryDetails[0].Airlines[0].Airline = new List<VisaAirline>();

                                    objVisa.CountryDetails[0].Airlines[0].Airline.Add(new VisaAirline()
                                    {
                                        Code = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["CountryDetails"]["Airlines"]["Airline"]["Code"]),
                                        Name = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["CountryDetails"]["Airlines"]["Airline"]["Name"])
                                    });
                                }

                            }
                        }

                        if (VisaJson["VisaDetail"]["Visa"]["CountryDetails"]["Holidays"].ToList().Count > 0)
                        {
                            if (VisaJson["VisaDetail"]["Visa"]["CountryDetails"]["Holidays"]["Holiday"].ToList().Count > 0)
                            {
                                var TypeOfHoliday = VisaJson["VisaDetail"]["Visa"]["CountryDetails"]["Holidays"]["Holiday"].GetType();

                                if (objVisa.CountryDetails.Count == 0)
                                {
                                    objVisa.CountryDetails.Add(new Models.VisaCountryDetails());
                                }
                                objVisa.CountryDetails[0].Holidays = new Models.VisaHolidays();
                                objVisa.CountryDetails[0].Holidays.Holiday = new List<Models.VisaHoliday>();


                                if (TypeOfHoliday.Name.ToUpper() == "JARRAY")
                                {
                                    int TotalHolidayRecords = VisaJson["VisaDetail"]["Visa"]["CountryDetails"]["Holidays"]["Holiday"].ToList().Count;

                                    for (int count = 0; count < TotalHolidayRecords; count++)
                                    {
                                        objVisa.CountryDetails[0].Holidays.Holiday.Add(new Models.VisaHoliday()
                                        {
                                            HolidayName = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["CountryDetails"]["Holidays"]["Holiday"][count]["HolidayName"]),
                                            Year = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["CountryDetails"]["Holidays"]["Holiday"][count]["Year"]),
                                            Date = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["CountryDetails"]["Holidays"]["Holiday"][count]["Date"]),
                                            Month = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["CountryDetails"]["Holidays"]["Holiday"][count]["Month"])
                                        });
                                    }
                                }
                                else
                                { // it is object
                                    objVisa.CountryDetails[0].Holidays.Holiday.Add(new Models.VisaHoliday()
                                    {
                                        HolidayName = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["CountryDetails"]["Holidays"]["Holiday"]["HolidayName"]),
                                        Year = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["CountryDetails"]["Holidays"]["Holiday"]["Year"]),
                                        Date = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["CountryDetails"]["Holidays"]["Holiday"]["Date"]),
                                        Month = Convert.ToString(VisaJson["VisaDetail"]["Visa"]["CountryDetails"]["Holidays"]["Holiday"]["Month"])
                                    });

                                }

                            }
                        }

                    }
                    #endregion

                    objVisaDetail.Visa.Add(objVisa);

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
