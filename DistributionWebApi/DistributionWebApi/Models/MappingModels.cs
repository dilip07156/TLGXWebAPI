using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using System.Collections.Generic;

namespace DistributionWebApi.Models
{
    public class MappingModels
    {
    }

    /// <summary>
    /// structure of country mapping collection
    /// </summary>
    public class CountryMapping
    {
        public ObjectId Id { get; set; }
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
        public string SupplierName { get; set; }
        public string SupplierCode { get; set; }
        public string SupplierCountryCode { get; set; }
        public string SupplierCountryName { get; set; }
        public string Status { get; set; }
        public int MapId { get; set; }
    }

    /// <summary>
    /// Cross supplier country mapping request.
    /// </summary>
    public class CrossSupplierCountryMapping_RQ
    {
        /// <summary>
        /// From Supplier Code. (Incase of system, specify "TLGX".)
        /// </summary>
        [Required]
        public string SourceSupplierCode { get; set; }

        /// <summary>
        /// From Supplier Country Code. (Incase of system, specify system country code.)
        /// </summary>
        [Required]
        public string SourceSupplierCountryCode { get; set; }

        /// <summary>
        /// To supplier code. (Incase of system, specify "TLGX".)
        /// </summary>
        [Required]
        public string TargetSupplierCode { get; set; }
    }

    /// <summary>
    /// Cross supplier country mapping response.
    /// </summary>
    public class CrossSupplierCountryMapping_RS
    {
        /// <summary>
        /// From supplier code, as specified in request.
        /// </summary>
        public string SourceSupplierCode { get; set; }
        /// <summary>
        /// From supplier country code, as specified in request.
        /// </summary>
        public string SourceSupplierCountryCode { get; set; }
        /// <summary>
        /// To supplier code, as specified in request.
        /// </summary>
        public string TargetSupplierCode { get; set; }
        /// <summary>
        /// To supplier country code.
        /// </summary>
        public string TargetSupplierCountryCode { get; set; }
        /// <summary>
        /// To supplier country name.
        /// </summary>
        public string TargetSupplierCountryName { get; set; }
        /// <summary>
        /// Mapping status. (If there are results then "Mapped" else "No results found")
        /// </summary>
        public string Status { get; set; }
    }

    /// <summary>
    /// System Country Mapping Response. This universal format is used for both mapping and reverse mapping against System / Supplier specific countries.
    /// </summary>
    public class CountryMapping_RS
    {
        /// <summary>
        /// System Country Name
        /// </summary>
        public string CountryName { get; set; }
        /// <summary>
        /// System Country Code
        /// </summary>
        public string CountryCode { get; set; }
        /// <summary>
        /// System Supplier Name
        /// </summary>
        public string SupplierName { get; set; }
        /// <summary>
        /// System Supplier Code
        /// </summary>
        public string SupplierCode { get; set; }
        /// <summary>
        /// Supplier-specific Country Code
        /// </summary>
        public string SupplierCountryCode { get; set; }
        /// <summary>
        /// Supplier-specific Country Name
        /// </summary>
        public string SupplierCountryName { get; set; }
        /// <summary>
        /// System Country Mapping Id
        /// </summary>
        public int MapId { get; set; }

    }

    public class SystemCountryMapping_RS
    {
        /// <summary>
        /// System Supplier Code
        /// </summary>
        public string SupplierCode { get; set; }
        /// <summary>
        /// Supplier-specific Country Code
        /// </summary>
        public string SupplierCountryCode { get; set; }
        /// <summary>
        /// System Country Mapping Id
        /// </summary>
        public int MapId { get; set; }
    }

    /// <summary>
    /// structure of city mapping collection
    /// </summary>
    public class CityMapping
    {
        public ObjectId Id { get; set; }
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
        public string CityName { get; set; }
        public string CityCode { get; set; }
        public string SupplierName { get; set; }
        public string SupplierCode { get; set; }
        public string SupplierCountryName { get; set; }
        public string SupplierCountryCode { get; set; }
        public string SupplierCityName { get; set; }
        public string SupplierCityCode { get; set; }

        public int MapId { get; set; }

    }

