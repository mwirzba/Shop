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

        public Product Get(int id)
        {
            return ApplicationDbContext.Products.FirstOrDefault(p=>p.Id == id);
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
            FilterProductParams filterProductParams = filterParams as FilterProductParams;


            if (filterProductParams.MaxPrice == 0)
                filterProductParams.MaxPrice = long.MaxValue;

            if (FilterProductParams.HasSearchStringAndValidPriceRange(filterProductParams))
            {

                products = products.Where(p => p.Name.Contains(filterProductParams.SearchString) && (p.Price >= filterProductParams.MinPrice && p.Price <= filterProductParams.MaxPrice));
            }
            else if (FilterProductParams.HasSearchString(filterProductParams))
            {
                products = products.Where(p => p.Name.Contains(filterProductParams.SearchString));
            }
            else if(FilterProductParams.HasValidPriceRange(filterProductParams))
            {
                products = products.Where(p => p.Price >= filterProductParams.MinPrice
                            && p.Price <= filterProductParams.MaxPrice);
            }
          
            if(FilterParams.HasSort(filterProductParams))
            {
                if (string.Equals(SortingTypes.ByName, filterProductParams.Sort,
                 System.StringComparison.CurrentCultureIgnoreCase) && filterProductParams.SortDirection == true)
                {
                    products = products.OrderBy(p => p.Name);
                }
                else if (string.Equals(SortingTypes.ByName, filterProductParams.Sort,
                    System.StringComparison.CurrentCultureIgnoreCase) && filterProductParams.SortDirection == false)
                {
                    products = products.OrderByDescending(p => p.Name);
                }

                else if (string.Equals(SortingTypes.ByPrice, filterProductParams.Sort,
                    System.StringComparison.CurrentCultureIgnoreCase) && filterProductParams.SortDirection == true)
                {
                    products = products.OrderBy(p => p.Price);
                }
                else if (string.Equals(SortingTypes.ByPrice, filterProductParams.Sort,
                    System.StringComparison.CurrentCultureIgnoreCase) && filterProductParams.SortDirection == false)
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