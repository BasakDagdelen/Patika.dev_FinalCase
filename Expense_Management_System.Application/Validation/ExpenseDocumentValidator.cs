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
            .NotNull().WithMessage("The file is required...")
            .Must(f => f.Length > 0).WithMessage("Empty files cannot be uploaded...")
            .Must(f => f.Length <= 5 * 1024 * 1024).WithMessage("The file size must not exceed 5 MB...")
            .Must(f => IsSupportedFileType(f)).WithMessage("Only .jpg, .jpeg, .png, or .pdf files are allowed...");
    }

    private bool IsSupportedFileType(IFormFile file)
    {
        if (file == null) 
            return false;

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".pdf" };
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        return allowedExtensions.Contains(extension);
    }
}
