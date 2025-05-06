using Expense_Management_System.Application.DTOs.Requests;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expense_Management_System.Application.Validation;

public class ExpenseDocumentValidator: AbstractValidator<ExpenseDocumentRequest>
{
    public ExpenseDocumentValidator()
    {
        RuleFor(x => x.File)
            .NotNull().WithMessage("The file is required...");
    }

}
