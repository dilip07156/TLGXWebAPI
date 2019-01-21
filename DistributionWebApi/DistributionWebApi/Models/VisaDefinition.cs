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
    public class VisaInformation
    {
        public string ProcessingTime { get; set; }
        public string VisaProcedure { get; set; }
        public string DocumentsRequired { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class VisaCategoryInfo
    {
        public VisaInformation Information { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class VisaCategoryRequirements
    {
        public string Requirements { get; set; }
    }

    //[BsonIgnoreExtraElements]
    //public class CategoryNotes
    //{
    //    public string Notes { get; set; }
    //}

    [BsonIgnoreExtraElements]
    public class VisaCategoryDetail
    {
        public string CategoryCode { get; set; }
        public VisaCategoryInfo CategoryInfo { get; set; }
        public VisaCategoryRequirements CategoryRequirements { get; set; }
        public string Category { get; set; }
        //public CategoryNotes CategoryNotes { get; set; }
        public BsonDocument CategoryNotes { get; set; }
    }

    //[BsonIgnoreExtraElements]
    //public class VisaCategoryFee
    //{
    //    public string CategoryCode  { get; set; }
    //    public string Category { get; set; }
    //    public string CategoryFeeAmountINR { get; set; }
    //    public string CategoryFeeAmountOther { get; set; }
    //}

    //[BsonIgnoreExtraElements]
    //public class VisaCategoryFees
    //{
    //    public List<VisaCategoryFee> Category { get; set; }
    //}

    //[BsonIgnoreExtraElements]
    //public class VisaCategories
    //{
    //    public List<VisaCategoryDetail> Category { get; set; }
    //}

    //[BsonIgnoreExtraElements]
    //public class VisaInformation
    //{
    //    public string TerritoryCity { get; set; }
    //    public VisaInfo VisaInfo { get; set; }
    //    public VisaCategories Categories { get; set; }
    //    public VisaCategoryFees CategoryFees { get; set; }
    //    public string CategoryForms { get; set; }
    //}

    [BsonIgnoreExtraElements]
    public class VisaWebsite
    {
    }

    [BsonIgnoreExtraElements]
    public class VisaCountryOffice
    {
        public string CountryID { get; set; }
        public string VisaRequired { get; set; }
        public string WhereToApply { get; set; }
        public VisaWebsite Website { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public string Name { get; set; }
        public string PinCode { get; set; }
        public string Address { get; set; }
        public string Telephone { get; set; }
        public string Fax { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class VisaCountryOffices
    {
        public List<VisaCountryOffice> CountryOffice { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class VisaSAARCInfo
    {
        public VisaCountryOffices CountryOffices { get; set; }
    }

    //[BsonIgnoreExtraElements]
    //public class VisaInformationLink
    //{
    //    public string href { get; set; }
    //    public string content { get; set; }
    //    public string target { get; set; }
    //}

    //[BsonIgnoreExtraElements]
    //public class ReciprocalVisaInfo2
    //{
    //    //public VisaInformationLink InformationLink { get; set; }
    //    public BsonDocument InformationLink { get; set; }
    //    public List<string> content { get; set; }
    //}

    [BsonIgnoreExtraElements]
    public class VisaDescription
    {
        //public ReciprocalVisaInfo2 ReciprocalVisaInfo { get; set; }
        public BsonDocument ReciprocalVisaInfo { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class ReciprocalVisaInfo
    {
        public VisaDescription Description { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class VisaDescription2
    {
        public string VisaInternationalAdvisory { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class VisaInternationalAdvisory
    {
        public VisaDescription2 Description { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class VisaDescription4
    {
    }

    [BsonIgnoreExtraElements]
    public class VisaHeading
    {
        public VisaDescription4 Description { get; set; }
        public string content { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class VisaDescription3
    {
        public VisaHeading Heading { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class VisaIVSAdvisory
    {
        public VisaDescription3 Description { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class VisaHelpAddress
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
    public class VisaIntlHelpAddress
    {
        public List<VisaHelpAddress> HelpAddress { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class VisaOffice
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

    //[BsonIgnoreExtraElements]
    //public class VisaIndianEmbassy
    //{
    //    public VisaOffice Office { get; set; }
    //    //public BsonDocument Office { get; set; }
    //}

    [BsonIgnoreExtraElements]
    public class VisaClimate
    {
    }

    //[BsonIgnoreExtraElements]
    //public class VisaGeneralInfo
    //{
    //    //public Climate Climate { get; set; }
    //    public string SmallMap { get; set; }
    //    public string Languages { get; set; }
    //    public string Time { get; set; }
    //    public string Capital { get; set; }
    //    public string Flag { get; set; }
    //    public string Code { get; set; }
    //    public string Area { get; set; }
    //    public string Currency { get; set; }
    //    public string LargeMap { get; set; }
    //    public string Population { get; set; }
    //    public string WorldFactBook { get; set; }
    //    public string NationalDay { get; set; }
    //    public string Location { get; set; }
    //}

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
        public VisaMonth Month { get; set; }
        public string HolidayName { get; set; }
        public string Year { get; set; }
        public VisaDate Date { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class VisaHolidays
    {
        public VisaHoliday Holiday { get; set; }
    }

    //[BsonIgnoreExtraElements]
    //public class Airline
    //{
    //    public string Code { get; set; }
    //    public string Name { get; set; }
    //}

    //[BsonIgnoreExtraElements]
    //public class VisaAirlines
    //{
    //    public List<VisaAirline> Airline { get; set; }
    //}

    [BsonIgnoreExtraElements]
    public class VisaCountryName
    {
        public string Name { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class VisaAirport
    {
        public string Type { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }

    //[BsonIgnoreExtraElements]
    //public class VisaAirports
    //{
    //    public List<VisaAirport> Airport { get; set; }
    //}

    [BsonIgnoreExtraElements]
    public class VisaCountryDetails
    {
        //public VisaGeneralInfo GeneralInfo { get; set; }
        public BsonDocument GeneralInfo { get; set; }
        public VisaHolidays Holidays { get; set; }
        public BsonDocument Airlines { get; set; }
        public VisaCountryName CountryName { get; set; }
        //public VisaAirports Airports { get; set; }
        public BsonDocument Airports { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class VisaOffice2
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
    public class VisaOffices
    {
        public List<VisaOffice2> Office { get; set; }
    }

    //[BsonIgnoreExtraElements]
    //public class VisaDiplomaticRepresentation
    //{
    //    // public Offices Offices { get; set; }
    //    public BsonDocument Offices { get; set; }
    //}

    //[BsonIgnoreExtraElements]
    //public class Visa
    //{
    //    public string AdditionalInfo { get; set; }
    //    //public List<VisaInformation> VisaInformation { get; set; }
    //    public BsonArray VisaInformation { get; set; }
    //    public VisaSAARCInfo SAARCInfo { get; set; }
    //    public ReciprocalVisaInfo ReciprocalVisaInfo { get; set; }
    //    public VisaInternationalAdvisory InternationalAdvisory { get; set; }
    //    public VisaIVSAdvisory IVSAdvisory { get; set; }

    //    //public IntlHelpAddress IntlHelpAddress { get; set; }
    //    public BsonDocument IntlHelpAddress { get; set; }

    //    public string CountryCode { get; set; }
    //    //public VisaIndianEmbassy IndianEmbassy { get; set; }
    //    public BsonDocument IndianEmbassy { get; set; }
    //    public VisaCountryDetails CountryDetails { get; set; }
    //    //public VisaDiplomaticRepresentation DiplomaticRepresentation { get; set; }
    //    public BsonDocument DiplomaticRepresentation { get; set; }
    //}

    [BsonIgnoreExtraElements]
    public class VisaDetail
    {
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        //public Visa Visa { get; set; }
        public BsonDocument Visa { get; set; }
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

        public string CallType { get; set; }
        /// <summary>
        /// End Supplier System Product Code. This code should be used in all communication with the End Supplier
        /// </summary>
        public string SupplierName { get; set; }

        public VisaDetail VisaDetail { get; set; }

    }
}