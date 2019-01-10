using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistributionWebApi.Models
{
    /// <summary>
    /// Activity related masters
    /// </summary>
    [BsonIgnoreExtraElements]
    public class Visamaster
    {
        /// <summary>
        /// attribute type master
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// parent attribute type master if exists
        /// </summary>
        [BsonIgnoreIfNull]
        public string ParentType { get; set; }
        /// <summary>
        /// values of the attribute master
        /// </summary>
        public List<VisamasterValues> Values { get; set; }
    }

    /// <summary>
    /// structure for values of the attribute master
    /// </summary>
    public class VisamasterValues
    {
        /// <summary>
        /// attribute value
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// parent attribute value if exists
        /// </summary>
        [BsonIgnoreIfNull]
        public string ParentValue { get; set; }
    }
}