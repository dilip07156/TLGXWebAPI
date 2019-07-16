using DistributionWebApi.ZoneModels;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DistributionWebApi.Models
{
    [BsonIgnoreExtraElements]
    public class Country
    {
        /// <summary>
        /// Gets or sets the value of continent name
        /// </summary>
        public string ContinentName { get; set; }

        /// <summary>
        /// Gets or sets the value of continent code
        /// </summary>
        public string ContinentCode { get; set; }

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
        
        /// <summary>
        /// Gets or sets the value of continent name
        /// </summary>
        public string ContinentName { get; set; }

        /// <summary>
        /// Gets or sets the value of continent code
        /// </summary>
        public string ContinentCode { get; set; }
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
    #region ===Zone
    /// <summary>
    /// structure of Zone Master
    /// </summary>
    public class ZoneMaster
    {
        /// <summary>
        /// unique id of Zone
        /// </summary>
        public string _id { get; set; }
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
        public string Zone_Radius { get; set; }
        /// <summary>
        /// TLGX Country Code .This field is mandatory
        /// </summary>
        public string TLGXCountryCode { get; set; }
        /// <summary>
        /// Zone Code .This field is mandatory
        /// </summary>
        public string Zone_Code { get; set; }
        /// <summary>
        /// List Of TLGX City Codes mapped to Zone
        /// </summary>
        public List<Zone_CityMapping> Zone_CityMapping { get; set; }
        /// <summary>
        /// List Of Hotels within 10km range from  lat-Long of zone
        /// </summary>
        public List<Zone_ProductMapping> Zone_ProductMapping { get; set; }
        /// <summary>
        /// List Of Geography Hiearchy for zone
        /// </summary>
        public List<Zone_GeographyMapping> Zone_GeographyMapping { get; set; }
        /// <summary>
        /// List Of Location Mapping Data for Zone with in 10 KM Range
        /// </summary>
        public List<Zone_LocationMapping> Zone_LocationMapping { get; set; }
        /// <summary>
        /// Object Containg Data for finding Near Location For Given Distance.
        /// </summary>
        public Zone_Geometry loc { get; set; }
        /// <summary>
        /// House Number Of Zone Name
        /// </summary>
        public string Zone_House_Number { get; set; }
        /// <summary>
        /// Street One Part Of Address Of Zone Name
        ///  </summary>
        public string Zone_Street_One { get; set; }
        /// <summary>
        /// Street Two Part Of Address Of Zone Name
        /// </summary>
        public string Zone_Street_Two { get; set; }
        /// <summary>
        /// Street Three Part Of Address Of Zone Name
        /// </summary>
        public string Zone_Street_Three { get; set; }
        /// <summary>
        /// City Name Under which That Zone Exist
        /// </summary>
        public string Zone_City { get; set; }
        /// <summary>
        /// City Area Name Under which That Zone Exist
        /// </summary>
        public string Zone_City_Area { get; set; }
        /// <summary>
        /// City Area Location Name Under which That Zone Exist
        /// </summary>
        public string Zone_City_Area_Location { get; set; }
        /// <summary>
        /// Zonal Postal Code Zone Name
        /// </summary>
        public string Zone_Postal_Code { get; set; }
        /// <summary>
        /// Combined Full Address For Zone Name
        /// </summary>
        public string Zone_Full_Adress { get; set; }

    }

    /// <summary>
    /// structure of Zone Location Master
    /// </summary>
    public class SupplierZoneMaster
    {
        /// <summary>
        /// unique id of ZoneSupplier
        /// </summary>
        public object _id { get; set; }
        /// <summary>
        /// SupplierName for ZoneSupplier Master
        /// </summary>
        public string Supplier_Name { get; set; }
        /// <summary>
        /// SupplierCode for ZoneSupplier Master
        /// </summary>
        public string Supplier_code { get; set; }
        /// <summary>
        /// Name of supplierZone Master
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Code of supplierZone Master
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Zone Type of supplierZone Master
        /// </summary>
        public string ZoneType { get; set; }
        /// <summary>
        /// Zone sub Type of supplierZone Master
        /// </summary>
        public string ZoneSubType { get; set; }
        /// <summary>
        /// Latitude of Address part  of supplierZone Master
        /// </summary>
        public double Latitude { get; set; }
        /// <summary>
        /// Longitude of Address part  of supplierZone Master
        /// </summary>
        public double Longitude { get; set; }
        /// <summary>
        /// House no of Address part  of supplierZone Master
        /// </summary>
        public string HouseNumber { get; set; }
        /// <summary>
        /// StreetName of Address part  of supplierZone Master
        /// </summary>
        public string StreetName { get; set; }
        /// <summary>
        /// Street2 of Address part  of supplierZone Master
        /// </summary>
        public string Street2 { get; set; }
        /// <summary>
        /// Street3 of Address part  of supplierZone Master
        /// </summary>
        public string Street3 { get; set; }
        /// <summary>
        /// CityName of Address part  of supplierZone Master
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// CityAreaName of Address part  of supplierZone Master
        /// </summary>
        public string CityArea { get; set; }
        /// <summary>
        /// CityAreaLocationName of Address part  of supplierZone Master
        /// </summary>
        public string CityAreaLocation { get; set; }
        /// <summary>
        /// stateCode of Address part  of supplierZone Master
        /// </summary>
        public string StateCode { get; set; }
        /// <summary>
        /// StateName of Address part  of supplierZone Master
        /// </summary>
        public string StateName { get; set; }
        /// <summary>
        /// CountryCode of Address part  of supplierZone Master
        /// </summary>
        public string CountryCode { get; set; }
        /// <summary>
        /// PostalCode of Address part  of supplierZone Master
        /// </summary>
        public string PostalCode { get; set; }
        /// <summary>
        /// FullAdress of Address part  of supplierZone Master
        /// </summary>
        public string FullAdress { get; set; }
    }



    /// <summary>
    /// structure of Zone Master
    /// </summary>
    public class ZoneMappingLocationResponse
    {
        /// <summary>
        /// unique id of Zone
        /// </summary>
        public string Zone_Code { get; set; }
        /// <summary>
        /// Type of the Zone.This field is mandatory.
        /// </summary>
        public string Supplier_code { get; set; }
        /// <summary>
        /// Sub Type Of the Zone.This field is optional.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Name of the zone.This field is mandatory.
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Sub Type Of the Zone.This field is optional.
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// Sub Type Of the Zone.This field is optional.
        /// </summary>
        public string SubType { get; set; }
        /// <summary>
        /// Radius(in km) for zone from LatLong. Upto this range,Hotels are included in Zone.This field is mandatory
        /// </summary>
        public double Distance { get; set; }
        /// <summary>
        /// Latitude of Zone.This field is mandatory
        /// </summary>
        public double Latitude { get; set; }
        /// <summary>
        /// Longitude of Zone.This field is mandatory
        /// </summary>
        public double Longitude { get; set; }
        /// <summary>
        /// FullAddress For SupplierZone Master
        /// </summary>
        public string FullAddress { get; set; }

    }


    ///<summary>
    ///This is the format for Zone Geometry
    /// </summary>

    public class Zone_Geometry
    {
        /// <summary>
        /// type part of Geometry Column
        /// </summary>
        
        public string type { get; set; }
        /// <summary>
        /// Zone Coordinates In form longtitude and latitude in decimals in Array
        /// </summary>
        
        public List<double> coordinates { get; set; }
    }
    ///<summary>
    ///This is the format for Mapping Locations
    /// </summary>

    public class Zone_LocationMapping
    {
        
        /// <summary>
        /// Zone Supplier Zone Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Zone Supplier Zone Code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Distance between Zone master to its respective supplier Mapping Location
        /// </summary>
        public double Distance { get; set; }

        /// <summary>
        /// Zone Supplier Zone Type
        /// </summary>
        public string ZoneType { get; set; }

        /// <summary>
        /// Zone supplier Zone Sub Type 
        /// </summary>
        public string ZoneSubType { get; set; }
        /// <summary>
        /// House Number Part Zone Adress.
        /// </summary>
        public string HouseNumber { get; set; }
        /// <summary>
        /// Street One Part Of Zone Adress
        /// </summary>
        public string StreetName { get; set; }
        /// <summary>
        /// Street two Part Of Zone Adress
        /// </summary>
        public string Street2 { get; set; }
        /// <summary>
        /// Street three Part Of Zone Adress
        /// </summary>
        public string Street3 { get; set; }
        /// <summary>
        /// City Name Under which That Zone Exist Of Zone Adress
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// City Area Name Under which That Zone Exist Of Zone Adress
        /// </summary>
        public string CityArea { get; set; }
        /// <summary>
        /// City Area Location Name Under which That Zone Exist Of Zone Adress
        /// </summary>
        public string CityAreaLocation { get; set; }
        /// <summary>
        /// State Code Under which That Zone Exist Of Zone Adress
        /// </summary>
        public string StateCode { get; set; }
        /// <summary>
        /// State Name Under which That Zone Exist Of Zone Adress
        /// </summary>
        public string StateName { get; set; }
        /// <summary>
        /// Country Code Under which That Zone Exist Of Zone Adress
        /// </summary>
        public string CountryCode { get; set; }
        /// <summary>
        /// Zone Postal Code Of Zone Adress
        /// </summary>
        public string PostalCode { get; set; }
        /// <summary>
        /// Zone Full Adress Part Of Zone Adress
        /// </summary>
        public string FullAdress { get; set; }
        /// <summary>
        /// Latitude Of That Zone under which That Zone Exist.
        /// </summary>
        public double Latitude { get; set; }
        /// <summary>
        /// Longitude Of That Zone under which That Zone Exist.
        /// </summary>
        public double Longitude { get; set; }
       
        /// <summary>
        /// Zone supplier Name for Which This Location is mapped
        /// </summary>
        public string Supplier_Name { get; set; }
        /// <summary>
        /// Zone supplier Code for which this location is mapped.
        /// </summary>
        public string Supplier_code { get; set; }
    }



    ///<summary>
    ///This is the response format for ZoneGeography
    /// </summary>
    public class Zone_GeographyMapping
    {
        ///<summary>
        ///NAK Country Code
        /// </summary>
        public string TLGXCountryCode { get; set; }
        ///<summary>
        ///NAK State Code 
        /// </summary>
        public string TLGXStateCode { get; set; }
        ///<summary>
        ///NAK City Codes covered by this zone 
        /// </summary>
        public string TLGXCityCode { get; set; }
        ///<summary>
        ///NAK CityArea Code (if zone at that level)
        /// </summary>
        public string TLGXCityAreaCode { get; set; }
        ///<summary>
        ///NAK CityAreaLocation Code (if zone at that level)
        /// </summary>
        public string TLGXCityAreaLocationCode { get; set; }
    }

    /// <summary>
    /// This is the Response format for ZoneSearch.
    /// </summary>
    public class ZoneSearchRS
    {
        /// <summary>
        /// The Total Number of Zones returned by the Search Query
        /// </summary>
        public long TotalNumberOfZones { get; set; }
        /// <summary>
        /// The Number of hotels included in the response per page
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// What is your current Page in the response
        /// </summary>
        public int CurrentPage { get; set; }
        /// <summary>
        /// What is the total number of pages in the response
        /// </summary>
        public int TotalPage { get; set; }
        /// <summary>
        /// List of zones
        /// </summary>
        public List<ZoneInfo> Zones { get; set; }
    }
    /// <summary>
    /// Format for ZoneInfo.
    /// </summary>
    public class ZoneInfo
    {
        /// <summary>
        ///  System Unique zoneId. Use this Id to get detailed information of Zone
        /// </summary>
        public string ZoneId { get; set; }
        /// <summary>
        /// Type of the Zone.
        /// </summary>
        public string Zone_Type { get; set; }
        /// <summary>
        /// Sub Type Of the Zone.
        /// </summary>
        public string Zone_SubType { get; set; }
        /// <summary>
        /// Name of the zone.
        /// </summary>
        public string Zone_Name { get; set; }
        /// <summary>
        /// System Country code .
        /// </summary>
        public string TLGXCountryCode { get; set; }
        /// <summary>
        /// Zone Code .
        /// </summary>
        public string Zone_Code { get; set; }
    }

    /// <summary>
    ///  Response format for ZoneDetails.
    /// </summary>
    public class ZoneDetails
    {
        /// <summary>
        ///  System Zone uniqueId (same as sent in the request parameters).
        /// </summary>
        public string ZoneId { get; set; }
        /// <summary>
        /// Name of the zone.
        /// </summary>
        public string Zone_Name { get; set; }
        /// <summary>
        /// Type of the Zone.
        /// </summary>
        public string Zone_Type { get; set; }
        /// <summary>
        /// Sub Type Of the Zone.
        /// </summary>
        public string Zone_SubType { get; set; }
        /// <summary> System Country Code
        /// </summary>
        public string SystemCountryCode { get; set; }
        /// <summary>
        /// Latitude of Zone.
        /// </summary>
        public string Latitude { get; set; }
        /// <summary>
        /// Longitude of Zone.
        /// </summary>
        public string Longitude { get; set; }
        /// <summary>
        /// Radius(in km) for zone from LatLong. Upto this range,Hotels are included in Zone. As of now Hotels are searchable within 10km of range.
        /// </summary>
        public string Zone_Radius { get; set; }
        /// <summary>
        /// The Total Number of Hotels in system as per Search 
        /// </summary>
        public long TotalNumberOfHotels { get; set; }
        /// <summary>
        /// The Number of hotels included in the response per page
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// The current Page in the response
        /// </summary>
        public int CurrentPage { get; set; }
        /// <summary>
        ///The total number of pages in the response
        /// </summary>
        public int TotalPage { get; set; }
        /// <summary>
        ///  List of Hotels within Zone matching the Search Request
        /// </summary>
        public List<Zone_ProductMapping> ZoneHotels { get; set; }
        /// <summary>
        /// Unique Code Of Zone
        /// </summary>
        public string Zone_Code { get; set; }

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
    /// structure of  Hotels within  zone
    /// </summary>
    [BsonIgnoreExtraElements]
    public class Zone_ProductMapping
    {
        /// <summary>
        /// Unique Id of TLGX Hotel(TLGX ProductCode)
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
        /// Distance Of Hotel(in Km) from Zone center point.
        /// </summary>
        public decimal? Distance { get; set; }
        /// <summary>
        /// Unit of Distance
        /// </summary>
        public string Unit { get; set; }
        /// <summary>
        /// This field specify if the hotel is included in zone or not.
        /// </summary>
        public bool? IsIncluded { get; set; }
    }

    /// <summary>
    /// This is the request format for retriving Zones. 
    /// </summary>
    public class ZoneSearchRQ
    {
        /// <summary>
        /// Search by Zone Name.This is mandatory field. Name Should be atleast 3 letters.
        /// </summary>
        [Required]
        public string Zone_name { get; set; }
        /// <summary>
        ///  How many Search Results do you wish to receive per request? .This is mandatory field.
        /// </summary>
        [Required]
        public int PageSize { get; set; }
        /// <summary>
        /// Which Page Number you wish to retrieve from the Search Results set. Page no starts From 0.This is mandatory field.
        /// </summary>
        [Required]
        public int PageNo { get; set; }
        /// <summary>
        /// Search Zones by System CountryCode. This is optional field.
        /// </summary>
        public string SystemCountryCode { get; set; }
        /// <summary>
        /// Search Zone by its Type. This is optional field. To get ZoneTypes use "ZoneTypeMaster" API
        /// </summary>
        public string Zone_Type { get; set; }
        /// <summary>
        /// Zone Sub types are dependant on ZoneTypes. This is optional field. To get ZoneSubTypes use "ZoneTypeMaster" API
        /// </summary>
        public string Zone_SubType { get; set; }
        /// <summary>
        /// Search by Zone Name.This is mandatory field. Name Should be atleast 3 letters.
        /// </summary>        
        public string Zone_Code { get; set; }
    }
    /// <summary>
    /// This is the request format for retriving ZoneDetails. 
    /// </summary>
    public class ZoneDetailRQ
    {
        /// <summary>
        /// Search by system ZoneId(retrived from ZoneSearch API).This is mandatory field.
        /// </summary>
        [Required]
        public string ZoneId { get; set; }
        /// <summary>
        ///  How many Search Results(Hotels) do you wish to receive per request? .This is mandatory field.
        /// </summary>
        [Required]
        public int PageSize { get; set; }
        /// <summary>
        /// Which Page Number you wish to retrieve from the Search Results set. Page no starts From 0.
        /// </summary>
        [Required]
        public int PageNo { get; set; }

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
        public List<string> Zone_SubType { get; set; }
    }
    #endregion
}