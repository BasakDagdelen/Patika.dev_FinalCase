using Expense_Management_System.Application.DTOs.entitys;
using Expense_Management_System.Domain.Models.Enums;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expense_Management_System.Application.Validation;

public class ExpenseValidator : AbstractValidator<Expenseentity>
{
    public ExpenseValidator()
    {
        RuleFor(x => x.ExpenseCategoryId)
            .NotEmpty().WithMessage("Expense category is required...");

        RuleFor(x => x.Amount)
             .NotEmpty().WithMessage("Amount is required...")
             .GreaterThan(0).WithMessage("Amount must be greater than 0....");

        RuleFor(x => x.PaymentLocation)
            .NotEmpty().WithMessage("Payment location information is required...")
            .MinimumLength(5).WithMessage("Payment location must contain at least 5 characters...");

        When(x => string.IsNullOrEmpty(x.Description),
          () => RuleFor(x => x.Description)
          .MinimumLength(2).WithMessage("Description must contain at least 2 characters..."));

        RuleFor(x => x.PaymentMethod)
            .IsInEnum().WithMessage("Please select a valid payment method...");

        RuleFor(x => x.Documents)
            .NotNull().WithMessage("You must upload at least one document...")
            .Must(docs => docs.Any()).WithMessage("You must upload at least one document...")
            .Must(docs => docs.Count <= 5).WithMessage("You can upload a maximum of 5 documents...")
            .Must(AllFilesAreValidTypes).WithMessage("Only JPG and PNG files are allowed...")
            .Must(AllFilesAreUnderSizeLimit).WithMessage("Each file must be less than 5 MB...");

    }

    private bool AllFilesAreValidTypes(List<ExpenseDocumenTEntity> documents)
    {
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
        return documents.All(doc =>
        {
            var extension = Path.GetExtension(doc.File.FileName).ToLowerInvariant();
            return allowedExtensions.Contains(extension);
        });
    }

    private bool AllFilesAreUnderSizeLimit(List<ExpenseDocumenTEntity> documents)
    {
        const long maxFileSizeInBytes = 5 * 1024 * 1024;
        return documents.All(doc => doc.File.Length <= maxFileSizeInBytes);
    }
}
