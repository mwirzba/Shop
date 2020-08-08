using Shop.Models;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System;

namespace Shop.Data.Repositories
{
    public class CategoryRepository : Repository<Category,int>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context) : base(context) { }

        public ApplicationDbContext ApplicationDbContext
        {
            get { return context as ApplicationDbContext; }
        }

        public Category SingleOrDefault(Expression<Func<Category, bool>> predicate)
        {
            return ApplicationDbContext.Categories.FirstOrDefault(predicate);
        }
    }
}
