﻿using Shop.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shop.Data.Repositories
{
    public interface IProductRepository :IRepository<Product,int>
    {
        Task<IEnumerable<Product>> GetProductsWthCategoriesAsync();
        Task<Product> GetProductWthCategorieAsync(int id);
    }
}
