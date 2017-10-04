using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DistributionWebApi.Models
{
    public class RoomTypeMappingModel
    {
        //[BsonId]
        //public ObjectId _id { get; set; }
        public string SupplierCode { get; set; }
        public string SupplierProductCode { get; set; }
        public string SupplierRoomTypeCode { get; set; }
        public string SupplierRoomTypeName { get; set; }
        public string SystemRoomTypeMapId { get; set; }
        public string SystemProductCode { get; set; }
        public string SystemRoomTypeCode { get; set; }
        public string SystemRoomTypeName { get; set; }
        public string SystemNormalizedRoomType { get; set; }
        public string SystemStrippedRoomType { get; set; }
        //public DC_RoomTypeMapping_AlternateRoomNames AlternateRoomNames { get; set; }
        public List<RoomTypeMapping_Attributes> Attibutes { get; set; }
        public string Status { get; set; }
    }

    //public class RoomTypeMapping_AlternateRoomNames
    //{
    //    public string Normalized;
    //    public string Stripped;
    //}

    /// <summary>
    /// TLGX system specific attributes extracted from supplier room name
    /// </summary>
    public class RoomTypeMapping_Attributes
    {
        /// <summary>
        /// Attribute type
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// Attribute value
        /// </summary>
        public string Value { get; set; }
    }

    /// <summary>
    /// Supplier Room type mapping request format
    /// </summary>
    public class RoomTypeMapping_RQ
    {
        /// <summary>
        /// Code for Supplier
        /// </summary>
        [Required]
        public string SupplierCode { get; set; }
        /// <summary>
        /// Product code for supplier
        /// </summary>
        [Required]
        public string SupplierProductCode { get; set; }
        /// <summary>
        /// Room type code for Supplier
        /// </summary>
        [Required]
        public string SupplierRoomTypeCode { get; set; }
        /// <summary>
        /// Room type name for Supplier
        /// </summary>
        [Required]
        public string SupplierRoomTypeName { get; set; }
        /// <summary>
        /// Request sequence number
        /// </summary>
        [Required]
        public int SequenceNumber { get; set; }
    }

    /// <summary>
    /// Supplier room type mapping respose format
    /// </summary>
    public class RoomTypeMapping_RS
    {
        /// <summary>
        /// Supplier Code sent in request
        /// </summary>
        public string SupplierCode { get; set; }
        /// <summary>
        /// Supplier product/hotel code sent in request
        /// </summary>
        public string SupplierProductCode { get; set; }
        /// <summary>
        /// Supplier room type id/code sent in request
        /// </summary>
        public string SupplierRoomTypeCode { get; set; }
        /// <summary>
        /// Supplier room type name sent in request
        /// </summary>
        public string SupplierRoomTypeName { get; set; }
        /// <summary>
        /// Request sequence number
        /// </summary>
        public int SequenceNumber { get; set; }
        /// <summary>
        /// Supplier room type unique map id from TLGX system. If mapping is not found this will be returned as ZERO.
        /// </summary>
        public string SystemRoomTypeMapId { get; set; }
        /// <summary>
        /// TLGX hotel/product code mapped with supplier product. If not mapped then this will be returned as NULL
        /// </summary>
        public string SystemProductCode { get; set; }
        /// <summary>
        /// TLGX room type code mapped with supplier room type for the respective product. If not mapped then this will be returned as NULL
        /// </summary>
        public string SystemRoomTypeCode { get; set; }
        /// <summary>
        /// TLGX room category name mapped with supplier room type for the respective product. If not mapped then this will be returned as NULL
        /// </summary>
        public string SystemRoomTypeName { get; set; }
        /// <summary>
        /// TLGX normalized room category name mapped with supplier room type for the respective product. 
        /// Normalized means Supplier Room Name is standardized to TLGX format by replacing the system specific keywords. 
        /// e.g DBL BED SEA VW changes to DOUBLE BED SEA VIEW
        /// </summary>
        public string SystemNormalizedRoomType { get; set; }
        /// <summary>
        /// TLGX stripped room category name mapped with supplier room type for the respective product. 
        /// Stripped means Supplier Room Name is standardized to TLGX format and then the attributes are extracted from room name.
        /// e.g DBL BED SEA VW changes to DOUBLE BED and SEA VIEW will be extracted as VIEW type. So the room name will be DOUBLE BED only.
        /// </summary>
        public string SystemStrippedRoomType { get; set; }
        /// <summary>
        /// TLGX specified extracted attributes from supplier room name
        /// e.g DBL BED SEA VW changes to DOUBLE BED and SEA VIEW will be extracted as VIEW type.
        /// So the attributes will be {"TYPE" : "VIEW", "VALUE" : "SEA VIEW"}, {}, {} and so on.
        /// </summary>
        public List<RoomTypeMapping_Attributes> Attibutes { get; set; }
        /// <summary>
        /// This is the match score of supplier room type name after converted to 
        /// TLGX standardized room name through TLGX specific keyword replacement and attribute extraction.
        /// </summary>
        public int MapScore { get; set; }
        /// <summary>
        /// Mapping status.
        /// MAPPED : match found
        /// UNMAPPED : match not found
        /// SUGGESTED : match not found but supplier room name is suggested for System room name after it is normalized and attributes are extracted
        /// ERROR : Errors/Validations while processing the request
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// This will specify the error details for response in case of when STATUS = ERROR
        /// </summary>
        public string Remarks { get; set; }
    }
}