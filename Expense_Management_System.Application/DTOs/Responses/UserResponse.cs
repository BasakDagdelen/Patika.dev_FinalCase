using Expense_Management_System.Application.DTOs.Common;
using Expense_Management_System.Domain.Enums;
using Expense_Management_System.Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expense_Management_System.Application.DTOs.Responses;

public class UserResponse: BaseResponse
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    //public string Password { get; set; }
    public string PhoneNumber { get; set; }
    public string? WorkPhoneNumber { get; set; }
    public string Address { get; set; }
    public string IBAN { get; set; }
    public UserRole UserRole { get; set; }
    //public ICollection<Expense> Expenses { get; set; }
    //public ICollection<Payment> Payments { get; set; }
}
