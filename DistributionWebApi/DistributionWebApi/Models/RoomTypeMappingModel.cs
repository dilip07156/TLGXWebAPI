using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DistributionWebApi.Models
{
    #region OldStructures

    /// <summary>
    /// 
    /// </summary>
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

    #endregion

    #region Request

    /// <summary>
    /// This is the main room type mapping request containing mode, tracking information and the collection of TLGX Accommodation room type mapping requests.
    /// </summary>
    public class RoomTypeMapping_SIRQ
    {
        /// <summary>
        /// This field specifies the type of room type mapping requests. You should always set this value to "online".
        /// </summary>
        [Required]
        public string Mode { get; set; }
        /// <summary>
        /// Please generate a unique Batch / Tracking ID to allow end to end tracebility.
        /// </summary>
        [Required]
        public string BatchId { get; set; }
        /// <summary>
        /// Your room type mapping requests for specific TLGX Accommodation CommonHotelId. This service will not work without this value.
        /// You can include multiple TLGX Accommodations for room type mapping.
        /// </summary>
        [Required]
        public List<RoomTypeMapping_SIRQ_HotelRoomTypeMappingRequest> HotelRoomTypeMappingRequests { get; set; }
    }

    /// <summary>
    /// Structure to contain single / multiple accommodation room type mapping requests by TLGX Accommodation CommonHotelId.
    /// </summary>
    public class RoomTypeMapping_SIRQ_HotelRoomTypeMappingRequest
    {
        /// <summary>
        /// TLGX Accommodation CommonHotelId - TLGX MDM unique identifier for a hotel property. This value can be retrived by using ProductMappingLite api.
        /// </summary>
        [Required]
        public string TLGXCommonHotelId { get; set; }
        /// <summary>
        /// Supplier data for room type mapping requests grouped at TLGX Accommodation CommonHotelId.
        /// This allows you to perform multiple room type mapping requests for a single TLGX Accommodation CommonHotelId.
        /// </summary>
        [Required]
        public List<RoomTypeMapping_SIRQ_SupplierData> SupplierData { get; set; }
    }

    /// <summary>
    /// Structure to contain single / multiple supplier room type mapping requests grouped at TLGX Accommodation CommonHotelId.
    /// </summary>
    public class RoomTypeMapping_SIRQ_SupplierData
    {
        /// <summary>
        /// Nakshatra mapping system's SupplierCode. This can be retreieved by using SupplierMaster api.
        /// </summary>
        [Required]
        public string SupplierId { get; set; }
        /// <summary>
        /// Supplier Product code for accommodation.
        /// </summary>
        [Required]
        public string SupplierProductId { get; set; }
        /// <summary>
        /// Collection of supplier room types. Ideally this should be de-duplicated at your end before making the request.
        /// </summary>
        [Required]
        public List<RoomTypeMapping_SIRQ_SupplierRoomType> SupplierRoomTypes { get; set; }
    }

    /// <summary>
    /// Structure to contain single / multiple supplier room type.
    /// </summary>
    public class RoomTypeMapping_SIRQ_SupplierRoomType
    {
        //public string AccommodationSupplierRoomTypeMappingId { get; set; }
        /// <summary>
        /// Supplier system room id.
        /// </summary>
        [Required]
        public string SupplierRoomId { get; set; }
        /// <summary>
        /// Supplier room type code. Certain suppliers use both id and code values.
        /// </summary>
        public string SupplierRoomTypeCode { get; set; }
        /// <summary>
        /// Supplier room type name.
        /// </summary>
        [Required]
        public string SupplierRoomName { get; set; }
        /// <summary>
        /// Supplier room category name. Certain suppliers use both room type and category.
        /// </summary>
        public string SupplierRoomCategory { get; set; }
        /// <summary>
        ///  Supplier room category id. Certain suppliers use both room type and category.
        /// </summary>
        public string SupplierRoomCategoryId { get; set; }
        /// <summary>
        /// The maximum number of adults permitted in the room stay.
        /// </summary>
        public string MaxAdults { get; set; }
        /// <summary>
        /// The maximum number of children permitted in the room stay.
        /// </summary>
        public string MaxChild { get; set; }
        /// <summary>
        /// The maximum number of infants permitted in the room stay.
        /// </summary>
        public string MaxInfants { get; set; }
        /// <summary>
        /// The maximum number of total occupancy permitted in the room stay.
        /// </summary>
        public string MaxGuestOccupancy { get; set; }
        /// <summary>
        /// Quantity of suppier rooms of this type.
        /// </summary>
        public string Quantity { get; set; }
        /// <summary>
        /// Specific rate plan name for this room stay (For future use).
        /// </summary>
        public string RatePlan { get; set; }
        /// <summary>
        /// Specific rate plan code for this room stay (For future use).
        /// </summary>
        public string RatePlanCode { get; set; }
        /// <summary>
        /// The size of the room
        /// </summary>
        public string RoomSize { get; set; }
        /// <summary>
        /// The type of bathroom in the room stay.
        /// </summary>
        public string BathRoomType { get; set; }
        /// <summary>
        /// The type of room view. Accepts either code or name.
        /// </summary>
        public string RoomView { get; set; }
        /// <summary>
        /// The name of the floor if specified by the supplier.
        /// </summary>
        public string FloorName { get; set; }
        /// <summary>
        /// The number of the floor if specified by the supplier.
        /// </summary>
        public string FloorNumber { get; set; }
        /// <summary>
        /// Amenities of the room stay if specified by the supplier (For future use).
        /// </summary>
        public string[] Amenities { get; set; }
        /// <summary>
        /// Any specific room location code if specified by the supplier.
        /// </summary>
        public string RoomLocationCode { get; set; }
        /// <summary>
        /// Maximum child age for this room stay if specified by the supplier.
        /// </summary>
        public string ChildAge { get; set; }
        /// <summary>
        /// if extra bed applicable for this room stay if specified by the supplier.
        /// </summary>
        public string ExtraBed { get; set; }
        /// <summary>
        /// The number of bedrooms for this room stay if specified by the supplier.
        /// </summary>
        public string Bedrooms { get; set; }
        /// <summary>
        /// Is Smoking permitted in this room stay if specified by the supplier.
        /// </summary>
        public string Smoking { get; set; }
        /// <summary>
        /// The type of bed for this room stay if specified by the supplier.
        /// </summary>
        public string BedType { get; set; }
        /// <summary>
        /// The minimum guest occupancy for this room stay if specified by the supplier.
        /// </summary>
        public string MinGuestOccupancy { get; set; }
        /// <summary>
        /// Any promotional vendor code for this room stay if specified by the supplier (For future use).
        /// </summary>
        public string PromotionalVendorCode { get; set; }
        /// <summary>
        /// Any specific bedding configuration for this room stay if specified by the supplier.
        /// </summary>
        public string BeddingConfig { get; set; }
    }

    #endregion

    #region Response

    /// <summary>
    /// This is the main room type mapping response containing mode, tracking information and the collection of TLGX Accommodation room type mapping responses.
    /// </summary>
    public class RoomTypeMapping_SIRS
    {
        /// <summary>
        /// Your mode that was submitted in the mapping request.
        /// </summary>
        public string Mode { get; set; }
        /// <summary>
        /// Your batch id for tracablility that was submitted in the mapping request.
        /// </summary>
        public string BatchId { get; set; }
        /// <summary>
        /// Room type mapping responses that were submitted in the mapping request.
        /// </summary>
        public List<RoomTypeMapping_SIRS_HotelRoomTypeMappingResponses> HotelRoomTypeMappingResponses { get; set; }
    }

    /// <summary>
    /// Structure to contain single / multiple supplier room type mapping responses grouped at TLGX Accommodation CommonHotelId.
    /// </summary>
    public class RoomTypeMapping_SIRS_SupplierRoomType
    {
        /// <summary>
        /// Original Supplier Room Id that was submitted in the mapping request.
        /// </summary>
        public string SupplierRoomId { get; set; }
        /// <summary>
        /// original supplier room name that was submitted in the mapping request.
        /// </summary>
        public string SupplierRoomName { get; set; }
        /// <summary>
        /// Original Supplier room type code that was submitted in the mapping request.
        /// </summary>
        public string SupplierRoomTypeCode { get; set; }
        /// <summary>
        /// Original Supplier room category that was submitted in the mapping request.
        /// </summary>
        public string SupplierRoomCategory { get; set; }
        /// <summary>
        /// Original Supplier room category id that was submitted in the mapping request.
        /// </summary>
        public string SupplierRoomCategoryId { get; set; }
        /// <summary>
        /// TLGX Accommodation Room Info Id.
        /// If value is empty, there is no map available.
        /// </summary>
        public string TLGXCommonRoomId { get; set; }
        /// <summary>
        /// Nakshatra room type mapping id.
        /// If value is 0, there is no map available.
        /// </summary>
        public string MapId { get; set; }
    }

    /// <summary>
    /// Structure to contain single / multiple supplier room type mapping responses grouped at TLGX Accommodation CommonHotelId. 
    /// </summary>
    public class RoomTypeMapping_SIRS_SupplierData
    {
        /// <summary>
        /// Supplier Code that was submitted in the mapping request.
        /// </summary>
        public string SupplierId { get; set; }
        /// <summary>
        /// Supplier product id that was submitted in the mapping request.
        /// </summary>
        public string SupplierProductId { get; set; }
        /// <summary>
        /// Collection of supplier room types that was submitted in the mapping request.
        /// </summary>
        public List<RoomTypeMapping_SIRS_SupplierRoomType> SupplierRoomTypes { get; set; }
    }

    /// <summary>
    /// Structure for Supplier room type mapping response.
    /// </summary>
    public class RoomTypeMapping_SIRS_HotelRoomTypeMappingResponses
    {
        /// <summary>
        /// TLGX Accommodation CommonHotelId that was submitted in the mapping request.
        /// </summary>
        public string TLGXCommonHotelId { get; set; }
        /// <summary>
        /// Collection of Supplier data for room type grouped at TLGX Accommodation CommonHotelId that was submitted in the mapping request.
        /// </summary>
        public List<RoomTypeMapping_SIRS_SupplierData> SupplierData { get; set; }
    }

    #endregion

    #region RoomTypeMappingOnline
    public class RoomTypeMappingOnline
    {
        /// <summary>
        /// Unique id of the document
        /// </summary>
        [BsonIgnoreIfDefault]
        [BsonId]
        public ObjectId _id { get; set; }
        /// <summary>
        /// This field specifies the type of room type mapping requests. You should always set this value to "online".
        /// </summary>
        public string Mode { get; set; }
        /// <summary>
        /// Please generate a unique Batch / Tracking ID to allow end to end tracebility.
        /// </summary>
        public string BatchId { get; set; }
        /// <summary>
        /// TLGX Accommodation CommonHotelId - TLGX MDM unique identifier for a hotel property. This value can be retrived by using ProductMappingLite api.
        /// </summary>
        public string TLGXCommonHotelId { get; set; }
        /// <summary>
        /// Nakshatra mapping system's SupplierCode. This can be retreieved by using SupplierMaster api.
        /// </summary>
        public string SupplierId { get; set; }
        /// <summary>
        /// Supplier Product code for accommodation.
        /// </summary>
        public string SupplierProductId { get; set; }
        /// <summary>
        /// Supplier system room id.
        /// </summary>
        public string SupplierRoomId { get; set; }
        /// <summary>
        /// Supplier room type code. Certain suppliers use both id and code values.
        /// </summary>
        public string SupplierRoomTypeCode { get; set; }
        /// <summary>
        /// Supplier room type name.
        /// </summary>
        public string SupplierRoomName { get; set; }
        /// <summary>
        /// Supplier room category name. Certain suppliers use both room type and category.
        /// </summary>
        public string SupplierRoomCategory { get; set; }
        /// <summary>
        ///  Supplier room category id. Certain suppliers use both room type and category.
        /// </summary>
        public string SupplierRoomCategoryId { get; set; }
        /// <summary>
        /// The maximum number of adults permitted in the room stay.
        /// </summary>
        public string MaxAdults { get; set; }
        /// <summary>
        /// The maximum number of children permitted in the room stay.
        /// </summary>
        public string MaxChild { get; set; }
        /// <summary>
        /// The maximum number of infants permitted in the room stay.
        /// </summary>
        public string MaxInfants { get; set; }
        /// <summary>
        /// The maximum number of total occupancy permitted in the room stay.
        /// </summary>
        public string MaxGuestOccupancy { get; set; }
        /// <summary>
        /// Quantity of suppier rooms of this type.
        /// </summary>
        public string Quantity { get; set; }
        /// <summary>
        /// Specific rate plan name for this room stay (For future use).
        /// </summary>
        public string RatePlan { get; set; }
        /// <summary>
        /// Specific rate plan code for this room stay (For future use).
        /// </summary>
        public string RatePlanCode { get; set; }
        /// <summary>
        /// The size of the room
        /// </summary>
        public string RoomSize { get; set; }
        /// <summary>
        /// The type of bathroom in the room stay.
        /// </summary>
        public string BathRoomType { get; set; }
        /// <summary>
        /// The type of room view. Accepts either code or name.
        /// </summary>
        public string RoomView { get; set; }
        /// <summary>
        /// The name of the floor if specified by the supplier.
        /// </summary>
        public string FloorName { get; set; }
        /// <summary>
        /// The number of the floor if specified by the supplier.
        /// </summary>
        public string FloorNumber { get; set; }
        /// <summary>
        /// Amenities of the room stay if specified by the supplier (For future use).
        /// </summary>
        public string[] Amenities { get; set; }
        /// <summary>
        /// Any specific room location code if specified by the supplier.
        /// </summary>
        public string RoomLocationCode { get; set; }
        /// <summary>
        /// Maximum child age for this room stay if specified by the supplier.
        /// </summary>
        public string ChildAge { get; set; }
        /// <summary>
        /// if extra bed applicable for this room stay if specified by the supplier.
        /// </summary>
        public string ExtraBed { get; set; }
        /// <summary>
        /// The number of bedrooms for this room stay if specified by the supplier.
        /// </summary>
        public string Bedrooms { get; set; }
        /// <summary>
        /// Is Smoking permitted in this room stay if specified by the supplier.
        /// </summary>
        public string Smoking { get; set; }
        /// <summary>
        /// The type of bed for this room stay if specified by the supplier.
        /// </summary>
        public string BedType { get; set; }
        /// <summary>
        /// The minimum guest occupancy for this room stay if specified by the supplier.
        /// </summary>
        public string MinGuestOccupancy { get; set; }
        /// <summary>
        /// Any promotional vendor code for this room stay if specified by the supplier (For future use).
        /// </summary>
        public string PromotionalVendorCode { get; set; }
        /// <summary>
        /// Any specific bedding configuration for this room stay if specified by the supplier.
        /// </summary>
        public string BeddingConfig { get; set; }

        public string CityCode { get; set; }
        public string CityName { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string StateCode { get; set; }
        public string StateName { get; set; }
        public string RoomDescription { get; set; }
        public string SupplierProductName { get; set; }
        public string SupplierProvider { get; set; }

        public DateTime CreateDateTime { get; set; }
        public DateTime? ProcessDateTime { get; set; }
        public string ProcessBatchId { get; set; }
        public int? ProcessBatchNo { get; set; }

        public string Accommodation_SupplierRoomType_Id { get; set; }
        public string Accommodation_Id { get; set; }
        public string Accommodation_RoomInfo_Id { get; set; }
        public string Status { get; set; }
        public int? SystemRoomTypeMapId { get; set; }
        public float? MatchingScore { get; set; }
        public int? SystemProductCode { get; set; }
        public string SystemRoomTypeCode { get; set; }
        public string TLGXRoomTypeCode { get; set; }
    }
    #endregion
}