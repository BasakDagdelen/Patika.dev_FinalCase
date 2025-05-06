using Expense_Management_System.Domain.Entities;
using Expense_Management_System.Domain.Interfaces.Repositories;
using Expense_Management_System.Domain.Interfaces.UnitOfWorks;
using Expense_Management_System.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore.Storage;


namespace Expense_Management_System.Infrastructure.UnitofWorks;

public class UnitofWork : IUnitOfWork, IDisposable
{
    private readonly ApplicationDbContext _dbContext;
    public UnitofWork(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IGenericRepository<Expense> Expenses => new GenericRepository<Expense>(_dbContext);
    public IGenericRepository<ExpenseCategory> ExpenseCategories => new GenericRepository<ExpenseCategory>(_dbContext);
    public IGenericRepository<ExpenseDocument> ExpenseDocuments => new GenericRepository<ExpenseDocument>(_dbContext);
    public IGenericRepository<Payment> Payments => new GenericRepository<Payment>(_dbContext);
    public IGenericRepository<User> Users => new GenericRepository<User>(_dbContext);


    private bool _disposed = false;
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }
        }
        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public async Task SaveChangesAsync()
    {
        using (var transaction = _dbContext.Database.BeginTransactionAsync())
        {
            try
            {
                await _dbContext.SaveChangesAsync();
                await transaction.Result.CommitAsync();
            }
            catch
            {
                await transaction.Result.RollbackAsync();
                throw;
            }
        }
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return await _dbContext.Database.BeginTransactionAsync();
    }
}
