using Expense_Management_System.Application.DTOs.Requests;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expense_Management_System.Application.Validation;

public class ExpenseCategoryValidator : AbstractValidator<ExpenseCategoryRequest>
{
    public ExpenseCategoryValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Category name is required...")
            .MaximumLength(50).WithMessage("Category name must contain a maximum of 50 characters...");

        RuleFor(x => x.Description)
             .NotEmpty().WithMessage("Description is required...")
             .MinimumLength(2).WithMessage("Description must contain at least 2 characters...");
    }
}
