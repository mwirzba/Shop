using System;
using System.Threading.Tasks;

namespace Shop.Data.Repositories
{
    public interface IUnitOfWork
    {
        IProductRepository Products { get; }
        ICategoryRepository Categories { get; }
        Task<int> CompleteAsync();
        
    }
}
