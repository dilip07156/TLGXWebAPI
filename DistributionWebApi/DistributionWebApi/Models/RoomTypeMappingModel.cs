using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
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

    public class RoomTypeMapping_Attributes
    {
        public string Type { get; set; }
        public string Value { get; set; }
    }

    public class RoomTypeMapping_RQ
    {
        public string SupplierCode { get; set; }
        public string SupplierProductCode { get; set; }
        public string SupplierRoomTypeCode { get; set; }
        public string SupplierRoomTypeName { get; set; }
        public int SequenceNumber { get; set; }
    }

    public class RoomTypeMapping_RS
    {
        public string SupplierCode { get; set; }
        public string SupplierProductCode { get; set; }
        public string SupplierRoomTypeCode { get; set; }
        public string SupplierRoomTypeName { get; set; }
        public int SequenceNumber { get; set; }
        public string SystemRoomTypeMapId { get; set; }
        public string SystemProductCode { get; set; }
        public string SystemRoomTypeCode { get; set; }
        public string SystemRoomTypeName { get; set; }
        public string SystemNormalizedRoomType { get; set; }
        public string SystemStrippedRoomType { get; set; }
        public List<RoomTypeMapping_Attributes> Attibutes { get; set; }
        public int MapScore { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
    }
}