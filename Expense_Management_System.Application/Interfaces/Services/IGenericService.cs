using System.Linq.Expressions;
namespace Expense_Management_System.Application.Interfaces.Services;

public interface IGenericService<TEntity> 
{
    Task<TEntity> GetByIdAsync(Guid id);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<IEnumerable<TEntity>> WhereAsync(Expression<Func<TEntity, bool>> expression);
    Task<TEntity> AddAsync(TEntity request);
    Task UpdateAsync(Guid id, TEntity request);
    Task DeleteAsync(Guid id);
}
