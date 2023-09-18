using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq.Expressions;
using Dapper;
using GenericRepository.Repository.Interfaces;
using GenericRepository.Extensions;

namespace GenericRepository.Repository
{
    public class GenericRepository<T> : IGenericRepository<T>
        where T : class
    {
        private readonly DbContext _context;

        public GenericRepository(DbContext context)
        {
            this._context = context;
        }

        public virtual T Add(T t, bool commit = true)
        {
            _context.Set<T>().Add(t);
            Commit(_context, commit);
            return t;
        }

        public virtual async Task<T> AddAsync(T t, bool commit = true)
        {
            _context.Set<T>().Add(t);
            await CommitAsync(_context, commit);
            return t;
        }

        public Task<bool> AnyAsync()
        {
            return _context.Set<T>().AnyAsync();
        }

        public Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().AnyAsync(predicate);
        }

        public int Count()
        {
            return _context.Set<T>().Count();
        }

        public async Task<int> CountAsync()
        {
            return await _context.Set<T>().CountAsync();
        }

        public virtual void Delete(T entity, bool commit = true)
        {
            _context.Set<T>().Remove(entity);
            Commit(_context, commit);
        }

        public virtual async Task<int> DeleteAsync(T entity, bool commit = true)
        {
            _context.Set<T>().Remove(entity);
            return await CommitAsync(_context, commit);
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public T Find(Expression<Func<T, bool>> match)
        {
            return _context.Set<T>().SingleOrDefault(match);
        }

        public ICollection<T> FindAll(Expression<Func<T, bool>> match)
        {
            return _context.Set<T>().Where(match).ToList();
        }

        public async Task<ICollection<T>> FindAllAsync(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _context.Set<T>();

            if (includeProperties.Length > 0)
            {
                query = _context.Set<T>().Include(includeProperties[0]);
                
                for (int queryIndex = 1; queryIndex < includeProperties.Length; ++queryIndex)
                {
                    query = query.Include(includeProperties[queryIndex]);
                }
            }

            return await query.ToListAsync();
        }

        public virtual async Task<T> FindAsync(Expression<Func<T, bool>> match, params Expression<Func<T, object>>[] includeProperties)
        {
            IIncludableQueryable<T, object> query = null;

            if (includeProperties.Length > 0)
            {
                query = _context.Set<T>().Include(includeProperties[0]);
                for (int queryIndex = 1; queryIndex < includeProperties.Length; ++queryIndex)
                {
                    query = query.Include(includeProperties[queryIndex]);
                }
            }
            if (query != null)
            {
                var data = await query.SingleOrDefaultAsync(match);
                return data;
            }
            else
            {
                var data = await _context.Set<T>().SingleOrDefaultAsync(match);
                return data;
            }
        }

        public virtual IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> query = _context.Set<T>().Where(predicate);
            return query;
        }

        public virtual async Task<ICollection<T>> FindByAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _context.Set<T>();

            if (includeProperties.Length > 0)
            {
                query = _context.Set<T>().Include(includeProperties[0]);

                for (int queryIndex = 1; queryIndex < includeProperties.Length; ++queryIndex)
                {
                    if (includeProperties[queryIndex].Body.Type == typeof(System.String))
                    {
                        var value = includeProperties[queryIndex].Body.ToString().Replace("\"", "");
                        query = query.Include(value);
                    }
                    else
                    {
                        query = query.Include(includeProperties[queryIndex]);
                    }
                }
            }

            return await query.Where(predicate).ToListAsync();
        }

        public virtual async Task<ICollection<T>> FindThanIncludeByAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, object>> includeProperties = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if (includeProperties != null)
            {
                query = includeProperties(query);

            } 

            return await query.Where(predicate).ToListAsync();
        }

        public virtual IQueryable<T> FindThanIncludeByAsyncQueryable(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, object>> includeProperties = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if (includeProperties != null)
            {
                query = includeProperties(query);

            }

            return query.Where(predicate); ;
        }

        public T Get(int id)
        {
            return _context.Set<T>().Find(id);
        }

        public IQueryable<T> GetAll()
        {
            return _context.Set<T>();
        }

        public async Task<ICollection<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public IQueryable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> queryable = GetAll();
            foreach (Expression<Func<T, object>> includeProperty in includeProperties)
            { 
                queryable = queryable.Include<T, object>(includeProperty);
            }

            return queryable;
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public T GetByIdInclude(int id, params Expression<Func<T, object>>[] includes)
        {
            var entry = Include(includes).FirstOfDefaultIdEquals(id);
            return entry;
        }

        public async Task<int> RemoveRangeAsync(IEnumerable<T> items, bool commit = true)
        {
            _context.Set<T>().RemoveRange(items);
            return await CommitAsync (_context, commit);
        }

        public virtual async Task TruncateAsync(bool commit = true)
        {
            _context.Set<T>().RemoveRange(_context.Set<T>());
            await CommitAsync(_context, commit);
        }

        public virtual T Update(T t, bool commit = true, params object[] keys)
        {
            if (t == null)
                return null;
            T exist = _context.Set<T>().Find(keys);
            if (exist != null)
            {
                _context.Entry(exist).CurrentValues.SetValues(t);
                Commit(_context, commit);
            }
            return exist;
        }

        public virtual async Task<T> UpdateAsync(T t, bool commit = true, params object[] keys)
        {
            if (t == null)
                return null;
            T exist = await _context.Set<T>().FindAsync(keys);
            if (exist != null)
            {
               
                _context.Entry(exist).CurrentValues.SetValues(t);
                await CommitAsync(_context, commit);

            }
            return exist;
        }

        public async Task<int> UpdateValueAsync(T entity, Func<T, bool> equalsExpression, bool commit = true, params Expression<Func<T, object>>[] properties)
        {
            _context.DetachLocal(entity, equalsExpression);
            _context.Set<T>().Attach(entity);
            foreach (var property in properties)
            {
                _context.Entry(entity).Property(property).IsModified = true;
            }
            return await CommitAsync(_context,commit);
        }

        protected IQueryable<T> Include(params Expression<Func<T, object>>[] includes)
        {
            IIncludableQueryable<T, object> query = null;

            if (includes.Length > 0)
            {
                query = _context.Set<T>().Include(includes[0]);
            }
            for (int queryIndex = 1; queryIndex < includes.Length; ++queryIndex)
            {
                query = query.Include(includes[queryIndex]);
            }

            return query == null ? _context.Set<T>() : (IQueryable<T>)query;
        }

        #region Save Section
        private void Commit(DbContext _context, bool commit)
        {
            if (commit)
            {
                _context.SaveChanges();
            }
        }
        private async Task<int> CommitAsync(DbContext _context, bool commit)
        {
            return commit ? await _context.SaveChangesAsync() : 0;
        }
        #endregion
    }
}
