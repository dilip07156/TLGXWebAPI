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
        /// <summary>
        /// Supplier Name Unique For Filter
        /// </summary>
        public string Supplier { get; set; }
        /// <summary>
        /// Unique Country Name For Filter
        /// </summary>

        public string Country { get; set; }
        /// <summary>
        /// Unique City Code For Filter
        /// </summary>

        public string CityCode { get; set; }
        /// <summary>
        /// Unique Holiday Name For Filter Criteria
        /// </summary>

        public string HolidayName { get; set; }
        /// <summary>
        /// Holiday Flavour name For Filter
        /// </summary>

        public string HolidayFlavour { get; set; }
        /// <summary>
        /// Checkbox for checking IsHoliday having caustomized packages.
        /// </summary>

        public bool CustomisedPackages { get; set; }
        /// <summary>
        /// Checkbox for checking IsHoliday having prebuilt packages.
        /// </summary>

        public bool PreBuiltPackages { get; set; }
        /// <summary>
        /// This Property used for checking mapping status for Holiday Mapping data.
        /// </summary>

        public string MappingStatus { get; set; }
        /// <summary>
        /// This flag used for checking IsHoliday traveller type missing.
        /// </summary>

        public bool IsTravellerTypeMissing { get; set; }
        /// <summary>
        /// This flag is used for checking IsHoliday Interest missing.
        /// </summary>

        public bool IsInterestsMissing { get; set; }
        /// <summary>
        /// This flag is used for checking IsHoliday travel frequency missing.
        /// </summary>

        public bool IsTravelFrequencyMissing { get; set; }
        /// <summary>
        /// This flag is used for IsHoliday stay type missing.
        /// </summary>
        public bool IsStayTypeMissing { get; set; }
        /// <summary>
        /// This flag is used for IsHoliday Comfort level Missing.
        /// </summary>

        public bool IsComfortLevelMissing { get; set; }
        /// <summary>
        /// This flag is used for IsHoliday USP Missing.
        /// </summary>

        public bool IsUSPMissing { get; set; }
        /// <summary>
        /// This flag is used for IsHoliday Pace of Holiday Missing.
        /// </summary>

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