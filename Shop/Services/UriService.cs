using Microsoft.AspNetCore.WebUtilities;
using Shop.ResponseHelpers;
using System;

namespace Shop.Services
{
    public class UriService : IUriService
    {
        private readonly string _baseUri;
        public UriService(string baseUri)
        {
            _baseUri = baseUri;
        }

        public Uri GetAllProductsUri(PaginationQuery paginationQuery = null)
        {
            var uri = new Uri(_baseUri);

            if (paginationQuery == null)
            {
                return uri;
            }

            var modUri = _baseUri + "api/products";
            modUri = QueryHelpers.AddQueryString(modUri, "pageNumber", paginationQuery.PageNumber.ToString());
            modUri = QueryHelpers.AddQueryString(modUri, "pageSize", paginationQuery.PageSize.ToString());

            return new Uri(modUri);
        }
    }
}
