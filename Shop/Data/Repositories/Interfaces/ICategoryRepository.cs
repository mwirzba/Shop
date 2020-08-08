using Shop.Models;
using System;
using System.Linq.Expressions;

namespace Shop.Data.Repositories
{
    public interface ICategoryRepository : IRepository<Category,int>
    {
       Category SingleOrDefault(Expression<Func<Category, bool>> predicate);
    }
}
