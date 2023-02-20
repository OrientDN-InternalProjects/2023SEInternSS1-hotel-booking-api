using System.Linq.Expressions;

namespace HotelBooking.Data.Infrastructure
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        IQueryable<T> GetByCondition(Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "");
        T GetByID(Guid id);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Delete(Guid id);
    }
}
