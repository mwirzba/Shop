﻿using Shop.Data.Repositories.Implementations;
using Shop.Data.Repositories.Interfaces;
using Shop.Models;
using System;
using System.Threading.Tasks;

namespace Shop.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ApplicationDbContext _context;
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Products = new ProductRepository(_context);
            Categories = new CategoryRepository(_context);
            Orders = new OrderRepository(_context);
            OrderStatuses = new OrderStatusRepository(_context);
        }
        public IProductRepository Products { get; }

        public ICategoryRepository Categories { get; }

        public IOrderRepository Orders { get; }

        public IOrderStatusRepository OrderStatuses { get; }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
            //_context.Dispose();
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            disposed = true;
        }

       
    }
}
