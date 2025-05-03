using System.Linq.Expressions;


namespace Expense_Management_System.Application.Interfaces.Services;

public interface IGenericService<TRequest, TResponse, TEntity> 
{
    Task<TResponse> GetByIdAsync(Guid id);
    Task<IEnumerable<TResponse>> GetAllAsync();
    Task<IEnumerable<TResponse>> WhereAsync(Expression<Func<TEntity, bool>> expression);
    Task<TResponse> AddAsync(TRequest request);
    Task UpdateAsync(Guid id, TRequest request);
    Task DeleteAsync(Guid id);
}
