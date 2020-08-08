using Microsoft.EntityFrameworkCore;
using Shop.Data.Repositories.Interfaces;
using Shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Data.Repositories.Implementations
{
    public class OrderRepository: Repository<Order,long>, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext context) : base(context) { }
        public ApplicationDbContext ApplicationDbContext
        {
            get { return context as ApplicationDbContext; }
        }

        public async Task<IEnumerable<Order>> GetOrdersWithLines()
        {
            return await ApplicationDbContext.Orders
                .Include(o => o.CartLines)
                .ThenInclude(p => p.Product)
                .ToListAsync();
        }
        
        public async Task<Order> GetOrderWithLines(long id)
        {
            return await ApplicationDbContext.Orders
                .Include(o => o.CartLines)
                .ThenInclude(p => p.Product)
                .FirstAsync(o => o.Id == id);
        }
        
        public async Task<Order> GetFullOrder(long id)
        {
            return await ApplicationDbContext.Orders
               .Include(o => o.CartLines)
               .ThenInclude(p => p.Product)
               .ThenInclude(c => c.Category)
               .Include(o => o.Status)
               .FirstAsync(o => o.Id == id);
        }

        public async Task<IEnumerable<Order>> GetFullOrders(long id)
        {
            return await ApplicationDbContext.Orders
               .Include(o => o.CartLines)
               .ThenInclude(p => p.Product)
               .Include(o => o.Status)
               .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetUserOrders(string userId)
        {
            return await ApplicationDbContext.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.CartLines)
                .ThenInclude(p => p.Product)
                .Include(o => o.Status)
                .ToListAsync();
        }
    }
}
 