using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using MongoDB.Bson.Serialization.Attributes;

namespace DistributionWebApi.Models
{
    [BsonIgnoreExtraElements]
    public class Country
    {
        /// <summary>
        /// TLGX Country Name
        /// </summary>
        public string CountryName { get; set; }
        /// <summary>
        /// TLGX Country Code. This field should be unique.
        /// </summary>
        public string CountryCode { get; set; }

    }

    [BsonIgnoreExtraElements]
    public class City
    {
        /// <summary>
        /// TLGX City Name
        /// </summary>
        public string CityName { get; set; }
        /// <summary>
        /// TLGX City Code. This value should be unique and consists of TLGX Country Code and alphanumeric identifier.
        /// </summary>
        public string CityCode { get; set; }
        /// <summary>
        /// The name of the State. This field is optional and is derived from ISO3166-2 where possible.
        /// </summary>
        public string StateName { get; set; }
        /// <summary>
        /// The Code for the State. This field is optional and is derived from ISO3166-2 where possible.
        /// </summary>
        public string StateCode { get; set; }
        /// <summary>
        /// TLGX Country Name
        /// </summary>
        public string CountryName { get; set; }
        /// <summary>
        /// TLGX Country Code. This field should be unique.
        /// </summary>
        public string CountryCode { get; set; }

    }

    [BsonIgnoreExtraElements]
    public class Supplier
    {
        /// <summary>
        /// TLGX Mapping Supplier Name
        /// </summary>
        public string SupplierName { get; set; }
        /// <summary>
        /// TLGX Mapping Supplier Code. This value should be unique.
        /// </summary>
        public string SupplierCode { get; set; }
        /// <summary>
        /// TLGX Supplier Owner Company
        /// </summary>
        public string SupplierOwner { get; set; }
        /// <summary>
        /// Type of Supplier
        /// </summary>
        public string SupplierType { get; set; }
    }
}