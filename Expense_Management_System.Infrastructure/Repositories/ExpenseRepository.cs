using Expense_Management_System.Domain.Entities;
using Expense_Management_System.Domain.Enums;
using Expense_Management_System.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expense_Management_System.Infrastructure.Repositories;

public class ExpenseRepository : GenericRepository<Expense>, IExpenseRepository
{
    private readonly ApplicationDbContext _context;
    public ExpenseRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }
    public async Task<IEnumerable<Expense>> GetActiveExpenseSAsync(Guid userId)
    {
        return await _context.Expenses.Where(e => e.UserId == userId && e.Status == ExpenseStatus.Pending && e.IsActive).ToListAsync();
    }

    public async Task<IEnumerable<Expense>> GetExpenseHistoryAsync(Guid userId)
    {
       return await _context.Expenses.Where(e => e.UserId == userId && e.Status == ExpenseStatus.Approved && e.IsActive).ToListAsync();
    }

    public async Task<IEnumerable<Expense>> GetExpensesByCategoryAsync(Guid categoryId)
    {
        return await _context.Expenses.Where(e => e.ExpenseCategoryId == categoryId && e.IsActive).ToListAsync();
    }

    public async Task<IEnumerable<Expense>> GetExpensesByStatusAsync(ExpenseStatus status)
    {
        return await _context.Expenses.Where(e => e.Status == status && e.IsActive).ToListAsync();
    }


}