    /// <summary>
    /// TLGX City Mapping Response. This universal format is used for both mapping and reverse mapping against TLGX / Supplier specific cities.
    /// </summary>
    public class CityMapping_RS
    {
        /// <summary>
        /// System Country Name
        /// </summary>
        public string CountryName { get; set; }
        /// <summary>
        /// System Country Code
        /// </summary>
        public string CountryCode { get; set; }
        /// <summary>
        /// System City Name
        /// </summary>
        public string CityName { get; set; }
        /// <summary>
        /// System City Code
        /// </summary>
        public string CityCode { get; set; }
        /// <summary>
        /// System Supplier Name
        /// </summary>
        public string SupplierName { get; set; }
        /// <summary>
        /// System Supplier Code. This field should always be defined.
        /// </summary>
        public string SupplierCode { get; set; }
        /// <summary>
        /// Supplier-specific Country Name
        /// </summary>
        public string SupplierCountryName { get; set; }
        /// <summary>
        /// Supplier-specific Country Code
        /// </summary>
        public string SupplierCountryCode { get; set; }
        /// <summary>
        /// Supplier-specific City Name
        /// </summary>
        public string SupplierCityName { get; set; }
        /// <summary>
        /// Supplier-specific City Code
        /// </summary>
        public string SupplierCityCode { get; set; }
        /// <summary>
        /// System Supplier City Mapping Id
        /// </summary>
        public int MapId { get; set; }
    }

    public class SystemCityMapping_RS
    {
        /// <summary>
        /// System Supplier Code. This field should always be defined.
        /// </summary>
        public string SupplierCode { get; set; }
        /// <summary>
        /// Supplier-specific City Name
        /// </summary>
        public string SupplierCityCode { get; set; }
        /// <summary>
        /// System City Mapping Id
        /// </summary>
        public int MapId { get; set; }
    }

    /// <summary>
    /// Cross supplier city mapping request.
    /// </summary>
    public class CrossSupplierCityMapping_RQ
    {
        /// <summary>
        /// From Supplier Code. (Incase of system, specify "TLGX".)
        /// </summary>
        [Required]
        public string SourceSupplierCode { get; set; }

        /// <summary>
        /// From Supplier Country Code. (Incase of system, specify system country code.)
        /// </summary>
        [Required]
        public string SourceSupplierCountryCode { get; set; }

        /// <summary>
        /// From Supplier City Code. (Incase of system, specify system city code.)
        /// </summary>
        [Required]
        public string SourceSupplierCityCode { get; set; }

        /// <summary>
        /// From Supplier Code. (Incase of system, specify "TLGX".)
        /// </summary>
        [Required]
        public string TargetSupplierCode { get; set; }
    }

    /// <summary>
    /// Cross supplier city mapping response.
    /// </summary>
    public class CrossSupplierCityMapping_RS
    {
        /// <summary>
        /// From supplier code, as specified in request.
        /// </summary>
        public string SourceSupplierCode { get; set; }
        /// <summary>
        /// From supplier country code, as specified in request.
        /// </summary>
        public string SourceSupplierCountryCode { get; set; }
        /// <summary>
        /// From supplier city code, as specified in request.
        /// </summary>
        public string SourceSupplierCityCode { get; set; }
        /// <summary>
        /// To supplier code, as specified in request.
        /// </summary>
        public string TargetSupplierCode { get; set; }
        /// <summary>
        /// To supplier city code.
        /// </summary>
        public string TargetSupplierCityCode { get; set; }
        /// <summary>
        /// To supplier city name.
        /// </summary>
        public string TargetSupplierCityName { get; set; }
        /// <summary>
        /// Mapping status. (If there are results then "Mapped" else "No results found")
        /// </summary>
        public string Status { get; set; }
    }

    /// <summary>
    /// product mapping collection structure
    /// </summary>
    public class ProductMapping
    {
        /// <summary>
        /// Collection unique id
        /// </summary>
        public ObjectId Id { get; set; }
        /// <summary>
        /// System supplier code
        /// </summary>
        public string SupplierCode { get; set; }
        /// <summary>
        /// Supplier specific product code
        /// </summary>
        public string SupplierProductCode { get; set; }
        /// <summary>
        /// Supplier specific product name
        /// </summary>
        public string SupplierProductName { get; set; }
        /// <summary>
        /// Supplier specific country name
        /// </summary>
        public string SupplierCountryName { get; set; }
        /// <summary>
        /// Supplier specific country code
        /// </summary>
        public string SupplierCountryCode { get; set; }
        /// <summary>
        /// Supplier specific city name
        /// </summary>
        public string SupplierCityName { get; set; }
        /// <summary>
        /// Supplier specific city code
        /// </summary>
        public string SupplierCityCode { get; set; }
        /// <summary>
        /// system product code
        /// </summary>
        public string SystemProductCode { get; set; }
        /// <summary>
        /// system product name
        /// </summary>
        public string SystemProductName { get; set; }
        /// <summary>
        /// system country name
        /// </summary>
        public string SystemCountryName { get; set; }
        /// <summary>
        /// system city name
        /// </summary>
        public string SystemCityName { get; set; }
        /// <summary>
        /// system country code
        /// </summary>
        public string SystemCountryCode { get; set; }
        /// <summary>
        /// system city code
        /// </summary>
        public string SystemCityCode { get; set; }
        /// <summary>
        /// system product type
        /// </summary>
        public string SystemProductType { get; set; }
        /// <summary>
        /// mapping status of the supplier product
        /// </summary>
        public string MappingStatus { get; set; }
        /// <summary>
        /// system unique mapping id
        /// </summary>
        public int MapId { get; set; }
        /// <summary>
        /// TLGX MDM Unique identifier for hotel property
        /// </summary>
        public string TlgxMdmHotelId { get; set; }
    }

