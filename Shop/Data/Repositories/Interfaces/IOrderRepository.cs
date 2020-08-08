using Shop.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shop.Data.Repositories.Interfaces
{
    public interface IOrderRepository : IRepository<Order,long>
    {
        Task<IEnumerable<Order>> GetOrdersWithLines();
        Task<Order> GetOrderWithLines(long id);
        Task<Order> GetFullOrder(long id);
        Task<IEnumerable<Order>> GetFullOrders(long id);
        Task<IEnumerable<Order>> GetUserOrders(string userId);
    }
}
