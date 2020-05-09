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
        Task<PagedList<Product>> GetPagedProductsWthCategoriesAsync(PaginationQuery pagination);
        Task<PagedList<Product>> GetPagedProductsWthCategoriesBySearchStringAsync(PaginationQuery pagination,string searchString);

        Task<PagedList<Product>> GetPagedProductsWthCategoriesByFiltersAsync(PaginationQuery pagination, FilterParams filterParams);
        Task<Product> GetProductWthCategorieAsync(int id);
    }
}
