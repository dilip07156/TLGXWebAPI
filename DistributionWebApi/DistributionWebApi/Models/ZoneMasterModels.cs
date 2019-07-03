using DistributionWebApi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DistributionWebApi.ZoneModels
{
    /// <summary>
    /// This is the request format for retriving Zones X char Basis. 
    /// </summary>
    public class ZoneSearchRequest
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
        /// Search Zone by its Type. This is optional field. To get ZoneTypes use "ZoneTypeMaster" API
        /// </summary>
        public List<ZoneTypes> ZoneTypes { get; set; }
        /// <summary>
        /// Zone Sub types are dependant on ZoneTypes. This is optional field. To get ZoneSubTypes use "ZoneTypeMaster" API
        /// </summary>
        public List<ZoneSubTypes> Zone_SubType { get; set; }
      
        /// <summary>
        /// Search Zones by System CountryCode. This is optional field.
        /// </summary>
        public string SystemCountryCode { get; set; }
        /// <summary>
        /// Search Zones by System City Code. This is optional field.
        /// </summary>
        public List<ZoneCities> ZoneCities { get; set; }       
    }
    /// <summary>
    /// This is the request format for Zone Types Master. 
    /// </summary>
    public class ZoneTypes
    {
        /// <summary>
        /// Search By Zone_Type will be from zone type master.
        /// </summary>
        public string Zone_Type { get; set; }
    }

    /// <summary>
    /// This is the request format for Zone City. 
    /// </summary>
    public class ZoneCities
    {
        /// <summary>
        /// Search By Zone_Type will be from city master.
        /// </summary>
        public string Zone_City { get; set; }
    }



    /// <summary>
    /// This is the request format for ZoneSub Types. 
    /// </summary>
    public class ZoneSubTypes
    {
        /// <summary>
        /// Search By ZoneSub_Type.
        /// </summary>
        public string ZoneSub_Type { get; set; }
    }

    /// <summary>
    /// This is the request format for Geo Location in the forms of LAT/LONG. 
    /// </summary>
    public class ZoneGeoLocation
    {
        /// <summary>
        /// Search By Zone Latitude should be in decimal format.
        /// </summary>
        public double Zone_Latitude { get; set; }

        ///<summary>
        ///Search by Zone Longitude should be in decimal format
        /// </summary>
        public double Zone_Longitude { get; set; }

    }

    /// <summary>
    /// This is the request format for retriving Zones in terms of geo point Basis. 
    /// </summary>
    public class ZoneSearchRequestForGeoPoint
    {
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
        /// Search Zone by its Type. This is optional field. To get ZoneTypes use "ZoneTypeMaster" API
        /// </summary>
        public List<ZoneTypes> ZoneTypes { get; set; }
        /// <summary>
        /// Zone Sub types are dependant on ZoneTypes. This is optional field. To get ZoneSubTypes use "ZoneTypeMaster" API
        /// </summary>
        public List<ZoneSubTypes> Zone_SubType { get; set; }
        ///<summary>
        ///Lat/Lon based search to pull zones, to be used on mouse click on map if required
        /// </summary>
        public List<ZoneGeoLocation> ZoneGeoLocations { get; set; }
        ///<summary>
        ///Maximum Geo Distance based on Latitude/Longitude by which data will get from system.
        /// </summary>
        public decimal GeoDistance { get; set; }      
    }



    /// <summary>
    /// This is the response format for retriving Zones. 
    /// </summary>

    public class ZoneSearchResponse
    {
        /// <summary>
        /// NAK Zone Code
        /// </summary>
        public string ZoneCode { get; set; }
        /// <summary>
        /// NAK Zone Name
        /// </summary>
        public string ZoneName { get; set; }
        /// <summary>
        /// NAK Zone Type
        /// </summary>
        public string ZoneType { get; set; }
        /// <summary>
        /// NAK Zone Sub Type
        /// </summary>
        public string ZoneSubType { get; set; }
        /// <summary>
        /// List of Central lat / lon for Zone
        /// </summary>
        public ZoneGeoLocationResponse ZoneLocationPoint { get; set; }
        ///<summary>
        ///For future use, should we require polygon binding instead of radius approach
        /// </summary>
        public string LocationBounding { get; set; }
        ///<summary>
        ///NAK Supplier Code to denote if this is a vendor specific Location (such as a Hertz Pickup/DropOff)
        /// </summary>
        public string ZoneVendorSpecificCode { get; set; }
        ///<summary>
        ///NAK Geographical Hierarchy for the Zone.
        /// </summary>
        public List<Zone_GeographyMapping> Zone_GeographyMapping { get; set; }
    }

    /// <summary>
    /// This is the response format for Geo Location. 
    /// </summary>
    public class ZoneGeoLocationResponse
    {
        /// <summary>
        /// Zone Latitude from Zone master.
        /// </summary>
        public string Zone_Latitude { get; set; }

        ///<summary>
        ///Longitude from Zone master
        /// </summary>
        public string Zone_Longitude { get; set; }

    }


    /// <summary>
    /// This is the request format for retriving Location Mapping List With Zone codes. 
    /// </summary>

    public class ZoneMappingSearchRequest
    {
        /// <summary>
        /// NAK Zone Code from Nakshtra Zone Master
        /// </summary>
        public List<string> ZoneCodes { get; set; }
        /// <summary>
        /// NAK Supplier Codes from Nakshtra Supplier Master
        /// </summary>
        public List<string> SupplierCodes { get; set; }

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
    }

    /// <summary>
    /// This is the response format for retriving Location Mapping List With Zone codes. 
    /// </summary>

    public class ZoneMappingSearchResponse
    {
        /// <summary>
        /// NAK Zone Code from Nakshtra Zone Master
        /// </summary>
        public string ZoneCodes { get; set; }
        /// <summary>
        /// NAK Supplier Codes from Nakshtra Supplier Master
        /// </summary>
        public List<Zone_Supplier> SupplierCodes { get; set; }       
    }

    /// <summary>
    /// This is the response format for retriving Location Mapping List With Zone codes. 
    /// </summary>

    public class Zone_Supplier
    {
        /// <summary>
        /// NAK Supplier Codes from Nakshtra Supplier Master
        /// </summary>
        public string SupplierCode { get; set; }
        ///<summary>
        ///List of Supplier Mapping Locations
        /// </summary>
        public List<ZoneSupplierLocation> MappingLocations { get; set; }

    }


        ///<summary>
        ///This is the format for Mapping Locations
        /// </summary>

        public  class ZoneSupplierLocation
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
            public string Type { get; set; }

            /// <summary>
            /// Zone supplier Zone Sub Type 
            /// </summary>
            public string SubType { get; set; }

            /// <summary>
            /// Zone supplier Zone Sub Type 
            /// </summary>
            public double Latitude { get; set; }

            /// <summary>
            /// Zone supplier Zone Sub Type 
            /// </summary>
            public double Longitude { get; set; }

            /// <summary>
            /// Zone supplier Zone Sub Type 
            /// </summary>
            public string FullAddress { get; set; }
        }


}