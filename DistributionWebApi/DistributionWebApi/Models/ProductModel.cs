using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Web.Http.Description;

namespace DistributionWebApi.Models
{
    public class ProductModel
    {
    }
    
    [BsonIgnoreExtraElements]
    [XmlRoot(ElementName = "Hotels",Namespace = "")]
    [XmlTypeAttribute(AnonymousType = true)]
    public class Hotels
    {

        private List<HotelsHotel> hotelField;

        [XmlElement(ElementName = "Hotel")]
        public List<HotelsHotel> Hotel
        {
            get
            {
                return this.hotelField;
            }
            set
            {
                this.hotelField = value;
            }
        }
    }

    [BsonIgnoreExtraElements]
    [XmlRoot(ElementName = "Hotel", Namespace = "")]
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class HotelsHotel
    {

        private HotelsHotelStarRating starRatingField;

        private HotelsHotelAddress addressField;

        private HotelsHotelImage[] imageField;

        //private object videoField;

        private HotelsHotelFacility[] hotelFacilityField;

        private HotelsHotelHotelAmenity hotelAmenityField;

        private HotelsHotelHotelDistance hotelDistanceField;

        private string supplierHotelIDField;

        private string hotelIdField;

        private string nameField;

        private string credicardsField;

        private string areatransportationField;

        private string restaurantsField;

        private string meetingfacilityField;

        private string descriptionField;

        private string highlightField;

        private string overviewField;

        private string checkintimeField;

        private string checkouttimeField;

        private string emailField;

        private string websiteField;

        private string roomsField;

        private string landmarkCategoryField;

        private string landmarkField;

        private string themeField;

        private string hotelChainField;

        private string brandNameField;

        private string recommendsField;

        private string latitudeField;

        private string longitudeField;

        private string landmarkDescriptionField;

        private string thumbField;

        [XmlElement(ElementName = "StarRating")]
        public HotelsHotelStarRating StarRating
        {
            get
            {
                return this.starRatingField;
            }
            set
            {
                this.starRatingField = value;
            }
        }

        [XmlElement(ElementName = "Address")]
        public HotelsHotelAddress Address
        {
            get
            {
                return this.addressField;
            }
            set
            {
                this.addressField = value;
            }
        }

        [XmlElement(ElementName = "image")]
        public HotelsHotelImage[] image
        {
            get
            {
                return this.imageField;
            }
            set
            {
                this.imageField = value;
            }
        }

        //public object video
        //{
        //    get
        //    {
        //        return this.videoField;
        //    }
        //    set
        //    {
        //        this.videoField = value;
        //    }
        //}

        [XmlElement(ElementName = "HotelFacility")]
        public HotelsHotelFacility[] HotelFacility
        {
            get
            {
                return this.hotelFacilityField;
            }
            set
            {
                this.hotelFacilityField = value;
            }
        }

        [XmlElement(ElementName = "HotelAmenity")]
        public HotelsHotelHotelAmenity HotelAmenity
        {
            get
            {
                return this.hotelAmenityField;
            }
            set
            {
                this.hotelAmenityField = value;
            }
        }

        [XmlElement(ElementName = "HotelDistance")]
        public HotelsHotelHotelDistance HotelDistance
        {
            get
            {
                return this.hotelDistanceField;
            }
            set
            {
                this.hotelDistanceField = value;
            }
        }

        [XmlAttribute(AttributeName = "SupplierHotelID")]
        public string SupplierHotelID
        {
            get
            {
                return this.supplierHotelIDField;
            }
            set
            {
                this.supplierHotelIDField = value;
            }
        }

        [XmlAttribute(AttributeName = "HotelId")]
        public string HotelId
        {
            get
            {
                return this.hotelIdField;
            }
            set
            {
                this.hotelIdField = value;
            }
        }

        [XmlAttribute(AttributeName = "name")]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        [XmlAttribute(AttributeName = "credicards")]
        public string credicards
        {
            get
            {
                return this.credicardsField;
            }
            set
            {
                this.credicardsField = value;
            }
        }

        [XmlAttribute(AttributeName = "areatransportation")]
        public string areatransportation
        {
            get
            {
                return this.areatransportationField;
            }
            set
            {
                this.areatransportationField = value;
            }
        }

        [XmlAttribute(AttributeName = "restaurants")]
        public string restaurants
        {
            get
            {
                return this.restaurantsField;
            }
            set
            {
                this.restaurantsField = value;
            }
        }

        [XmlAttribute(AttributeName = "meetingfacility")]
        public string meetingfacility
        {
            get
            {
                return this.meetingfacilityField;
            }
            set
            {
                this.meetingfacilityField = value;
            }
        }

        [XmlAttribute(AttributeName = "description")]
        public string description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        [XmlAttribute(AttributeName = "highlight")]
        public string highlight
        {
            get
            {
                return this.highlightField;
            }
            set
            {
                this.highlightField = value;
            }
        }

        [XmlAttribute(AttributeName = "overview")]
        public string overview
        {
            get
            {
                return this.overviewField;
            }
            set
            {
                this.overviewField = value;
            }
        }

        [XmlAttribute(AttributeName = "checkintime")]
        public string checkintime
        {
            get
            {
                return this.checkintimeField;
            }
            set
            {
                this.checkintimeField = value;
            }
        }

        [XmlAttribute(AttributeName = "checkouttime")]
        public string checkouttime
        {
            get
            {
                return this.checkouttimeField;
            }
            set
            {
                this.checkouttimeField = value;
            }
        }

        [XmlAttribute(AttributeName = "email")]
        public string email
        {
            get
            {
                return this.emailField;
            }
            set
            {
                this.emailField = value;
            }
        }

        [XmlAttribute(AttributeName = "website")]
        public string website
        {
            get
            {
                return this.websiteField;
            }
            set
            {
                this.websiteField = value;
            }
        }

        [XmlAttribute(AttributeName = "rooms")]
        public string rooms
        {
            get
            {
                return this.roomsField;
            }
            set
            {
                this.roomsField = value;
            }
        }

        [XmlAttribute(AttributeName = "LandmarkCategory")]
        public string LandmarkCategory
        {
            get
            {
                return this.landmarkCategoryField;
            }
            set
            {
                this.landmarkCategoryField = value;
            }
        }

        [XmlAttribute(AttributeName = "Landmark")]
        public string Landmark
        {
            get
            {
                return this.landmarkField;
            }
            set
            {
                this.landmarkField = value;
            }
        }

        [XmlAttribute(AttributeName = "theme")]
        public string theme
        {
            get
            {
                return this.themeField;
            }
            set
            {
                this.themeField = value;
            }
        }

        [XmlAttribute(AttributeName = "HotelChain")]
        public string HotelChain
        {
            get
            {
                return this.hotelChainField;
            }
            set
            {
                this.hotelChainField = value;
            }
        }

        [XmlAttribute(AttributeName = "BrandName")]
        public string BrandName
        {
            get
            {
                return this.brandNameField;
            }
            set
            {
                this.brandNameField = value;
            }
        }

        [XmlAttribute(AttributeName = "recommends")]
        public string recommends
        {
            get
            {
                return this.recommendsField;
            }
            set
            {
                this.recommendsField = value;
            }
        }

        [XmlAttribute(AttributeName = "latitude")]
        public string latitude
        {
            get
            {
                return this.latitudeField;
            }
            set
            {
                this.latitudeField = value;
            }
        }

        [XmlAttribute(AttributeName = "longitude")]
        public string longitude
        {
            get
            {
                return this.longitudeField;
            }
            set
            {
                this.longitudeField = value;
            }
        }

        [XmlAttribute(AttributeName = "LandmarkDescription")]
        public string LandmarkDescription
        {
            get
            {
                return this.landmarkDescriptionField;
            }
            set
            {
                this.landmarkDescriptionField = value;
            }
        }

        [XmlAttribute(AttributeName = "thumb")]
        public string thumb
        {
            get
            {
                return this.thumbField;
            }
            set
            {
                this.thumbField = value;
            }
        }
    }

