using AutoMapper;
using AutoMapper.QueryableExtensions;
using Expense_Management_System.Application.Interfaces.Services;
using Expense_Management_System.Domain.Common;
using Expense_Management_System.Domain.Interfaces.Repositories;
using Expense_Management_System.Domain.Interfaces.UnitOfWorks;
using System.Linq.Expressions;

namespace Expense_Management_System.Application.Services;

public class GenericService<TEntity> : IGenericService<TEntity> where TEntity : BaseEntity
{
    private readonly IGenericRepository<TEntity> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public GenericService(IGenericRepository<TEntity> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<TEntity> AddAsync(TEntity entity)
    {
        await _repository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity is null)
            throw new Exception($"Entity with id {id} not found.");

        entity.IsActive = false; // Soft delete
        _repository.UpdateAsync(entity);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<TEntity> GetByIdAsync(Guid id)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity is null)
            throw new Exception($"Entity with id {id} not found.");

        return entity;
    }

    public async Task UpdateAsync(Guid id, TEntity entity)
    {
        var existingEntity = await _repository.GetByIdAsync(id);
        if (existingEntity is null)
            throw new Exception($"Entity with id {id} not found.");

        _repository.UpdateAsync(existingEntity);
        await _unitOfWork.SaveChangesAsync();
    }


    public async Task<IEnumerable<TEntity>> WhereAsync(Expression<Func<TEntity, bool>> expression)
    {
        return await _repository.WhereAsync(expression);
    }
}
