using Expense_Management_System.Domain.Common;
using Expense_Management_System.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Expense_Management_System.Infrastructure.Repositories;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<TEntity> _entities;
    public GenericRepository(ApplicationDbContext context)
    {
        _context = context;
        _entities = _context.Set<TEntity>();
    }

    public async Task AddAsync(TEntity entity)
    {
        await _entities.AddAsync(entity);
    }

    public async Task AddRangeAsync(IEnumerable<TEntity> entities)
    {
        await _entities.AddRangeAsync(entities);
    }

    public void Delete(TEntity entity)
    {
        _entities.Remove(entity);
    }

    public IQueryable<TEntity> GetAll()
    {
        return _entities.AsNoTracking().AsQueryable();
    }

    public async Task<TEntity> GetByIdAsync(Guid id)
    {
        return await _entities.FindAsync(id);
    }

    public void Update(TEntity entity)
    {
        _entities.Update(entity);
    }

    public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> expression)
    {
        return _entities.Where(expression);
    }
}