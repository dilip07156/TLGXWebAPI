using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistributionWebApi.Models
{
    public class HolidaySearchResponse
    {
        public string NAKID { get; set; }

        public string Source { get; set; }

        public string HolidayId { get; set; }

        public string HolidayName { get; set; }

        public string FlavourId { get; set; }

        public string HolidayFlavour { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        //public bool TLGX { get; set; }

       // public bool Overlay { get; set; }


    }

    public class HolidayMappingSearchResult
    {
        /// <summary>
        /// The Total Number of Activities returned by the Search Query
        /// </summary>
        public long TotalNumberOfHolidays { get; set; }
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
        public List<HolidaySearchResponse> Holidays { get; set; }

    }
}