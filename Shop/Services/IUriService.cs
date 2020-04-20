using Shop.ResponseHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Services
{
    public interface IUriService
    {
        Uri GetAllProductsUri(PaginationQuery paginationQuery);
    }
}