    /// <summary>
    /// Product Mapping Request Parameter
    /// </summary>
    public class ProductMapping_RQ
    {
        /// <summary>
        /// Session Id from Booking Engine for the originating Search Response that is to be mapped. 
        /// This field is designed to be used to interrogate original Supplier Static calls should a map not be found within the TLGX Mapping Database
        /// </summary>
        public string SessionId { get; set; }

        /// <summary>
        /// Sequence Number for the individual mapping requirement within the overall request
        /// </summary>
        public string SequenceNumber { get; set; }

        /// <summary>
        /// User Input Product Type
        /// </summary>
        public string ProductType { get; set; }

        /// <summary>
        /// Should be set to "Hotel" 
        /// </summary>
        [Required]
        public string SupplierCode { get; set; }

        /// <summary>
        /// TLGX Supplier Master Code. These can be retrieved using Supplier Master API Framework.
        /// </summary>
        [Required]
        public string SupplierProductCode { get; set; }

        /// <summary>
        /// Supplier Product Code received in the Supplier Availability Response.
        /// </summary>
        public string SupplierProductName { get; set; }

        /// <summary>
        /// Supplier Product Name received in the Supplier Availability Response.
        /// </summary>
        public string SupplierCountryName { get; set; }

        /// <summary>
        /// Supplier Country Name for Product received in the Supplier Availability Response.
        /// </summary>
        public string SupplierCountryCode { get; set; }

        /// <summary>
        /// Supplier City Name for Product received in the Supplier Availability Response.
        /// </summary>
        public string SupplierCityName { get; set; }

        /// <summary>
        /// Supplier City Code for Product received in the Supplier Availability Response.
        /// </summary>
        public string SupplierCityCode { get; set; }

        /// <summary>
        /// Mapping Action Request. Should be set to "TO MAP" in order to receive a mapping request. 
        /// </summary>
        public string Status { get; set; }
    }

    /// <summary>
    /// Product Mapping Response Schema
    /// </summary>
    public class ProductMapping_RS
    {
        /// <summary>
        /// Original value submitted in Request
        /// </summary>
        public string SessionId { get; set; }
        /// <summary>
        /// Original value submitted in Request
        /// </summary>
        public string SequenceNumber { get; set; }
        /// <summary>
        /// Original value submitted in Request
        /// </summary>
        public string ProductType { get; set; }
        /// <summary>
        /// Original value submitted in Request
        /// </summary>
        public string SupplierCode { get; set; }
        /// <summary>
        /// Original value submitted in Request
        /// </summary>
        public string SupplierProductCode { get; set; }
        /// <summary>
        /// System Product Code / Common product code 
        /// </summary>
        public string SystemProductCode { get; set; }
        /// <summary>
        /// Original value submitted in Request
        /// </summary>
        public string SupplierProductName { get; set; }
        /// <summary>
        /// Original value submitted in Request
        /// </summary>
        public string SupplierCountryName { get; set; }
        /// <summary>
        /// Original value submitted in Request
        /// </summary>
        public string SupplierCountryCode { get; set; }
        /// <summary>
        /// Original value submitted in Request
        /// </summary>
        public string SupplierCityName { get; set; }
        /// <summary>
        /// Original value submitted in Request
        /// </summary>
        public string SupplierCityCode { get; set; }
        /// <summary>
        /// System Product Mapping status. 
        /// Values should be "Mapped" to indicate a successful Product Mapping, "No results found" for unsuccessful Product Mapping 
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// System Mapping Id
        /// </summary>
        public int MapId { get; set; }
        /// <summary>
        /// TLGX MDM Unique identifier for hotel property
        /// </summary>
        public string TlgxMdmHotelId { get; set; }
    }

