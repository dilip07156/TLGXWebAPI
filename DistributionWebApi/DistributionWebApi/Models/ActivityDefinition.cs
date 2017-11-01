﻿using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistributionWebApi.Models.Activity
{
    public class ActivitySearchByCountry_RQ
    {
        public int PageSize { get; set; }
        public int PageNo { get; set; }
        public string RequestingSupplierCode { get; set; }
        public string[] CountryCodes { get; set; }
    }

    public class ActivitySearchByCity_RQ
    {
        public int PageSize { get; set; }
        public int PageNo { get; set; }
        public string RequestingSupplierCode { get; set; }
        public string[] CityCodes { get; set; }
    }

    public class ActivitySearchByTypes_RQ
    {
        public int PageSize { get; set; }
        public int PageNo { get; set; }
        public string RequestingSupplierCode { get; set; }
        public string[] ActivityTypes { get; set; }
    }

    public class ActivityDefinition_PDP
    {

        //public string Activity_Flavour_Id { get; set; }

        [BsonId]
        [Newtonsoft.Json.JsonProperty("_id")]
        public int TLGXActivityCode { get; set; }
        public string SupplierCompanyCode { get; set; }
        public string SupplierProductCode { get; set; }
        public string Category { get; set; }
        public string Type { get; set; }
        public string SubType { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Session { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string DeparturePoint { get; set; }
        public string ReturnDetails { get; set; }
        public string DaysOfTheWeek { get; set; }
        public string PhysicalIntensity { get; set; }
        public string Overview { get; set; }
        public string Recommended { get; set; }
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
        public string CityName { get; set; }
        public string CityCode { get; set; }
        public string StarRating { get; set; }
        public string NumberOfPassengers { get; set; }
        public string NumberOfReviews { get; set; }
        public string NumberOfLikes { get; set; }
        public string NumberOfViews { get; set; }
        public string[] ActivityInterests { get; set; }
        public List<Inclusions> Inclusions { get; set; }
        public List<Exclusions> Exclusions { get; set; }
        public string[] Highlights { get; set; }
        public string[] TermsAndConditions { get; set; }
        public List<ImportantInfoAndBookingPolicies> BookingPolicies { get; set; }
        public List<Media> ActivityMedia { get; set; }
        public ActivityDuration Duration { get; set; }
        public List<ReviewScores> ReviewScores { get; set; }
        public List<CustomerReviews> CustomerReviews { get; set; }
        public ActivityLocation ActivityLocation { get; set; }
        public List<TourGuideLanguages> TourGuideLanguages { get; set; }
        public SupplierDetails SupplierDetails { get; set; }
        public List<SimliarProducts> SimliarProducts { get; set; }
        public List<ClassificationAttrributes> ClassificationAttrributes { get; set; }
        public List<Deals> Deals { get; set; }
        public List<Prices> Prices { get; set; }
        public SystemMapping SystemMapping { get; set; }
    }

    public class ActivityDefinition_SRP
    {
        public long TotalNumberOfActivities { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPage { get; set; }
        public string Message { get; set; }
        public List<Activities> Activities { get; set; }

    }

    public class Activities
    {
        public long TLGXActivityCode { get; set; }
        public string SupplierCompanyCode { get; set; }
        public string SupplierProductCode { get; set; }
        public string Category { get; set; }
        public string Type { get; set; }
        public string SubType { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Session { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string DeparturePoint { get; set; }
        public string ReturnDetails { get; set; }
        public string DaysOfTheWeek { get; set; }
        public string PhysicalIntensity { get; set; }
        public string Overview { get; set; }
        public string Recommended { get; set; }
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
        public string CityName { get; set; }
        public string CityCode { get; set; }
        public string StarRating { get; set; }
        public string NumberOfReviews { get; set; }
        public string NumberOfLikes { get; set; }
        public string NumberOfViews { get; set; }
        public List<Media> ActivityMedia { get; set; }
        public ActivityDuration Duration { get; set; }
        public List<SimliarProducts> SimliarProducts { get; set; }
    }

    public class Inclusions
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class Exclusions
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class ImportantInfoAndBookingPolicies
    {
        public string InfoType { get; set; }
        public string InfoText { get; set; }
    }

    public class Media
    {
        public string MediaType { get; set; }
        public string MediaSubType { get; set; }
        public string FullUrl { get; set; }
        public string ThumbUrl { get; set; }
        public string SortOrder { get; set; }
        public string Description { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Caption { get; set; }

    }

    public class ActivityDuration
    {
        public string Text { get; set; }
        public string Hours { get; set; }
        public string Minutes { get; set; }
    }

    public class ReviewScores
    {
        public string Source { get; set; }
        public string Type { get; set; }
        public decimal? Score { get; set; }
    }

    public class CustomerReviews
    {
        public string Source { get; set; }
        public string Type { get; set; }
        public decimal? Score { get; set; }
        public string Author { get; set; }
        public string Date { get; set; }
        public string Title { get; set; }
        public string Comment { get; set; }
    }

    public class ActivityLocation
    {
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string Address { get; set; }
        public string Location { get; set; }
        public string Area { get; set; }

    }

    public class TourGuideLanguages
    {
        public string Language { get; set; }
        public string LanguageID { get; set; }
    }

    public class SupplierDetails
    {
        public string RequestorID { get; set; }
        public string PricingCurrency { get; set; }
        public string PrimaryLanguageID { get; set; }
        public string SupplierBrandCode { get; set; }
        public string SupplierName { get; set; }
        public string SupplierID { get; set; }
        public string TourActivityID { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string CityCode { get; set; }
        public string CityName { get; set; }
        public string StartPeriod { get; set; }
        public string EndPeriod { get; set; }
        public int? Quantity { get; set; }
        public int? Age { get; set; }
        public string QualifierInfo { get; set; }
        public string TimeSlotCode { get; set; }

    }

    public class SimliarProducts
    {
        public string TLGXActivityOptionCode { get; set; }
        public string OptionCode { get; set; }
        public string DealText { get; set; }
        public string Options { get; set; }
        public string ActivityType { get; set; }
        public string TotalNetPrice { get; set; }

    }

    public class ClassificationAttrributes
    {
        public string Group { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
    }

    public class Deals
    {
        public decimal? DealPrice { get; set; }
        public string Currency { get; set; }
        public string DealText { get; set; }
        public string OfferTermsAndConditions { get; set; }
        public string DealId { get; set; }
    }

    public class Prices
    {
        public string SupplierCurrency { get; set; }
        public decimal? Price { get; set; }
        public string PriceType { get; set; }
        public string PriceBasis { get; set; }
        public string PriceId { get; set; }
    }

    public class SystemMapping
    {
        public string SystemName { get; set; }
        public string SystemID { get; set; }
    }
}