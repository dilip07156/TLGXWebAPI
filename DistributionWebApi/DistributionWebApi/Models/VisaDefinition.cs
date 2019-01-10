using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DistributionWebApi.Models.VisaMapping
{
    /// <summary>
    /// This is the request format for Country-based Visas Mapping Searches. It is a paged request/response service.
    /// </summary>
 

    [BsonIgnoreExtraElements]
    public class VisaInformation2
    {
        public object Information { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class VisaGeneralInformation
    {
        public string GeneralInfo { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class VisaInfo
    {
        public VisaInformation2 VisaInformation { get; set; }
        public VisaGeneralInformation VisaGeneralInformation { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class Information
    {
        public string ProcessingTime { get; set; }
        public string VisaProcedure { get; set; }
        public string DocumentsRequired { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class CategoryInfo
    {
        public Information Information { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class CategoryRequirements
    {
        public string Requirements { get; set; }
    }

    //[BsonIgnoreExtraElements]
    //public class CategoryNotes
    //{
    //    public string Notes { get; set; }
    //}

    [BsonIgnoreExtraElements]
    public class CategoryDetail
    {
        public string CategoryCode { get; set; }
        public CategoryInfo CategoryInfo { get; set; }
        public CategoryRequirements CategoryRequirements { get; set; }
        public string Category { get; set; }
        //public CategoryNotes CategoryNotes { get; set; }
        public BsonDocument CategoryNotes { get; set; }
    }

    //[BsonIgnoreExtraElements]
    //public class CategoryFee
    //{
    //    public string CategoryCode  { get; set; }
    //    public string Category { get; set; }
    //    public string CategoryFeeAmountINR { get; set; }
    //    public string CategoryFeeAmountOther { get; set; }
    //}

    //[BsonIgnoreExtraElements]
    //public class CategoryFees
    //{
    //    public List<CategoryFee> Category { get; set; }
    //}

    //[BsonIgnoreExtraElements]
    //public class Categories
    //{
    //    public List<CategoryDetail> Category { get; set; }
    //}

    //[BsonIgnoreExtraElements]
    //public class VisaInformation
    //{
    //    public string TerritoryCity { get; set; }
    //    public VisaInfo VisaInfo { get; set; }
    //    public Categories Categories { get; set; }
    //    public CategoryFees CategoryFees { get; set; }
    //    public string CategoryForms { get; set; }
    //}

    [BsonIgnoreExtraElements]
    public class Website
    {
    }

    [BsonIgnoreExtraElements]
    public class CountryOffice
    {
        public string CountryID { get; set; }
        public string VisaRequired { get; set; }
        public string WhereToApply { get; set; }
        public Website Website { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public string Name { get; set; }
        public string PinCode { get; set; }
        public string Address { get; set; }
        public string Telephone { get; set; }
        public string Fax { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class CountryOffices
    {
        public List<CountryOffice> CountryOffice { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class SAARCInfo
    {
        public CountryOffices CountryOffices { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class InformationLink
    {
        public string href { get; set; }
        public string content { get; set; }
        public string target { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class ReciprocalVisaInfo2
    {
        public InformationLink InformationLink { get; set; }
        public List<string> content { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class Description
    {
        public ReciprocalVisaInfo2 ReciprocalVisaInfo { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class ReciprocalVisaInfo
    {
        public Description Description { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class Description2
    {
        public string InternationalAdvisory { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class InternationalAdvisory
    {
        public Description2 Description { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class Description4
    {
    }

    [BsonIgnoreExtraElements]
    public class Heading
    {
        public Description4 Description { get; set; }
        public string content { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class Description3
    {
        public Heading Heading { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class IVSAdvisory
    {
        public Description3 Description { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class HelpAddress
    {
        public string Phone { get; set; }
        public string Country { get; set; }
        public string Website { get; set; }
        public string City { get; set; }
        public string Fax { get; set; }
        public string URL { get; set; }
        public string Name { get; set; }
        public string PinCode { get; set; }
        public string Address { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class IntlHelpAddress
    {
        public List<HelpAddress> HelpAddress { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class Office
    {
        public string Email { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Country { get; set; }
        public string SystemCountryCode { get; set; }
        public string SystemCountryName { get; set; }
        public string Website { get; set; }
        public string City { get; set; }
        public string SystemCityCode { get; set; }
        public string SystemCityName { get; set; }
        public string Fax { get; set; }
        public string URL { get; set; }
        public string Name { get; set; }
        public string PinCode { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class IndianEmbassy
    {
        //public Office Office { get; set; }
        public BsonDocument Office { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class Climate
    {
    }

    [BsonIgnoreExtraElements]
    public class GeneralInfo
    {
        //public Climate Climate { get; set; }
        public string SmallMap { get; set; }
        public string Languages { get; set; }
        public string Time { get; set; }
        public string Capital { get; set; }
        public string Flag { get; set; }
        public string Code { get; set; }
        public string Area { get; set; }
        public string Currency { get; set; }
        public string LargeMap { get; set; }
        public string Population { get; set; }
        public string WorldFactBook { get; set; }
        public string NationalDay { get; set; }
        public string Location { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class Month
    {
    }

    public class Date
    {
    }

    [BsonIgnoreExtraElements]
    public class Holiday
    {
        public Month Month { get; set; }
        public string HolidayName { get; set; }
        public string Year { get; set; }
        public Date Date { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class Holidays
    {
        public Holiday Holiday { get; set; }
    }

    //[BsonIgnoreExtraElements]
    //public class Airline
    //{
    //    public string Code { get; set; }
    //    public string Name { get; set; }
    //}

    [BsonIgnoreExtraElements]
    public class Airlines
    {
        // public List<Airline> Airline { get; set; }
        public BsonDocument Airline { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class CountryName
    {
        public string Name { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class Airport
    {
        public string Type { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class Airports
    {
        public List<Airport> Airport { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class CountryDetails
    {
        public GeneralInfo GeneralInfo { get; set; }
        public Holidays Holidays { get; set; }
        public Airlines Airlines { get; set; }
        public CountryName CountryName { get; set; }
        public Airports Airports { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class Office2
    {
        public string Timings { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Telephone { get; set; }
        public string VisaTimings { get; set; }
        public string Website { get; set; }
        public string City { get; set; }
        public string Name { get; set; }
        public string PinCode { get; set; }
        public string Phone { get; set; }
        public string CollectionTimings { get; set; }
        public string Country { get; set; }
        public string PublicTimings { get; set; }
        public string Fax { get; set; }
        public string Notes { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class Offices
    {
        public List<Office2> Office { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class DiplomaticRepresentation
    {
        // public Offices Offices { get; set; }
        public BsonDocument Offices { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class Visa
    {
        public string AdditionalInfo { get; set; }
        //public List<VisaInformation> VisaInformation { get; set; }
        public BsonArray VisaInformation { get; set; }
        public SAARCInfo SAARCInfo { get; set; }
        public ReciprocalVisaInfo ReciprocalVisaInfo { get; set; }
        public InternationalAdvisory InternationalAdvisory { get; set; }
        public IVSAdvisory IVSAdvisory { get; set; }

        //public IntlHelpAddress IntlHelpAddress { get; set; }
        public BsonDocument IntlHelpAddress { get; set; }

        public string CountryCode { get; set; }
        public IndianEmbassy IndianEmbassy { get; set; }
        public CountryDetails CountryDetails { get; set; }
        public DiplomaticRepresentation DiplomaticRepresentation { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class VisaDetail
    {
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public Visa Visa { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class VisaDefinition
    {
        [BsonId]
        [Newtonsoft.Json.JsonProperty("_id")]
        public object SystemVisaCode { get; set; }
        /// <summary>
        /// Mapping System Code for End Supplier. Full List of Codes can be retrieved from the Supplier Code API
        /// </summary>
        public string SupplierCode { get; set; }

        public string CallType { get; set; }
        /// <summary>
        /// End Supplier System Product Code. This code should be used in all communication with the End Supplier
        /// </summary>
        public string SupplierName { get; set; }

        public VisaDetail VisaDetail { get; set; }

    }
}