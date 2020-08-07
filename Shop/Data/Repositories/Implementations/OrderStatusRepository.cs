
using AutoMapper.Configuration;
using Shop.Data.Repositories.Interfaces;
using Shop.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Data.Repositories.Implementations
{
    public class OrderStatusRepository : Repository<OrderStatus, int> , IOrderStatusRepository
    {
        public OrderStatusRepository(ApplicationDbContext context) : base(context) { }
        public ApplicationDbContext ApplicationDbContext
        {
            get { return context as ApplicationDbContext; }
        }

        public OrderStatus Get(int id)
        {
            return ApplicationDbContext.OrderStatuses.FirstOrDefault(os => os.Id == id);
        }
    }
}
