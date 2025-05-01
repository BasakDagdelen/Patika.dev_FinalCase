using Expense_Management_System.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Expense_Management_System.Application.Interfaces.Services;

public interface IGenericService<TRequest, TResponse> 
{
    Task<TResponse> GetByIdAsync(Guid id);
    Task<IEnumerable<TResponse>> GetAllAsync();
    Task<IEnumerable<TResponse>> WhereAsync(Expression<Func<TResponse, bool>> expression);
    Task<TResponse> AddAsync(TRequest entity);
    Task UpdateAsync(Guid id, TRequest entity);
    Task DeleteAsync(Guid id);
}
