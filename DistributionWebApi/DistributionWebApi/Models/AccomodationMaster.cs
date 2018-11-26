using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.Serialization.Attributes;

namespace DistributionWebApi.Models
{
    [BsonIgnoreExtraElements]
    public class DC_AccomodationMasterMapping
    {
        [BsonId]
        [Newtonsoft.Json.JsonProperty("_id")]
        public int CommonHotelId { get; set; }
        public string HotelName { get; set; }
        public string ProductCategory { get; set; }
        public string ProductCategorySubType { get; set; }
        public string Brand { get; set; }
        public string Chain { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string CityCode { get; set; }
        public string CityName { get; set; }
        public string StateCode { get; set; }
        public string StateName { get; set; }
        public string StreetName { get; set; }
        public string StreetNumber { get; set; }
        public string Street4 { get; set; }
        public string Street5 { get; set; }
        public string PostalCode { get; set; }
        public string Town { get; set; }
        public string Location { get; set; }
        public string Area { get; set; }
        public string TLGXAccoId { get; set; }
        public bool IsRoomMappingCompleted { get; set; }
        public string Email { get; set; }
        public string Fax { get; set; }
        public string WebSiteURL { get; set; }
        public string Telephone { get; set; }
        public string FullAddress { get; set; }
        public string HotelStarRating { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }
}




