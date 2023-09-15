using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TestTemplate6.Common.Base;

namespace TestTemplate6.Common.Interfaces
{
    public interface IRepository<T>
        where T : BaseEntity<Guid>
    {
        Task<T> GetByIdAsync(Guid id, bool isTracked = true);
        Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate, bool isTracked = true);
        Task<IEnumerable<T>> ListAllAsync(Expression<Func<T, bool>> predicate = null, bool isTracked = true);
        Task<bool> ExistsAsync(Guid id);
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Delete(IEnumerable<T> entities);
        Task<bool> ExistsAsync(IEnumerable<Guid> ids);
        Task<int> CountAsync(Expression<Func<T, bool>> predicate = null);
    }
}
