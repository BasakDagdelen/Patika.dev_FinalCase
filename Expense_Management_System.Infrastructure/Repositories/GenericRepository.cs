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
    private readonly DbSet<TEntity> _dbSet;
    public GenericRepository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<TEntity>();
    }

    public async Task<TEntity> AddAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;

    }

    public async Task AddRangeAsync(IEnumerable<TEntity> entities)
       => await _dbSet.AddRangeAsync(entities);

    public async Task DeleteAsync(TEntity entity)
    {
        _dbSet.Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
       => await _dbSet.ToListAsync();

    public async Task<TEntity> GetByIdAsync(Guid id)
        => await _dbSet.FindAsync(id);

    public async Task UpdateAsync(TEntity entity)
    {
        var existingEntity = await _dbSet.FindAsync(entity.Id);
        if (existingEntity is null)
            throw new DbUpdateException($"Entity with id {entity.Id} not found.");
        _context.Entry(existingEntity).CurrentValues.SetValues(entity);

        // 3. Sadece değişen alanları güncelle
        _context.Entry(existingEntity).Property(x => x.UpdatedDate).IsModified = true;
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<TEntity>> WhereAsync(Expression<Func<TEntity, bool>> expression)
       => await _dbSet.Where(expression).ToListAsync();


}