    [XmlRoot("StarRating")]
    public partial class HotelsHotelStarRating
    {

        private string levelField;

        [XmlAttribute(AttributeName = "Level")]
        public string Level
        {
            get
            {
                return this.levelField;
            }
            set
            {
                this.levelField = value;
            }
        }
    }

    [XmlRoot("Address")]
    public class HotelsHotelAddress
    {

        private string addressField;

        private string cityField;

        private string stateField;

        private string countryField;

        private string pincodeField;

        private string locationField;

        private string phoneField;

        private string faxField;

        [XmlAttribute(AttributeName = "address")]
        public string address
        {
            get
            {
                return this.addressField;
            }
            set
            {
                this.addressField = value;
            }
        }

        [XmlAttribute(AttributeName = "city")]
        public string city
        {
            get
            {
                return this.cityField;
            }
            set
            {
                this.cityField = value;
            }
        }

        [XmlAttribute(AttributeName = "state")]
        public string state
        {
            get
            {
                return this.stateField;
            }
            set
            {
                this.stateField = value;
            }
        }

        [XmlAttribute(AttributeName = "country")]
        public string country
        {
            get
            {
                return this.countryField;
            }
            set
            {
                this.countryField = value;
            }
        }

        [XmlAttribute(AttributeName = "pincode")]
        public string pincode
        {
            get
            {
                return this.pincodeField;
            }
            set
            {
                this.pincodeField = value;
            }
        }

