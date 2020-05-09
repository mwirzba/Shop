
using Microsoft.AspNetCore.Mvc;

namespace Shop.ResponseHelpers
{
    public class FilterParams
    {
        [FromQuery(Name = "searchString")]
        public string SearchString { get; set; } = "";

        [FromQuery(Name = "maxPrice")]
        public long MaxPrice { get; set; }

        [FromQuery(Name = "minPrice")]
        public long MinPrice { get; set; }

        [FromQuery(Name = "sort")]
        public string Sort { get; set; }

        [FromQuery(Name = "sortDirection")]
        public bool SortDirection { get; set; } = true;
    }

    public static class FilterCheckMethods
    {
        public static bool HasSort(FilterParams filterParams)
        {
            if (filterParams.MaxPrice >= filterParams.MinPrice
                && (string.Equals(SortingTypes.ByName, filterParams.Sort, System.StringComparison.CurrentCultureIgnoreCase) ||
                string.Equals(SortingTypes.ByName, filterParams.Sort, System.StringComparison.CurrentCultureIgnoreCase)))
                return true;
            return false;
        }

        public static bool HasValidPriceRange(FilterParams filterParams)
        {
            if (filterParams.MaxPrice >= filterParams.MinPrice)
                return true;
            return false;
        }

        public static bool HasSearchStringAndValidPriceRange(FilterParams filterParams)
        {
            if (filterParams.SearchString.Length > 1  && filterParams.MaxPrice >= filterParams.MinPrice)
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
