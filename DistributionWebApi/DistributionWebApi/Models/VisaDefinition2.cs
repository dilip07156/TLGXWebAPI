using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
namespace DistributionWebApi.Models
{
    /// <summary>
    /// This is the request format for Country-based Visas Mapping Searches. It is a paged request/response service.
    /// </summary>

    public class InformationLink
    {
        /// <summary>
        /// contain href information
        /// </summary>
        public string href { get; set; }

        /// <summary>
        /// contain content information
        /// </summary>
        public string content { get; set; }

        /// <summary>
        /// contain target information
        /// </summary>
        public string target { get; set; }
    }

    public class Information
    {

        /// <summary>
        /// contain list of information links node having information like content,target
        /// </summary>
        public List<InformationLink> InformationLink { get; set; }

        /// <summary>
        /// Hold List of content
        /// </summary>
        public List<string> content { get; set; }
    }



    [BsonIgnoreExtraElements]
    public class VisaInformationNode
    {
        /// <summary>
        /// hold list of information nodes
        /// </summary>
        public Information Information { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class VisaGeneralInformation
    {
        /// <summary>
        /// Contain visa general information
        /// </summary>
        public string GeneralInfo { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class VisaInfo
    {
        /// <summary>
        /// hold list of visa inforrmation 
        /// </summary>
        public List<VisaInformationNode> VisaInformation { get; set; }

        /// <summary>
        /// hold list of visa General inforrmation 
        /// </summary>
        public List<VisaGeneralInformation> VisaGeneralInformation { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class VisaInformationChildNode
    {
        /// <summary>
        /// Contain visa processing time 
        /// </summary>
        public string ProcessingTime { get; set; }

        /// <summary>
        /// Contain visa Procedure information 
        /// </summary>
        public string VisaProcedure { get; set; }

        /// <summary>
        /// Contain Documents required to get visa 
        /// </summary>
        public string DocumentsRequired { get; set; }

        /// <summary>
        /// Contain content Information  
        /// </summary>
        public string content { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class VisaCategoryInfo
    {
        /// <summary>
        /// Contain list of information like processing time and visa procedure  
        /// </summary>
        public List<VisaInformationChildNode> Information { get; set; }
    }

    public class Requirements
    {
        /// <summary>
        /// Contain Information about Requirements  
        /// </summary>
        public string Line { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class VisaCategoryRequirements
    {
        /// <summary>
        /// Contain Requirements node  
        /// </summary>
        public Requirements Requirements { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class CategoryNotes
    {
        /// <summary>
        /// hold list of notes of category requirement node  
        /// </summary>
        public List<string> Notes { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class VisaCategoryDetail
    {
        /// <summary>
        /// Contain Category code  
        /// </summary>
        public string CategoryCode { get; set; }

        /// <summary>
        /// Holds list of category information nodes
        /// </summary>
        public List<VisaCategoryInfo> CategoryInfo { get; set; }

        /// <summary>
        /// Holds list of category requirements nodes
        /// </summary>
        public List<VisaCategoryRequirements> CategoryRequirements { get; set; }

        /// <summary>
        /// Contain the type of Visa category
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Contain the category notes of Visa 
        /// </summary>
        public CategoryNotes CategoryNotes { get; set; }
        //public BsonDocument CategoryNotes { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class VisaCategoryFee
    {
        /// <summary>
        /// Contain Category Code
        /// </summary>
        public string CategoryCode { get; set; }

        /// <summary>
        /// Contain Category name
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Contain Category fee in  Indian Currency
        /// </summary>
        public string CategoryFeeAmountINR { get; set; }

        /// <summary>
        /// Contain Category fee in  Other Currency
        /// </summary>
        public string CategoryFeeAmountOther { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class VisaCategoryFees
    {
        /// <summary>
        /// Contain list of visa category fee
        /// </summary>
        public List<List<VisaCategoryFee>> Category { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class VisaCategories
    {
        /// <summary>
        /// Contain list of visa category 
        /// </summary>
        public List<VisaCategoryDetail> Category { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class VisaInformation
    {
        /// <summary>
        /// Contain Terror city name
        /// </summary>
        public string TerritoryCity { get; set; }

        /// <summary>
        /// Contain List of Visa Info Nodes
        /// </summary>
        public List<VisaInfo> VisaInfo { get; set; }

        /// <summary>
        /// Contain List of Visa Categories Nodes
        /// </summary>
        public List<VisaCategories> Categories { get; set; }
        
        /// <summary>
        /// Contain List of Visa Category Fees Nodes
        /// </summary>
        public List<VisaCategoryFees> CategoryFees { get; set; }

        /// <summary>
        /// Contain Category Form Node
        /// </summary>
        public CategoryForms CategoryForms { get; set; }
    }


    public class CategoryForm
    {
        /// <summary>
        /// Contain List of Category Codes
        /// </summary>
        public List<string> CategoryCode { get; set; }

        /// <summary>
        /// Contain List of Form Nodes
        /// </summary>
        public List<string> Form { get; set; }

        /// <summary>
        /// Contain List of Form Path Nodes
        /// </summary>
        public List<string> FormPath { get; set; }
    }

    public class CategoryForms
    {
        /// <summary>
        /// Contain List of Visa Category Form Nodes
        /// </summary>
        public List<CategoryForm> CategoryForm { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class VisaWebsite
    {
    }

    [BsonIgnoreExtraElements]
    public class VisaCountryOffice
    {
        /// <summary>
        /// Contain Code of country
        /// </summary>
        public string CountryID { get; set; }

        /// <summary>
        /// Contain visa required Yes or No
        /// </summary>
        public string VisaRequired { get; set; }
       
        /// <summary>
        /// Contain the location where to apply for visa
        /// </summary>
        public string WhereToApply { get; set; }

        /// <summary>
        /// Contain the web site information
        /// </summary>
        public string Website { get; set; }

        /// <summary>
        /// Contain the city name 
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Contain the Country name 
        /// </summary>
        public string County { get; set; }

        /// <summary>
        /// Contain the Name of country office 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Contain the Pin code of country office 
        /// </summary>
        public string PinCode { get; set; }

        /// <summary>
        /// Contain the Address of country office 
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Contain the telephone of country office 
        /// </summary>
        public string Telephone { get; set; }

        /// <summary>
        /// Contain the fax number of country office 
        /// </summary>
        public string Fax { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class VisaCountryOffices
    {
        /// <summary>
        /// holds the list of country office to get visa
        /// </summary>
        public List<VisaCountryOffice> CountryOffice { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class VisaSAARCInfo
    {
        /// <summary>
        /// holds the list of country office to get visa
        /// </summary>
        public List<VisaCountryOffices> CountryOffices { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class VisaInformationLink
    {
        /// <summary>
        /// contain the href of a link
        /// </summary>
        public string href { get; set; }

        // <summary>
        /// contain content information a link
        /// </summary>
        public List<string> content { get; set; }

        // <summary>
        /// contain target information a link
        /// </summary>
        public string target { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class ReciprocalVisaInfoChildNode
    {
        /// <summary>
        /// hold the list of information links having information like content and target
        /// </summary>
        public List<VisaInformationLink> InformationLink { get; set; }
        //public BsonDocument InformationLink { get; set; }

        /// <summary>
        /// hold the list of Content information
        /// </summary>
        public List<string> content { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class VisaDescription
    {
        /// <summary>
        /// hold the list of reciprocal visa info nodes having information like content and target
        /// </summary>
        public List<ReciprocalVisaInfoChildNode> ReciprocalVisaInfo { get; set; }
        //public BsonDocument ReciprocalVisaInfo { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class ReciprocalVisaInfo
    {
        /// <summary>
        /// hold the list of description nodes having information like content and target
        /// </summary>
        public List<VisaDescription> Description { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class VisaDescriptionInnerNode
    {
        /// <summary>
        /// contain the information of visa international advisory 
        /// </summary>
        public string VisaInternationalAdvisory { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class VisaInternationalAdvisory
    {
        /// <summary>
        /// hold the list of visa international advisory information
        /// </summary>
        public List<VisaDescriptionInnerNode> Description { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class VisaDescriptionSubNode
    {
    }

    [BsonIgnoreExtraElements]
    public class VisaHeading
    {
        /// <summary>
        /// hold the list of description information
        /// </summary>
        public List<VisaDescriptionSubNode> Description { get; set; }

        /// <summary>
        /// Contains the content information
        /// </summary>
        public string content { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class VisaDescriptionNode
    {
        /// <summary>
        /// hold the list of Visa heading information like content, description
        /// </summary>
        public List<VisaHeading> Heading { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class VisaIVSAdvisory
    {
        /// <summary>
        /// hold the list of Visa IVS advisory information like description, content
        /// </summary>
        public List<VisaDescriptionNode> Description { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class VisaHelpAddress
    {
        /// <summary>
        /// Contain phone number of help address
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Contain Country name of help address
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Contain name of web site of help address
        /// </summary>
        public string Website { get; set; }

        /// <summary>
        /// Contain city name of help address
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Contain fax name of help address
        /// </summary>
        public string Fax { get; set; }

        /// <summary>
        /// Contain fax name of help address
        /// </summary>
        public string URL { get; set; }

        /// <summary>
        /// Contain URL of help address
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Contain pincode of help address
        /// </summary>
        public string PinCode { get; set; }

        /// <summary>
        /// Contain Address of help address
        /// </summary>
        public string Address { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class VisaIntlHelpAddress
    {
        /// <summary>
        /// hold list of help address to get visa
        /// </summary>
        public List<VisaHelpAddress> HelpAddress { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class VisaOffice
    {
        /// <summary>
        /// Contain Email Id of a visa office
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Contain Address of a visa office
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Contain phone number of a visa office
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Contain country name of a visa office
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Contain system country code of a visa office
        /// </summary>
        public string SystemCountryCode { get; set; }

        /// <summary>
        /// Contain system country name of a visa office
        /// </summary>
        public string SystemCountryName { get; set; }

        /// <summary>
        /// Contain Web site name of a visa office
        /// </summary>
        public string Website { get; set; }

        /// <summary>
        /// Contain Web city name of a visa office
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Contain system city code of a visa office
        /// </summary>
        public string SystemCityCode { get; set; }

        /// <summary>
        /// Contain system city name of a visa office
        /// </summary>
        public string SystemCityName { get; set; }

        /// <summary>
        /// Contain fax number of a visa office
        /// </summary>
        public string Fax { get; set; }

        /// <summary>
        /// Contain URL of a visa office
        /// </summary>
        public string URL { get; set; }

        /// <summary>
        /// Contain name of a visa office
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Contain pin code of a visa office
        /// </summary>
        public string PinCode { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class VisaIndianEmbassy
    {
        /// <summary>
        /// hold list of visa office information like email, phone
        /// </summary>
        public List<VisaOffice> Office { get; set; }
        //public BsonDocument Office { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class VisaClimate
    {
    }

    [BsonIgnoreExtraElements]
    public class VisaGeneralInfo
    {
        //public Climate Climate { get; set; }

        /// <summary>
        /// Contain path of image of small map
        /// </summary>
        public string SmallMap { get; set; }

        /// <summary>
        /// Contain Languages spoken in country
        /// </summary>
        public string Languages { get; set; }

        /// <summary>
        /// Contain time zone of country  
        /// </summary>
        public string Time { get; set; }

        /// <summary>
        /// Contain capital of country
        /// </summary>
        public string Capital { get; set; }

        /// <summary>
        /// Contain path of image of flag for a country
        /// </summary>
        public string Flag { get; set; }

        /// <summary>
        /// Contain Code of country
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// describes area of country
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// Contain currency used within country
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// Contain path of image of large map
        /// </summary>
        public string LargeMap { get; set; }

        /// <summary>
        /// Contain approximate population of a country
        /// </summary>
        public string Population { get; set; }

        /// <summary>
        /// Contain URL of a country Fact book
        /// </summary>
        public string WorldFactBook { get; set; }

        /// <summary>
        /// Contain national day of a country
        /// </summary>
        public string NationalDay { get; set; }

        /// <summary>
        /// Contain location of a country
        /// </summary>
        public string Location { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class VisaMonth
    {
    }

    public class VisaDate
    {
    }

    [BsonIgnoreExtraElements]
    public class VisaHoliday
    {
        /// <summary>
        /// Contain name of a month for particular holiday
        /// </summary>
        public string Month { get; set; }

        /// <summary>
        /// Contain name of a holiday
        /// </summary>
        public string HolidayName { get; set; }

        /// <summary>
        /// Contain year of a holiday
        /// </summary>
        public string Year { get; set; }

        /// <summary>
        /// Contain date of a holiday
        /// </summary>
        public string Date { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class VisaHolidays
    {
        /// <summary>
        /// list of holiday information like month, date and year
        /// </summary>
        public List<VisaHoliday> Holiday { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class VisaAirline
    {
        /// <summary>
        /// list of code of a airline
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// list of name of a airline
        /// </summary>
        public string Name { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class VisaAirlines
    {
        /// <summary>
        /// list of air line information like code and name
        /// </summary>
        public List<VisaAirline> Airline { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class VisaCountryName
    {
        /// <summary>
        /// contains the name of a country
        /// </summary>
        public string Name { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class VisaAirport
    {
        /// <summary>
        /// contains the type of a airport
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// contains the code of a airport
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// contains the name of a airport
        /// </summary>
        public string Name { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class VisaAirports
    {
        /// <summary>
        /// hold a list of airports information like type and code
        /// </summary>
        public List<VisaAirport> Airport { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class VisaCountryDetails
    {
        /// <summary>
        /// Hold list of general information like location, language
        /// </summary>
        public List<VisaGeneralInfo> GeneralInfo { get; set; }
        //public BsonDocument GeneralInfo { get; set; }

        /// <summary>
        /// Hold list of holiday information
        /// </summary>
        public VisaHolidays Holidays { get; set; }
        // public BsonDocument Airlines { get; set; }

        /// <summary>
        /// Hold list of visa Air line information
        /// </summary>
        public List<VisaAirlines> Airlines { get; set; }
      
        /// <summary>
        /// Hold list of country name
        /// </summary>
        public List<VisaCountryName> CountryName { get; set; }

        /// <summary>
        /// Hold list of visa Air port information
        /// </summary>
        public List<VisaAirports> Airports { get; set; }
        //public BsonDocument Airports { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class VisaOfficeNode
    {
        /// <summary>
        /// Contain working timing of visa office
        /// </summary>
        public string Timings { get; set; }

        /// <summary>
        /// Contain Email id of visa office
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Contain Address of visa office
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Contain telephone number of visa office
        /// </summary>
        public string Telephone { get; set; }

        /// <summary>
        /// Contain Visa timings of visa office
        /// </summary>
        public string VisaTimings { get; set; }

        /// <summary>
        /// Contain website name of visa office
        /// </summary>
        public string Website { get; set; }

        /// <summary>
        /// Contain city name of visa office
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Contain city name of visa office
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Contain pin code of visa office
        /// </summary>
        public string PinCode { get; set; }

        /// <summary>
        /// Contain phone number of visa office
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Contain Collection timings of visa office
        /// </summary>
        public string CollectionTimings { get; set; }

        /// <summary>
        /// Contain country name of visa office
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Contain public timings of visa office
        /// </summary>
        public string PublicTimings { get; set; }

        /// <summary>
        /// Contain fax name of visa office
        /// </summary>
        public string Fax { get; set; }

        /// <summary>
        /// Contain notes if any of visa office
        /// </summary>
        public string Notes { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class VisaOffices
    {
        /// <summary>
        /// Holds the List of Visa Information offices available
        /// </summary>
        public List<VisaOfficeNode> Office { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class VisaDiplomaticRepresentation
    {
        /// <summary>
        /// Holds the List of Visa Information offices available
        /// </summary>
        public List<VisaOffices> Offices { get; set; }
        //public BsonDocument Offices { get; set; }
    }

    //[BsonIgnoreExtraElements]
    public class Visa
    {
        /// <summary>
        /// Contain Additional Info
        /// </summary>
        public string AdditionalInfo { get; set; }

        /// <summary>
        /// Holds the List of Visa Information Nodes
        /// </summary>
        public List<VisaInformation> VisaInformation { get; set; }
        //public BsonArray VisaInformation { get; set; }

        /// <summary>
        /// Holds the List of Saarc Info Nodes
        /// </summary>
        public List<VisaSAARCInfo> SAARCInfo { get; set; }

        /// <summary>
        /// Holds the List of Reciprocal Visa Info Nodes
        /// </summary>
        public List<ReciprocalVisaInfo> ReciprocalVisaInfo { get; set; }

        /// <summary>
        /// Holds the List of International Advisory Nodes
        /// </summary>
        public List<VisaInternationalAdvisory> InternationalAdvisory { get; set; }

        /// <summary>
        /// Holds the List of IVS Advisory Nodes
        /// </summary>
        public List<VisaIVSAdvisory> IVSAdvisory { get; set; }

        /// <summary>
        /// Holds the International help address
        /// </summary>
        public VisaIntlHelpAddress IntlHelpAddress { get; set; }
        //public BsonDocument IntlHelpAddress { get; set; }

        /// <summary>
        /// Contain Country code
        /// </summary>
        public string CountryCode { get; set; }

        /// <summary>
        /// Holds the List of Indian Embassy
        /// </summary>
        public List<VisaIndianEmbassy> IndianEmbassy { get; set; }
        //public BsonDocument IndianEmbassy { get; set; }

        /// <summary>
        /// Holds the List of Country details nodes
        /// </summary>
        public List<VisaCountryDetails> CountryDetails { get; set; }

        /// <summary>
        /// Holds the List of Diplomatic Representation nodes
        /// </summary>
        public List<VisaDiplomaticRepresentation> DiplomaticRepresentation { get; set; }
        //public BsonDocument DiplomaticRepresentation { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class VisaDetail
    {
        /// <summary>
        /// Contain Country code
        /// </summary>
        public string CountryCode { get; set; }

        /// <summary>
        /// Contain Country Name
        /// </summary>
        public string CountryName { get; set; }

        /// <summary>
        /// Holds list of Visa Nodes
        /// </summary>
        public List<Visa> Visa { get; set; }
        //public BsonDocument Visa { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class VisaDefinition
    {
        //[BsonId]
        //[Newtonsoft.Json.JsonProperty("_id")]
        //public object SystemVisaCode { get; set; }

        /// <summary>
        /// Mapping System Code for End Supplier. Full List of Codes can be retrieved from the Supplier Code API
        /// </summary>
        public string SupplierCode { get; set; }

        /// <summary>
        /// Holds the Call type
        /// </summary>
        public string CallType { get; set; }

        /// <summary>
        /// End Supplier System Product Code. This code should be used in all communication with the End Supplier
        /// </summary>
        public string SupplierName { get; set; }

        /// <summary>
        /// holds a list of Visa Details
        /// </summary>
        public List<VisaDetail> VisaDetail { get; set; }

    }
}