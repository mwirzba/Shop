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
        
        public async Task<PagedList<Product>> GetPagedProductsWthCategoriesAsync(PaginationQuery pagination)
        {
            var products = ApplicationDbContext.Products.Include(a => a.Category).OrderBy(on => on.Name);
            return await PagedList<Product>.ToPagedList(products, pagination.PageNumber, pagination.PageSize);
        }

        public async Task<Product> GetProductWthCategorieAsync(int id)
        {
            return await ApplicationDbContext.Products.Include(c=>c.Category)
                .FirstOrDefaultAsync(p=>p.Id == id);
        }

        public async Task<PagedList<Product>> GetPagedProductsAsync(PaginationQuery pagination)
        {
            var products = ApplicationDbContext.Products.OrderBy(on => on.Name);
            return await PagedList<Product>.ToPagedList(products,pagination.PageNumber, pagination.PageSize);
        }

        public async Task<PagedList<Product>> GetPagedProductsWthCategoriesBySearchStringAsync(PaginationQuery pagination, string searchString)
        {
            var products = ApplicationDbContext.Products
                .Include(a => a.Category)
                .Where(p=>p.Name.Contains(searchString))
                .OrderBy(on => on.Name);

            return await PagedList<Product>.ToPagedList(products, pagination.PageNumber, pagination.PageSize);
        }

        public async Task<PagedList<Product>> GetPagedProductsWthCategoriesByFiltersAsync(PaginationQuery pagination, FilterParams filterParams)
        {
            IQueryable<Product> products = ApplicationDbContext.Products
                                            .Include(a => a.Category);

            if (FilterCheckMethods.HasSearchStringAndValidPriceRange(filterParams))
            {

                products = products.Where(p => p.Name.Contains(filterParams.SearchString) 
                            && p.Price >= filterParams.MinPrice 
                            && p.Price <= filterParams.MaxPrice);
            }
            else if(FilterCheckMethods.HasValidPriceRange(filterParams))
            {
                products = products.Where(p => p.Price >= filterParams.MinPrice
                            && p.Price <= filterParams.MaxPrice);
            }
            if(FilterCheckMethods.HasSort(filterParams))
            {
                if (string.Equals(SortingTypes.ByName, filterParams.Sort,
                 System.StringComparison.CurrentCultureIgnoreCase) && filterParams.SortDirection)
                {
                    products = products.OrderBy(p => p.Name);
                }
                else if (string.Equals(SortingTypes.ByName, filterParams.Sort,
                    System.StringComparison.CurrentCultureIgnoreCase) && !filterParams.SortDirection)
                {
                    products = products.OrderByDescending(p => p.Name);
                }

                if (string.Equals(SortingTypes.ByPrice, filterParams.Sort,
                    System.StringComparison.CurrentCultureIgnoreCase) && filterParams.SortDirection)
                {
                    products = products.OrderBy(p => p.Price);
                }
                else if (string.Equals(SortingTypes.ByPrice, filterParams.Sort,
                    System.StringComparison.CurrentCultureIgnoreCase) && !filterParams.SortDirection)
                {
                    products = products.OrderByDescending(p => p.Price);
                }
            }
            else 
            {
                products = products.OrderBy(p => p.Name);
            }

            return await PagedList<Product>.ToPagedList(products, pagination.PageNumber, pagination.PageSize);
        }

        public ApplicationDbContext ApplicationDbContext
        {
            get { return context as ApplicationDbContext; }
        }
    }
}