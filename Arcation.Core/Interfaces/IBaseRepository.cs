using Arcation.Core.Consts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Arcation.Core.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        /*IEnumerable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties);
        Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> predicate);
        Task<int> CountAsync();
        Task<T> GetSingle(Expression<Func<T, bool>> predicate);
        T GetSingle(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
        IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate);
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        void DeleteWhere(Expression<Func<T, bool>> predicate);
        Task CommitAsync();*/

        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();

        Task<T> FindAsync(Expression<Func<T, bool>> criteria, string[] includes = null);
        Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria, string[] includes = null, Expression<Func<T, object>> orderBy = null, string orderByDirection = OrderBy.Ascending);
        Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria, int? skip, int? take,
            Expression<Func<T, object>> orderBy = null, string orderByDirection = OrderBy.Ascending);

        Task<T> AddAsync(T entity);
        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities);

        T Update(T entity);

        void Delete(T entity);
        void DeleteWhere(Expression<Func<T, bool>> predicate);
        void DeleteRange(IEnumerable<T> entities);

        void Attach(T entity);
        void AttachRange(IEnumerable<T> entities);

        Task<int> CountAsync();
        Task<int> CountAsync(Expression<Func<T, bool>> criteria);

    }
}
