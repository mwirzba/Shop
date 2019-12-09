using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Data
{
    public static class DbSetExtensions
    {
        public static async Task<IEnumerable<TEntity>> WhereAsync<TEntity>(this DbSet<TEntity> source,Func<TEntity, Task<bool>> predicate) where TEntity : class
        {
            var results = new ConcurrentQueue<TEntity>();
            var sourceResult = await source.ToListAsync();
            var tasks = sourceResult.Select(
                async x =>
                {
                    if (await predicate(x))
                        results.Enqueue(x);
                });
            await Task.WhenAll(tasks);
            return results;
        }
    }
}
