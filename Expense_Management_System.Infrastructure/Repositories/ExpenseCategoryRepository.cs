using Expense_Management_System.Domain.Entities;
using Expense_Management_System.Domain.Enums;
using Expense_Management_System.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expense_Management_System.Infrastructure.Repositories
{
    public class ExpenseCategoryRepository : GenericRepository<ExpenseCategory>, IExpenseCategoryRepository
    {
        private readonly ApplicationDbContext _context;
        public ExpenseCategoryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> HasActiveExpensesAsync(Guid categoryId)
        {
            return await _context.Expenses.AnyAsync(e => e.ExpenseCategoryId == categoryId && e.Status == ExpenseStatus.Approved);

        }

        public async Task<bool> IsCategoryNameUniqueAsync(string categoryName, Guid? excludeId = null)
        {
            if (excludeId.HasValue)
                return !await _context.ExpenseCategories.AnyAsync(c => c.Name.ToLower() == categoryName.ToLower() && c.Id != excludeId.Value);

            return !await _context.ExpenseCategories.AnyAsync(c => c.Name.ToLower() == categoryName.ToLower());

        }
      
    }
}