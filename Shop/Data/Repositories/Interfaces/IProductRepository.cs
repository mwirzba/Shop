using Shop.Models;
using Shop.Respn;
using Shop.ResponseHelpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shop.Data.Repositories
{
    public interface IProductRepository :IRepository<Product,int>
    {
        Task<IEnumerable<Product>> GetProductsWthCategoriesAsync();
        Task<PagedList<Product>> GetPagedProductsAsync(PaginationQuery pagination);
        Task<Product> GetProductWthCategorieAsync(int id);
    }
}
