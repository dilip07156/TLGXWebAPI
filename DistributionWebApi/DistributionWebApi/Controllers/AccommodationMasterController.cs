using DistributionWebApi.Models;
using DistributionWebApi.Models.Static;
using DistributionWebApi.Mongo;
using MongoDB.Driver;
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
    /// Used to retrieve TLGX Accommodation Master information.
    /// </summary>
    [RoutePrefix("Masters/Get")]
    public class AccommodationMasterController : ApiController
    {

        /// <summary>
        /// Mongo database handler
        /// </summary>
        protected static IMongoDatabase _database;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Retrieve all TLGX System Accommodation
        /// </summary>
        /// <returns>List of TLGX Accommodation Masters. Currently restricted to internal Name and Code data.</returns>
        [Route("Accommodation")]
        [HttpGet]
        [ResponseType(typeof(List<AccommodationMasterRS>))]
        public async Task<HttpResponseMessage> GetAllAccommodation()
        {
            try
            {
                _database = MongoDBHandler.mDatabase();
                var collection = _database.GetCollection<AccommodationMaster>("AccommodationMaster");
                var result = await collection.Find(c => true && c.TLGXAccoId != null)
                    .Project(u => new AccommodationMasterRS {
                        HotelID = u.TLGXAccoId,
                    HotelName = u.HotelName,
                    HotelType = u.ProductCategorySubType,
                    HotelStarRating = u.HotelStarRating,
                    StreetName = u.StreetName,
                    StreetNumber = u.StreetNumber,
                    Street3 = u.Street3,
                    Street4=u.Street4,
                    Street5 = u.Street5,
                    PostalCode = u.PostalCode,
                    Town = u.Town,
                    Location = u.Location,
                    Area = u.Area,
                    CityCode = u.CityCode,
                    CityName = u.CityName,
                    StateCode = u.StateCode,
                    StateName = u.StateName,
                    CountryCode = u.CountryCode,
                    CountryName = u.CountryName,
                    FullAddress = u.FullAddress,
                    TelephoneNumber = u.Telephone,
                    Fax = u.Fax,
                    URL = u.WebSiteURL,
                    EmailAddress = u.Email,
                    Latitude = u.Latitude,
                    Longitude = u.Longitude


                }).ToListAsync();
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
        /// Retrieve all TLGX System Accommodation with StartsWith Filter on TLGX Country Code
        /// </summary>
        /// <param name="CountryCode"></param>
        /// <returns>List of TLGX Accommodation Masters. Currently restricted to internal Name and Code data.</returns>
        [Route("Accommodation/CountryCode/{CountryCode}")]
        [HttpGet]
        [ResponseType(typeof(List<AccommodationMaster>))]
        public async Task<HttpResponseMessage> GetAccommodationByCountryCode(string CountryCode)
        {
            try
            {
                _database = MongoDBHandler.mDatabase();
                var collection = _database.GetCollection<AccommodationMaster>("AccommodationMaster");
                var result = await collection.Find(c => c.CountryCode == CountryCode.Trim().ToUpper() && c.TLGXAccoId != null)
                    .Project(u => new AccommodationMasterRS
                    {
                        HotelID = u.TLGXAccoId,
                        HotelName = u.HotelName,
                        HotelType = u.ProductCategorySubType,
                        HotelStarRating = u.HotelStarRating,
                        StreetName = u.StreetName,
                        StreetNumber = u.StreetNumber,
                        Street3 = u.Street3,
                        Street4 = u.Street4,
                        Street5 = u.Street5,
                        PostalCode = u.PostalCode,
                        Town = u.Town,
                        Location = u.Location,
                        Area = u.Area,
                        CityCode = u.CityCode,
                        CityName = u.CityName,
                        StateCode = u.StateCode,
                        StateName = u.StateName,
                        CountryCode = u.CountryCode,
                        CountryName = u.CountryName,
                        FullAddress = u.FullAddress,
                        TelephoneNumber = u.Telephone,
                        Fax = u.Fax,
                        URL = u.WebSiteURL,
                        EmailAddress = u.Email,
                        Latitude = u.Latitude,
                        Longitude = u.Longitude


                    })
                    .SortBy(s => s.TLGXAccoId).ToListAsync();
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
        /// Retrieve all TLGX System Accommodation with StartsWith Filter on TLGX Country Name
        /// </summary>
        /// <param name="CountryName"></param>
        /// <returns>List of TLGX Accommodation Masters. Currently restricted to internal Name and Code data.</returns>
        [Route("Accommodation/CountryName/{CountryName}")]
        [HttpGet]
        [ResponseType(typeof(List<AccommodationMaster>))]
        public async Task<HttpResponseMessage> GetAccommodationByCountryName(string CountryName)
        {
            try
            {
                _database = MongoDBHandler.mDatabase();
                var collection = _database.GetCollection<AccommodationMaster>("AccommodationMaster");
                var result = await collection.Find(c => c.CountryName.ToUpper() == CountryName.Trim().ToUpper() && c.TLGXAccoId != null)
                    .Project(u => new AccommodationMasterRS
                    {
                        HotelID = u.TLGXAccoId,
                        HotelName = u.HotelName,
                        HotelType = u.ProductCategorySubType,
                        HotelStarRating = u.HotelStarRating,
                        StreetName = u.StreetName,
                        StreetNumber = u.StreetNumber,
                        Street3 = u.Street3,
                        Street4 = u.Street4,
                        Street5 = u.Street5,
                        PostalCode = u.PostalCode,
                        Town = u.Town,
                        Location = u.Location,
                        Area = u.Area,
                        CityCode = u.CityCode,
                        CityName = u.CityName,
                        StateCode = u.StateCode,
                        StateName = u.StateName,
                        CountryCode = u.CountryCode,
                        CountryName = u.CountryName,
                        FullAddress = u.FullAddress,
                        TelephoneNumber = u.Telephone,
                        Fax = u.Fax,
                        URL = u.WebSiteURL,
                        EmailAddress = u.Email,
                        Latitude = u.Latitude,
                        Longitude = u.Longitude


                    }).SortBy(s => s.TLGXAccoId).ToListAsync();
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
