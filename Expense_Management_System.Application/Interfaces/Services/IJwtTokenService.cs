using Expense_Management_System.Domain.Entities;

namespace Expense_Management_System.Application.Interfaces.Services;

public interface IJwtTokenService
{
   public string GenerateToken(User user);
}
