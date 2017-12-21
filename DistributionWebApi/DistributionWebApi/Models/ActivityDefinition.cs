using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DistributionWebApi.Models.Activity
{
    /// <summary>
    /// This is the request format for Country-based Activity Mapping Searches. It is a paged request/response service.
    /// </summary>
    public class ActivitySearchByCountry_RQ
    {
        /// <summary>
        /// How many Search Results do you wish to receive per request?
        /// </summary>
        
        [Required]
        public int PageSize { get; set; }
        /// <summary>
        /// Which Page Number you wish to retrieve from the Search Results set
        /// </summary>
        [Required]
        public int PageNo { get; set; }
        /// <summary>
        /// Your System Identification Code as given by Mapping Team. (Don't send this if you are using system country codes)
        /// </summary>
        public string RequestingSupplierCode { get; set; }
        /// <summary>
        /// A collection of Country Codes to serarch for. The values should be your system Country Codes.
        /// </summary>
        public string[] CountryCodes { get; set; }
    }

    /// <summary>
    ///This is the request format for City-based Activity Mapping Searches. It is a paged request/response service.
    /// </summary>
    public class ActivitySearchByCity_RQ
    {
        /// <summary>
        /// How many Search Results do you wish to receive per request?
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// Which Page Number you wish to retrieve from the Search Results set
        /// </summary>
        public int PageNo { get; set; }
        /// <summary>
        /// Your System Identification Code as given by Mapping Team (Don't send this if you are using system city codes)
        /// </summary>
        public string RequestingSupplierCode { get; set; }
        /// <summary>
        /// A collection of City Codes to serarch for. The values should be your system Country Codes.
        /// </summary>
        public string[] CityCodes { get; set; }
    }

    /// <summary>
    /// This Service allows the Activity Static Data to be queried based on the Type of Activity. System Masters can be retrieved from 
    /// </summary>
    public class ActivitySearchByTypes_RQ
    {
        /// <summary>
        /// How many Search Results do you wish to receive per request?
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// Which Page Number you wish to retrieve from the Search Results set
        /// </summary>
        public int PageNo { get; set; }
        /// <summary>
        /// Your System Identification Code as given by Mapping Team
        /// </summary>
        public string RequestingSupplierCode { get; set; }
        /// <summary>
        /// A collection of Activity Typesto serarch for. The values should be our Activity Codes, which can be retrieved from the Service GetActivityClassificationStructure.
        /// </summary>
        public string[] ActivityTypes { get; set; }
    }

    /// <summary>
    /// This response gives the Full Static Data definition for the Activity. It is a standardised format containing the static data of the Individual Supplier.
    /// </summary>
    public class ActivityDefinition
    {

        //public string Activity_Flavour_Id { get; set; }
        /// <summary>
        /// Mapping System Internal Code for Activity
        /// </summary>
        [BsonId]
        [Newtonsoft.Json.JsonProperty("_id")]
        public int SystemActivityCode { get; set; }
        /// <summary>
        /// Mapping System Code for End Supplier. Full List of Codes can be retrieved from the Supplier Code API
        /// </summary>
        public string SupplierCompanyCode { get; set; }
        /// <summary>
        /// End Supplier System Product Code. This code should be used in all communication with the End Supplier
        /// </summary>
        public string SupplierProductCode { get; set; }
        /// <summary>
        /// Mapping System Activity Category for Product Classification. THis is the highest level of classification.
        /// </summary>
        public string Category { get; set; }
        /// <summary>
        /// Mapping System Activity Product Type Category for Product Classification. This is the second level of classification
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// Mapping System Activity Category for Product Classification. THis is the third level of classification.
        /// </summary>
        public string SubType { get; set; }
        /// <summary>
        /// The Name of the Activity Product as per the End Supplier
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The Main Description of the Activity from the End Supplier.
        /// </summary>
        public string Description { get; set; }
        
        ///// <summary>
        ///// Mapping System Activity Category for Product Session. 
        ///// THis classification is designed to group products into time-bands. 
        ///// It may not be defined automatically due to inconsistencies in Supplier Static Data. Master Values can be retrieved from GetActivityClassificationStructure.
        ///// </summary>
        //public Session Session { get; set; }
        ///// <summary>
        ///// The Start Time of the Activity. This may not be defined due to variance in Supplier Static Data.
        ///// </summary>
        //public string StartTime { get; set; }
        ///// <summary>
        ///// The End Time of the Activity. This may not be defined due to variance in Supplier Static Data.
        ///// </summary>
        //public string EndTime { get; set; }


        /// <summary>
        /// Where does the Activity Depart from? This may not be defined due to variance in Supplier Static Data.
        /// </summary>
        public string DeparturePoint { get; set; }
        /// <summary>
        /// When the Activity is complete, where does it Return To? This may not be defined due to variance in Supplier Static Data.
        /// </summary>
        public string ReturnDetails { get; set; }
        /// <summary>
        /// What days of the week does this Activity Operate? Values should be in MTWTFSS format.
        /// </summary>
        public List<DaysOfWeek> DaysOfTheWeek { get; set; }

        /// <summary>
        /// The Physical Intensity Level of the Activity. Most End Suppliers do not carry a specific value for this attribute so it is inferred from Product Classification. Master Values can be retrieved from GetActivityClassificationStructure.
        /// </summary>
        public string PhysicalIntensity { get; set; }
        /// <summary>
        /// This field specifies for whom this activity is suitable.
        /// </summary>
        public string SuitableFor { get; set; }
        /// <summary>
        /// This is a Longer description of the Activity.
        /// </summary>
        public string Overview { get; set; }
        /// <summary>
        /// Is this Activity Recommended by either the Supplier or Mapping System
        /// </summary>
        public string Recommended { get; set; }
        /// <summary>
        /// This is the Mapping System Country Name. All Supplier values have been converted to this to allow simple cross supplier searches. Actual Supplier values is contained in the element SupplierDetails.
        /// </summary>
        public string CountryName { get; set; }
        /// <summary>
        /// This is the Mapping System Country Code. All Supplier values have been converted to this to allow simple cross supplier searches. Actual Supplier values is contained in the element SupplierDetails.
        /// </summary>
        public string CountryCode { get; set; }
        /// <summary>
        /// This is the Mapping System City Name. All Supplier values have been converted to this to allow simple cross supplier searches. Actual Supplier values is contained in the element SupplierDetails.
        /// </summary>
        public string CityName { get; set; }
        /// <summary>
        /// This is the Mapping System City Code. All Supplier values have been converted to this to allow simple cross supplier searches. Actual Supplier values is contained in the element SupplierDetails.
        /// </summary>
        public string CityCode { get; set; }
        /// <summary>
        /// What Star Rating does this Activity have? 
        /// </summary>
        public string StarRating { get; set; }
        /// <summary>
        /// What is the maximum number of passengers supported by this Activity? 
        /// </summary>
        public string NumberOfPassengers { get; set; }
        /// <summary>
        /// How many reviews does this Activity have? This value may be populated by Supplier Data, where it is provided.
        /// </summary>
        public string NumberOfReviews { get; set; }
        /// <summary>
        /// How many Likes does this Activity have? This value may be populated by Supplier Data, where it is provided.
        /// </summary>
        public string NumberOfLikes { get; set; }
        /// <summary>
        /// HOw many views does this Activity have? This value may be populated by Supplier Data, where it is provided.
        /// </summary>
        public string NumberOfViews { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string[] ActivityInterests { get; set; }

        /// <summary>
        /// A list of things included in this Activity. This is End Supplier Data so the qulaity of this may vary. It may also contain inline HTML.
        /// </summary>
        public List<Inclusions> Inclusions { get; set; }
        /// <summary>
        /// A list of this not included in this Activity. This is End Supplier Data so the qulaity of this may vary. It may also contain inline HTML.
        /// </summary>
        public List<Exclusions> Exclusions { get; set; }
        /// <summary>
        /// A list of the Highlights in this Activity. This is End Supplier Data so the qulaity of this may vary. It may also contain inline HTML.
        /// </summary>
        public string[] Highlights { get; set; }
        /// <summary>
        /// A list of Terms and Conditions in this Activity. This is End Supplier Data so the qulaity of this may vary. It may also contain inline HTML.
        /// </summary>
        public string[] TermsAndConditions { get; set; }
        /// <summary>
        /// Contains additional information for this Activity that are not formal terms and conditions, but should be displayed to the End User. 
        /// </summary>
        public List<ImportantInfoAndBookingPolicies> BookingPolicies { get; set; }
        /// <summary>
        /// A list of pictures or videos available for this Activity. The item will be a remote URL reference to picture / video.  This is End Supplier Data so the qulaity of this may vary. It may also contain inline HTML.
        /// </summary>
        public List<Media> ActivityMedia { get; set; }
        
        ///// <summary>
        ///// The duration of the activity. This is End Supplier Data so the qulaity of this may vary. It may also contain inline HTML
        ///// </summary>
        //public ActivityDuration Duration { get; set; }

        /// <summary>
        /// A list containing a type of review and the number of reviews. For example, it may contain NUmber of 5,4,3,2,1 Star Reviews.
        /// </summary>
        public List<ReviewScores> ReviewScores { get; set; }
        /// <summary>
        /// A List of Actual Customer reviews for the Activity. This is End Supplier Data so the qulaity of this may vary. It may also contain inline HTML
        /// </summary>
        public List<CustomerReviews> CustomerReviews { get; set; }
        /// <summary>
        /// Contains the Location Data for the Activity including Address, Lat/Lon etc
        /// </summary>
        public ActivityLocation ActivityLocation { get; set; }
        /// <summary>
        /// A list of Tour Guide Langauges offered by this Activity. This attribute may not be defined for all Suppliers
        /// </summary>
        public List<TourGuideLanguages> TourGuideLanguages { get; set; }
        /// <summary>
        /// Contains Original Supplier Values for Key PRoduct Attributes
        /// </summary>
        public SupplierDetails SupplierDetails { get; set; }
        /// <summary>
        /// A list of "options" for an Activity. This data may need to be used when making specific Booking requests with End Suppliers. Not all Suppliers provide this information as part of their static data.
        /// </summary>
        public List<SimliarProducts> SimliarProducts { get; set; }
        /// <summary>
        /// A List of Additional Classification Attributes for the Product
        /// </summary>
        public List<ClassificationAttrributes> ClassificationAttrributes { get; set; }
        /// <summary>
        /// A List of Deals offered by the SUpplier for this Activity. Not all Suppliers provide this data
        /// </summary>
        public List<Deals> Deals { get; set; }
        /// <summary>
        /// A list of Prices for the Activity. The data here is retrieved for the product is contained ONLY in the static data and it is advised that a formal Pricing / Availability request is made to retrieve the actual price before booking.
        /// </summary>
        public List<Prices> Prices { get; set; }

        /// <summary>
        /// For future Use.
        /// </summary>
        public SystemMapping SystemMapping { get; set; }
    }

    /// <summary>
    /// Search Result Response for Activity Mapping
    /// </summary>
    public class ActivitySearchResult
    {
        /// <summary>
        /// The Total Number of Activities returned by the Search Query
        /// </summary>
        public long TotalNumberOfActivities { get; set; }
        /// <summary>
        /// The NUmber of records included in the response per page
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// What is your current Page in the response
        /// </summary>
        public int CurrentPage { get; set; }
        /// <summary>
        /// What is the total number of pages in the response
        /// </summary>
        public int TotalPage { get; set; }
        /// <summary>
        /// Error Messages will appear here
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// A List containing the Activities matching the Search Request
        /// </summary>
        public List<Activity> Activities { get; set; }

    }

    /// <summary>
    /// Summary Activity Product Information for display as part of a Search Results page. The quality of this information may vary as Suppliers provide different 
    /// levels of static data to Consuming Systems.  Whilst there is the capability to return a Price within the Search Result, not all Suppliers provide this information
    /// within their Static Data definition. It is recommended that a Pricing/Availabilty call be made in real time to the Supplier to supplement
    /// any information returned by this API
    /// </summary>
    public class Activity
    {
        /// <summary>
        /// Mapping System Internal Code for Activity
        /// </summary>
        public long ActivityCode { get; set; }
        /// <summary>
        /// Mapping System Code for End Supplier. Full List of Codes can be retrieved from the Supplier Code API
        /// </summary>
        public string SupplierCompanyCode { get; set; }
        /// <summary>
        /// End Supplier System Product Code. This code should be used in all communication with the End Supplier
        /// </summary>
        public string SupplierProductCode { get; set; }
        /// <summary>
        /// Mapping System Activity Category for Product Classification. THis is the highest level of classification.
        /// </summary>
        public string Category { get; set; }
        /// <summary>
        /// Mapping System Activity Product Type Category for Product Classification. This is the second level of classification
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// Mapping System Activity Category for Product Classification. THis is the third level of classification.
        /// </summary>
        public string SubType { get; set; }
        /// <summary>
        /// The Name of the Activity Product as per the End Supplier
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The Main Description of the Activity from the End Supplier.
        /// </summary>
        public string Description { get; set; }

        ///// <summary>
        ///// Mapping System Activity Category for Product Session. 
        ///// THis classification is designed to group products into time-bands. 
        ///// It may not be defined automatically due to inconsistencies in SUpplier Static Data. Master Values can be retrieved from GetActivityClassificationStructure.
        ///// </summary>
        //public Session Session { get; set; }
        ///// <summary>
        ///// The Start Time of the Activity. This may not be defined due to variance in Supplier Static Data.
        ///// </summary>
        //public string StartTime { get; set; }
        ///// <summary>
        ///// The End Time of the Activity. This may not be defined due to variance in Supplier Static Data.
        ///// </summary>
        //public string EndTime { get; set; }


        /// <summary>
        /// Where does the Activity Depart from? This may not be defined due to variance in Supplier Static Data.
        /// </summary>
        public string DeparturePoint { get; set; }
        /// <summary>
        /// When the Activity is complete, where does it Return To? This may not be defined due to variance in Supplier Static Data.
        /// </summary>
        public string ReturnDetails { get; set; }
        /// <summary>
        /// What days of the week does this Activity Operate? Values should be in MTWTFSS format.
        /// </summary>
        public List<DaysOfWeek> DaysOfTheWeek { get; set; }

        /// <summary>
        /// The Physical Intensity Level of the Activity. Most End Suppliers do not carry a specific value for this attribute so it is inferred from Product Classification. Master Values can be retrieved from GetActivityClassificationStructure.
        /// </summary>
        public string PhysicalIntensity { get; set; }
        /// <summary>
        /// This field specifies for whom this activity is suitable.
        /// </summary>
        public string SuitableFor { get; set; }
        /// <summary>
        /// This is a Longer description of the Activity.
        /// </summary>
        public string Overview { get; set; }
        /// <summary>
        /// Is this Activity Recommended by either the Supplier or Mapping System
        /// </summary>
        public string Recommended { get; set; }
        /// <summary>
        /// This is the Mapping System Country Name. All Supplier values have been converted to this to allow simple cross supplier searches. Actual Supplier values is contained in the element SupplierDetails.
        /// </summary>
        public string CountryName { get; set; }
        /// <summary>
        /// This is the Mapping System Country Code. All Supplier values have been converted to this to allow simple cross supplier searches. Actual Supplier values is contained in the element SupplierDetails.
        /// </summary>
        public string CountryCode { get; set; }
        /// <summary>
        /// This is the Mapping System City Name. All Supplier values have been converted to this to allow simple cross supplier searches. Actual Supplier values is contained in the element SupplierDetails.
        /// </summary>
        public string CityName { get; set; }
        /// <summary>
        /// This is the Mapping System City Code. All Supplier values have been converted to this to allow simple cross supplier searches. Actual Supplier values is contained in the element SupplierDetails.
        /// </summary>
        public string CityCode { get; set; }
        /// <summary>
        /// What Star Rating does this Activity have? 
        /// </summary>
        public string StarRating { get; set; }
        /// <summary>
        /// What is the maximum number of passengers supported by this Activity? 
        /// </summary>
        public string NumberOfPassengers { get; set; }
        /// <summary>
        /// How many reviews does this Activity have? This value may be populated by Supplier Data, where it is provided.
        /// </summary>
        public string NumberOfReviews { get; set; }
        /// <summary>
        /// How many Likes does this Activity have? This value may be populated by Supplier Data, where it is provided.
        /// </summary>
        public string NumberOfLikes { get; set; }
        /// <summary>
        /// HOw many views does this Activity have? This value may be populated by Supplier Data, where it is provided.
        /// </summary>
        public string NumberOfViews { get; set; }
        /// <summary>
        /// A list of pictures or videos available for this Activity. The item will be a remote URL reference to picture / video.  This is End Supplier Data so the qulaity of this may vary. It may also contain inline HTML.
        /// </summary>
        public List<Media> ActivityMedia { get; set; }
        
        
        ///// <summary>
        ///// The duration of the activity. This is End Supplier Data so the qulaity of this may vary. It may also contain inline HTML
        ///// </summary>
        //public ActivityDuration Duration { get; set; }

        /// <summary>
        /// A list of "options" for an Activity. This data may need to be used when making specific Booking requests with End Suppliers. Not all Suppliers provide this information as part of their static data.
        /// </summary>
        public List<SimliarProducts> SimliarProducts { get; set; }
        /// <summary>
        /// A list of Prices for the Activity. The data here is retrieved for the product is contained ONLY in the static data and it is advised that a formal Pricing / Availability request is made to retrieve the actual price before booking.
        /// </summary>
        public List<Prices> Prices { get; set; }
    }

    /// <summary>
    /// In which session this activity falls in.
    /// </summary>
    public class Session
    {
        /// <summary>
        /// Supplier provided start time / departure time / commencement time
        /// </summary>
        public string SupplierValue { get; set; }
        /// <summary>
        /// extracted session value
        /// </summary>
        public string MappedValue { get; set; }
    }

    /// <summary>
    /// What is included as part of the Activity
    /// </summary>
    public class Inclusions
    {
        /// <summary>
        /// The Name of the Inclusion
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// A description of the inclusion. This data may contain inline HTML .
        /// </summary>
        public string Description { get; set; }
    }

    /// <summary>
    /// What is not included as part of the activity
    /// </summary>
    public class Exclusions
    {
        /// <summary>
        /// The Name of the Exclusion
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        ///  A description of the exclusion. This data may contain inline HTML .
        /// </summary>
        public string Description { get; set; }
    }

    /// <summary>
    /// Contains additional important information for display on a Product Detail page. This does not include formal 
    /// </summary>
    public class ImportantInfoAndBookingPolicies
    {
        /// <summary>
        /// What is the Type of Information. This will include Additional Information, Things to Carry, Voucher requirements etc
        /// </summary>
        public string InfoType { get; set; }
        /// <summary>
        ///  A description of the Policy. This data may contain inline HTML .
        /// </summary>
        public string InfoText { get; set; }
    }

    /// <summary>
    /// A collection of multimedia items for use on UI. Each individual supplier provides data in varying quality and as a result not all fields may be defined.
    /// </summary>
    public class Media
    {
        /// <summary>
        /// What type of Media. Currently supports image and video. 
        /// </summary>
        public string MediaType { get; set; }
        /// <summary>
        /// What sort of image is this? This includes High Resolution, Thumbnail.
        /// </summary>
        public string MediaSubType { get; set; }
        /// <summary>
        /// The full remote URL for the media item
        /// </summary>
        public string FullUrl { get; set; }
        /// <summary>
        /// The rull remote URL for the thumbnail version of the 
        /// </summary>
        public string ThumbUrl { get; set; }
        /// <summary>
        /// What order should the media items be displayed in
        /// </summary>
        public string SortOrder { get; set; }
        /// <summary>
        /// The descrition for the media item
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// the width in pixels for the media item
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// The height in pixels for the media item
        /// </summary>
        public int Height { get; set; }
        /// <summary>
        /// The Caption for the media item
        /// </summary>
        public string Caption { get; set; }

    }

    /// <summary>
    /// The Duration options for an Activity. Not all Suppliers provide full information for duration and as a result the quality of this information may vary
    /// </summary>
    public class ActivityDuration
    {
        /// <summary>
        /// Text description for the Activity. This is the most commonly-provided type of duration but is not ideal for faceting search results.
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// Duration of the Activity in Hours. For example, if an activity is 2 hours 30 minutes long, the value would be 2
        /// </summary>
        public string Hours { get; set; }
        /// <summary>
        /// Duration of the Activity in minutes. For example, if an activity is 2 hours 30 minutes long, the value would be 30.
        /// </summary>
        public string Minutes { get; set; }
    }

    /// <summary>
    /// The Type and Number of reviews for the Activity. 
    /// </summary>
    public class ReviewScores
    {
        /// <summary>
        /// Where does the review come from? This will mainly be pulled from Activity Static Data, but can also be used to store Internal Reviews
        /// </summary>
        public string Source { get; set; }
        /// <summary>
        /// What is the type of review score? For example "5 Star", "Cleanliness"
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// What score for this type of review. For example The Product has 25 (ReviewScores.Score) 5 Star Reviews (ReviewScores.Type)
        /// </summary>
        public decimal? Score { get; set; }
    }

    /// <summary>
    /// Actual written customer reviews for the Activity. This is designed to be used on a Product Detail Page.
    /// </summary>
    public class CustomerReviews
    {
        /// <summary>
        /// Where did the review come from? This is generally Supplier Data
        /// </summary>
        public string Source { get; set; }
        /// <summary>
        /// What is the type of review?
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// What is the score of the review?
        /// </summary>
        public decimal? Score { get; set; }
        /// <summary>
        /// Who wrote the review
        /// </summary>
        public string Author { get; set; }
        /// <summary>
        /// When was the review added
        /// </summary>
        public string Date { get; set; }
        /// <summary>
        /// What is the title of the review
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// The review detail
        /// </summary>
        public string Comment { get; set; }
    }

    /// <summary>
    /// Contains the location of the Activity. Not all Suppliers provide this information within their static data and as a result, the quality of this information may
    /// vary on a supplier to supplier basis.
    /// </summary>
    public class ActivityLocation
    {
        /// <summary>
        /// The latitide for the Activity
        /// </summary>
        public decimal? Latitude { get; set; }
        /// <summary>
        /// The Longitude for the Activity
        /// </summary>
        public decimal? Longitude { get; set; }
        /// <summary>
        /// The Physical Address for the Activity. This usually contains a combination of street, City, Country and Postal Code.
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// The "below" city position for the Activity. For example, if the activity is in Mumbai, the Location would be Andheri East, Andheri West etc.
        /// </summary>
        public string Location { get; set; }
        /// <summary>
        /// Further "below" city position for the Activity. For example, if the activity is in Mumbaoi (City), Andheri East (Location) then the Area could be Saki Naka, Marol, SEEPZ etc. 
        /// </summary>
        public string Area { get; set; }

    }

    /// <summary>
    /// What languages are supported by the Tour Guide (if applicable)
    /// </summary>
    public class TourGuideLanguages
    {
        /// <summary>
        /// The name of the language
        /// </summary>
        public string Language { get; set; }
        /// <summary>
        /// The code for the language
        /// </summary>
        public string LanguageID { get; set; }
    }

    /// <summary>
    /// Contains Key Supplier Specific Attributes that may be required during any Booking Process. Not all of these values will be provided by the ActivityDefinition Service
    /// </summary>
    public class SupplierDetails
    {
        /// <summary>
        /// This value will not be returned by the ActivityDefinition, but is included on request of the Consuming Application.
        /// </summary>
        public string RequestorID { get; set; }
        /// <summary>
        /// What pricing currency is the price information returned in
        /// </summary>
        public string PricingCurrency { get; set; }
        /// <summary>
        /// This value will not be returned by the ActivityDefinition, but is included on request of the Consuming Application.
        /// </summary>
        public string PrimaryLanguageID { get; set; }
        /// <summary>
        /// This value will not be returned by the ActivityDefinition, but is included on request of the Consuming Application.
        /// </summary>
        public string SupplierBrandCode { get; set; }
        /// <summary>
        /// The Mapping System Name for the Supplier
        /// </summary>
        public string SupplierName { get; set; }
        /// <summary>
        /// The Mapping System Code for the Supplier
        /// </summary>
        public string SupplierID { get; set; }
        /// <summary>
        /// The End Suppliers System Product Id
        /// </summary>
        public string TourActivityID { get; set; }
        /// <summary>
        /// The End Suppliers System COuntry Code
        /// </summary>
        public string CountryCode { get; set; }
        /// <summary>
        /// The End Suppliers System Country Name
        /// </summary>
        public string CountryName { get; set; }
        /// <summary>
        /// The End Suppliers System City Code
        /// </summary>
        public string CityCode { get; set; }
        /// <summary>
        /// The End Suppliers System City Name
        /// </summary>
        public string CityName { get; set; }
        /// <summary>
        /// This value will not be returned by the ActivityDefinition, but is included on request of the Consuming Application.
        /// </summary>
        public string StartPeriod { get; set; }
        /// <summary>
        /// This value will not be returned by the ActivityDefinition, but is included on request of the Consuming Application.
        /// </summary>
        public string EndPeriod { get; set; }
        /// <summary>
        /// This value will not be returned by the ActivityDefinition, but is included on request of the Consuming Application.
        /// </summary>
        public int? Quantity { get; set; }
        /// <summary>
        /// This value will not be returned by the ActivityDefinition, but is included on request of the Consuming Application.
        /// </summary>
        public int? Age { get; set; }
        /// <summary>
        /// This value will not be returned by the ActivityDefinition, but is included on request of the Consuming Application.
        /// </summary>
        public string QualifierInfo { get; set; }
        /// <summary>
        /// This value will not be returned by the ActivityDefinition, but is included on request of the Consuming Application.
        /// </summary>
        public string TimeSlotCode { get; set; }

    }

    /// <summary>
    /// Contains information relating to bifurcations of a Product. For example, Tour of Taj Mahal may have options for different durations or Guide types. 
    /// Not all Suppliers Provide this information
    /// </summary>
    public class SimliarProducts
    {
        /// <summary>
        /// The Mapping System Code for the Activity Option 
        /// </summary>
        public string SystemActivityOptionCode { get; set; }
        /// <summary>
        /// The End Supplier Code for the Activity Option
        /// </summary>
        public string OptionCode { get; set; }
        /// <summary>
        /// Any specific Deal Text that is applicable to the Activity Option. This 
        /// </summary>
        public string DealText { get; set; }
        /// <summary>
        /// The name of the Activity Option
        /// </summary>
        public string Options { get; set; }
        /// <summary>
        /// The Type of the Activity Option
        /// </summary>
        public string ActivityType { get; set; }
    }

    /// <summary>
    /// This will contain all classification attribute of Sub Type INTERNAL.
    /// </summary>
    public class ClassificationAttrributes
    {
        /// <summary>
        /// Classification Attribute SubType
        /// </summary>
        public string Group { get; set; }
        /// <summary>
        /// Classification Attribute Type
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// Classification Attribute Value
        /// </summary>
        public string Value { get; set; }
    }

    /// <summary>
    /// Deals
    /// </summary>
    public class Deals
    {
        public decimal? DealPrice { get; set; }
        public string Currency { get; set; }
        public string DealText { get; set; }
        public string OfferTermsAndConditions { get; set; }
        public string DealId { get; set; }
    }

    /// <summary>
    /// Contains any prices retrieved from the End Supplier Static Data. Not all Suppliers provide this information within their static data and it is advised that
    /// a specific Pricing / Availability check be made in real time to the Supplier when showing a Product Detail page. The information contained within this section should then
    /// be discarded in favour of the real time information.
    /// A product may contain more than one type of Price.
    /// </summary>
    public class Prices
    {
        /// <summary>
        /// What currency is the price?
        /// </summary>
        public string SupplierCurrency { get; set; }
        /// <summary>
        /// The Price for the value
        /// </summary>
        public double? Price { get; set; }
        /// <summary>
        /// What sort of price is this? 
        /// MERCHANT NET PRICE - 
        /// PRICE - 
        /// RACK - 
        /// </summary>
        public string PriceType { get; set; }
        /// <summary>
        /// How is this price constructed? 
        /// ADULT - Per Adult
        /// CHILD - Per Child
        /// </summary>
        public string PriceBasis { get; set; }
        /// <summary>
        /// The End Supplier Price Code 
        /// </summary>
        public string PriceId { get; set; }
        /// <summary>
        /// If it is the Option code for Similar Products
        /// </summary>
        public string OptionCode { get; set; }
        /// <summary>
        /// Whether the price is for Product or Option (Similar Product)
        /// </summary>
        public string PriceFor { get; set; }
    }

    /// <summary>
    /// For Future Use to handle Mapping to Extenal Systems
    /// </summary>
    public class SystemMapping
    {
        /// <summary>
        /// What is the System name that we are mappingt to? For example CKIS, TLGX, VGER
        /// </summary>
        public string SystemName { get; set; }
        /// <summary>
        /// What is the System Code for this Activity
        /// </summary>
        public string SystemID { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class DaysOfWeek
    {
        public string SupplierFrequency { get; set; }
        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }
        public bool Sunday { get; set; }
        public string SupplierStartTime { get; set; }
        public string StartTime { get; set; }
        public string SupplierEndTime { get; set; }
        public string EndTime { get; set; }
        public string SupplierDuration { get; set; }
        public string Duration { get; set; }
        public string SupplierSession { get; set; }
        public string Session { get; set; }
        public string OperatingFromDate { get; set; }
        public string OperatingToDate { get; set; }
    }
}