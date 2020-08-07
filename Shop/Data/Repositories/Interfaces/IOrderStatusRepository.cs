using Shop.Models;

namespace Shop.Data.Repositories.Interfaces
{
    public interface IOrderStatusRepository : IRepository<OrderStatus, int>
    {
        OrderStatus Get(int id);
    }
}