        [XmlAttribute(AttributeName = "location")]
        public string location
        {
            get
            {
                return this.locationField;
            }
            set
            {
                this.locationField = value;
            }
        }

        [XmlAttribute(AttributeName = "phone")]
        public string phone
        {
            get
            {
                return this.phoneField;
            }
            set
            {
                this.phoneField = value;
            }
        }

        [XmlAttribute(AttributeName = "fax")]
        public string fax
        {
            get
            {
                return this.faxField;
            }
            set
            {
                this.faxField = value;
            }
        }
    }

    [XmlRoot("image")]
    public class HotelsHotelImage
    {

        private string pathField;

        [XmlAttribute(AttributeName = "path")]
        public string path
        {
            get
            {
                return this.pathField;
            }
            set
            {
                this.pathField = value;
            }
        }
    }

    [XmlRoot("HotelFacility")]
    public class HotelsHotelFacility
    {

        private string nameField;

        [XmlAttribute(AttributeName = "name")]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }

    [XmlRoot("HotelAmenity")]
    public class HotelsHotelHotelAmenity
    {

        private bool restaurantField;

        private bool conferenceField;

        private bool fitnessField;

        private bool travelField;

        private bool forexField;

        private bool shoppingField;

        private bool banquetField;

        private bool gamesField;

        private bool barField;

        private bool coffee_ShopField;

        private bool room_ServiceField;

        private bool internet_AccessField;

        private bool business_CentreField;

        private bool swimming_PoolField;

        private bool petsField;

        private bool tennis_CourtField;

        private bool golfField;

        private bool air_ConditioningField;

        private bool parkingField;

        private bool wheel_ChairField;

        private bool health_ClubField;

        [XmlAttribute(AttributeName = "Restaurant")]
        public bool Restaurant
        {
            get
            {
                return this.restaurantField;
            }
            set
            {
                this.restaurantField = value;
            }
        }

        [XmlAttribute(AttributeName = "conference")]
        public bool conference
        {
            get
            {
                return this.conferenceField;
            }
            set
            {
                this.conferenceField = value;
            }
        }

        [XmlAttribute(AttributeName = "fitness")]
        public bool fitness
        {
            get
            {
                return this.fitnessField;
            }
            set
            {
                this.fitnessField = value;
            }
        }

        [XmlAttribute(AttributeName = "travel")]
        public bool travel
        {
            get
            {
                return this.travelField;
            }
            set
            {
                this.travelField = value;
            }
        }

        [XmlAttribute(AttributeName = "forex")]
        public bool forex
        {
            get
            {
                return this.forexField;
            }
            set
            {
                this.forexField = value;
            }
        }

        [XmlAttribute(AttributeName = "shopping")]
        public bool shopping
        {
            get
            {
                return this.shoppingField;
            }
            set
            {
                this.shoppingField = value;
            }
        }

        [XmlAttribute(AttributeName = "banquet")]
        public bool banquet
        {
            get
            {
                return this.banquetField;
            }
            set
            {
                this.banquetField = value;
            }
        }

