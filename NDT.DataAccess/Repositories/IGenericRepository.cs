using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NDT.DataAccess.Repositories
{
    public interface IGenericRepository<T, TKey> where T : class
    {
        Task<T?> GetByIdAsync(TKey id);
        Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            params Expression<Func<T, object>>[] includes
        );
        Task<T?> FirstOrDefaultAsync(
            Expression<Func<T, bool>> predicate,
            params Expression<Func<T, object>>[] includes
        );

        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(TKey id);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
    }
}
