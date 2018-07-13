using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

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

    [BsonIgnoreExtraElements]
    public class State
    {
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

    /// <summary>
    /// Structure of Oag Port with system geo info
    /// </summary>
    [BsonIgnoreExtraElements]
    public class Port
    {
        /// <summary>
        /// The port or city code, e.g. LGW, STN or LON.
        /// </summary>
        public string oag_location_code { get; set; }

        /// <summary>
        /// The City code that the port belongs to if applicable e.g. LON.
        /// </summary>
        public string oag_multi_airport_citycode { get; set; }

        /// <summary>
        /// Location Type Code
        /// L = Location with one port, e.g., AMS. 
        /// A = Airport belonging to multi airport city, e.g., LHR, LGW, STN, LCY & LTN.
        /// M = Multi airport city, e.g., LON.
        /// </summary>
        public string oag_location_type_code { get; set; }

        /// <summary>
        /// Location Type
        /// </summary>
        public string oag_location_type { get; set; }

        /// <summary>
        /// Location Sub-Type code
        /// A = Airport
        /// B = Bus Station
        /// H = Harbour
        /// O = Off - line Point
        /// R = Rail Station U = Metro / Underground
        /// V = Miscellaneous
        /// Blank = Multi Airport City
        /// </summary>
        public string oag_location_subtype_code { get; set; }

        /// <summary>
        /// Location Sub-Type
        /// </summary>
        public string oag_location_subtype { get; set; }

        /// <summary>
        /// The location name is not always the same as the airport especially with L type records like AMS where the location name is Amsterdam rather than Amsterdam Schiphol. IATA does not collect all specific airport names. “A” type records will have the airport name, e.g., LGW = London Gatwick.
        /// </summary>
        public string oag_location_name { get; set; }

        /// <summary>
        /// Port Name
        /// </summary>
        public string oag_port_name { get; set; }

        /// <summary>
        /// The ISO country code.
        /// </summary>
        public string oag_country_code { get; set; }

        /// <summary>
        /// Some countries are split into regions, or separate out remote island groups, e.g. Russia is split into East & West along a line approximating to the position of the Urals, Portugal has two sub-countries allowing identification of Madeira and The Azores.
        /// </summary>
        public string oag_country_subcode { get; set; }

        /// <summary>
        /// The ISO country name.
        /// </summary>
        public string oag_country_name { get; set; }

        /// <summary>
        /// Some countries are divided into states. Two letter state codes are used.
        /// </summary>
        public string oag_state_code { get; set; }

        /// <summary>
        /// Sub-codes are not currently used.
        /// </summary>
        public string oag_state_subcode { get; set; }

        /// <summary>
        /// Some countries, e.g. USA, Canada and Russia are split up into various time zones. These can be linked to a Daylight Saving time file if required.
        /// </summary>
        public string oag_time_division { get; set; }

        /// <summary>
        /// Latitude in decimal
        /// </summary>
        public string oag_latitiude { get; set; }

        /// <summary>
        /// Longitude in decimal
        /// </summary>
        public string oag_longitude { get; set; }

        /// <summary>
        /// A check is made for each location, against the schedules database, and if no schedules are operating to/from that location it is marked with an ‘I’ to indicate that it is currently inactive.
        /// </summary>
        public string oag_inactive_indicator { get; set; }

        /// <summary>
        /// System's country code
        /// </summary>
        public string tlgx_country_code { get; set; }

        /// <summary>
        /// System's country name
        /// </summary>
        public string tlgx_country_name { get; set; }

        /// <summary>
        /// System's state code
        /// </summary>
        public string tlgx_state_code { get; set; }

        /// <summary>
        /// System's state name
        /// </summary>
        public string tlgx_state_name { get; set; }

        /// <summary>
        /// System's city code
        /// </summary>
        public string tlgx_city_code { get; set; }

        /// <summary>
        /// System's city name
        /// </summary>
        public string tlgx_city_name { get; set; }
    }

    /// <summary>
    /// Structure of Active Ports
    /// </summary>
    [BsonIgnoreExtraElements]
    public class Active_Port
    {
        /// <summary>
        /// The port or city code, e.g. LGW, STN or LON.
        /// </summary>
        public string oag_location_code { get; set; }

        /// <summary>
        /// The City code that the port belongs to if applicable e.g. LON.
        /// </summary>
        public string oag_multi_airport_citycode { get; set; }

        /// <summary>
        /// Location Type Code
        /// L = Location with one port, e.g., AMS. 
        /// A = Airport belonging to multi airport city, e.g., LHR, LGW, STN, LCY & LTN.
        /// M = Multi airport city, e.g., LON.
        /// </summary>
        public string oag_location_type_code { get; set; }

        /// <summary>
        /// Location Type
        /// </summary>
        public string oag_location_type { get; set; }

        /// <summary>
        /// Location Sub-Type code
        /// A = Airport
        /// B = Bus Station
        /// H = Harbour
        /// O = Off - line Point
        /// R = Rail Station U = Metro / Underground
        /// V = Miscellaneous
        /// Blank = Multi Airport City
        /// </summary>
        public string oag_location_subtype_code { get; set; }

        /// <summary>
        /// Location Sub-Type
        /// </summary>
        public string oag_location_subtype { get; set; }

        /// <summary>
        /// The location name is not always the same as the airport especially with L type records like AMS where the location name is Amsterdam rather than Amsterdam Schiphol. IATA does not collect all specific airport names. “A” type records will have the airport name, e.g., LGW = London Gatwick.
        /// </summary>
        public string oag_location_name { get; set; }

        /// <summary>
        /// Port Name
        /// </summary>
        public string oag_port_name { get; set; }

        /// <summary>
        /// The ISO country code.
        /// </summary>
        public string oag_country_code { get; set; }

        /// <summary>
        /// Some countries are split into regions, or separate out remote island groups, e.g. Russia is split into East & West along a line approximating to the position of the Urals, Portugal has two sub-countries allowing identification of Madeira and The Azores.
        /// </summary>
        public string oag_country_subcode { get; set; }

        /// <summary>
        /// The ISO country name.
        /// </summary>
        public string oag_country_name { get; set; }

        /// <summary>
        /// Some countries are divided into states. Two letter state codes are used.
        /// </summary>
        public string oag_state_code { get; set; }

        /// <summary>
        /// Sub-codes are not currently used.
        /// </summary>
        public string oag_state_subcode { get; set; }

        /// <summary>
        /// Some countries, e.g. USA, Canada and Russia are split up into various time zones. These can be linked to a Daylight Saving time file if required.
        /// </summary>
        public string oag_time_division { get; set; }

        /// <summary>
        /// Latitude in decimal
        /// </summary>
        public string oag_latitiude { get; set; }

        /// <summary>
        /// Longitude in decimal
        /// </summary>
        public string oag_longitude { get; set; }

        /// <summary>
        /// A check is made for each location, against the schedules database, and if no schedules are operating to/from that location it is marked with an ‘I’ to indicate that it is currently inactive.
        /// </summary>
        public string oag_inactive_indicator { get; set; }

        /// <summary>
        /// System's country code
        /// </summary>
        public string tlgx_country_code { get; set; }

        /// <summary>
        /// System's country name
        /// </summary>
        public string tlgx_country_name { get; set; }

        /// <summary>
        /// System's state code
        /// </summary>
        public string tlgx_state_code { get; set; }

        /// <summary>
        /// System's state name
        /// </summary>
        public string tlgx_state_name { get; set; }

        /// <summary>
        /// System's city code
        /// </summary>
        public string tlgx_city_code { get; set; }

        /// <summary>
        /// System's city name
        /// </summary>
        public string tlgx_city_name { get; set; }
    }

    /// <summary>
    /// structure of Zone Master
    /// </summary>
    [BsonIgnoreExtraElements]
    public class Zone
    {
        /// <summary>
        /// Type of the Zone.This field is mandatory.
        /// </summary>
        public string Zone_Type { get; set; }
        /// <summary>
        /// Sub Type Of the Zone.This field is optional.
        /// </summary>
        public string Zone_SubType { get; set; }
        /// <summary>
        /// Name of the zone.This field is mandatory.
        /// </summary>
        public string Zone_Name { get; set; }
        /// <summary>
        /// Latitude of Zone.This field is mandatory
        /// </summary>
        public string Latitude { get; set; }
        /// <summary>
        /// Longitude of Zone.This field is mandatory
        /// </summary>
        public string Longitude { get; set; }
        /// <summary>
        /// Radius(in km) for zone from LatLong. Upto this range,Hotels are included in Zone.This field is mandatory
        /// </summary>
        public decimal? Zone_Radius { get; set; }
        /// <summary>
        /// TLGX Country Code .This field is mandatory
        /// </summary>
        public string TLGXCountryCode { get; set; }
        /// <summary>
        /// List Of TLGX City Codes mapped to Zone
        /// </summary>
        public List<Zone_CityMapping> Zone_CityMapping { get; set; }
        /// <summary>
        /// List Of Hotels within 10km range from  lat-Long of zone
        /// </summary>
        public List<Zone_ProductMapping> Zone_ProductMapping { get; set; }


    }
    /// <summary>
    /// Structure of Cities mapped to zone
    /// </summary>
    [BsonIgnoreExtraElements]
    public class Zone_CityMapping
    {
        /// <summary>
        /// TLGX City Code .This field is dependant on TLGX CountryCode of Zone and is optional.
        /// </summary>
        public string TLGXCityCode { get; set; }
    }
    /// <summary>
    /// structure of  Hotels within 10 km of zone
    /// </summary>
    [BsonIgnoreExtraElements]
    public class Zone_ProductMapping
    {
        /// <summary>
        /// Unique Id of TLGX Hotel(TLGX HotelCode)
        /// </summary>
        public int? TLGXCompanyHotelID { get; set; }
        /// <summary>
        /// TLGX Hotel Name
        /// </summary>
        public string TLGXHotelName { get; set; }
        /// <summary>
        /// TLGX Hotel Type
        /// </summary>
        public string TLGXProductType { get; set; }
        /// <summary>
        /// Distance Of Hotel(in Km) from Latitude-Longitude of Zone.
        /// </summary>
        public decimal? Distance { get; set; }
        /// <summary>
        /// Unit of Distance
        /// </summary>
        public string Unit { get; set; }
        /// <summary>
        /// If this hotel is Included in Zone or Not.
        /// </summary>
        public bool? IsIncluded { get; set; }
    }
    /// <summary>
    /// Structure for Zone Search Rquest Parameters.
    /// </summary>

    public class ZoneSearchRQ
    {
        /// <summary>
        /// Search by Zone Name.This is mandatory field. Name Should be atleast 3chars.
        /// </summary>
        [Required]
        public string Zone_name { get; set; }
        /// <summary>
        /// Search Zones by System CountryCode.This is optional field.
        /// </summary>
        public string SystemCountryCode { get; set; }
        /// <summary>
        /// Search Zone by its Type. This is optional field.To get ZoneTypes use "ZoneTypeMaster" service
        /// </summary>
        public string Zone_Type { get; set; }
        /// <summary>
        /// Zone Sub types are dependant on ZoneTypes. This is optional field. To get ZoneSubTypes use "ZoneTypeMaster" service
        /// </summary>
        public string Zone_SubType { get; set; }

    }
    /// <summary>
    /// ZoneTypes and Subtypes
    /// </summary>
    [BsonIgnoreExtraElements]
    public class ZoneTypeMaster
    {
        /// <summary>
        /// Type of Zone
        /// </summary>
        public string Zone_Type { get; set; }
        /// <summary>
        /// SubTypes of ZoneType
        /// </summary>
        public List<string> Zone_SubTypes { get; set; }
    }
}