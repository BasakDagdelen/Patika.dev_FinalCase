using Expense_Management_System.Domain.Entities;
using Expense_Management_System.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expense_Management_System.Domain.Interfaces.UnitOfWorks;

public interface IUnitOfWork : IDisposable
{
    IGenericRepository<Expense> Expenses { get; }
    IGenericRepository<ExpenseCategory> ExpenseCategories { get; }
    IGenericRepository<ExpenseDocument> ExpenseDocuments { get; }
    IGenericRepository<Payment> Payments { get; }
    IGenericRepository<User> Users { get; }
    Task SaveChangesAsync();
}
