﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expense_Management_System.Application.DTOs.Requests;

public class LoginRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}