        [XmlAttribute(AttributeName = "games")]
        public bool games
        {
            get
            {
                return this.gamesField;
            }
            set
            {
                this.gamesField = value;
            }
        }

        [XmlAttribute(AttributeName = "Bar")]
        public bool Bar
        {
            get
            {
                return this.barField;
            }
            set
            {
                this.barField = value;
            }
        }

        [XmlAttribute(AttributeName = "Coffee_Shop")]
        public bool Coffee_Shop
        {
            get
            {
                return this.coffee_ShopField;
            }
            set
            {
                this.coffee_ShopField = value;
            }
        }

        [XmlAttribute(AttributeName = "Room_Service")]
        public bool Room_Service
        {
            get
            {
                return this.room_ServiceField;
            }
            set
            {
                this.room_ServiceField = value;
            }
        }

        [XmlAttribute(AttributeName = "Internet_Access")]
        public bool Internet_Access
        {
            get
            {
                return this.internet_AccessField;
            }
            set
            {
                this.internet_AccessField = value;
            }
        }

        [XmlAttribute(AttributeName = "Business_Centre")]
        public bool Business_Centre
        {
            get
            {
                return this.business_CentreField;
            }
            set
            {
                this.business_CentreField = value;
            }
        }

        [XmlAttribute(AttributeName = "Swimming_Pool")]
        public bool Swimming_Pool
        {
            get
            {
                return this.swimming_PoolField;
            }
            set
            {
                this.swimming_PoolField = value;
            }
        }

        [XmlAttribute(AttributeName = "Pets")]
        public bool Pets
        {
            get
            {
                return this.petsField;
            }
            set
            {
                this.petsField = value;
            }
        }

        [XmlAttribute(AttributeName = "Tennis_Court")]
        public bool Tennis_Court
        {
            get
            {
                return this.tennis_CourtField;
            }
            set
            {
                this.tennis_CourtField = value;
            }
        }

        [XmlAttribute(AttributeName = "Golf")]
        public bool Golf
        {
            get
            {
                return this.golfField;
            }
            set
            {
                this.golfField = value;
            }
        }

        [XmlAttribute(AttributeName = "Air_Conditioning")]
        public bool Air_Conditioning
        {
            get
            {
                return this.air_ConditioningField;
            }
            set
            {
                this.air_ConditioningField = value;
            }
        }

        [XmlAttribute(AttributeName = "Parking")]
        public bool Parking
        {
            get
            {
                return this.parkingField;
            }
            set
            {
                this.parkingField = value;
            }
        }

        [XmlAttribute(AttributeName = "Wheel_Chair")]
        public bool Wheel_Chair
        {
            get
            {
                return this.wheel_ChairField;
            }
            set
            {
                this.wheel_ChairField = value;
            }
        }

        [XmlAttribute(AttributeName = "Health_Club")]
        public bool Health_Club
        {
            get
            {
                return this.health_ClubField;
            }
            set
            {
                this.health_ClubField = value;
            }
        }
    }

    [XmlRoot("HotelDistance")]
    public class HotelsHotelHotelDistance
    {

        private string distancefromAirportField;

        private string distancefromStationField;

        private string distancefromBusField;

        private string distancefromCityCenterField;

        [XmlAttribute(AttributeName = "DistancefromAirport")]
        public string DistancefromAirport
        {
            get
            {
                return this.distancefromAirportField;
            }
            set
            {
                this.distancefromAirportField = value;
            }
        }

        [XmlAttribute(AttributeName = "DistancefromStation")]
        public string DistancefromStation
        {
            get
            {
                return this.distancefromStationField;
            }
            set
            {
                this.distancefromStationField = value;
            }
        }

        [XmlAttribute(AttributeName = "DistancefromBus")]
        public string DistancefromBus
        {
            get
            {
                return this.distancefromBusField;
            }
            set
            {
                this.distancefromBusField = value;
            }
        }

        [XmlAttribute(AttributeName = "DistancefromCityCenter")]
        public string DistancefromCityCenter
        {
            get
            {
                return this.distancefromCityCenterField;
            }
            set
            {
                this.distancefromCityCenterField = value;
            }
        }
    }

}