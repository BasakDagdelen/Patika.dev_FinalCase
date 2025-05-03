using Expense_Management_System.Domain.Common;
using System.Linq.Expressions;


namespace Expense_Management_System.Domain.Interfaces.Repositories;

public interface IGenericRepository<TEntity> where TEntity : BaseEntity
{
    Task<TEntity> GetByIdAsync(Guid id);
    IQueryable<TEntity> GetAll();
    IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> expression);
    Task AddAsync(TEntity entity);
    Task AddRangeAsync(IEnumerable<TEntity> entities);
    void Update(TEntity entity);
    void Delete(TEntity entity);
}
