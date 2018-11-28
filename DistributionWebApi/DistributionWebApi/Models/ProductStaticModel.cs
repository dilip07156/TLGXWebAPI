using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace DistributionWebApi.Models.Static
{
    /// <summary>
    /// product static data request format. 
    /// </summary>
    public class StaticData_RQ
    {
        /// <summary>
        /// Supplier company code as mapped against system
        /// </summary>
        public string SupplierCode { get; set; }

        /// <summary>
        /// Supplier specific product code (not a mapping system specific product code)
        /// </summary>
        public string SupplierProductCode { get; set; }
    }

    /// <summary>
    /// response containing a collection of requested product details
    /// </summary>
    public class StaticData_RS
    {
        /// <summary>
        /// Supplier company code as mapped against system as received in request
        /// </summary>
        public string SupplierCode { get; set; }

        /// <summary>
        /// Supplier specific product code (not a mapping system specific product code)  as received in request
        /// </summary>
        public string SupplierProductCode { get; set; }

        /// <summary>
        /// Product static information. If the search result is not successfull then a null value will be returned.
        /// </summary>
        public Accomodation Result { get; set; }
    }

    /// <summary>
    /// Contains the base info for all accommodation products
    /// </summary>
    [BsonIgnoreExtraElements(true)]
    public class Accomodation
    {
        //[Newtonsoft.Json.JsonProperty("_id")]
        //public string BsonId { get; set; }

        /// <summary>
        /// A collection of room ids
        /// </summary>
        public string[] RoomIds { get; set; }

        /// <summary>
        /// Key information about the product (Includes Address, Description, Contact info etc)
        /// </summary>
        public AccomodationInfo AccomodationInfo { get; set; }

        /// <summary>
        /// Classification info about the product includes usp, recommendations about the product
        /// </summary>
        public Overview Overview { get; set; }

        /// <summary>
        /// future use for home stay
        /// </summary>
        public StaffContactInfo StaffContactInfo { get; set; }

        /// <summary>
        /// Product facilities and amenities information
        /// </summary>
        public List<Facility> Facility { get; set; }

        /// <summary>
        /// for future use
        /// </summary>
        public PassengerOccupancy PassengerOccupancy { get; set; }

        /// <summary>
        /// how to get to the product
        /// </summary>
        public List<Directions> Directions { get; set; }

        /// <summary>
        /// near by places includes attractions, musuems etc
        /// </summary>
        public List<InAndAround> InAndAround { get; set; }

        /// <summary>
        /// near by famous landmarks
        /// </summary>
        public List<Landmark> Landmark { get; set; }

        /// <summary>
        /// for future use
        /// </summary>
        public List<Rule> Rule { get; set; }

        /// <summary>
        /// Images/ Videos of the product
        /// </summary>
        public List<StaticMedia> Media { get; set; }

        /// <summary>
        /// for future use
        /// </summary>
        public List<Updates> Updates { get; set; }

        /// <summary>
        /// for future use
        /// </summary>
        public List<SafetyRegulations> SafetyRegulations { get; set; }

        /// <summary>
        /// for future use
        /// </summary>
        public List<Ancillary> Ancillary { get; set; }

        /// <summary>
        /// for future use
        /// </summary>
        public ProductStatus ProductStatus { get; set; }

        //New field Added
        /// <summary>
        /// Policies of product
        /// </summary>
        public List<Policies> Policies { get; set; }

        //New field Added
        /// <summary>
        /// Collection rooms details of product
        /// </summary>
        public List<Rooms> Rooms { get; set; }
        //public SupplierDetails SupplierDetails { get; set; }

        //public string CreatedBy { get; set; }
        //public string UpdatedBy { get; set; }
        //public string CreatedAt { get; set; }
        //public string LastUpdated { get; set; }
        //public bool Deleted { get; set; }

        //public Lock Lock { get; set; }
    }

    //public class SupplierDetails
    //{
    //    public string SupplierCode { get; set; }
    //    public string SupplierProductCode { get; set; }
    //}

    //public class Lock
    //{
    //    public bool Enabled { get; set; }
    //    public string User { get; set; }
    //}

    /// <summary>
    /// Classification info for Accommodation product
    /// </summary>
    public class AccomodationInfo
    {
        /// <summary>
        ///  Supplier Company Code
        /// </summary>
        public string CompanyId { get; set; }
        /// <summary>
        /// Supplier Company Name
        /// </summary>
        public string CompanyName { get; set; }

        //New field Added
        /// <summary>
        /// Accommodation Type Code : Including HT, MT, APT,1,2 etc.
        /// </summary>
        public string ProductCategorySubTypeId { get; set; }


        //[JsonProperty(Required = Required.Always)]
        /// <summary>
        /// Accommodation Type : Including Hotel, Motel, Apartment, HomeStay, Caravan Park
        /// </summary>
        public string ProductCatSubType { get; set; }

        /// <summary>
        /// Is this a mystery product
        /// </summary>
        public bool IsMysteryProduct { get; set; }

        /// <summary>
        /// System Product Id, if mapped to system master product
        /// </summary>
        public string CommonProductId { get; set; }

        /// <summary>
        /// Supplier Product code
        /// </summary>
        public string CompanyProductId { get; set; }

        //[JsonProperty(Required = Required.Always)]
        /// <summary>
        /// Accommodation Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Not specified (for future use)
        /// </summary>
        public string FinanceControlId { get; set; }

        /// <summary>
        /// Chain of the accommodation product
        /// </summary>
        public string Chain { get; set; }

        /// <summary>
        /// Brand of the accommodation product
        /// </summary>
        public string Brand { get; set; }

        /// <summary>
        /// Not specified (for future use)
        /// </summary>
        public string Affiliations { get; set; }

        //[JsonProperty(Required = Required.Always)]
        /// <summary>
        /// Product star rating
        /// </summary>
        public string Rating { get; set; }

        //New field Added
        /// <summary>
        /// Exact Star rating of this Product
        /// </summary>
        public string ExactRating { get; set; }

        /// <summary>
        /// Star rating of this Product
        /// </summary>
        public string Stars { get; set; }

        /// <summary>
        /// Review score of this Product
        /// </summary>
        public string ReviewScore { get; set; }

        /// <summary>
        /// The review score of the Product
        /// </summary>
        public string noOfReviews { get; set; }



        /// <summary>
        /// Internal Product star rating
        /// </summary>
        public string CompanyRating { get; set; }

        /// <summary>
        /// Product Ranking
        /// </summary>
        //New field Added
        public string HotelRanking { get; set; }


        /// <summary>
        /// Internal Product star rating reviewed on
        /// </summary>
        public Date RatingDatedOn { get; set; }

        /// <summary>
        /// How many floor does the product have
        /// </summary>
        public int NoOfFloors { get; set; }

        /// <summary>
        /// How many room does the product have
        /// </summary>
        public int NoOfRooms { get; set; }

        /// <summary>
        /// Is this a recommended product ?
        /// </summary>
        public string[] RecommendedFor { get; set; }

        /// <summary>
        /// The display name for the product
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Does the accommodation offer 24hr checkout (for future use)
        /// </summary>
        public bool IsTwentyFourHourCheckout { get; set; }

        //[JsonProperty(Required = Required.Always)]
        /// <summary>
        /// What is the check in time
        /// </summary>
        public string CheckInTime { get; set; }

        //[JsonProperty(Required = Required.Always)]
        /// <summary>
        /// What is the check out time
        /// </summary>
        public string CheckOutTime { get; set; }

        //New field Added
        /// <summary>
        /// What is the check in close time
        /// </summary>
        public string CheckinCloseTime { get; set; }

        /// <summary>
        /// What is the check out close time
        /// </summary>
        public string CheckoutCloseTime { get; set; }


        /// <summary>
        /// (for future use)
        /// </summary>
        public FamilyDetails FamilyDetails { get; set; }

        /// <summary>
        /// (for future use)
        /// </summary>
        public string ResortType { get; set; }

        /// <summary>
        /// Contains the address information for the product
        /// </summary>
        public Address Address { get; set; }

        /// <summary>
        /// Contains the contact information for the product
        /// </summary>
        public List<ContactDetails> ContactDetails { get; set; }

        /// <summary>
        /// Contains general descriptive information about the product
        /// </summary>
        public General General { get; set; }



        //New field Added
        /// <summary>
        /// Contain flag for product, is bookable without credit card.
        /// </summary>
        public bool IsBookWithoutCC { get; set; }
        /// <summary>
        /// Is product is recommended.
        /// </summary>
        public bool IsRecommended { get; set; }
        /// <summary>
        /// Is rating is estimated for Product
        /// </summary>
        public bool IsRatingEstimatedAutomatically { get; set; }

        /// <summary>
        /// Is creditcard is required 
        /// </summary>
        public bool IsCreditcardRequired { get; set; }

        /// <summary>
        /// Contain info of the inside look at the hotel.
        /// </summary>
        public string HotelMessage { get; set; }

        /// <summary>
        /// Maximum number of persons in a reservation.
        /// </summary>
        public string MaxPersonReservation { get; set; }

        /// <summary>
        ///Maximum number of rooms this property allows in one booking.
        /// </summary>
        public string MaxRoomReservation { get; set; }

        /// <summary>
        /// The default language of the Product.
        /// </summary>
        public string DefaultLanguage { get; set; }

        /// <summary>
        /// Languages spoken by the product's staff
        /// </summary>
        public string SpokenLanguages { get; set; }

        /// <summary>
        ///  currency code that the charge is shown in.
        /// </summary>
        public string currency { get; set; }

        /// <summary>
        /// Limit the results to properties with the specified facilities.  
        /// </summary>
        public string Facilities { get; set; }

    }

    public class Date
    {
        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
    }

    public class FamilyDetails
    {
        //[JsonProperty(Required = Required.Always)]
        public string FamilyName { get; set; }

        //[JsonProperty(Required = Required.Always)]
        public string FamilyDescription { get; set; }
    }

    /// <summary>
    /// Holds the address information for a product
    /// </summary>
    public class Address
    {
        /// <summary>
        /// Flat /  House / Building Block Number
        /// </summary>
        public string HouseNumber { get; set; }

        //[JsonProperty(Required = Required.Always)]
        /// <summary>
        /// Address detailing
        /// </summary>
        public string Street { get; set; }

        /// <summary>
        /// Address detailing
        /// </summary>
        public string Street2 { get; set; }

        /// <summary>
        /// Address detailing
        /// </summary>
        public string Street3 { get; set; }

        /// <summary>
        /// Address detailing
        /// </summary>
        public string Street4 { get; set; }

        /// <summary>
        /// Address detailing
        /// </summary>
        public string Street5 { get; set; }

        /// <summary>
        /// (for future use)
        /// </summary>
        public string Zone { get; set; }

        /// <summary>
        /// Address detailing
        /// </summary>
        //[JsonProperty(Required = Required.Always)]
        public string PostalCode { get; set; }

        //New field Added
        /// <summary>
        /// Contain unique id of the Country
        /// </summary>
        public string CountryCode { get; set; }

        /// <summary>
        /// Address detailing
        /// </summary>
        //[JsonProperty(Required = Required.Always)]
        public string Country { get; set; }

        //New field Added
        /// <summary>
        /// Contain state code
        /// </summary>
        public string StateCode { get; set; }

        /// <summary>
        /// Address detailing
        /// </summary>
        //[JsonProperty(Required = Required.Always)]
        public string State { get; set; }

        //New field Added
        /// <summary>
        /// Contain unique id of the district
        /// </summary>
        public string DistrictId { get; set; }

        /// <summary>
        /// Contain district name 
        /// </summary>
        public string DistrictName { get; set; }

        //New field Added
        /// <summary>
        /// Contain unique id of the city
        /// </summary>
        public string CityCode { get; set; }

        /// <summary>
        /// Address detailing
        /// </summary>
        //[JsonProperty(Required = Required.Always)]
        public string City { get; set; }

        /// <summary>
        /// Address detailing - First level below the city
        /// </summary>
        //[JsonProperty(Required = Required.Always)]
        public string Area { get; set; }

        //New field Added
        /// <summary>
        /// Contain the region
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// Address detailing - Second level below the city 
        /// </summary>
        //[JsonProperty(Required = Required.Always)]
        public string Location { get; set; }

        /// <summary>
        /// Address detailing (Latitude and Longitude)
        /// </summary>
        public Geometry Geometry { get; set; }


    }

    public class Geometry
    {
        public string Type { get; set; }

        //[JsonProperty(Required = Required.Always)]
        /// <summary>
        /// Latitude, Longitude
        /// </summary>
        public List<decimal> Coordinates { get; set; }
    }

    /// <summary>
    /// Specifies the contact information for a product
    /// </summary>
    public class ContactDetails
    {
        /// <summary>
        /// Telephone number
        /// </summary>
        public TelephoneFormat Phone { get; set; }

        /// <summary>
        /// Fax number
        /// </summary>
        public TelephoneFormat Fax { get; set; }

        /// <summary>
        /// Website URL for the product
        /// </summary>
        public string Website { get; set; }

        /// <summary>
        /// Mobile App URL for the product
        /// </summary>
        //New field Added
        public string MobileAppUrl { get; set; }

        /// <summary>
        /// general email address of the product
        /// </summary>
        public string EmailAddress { get; set; }
    }

    /// <summary>
    /// Structure of the telephone / fax format
    /// </summary>
    public class TelephoneFormat
    {
        /// <summary>
        /// international country code
        /// </summary>
        public string CountryCode { get; set; }

        /// <summary>
        /// international city code
        /// </summary>
        public string CityCode { get; set; }

        /// <summary>
        /// contact number of the product
        /// </summary>
        //[JsonProperty(Required = Required.Always)]
        public string Number { get; set; }
    }

    /// <summary>
    /// Structure to hold the general information for a product
    /// </summary>
    public class General
    {
        /// <summary>
        /// Carbon foot print
        /// </summary>
        public string Cfp { get; set; }

        /// <summary>
        /// When was the product built
        /// </summary>
        public string YearBuilt { get; set; }

        /// <summary>
        /// When was the product renovated
        /// </summary>
        public string YearRenovated { get; set; }

        /// <summary>
        /// Awards received by the product 
        /// </summary>
        public string AwardsReceived { get; set; }

        /// <summary>
        /// Contains descriptions 
        /// </summary>
        public List<Extras> Extras { get; set; }

        /// <summary>
        /// for future use
        /// </summary>
        public string InternalRemarks { get; set; }

        /// <summary>
        /// for future use
        /// </summary>
        public string CopiedFrom { get; set; }
    }


    /// <summary>
    /// Sturcuture to hold descriptions information
    /// </summary>
    public class Extras
    {
        /// <summary>
        /// Product description type (e.g. Short / Long)
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Product description
        /// </summary>
        public string Description { get; set; }
    }

    /// <summary>
    /// Holds classification information for the product
    /// </summary>
    public class Overview
    {
        /// <summary>
        /// for future use
        /// </summary>
        public List<Duration> Duration { get; set; }

        //[JsonProperty(Required = Required.Always)]
        /// <summary>
        /// is the product recommended for any particular interest
        /// </summary>
        public string[] Interest { get; set; }

        /// <summary>
        /// unique selling points
        /// </summary>
        public string Usp { get; set; }

        /// <summary>
        /// for future use
        /// </summary>
        public string[] HashTag { get; set; }

        /// <summary>
        /// any particular highlights for the product
        /// </summary>
        public string Highlights { get; set; }

        /// <summary>
        /// unique selling trips 
        /// </summary>
        public string SellingTips { get; set; }

        /// <summary>
        /// is the product recommended
        /// </summary>
        public bool IsCompanyRecommended { get; set; }
    }

    /// <summary>
    /// for future use
    /// </summary>
    public class Duration
    {
        public Date From { get; set; }
        public Date To { get; set; }
        public string TypeOfDesc { get; set; }
        public string Description { get; set; }
    }

    /// <summary>
    /// for future use
    /// </summary>
    public class StaffContactInfo
    {
        public HostFamilyDetails HostFamilyDetails { get; set; }
        public CertificationDetails CertificationDetails { get; set; }
        public LocationDetails LocationDetails { get; set; }
    }

    /// <summary>
    /// for future use
    /// </summary>
    public class HostFamilyDetails
    {
        public string description { get; set; }
        public int MemberCount { get; set; }
        public List<Members> Members { get; set; }
        public Childrens Childrens { get; set; }
        public Pets Pets { get; set; }
        public bool IsNonSmokinghouseHold { get; set; }

    }

    /// <summary>
    /// for future use
    /// </summary>
    public class Members
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string Description { get; set; }
        public string LanguageSpoken { get; set; }
        public string Interest { get; set; }
    }

    /// <summary>
    /// for future use
    /// </summary>
    public class Childrens
    {
        public bool IsPresent { get; set; }
        public int Count { get; set; }
    }

    /// <summary>
    /// for future use
    /// </summary>
    public class Pets
    {
        public bool IsPresent { get; set; }
        public int Count { get; set; }
        public string PetTypes { get; set; }
    }

    /// <summary>
    /// for future use
    /// </summary>
    public class CertificationDetails
    {
        public string CertifiedHostId { get; set; }
        public string UserCertificationDescription { get; set; }
        public string CriminalRecordDescription { get; set; }
    }

    /// <summary>
    /// for future use
    /// </summary>
    public class LocationDetails
    {
        public string NeighbourHoodDescription { get; set; }
        public string DistanceFromCenter { get; set; }
        public string DistanceFromPublicTransportation { get; set; }
    }

    /// <summary>
    /// the facilities of the product
    /// </summary>
    public class Facility
    {

        //New field Added
        /// <summary>
        /// facility type id
        /// </summary>
        public string Facility_Id { get; set; }

        //New field Added
        /// <summary>
        /// the type of facility
        /// </summary>
        public string Type { get; set; }

        //New field Added
        //New field Added
        /// <summary>
        /// facility category id
        /// </summary>
        public string FacilityCategoryID { get; set; }

        /// <summary>
        /// facility category
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// description of facility
        /// </summary>
        public string Desc { get; set; }

        /// <summary>
        /// starttime of facility (if applicable)
        /// </summary>
        public string OperationalTimeFrom { get; set; }

        /// <summary>
        /// endtime of facility (if applicable)
        /// </summary>
        public string OperationalTimeTo { get; set; }

        /// <summary>
        /// duration of facility (if applicable)
        /// </summary>
        public string Duration { get; set; }

        /// <summary>
        /// if the facility is chargable
        /// </summary>
        public bool IsChargeable { get; set; }

        //New field Added
        /// <summary>
        /// Extra char per facility
        /// </summary>
        public string ExtraCharge { get; set; }


    }

    /// <summary>
    /// for future use
    /// </summary>
    public class PassengerOccupancy
    {
        public List<Type1> Type1 { get; set; }
        public List<Type2> Type2 { get; set; }
    }

    /// <summary>
    /// for future use
    /// </summary>
    public class Type1
    {
        public string RoomCategory { get; set; }
        public string RoomType { get; set; }
        public int MaxAdults { get; set; }
        public AgeFor AgeForPaxExtraBed { get; set; }
        public int MaxPaxWithExtraBeds { get; set; }
        public AgeFor AgeBracketForCnb { get; set; }
        public int MaxCnb { get; set; }
        public AgeFor AgeForCior { get; set; }
        public int MaxCior { get; set; }
        public int MaxPax { get; set; }
    }

    /// <summary>
    /// for future use
    /// </summary>
    public class AgeFor
    {
        public int From { get; set; }
        public int To { get; set; }
    }

    /// <summary>
    /// for future use
    /// </summary>
    public class Type2
    {
        public string RoomCategory { get; set; }

        /// <summary>
        /// Maximum adult people for the room.
        /// </summary>
        public int MaxAdults { get; set; }

        /// <summary>
        /// Maximum children for the room.
        /// </summary>
        public int MaxChild { get; set; }

        /// <summary>
        /// Maximum people for the room.
        /// </summary>
        public int MaxPax { get; set; }
    }

    /// <summary>
    /// structure to hold the mesurement information
    /// </summary>
    public class Distance
    {
        public string Value { get; set; }
        public string Unit { get; set; }
    }

    /// <summary>
    /// directions to landmark from the property
    /// </summary>
    public class Directions
    {
        //[JsonProperty(Required = Required.Always)]
        public string From { get; set; }

        //[JsonProperty(Required = Required.Always)]
        public string NameOfPlace { get; set; }

        //[JsonProperty(Required = Required.Always)]
        public string ModeOfTransport { get; set; }

        //[JsonProperty(Required = Required.Always)]
        public string TransportType { get; set; }

        public Distance DistanceFromProperty { get; set; }
        public Distance ApproxDuration { get; set; }

        //[JsonProperty(Required = Required.Always)]
        public string Description { get; set; }

        public string DrivingDirection { get; set; }
        public Date ValidityFrom { get; set; }
        public Date ValidityTo { get; set; }
    }

    /// <summary>
    /// near by places of the property includes museums, attractions etc
    /// </summary>
    public class InAndAround
    {
        /// <summary>
        /// Category of the near by places
        /// </summary>
        public string PlaceCategory { get; set; }

        /// <summary>
        /// Name of the near by places
        /// </summary>
        public string PlaceName { get; set; }

        /// <summary>
        /// Distance From Property
        /// </summary>
        public Distance DistanceFromProperty { get; set; }

        /// <summary>
        /// any specific highlights about this place
        /// </summary>
        /// 
        public string LocationHighlights { get; set; }

        /// <summary>
        /// Description about this place
        /// </summary>
        public string Description { get; set; }
    }

    /// <summary>
    /// near by landmarks of the property
    /// </summary>
    public class Landmark
    {
        /// <summary>
        /// Category of the near by places
        /// </summary>
        public string PlaceCategory { get; set; }

        /// <summary>
        /// Name of the near by places
        /// </summary>
        public string PlaceName { get; set; }

        /// <summary>
        /// Distance From Property
        /// </summary>
        public Distance DistanceFromProperty { get; set; }

        /// <summary>
        /// Description about this place
        /// </summary>
        public string Description { get; set; }
    }

    /// <summary>
    /// for future use
    /// </summary>
    public class Rule
    {
        //[JsonProperty(Required = Required.Always)]
        public string Name { get; set; }

        //[JsonProperty(Required = Required.Always)]
        public string Description { get; set; }
    }


    /// <summary>
    /// images and videos of the propertys
    /// </summary>
    public class StaticMedia
    {
        //[JsonProperty(Required = Required.Always)]
        public string MediaId { get; set; }

        /// <summary>
        /// Media File category
        /// </summary>
        public string FileCategory { get; set; }

        /// <summary>
        /// Image / Video
        /// </summary>
        public string FileType { get; set; }

        /// <summary>
        /// Path / URL of the media
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Description of the media
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Media category
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Display order of the media
        /// </summary>
        public string MediaPosition { get; set; }

        /// <summary>
        /// Valid from date
        /// </summary>
        public Date From { get; set; }

        /// <summary>
        /// Valid to date
        /// </summary>
        public Date To { get; set; }

        //New field Added
        /// <summary>
        /// Media width
        /// </summary>
        public string MediaWidth { get; set; }

        /// <summary>
        /// Media Height
        /// </summary>
        public string MediaHeight { get; set; }

        /// <summary>
        /// Media Thumbnail
        /// </summary>
        public string ThumbnailUrl { get; set; }

        /// <summary>
        /// Media LargeImageURL
        /// </summary>
        public string LargeImageURL { get; set; }

        /// <summary>
        /// Media SmallImageURL
        /// </summary>
        public string SmallImageURL { get; set; }

        /// <summary>
        /// for future use
        /// </summary>
        public string MediaFileMaster { get; set; }

        /// <summary>
        /// Media file format
        /// </summary>
        public string MediaFileFormat { get; set; }
        //New field Added
    }

    /// <summary>
    /// for future use
    /// </summary>
    public class Updates
    {
        public string UpdateCategory { get; set; }
        public DescriptionType DescriptionType { get; set; }
        public string Description { get; set; }
        public Display Display { get; set; }
        public string Source { get; set; }
        public bool SendUpdates { get; set; }
        public ModeOfCommunication ModeOfCommunication { get; set; }
        public bool DisplayUpdatesOnVoucher { get; set; }
    }

    /// <summary>
    /// for future use
    /// </summary>
    public class DescriptionType
    {
        public bool Internal { get; set; }
        public bool External { get; set; }
    }

    /// <summary>
    /// c
    /// </summary>
    public class Display
    {
        public Date From { get; set; }
        public Date To { get; set; }
    }

    /// <summary>
    /// for future use
    /// </summary>
    public class ModeOfCommunication
    {
        public bool Email { get; set; }
        public bool Sms { get; set; }
    }

    /// <summary>
    /// Safety Regulations for the product
    /// </summary>
    public class SafetyRegulations
    {
        public string Category { get; set; }
        public string Name { get; set; }
        public string Remarks { get; set; }
        public string Value { get; set; }
        public Date LastUpdated { get; set; }
    }

    /// <summary>
    /// for future use
    /// </summary>
    public class Ancillary
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public Date From { get; set; }
        public Date To { get; set; }
    }

    /// <summary>
    /// for future use
    /// </summary>
    public class ProductStatus
    {
        public List<Deactivated> Deactivated { get; set; }
        public string Status { get; set; }
        public Date From { get; set; }
        public Date To { get; set; }
        public string SubmitFor { get; set; }
        public string Reason { get; set; }
        public string Remark { get; set; }
    }

    /// <summary>
    /// for future use
    /// </summary>
    public class Deactivated
    {
        public string MarketId { get; set; }
        public string MarketName { get; set; }
        public Date From { get; set; }
        public Date To { get; set; }
        public string Reason { get; set; }
    }



    //New field Added
    /// <summary>
    /// Containt he polices type and their description
    /// </summary>
    public class Policies
    {
        /// <summary>
        /// Type of policy
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Policy description
        /// </summary>
        public string Description { get; set; }
    }

    /// <summary>
    /// Contain the Room Info of product
    /// </summary>
    public class Rooms
    {
        /// <summary>
        /// ID of this type of the room.
        /// </summary>
        public string RoomTypeId { get; set; }

        /// <summary>
        /// Localized name of the room type
        /// </summary>
        public string RoomTypeName { get; set; }

        /// <summary>
        /// Category ID of this type of the room.
        /// </summary>
        public string RoomCategoryCode { get; set; }

        /// <summary>
        /// Category Name of this type of the room.
        /// </summary>
        public string RoomCategoryName { get; set; }


        public string RoomCode { get; set; }
        public string CompanyRoomCategory { get; set; }

        /// <summary>
        /// The description of this room.
        /// </summary>
        public string RoomDescription { get; set; }

        /// <summary>
        /// Available facility in this room.
        /// </summary>
        public string AmenitiesType { get; set; }

        /// <summary>
        /// Total number of bathrooms for this room.
        /// </summary>
        public int BathRoomCount { get; set; }

        /// <summary>
        /// Type of bathrooms for this room.
        /// </summary>
        public string BathRoomType { get; set; }

        /// <summary>
        /// The size of the room.
        /// </summary>
        public string Size { get; set; }


        public string View { get; set; }
        /// <summary>
        /// Ranking refers to the ranking score of this room relative to the other rooms at the property.
        /// </summary>
        public string RoomRanking { get; set; }

        /// <summary>
        /// If this room can be booked.
        /// </summary>
        public bool IsRoomBookable { get; set; }

        /// <summary>
        /// The maximum price of the room.
        /// </summary>
        public string MaxPrice { get; set; }

        /// <summary>
        /// The minimum price of the room.
        /// </summary>
        public string MinPrice { get; set; }

        /// <summary>
        /// Contain the pax occupancy details.
        /// </summary>
        public PassengerOccupancy PassengerOccupancy { get; set; }

        /// <summary>
        /// Collection of room facilities
        /// </summary>
        public List<RoomAmenities> RoomAmenities { get; set; }
        /// <summary>
        /// Collection of Bedroom details.
        /// </summary>
        public List<BedRooms> BedRooms { get; set; }

        /// <summary>
        /// Collection of room media details.
        /// </summary>
        public List<StaticMedia> RoomMedia { get; set; }


    }

    /// <summary>
    /// Contain the Bed details of room
    /// </summary>
    public class BedRooms
    {
        /// <summary>
        /// Bed type Id in this bed configuration
        /// </summary>
        public string BedTypeID { get; set; }

        /// <summary>
        /// Bed type in this bed configuration
        /// </summary>
        public string BedType { get; set; }

        /// <summary>
        /// Description of the number and size of various bed types in the room.
        /// </summary>

        public string BeddingConfiguration { get; set; }




        /// <summary>
        /// Total number of bedrooms for this room.
        /// </summary>
        public string BedRoomCount { get; set; }

        /// <summary>
        /// Total number of extra beds for adults this room.
        /// </summary>
        public string MaxAdultWithExtraBed { get; set; }

        /// <summary>
        /// Total number of extra beds for childrens this room.
        /// </summary>
        public string MaxChildWithExtraBed { get; set; }

        /// <summary>
        /// Total number of extra beds for this room.
        /// </summary>
        public string NoOfExreaBeds { get; set; }

    }

    /// <summary>
    /// Room facilities of Product
    /// </summary>
    public class RoomAmenities
    {
        /// <summary>
        /// The type id of facility this is (like a category) per room.
        /// </summary>
        public string AmenityId { get; set; }

        /// <summary>
        /// The type of facility this is (like a category) per room.
        /// </summary>
        public string RoomAmenityType { get; set; }

        /// <summary>
        /// The category id of facility this is per room.
        /// </summary>
        public string AmenityCategoryID { get; set; }

        /// <summary>
        /// The category of facility this is per room.
        /// </summary>
        public string AmenityCategory { get; set; }
    }

    /// <summary>
    /// Accommodation Master
    /// </summary>
    public class AccommodationMaster
    {

        /// <summary>
        /// The type id is Hotel ID
        /// </summary>

        //[BsonId]
        //[Newtonsoft.Json.JsonProperty("_id")]
        //[JsonIgnore]
        //[IgnoreDataMember]
        [BsonElement("_id")]
        public int CommonHotelId { get; set; }


        /// <summary>
        /// Contain TLGX Accommodation id of Accommodation
        /// </summary>
        
        public string TLGXAccoId { get; set; }



        /// <summary>
        /// Contain Accommodation Name
        /// </summary>
        public string HotelName { get; set; }


        /// <summary>
        /// Contain Type of Accommodation
        /// </summary>
        public string ProductCategory { get; set; }


        /// <summary>
        /// Contain Subtype of Accommodation
        /// </summary>
        public string ProductCategorySubType { get; set; }

        /// <summary>
        /// Contain start rating of Accommodation
        /// </summary>
        public string HotelStarRating { get; set; }




        /// <summary>
        /// Contain State Code
        /// </summary>
        public string StateCode { get; set; }

        /// <summary>
        /// Contain State Name
        /// </summary>
        public string StateName { get; set; }

        /// <summary>
        /// Contain Street info of Accommodation
        /// </summary>
        public string StreetName { get; set; }

        /// <summary>
        /// Contain Street Number of Accommodation
        /// </summary>
        public string StreetNumber { get; set; }

        /// <summary>
        /// Contain Street3 of Accommodation
        /// </summary>
        public string Street3 { get; set; }

        /// <summary>
        /// Contain Street4 of Accommodation
        /// </summary>
        public string Street4 { get; set; }

        /// <summary>
        /// Contain Street5 of Accommodation
        /// </summary>
        public string Street5 { get; set; }

        /// <summary>
        /// Contain Postal Code info of Accommodation
        /// </summary>
        public string PostalCode { get; set; }

        /// <summary>
        /// Contain Town info of Accommodation
        /// </summary>
        public string Town { get; set; }

        /// <summary>
        /// Contain Location of Accommodation
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Contain Area of Accommodation
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// Contain City Code
        /// </summary>
        public string CityCode { get; set; }

        /// <summary>
        /// Contain City Name
        /// </summary>
        public string CityName { get; set; }


        /// <summary>
        /// Contain ISO Country Code
        /// </summary>
        public string CountryCode { get; set; }

        /// <summary>
        /// Contain Country Name
        /// </summary>
        public string CountryName { get; set; }


        /// <summary>
        /// Contain Room Mapping Completed info of Accommodation
        /// </summary>
        public bool IsRoomMappingCompleted { get; set; }

        /// <summary>
        /// Contain Telephone info of Accommodation
        /// </summary>
        public string Telephone { get; set; }

        /// <summary>
        /// Contain Contact Fax of Accommodation
        /// </summary>
        public string Fax { get; set; }

        /// <summary>
        /// Contain Contact Email of Accommodation
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Contain Contact WebSite of Accommodation
        /// </summary>
        public string WebSiteURL { get; set; }

        /// <summary>
        /// Contain Full Address of Accommodation
        /// </summary>
        public string FullAddress { get; set; }

        /// <summary>
        /// Contain Latitude of Accommodation
        /// </summary>
        public string Latitude { get; set; }

        /// <summary>
        /// Contain Longitude of Accommodation
        /// </summary>
        public string Longitude { get; set; }

        /// <summary>
        /// Contain Status of Accommodation
        /// </summary>
        public string CodeStatus { get; set; }

        /// <summary>
        /// Contain Full SuburbDowntown of Accommodation
        /// </summary>
        public string SuburbDowntown { get; set; }
        
    }


    /// <summary>
    /// Accommodation Master RS
    /// </summary>
    public class AccommodationMasterRS
    {




        /// <summary>
        /// Contain TLGX Accommodation id of Accommodation
        /// </summary>
        //[BsonElement("TLGXAccoId")]
        public string HotelID { get; set; }



        /// <summary>
        /// Contain Accommodation Name
        /// </summary>
        public string HotelName { get; set; }


        /// <summary>
        /// Contain Type of Accommodation
        /// </summary>
        public string ProductCategory { get; set; }


        /// <summary>
        /// Contain ProductCategory Subtype of Accommodation
        /// </summary>
        public string HotelType { get; set; }

        /// <summary>
        /// Contain start rating of Accommodation
        /// </summary>
        public string HotelStarRating { get; set; }


        /// <summary>
        /// Contain Street info of Accommodation
        /// </summary>
        public string StreetName { get; set; }

        /// <summary>
        /// Contain Street Number of Accommodation
        /// </summary>
        public string StreetNumber { get; set; }

        /// <summary>
        /// Contain Street3 of Accommodation
        /// </summary>
        public string Street3 { get; set; }

        /// <summary>
        /// Contain Street4 of Accommodation
        /// </summary>
        public string Street4 { get; set; }

        /// <summary>
        /// Contain Street5 of Accommodation
        /// </summary>
        public string Street5 { get; set; }

        /// <summary>
        /// Contain Postal Code info of Accommodation
        /// </summary>
        public string PostalCode { get; set; }

        /// <summary>
        /// Contain Town info of Accommodation
        /// </summary>
        public string Town { get; set; }

        /// <summary>
        /// Contain Location of Accommodation
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Contain Area of Accommodation
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// Contain City Code
        /// </summary>
        public string CityCode { get; set; }

        /// <summary>
        /// Contain City Name
        /// </summary>
        public string CityName { get; set; }



        /// <summary>
        /// Contain State Code
        /// </summary>
        public string StateCode { get; set; }

        /// <summary>
        /// Contain State Name
        /// </summary>
        public string StateName { get; set; }


        /// <summary>
        /// Contain ISO Country Code
        /// </summary>
        public string CountryCode { get; set; }

        /// <summary>
        /// Contain Country Name
        /// </summary>
        public string CountryName { get; set; }


        /// <summary>
        /// Contain Full Address of Accommodation
        /// </summary>
        public string FullAddress { get; set; }

        ///// <summary>
        ///// Contain Room Mapping Completed info of Accommodation
        ///// </summary>
        //public bool IsRoomMappingCompleted { get; set; }

        /// <summary>
        /// Contain Telephone info of Accommodation
        /// </summary>
        public string TelephoneNumber { get; set; }

        /// <summary>
        /// Contain Contact Fax of Accommodation
        /// </summary>
        public string Fax { get; set; }

        /// <summary>
        /// Contain Contact WebSite of Accommodation
        /// </summary>
        public string URL { get; set; }

        /// <summary>
        /// Contain Contact Email of Accommodation
        /// </summary>
        public string EmailAddress { get; set; }

      

        /// <summary>
        /// Contain Latitude of Accommodation
        /// </summary>
        public string Latitude { get; set; }

        /// <summary>
        /// Contain Longitude of Accommodation
        /// </summary>
        public string Longitude { get; set; }

        /// <summary>
        /// Contain Status of Accommodation
        /// </summary>
        public string CodeStatus { get; set; }

    }

    //GAURAV_TMAP_889
    /// <summary>
    /// Accommodation Master RS
    /// </summary>
    public class AccommodationMasterGIATARS
    {
        

        /// <summary>
        /// Contain TLGX Accommodation id of Accommodation
        /// </summary>
        //[BsonElement("TLGXAccoId")]
        public string HotelID { get; set; }



        /// <summary>
        /// Contain Accommodation Name
        /// </summary>
        public string HotelName { get; set; }

        /// <summary>
        /// Contain ProductCategory Subtype of Accommodation
        /// </summary>
        public string HotelType { get; set; }

        /// <summary>
        /// Contain start rating of Accommodation
        /// </summary>
        public string HotelStarRating { get; set; }


        /// <summary>
        /// Contain Street info of Accommodation
        /// </summary>
        public string AddressStreet { get; set; }

        ///// <summary>
        ///// Contain Street Number of Accommodation
        ///// </summary>
        //public string StreetNumber { get; set; }

        /// <summary>
        /// Contain AddressSuburb of Accommodation
        /// </summary>
        public string AddressSuburb { get; set; }

        ///// <summary>
        ///// Contain Street4 of Accommodation
        ///// </summary>
        //public string Street4 { get; set; }

        ///// <summary>
        ///// Contain Street5 of Accommodation
        ///// </summary>
        //public string Street5 { get; set; }

        /// <summary>
        /// Contain Postal Code info of Accommodation
        /// </summary>
        public string PostalCode { get; set; }

      

        /// <summary>
        /// Contain City Name
        /// </summary>
        public string CityName { get; set; }




        /// <summary>
        /// Contain State Name
        /// </summary>
        public string StateName { get; set; }


        ///// <summary>
        ///// Contain ISO Country Code
        ///// </summary>
        //public string CountryCode { get; set; }

        ///// <summary>
        ///// Contain Country Name
        ///// </summary>
        //public string CountryName { get; set; }


        /// <summary>
        /// Contain Full Address of Accommodation
        /// </summary>
        public string GIATA_ID { get; set; }

        ///// <summary>
        ///// Contain Room Mapping Completed info of Accommodation
        ///// </summary>
        //public bool IsRoomMappingCompleted { get; set; }


        /// <summary>
        /// Contain Telephone info of Accommodation
        /// </summary>
        public string TelephoneNumber { get; set; }

        /// <summary>
        /// Contain Contact Fax of Accommodation
        /// </summary>
        public string FaxNumber { get; set; }

        /// <summary>
        /// Contain Contact WebSite of Accommodation
        /// </summary>
        public string URL { get; set; }

        /// <summary>
        /// Contain Contact Email of Accommodation
        /// </summary>
        public string EmailAddress { get; set; }



        /// <summary>
        /// Contain Latitude of Accommodation
        /// </summary>
        public string Latitude { get; set; }

        /// <summary>
        /// Contain Longitude of Accommodation
        /// </summary>
        public string Longitude { get; set; }

        /// <summary>
        /// Contain Status of Accommodation
        /// </summary>
        public string CodeStatus { get; set; }
    }
}