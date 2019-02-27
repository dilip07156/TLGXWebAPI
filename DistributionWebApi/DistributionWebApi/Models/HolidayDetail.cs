using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistributionWebApi.Models
{
    /// <summary>
    /// Model for HolidayDetail
    /// </summary>
    [BsonIgnoreExtraElements]
    public class HolidayDetail
    {
        /// <summary>
        /// Supplier Name
        /// </summary>
        public string SupplierName { get; set; }
        /// <summary>
        /// Call Type
        /// </summary>
        public string CallType { get; set; }
        /// <summary>
        /// Date
        /// </summary>
        public DateTime? Date { get; set; }
        /// <summary>
        /// Call Details
        /// </summary>
        public CallDetails CallDetails { get; set; }
    }

    /// <summary>
    /// CallDetails
    /// </summary>
    [BsonIgnoreExtraElements]
    public class CallDetails
    {
        /// <summary>
        /// CallID
        /// </summary>
        public int CallID { get; set; }
        /// <summary>
        /// SupplierResponse
        /// </summary>
        public string SupplierResponse { get; set; }
    }
   
}