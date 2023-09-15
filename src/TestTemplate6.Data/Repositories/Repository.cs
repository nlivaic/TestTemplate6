using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TestTemplate6.Common.Base;
using TestTemplate6.Common.Interfaces;

namespace TestTemplate6.Data.Repositories
{
    public class Repository<T> : IRepository<T>
        where T : BaseEntity<Guid>
    {
        public Repository(TestTemplate6DbContext context)
        {
            Context = context;
        }

        protected TestTemplate6DbContext Context { get; private set; }

        public async Task<T> GetByIdAsync(Guid id, bool isTracked = true)
        {
            var q = Context.Set<T>() as IQueryable<T>;
            if (!isTracked)
            {
                q = q.AsNoTracking();
            }

            return await q.SingleOrDefaultAsync(t => t.Id == id);
        }

        public async Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate, bool isTracked = true)
        {
            var q = Context.Set<T>() as IQueryable<T>;
            if (!isTracked)
            {
                q = q.AsNoTracking();
            }

            return await q.SingleOrDefaultAsync(predicate);
        }

        public async Task<IEnumerable<T>> ListAllAsync(Expression<Func<T, bool>> predicate = null, bool isTracked = true)
        {
            var q = Context.Set<T>().AsQueryable();
            if (predicate != null)
            {
                q = q.Where(predicate);
            }

            if (!isTracked)
            {
                q = q.AsNoTracking();
            }

            return await q.ToListAsync();
        }

        public async Task<bool> ExistsAsync(Guid id) =>
            await Context.Set<T>().AnyAsync(x => x.Id == id);

        public async Task<bool> ExistsAsync(IEnumerable<Guid> ids) =>
            await Context.Set<T>().CountAsync(x => ids.Contains(x.Id)) == ids.Count();

        public async Task AddAsync(T entity) => await Context.Set<T>().AddAsync(entity);

        public void Update(T entity) => Context.Set<T>().Attach(entity).State = EntityState.Modified;

        public void Delete(T entity) => Context.Set<T>().Remove(entity);

        public void Delete(IEnumerable<T> entities) => Context.Set<T>().RemoveRange(entities);

        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate = null)
        {
            if (predicate != null)
            {
                return await Context.Set<T>().CountAsync(predicate);
            }
            else
            {
                return await Context.Set<T>().CountAsync();
            }
        }
    }
}
