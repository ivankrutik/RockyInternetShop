using Microsoft.EntityFrameworkCore;

namespace RockyDataAccess.Reporitory
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbContext _dbContext;
        internal DbSet<T> dbSet;

        public Repository(DbContext dbContext)
        {
            _dbContext = dbContext;
            dbSet = _dbContext.Set<T>();
        }

        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public T Find(long id)
        {
            return dbSet.Find(id);
        }

        public T FirstOrDefault(System.Linq.Expressions.Expression<Func<T, bool>>? filter = null, string? includedProperties = null, bool isTracking = true)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (includedProperties != null)
            {
                foreach (var property in includedProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                }
            }
            if (!isTracking)
            {
                query.AsNoTracking();
            }
            return query.FirstOrDefault();
        }

        public IEnumerable<T> GetAll(System.Linq.Expressions.Expression<Func<T, bool>>? filter = null,
                                        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
                                        string? includedProperties = null,
                                        bool isTracking = true)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (includedProperties != null)
            {
                foreach (var property in includedProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                }
            }
            if (!isTracking)
            {
                query.AsNoTracking();
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            return query.ToList();
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            dbSet.RemoveRange(entities);
        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }
    }
}
