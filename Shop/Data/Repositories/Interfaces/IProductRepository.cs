using Shop.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shop.Data.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProductsWthCategoriesAsync();

    }
}
