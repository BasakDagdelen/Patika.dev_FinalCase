using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expense_Management_System.Application.DTOs.Responses;

public class LoginResponse
{
    public string Token { get; set; }
    public string FullName { get; set; }
    public string Role { get; set; }
    public DateTime ExpireAt { get; set; }
}
