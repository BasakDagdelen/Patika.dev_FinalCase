using Expense_Management_System.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expense_Management_System.Application.Interfaces.Services;

public interface IExpenseCategoryService : IGenericService<ExpenseCategory>
{
    Task<bool> CanDeleteCategoryAsync(Guid id);
    Task<bool> IsCategoryNameExistsAsync(string name, Guid? excludeId = null);
}