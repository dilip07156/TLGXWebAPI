using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DistributionWebApi.Models
{
    public class HolidaySearchRequestParams
    {

        public string Supplier { get; set; }

        public string Country { get; set; }

        public string CityCode { get; set; }

        public string HolidayName { get; set; }

        public string HolidayFlavour { get; set; }

        public bool CustomisedPackages { get; set; }

        public bool PreBuiltPackages { get; set; }

        public string MappingStatus { get; set; }

        public bool IsTravellerTypeMissing { get; set; }

        public bool IsInterestsMissing { get; set; }

        public bool IsTravelFrequencyMissing { get; set; }

        public bool IsStayTypeMissing { get; set; }

        public bool IsComfortLevelMissing { get; set; }

        public bool IsUSPMissing { get; set; }

        public bool IsPaceOfHolidayMissing { get; set; }

        /// <summary>
        /// How many Search Results do you wish to receive per request?
        /// Default Value = 10
        /// </summary>
        [Required]
        [DefaultValue(10)]
        [Range(1, 100, ErrorMessage = "PageSize must be between 1 to 100")]
        public int PageSize { get; set; } = 10;
        /// <summary>
        /// Which Page Number you wish to retrieve from the Search Results set
        /// </summary>
        [Required]
        public int PageNo { get; set; }

    }
}