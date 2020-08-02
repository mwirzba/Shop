using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.ResponseHelpers
{
    /// <summary>
    /// Data passed in query used for filtering products list 
    /// </summary>
    public class FilterProductParams : FilterParams
    {
        /// <summary>
        /// Max price of product
        /// </summary>
        [FromQuery(Name = "maxPrice")]
        public long MaxPrice { get; set; }

        /// <summary>
        /// Minimum price of product
        /// </summary>
        [FromQuery(Name = "minPrice")]
        public long MinPrice { get; set; }

        public static bool HasValidPriceRange(FilterProductParams filterParams)
        {
            if (filterParams.MaxPrice > filterParams.MinPrice && filterParams.MaxPrice != 0)
                return true;
            return false;
        }

        public static bool HasSearchStringAndValidPriceRange(FilterProductParams filterParams)
        {
            if (filterParams.SearchString != null && filterParams.SearchString.Length > 1 && filterParams.MaxPrice >= filterParams.MinPrice)
                return true;
            return false;
        }

    }

}
