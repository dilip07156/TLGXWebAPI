using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DistributionWebApi.Models
{
    /// <summary>
    /// Request structure for unified hotel and room mapping
    /// </summary>
    public class UnifiedHotelAndRoomMapping_RQ
    {
        /// <summary>
        /// Session Id from Booking Engine for the originating Search Response that is to be mapped. 
        /// This field is designed to be used to interrogate original Supplier Static calls should a map not be found within the TLGX Mapping Database
        /// </summary>
        [Required]
        public string SessionId { get; set; }

        /// <summary>
        /// Mapping requests
        /// </summary>
        [Required]
        public List<UnifiedHotelAndRoomMapping_Request> MappingRequests { get; set; }
    }



    public class UnifiedHotelAndRoomMapping_Request
    {
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

        //Need to remove once it is Confirm
        ///// <summary>
        ///// Supplier Product Company Id received in the Supplier Availability Response.
        ///// </summary>
        //[Required]
        //public string TLGXCompanyId { get; set; }
        //Need to remove once it is Confirm

        /// <summary>
        /// Collection of Supplier Room Types
        /// </summary>
        public List<UnifiedHotelAndRoomMapping_RoomTypeRequest> SupplierRoomTypes { get; set; }
    }

    public class UnifiedHotelAndRoomMapping_RoomTypeRequest
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
        /// <summary>
        /// Any specific meal plan for this room stay if specified by the supplier.
        /// </summary>
        public string MealPlan { get; set; }
        /// <summary>
        /// Any specific meal plan code for this room stay if specified by the supplier.
        /// </summary>
        public string MealPlanCode { get; set; }
    }

    /// <summary>
    /// Response structure for unified hotel and room mapping
    /// </summary>
    public class UnifiedHotelAndRoomMapping_RS
    {
        /// <summary>
        /// Session Id from Booking Engine for the originating Search Response that is to be mapped. 
        /// This field is designed to be used to interrogate original Supplier Static calls should a map not be found within the TLGX Mapping Database
        /// </summary>
        [Required]
        public string SessionId { get; set; }

        /// <summary>
        /// Mapping responses
        /// </summary>
        [Required]
        public List<UnifiedHotelAndRoomMapping_Response> MappingResponses { get; set; }
    }

    public class UnifiedHotelAndRoomMapping_Response
    {
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

        //Need to remove once it is Confirm
        /// <summary>
        /// Tlgx Specified Unique Company Id
        /// </summary>
        //[Required]
        //public string TlgxCompanyId { get; set; }
        //Need to remove once it is Confirm

        /// <summary>
        /// Tlgx Specified Unique Company Id
        /// </summary>
        [Required]
        public string TlgxCompanyHotelId { get; set; }

        /// <summary>
        /// System Product Code / Common product code
        /// </summary>
        public string CommonHotelId { get; set; }

        /// <summary>
        /// System Mapping Id
        /// </summary>
        public int ProductMapId { get; set; }

        /// <summary>
        /// TLGX MDM Unique identifier for hotel property
        /// </summary>
        public string TlgxAccoId { get; set; }

        /// <summary>
        /// TLGX MDM hotel property name
        /// </summary>
        public string TlgxAccoName { get; set; }

        /// <summary>
        /// TLGX MDM hotel Product Sub Type
        /// </summary>
        public string ProductSubType { get; set; }

        /// <summary>
        /// TLGX MDM hotel Brand
        /// </summary>
        public string Brand { get; set; }

        /// <summary>
        /// TLGX MDM hotel Chain
        /// </summary>
        public string Chain { get; set; }

        /// <summary>
        /// System Country Code
        /// </summary>
        public string SystemCountryCode { get; set; }

        /// <summary>
        /// System Country Name
        /// </summary>
        public string SystemCountryName { get; set; }

        /// <summary>
        /// System City Code
        /// </summary>
        public string SystemCityCode { get; set; }

        /// <summary>
        /// System City Name
        /// </summary>
        public string SystemCityName { get; set; }

        /// <summary>
        /// System State Code
        /// </summary>
        public string SystemStateCode { get; set; }

        /// <summary>
        /// System State Name
        /// </summary>
        public string SystemStateName { get; set; }

        /// <summary>
        /// This field will indicate whether the hotel have any room mapping attached to it or not
        /// </summary>
        public bool ContainsRoomMappings { get; set; }

        /// <summary>
        /// Collection of Supplier Room Types
        /// </summary>
        public List<UnifiedHotelAndRoomMapping_RoomTypeResponse> SupplierRoomTypes { get; set; }
    }

    public class UnifiedHotelAndRoomMapping_RoomTypeResponse
    {
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
        /// List of mapped rooms to supplier room.
        /// </summary>
        public List<UnifiedHotelAndRoomMapping_MappedRoomType> MappedRooms { get; set; }
    }

    public class UnifiedHotelAndRoomMapping_MappedRoomType
    {
        /// <summary>
        /// TLGX Accommodation Room Info Id.
        /// </summary>
        public string TlgxAccoRoomId { get; set; }

        /// <summary>
        /// TLGX Accommodation Room Type Name.
        /// </summary>
        public string TlgxAccoRoomName { get; set; }

        /// <summary>
        /// TLGX Accommodation Room Type Category.
        /// </summary>
        public string TlgxAccoRoomCategory { get; set; }

        /// <summary>
        /// Nakshatra room type mapping id.
        /// </summary>
        public Int64 RoomMapId { get; set; }
    }

    //GAURAV_TMAP_1035

    /// <summary>
    /// Request structure for unified hotel and room mapping
    /// </summary>
    public class CompanySpecificHotelAndRoomType_Rq
    {
        /// <summary>
        /// Session Id from Booking Engine for the originating Search Response that is to be mapped. 
        /// This field is designed to be used to interrogate original Supplier Static calls should a map not be found within the TLGX Mapping Database
        /// </summary>
        [Required]
        public string SessionId { get; set; }

        /// <summary>
        /// Mapping requests
        /// </summary>
        [Required]
        public List<CompanySpecificHotelAndRoomType_Request> MappingRequests { get; set; }
    }

    public class CompanySpecificHotelAndRoomType_Request
    {
        /// <summary>
        /// Sequence Number for the individual mapping requirement within the overall request
        /// </summary>
        [Required]
        public string SequenceNumber { get; set; }

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

        /// <summary>
        /// Supplier Product Company Code received in the Supplier Availability Response.
        /// </summary>
        [Required]
        public string TLGXCompanyCode { get; set; }

        /// <summary>
        /// Collection of Supplier Room Types
        /// </summary>
        public List<CompanySpecificHotelAndRoomType_RoomTypeRequest> SupplierRoomTypes { get; set; }
    }


    public class CompanySpecificHotelAndRoomType_RoomTypeRequest
    {

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

    }

    /// <summary>
    /// Response structure for unified hotel and room mapping
    /// </summary>
    public class CompanySpecificHotelAndRoomType_RS
    {
        /// <summary>
        /// Session Id from Booking Engine for the originating Search Response that is to be mapped. 
        /// This field is designed to be used to interrogate original Supplier Static calls should a map not be found within the TLGX Mapping Database
        /// </summary>
        [Required]
        public string SessionId { get; set; }

        /// <summary>
        /// Mapping responses
        /// </summary>
        [Required]
        public List<CompanySpecificHotelAndRoomType_Response> MappingResponses { get; set; }
    }

    public class CompanySpecificHotelAndRoomType_Response
    {
        /// <summary>
        /// Sequence Number for the individual mapping requirement within the overall request
        /// </summary>
        [Required]
        public string SequenceNumber { get; set; }

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


        /// <summary>        
        /// Supplier Product Name received in the Supplier Availability Response.
        /// If data not found for requested SupplierProductCode then SupplierProductName field will be return blank/null value
        /// </summary>
        [Required]
        public string SupplierProductName { get; set; }

        /// <summary>
        /// Tlgx Specified Unique Company Id
        /// </summary>
        [Required]
        public string TlgxCompanyCode { get; set; } //Filter by Company Code in Version Tables


        /// <summary>
        /// System Product Code / Common product code
        /// If data not found for requested SupplierProductCode then TlgxCommonProductId field will be return blank/null value
        /// </summary>
        public string TlgxCommonProductId { get; set; } //Master Acco CommonProductId

        /// <summary>
        /// TLGX MDM Unique identifier for hotel property
        /// If data not found for requested SupplierProductCode then TlgxCompanyProductId field will be return blank/null value
        /// </summary>
        public string TlgxCompanyProductId { get; set; } //TLGX Acco Id

        /// <summary>
        /// Hotel property name
        /// If data not found for requested SupplierProductCode then ProductName field will be return blank/null value 
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// TLGX MDM hotel Product Category Type
        /// If data not found for requested SupplierProductCode then ProductCategory field will be return blank/null value
        /// </summary>
        public string ProductCategory { get; set; }

        /// <summary>
        /// TLGX MDM hotel Product Category Sub Type
        /// If data not found for requested SupplierProductCode then ProductCategorySubType field will be return blank/null value
        /// </summary>
        public string ProductCategorySubType { get; set; }

        /// <summary>
        /// TLGX MDM hotel Brand
        /// If data not found for requested SupplierProductCode then Brand field will be return blank/null value
        /// </summary>
        public string Brand { get; set; }

        /// <summary>
        /// TLGX MDM hotel Chain
        /// If data not found for requested SupplierProductCode then Chain field will be return blank/null value
        /// </summary>
        public string Chain { get; set; }


        /// <summary>
        /// Hotel Overview
        /// If data not found for requested SupplierProductCode then Interests field will be return empty collection value
        /// </summary>
        public List<string> Interests { get; set; }

        /// <summary>
        /// Hotel Rating
        /// If data not found for requested SupplierProductCode then Rating field will be return blank/null value
        /// </summary>
        public string Rating { get; set; }

        /// <summary>        
        /// This field will indicate whether the hotel have any room mapping attached to it or not
        /// If data not found for requested SupplierProductCode then ContainsRoomMappings field will be return default false value
        /// </summary>
        public bool ContainsRoomMappings { get; set; }

        /// <summary>
        /// This field will indicate whether the hotel have any direct contract or not 
        /// If data not found for requested SupplierProductCode then DirectContract field will be return default false value
        /// </summary>
        public bool DirectContract { get; set; }

        /// <summary>
        /// Collection of Supplier Room Types
        /// If Supplier Room Type data is requested then SupplierRoomTypes will be not empty collection 
        /// </summary>
        public List<CompanySpecificHotelAndRoomTypeMapping_RoomTypeResponse> SupplierRoomTypes { get; set; }

    }

    public class CompanySpecificHotelAndRoomTypeMapping_RoomTypeResponse
    {
        /// <summary>
        /// Supplier system room id.
        /// if data is not found requested SupplierRoomTypes then SupplierRoomId field will return null or returns requested parameter value
        /// </summary>
        [Required]
        public string SupplierRoomId { get; set; }

        /// <summary>
        /// Supplier room type code. Certain suppliers use both id and code values.
        /// if data is not found requested SupplierRoomTypes then SupplierRoomTypeCode field will return null or returns requested parameter value
        /// </summary>
        public string SupplierRoomTypeCode { get; set; }

        /// <summary>
        /// Supplier room type name.
        /// if data is not found requested SupplierRoomTypes then SupplierRoomName field will return null or returns requested parameter value
        /// </summary>
        [Required]
        public string SupplierRoomName { get; set; }

        /// <summary>
        /// Supplier room category name. Certain suppliers use both room type and category.
        /// if data is not found requested SupplierRoomTypes then SupplierRoomCategory field will return null or returns requested parameter value
        /// </summary>
        public string SupplierRoomCategory { get; set; }

        /// <summary>
        ///  Supplier room category id. Certain suppliers use both room type and category.
        ///  if data is not found requested SupplierRoomTypes then SupplierRoomCategoryId field will return null or returns requested parameter value
        /// </summary>
        public string SupplierRoomCategoryId { get; set; }

        /// <summary>
        /// List of mapped rooms to supplier room.
        /// if data is not found then MappedRooms field will return empty collection
        /// </summary>
        public List<CompanySpecificHotelAndRoomTypeMapping_MappedRoomType> MappedRooms { get; set; }
    }

    public class CompanySpecificHotelAndRoomTypeMapping_MappedRoomType
    {
        /// <summary>
        /// TLGX Accommodation Room Info Id.
        /// </summary>
        public string TlgxCompanyRoomId { get; set; }//TlgxAccoRoomId

        /// <summary>
        /// TLGX Accommodation Room Common Room Id.
        /// </summary>
        public string TLGXCommonRoomId { get; set; }//TlgxAccoRoomCategory 

        /// <summary>
        /// TLGX Accommodation Room Type Name.
        /// </summary>
        public string CompanyRoomName { get; set; }

        /// <summary>
        /// TLGX Accommodation Room Type Category.
        /// </summary>
        public string CompanyRoomCategory { get; set; }//TlgxAccoRoomCategory 


        /// <summary>
        /// Nakshatra room type mapping id.
        /// </summary>
        public Int64 NakshatraRoomMappingId { get; set; }
    }

}