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
    /// 
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
    /// TLGX Country Mapping Response. This universal format is used for both mapping and reverse mapping against TLGX / Supplier specific countries.
    /// </summary>
    public class CountryMapping_RS
    {
        /// <summary>
        /// TLGX Country Name
        /// </summary>
        public string CountryName { get; set; }
        /// <summary>
        /// TLGX Country Code
        /// </summary>
        public string CountryCode { get; set; }
        /// <summary>
        /// TLGX Supplier Name
        /// </summary>
        public string SupplierName { get; set; }
        /// <summary>
        /// TLGX Supplier Code
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
        /// TLGX Mapping Id
        /// </summary>
        public int MapId { get; set; }

    }

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
        /// TLGX Country Name
        /// </summary>
        public string CountryName { get; set; }
        /// <summary>
        /// TLGX Country Code
        /// </summary>
        public string CountryCode { get; set; }
        /// <summary>
        /// TLGX City Name
        /// </summary>
        public string CityName { get; set; }
        /// <summary>
        /// TLGX City Code
        /// </summary>
        public string CityCode { get; set; }
        /// <summary>
        /// TLGX Supplier Name
        /// </summary>
        public string SupplierName { get; set; }
        /// <summary>
        /// TLGX Supplier Code. This field should always be defined.
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
        /// TLGX Supplier City Mapping Id
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

    public class ProductMapping
    {
        public ObjectId Id { get; set; }
        public string SupplierCode { get; set; }
        public string SupplierProductCode { get; set; }
        public string SystemProductCode { get; set; }
        public string SupplierCountryName { get; set; }
        public string SupplierCountryCode { get; set; }
        public string SupplierCityName { get; set; }
        public string SupplierCityode { get; set; }
        public string SystemProductType { get; set; }
        public string SupplierProductName { get; set; }
        public string SystemProductName { get; set; }
        public string SystemCountryName { get; set; }
        public string SystemCityName { get; set; }
        public string MappingStatus { get; set; }
        public int MapId { get; set; }
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
        /// TLGX Product Code
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
        /// TLGX Product Mapping status. 
        /// Values should be "Mapped" to indicate a successful Product Mapping, "No results found" for unsuccessful Product Mapping 
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// TLGX Mapping Id
        /// </summary>
        public int MapId { get; set; }
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
        /// TLGX Product Code
        /// </summary>
        public string SystemProductCode { get; set; }
        /// <summary>
        /// TLGX Mapping Id
        /// </summary>
        public int MapId { get; set; }
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
}