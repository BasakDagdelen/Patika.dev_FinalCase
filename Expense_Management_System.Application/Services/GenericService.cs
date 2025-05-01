using AutoMapper;
using Expense_Management_System.Application.Interfaces.Services;
using Expense_Management_System.Domain.Common;
using Expense_Management_System.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Expense_Management_System.Application.Services;

public class GenericService<TEntity,TRequest, TResponse> : IGenericService<TRequest, TResponse> where TEntity : BaseEntity
{
    private readonly IGenericRepository<TEntity> _repository;
    private readonly IMapper _mapper;
    public GenericService(IGenericRepository<TEntity> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<TResponse> AddAsync(TRequest request)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<TResponse>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<TResponse> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Guid id, TRequest entity)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<TResponse>> WhereAsync(Expression<Func<TResponse, bool>> expression)
    {
        throw new NotImplementedException();
    }
}
