using Expense_Management_System.Application.DTOs.Requests;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expense_Management_System.Application.Validations;

public class RejectExpenseValidator : AbstractValidator<RejectExpenseRequest>
{
    public RejectExpenseValidator()
    {
        RuleFor(x => x.Reason).NotEmpty().WithMessage("Season is required...")
            .MinimumLength(5).WithMessage("Season must be at least 5 characters long...");
    }
}