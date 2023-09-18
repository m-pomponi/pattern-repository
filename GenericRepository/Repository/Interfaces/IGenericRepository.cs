using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace GenericRepository.Repository.Interfaces
{
    public interface IGenericRepository<T> 
        where T : class
    {
        T Add(T t, bool commit = true);
        Task<T> AddAsync(T t, bool commit = true);
        int Count();
        Task<int> CountAsync();
        void Delete(T entity, bool commit = true);
        Task<int> DeleteAsync(T entity, bool commit = true);
        void Dispose();
        T Find(Expression<Func<T, bool>> match);
        ICollection<T> FindAll(Expression<Func<T, bool>> match);
        Task<ICollection<T>> FindAllAsync(params Expression<Func<T, object>>[] includeProperties);
        Task<T> FindAsync(Expression<Func<T, bool>> match, params Expression<Func<T, object>>[] includeProperties);
        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);
        Task<ICollection<T>> FindByAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
        T Get(int id);
        IQueryable<T> GetAll();
        Task<ICollection<T>> GetAllAsync();
        IQueryable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties);
        Task<T> GetByIdAsync(int id);
        T GetByIdInclude(int id, params Expression<Func<T, object>>[] includes);
        T Update(T t, bool commit = true, params object[] keys);
        Task<T> UpdateAsync(T t, bool commit = true, params object[] keys);
        Task<ICollection<T>> FindThanIncludeByAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, object>> includeProperties = null);
        IQueryable<T> FindThanIncludeByAsyncQueryable(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, object>> includeProperties = null);
        Task<bool> AnyAsync();
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
        Task TruncateAsync(bool commit = true);
        Task<int> UpdateValueAsync(T entity, Func<T, bool> equalsExpression, bool commit = true, params Expression<Func<T, object>>[] properties);
        Task<int> RemoveRangeAsync(IEnumerable<T> items, bool commit = true);

    }
}