    public class SystemProductMapping_RS
    {
        /// <summary>
        /// Supplier Code
        /// </summary>
        public string SupplierCode { get; set; }
        /// <summary>
        /// Supplier Specific Product Code
        /// </summary>
        public string SupplierProductCode { get; set; }
        /// <summary>
        /// System Product Code / Common product code 
        /// </summary>
        public string SystemProductCode { get; set; }
        /// <summary>
        /// System Mapping Id
        /// </summary>
        public int MapId { get; set; }
        /// <summary>
        /// TLGX MDM Unique identifier for hotel property
        /// </summary>
        public string TlgxMdmHotelId { get; set; }
    }

    /// <summary>
    /// Product Mapping Lite Request Parameter
    /// </summary>
    public class ProductMappingLite_RQ
    {
        /// <summary>
        /// Session Id from Booking Engine for the originating Search Response that is to be mapped. 
        /// This field is designed to be used to interrogate original Supplier Static calls should a map not be found within the TLGX Mapping Database
        /// </summary>
        [Required]
        public string SessionId { get; set; }

        /// <summary>
        /// Sequence Number for the individual mapping requirement within the overall request
        /// </summary>
        [Required]
        public string SequenceNumber { get; set; }

        /// <summary>
        /// User Input Product Type
        /// </summary>
        public string ProductType { get; set; }

        /// <summary>
        /// TLGX Supplier Master Code. These can be retrieved using Supplier Master API Framework.
        /// </summary>
        [Required]
        public string SupplierCode { get; set; }

        /// <summary>
        /// Supplier Product Code received in the Supplier Availability Response.
        /// </summary>
        [Required]
        public string SupplierProductCode { get; set; }
    }

