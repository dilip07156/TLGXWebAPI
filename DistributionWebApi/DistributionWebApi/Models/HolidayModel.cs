using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DistributionWebApi.Models
{
    /// <summary>
    ///
    /// </summary>
    [BsonIgnoreExtraElements]
    public class HolidayModel
    {
        /// <summary>
        /// Unique Id for each Tour.
        /// </summary>
        [Required]
        public string Id { get; set; }
        /// <summary>
        /// Name of the supplier providing this holiday.
        /// </summary>
        [Required]
        public string SupplierName { get; set; }
        /// <summary>
        /// Product Code provided by supplier. may be required in the booking process.
        /// </summary>
        public string SupplierProductCode { get; set; }
        /// <summary>
        /// Holiday is effective from this date.
        /// </summary>
        public string EffectiveFromDate { get; set; }
        /// <summary>
        /// Date on which Holiday is created on system.
        /// </summary>
        [Required]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// The user who created the product.
        /// </summary>
        public string CreateUSer { get; set; }
        /// <summary>
        ///Date on which Holiday was last edited.
        /// </summary>
        public DateTime? EditDate { get; set; }
        /// <summary>
        /// The user who edited Holiday.
        /// </summary>
        public string EditUser { get; set; }
        /// <summary>
        /// The Product category for this Holiday.Master values may be retrived from a Master Service.
        /// </summary>
        public string ProductCategory { get; set; }
        /// <summary>
        /// The Product SubCategory for this Holiday.Master values may be retrived from a Master Service.
        /// </summary>
        public string ProductCategorySubtype { get; set; }
        /// <summary>
        /// Name of the Holiday
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Short Description for the holiday.
        /// </summary>
        public string ShortDescription { get; set; }
        /// <summary>
        /// Long Description for the holiday.
        /// </summary>
        public string LongDescription { get; set; }
        /// <summary>
        /// Holiday Type can be all,group,Individual.Master values may be retrived from a Master Service.
        /// </summary>
        public string TypeOfHoliday { get; set; }
        /// <summary>
        /// Id of the holiday flavor.Flavor is any change in activity,travelType for the Holiday.
        /// </summary>
        public string ProductFlavorID { get; set; }
        /// <summary>
        /// Name of the flavor. 
        /// </summary>
        public string ProductFlavorName { get; set; }
        /// <summary>
        /// TourOptionID. 
        /// </summary>
        public string TourOptionID { get; set; }
        /// <summary>
        /// It is a type of flavour. e.g self drive package ,cruise package.
        /// </summary>
        public string FlavorType { get; set; }
        /// <summary>
        /// This will have value either FIT(Fully Independant Traveller) or Group
        /// </summary>
        public string ProductType { get; set; }
        /// <summary>
        /// This will have value either 'SET Packages' or 'Customized Packages'.
        /// </summary>
        public string PackageType { get; set; }
        /// <summary>
        /// This has values like Standard,Delux,Executive.
        /// </summary>
        public string CompanyPackageCategory { get; set; }
        /// <summary>
        /// Collection is a name of Supplier providing holiday.
        /// </summary>
        public Collections Collections { get; set; }
        /// <summary>
        /// Biforgation of Holiday by types of traveller such as Couple ,Men,Women etc..Values may be retrived from Master Service.   
        /// </summary>
        public List<Types> TravellerType { get; set; }
        /// <summary>
        /// Biforgation of Holiday by interests such as Beach ,Backpack,Hills etc..Values may be retrived from Master Service.   
        /// </summary>
        public List<Types> Interests { get; set; }
        /// <summary>
        /// Biforgation of Holiday by Frequency of travel  such as First Time ,Been there before etc..Values may be retrived from Master Service. 
        /// </summary>
        public List<string> TravelFrequency { get; set; }
        /// <summary>
        /// Rating based on entire holiday.
        /// </summary>
        public string Rating { get; set; }
        /// <summary>
        /// This field gives you pace of holiday such as Relaxing,Moderate etc..
        /// </summary>
        public List<string> PaceofHoliday { get; set; }
        /// <summary>
        /// This field gives you different stay Types available for holiday.e.g Hotels,Villas,Homestays etc..
        /// </summary>
        public List<string> StayType { get; set; }
        /// <summary>
        /// Number of days included in holiday.( e.g 3 days)
        /// </summary>
        public string Days { get; set; }
        /// <summary>
        /// Number of nights included in holiday.(e.g 2 Nights)
        /// </summary>
        public string Nights { get; set; }
        /// <summary>
        /// Total duration of holiday( e.g 3 days this field takes the total no. of days of Holiday as duration).
        /// </summary>
        public string Duration { get; set; }
        /// <summary>
        /// Starting price of the holiday
        /// </summary>
        public List<StartingPrice> StartingPrice { get; set; }
        /// <summary>
        /// Comfort level of the holiday.
        /// </summary>
        public string ComfortLevel { get; set; }
        /// <summary>
        /// Is this Holiday recommended.
        /// </summary>
        public string IsRecommended { get; set; }
        /// <summary>
        /// Different destinations included in holiday.
        /// </summary>
        public List<Destinations> Destinations { get; set; }
        /// <summary>
        /// what is included in the holiday. values are'Y' /'N'.
        /// </summary>
        public HolidayIncludes HolidayIncludes { get; set; }
        /// <summary>
        /// Main attractions for holiday.
        /// </summary>
        public List<UniqueSellingPoints> UniqueSellingPoints { get; set; }
        /// <summary>
        /// Information about Highlights of the holiday.
        /// </summary>
        public List<Highlights> Highlights { get; set; }
        /// <summary>
        /// Brand information of holiday.
        /// </summary>
        public BrandsAndBrochures Brands { get; set; }
        /// <summary>
        /// Documment of details of holiday
        /// </summary>
        public List<BrandsAndBrochures> Brochures { get; set; }
        /// <summary>
        /// Information about images,videos for holidays
        /// </summary>
        public List<Media> Media { get; set; }
        /// <summary>
        /// What is included in holiday.
        /// </summary>
        public List<InclusionExclusion> Inclusions { get; set; }
        /// <summary>
        /// What is excluded from package.
        /// </summary>
        public List<InclusionExclusion> Exclusions { get; set; }
        /// <summary>
        /// Terms and conditions for Holiday.
        /// </summary>
        public List<TermsConditions> TermsConditions { get; set; }
        /// <summary>
        /// policies about booking of holiday
        /// </summary>
        public List<TermsConditions> BookingPolicy { get; set; }
        /// <summary>
        /// Key notes about holiday.
        /// </summary>
        public List<TermsConditions> TourNotes { get; set; }
        /// <summary>
        /// Information about Starting cities for holiday
        /// </summary>
        public List<HubDetails> HubDetails { get; set; }
        /// <summary>
        /// Day wise itinerary information.
        /// </summary>
        public List<DayWiseItineraries> DayWiseItineraries { get; set; }
        /// <summary>
        /// Details about Accomodations provided in Holiday.
        /// </summary>
        public List<Accommodation> Accommodation { get; set; }
        /// <summary>
        /// Details about Sightseeings/Activities included in Holiday.
        /// </summary>
        public List<HolidayActivity> Activity { get; set; }
        /// <summary>
        /// Flight Details for Holiday.
        /// </summary>
        public List<Flights> Flights { get; set; }
        /// <summary>
        /// Departure Details for Holiday.
        /// </summary>
        public List<Departure> Departure { get; set; }
        /// <summary>
        /// Pre Tour Details.
        /// </summary>
        public List<PrePost> Pre { get; set; }
        /// <summary>
        /// Post Tour Details.
        /// </summary>
        public List<PrePost> Post { get; set; }
        /// <summary>
        /// Post Tour Details.
        /// </summary>
        public List<Optionals> Optionals { get; set; }
        /// <summary>
        /// Informatio about Extension to holiday.
        /// </summary>
        public List<Extension> Extension { get; set; }
        /// <summary>
        /// Information about Extension to holiday.
        /// </summary>
        public List<Review> Review { get; set; }
        ///// <summary>
        ///// Detailed information about Price  for holiday.
        ///// </summary>
        //public List<Price> Price { get; set; }


    }
}