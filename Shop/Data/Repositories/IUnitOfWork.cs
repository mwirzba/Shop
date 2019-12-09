using System;
using System.Threading.Tasks;

namespace Shop.Data.Repositories
{
    public interface IUnitOfWork
    {
        ProductRepository Products { get; }
        CategoryRepository Categories { get; }
        Task<int> CompleteAsync();
        
    }
}
