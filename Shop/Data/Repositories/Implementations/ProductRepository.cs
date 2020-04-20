using Microsoft.EntityFrameworkCore;
using Shop.Models;
using Shop.Respn;
using Shop.ResponseHelpers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Data.Repositories
{
    public class ProductRepository : Repository<Product,int>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context){ }

        public async Task<IEnumerable<Product>> GetProductsWthCategoriesAsync()
        {
            return await ApplicationDbContext.Products.Include(a => a.Category).ToListAsync();
        }
        
        public async Task<IEnumerable<Product>> GetPagedProductsWthCategoriesAsync(PaginationQuery paginationQuery)
        {
            return await ApplicationDbContext.Products.Include(a => a.Category)
                                .Skip((paginationQuery.PageNumber - 1) * paginationQuery.PageSize)
                                .Take(paginationQuery.PageSize)
                                .ToListAsync();
        }

        public async Task<PagedList<Product>> GetPagedProductsAsync(PaginationQuery pagination)
        {
            var products = ApplicationDbContext.Products.OrderBy(on => on.Name);
            return await PagedList<Product>.ToPagedList(products,pagination.PageNumber, pagination.PageSize);
        }

        public ApplicationDbContext ApplicationDbContext
        {
            get { return context as ApplicationDbContext; }
        }
    }
}