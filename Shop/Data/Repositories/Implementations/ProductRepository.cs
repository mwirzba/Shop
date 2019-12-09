﻿using Microsoft.EntityFrameworkCore;
using Shop.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shop.Data.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context){ }

        public async Task<IEnumerable<Product>> GetProductsWthCategoriesAsync()
        {
            return await ApplicationDbContext.Products.Include(a => a.Category).ToListAsync();
        }

        public async Task<Product> GetProductWthCategorieAsync(int id)
        {
            return await ApplicationDbContext.Products.
                Include(a => a.Category).
                FirstOrDefaultAsync(p=>p.Id == id);
        }

        public ApplicationDbContext ApplicationDbContext
        {
            get { return context as ApplicationDbContext; }
        }
    }
}