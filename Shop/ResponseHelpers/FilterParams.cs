
using Microsoft.AspNetCore.Mvc;
using System;

namespace Shop.ResponseHelpers
{
    public abstract class FilterParams
    {
        /// <summary>
        /// String used to search for products which contains it
        /// </summary>
        /// <example>'Asus'</example>
        [FromQuery(Name = "searchString")]
        public string SearchString { get; set; } = "";

        /// <summary>
        /// String that defines sorting type.
        /// Can have 2 values: 'byName' or 'byPrice'
        /// </summary>
        [FromQuery(Name = "sort")]
        public string Sort { get; set; }

        /// <summary>
        /// String that defines sorting order direction
        /// True - ascending order , False -  descending order
        /// </summary>
        [FromQuery(Name = "sortDirection")]
        public bool SortDirection { get; set; } = true;

        public static bool HasSort(FilterParams filterParams)
        {
            if (filterParams.Sort != null &&
                (string.Equals(SortingTypes.ByName, filterParams.Sort, StringComparison.CurrentCultureIgnoreCase) ||
                string.Equals(SortingTypes.ByPrice, filterParams.Sort, StringComparison.CurrentCultureIgnoreCase)))
                return true;
            return false;
        }
        internal static bool HasSearchString(FilterParams filterParams)
        {
            if (filterParams.SearchString != null && filterParams.SearchString.Length > 1)
                return true;
            return false;
        }
    }

    public static class SortingTypes
    {
        public static readonly string ByName = "byName";
        public static readonly string ByPrice = "byPrice";
    }
}
