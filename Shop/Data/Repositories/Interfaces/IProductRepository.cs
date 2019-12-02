using Shop.Models;
using System.Collections.Generic;

namespace Shop.Data.Repositories
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetProductsWthCategories();

    }
}
