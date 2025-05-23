﻿using Expense_Management_System.Application.DTOs.Common;
using Expense_Management_System.Domain.Common;
using Expense_Management_System.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expense_Management_System.Application.DTOs.Requests;

public class UserRequest: BaseRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string PhoneNumber { get; set; }
    public string? WorkPhoneNumber { get; set; }
    public string Address { get; set; } 
    public UserRole UserRole { get; set; }
    public string IBAN { get; set; }
    public string AccountNumber { get; set; }
}

