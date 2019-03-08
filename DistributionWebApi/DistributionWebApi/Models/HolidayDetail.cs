using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace DistributionWebApi.Models
{
    /// <summary>
    /// Model for Holiday details
    /// </summary>
    [BsonIgnoreExtraElements]
    public class HolidayDetail
    {
        /// <summary>
        /// Supplier  name
        /// </summary>
        public string SupplierName { get; set; }
        /// <summary>
        /// Call type 
        /// </summary>
        public string CallType { get; set; }

        /// <summary>
        /// contains call details
        /// </summary>
        public CallDetails CallDetails { get; set; }
    }
    
    /// <summary>
    /// Contains tour info
    /// </summary>
    [BsonIgnoreExtraElements]
    public class TourIDs
    {
        /// <summary>
        /// Tour ID
        /// </summary>
        public string TourID { get; set; }
        /// <summary>
        /// Tour Name
        /// </summary>
        public string TourName { get; set; }
    }

    /// <summary>
    /// Call Details
    /// </summary>
    [BsonIgnoreExtraElements]
    public class CallDetails
    {
        /// <summary>
        /// Supplier Response
        /// </summary>
        public string SupplierResponse { get; set; }

        /// <summary>
        /// List of tour Ids
        /// </summary>
        public List<TourIDs> TourIDs { get; set; }
    }
}