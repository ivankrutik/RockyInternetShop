using System.Linq.Expressions;

namespace RockyDataAccess.Reporitory
{
    public interface IRepository<T> where T : class
    {
        T Find(long id);

        IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null,
                                Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
                                string? includedProperties = null,
                                bool isTracking = true);

        T FirstOrDefault(Expression<Func<T, bool>>? filter = null,
                            string? includedProperties = null,
                            bool isTracking = true);

        void Add(T entity);
        void Remove(T entity);

        void SaveChanges();
    }
}
