using Expense_Management_System.Application.DTOs.Requests;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expense_Management_System.Application.Validations;

public class ExpenseFilterValidator : AbstractValidator<ExpenseFilterRequest>
{
    public ExpenseFilterValidator()
    {
        RuleFor(x => x.MinAmount)
       .GreaterThanOrEqualTo(0).WithMessage("The minimum amount cannot be negative.");

        RuleFor(x => x.MaxAmount)
            .GreaterThanOrEqualTo(0).WithMessage("The maximum amount cannot be negative.");

        RuleFor(x => x.MinAmount)
            .LessThanOrEqualTo(x => x.MaxAmount).When(x => x.MaxAmount.HasValue)
            .WithMessage("MinAmount cannot be greater than MaxAmount.");

        RuleFor(x => x.FromDate)
            .LessThanOrEqualTo(x => x.ToDate).When(x => x.ToDate.HasValue)
            .WithMessage("FromDate cannot be later than ToDate.");

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Invalid status value.");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId cannot be empty.").When(x => x.UserId.HasValue);
    }
}
