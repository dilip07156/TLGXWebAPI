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
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace DistributionWebApi.Controllers
{
    /// <summary>
    /// Used to replicate EZ1 Legacy Product Distribution Web Services. 
    /// </summary>
    [RoutePrefix("hotelsws/api")]
    public class LegacyProductDetailsController : ApiController
    {
        /// <summary>
        /// Mongo database handler
        /// </summary>
        protected static IMongoDatabase _database;

        /// <summary>
        /// Retrieves Legacy Hotel Static Data definition based on TLGX Hotel Id
        /// </summary>
        /// <param name="id">TLGX Hotel Product Code</param>
        /// <returns>Return Legacy EZ1 Hotel Static Data definition</returns>
        [Route("ezeego1/id/{id}")]
        [ResponseType(typeof(Hotels))]
        public async Task<HttpResponseMessage> GetProductByEzeegoId(string id)
        {
            try
            {
                _database = MongoDBHandler.mDatabase();
                var collection = _database.GetCollection<HotelsHotel>("Accommodations");
                var result = await collection.Find(c => c.HotelId == id).ToListAsync();

                var hotels = new Hotels();
                hotels.Hotel = result;

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, hotels);
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
        /// Retrieves Legacy Hotel Static Data definition based on TLGX Hotel Id and outputs in the selected format
        /// </summary>
        /// <param name="id">TLGX Hotel Product Code</param>
        /// <param name="format">Format response should be returned in. Values are "JSON", "XML"</param>
        /// <returns>Return Legacy Hotel Static Data format</returns>
        [Route("ezeego1/id/{id}/format/{format}")]
        [ResponseType(typeof(Hotels))]
        public async Task<HttpResponseMessage> GetProductByEzeegoIdandFormat(string id, string format)
        {
            try
            {
                _database = MongoDBHandler.mDatabase();
                var collection = _database.GetCollection<HotelsHotel>("Accommodations");
                var result = await collection.Find(c => c.HotelId == id).ToListAsync();

                HttpResponseMessage response = new HttpResponseMessage();

                if (result == null || result.Count == 0)
                {
                    if (format.ToLower() == "xml")
                    {
                        response.Content = new StringContent("<message>Zero Results.</message>", Encoding.UTF8, "application/xml");
                        response.StatusCode = HttpStatusCode.NotFound;
                    }
                    else if (format.ToLower() == "json")
                    {
                        response.Content = new StringContent("Zero Results.", Encoding.UTF8, "application/json");
                        response.StatusCode = HttpStatusCode.NotFound;
                    }

                    return response;
                }

                var hotels = new Hotels();
                hotels.Hotel = result;

                //XmlWriterSettings writerSettings = new XmlWriterSettings();
                //writerSettings.OmitXmlDeclaration = true;
                //writerSettings.ConformanceLevel = ConformanceLevel.Fragment;
                //writerSettings.CloseOutput = false;
                //System.Text.StringBuilder localMemoryStream = new System.Text.StringBuilder();
                //using (XmlWriter writer = XmlWriter.Create(localMemoryStream, writerSettings))
                //{
                //    writer.WriteStartElement("Hotels");
                //    writer.WriteStartElement("Hotel");
                //    writer.WriteElementString("title", "A Programmer's Guide to ADO.NET");
                //    writer.WriteElementString("author", "Mahesh Chand");
                //    writer.WriteElementString("publisher", "APress");
                //    writer.WriteElementString("price", "44.95");
                //    writer.WriteEndElement();
                //    writer.Flush();
                //}

                XDocument doc = new XDocument();
                //doc.Add(
                //    new XElement("Hotels", new XElement
                //    ("Hotel", new XAttribute("SupplierHotelID", ""), new XElement("StarRating", new XAttribute("Level", 2)))));

                doc.Add(new XElement("Hotels"));

                foreach (var hotel in result)
                {
                    doc.Element("Hotels").Add(new XElement("Hotel"));
                    doc.Element("Hotels").Element("Hotel").Add(new XAttribute("SupplierHotelID", string.Empty));
                    doc.Element("Hotels").Element("Hotel").Add(new XAttribute("HotelId", hotel.HotelId));
                    doc.Element("Hotels").Element("Hotel").Add(new XAttribute("name", hotel.name));
                    doc.Element("Hotels").Element("Hotel").Add(new XAttribute("credicards", hotel.credicards));
                    doc.Element("Hotels").Element("Hotel").Add(new XAttribute("areatransportation", hotel.credicards));
                    doc.Element("Hotels").Element("Hotel").Add(new XAttribute("restaurants", hotel.restaurants));
                    doc.Element("Hotels").Element("Hotel").Add(new XAttribute("meetingfacility", hotel.meetingfacility));
                    doc.Element("Hotels").Element("Hotel").Add(new XAttribute("description", hotel.description));
                    doc.Element("Hotels").Element("Hotel").Add(new XAttribute("highlight", hotel.highlight));
                    doc.Element("Hotels").Element("Hotel").Add(new XAttribute("overview", hotel.overview));
                    doc.Element("Hotels").Element("Hotel").Add(new XAttribute("checkintime", hotel.checkintime));
                    doc.Element("Hotels").Element("Hotel").Add(new XAttribute("checkouttime", hotel.checkouttime));
                    doc.Element("Hotels").Element("Hotel").Add(new XAttribute("email", hotel.email));
                    doc.Element("Hotels").Element("Hotel").Add(new XAttribute("website", hotel.website));
                    doc.Element("Hotels").Element("Hotel").Add(new XAttribute("rooms", hotel.rooms));
                    doc.Element("Hotels").Element("Hotel").Add(new XAttribute("LandmarkCategory", hotel.LandmarkCategory));
                    doc.Element("Hotels").Element("Hotel").Add(new XAttribute("Landmark", hotel.Landmark));
                    doc.Element("Hotels").Element("Hotel").Add(new XAttribute("theme", hotel.theme));
                    doc.Element("Hotels").Element("Hotel").Add(new XAttribute("HotelChain", hotel.HotelChain));
                    doc.Element("Hotels").Element("Hotel").Add(new XAttribute("BrandName", hotel.BrandName));
                    doc.Element("Hotels").Element("Hotel").Add(new XAttribute("recommends", hotel.recommends));
                    doc.Element("Hotels").Element("Hotel").Add(new XAttribute("latitude", hotel.latitude));
                    doc.Element("Hotels").Element("Hotel").Add(new XAttribute("longitude", hotel.longitude));
                    doc.Element("Hotels").Element("Hotel").Add(new XAttribute("LandmarkDescription", hotel.LandmarkDescription));
                    doc.Element("Hotels").Element("Hotel").Add(new XAttribute("thumb", hotel.thumb));
                    doc.Element("Hotels").Element("Hotel").Add(new XElement("StarRating"));
                    doc.Element("Hotels").Element("Hotel").Element("StarRating").Add(new XAttribute("Level", hotel.StarRating.Level));
                    doc.Element("Hotels").Element("Hotel").Add(new XElement("Address"));
                    doc.Element("Hotels").Element("Hotel").Element("Address").Add(new XAttribute("address", hotel.Address.address));
                    doc.Element("Hotels").Element("Hotel").Element("Address").Add(new XAttribute("city", hotel.Address.city));
                    doc.Element("Hotels").Element("Hotel").Element("Address").Add(new XAttribute("state", hotel.Address.state));
                    doc.Element("Hotels").Element("Hotel").Element("Address").Add(new XAttribute("country", hotel.Address.country));
                    doc.Element("Hotels").Element("Hotel").Element("Address").Add(new XAttribute("pincode", hotel.Address.pincode));
                    doc.Element("Hotels").Element("Hotel").Element("Address").Add(new XAttribute("location", hotel.Address.location));
                    doc.Element("Hotels").Element("Hotel").Element("Address").Add(new XAttribute("phone", hotel.Address.phone));
                    doc.Element("Hotels").Element("Hotel").Element("Address").Add(new XAttribute("fax", hotel.Address.fax));

                    foreach (var img in hotel.image)
                    {
                        doc.Element("Hotels").Element("Hotel").Add(new XElement("image", new XAttribute("path", img.path)));
                    }

                    doc.Element("Hotels").Element("Hotel").Add(new XElement("video"));

                    doc.Element("Hotels").Element("Hotel").Add(new XElement("HotelFacility"));
                    foreach (var facility in hotel.HotelFacility)
                    {
                        doc.Element("Hotels").Element("Hotel").Element("HotelFacility").Add(new XElement("Facility", new XAttribute("name", facility.name)));
                    }

                    doc.Element("Hotels").Element("Hotel").Add(new XElement("HotelAmenity"));
                    doc.Element("Hotels").Element("Hotel").Element("HotelAmenity").Add(new XAttribute("Restaurant", hotel.HotelAmenity.Restaurant ? 1 : 0));
                    doc.Element("Hotels").Element("Hotel").Element("HotelAmenity").Add(new XAttribute("conference", hotel.HotelAmenity.conference ? 1 : 0));
                    doc.Element("Hotels").Element("Hotel").Element("HotelAmenity").Add(new XAttribute("fitness", hotel.HotelAmenity.fitness ? 1 : 0));
                    doc.Element("Hotels").Element("Hotel").Element("HotelAmenity").Add(new XAttribute("travel", hotel.HotelAmenity.travel ? 1 : 0));
                    doc.Element("Hotels").Element("Hotel").Element("HotelAmenity").Add(new XAttribute("forex", hotel.HotelAmenity.forex ? 1 : 0));
                    doc.Element("Hotels").Element("Hotel").Element("HotelAmenity").Add(new XAttribute("shopping", hotel.HotelAmenity.shopping ? 1 : 0));
                    doc.Element("Hotels").Element("Hotel").Element("HotelAmenity").Add(new XAttribute("banquet", hotel.HotelAmenity.banquet ? 1 : 0));
                    doc.Element("Hotels").Element("Hotel").Element("HotelAmenity").Add(new XAttribute("games", hotel.HotelAmenity.games ? 1 : 0));
                    doc.Element("Hotels").Element("Hotel").Element("HotelAmenity").Add(new XAttribute("Bar", hotel.HotelAmenity.Bar ? 1 : 0));
                    doc.Element("Hotels").Element("Hotel").Element("HotelAmenity").Add(new XAttribute("Coffee_Shop", hotel.HotelAmenity.Coffee_Shop ? 1 : 0));
                    doc.Element("Hotels").Element("Hotel").Element("HotelAmenity").Add(new XAttribute("Room_Service", hotel.HotelAmenity.Room_Service ? 1 : 0));
                    doc.Element("Hotels").Element("Hotel").Element("HotelAmenity").Add(new XAttribute("Internet_Access", hotel.HotelAmenity.Internet_Access ? 1 : 0));
                    doc.Element("Hotels").Element("Hotel").Element("HotelAmenity").Add(new XAttribute("Business_Centre", hotel.HotelAmenity.Business_Centre ? 1 : 0));
                    doc.Element("Hotels").Element("Hotel").Element("HotelAmenity").Add(new XAttribute("Swimming_Pool", hotel.HotelAmenity.Swimming_Pool ? 1 : 0));
                    doc.Element("Hotels").Element("Hotel").Element("HotelAmenity").Add(new XAttribute("Pets", hotel.HotelAmenity.Pets ? 1 : 0));
                    doc.Element("Hotels").Element("Hotel").Element("HotelAmenity").Add(new XAttribute("Tennis_Court", hotel.HotelAmenity.Tennis_Court ? 1 : 0));
                    doc.Element("Hotels").Element("Hotel").Element("HotelAmenity").Add(new XAttribute("Golf", hotel.HotelAmenity.Golf ? 1 : 0));
                    doc.Element("Hotels").Element("Hotel").Element("HotelAmenity").Add(new XAttribute("Air_Conditioning", hotel.HotelAmenity.Air_Conditioning ? 1 : 0));
                    doc.Element("Hotels").Element("Hotel").Element("HotelAmenity").Add(new XAttribute("Parking", hotel.HotelAmenity.Parking ? 1 : 0));
                    doc.Element("Hotels").Element("Hotel").Element("HotelAmenity").Add(new XAttribute("Wheel_Chair", hotel.HotelAmenity.Wheel_Chair ? 1 : 0));
                    doc.Element("Hotels").Element("Hotel").Element("HotelAmenity").Add(new XAttribute("Health_Club", hotel.HotelAmenity.Health_Club ? 1 : 0));

                    doc.Element("Hotels").Element("Hotel").Add(new XElement("HotelDistance"));
                    doc.Element("Hotels").Element("Hotel").Element("HotelDistance").Add(new XAttribute("DistancefromAirport", hotel.HotelDistance.DistancefromAirport ?? string.Empty));
                    doc.Element("Hotels").Element("Hotel").Element("HotelDistance").Add(new XAttribute("DistancefromStation", hotel.HotelDistance.DistancefromStation ?? string.Empty));
                    doc.Element("Hotels").Element("Hotel").Element("HotelDistance").Add(new XAttribute("DistancefromBus", hotel.HotelDistance.DistancefromBus ?? string.Empty));
                    doc.Element("Hotels").Element("Hotel").Element("HotelDistance").Add(new XAttribute("DistancefromCityCenter", hotel.HotelDistance.DistancefromCityCenter ?? string.Empty));
                }

                if (format.ToLower() == "xml")
                {
                    response.Content = new StringContent(doc.ToString(), Encoding.UTF8, "application/xml");
                    response.StatusCode = HttpStatusCode.OK;
                    //response = Request.CreateResponse(HttpStatusCode.OK, localMemoryStream.ToString(), "application/xml");
                }
                else if (format.ToLower() == "json")
                {
                    response = Request.CreateResponse(HttpStatusCode.OK, hotels, "application/json");
                }

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
        /// Retrieves Legacy Hotel Static Data definition based on TLGX Supplier Name and Supplier-specific Product Code
        /// </summary>
        /// <param name="spname">TLGX Supplier Name</param>
        /// <param name="id">Supplier-specific Product Id (multiple as comma separated values)</param>
        /// <returns>Return Legacy Hotel Static Data format</returns>
        [Route("supplier/spname/{spname}/id/{id}")]
        [ResponseType(typeof(Hotels))]
        public async Task<HttpResponseMessage> GetProductDetailsBy_SuppName_SupHtlId(string spname, string id)
        {
            try
            {
                _database = MongoDBHandler.mDatabase();
                var accomodationsList = _database.GetCollection<HotelsHotel>("Accommodations");
                var productMappingList = _database.GetCollection<ProductMapping>("ProductMapping");

                FilterDefinition<HotelsHotel> filterAcco;
                FilterDefinition<ProductMapping> filterPM;
                var idList = id.Split(',');

                XDocument doc = new XDocument();
                doc.Add(new XElement("Hotels"));

                Hotels _Hotels = new Hotels();
                List<HotelsHotel> _HotelsHotel = new List<HotelsHotel>();

                foreach (var hotelId in idList)
                {
                    var searchResultPM = await productMappingList.Find(x => (x.SupplierCode.ToLower() == spname.ToLower() && x.SupplierProductCode.ToLower() == hotelId.ToLower())).ToListAsync();

                    foreach (var resultPM in searchResultPM)
                    {
                        var searchResultAcco = await accomodationsList.Find(x => (x.HotelId.ToLower() == resultPM.SystemProductCode.ToLower())).ToListAsync();
                        foreach (var hotel in searchResultAcco)
                        {
                            hotel.SupplierHotelID = hotelId;
                            _HotelsHotel.Add(hotel);
                        }

                    }

                }

                _Hotels.Hotel = _HotelsHotel;

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, _Hotels);
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
        /// Retrieves Legacy Hotel Static Data definition based on TLGX Supplier Name and Supplier-specific Product Code
        /// </summary>
        /// <param name="spname">TLGX Supplier Name</param>
        /// <param name="id">Supplier-specific Product Id</param>
        /// <param name="format">Format in XML or JSON</param>
        /// <returns>Return Legacy Hotel Static Data format</returns>
        [Route("supplier/spname/{spname}/id/{id}/format/{format}")]
        [ResponseType(typeof(Hotels))]
        public async Task<HttpResponseMessage> GetProductDetailsBy_SuppName_SupHtlId_Format(string spname, string id, string format)
        {
            try
            {
                _database = MongoDBHandler.mDatabase();
                var accomodationsList = _database.GetCollection<HotelsHotel>("Accommodations");
                var productMappingList = _database.GetCollection<ProductMapping>("ProductMapping");

                FilterDefinition<HotelsHotel> filterAcco;
                FilterDefinition<ProductMapping> filterPM;
                var idList = id.Split(',');

                XDocument doc = new XDocument();
                doc.Add(new XElement("Hotels"));

                Hotels _Hotels = new Hotels();
                List<HotelsHotel> _HotelsHotel = new List<HotelsHotel>();

                foreach (var hotelId in idList)
                {
                    var searchResultPM = await productMappingList.Find(x => (x.SupplierCode.ToLower() == spname.ToLower() && x.SupplierProductCode.ToLower() == hotelId.ToLower())).ToListAsync();

                    foreach (var resultPM in searchResultPM)
                    {
                        var searchResultAcco = await accomodationsList.Find(x => (x.HotelId.ToLower() == resultPM.SystemProductCode.ToLower())).ToListAsync();
                        foreach (var hotel in searchResultAcco)
                        {
                            hotel.SupplierHotelID = hotelId;
                            _HotelsHotel.Add(hotel);
                        }

                    }

                }

                _Hotels.Hotel = _HotelsHotel;

                HttpResponseMessage response = new HttpResponseMessage();
                if (format.ToLower() == "xml")
                {
                    StringWriter sw = new StringWriter();
                    XmlTextWriter tw = null;
                    XmlSerializer serializer = new XmlSerializer(_Hotels.GetType());
                    tw = new XmlTextWriter(sw);
                    serializer.Serialize(tw, _Hotels);
                    sw.Close();
                    if (tw != null)
                    {
                        tw.Close();
                    }

                    response.Content = new StringContent(sw.ToString(), Encoding.UTF8, "application/xml");
                    response.StatusCode = HttpStatusCode.OK;
                }
                else if (format.ToLower() == "json")
                {
                    response = Request.CreateResponse(HttpStatusCode.OK, _Hotels, "application/json");
                }
                response.StatusCode = HttpStatusCode.OK;
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
        /// Retrieves Legacy Hotel Static Data definition based on TLGX Supplier Name and Supplier-specific City Name
        /// </summary>
        /// <param name="spname">TLGX Supplier Name</param>
        /// <param name="city">Supplier-specific City Name</param>
        /// <returns>Return Legacy Hotel Static Data format</returns>
        [Route("supplier/spname/{spname}/city/{city}")]
        [ResponseType(typeof(Hotels))]
        public async Task<HttpResponseMessage> GetProductDetailsBy_SuppName_SupCityName(string spname, string city)
        {
            try
            {
                _database = MongoDBHandler.mDatabase();
                var accomodationsList = _database.GetCollection<HotelsHotel>("Accommodations");
                var productMappingList = _database.GetCollection<ProductMapping>("ProductMapping");

                var idList = await productMappingList.Find(x => (x.SupplierCode.ToLower() == spname.ToLower() && x.SupplierCityName.ToLower() == city.ToLower())).ToListAsync();

                XDocument doc = new XDocument();
                doc.Add(new XElement("Hotels"));

                Hotels _Hotels = new Hotels();
                List<HotelsHotel> _HotelsHotel = new List<HotelsHotel>();

                foreach (var hotelId in idList)
                {
                    var searchResultAcco = await accomodationsList.Find(x => (x.HotelId.ToLower() == hotelId.SystemProductCode.ToLower())).ToListAsync();
                    foreach (var hotel in searchResultAcco)
                    {
                        hotel.SupplierHotelID = hotelId.SupplierProductCode;
                        _HotelsHotel.Add(hotel);
                    }
                }

                _Hotels.Hotel = _HotelsHotel;

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, _Hotels);
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
        /// Retrieves Legacy Hotel Static Data definition based on TLGX Supplier Name and Supplier-specific City Name
        /// </summary>
        /// <param name="spname">TLGX Supplier Name</param>
        /// <param name="city">Supplier-specific City Name</param>
        /// <param name="format">Format in XML or JSON</param>
        /// <returns>Return Legacy Hotel Static Data format</returns>
        [Route("supplier/spname/{spname}/city/{city}/format/{format}")]
        [ResponseType(typeof(Hotels))]
        public async Task<HttpResponseMessage> GetProductDetailsBy_SuppName_SupCityName_Format(string spname, string city, string format)
        {

            try
            {
                _database = MongoDBHandler.mDatabase();
                var accomodationsList = _database.GetCollection<HotelsHotel>("Accommodations");
                var productMappingList = _database.GetCollection<ProductMapping>("ProductMapping");

                var idList = await productMappingList.Find(x => (x.SupplierCode.ToLower() == spname.ToLower() && x.SupplierCityName.ToLower() == city.ToLower())).ToListAsync();

                XDocument doc = new XDocument();
                doc.Add(new XElement("Hotels"));

                Hotels _Hotels = new Hotels();
                List<HotelsHotel> _HotelsHotel = new List<HotelsHotel>();

                foreach (var hotelId in idList)
                {
                    var searchResultAcco = await accomodationsList.Find(x => (x.HotelId.ToLower() == hotelId.SystemProductCode.ToLower())).ToListAsync();
                    foreach (var hotel in searchResultAcco)
                    {
                        hotel.SupplierHotelID = hotelId.SupplierProductCode;
                        _HotelsHotel.Add(hotel);
                    }
                }

                _Hotels.Hotel = _HotelsHotel;

                HttpResponseMessage response = new HttpResponseMessage();
                if (format.ToLower() == "xml")
                {
                    StringWriter sw = new StringWriter();
                    XmlTextWriter tw = null;
                    XmlSerializer serializer = new XmlSerializer(_Hotels.GetType());
                    tw = new XmlTextWriter(sw);
                    serializer.Serialize(tw, _Hotels);
                    sw.Close();
                    if (tw != null)
                    {
                        tw.Close();
                    }

                    response.Content = new StringContent(sw.ToString(), Encoding.UTF8, "application/xml");
                    response.StatusCode = HttpStatusCode.OK;
                }
                else if (format.ToLower() == "json")
                {
                    response = Request.CreateResponse(HttpStatusCode.OK, _Hotels, "application/json");
                }
                response.StatusCode = HttpStatusCode.OK;
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