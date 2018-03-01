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

        /// <summary>
        /// Internal Product star rating
        /// </summary>
        public string CompanyRating { get; set; }

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

        //public string ShortDescription { get; set; }
        //public string LongDescription { get; set; }
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

        /// <summary>
        /// Address detailing
        /// </summary>
        //[JsonProperty(Required = Required.Always)]
        public string Country { get; set; }

        /// <summary>
        /// Address detailing
        /// </summary>
        //[JsonProperty(Required = Required.Always)]
        public string State { get; set; }

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
        /// <summary>
        /// facility category
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// the type of facility
        /// </summary>
        public string Type { get; set; }

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
        public int MaxAdults { get; set; }
        public int MaxChild { get; set; }
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
}