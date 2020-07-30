using Shop.Data.Repositories.Interfaces;
using System.Threading.Tasks;

namespace Shop.Data.Repositories
{
    public interface IUnitOfWork
    {
        IProductRepository Products { get; }
        ICategoryRepository Categories { get; }
        IOrderRepository Orders { get; }
        IOrderStatusRepository OrderStatuses { get; }
        Task<int> CompleteAsync();
    }
}
