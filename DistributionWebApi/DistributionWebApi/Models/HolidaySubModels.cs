using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistributionWebApi.Models
{
    #region Holiday.Collections
    /// <summary>
    /// Structure for Holiday.Collections
    /// </summary>
    public class Collections
    {
        /// <summary>
        /// Master values may be retrived from a Master Service.
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// Name of the Supplier.
        /// </summary>
        public string DisplayName { get; set; }
    }
    #endregion

    #region Holiday.Interests & Holiday.TravellerType
    /// <summary>
    /// Structure for Holiday.Interests,Holiday.TravellerType,Holiday.TravelFrequency
    /// </summary>
    public class Types
    {
        /// <summary>
        ///  Values may be retrived from Master Service.
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        ///   SubTypes for Type.Values may be retrived from Master Service.
        /// </summary>
        public List<string> SubType { get; set; }
    }
    #endregion

    #region Holiday.Destinations
    /// <summary>
    /// Structure for Holiday.Destinations
    /// </summary>
    public class Destinations
    {
        /// <summary>
        ///  TLGX Country Code.Values may be retrived from Master Service.
        /// </summary>
        public string TLGXCountryCode { get; set; }
        /// <summary>
        ///  TLGX Country Name.Values may be retrived from Master Service.
        /// </summary>
        public string TLGXCountryName { get; set; }
        /// <summary>
        ///  TLGX City Code.Values may be retrived from Master Service.
        /// </summary>
        public string TLGXCityCode { get; set; }
        /// <summary>
        ///  TLGX City Name.Values may be retrived from Master Service.
        /// </summary>
        public string TLGXCityName { get; set; }
        /// <summary>
        ///  CountryCode given by Supplier
        /// </summary>
        public string SupplierCountryCode { get; set; }
        /// <summary>
        ///  CountryName given by Supplier
        /// </summary>
        public string SupplierCountryName { get; set; }
        /// <summary>
        ///  CityCode given by Supplier
        /// </summary>
        public string SupplierCityCode { get; set; }
        /// <summary>
        ///  CityName given by Supplier
        /// </summary>
        public string SupplierCityName { get; set; }
    }
    #endregion

    #region Holiday.HolidayIncludes
    /// <summary>
    /// Structure for Holiday.HolidayIncludes
    /// </summary>
    public class HolidayIncludes
    {
        /// <summary>
        /// If Air service is included in Holiday or not.
        /// </summary>
        public string Air { get; set; }
        /// <summary>
        /// If Hotels are included in Holiday or not.
        /// </summary>
        public string Hotel { get; set; }
        /// <summary>
        ///  If Transfer services are included in Holiday or not.
        /// </summary>
        public string Transfers { get; set; }
        /// <summary>
        /// If Sightseeing is included in Holiday or not.
        /// </summary>
        public string Sightseeing { get; set; }
        /// <summary>
        /// If Meals are included in Holiday or not.
        /// </summary>
        public string Meals { get; set; }
        /// <summary>
        /// If Visa service is provided in Holiday or not.
        /// </summary>
        public string Visa { get; set; }
        /// <summary>
        /// if Insurance is included in holiday or not.
        /// </summary>
        public string Insurance { get; set; }
    }
    #endregion

    #region Holiday.UniqueSellingPoints
    /// <summary>
    /// Structure for Holiday.UniqueSellingPoints
    /// </summary>
    public class UniqueSellingPoints
    {
        /// <summary>
        ///  Id of unique sellinf Points
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        ///  Description about attractions/UniqueSellingPoints.
        /// </summary>
        public string Text { get; set; }
    }
    #endregion

    #region Holiday.Highlights
    /// <summary>
    /// Structure for Holiday.Highlights
    /// </summary>
    public class Highlights
    {
        /// <summary>
        /// Name of the destination provided by supplier.
        /// </summary>
        public string Destination { get; set; }
        /// <summary>
        ///  City Name  provided by supplier.
        /// </summary>
        public string City { get; set; }
        /// <summary>
        ///  Country Name  provided by supplier.
        /// </summary>
        public string Country { get; set; }
        /// <summary>
        /// Day
        /// </summary>
        public string Day { get; set; }
        /// <summary>
        /// Date on which this information is valid.
        /// </summary>
        public string Date { get; set; }
        /// <summary>
        /// Type of the attraction.
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// Description for the attraction.
        /// </summary>
        public string Text { get; set; }

    }
    #endregion

    #region Holiday.Brands & Holiday.Brochures
    /// <summary>
    /// Structure for Holiday.Brands
    /// </summary>
    public class BrandsAndBrochures
    {
        /// <summary>
        ///  Id. 
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        ///  Name.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        ///  Code provided by supplier.
        /// </summary>
        public string Code { get; set; }
    }
    #endregion

    #region Holiday.Media
    /// <summary>
    /// Structure for Holiday.Media
    /// </summary>
    public class Media
    {
        /// <summary>
        /// Url to access the media file.
        /// </summary>
        public string URL { get; set; }
        /// <summary>
        /// width of the media file.
        /// </summary>
        public string Width { get; set; }
        /// <summary>
        /// Height of the media file.
        /// </summary>
        public string Height { get; set; }
        /// <summary>
        /// name of the media file.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Display name for media file.
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// category of  media .
        /// </summary>
        public string FileCategory { get; set; }
        /// <summary>
        /// Type of  media.
        /// </summary>
        public string FileType { get; set; }
        /// <summary>
        /// path to access media.
        /// </summary>
        public string FilePath { get; set; }
        /// <summary>
        /// from which date media information is valid.
        /// </summary>
        public string ValidFromDate { get; set; }
        /// <summary>
        /// upto which date media information is valid.
        /// </summary>
        public string ValidToDate { get; set; }
        /// <summary>
        /// Display order for media file.
        /// </summary>
        public string DisplayOrder { get; set; }
        /// <summary>
        /// detail description about media.
        /// </summary>
        public string Description { get; set; }
    }
    #endregion

    #region Holiday.TermsConditions & Holiday.BookingPolicy & Holiday.TourNotes
    /// <summary>
    /// Structure for Holiday.TermsConditions,Holiday.BookingPolicy,Holiday.TourNotes
    /// </summary>
    public class TermsConditions
    {
        /// <summary>
        /// Type of information
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// name.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Description
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// order number.
        /// </summary>
        public string Order { get; set; }

    }
    #endregion

    #region Holiday.Inclusions & Holiday.Exclusions
    /// <summary>
    /// Structure for Holiday.Inclusions,Holiday.Exclusions
    /// </summary>
    public class InclusionExclusion
    {
        /// <summary>
        /// Inclusion/exclusion type.
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// Name for inclusion/exclusion.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// description of inclusion/exclusion.
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// order of inclusion/exclusion.
        /// </summary>
        public string Order { get; set; }
        /// <summary>
        /// IsMandatoryPaid for the inclusion/exclusion.
        /// </summary>
        public string IsMandatoryPaid { get; set; }

    }
    #endregion

    #region Holiday.StartingPrice 
    /// <summary>
    /// Structure for Holiday.StartingPrice
    /// </summary>
    public class StartingPrice
    {
        /// <summary>
        /// Currency in which price is given by supplier.
        /// </summary>
        public string Currency { get; set; }
        /// <summary>
        /// Price for Holiday.
        /// </summary>
        public string Amount { get; set; }
        /// <summary>
        ///  Information about on which basis price is calculated.
        /// </summary>
        public string PriceBasis { get; set; }
        /// <summary>
        /// Description about price.
        /// </summary>
        public string PriceText { get; set; }

    }
    #endregion Holiday.StartingPrice

    #region Holiday.HubDetails 
    /// <summary>
    /// Structure for Holiday.HubDetails
    /// </summary>
    public class HubDetails
    {
        /// <summary>
        /// Id of the Hub.
        /// </summary>
        public string HubId { get; set; }
        /// <summary>
        /// Name of the Hub.
        /// </summary>
        public string HubName { get; set; }

    }
    #endregion Holiday.HubDetails

    #region Holiday.DayWiseItineraries 
    /// <summary>
    /// Structure for Holiday.DayWiseItineraries
    /// </summary>
    public class DayWiseItineraries
    {
        /// <summary>
        /// Date from which Itinerary details are valid.
        /// </summary>
        public string FromDate { get; set; }
        /// <summary>
        /// Date upto  which Itinerary details are valid.
        /// </summary>
        public string ToDate { get; set; }
        /// <summary>
        /// Package category is the Holiday category(e.g 2*,4* ).
        /// </summary>
        public string PackageCategory { get; set; }
        /// <summary>
        /// Host category is type of Holiday(e.g economy, delux,premium etc..).
        /// </summary>
        public string HostCategory { get; set; }
        /// <summary>
        /// Day wise Itinerary
        /// </summary>
        public List<Day> Day { get; set; }


    }

    #region Holiday.DayWiseItineraries.Day
    /// <summary>
    /// Holiday.DayWiseItineraries.Day
    /// </summary>
    public class Day
    {
        /// <summary>
        /// Day Number for Itinerary.
        /// </summary>
        public string DayNumber { get; set; }
        /// <summary>
        /// Date for itinerary details .
        /// </summary>
        public string Date { get; set; }
        /// <summary>
        /// header info for itinearary
        /// </summary>
        public string DayHeader { get; set; }
        /// <summary>
        /// Country Name given by supplier.
        /// </summary>
        public string Country { get; set; }
        /// <summary>
        /// CountryCode given by supplier.
        /// </summary>
        public string CountryCode { get; set; }
        /// <summary>
        /// City Name given by supplier.
        /// </summary>
        public string City { get; set; }
        /// <summary>
        ///  City code given by supplier.
        /// </summary>
        public string CityCode { get; set; }
        /// <summary>
        /// Type of the itinerary.
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// overninght stay for the day.
        /// </summary>
        public string OvernightStay { get; set; }
        /// <summary>
        /// Short Description for itinerary on the day.
        /// </summary>
        public string ShortDescription { get; set; }
        /// <summary>
        /// Long Description for itinerary on the day.
        /// </summary>
        public string LongDescription { get; set; }
        /// <summary>
        /// Note.
        /// </summary>
        public string DayNote { get; set; }
        /// <summary>
        /// information about meals
        /// </summary>
        public List<Meals> Meals { get; set; }
        /// <summary>
        /// Information about different services(Itinerary items) on this day.
        /// </summary>
        public List<ItineraryItems> ItineraryItems { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Media> Media { get; set; }
    }

    #region Holiday.DayWiseItineraries.Day.Meals
    /// <summary>
    /// Holiday.DayWiseItineraries.Day.Meals
    /// </summary>
    public class Meals
    {
        /// <summary>
        /// Type of the meal.
        /// </summary>
        public string MealType { get; set; }
        /// <summary>
        /// Description about meal.
        /// </summary>
        public string MealDescription { get; set; }
    }
    #endregion Holiday.DayWiseItineraries.Day.Meals

    #region Holiday.DayWiseItineraries.Day.ItineraryItems
    /// <summary>
    /// Holiday.DayWiseItineraries.Day.ItineraryItems
    /// </summary>
    public class ItineraryItems
    {
        /// <summary>
        /// Type of Service detail
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// Name of Service detail
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Country name provided by supplier
        /// </summary>
        public string Country { get; set; }
        /// <summary>
        /// City name provided by supplier
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// Information about overnight stay for this service.
        /// </summary>
        public string OvernightStay { get; set; }
        /// <summary>
        /// Long description about service.
        /// </summary>
        public string LongDescription { get; set; }
        /// <summary>
        ///  short description about service.
        /// </summary>
        public string ShortDescription { get; set; }
        /// <summary>
        /// Pick up point for the Itinerary service.
        /// </summary>
        public string PickupPoint { get; set; }
        /// <summary>
        /// Drop point for the Itinerary service.
        /// </summary>
        public string DropPoint { get; set; }
        /// <summary>
        /// Pick up time for the Itinerary service.
        /// </summary>
        public string PickupTime { get; set; }
        /// <summary>
        /// Drop time for the Itinerary service.
        /// </summary>
        public string DropTime { get; set; }
        /// <summary>
        /// Total duration for the completion.
        /// </summary>
        public string Duration { get; set; }
    }
    #endregion Holiday.DayWiseItineraries.Day.ItineraryItems

    #endregion Holiday.DayWiseItineraries.Day

    #endregion Holiday.DayWiseItineraries

    #region Holiday.Accommodation 
    /// <summary>
    /// Structure for Holiday.Accommodation
    /// </summary>
    public class Accommodation
    {
        /// <summary>
        /// TLGX Code for Product given by supplier.
        /// </summary>
        public string TLGXProductCode { get; set; }
        /// <summary>
        /// Code provided by Supplier for Product.
        /// </summary>
        public string SupplierProductCode { get; set; }
        /// <summary>
        ///  TLGX CountryName.Master values may be retrived from a Master Service.
        /// </summary>
        public string TLGXCountryName { get; set; }
        /// <summary>
        ///TLGX country code.
        /// </summary>
        public string TLGXCountryCode { get; set; }
        /// <summary>
        /// TLGX Code for Product given by supplier.
        /// </summary>
        public string TLGXCityCode { get; set; }
        /// <summary>
        /// City Name provided by Supplier.
        /// </summary>
        public string SupplierCityName { get; set; }
        /// <summary>
        ///  City Code provided by Supplier.
        /// </summary>
        public string SupplierCityCode { get; set; }
        /// <summary>
        /// Number of Nights.
        /// </summary>
        public string NumberOfNights { get; set; }
        /// <summary>
        ///  Tour Package Category under which this accomodation is provided. values may be 2*,3* etc..
        /// </summary>
        public string PackageCategory { get; set; }
        /// <summary>
        /// Tour SubCategory values may be Economy,Delux,Premium etc..
        /// </summary>
        public string HostCategory { get; set; }
        /// <summary>
        /// Category of Hotel.
        /// </summary>
        public string HotelCategory { get; set; }
        /// <summary>
        ///  Name of the Hotel.
        /// </summary>
        public string HotelName { get; set; }
        /// <summary>
        /// star Rtaing of Hotel (e.g 3*,4*)
        /// </summary>
        public string StarRating { get; set; }
        /// <summary>
        ///  Date on which this accomodation is valid.
        /// </summary>
        public string StartDate { get; set; }
        /// <summary>
        /// Date upto which this accomodation is valid.
        /// </summary>
        public string FinishDate { get; set; }
        /// <summary>
        /// Departure Id
        /// </summary>
        public string DepartureId { get; set; }
        /// <summary>
        ///  Date for Departure.
        /// </summary>
        public string DepartureStartDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string DepartureEndDate { get; set; }
        /// <summary>
        ///  Rating by Trip Advisor for the hotel.
        /// </summary>
        public string TripAdvisorRating { get; set; }
        /// <summary>
        /// Description for Hotel.
        /// </summary>
        public string HotelDescription { get; set; }
        /// <summary>
        ///  information of Facilities at Hotel.
        /// </summary>
        public List<Facility> Facilities { get; set; }
        /// <summary>
        ///  Media for Hotel(images/Videos)
        /// </summary>
        public List<Media> Media { get; set; }
    }
    #endregion Holiday.Accommodation

    #region Holiday.Accommodation.Facilities
    /// <summary>
    /// Structure for Holiday.Accommodation.Facility
    /// </summary>
    public class Facility
    {
        /// <summary>
        /// Facility Type
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// Name of the Facility.
        /// </summary>
        public string Name { get; set; }
    }
    #endregion Holiday.Accommodation.Facilities

    #region Holiday.Activity 
    /// <summary>
    /// Structure for Holiday.Activity
    /// </summary>
    public class HolidayActivity
    {
        /// <summary>
        /// TLGX Code for Product given by supplier.
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// Code provided by Supplier for Product.
        /// </summary>
        public string ActivityName { get; set; }
        /// <summary>
        ///  TLGX CountryName.Master values may be retrived from a Master Service.
        /// </summary>
        public string ActivityDescription { get; set; }
        /// <summary>
        ///TLGX country code.
        /// </summary>
        public string CountryName { get; set; }
        /// <summary>
        /// TLGX Code for Product given by supplier.
        /// </summary>
        public string CityName { get; set; }
        /// <summary>
        /// City Name provided by Supplier.
        /// </summary>
        public string SupplierCityCode { get; set; }
        /// <summary>
        ///  City Code provided by Supplier.
        /// </summary>
        public string TLGXCountryName { get; set; }
        /// <summary>
        /// Number of Nights.
        /// </summary>
        public string TLGXCountryCode { get; set; }
        /// <summary>
        ///  Tour Package Category under which this accomodation is provided. values may be 2*,3* etc..
        /// </summary>
        public string TLGXCityName { get; set; }
        /// <summary>
        /// Tour SubCategory values may be Economy,Delux,Premium etc..
        /// </summary>
        public string TLGXCityCode { get; set; }
        /// <summary>
        /// Category of Hotel.
        /// </summary>
        public string DayNumber { get; set; }
        /// <summary>
        ///  Name of the Hotel.
        /// </summary>
        public string Date { get; set; }
        /// <summary>
        /// star Rtaing of Hotel (e.g 3*,4*)
        /// </summary>
        public string StartTime { get; set; }
        /// <summary>
        ///  Date on which this accomodation is valid.
        /// </summary>
        public string EndTime { get; set; }
        /// <summary>
        /// Date upto which this accomodation is valid.
        /// </summary>
        public string Lat { get; set; }
        /// <summary>
        /// Departure Id
        /// </summary>
        public string Lon { get; set; }
        /// <summary>
        ///  Date for Departure.
        /// </summary>
        public string StartDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string FinishDate { get; set; }
        /// <summary>
        ///  Rating by Trip Advisor for the hotel.
        /// </summary>
        public string DepartureId { get; set; }
        /// <summary>
        /// Description for Hotel.
        /// </summary>
        public string DepartureStartDate { get; set; }
        /// <summary>
        ///  information of Facilities at Hotel.
        /// </summary>
        public List<Facility> DepartureEndDate { get; set; }
        /// <summary>
        ///  Package category for Holiday
        /// </summary>
        public List<Media> PackageCategory { get; set; }
        /// <summary>
        /// Host Category of Holiday. Values may be (Economy,Delux,Premium etc..)
        /// </summary>
        public string HostCategory { get; set; }
        /// <summary>
        ///  Pick Up Point.
        /// </summary>
        public string PickupDestination { get; set; }
        /// <summary>
        /// Drop point.
        /// </summary>
        public string DropDestination { get; set; }
        /// <summary>
        ///  Media details.
        /// </summary>
        public List<Media> Media { get; set; }
        /// <summary>
        ///  what is included in Holiday.
        /// </summary>
        public List<InclusionExclusion> Inclusion { get; set; }
        /// <summary>
        ///  what is excluded in Holiday.
        /// </summary>
        public List<InclusionExclusion> Exclusion { get; set; }
        /// <summary>
        ///  Booking policies
        /// </summary>
        public List<TermsConditions> BookingPolicies { get; set; }
        /// <summary>
        ///  Review for Activity.
        /// </summary>
        public List<HolidayReview> Review { get; set; }
    }
    #endregion Holiday.Activity

    #region Review
    /// <summary>
    /// Structure for Holiday.Activity.Review
    /// </summary>
    public class HolidayReview
    {
        /// <summary>
        /// Title for review.
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Description of review.
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// Name for review.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Review score.
        /// </summary>
        public string Score { get; set; }
        /// <summary>
        /// Image URL for review.
        /// </summary>
        public string ImageURL { get; set; }
        /// <summary>
        /// Date on which review is given.
        /// </summary>
        public string Date { get; set; }
        /// <summary>
        /// Who has given the review.
        /// </summary>
        public string ReviewProvider { get; set; }

    }
    #endregion Review

    #region Holiday.Flights
    /// <summary>
    /// Structure for Holiday.Flights
    /// </summary>
    public class Flights
    {
        /// <summary>
        /// Name of the Airline Company.
        /// </summary>
        public string Airline { get; set; }
        /// <summary>
        /// Flight Number.
        /// </summary>
        public string FlightNumber { get; set; }
        /// <summary>
        /// Flight is included in holiday or not.
        /// </summary>
        public string IncludedInTrip { get; set; }
        /// <summary>
        /// Airport Code from which Departure will take place.
        /// </summary>
        public string DepartureAirportCode { get; set; }
        /// <summary>
        /// Airport Name from which Departure will take place.
        /// </summary>
        public string DepartureAirportName { get; set; }
        /// <summary>
        /// Name of the terminal.
        /// </summary>
        public string DepartureTerminal { get; set; }
        /// <summary>
        /// Time for Departure.
        /// </summary>
        public string DepartureTime { get; set; }
        /// <summary>
        /// Date of Departure.
        /// </summary>
        public string DepartureDate { get; set; }
        /// <summary>
        /// Duration.
        /// </summary>
        public string Duration { get; set; }
        /// <summary>
        /// Stops for Flight.
        /// </summary>
        public string FlightStops { get; set; }
        /// <summary>
        /// Airport Code .
        /// </summary>
        public string ArrivalAirportCode { get; set; }
        /// <summary>
        /// Airport name.
        /// </summary>
        public string ArrivalAirportName { get; set; }
        /// <summary>
        /// Terminal information.
        /// </summary>
        public string ArrivalTerminal { get; set; }
        /// <summary>
        /// Time of Flight arrival.
        /// </summary>
        public string ArrivalTime { get; set; }
        /// <summary>
        /// date for Flight Arrival.
        /// </summary>
        public string ArrivalDate { get; set; }
        /// <summary>
        /// Class of Flight ticket(e.g Economy,Buisness)
        /// </summary>
        public string TicketClass { get; set; }
        /// <summary>
        /// Information about Baggage Allowance.
        /// </summary>
        public string BaggageAllowance { get; set; }
        /// <summary>
        /// Information about meals provided.
        /// </summary>
        public string Meals { get; set; }
        /// <summary>
        /// Information about Refunds.
        /// </summary>
        public string RefundPolicy { get; set; }
        /// <summary>
        /// Flight Booking Policies
        /// </summary>
        public string BookingPolicyText { get; set; }
        /// <summary>
        /// Inclusion details.
        /// </summary>
        public List<InclusionExclusion> Inclusion { get; set; }
        /// <summary>
        /// Exclusion details.
        /// </summary>
        public List<InclusionExclusion> Exclusion { get; set; }

    }
    #endregion Holiday.Flights

    #region Holiday.Departure
    /// <summary>
    /// Structure for Holiday.Departure
    public class Departure
    {
        /// <summary>
        ///
        /// </summary>
        public string DepartureId { get; set; }
        /// <summary>
        ///
        /// </summary>
        public string DepartureName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string StartDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string EndDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string StartCity { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string EndCity { get; set; }

    }
    #endregion Holiday.Departure

    #region Holiday.PrePost
    /// <summary>
    /// Structure for Holiday.Pre, Holiday.Post
    /// </summary>
    public class PrePost
    {
        /// <summary>
        /// Type of information.
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// CountryN ame provided by Supplier.
        /// </summary>
        public string Country { get; set; }
        /// <summary>
        /// City Name provided by Supplier.
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// TLGX HotelCode.
        /// </summary>
        public string TLGXProductCode { get; set; }
        /// <summary>
        ///  Supplier HotelCode.
        /// </summary>
        public string SupplierProductCode { get; set; }
        /// <summary>
        /// Name of the Hotel.
        /// </summary>
        public string HotelName { get; set; }
        /// <summary>
        /// Hotel Star  Rating.
        /// </summary>
        public string StarRating { get; set; }
        /// <summary>
        ///  Hotel Description
        /// </summary>
        public string HotelDescription { get; set; }
        /// <summary>
        /// Currency in which price is given.
        /// </summary>
        public string Currency { get; set; }
        /// <summary>
        /// Price.
        /// </summary>
        public string Amount { get; set; }
        /// <summary>
        /// Description about price.
        /// </summary>
        public string PriceFor { get; set; }
        /// <summary>
        /// Number Of Nights in the Hotel.
        /// </summary>
        public string NumberOfNights { get; set; }
        /// <summary>
        /// Number Of Days in the Hotel.
        /// </summary>
        public string DayNo { get; set; }
        /// <summary>
        /// Rating by Trip Advisor.
        /// </summary>
        public string TripAdvisorRating { get; set; }
        /// <summary>
        /// Pax description for hotel.
        /// </summary>
        public string PaxDescription { get; set; }

    }
    #endregion Holiday.PrePost

    #region Holiday.Optionals
    /// <summary>
    /// Structure for Holiday.Pre, Holiday.Post
    /// </summary>
    public class Optionals
    {
        /// <summary>
        /// Country name given by supplier.
        /// </summary>
        public string Country { get; set; }
        /// <summary>
        /// City Name provided by Supplier.
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// TLGX Country Name.
        /// </summary>
        public string TLGXCountryName { get; set; }
        /// <summary>
        /// TLGX Country Code
        /// </summary>
        public string TLGXCountryCode { get; set; }
        /// <summary>
        ///  TLGX City Name
        /// </summary>
        public string TLGXCityName { get; set; }
        /// <summary>
        /// TLGX City Code
        /// </summary>
        public string TLGXCityCode { get; set; }
        /// <summary>
        ///On which day Optional  Activity is available .
        /// </summary>
        public string AvailableonDays { get; set; }
        /// <summary>
        ///  Date for  optional Activity.
        /// </summary>
        public string Date { get; set; }
        /// <summary>
        /// Starting Time.
        /// </summary>
        public string StartTime { get; set; }
        /// <summary>
        /// End Time.
        /// </summary>
        public string EndTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Session { get; set; }
        /// <summary>
        ///Total  Duration of this optional activity.
        /// </summary>
        public string Duration { get; set; }
        /// <summary>
        /// Name of Activity.
        /// </summary>
        public string ActivityName { get; set; }
        /// <summary>
        /// Description for activity.
        /// </summary>
        public string ActivityDescription { get; set; }
        /// <summary>
        /// Image for activity.
        /// </summary>
        public string ActivityImage { get; set; }
        /// <summary>
        /// Review ratings for activity.
        /// </summary>
        public string AverageReviewRating { get; set; }
        /// <summary>
        /// In which Currency price is given by supplier.
        /// </summary>
        public string Currency { get; set; }
        /// <summary>
        /// price for this optional activity.
        /// </summary>
        public string Price { get; set; }
        /// <summary>
        /// Description about no. of peoples.
        /// </summary>
        public string PaxDescription { get; set; }
        /// <summary>
        /// On which day this Optional Activity is valid.
        /// </summary>
        public string ItienraryDay { get; set; }
        /// <summary>
        /// What is Included in Activity.
        /// </summary>
        public List<InclusionExclusion> Inclusion { get; set; }
        /// <summary>
        /// What is excluded from activity
        /// </summary>
        public List<InclusionExclusion> Exclusion { get; set; }

    }
    #endregion Holiday.optionals

    #region Holiday.Extension
    /// <summary>
    /// Structure for Holiday.Extension
    /// </summary>
    public class Extension
    {
        /// <summary>
        /// Country name given by supplier.
        /// </summary>
        public string Country { get; set; }
        /// <summary>
        /// City Name provided by Supplier.
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// Name.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// No. of days extended.
        /// </summary>
        public string Days { get; set; }
        /// <summary>
        ///  No. of nights extended.
        /// </summary>
        public string Nights { get; set; }
        /// <summary>
        /// Detail description for extension.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        ///What is included.
        /// </summary>
        public string Inclusions { get; set; }
        /// <summary>
        /// price for this.
        /// </summary>
        public string Price { get; set; }
        /// <summary>
        /// in which currency price is given by Supplier.
        /// </summary>
        public string Currency { get; set; }
        /// <summary>
        /// On what basis price is valid.( Detail Information about price.)
        /// </summary>
        public string PriceBasis { get; set; }
        /// <summary>
        /// Holiday Reference Number
        /// </summary>
        public string HolidayReferenceNumber { get; set; }
    }
    #endregion Holiday.Extension

    #region Holiday.Review
    /// <summary>
    /// Structure for Holiday.Extension
    /// </summary>
    public class Review
    {
        /// <summary>
        /// Summary of Reviews.
        /// </summary>
        public ReviewSummary ReviewSummary { get; set; }
        /// <summary>
        /// Details of Reviews
        /// </summary>
        public HolidayReview ReviewDetails { get; set; }

    }
    #endregion Holiday.Review

    #region Holiday.ReviewSummary
    /// <summary>
    /// Structure for Holiday.ReviewSummary
    /// </summary>
    public class ReviewSummary
    {
        /// <summary>
        /// Source of Reviews.
        /// </summary>
        public string Source { get; set; }
        /// <summary>
        /// Type of Reviews.
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        ///Review Score.
        /// </summary>
        public string score { get; set; }

    }

    #endregion Holiday.ReviewSummary

}

