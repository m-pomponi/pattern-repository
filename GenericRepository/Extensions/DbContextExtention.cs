using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace GenericRepository.Extensions
{
    public static class DbContextExtention
    {
        public static TEntity FirstOfDefaultIdEquals<TEntity, TKey>(
            this IQueryable<TEntity> source, TKey otherKeyValue)
            where TEntity : class
        {
            var parameter = Expression.Parameter(typeof(TEntity), "x");
            var property = Expression.Property(parameter, "ID");
            var equal = Expression.Equal(property, Expression.Constant(otherKeyValue));
            var lambda = Expression.Lambda<Func<TEntity, bool>>(equal, parameter);
            return source.FirstOrDefault(lambda);
        }
        public static TEntity FirstOfDefaultIdEquals<TEntity>(
            this ObservableCollection<TEntity> source, TEntity entity)
            where TEntity : class
        {
            var value = (int)entity.GetType().GetProperty("ID").GetValue(entity, null);
            var parameter = Expression.Parameter(typeof(TEntity), "x");
            var property = Expression.Property(parameter, "ID");
            var equal = Expression.Equal(property, Expression.Constant(value));
            var lambda = Expression.Lambda<Func<TEntity, bool>>(equal, parameter);
            var queryableList = new List<TEntity>(source).AsQueryable();
            return queryableList.FirstOrDefault(lambda);
        }
        public static void DetachLocal<T>(this DbContext context, T t, Func<T, bool> predicate)
            where T : class
        {
            var local = context.Set<T>()
                .Local
                .FirstOrDefault(predicate);
            if (local != null)
            {
                context.Entry(local).State = EntityState.Detached;
            }
            context.Entry(t).State = EntityState.Modified;
        }
    }
}
