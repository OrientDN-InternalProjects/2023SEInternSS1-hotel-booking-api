using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HotelBooking.Data.Infrastructure
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly BookingDbContext context;
        private readonly DbSet<T> dbSet;

        public GenericRepository(BookingDbContext context)
        {
            this.context = context;
            this.dbSet = context.Set<T>();
        }

        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public virtual void Delete(Guid id)
        {
            T entityToDelete = dbSet.Find(id);
            dbSet.Remove(entityToDelete);
        }

        public virtual void Delete(T entityToDelete)
        {
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
        }

        public virtual IQueryable<T> GetAll()
        {
            return dbSet.AsNoTracking();
        }

        public virtual IQueryable<T> GetByCondition(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "")
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
            if (orderBy != null)
            {
                return orderBy(query);
            }
            else
            {
                return query;
            }
        }

        public virtual void Update(T entity)
        {
            dbSet.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
        }

        public virtual T GetByID(Guid id)
        {
            return dbSet.Find(id);
        }
    }
}
