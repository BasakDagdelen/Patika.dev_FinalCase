using Expense_Management_System.Application.DTOs.Requests;
using Expense_Management_System.Domain.Entities;
using Expense_Management_System.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expense_Management_System.Application.Interfaces.Services;

public interface IUserService : IGenericService<User>
{
    Task<IEnumerable<User>> GetUsersByRoleAsync(UserRole role);
    Task<User?> GetUserByEmailAsync(string email);
    Task<User?> GetUserByIdWithExpensesAsync(Guid userId);
    Task<bool> CheckUserExistsAsync(string email);
    Task ChangeUserRoleAsync(Guid userId, UserRole newRole);
    Task<User> CreateUserAsync(RegisterRequest request);
}