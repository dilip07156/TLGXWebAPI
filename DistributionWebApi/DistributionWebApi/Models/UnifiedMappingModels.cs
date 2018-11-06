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

}