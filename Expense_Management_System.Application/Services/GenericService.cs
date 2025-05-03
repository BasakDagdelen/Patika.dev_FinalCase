using AutoMapper;
using AutoMapper.QueryableExtensions;
using Expense_Management_System.Application.Interfaces.Services;
using Expense_Management_System.Domain.Common;
using Expense_Management_System.Domain.Interfaces.Repositories;
using Expense_Management_System.Domain.Interfaces.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Expense_Management_System.Application.Services;

public class GenericService<TEntity, TRequest, TResponse> : IGenericService<TRequest, TResponse, TEntity> where TEntity : BaseEntity
{
    private readonly IGenericRepository<TEntity> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public GenericService(IGenericRepository<TEntity> repository, IMapper mapper, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<TResponse> AddAsync(TRequest request)
    {
        var entity = _mapper.Map<TEntity>(request);
        await _repository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<TResponse>(entity);
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity is null)
            throw new Exception("Entity not found.");
        _repository.Delete(entity);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<IEnumerable<TResponse>> GetAllAsync()
    {
        var entities =  _repository.GetAll();
        return _mapper.Map<IEnumerable<TResponse>>(entities);
    }

    public async Task<TResponse> GetByIdAsync(Guid id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return _mapper.Map<TResponse>(entity);
    }

    public async Task UpdateAsync(Guid id, TRequest request)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null)
            throw new Exception("Entity not found.");

        var updatedEntity = _mapper.Map(request, entity);
        _repository.Update(updatedEntity);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<IEnumerable<TResponse>> WhereAsync(Expression<Func<TEntity, bool>> expression)
    {
        var entities = _repository.Where(expression);
        var result = await entities.ProjectTo<TResponse>(_mapper.ConfigurationProvider).ToListAsync();
        return result;
    }
}
