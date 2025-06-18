using System.Linq.Expressions;

namespace Quiz.DataAccessLayer.Interfaces;

public interface IRepository<T> where T : class, IEntity
{
    Task<T?> GetByIdAsync(Guid id);

    Task<IEnumerable<T>> GetAllAsync();

    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

    Task<T> CreateAsync(T entity);

    Task<T> UpdateAsync(T entity);

    Task<T> DeleteAsync(Guid id);
}