    /// <summary>
    /// Product Mapping Lite Response Schema. There is no Status on this structure. Incase of no result, a blank value will be returned in SystemProductCode.
    /// </summary>
    public class ProductMappingLite_RS
    {
        /// <summary>
        /// Original value submitted in Request
        /// </summary>
        public string SessionId { get; set; }
        /// <summary>
        /// Original value submitted in Request
        /// </summary>
        public string SequenceNumber { get; set; }
        /// <summary>
        /// Original value submitted in Request
        /// </summary>
        public string ProductType { get; set; }
        /// <summary>
        /// Original value submitted in Request
        /// </summary>
        public string SupplierCode { get; set; }
        /// <summary>
        /// Original value submitted in Request
        /// </summary>
        public string SupplierProductCode { get; set; }
        /// <summary>
        /// System Product Code / Common product code
        /// </summary>
        public string SystemProductCode { get; set; }
        /// <summary>
        /// System Mapping Id
        /// </summary>
        public int MapId { get; set; }
        /// <summary>
        /// TLGX MDM Unique identifier for hotel property
        /// </summary>
        public string TlgxMdmHotelId { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class ProductMappingLite
    {
        [BsonId]
        public ObjectId _id { get; set; }
        [BsonElement]
        public string SupplierCode { get; set; }
        [BsonElement]
        public string SupplierProductCode { get; set; }
        [BsonElement]
        public int MapId { get; set; }
        [BsonElement]
        public string SystemProductCode { get; set; }
        [BsonElement]
        public string TlgxMdmHotelId { get; set; }
    }

    //public class MyListProductMappingLiteSerializer : SerializerBase<List<ProductMappingLite>>
    //{
    //    public override void Serialize(MongoDB.Bson.Serialization.BsonSerializationContext context, MongoDB.Bson.Serialization.BsonSerializationArgs args, List<ProductMappingLite> value)
    //    {
    //        context.Writer.WriteStartDocument();
    //        context.Writer.WriteName("SupplierCode");
    //        context.Writer.WriteString(value.GetType().);
    //        context.Writer.WriteName("_t");
    //        context.Writer.WriteString(value.GetType().Name);
    //        context.Writer.WriteName("Name");
    //        context.Writer.WriteString(value.Name);
    //        context.Writer.WriteEndDocument();
    //    }

    //    public override List<Animals> Deserialize(MongoDB.Bson.Serialization.BsonDeserializationContext context, MongoDB.Bson.Serialization.BsonDeserializationArgs args)
    //    {
    //        context.Reader.ReadStartArray();

    //        List<Animals> result = new List<Animals>();

    //        while (true)
    //        {
    //            try
    //            {
    //                //this catch block only need to identify the end of the Array
    //                context.Reader.ReadStartDocument();
    //            }
    //            catch (Exception exp)
    //            {
    //                context.Reader.ReadEndArray();
    //                break;
    //            }

    //            var type = context.Reader.ReadString();
    //            var _id = context.Reader.ReadObjectId();
    //            var name = context.Reader.ReadString();
    //            if (type == "Tiger")
    //            {
    //                double tiger_height = context.Reader.ReadDouble();
    //                result.Add(new Tiger()
    //                {
    //                    Id = id,
    //                    Name = animal_name,
    //                    Height = tiger_height
    //                });
    //            }
    //            else
    //            {
    //                long zebra_stripes = context.Reader.ReadInt64();
    //                result.Add(return new Zebra()
    //                {
    //                    Id = id,
    //                    Name = animal_name,
    //                    StripesAmount = zebra_stripes
    //                });
    //            }
    //            context.Reader.ReadEndDocument();
    //        }
    //        return result;
    //    }
    //}

    /// <summary>
    /// Complete Product Mapping Response Schema
    /// </summary>
    public class CompleteProductMapping_RS
    {
        /// <summary>
        /// System supplier code
        /// </summary>
        public string SupplierCode { get; set; }
        /// <summary>
        /// Supplier specific product code
        /// </summary>
        public string SupplierProductCode { get; set; }
        /// <summary>
        /// Supplier specific product name
        /// </summary>
        public string SupplierProductName { get; set; }
        /// <summary>
        /// Supplier specific country name
        /// </summary>
        public string SupplierCountryName { get; set; }
        /// <summary>
        /// Supplier specific country code
        /// </summary>
        public string SupplierCountryCode { get; set; }
        /// <summary>
        /// Supplier specific city name
        /// </summary>
        public string SupplierCityName { get; set; }
        /// <summary>
        /// Supplier specific city code
        /// </summary>
        public string SupplierCityCode { get; set; }
        /// <summary>
        /// system product code
        /// </summary>
        public string SystemProductCode { get; set; }
        /// <summary>
        /// system product name
        /// </summary>
        public string SystemProductName { get; set; }
        /// <summary>
        /// system country name
        /// </summary>
        public string SystemCountryName { get; set; }
        /// <summary>
        /// system city name
        /// </summary>
        public string SystemCityName { get; set; }
        /// <summary>
        /// system country code
        /// </summary>
        public string SystemCountryCode { get; set; }
        /// <summary>
        /// system city code
        /// </summary>
        public string SystemCityCode { get; set; }
        /// <summary>
        /// system product type
        /// </summary>
        public string SystemProductType { get; set; }
        ///// <summary>
        ///// mapping status of the supplier product
        ///// </summary>
        //public string MappingStatus { get; set; }
        /// <summary>
        /// system unique mapping id
        /// </summary>
        public int MapId { get; set; }
        /// <summary>
        /// TLGX MDM Unique identifier for hotel property
        /// </summary>
        public string TlgxMdmHotelId { get; set; }
    }




    [BsonIgnoreExtraElements]
    public class DC_ConpanyAccommodationMapping
    {
        [BsonIgnore]
        public Guid SupplierId { get; set; }

        [BsonId]
        [Newtonsoft.Json.JsonProperty("_id")]
        public string _id { get; set; }

        public String SupplierCode { get; set; }

        public string SupplierProductCode { get; set; }

        public string SupplierProductName { get; set; }

        public string CompanyProductName { get; set; }

        public string CompanyProductId { get; set; }

        public string CommonProductId { get; set; }

        public string TLGXCompanyId { get; set; }

        public string Rating { get; set; }

        public string TLGXCompanyName { get; set; }

        public string CountryName { get; set; }

        public string CityName { get; set; }

        public string StateName { get; set; }

        public string ProductCategorySubType { get; set; }

        public string Brand { get; set; }

        public string Chain { get; set; }

        public string Interest { get; set; }

        [BsonIgnore]
        public Guid Accommodation_CompanyVersion_Id { get; set; }

        public List<DC_ConpanyAccommodationRoomMapping> MappedRooms { get; set; }

        //Collection of Room data from version data
    }


    [BsonIgnoreExtraElements]
    public class DC_ConpanyAccommodationRoomMapping
    {
        public string SupplierRoomId { get; set; }

        public string SupplierRoomTypeCode { get; set; }

        public string SupplierRoomName { get; set; }

        public string SupplierRoomCategory { get; set; }

        public string SupplierRoomCategoryId { get; set; }

        public string CompanyRoomId { get; set; }

        public string CompanyRoomName { get; set; }

        public string CompanyRoomCategory { get; set; }

        public string NakshatraRoomMappingId { get; set; }

        [BsonIgnore]
        public Guid Accommodation_CompanyVersion_Id { get; set; }

        [BsonIgnore]

        public string SupplierProductId { get; set; }

        [BsonIgnore]
        public Guid Supplier_Id { get; set; }
    }
}