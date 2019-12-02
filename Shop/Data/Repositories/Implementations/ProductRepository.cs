using Microsoft.EntityFrameworkCore;
using Shop.Models;
using System.Collections.Generic;

namespace Shop.Data.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context){ }

        public IEnumerable<Product> GetProductsWthCategories()
        {
            return ApplicationDbContext.Products.Include(a => a.Category);
        }

        public ApplicationDbContext ApplicationDbContext
        {
            get { return Context as ApplicationDbContext; }
        }
    }
}
